// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Buffers;

namespace Azure.Core.Pipeline
{
    /// <summary>
    /// Pipeline policy to buffer response content or add a timeout to response content managed by the client
    /// </summary>
    internal class ResponseBodyPolicy : HttpPipelinePolicy
    {
        private readonly TimeSpan _networkTimeout;

        public ResponseBodyPolicy(TimeSpan networkTimeout)
        {
            _networkTimeout = networkTimeout;
        }

        public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline) =>
            ProcessAsync(message, pipeline, true);

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline) =>
            ProcessAsync(message, pipeline, false).EnsureCompleted();

        private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
        {
            CancellationToken oldToken = message.CancellationToken;
            using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(oldToken);

            var networkTimeout = _networkTimeout;

            if (message.NetworkTimeout is TimeSpan networkTimeoutOverride)
            {
                networkTimeout = networkTimeoutOverride;
            }

            cts.CancelAfter(networkTimeout);
            try
            {
                message.CancellationToken = cts.Token;
                if (async)
                {
                    await ProcessNextAsync(message, pipeline).ConfigureAwait(false);
                }
                else
                {
                    ProcessNext(message, pipeline);
                }
            }
            finally
            {
                message.CancellationToken = oldToken;
                cts.CancelAfter(Timeout.Infinite);
            }

            Stream? responseContentStream = message.Response.ContentStream;
            if (responseContentStream == null || responseContentStream.CanSeek)
            {
                return;
            }

            if (message.BufferResponse)
            {
                await message.BufferResponseAsync(async, cts).ConfigureAwait(false);
            }
            else if (networkTimeout != Timeout.InfiniteTimeSpan)
            {
                message.Response.ContentStream = new ReadTimeoutStream(responseContentStream, networkTimeout);
            }
        }
    }
}