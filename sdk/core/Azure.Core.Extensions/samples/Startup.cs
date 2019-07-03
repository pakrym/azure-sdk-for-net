// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core.Pipeline;
using Azure.Core.Pipeline.Policies;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<DIEnabledPolicy>();

            services.AddAzureClients(builder => {

                builder.AddKeyVaultSecrets(Configuration.GetSection("KeyVault"))
                    .WithName("Default")
                    .WithCredential(new DefaultAzureCredential())
                    .ConfigureOptions(options => options.RetryPolicy.MaxRetries = 10);

                builder.AddKeyVaultSecrets(new Uri("http://my.keyvault.com"));

                builder.UseCredential(new DefaultAzureCredential());

                // This would use configuration for auth and client settings
                builder.UseDefaultConfiguration(Configuration.GetSection("Default"));

                // Configure global defaults
                builder.ConfigureDefaults(options => options.RetryPolicy.Mode = RetryMode.Exponential);

                // Advanced configure global defaults
                builder.ConfigureDefaults((options, provider) =>  options.AddPolicy(HttpPipelinePosition.PerCall, provider.GetService<DIEnabledPolicy>()));

                // Configure default credential
                // builder.UseDefaultCredential(new ClientSecretCredential("tenantId","clientId","clientSecret"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SecretClient secretClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context => {
                context.Response.ContentType = "text";
                foreach (var secret in secretClient.GetSecrets())
                {
                    await context.Response.WriteAsync(secret.Value.Name + Environment.NewLine);
                }
            });
        }
    }
}
