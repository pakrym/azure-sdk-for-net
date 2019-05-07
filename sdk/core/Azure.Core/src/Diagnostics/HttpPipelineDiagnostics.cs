// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;

namespace Azure.Core.Pipeline
{
    public class HttpPipelineDiagnostics
    {
        internal static readonly DiagnosticListener DiagnosticListener = new DiagnosticListener("Azure.Core.Client");

        public HttpPipelineOperation StartOperation(string name)
        {
            Activity activity = null;

            if (DiagnosticListener.IsEnabled(name))
            {
                activity = new Activity(name);
                DiagnosticListener.StartActivity(activity, null);
            }

            return new HttpPipelineOperation(activity, DiagnosticListener);
        }
    }
}
