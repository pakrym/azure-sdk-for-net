// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Core.TestFramework
{
    internal class RecordingServer: IAsyncDisposable
    {
        private IWebHost _host;
        private RecordSession _session;

        public RecordingServer(
            string recordingFile,
            RecordedTestMode mode,
            RecordedTestSanitizer sanitizer,
            RecordMatcher matcher)
        {
            if (mode == RecordedTestMode.Live)
            {
                throw new NotSupportedException();
            }

            using FileStream fileStream = File.OpenRead(recordingFile);
            using JsonDocument jsonDocument = JsonDocument.Parse(fileStream);
            _session = RecordSession.Deserialize(jsonDocument.RootElement);
        }

        public Uri Address => new Uri(_host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First());

        public async ValueTask StartAsync()
        {
            _host = new WebHostBuilder()
                .UseKestrel(options => options.Listen(new IPEndPoint(IPAddress.Loopback, 0)))
                .ConfigureServices(services =>
                {
                    services.AddSingleton<HttpPipelineTransport>(new HttpClientTransport());
                    services.AddSingleton(_session);
                })
                .Configure(Configure)
                .Build();

            await _host.StartAsync();
        }

        private void Configure(IApplicationBuilder builder)
        {
            builder.Map("/__", b => b.UseMiddleware<ControlMiddlware>());
            builder.UseMiddleware<RecordingMiddlware>();
        }

        private class ControlMiddlware
        {
            private readonly RequestDelegate _next;

            public ControlMiddlware(RequestDelegate next)
            {
                _next = next;
            }

            public Task InvokeAsync(HttpContext context)
            {
                return Task.CompletedTask;
            }
        }

        private class RecordingMiddlware
        {
            private readonly RequestDelegate _next;
            private readonly RecordSession _session;
            private readonly HttpPipelineTransport _transport;

            public RecordingMiddlware(RequestDelegate next, RecordSession session, HttpPipelineTransport transport)
            {
                _next = next;
                _session = session;
                _transport = transport;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                using var request = _transport.CreateRequest();
                request.Uri.Reset(new Uri(context.Request.GetEncodedUrl()));
                if (context.Request.Body != null)
                {
                    request.Content = RequestContent.Create(context.Request.Body);
                }

                foreach (var header in context.Request.Headers)
                {
                    foreach (var value in header.Value)
                    {
                        request.Headers.Add(header.Key, value);
                    }
                }

                using var httpMessage = new HttpMessage(request, new ResponseClassifier());
                await _transport.ProcessAsync(httpMessage);
                var entry = RecordTransport.CreateEntry(request, httpMessage.Response);
                _session.Entries.Add(entry);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _host.StopAsync().ConfigureAwait(false);
        }
    }
}