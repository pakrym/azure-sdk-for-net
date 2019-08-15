﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
    ///
    /// <seealso cref="IAsyncEnumerable{T}" />
    ///
    internal static class ChannelEnumerableSubscription
    {
        /// <summary>
        ///   Enumerates the events as they become available in the associated channel.
        /// </summary>
        /// <typeparam name="T">The type of data contained in the channel.</typeparam>
        /// <param name="reader"></param>
        /// <param name="maximumWaitTime">The maximum amount of time to wait to for an event to be available before emitting an empty item; if <c>null</c>, empty items will not be emitted.</param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> instance to signal the request to cancel enumeration.</param>
        /// 
        /// <returns>An asynchronous enumerator that can be used to iterate over events as they are available.</returns>
        ///
        public static async IAsyncEnumerable<T> EnumerateChannel<T>(this ChannelReader<T> reader, TimeSpan? maximumWaitTime, [EnumeratorCancellation]CancellationToken cancellationToken)
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

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
