// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

namespace Azure.Core.Pipeline
{
    public struct HttpPipelineOperation: IDisposable
    {
        public Activity Activity { get; }

        private readonly DiagnosticListener _diagnosticListener;

        public HttpPipelineOperation(Activity activity, DiagnosticListener diagnosticListener)
        {
            Activity = activity;
            _diagnosticListener = diagnosticListener;
        }

        public void Dispose()
        {
            if (Activity != null)
            {
                _diagnosticListener.StopActivity(Activity, null);
            }
        }
    }
}
