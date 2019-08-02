// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.Azure.Samples
{
    internal class DependencyInjectionEnabledPolicy : SynchronousHttpPipelinePolicy
    {
#pragma warning disable 618
        private readonly IHostingEnvironment _environment;

        public DependencyInjectionEnabledPolicy(IHostingEnvironment environment)
#pragma warning restore 618
        {
            this._environment = environment;
        }

        public override void OnSendingRequest(HttpPipelineMessage message)
        {
            message.Request.Headers.Add("application-name", _environment.ApplicationName);
            base.OnSendingRequest(message);
        }
    }
}