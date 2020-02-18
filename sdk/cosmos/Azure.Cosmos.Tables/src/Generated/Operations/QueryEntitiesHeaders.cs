// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

using System;
using Azure;
using Azure.Core;

namespace Azure.Cosmos.Tables
{
    internal class QueryEntitiesHeaders
    {
        private readonly Azure.Response _response;
        public QueryEntitiesHeaders(Azure.Response response)
        {
            _response = response;
        }
        public string XMsClientRequestId => _response.Headers.TryGetValue("x-ms-client-request-id", out string value) ? value : null;
        public string XMsRequestId => _response.Headers.TryGetValue("x-ms-request-id", out string value) ? value : null;
        public string XMsVersion => _response.Headers.TryGetValue("x-ms-version", out string value) ? value : null;
        public System.DateTimeOffset? Date => _response.Headers.TryGetValue("Date", out System.DateTimeOffset? value) ? value : null;
        public string XMsContinuationNextPartitionKey => _response.Headers.TryGetValue("x-ms-continuation-NextPartitionKey", out string value) ? value : null;
        public string XMsContinuationNextRowKey => _response.Headers.TryGetValue("x-ms-continuation-NextRowKey", out string value) ? value : null;
    }
}
