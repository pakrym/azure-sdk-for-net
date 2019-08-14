// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Azure.Messaging.EventHubs.Core
{
    /// <summary>
    ///   A read-only subscription to a channel, exposed as an
    ///   asynchronous enumerator.
    /// </summary>
    ///
    /// <typeparam name="T">The type of data contained in the channel.</typeparam>
    ///
    /// <seealso cref="IAsyncEnumerable{T}" />
    ///
    internal class ChannelEnumerableSubscription<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>, IAsyncDisposable
    {
        /// <summary>The callback function to invoke to unsubscribe.</summary>
        private readonly Func<Task> UnsubscribeCallbackAsync;

        /// <summary>The source enumerator to use for iterating through the published events.</summary>
        private readonly Lazy<IAsyncEnumerator<T>> SourceEnumerator;

        /// <summary>A flag that indicates whether or not the instance has been disposed.</summary>
        private bool _disposed = false;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ChannelEnumerableSubscription{T}"/> class.
        /// </summary>
        ///
        /// <param name="reader">The reader for the channel associated with the subscription.</param>
        /// <param name="maximumWaitTime">The maximum amount of time to wait to for an event to be available before emitting an empty item; if <c>null</c>, empty items will not be emitted.</param>
        /// <param name="unsubscribeCallbackAsync">The callback function to invoke to unsubscribe.</param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> instance to signal the request to cancel reading from the channel for enumeration.</param>
        ///
        public ChannelEnumerableSubscription(ChannelReader<T> reader,
                                             TimeSpan? maximumWaitTime,
                                             Func<Task> unsubscribeCallbackAsync,
                                             CancellationToken cancellationToken)
        {
            Guard.ArgumentNotNull(nameof(reader), reader);
            Guard.ArgumentNotNull(nameof(unsubscribeCallbackAsync), unsubscribeCallbackAsync);

            if (maximumWaitTime.HasValue)
            {
                Guard.ArgumentNotNegative(nameof(maximumWaitTime), maximumWaitTime.Value);
            }

            UnsubscribeCallbackAsync = unsubscribeCallbackAsync;

            SourceEnumerator = new Lazy<IAsyncEnumerator<T>>(
                () => EnumerateChannel(reader, maximumWaitTime, cancellationToken).GetAsyncEnumerator(cancellationToken),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        ///   Builds an asynchronous enumerator based on the event channel.
        /// </summary>
        ///
        /// <param name="cancellationToken">The cancellation token.</param>
        ///
        /// <returns>The asynchronous enumerator to use for iterating over events.</returns>
        ///
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ChannelEnumerableSubscription<T>));
            }

            return this;
        }

        /// <summary>
        ///   Performs the tasks needed to clean up the enumerable, including
        ///   attempting to unsubscribe from the associated subscription.
        /// </summary>
        ///
        ///
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (SourceEnumerator.IsValueCreated)
                {
                    await SourceEnumerator.Value.DisposeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
               await UnsubscribeCallbackAsync().ConfigureAwait(false);
            }

            _disposed = true;
        }

        /// <summary>
        ///   Enumerates the events as they become available in the associated channel.
        /// </summary>
        ///
        /// <param name="reader">The reader for the channel from which events are to be sourced.</param>
        /// <param name="maximumWaitTime">The maximum amount of time to wait to for an event to be available before emitting an empty item; if <c>null</c>, empty items will not be emitted.</param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> instance to signal the request to cancel enumeration.</param>
        ///
        /// <returns>An asynchronous enumerator that can be used to iterate over events as they are available.</returns>
        ///
        private static async IAsyncEnumerable<T> EnumerateChannel(ChannelReader<T> reader,
                                                                  TimeSpan? maximumWaitTime,
                                                                  [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            T result;

            var waitToken = cancellationToken;
            var waitSource = default(CancellationTokenSource);
            var maximumDelay = TimeSpan.FromDays(10);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (reader.TryRead(out result))
                    {
                        waitSource?.CancelAfter(maximumDelay);
                        yield return result;
                    }
                    else if (reader.Completion.IsCompleted)
                    {
                        // If the channel was marked as final, then await the completion task to surface any exceptions.

                        await reader.Completion.ConfigureAwait(false);
                        break;
                    }
                    else
                    {
                        try
                        {
                            if (maximumWaitTime.HasValue)
                            {
                                if ((waitSource == null) || (waitSource.IsCancellationRequested))
                                {
                                    waitSource?.Dispose();
                                    waitSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                                }

                                waitSource.CancelAfter(maximumWaitTime.Value);
                                waitToken = waitSource.Token;
                            }

                            // Wait for an item to be available in the channel; if it becomes so, then
                            // reset the loop so that it can be read and emitted.

                            if (await reader.WaitToReadAsync(waitToken).ConfigureAwait(false))
                            {
                                waitSource?.CancelAfter(maximumDelay);
                                continue;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            // This is thrown when the wait token expires.  It may be caused by the maximum wait time
                            // being exceeded or the main cancellation token being set.  Ignore this as an expected
                            // case; if the iteration was canceled, it will be detected in the body of the loop and
                            // appropriate action taken.

                            waitSource?.Dispose();
                            waitSource = null;
                        }

                        // If the wait token was set, but the main cancellation token was not, then the wait time was
                        // exceeded and a default item needs to be emitted.

                        if ((waitToken.IsCancellationRequested) && (!cancellationToken.IsCancellationRequested))
                        {
                            yield return default;
                        }
                    }
                }
            }
            finally
            {
                waitSource?.Dispose();
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            yield break;
        }

        /// <summary>
        ///   The current event in the set being enumerated.
        /// </summary>
        ///
        T IAsyncEnumerator<T>.Current => SourceEnumerator.Value.Current;

        /// <summary>
        ///   Moves to the next event in the set being enumerated.
        /// </summary>
        ///
        /// <returns><c>true</c> if there was an event to move to; otherwise, <c>false</c>.</returns>
        ///
        ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync() => SourceEnumerator.Value.MoveNextAsync();
    }
}
