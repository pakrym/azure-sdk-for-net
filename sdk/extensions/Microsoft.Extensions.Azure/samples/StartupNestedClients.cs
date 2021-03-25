// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Azure.Samples
{
    public class StartupNestedClients
    {
        #region Snippet:NestedClient

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureClients(builder =>
            {
                // Register the main service client
                builder.AddBlobServiceClient(new Uri("<endpoint>"));
                // Register a nested blob client
                var mainClient = builder.AddClient<BlobContainerClient, BlobClientOptions>(
                    (provider, _) => provider.GetService<BlobServiceClient>().GetBlobContainerClient("images"));

                mainClient.AddNestedClient(client => client.GetBlobClient("blob")).WithName("123");

                builder.AddNestedClient<BlobServiceClient, BlobContainerClient>(serviceClient => serviceClient.GetBlobContainerClient("images"))
                    .WithName();
            });
        }

        #endregion
    }
}