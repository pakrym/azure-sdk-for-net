// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

#nullable enable

namespace Azure.Core.Pipeline
{
    internal class ClientDiagnostics : DiagnosticScopeFactory
    {
        public ClientDiagnostics(ClientOptions options) : base(
            options.GetType().Namespace!,
            GetResourceProviderNamespace(options.GetType().Assembly),
            options.Diagnostics.IsDistributedTracingEnabled)
        {
        }

    }
}
