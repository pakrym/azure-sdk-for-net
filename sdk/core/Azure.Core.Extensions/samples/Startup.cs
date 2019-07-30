// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Collector.AspNetCore;
using OpenTelemetry.Collector.Dependencies;
using OpenTelemetry.Exporter.ApplicationInsights;
using OpenTelemetry.Exporter.Zipkin;
using OpenTelemetry.Stats;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Sampler;

namespace Azure.Core.Extensions.Samples
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Registering policy to use in ConfigureDefaults later
            services.AddSingleton<DependencyInjectionEnabledPolicy>();

            services.AddAzureClients(builder => {

                builder.AddSecretClient(Configuration.GetSection("KeyVault"))
                    .ConfigureOptions(options => options.Retry.MaxRetries = 10);

                builder.UseCredential(new DefaultAzureCredential());

                // This would use configuration for auth and client settings
                builder.ConfigureDefaults(Configuration.GetSection("Default"));

                // Configure global defaults
                builder.ConfigureDefaults(options => options.Retry.Mode = RetryMode.Exponential);

                // Advanced configure global defaults
                builder.ConfigureDefaults((options, provider) =>  options.AddPolicy(HttpPipelinePosition.PerCall, provider.GetService<DependencyInjectionEnabledPolicy>()));

                builder.AddBlobServiceClient(Configuration.GetSection("Storage"))
                        .WithVersion(BlobClientOptions.ServiceVersion.V2018_11_09)
                        .ConfigureOptions(options => options.Diagnostics.IsLoggingEnabled = true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable 618
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SecretClient secretClient, BlobServiceClient blobServiceClient)
#pragma warning restore 618
        {
            var tracer = Tracing.Tracer;
            var sampler = Samplers.AlwaysSample;

            var exporter = new ZipkinTraceExporter(
                new ZipkinTraceExporterOptions()
                {
                    Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans"),
                    ServiceName = typeof(Program).Assembly.GetName().Name,
                },
                Tracing.SpanExporter);
            var aiExporter = new ApplicationInsightsExporter(
                Tracing.SpanExporter, Stats.ViewManager, new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration()
                {
                    InstrumentationKey = "ff2bad89-019f-4bd8-bd83-706e98aef500"
                });
            aiExporter.Start();
            exporter.Start();

            var requestsCollector = new RequestsCollector(new RequestsCollectorOptions(), tracer, sampler);
            var dependencyCollector = new DependenciesCollector(new DependenciesCollectorOptions(), tracer, sampler);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context => {
                context.Response.ContentType = "text";

                if (context.Request.Path == "/sub")
                {
                    await blobServiceClient.GetBlobContainerClient("myblobcontainer").GetBlobClient(context.Request.Query["name"]).DownloadAsync();
                }
                else
                {
                    //await context.Response.WriteAsync("Secrets" + Environment.NewLine);

                    //await foreach (var response in secretClient.GetSecretsAsync())
                    //{
                    //    await context.Response.WriteAsync(response.Value.Name + Environment.NewLine);
                    //}

                    await context.Response.WriteAsync("Blobs" + Environment.NewLine);

                    await foreach (var response in blobServiceClient.GetBlobContainerClient("myblobcontainer").GetBlobsAsync())
                    {
                        await context.Response.WriteAsync(response.Value.Name + Environment.NewLine);

                        await new HttpClient().GetAsync("http://" + context.Request.Host.ToString() + "/sub?name="+response.Value.Name);
                    }

                }
            });
        }
    }
}
