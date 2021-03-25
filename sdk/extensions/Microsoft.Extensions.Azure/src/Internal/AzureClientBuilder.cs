// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Azure
{
    internal sealed class AzureClientBuilder<TClient, TOptions>: IAzureClientBuilder<TClient, TOptions> where TOptions : class
    {
        public AzureClientFactoryBuilder FactoryBuilder { get; }
        public ClientRegistration<TClient> Registration { get; }
        public IServiceCollection ServiceCollection { get; }

        internal AzureClientBuilder(AzureClientFactoryBuilder factoryBuilder, ClientRegistration<TClient> clientRegistration, IServiceCollection serviceCollection)
        {
            FactoryBuilder = factoryBuilder;
            Registration = clientRegistration;
            ServiceCollection = serviceCollection;
        }
    }
}