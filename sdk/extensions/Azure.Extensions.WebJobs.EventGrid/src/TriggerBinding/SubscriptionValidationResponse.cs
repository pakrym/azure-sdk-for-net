// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.EventGrid
{
    internal class SubscriptionValidationResponse
    {
        [JsonProperty(PropertyName = "validationResponse")]
        public string ValidationResponse { get; set; }
    }
}
