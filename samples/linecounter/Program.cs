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
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            OpenTelemetry.Sdk.CreateTracerProviderBuilder()
                .AddSource("Azure.*")
                .AddAzureMonitorTraceExporter(o => {
                    o.ConnectionString = "InstrumentationKey=ff2bad89-019f-4bd8-bd83-706e98aef500;IngestionEndpoint=https://westus2-0.in.applicationinsights.azure.com/";
                })
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    services => { services.AddSingleton<IHostedService, LineCounterService>(); })
                .ConfigureAppConfiguration(builder => builder.AddUserSecrets(typeof(Program).Assembly))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}