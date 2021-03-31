using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai_console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create the DI container.
            IServiceCollection services = new ServiceCollection();

            services.AddApplicationInsightsTelemetryWorkerService("579edbf1-5db6-4bb1-be3d-a8dc18a5edc0");

            // Build ServiceProvider.
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
            var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

            using (var operation = telemetryClient.StartOperation<RequestTelemetry>("My App Is Running!"))
            {
                var endpoint = new Uri("https://linecounterstorage.blob.core.windows.net/");
                var client = new BlobServiceClient(endpoint, new DefaultAzureCredential());
                var containerClient = client.GetBlobContainerClient("images");

                var stream = File.OpenRead(@"d:\temp\ai_console\ai_console.csproj");

                await containerClient.CreateIfNotExistsAsync();

                await containerClient.UploadBlobAsync("ai_console 3", stream);


            }

            telemetryClient.Flush();
            Task.Delay(5000).Wait();
        }
    }
}
