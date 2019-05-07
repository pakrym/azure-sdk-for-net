// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline.Policies
{
    public class DistributedTracingPolicy : HttpPipelinePolicy
    {
        private static string HttpRequestActivityName = "HttpRequest";
        private static string HttpErrorResponseEvent = "HttpResponseError";
        private static string HttpErrorResponseException = "HttpResponseException";

        public override async Task ProcessAsync(HttpPipelineMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            await ProcessAsync(message, pipeline, true);
        }

        public override void Process(HttpPipelineMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            ProcessAsync(message, pipeline, false).EnsureCompleted();
        }

        private static async Task ProcessAsync(HttpPipelineMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool isAsync)
        {
            var diagnosticSource = HttpPipelineDiagnostics.DiagnosticListener;
            bool isEnabled = diagnosticSource.IsEnabled();

            if (!isEnabled)
            {
                if (isAsync)
                {
                    await ProcessNextAsync(pipeline, message).ConfigureAwait(false);
                }
                else
                {
                    ProcessNext(pipeline, message);
                }
            }

            Activity activity = null;
            if (diagnosticSource.IsEnabled(HttpRequestActivityName))
            {
                activity = new Activity(HttpRequestActivityName);
                activity.AddTag("Uri", message.Request.UriBuilder.ToString());
                activity.AddTag("Method", HttpPipelineMethodConverter.ToString(message.Request.Method));
                diagnosticSource.StartActivity(activity, new { Message = message });
            }

            try
            {
                if (isAsync)
                {
                    await ProcessNextAsync(pipeline, message).ConfigureAwait(false);
                }
                else
                {
                    ProcessNext(pipeline, message);
                }

                if (isEnabled && message.ResponseClassifier.IsErrorResponse(message.Response))
                {
                    diagnosticSource.Write(HttpErrorResponseEvent, new { Message = message });
                }
            }
            catch (Exception ex)
            {
                if (isEnabled)
                {
                    diagnosticSource.Write(HttpErrorResponseException, new { Message = message, Exception = ex });
                }
            }
            finally
            {
                if (activity != null)
                {
                    activity.AddTag("ResponseStatus", message.Response.Status.ToString());
                    diagnosticSource.StopActivity(activity, new { Message = message });
                }
            }
        }

    }
}
