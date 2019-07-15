using System;
using System.Threading;
using Azure.ApplicationModel.Configuration;
using OpenTelemetry.Exporter.Zipkin;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Sampler;
using src;

namespace samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var tracer = Tracing.Tracer;

            var azureDiagnosticListener = new AzureDiagnosticListener(Tracing.Tracer);
            azureDiagnosticListener.Subscribe();

            var exporter = new ZipkinTraceExporter(
                new ZipkinTraceExporterOptions()
                {
                    Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans"),
                    ServiceName = typeof(Program).Assembly.GetName().Name,
                },
                Tracing.ExportComponent);
            exporter.Start();

            var appspan = tracer
                .SpanBuilder("AppRun")
                .SetSampler(Samplers.AlwaysSample)
                .StartSpan();

            using (var appscope = tracer.WithSpan(appspan))
            {

                var client = new ConfigurationClient(
                    "Endpoint=https://pakrym-azconfig-ui.azconfig.io;Id=0-l1-s0:h5pHKElA5IjSL3z+FOYz;Secret=hZyskYCWgnmEYpkAMv/R67J2nl9y2wpxcEvwB4l0DHQ=");
                client.Set("Sample_key", "Sample_value");

                for (int i = 0; i < 10; i++)
                {

                    var span = tracer
                        .SpanBuilder("incoming request " + i)
                        .SetSampler(Samplers.AlwaysSample)
                        .StartSpan();

                    using (var scope = tracer.WithSpan(span))
                    {
                        Console.WriteLine(client.Get("Sample_key").Value.Value);

                        Thread.Sleep(TimeSpan.FromSeconds(1));

                        Console.WriteLine(client.Get("Sample_key").Value.Value);


                        span.End();
                    }
                }
            }

            exporter.Stop();
        }
    }
}
