// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;

namespace LineCounter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = OpenTelemetry.Sdk.CreateTracerProviderBuilder()
                .AddSource("Azure.*")
                .AddAzureMonitorTraceExporter(options => options.ConnectionString = "InstrumentationKey=0422f781-b57b-45c4-8f62-b1b9df4f97c7;IngestionEndpoint=https://westus2-1.in.applicationinsights.azure.com/")
                .AddAspNetCoreInstrumentation();

            using var trace = builder.Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IHostedService, LineCounterService>();
                    })
                .ConfigureAppConfiguration(builder => builder.AddUserSecrets(typeof(Program).Assembly))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}