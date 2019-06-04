// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;

namespace Azure.Core
{
    public class NoopTransport : HttpPipelineTransport
    {
        public override void Process(HttpPipelineMessage message)
        {
        }

        public override Task ProcessAsync(HttpPipelineMessage message)
        {
        }

        public override Request CreateRequest()
        {
        }
    }

    public struct BatchOperationResult<T>
    {
        private readonly object _state;

        public BatchOperationResult(object state)
        {
            _state = state;
        }

        public T GetValue(BatchOperationResponse response)
        {
            return response.GetValue<T>(_state);
        }
    }

    public abstract class BatchOperationResponse
    {
        public abstract Response GetRawResponse();

        protected internal T GetValue<T>(object state)
        {

        }
    }

    public class BlobBatchClient
    {
        public BatchOperationResult<Response> Delete(
            string name,
            DeleteSnapshotsOption? deleteOptions = default,
            BlobAccessConditions? accessConditions = default) { }

        public BatchOperationResult<Response> SetTier(
            string name,
            AccessTier accessTier,
            LeaseAccessConditions? leaseAccessConditions = default) { }

        public Task<BatchOperationResponse> SendAsync(CancellationToken cancellationToken) { }
        public BatchOperationResponse Send(CancellationToken cancellationToken) { }
    }

    public class BatchClient
    {
        private readonly HttpPipeline _realPipeline = null;
        private readonly HttpPipeline _batchPipeline = null;

        private List<Request> _requests = new List<Request>();

        public BatchClient()
        {

        }

        public BatchOperationResult<string> DoSomething()
        {
            var request = _batchPipeline.CreateRequest();
            request.UriBuilder.AppendPath("/DoSomething");

            _requests.Add(request);

            return new BatchOperationResultImplementation<string>(request);
        }
        public BatchOperationResult<int> DoSomethingElse()
        {
            var request = _batchPipeline.CreateRequest();
            request.UriBuilder.AppendPath("/DoSomethingElse");

            _requests.Add(request);

            return new BatchOperationResultImplementation<int>(request);
        }

        public async Task<BatchOperationResponse> SendAsync()
        {
            foreach (Request request in _requests)
            {
                // Request objects are mutated inplace so invoking the pipeline is enough

                await _batchPipeline.SendRequestAsync(request, CancellationToken.None);
            }

            var realRequest = _realPipeline.CreateRequest();
            realRequest.UriBuilder.Uri = new Uri("http://localhost/BatchEndpoint");
            var stringBuilder = new StringBuilder();
            foreach (Request request in _requests)
            {
                stringBuilder.AppendLine(request.UriBuilder.ToString());
            }

            return new BatchOperationResponseImplementation(null);
        }

        private class BatchOperationResponseImplementation : BatchOperationResponse
        {
            private readonly Response _raw;

            public BatchOperationResponseImplementation(Response raw)
            {
                _raw = raw;
            }

            public override Response GetRawResponse()
            {
                return _raw;
            }
        }

        private class BatchOperationResultImplementation<T>: BatchOperationResult<T>
        {
            private readonly Request _request;

            public BatchOperationResultImplementation(Request request)
            {
                _request = request;
            }

            public override T GetValue(BatchOperationResponse response)
            {
                return ((BatchOperationResponseImplementation)response).Get
            }
        }
    }

    public class Program
    {
        public async Task Main()
        {
            var batchClient = new BatchClient();

            BatchOperationResult<string> result1 = batchClient.DoSomething();
            BatchOperationResult<int> result2 = batchClient.DoSomethingElse();

            BatchOperationResponse response = await batchClient.SendAsync();

            string realResult = result1.GetValue(response);

        }
    }

    /// <summary>
    /// Represents a credential capable of providing an OAuth token
    /// </summary>
    public abstract class TokenCredential
    {
        public abstract ValueTask<string> GetTokenAsync(string[] scopes, CancellationToken cancellationToken);
        public abstract string GetToken(string[] scopes, CancellationToken cancellationToken);
    }
}
