// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.AI.TextAnalytics;
using Azure.Core;

namespace Azure.AI.TextAnalytics.Models
{
    /// <summary> The AnalyzeJobErrorsAndStatistics. </summary>
    internal partial class AnalyzeJobErrorsAndStatistics
    {
        /// <summary> Initializes a new instance of AnalyzeJobErrorsAndStatistics. </summary>
        internal AnalyzeJobErrorsAndStatistics()
        {
            Errors = new ChangeTrackingList<TextAnalyticsErrorInternal>();
        }

        /// <summary> Initializes a new instance of AnalyzeJobErrorsAndStatistics. </summary>
        /// <param name="errors"> . </param>
        /// <param name="statistics"> if showStats=true was specified in the request this field will contain information about the request payload. </param>
        internal AnalyzeJobErrorsAndStatistics(IReadOnlyList<TextAnalyticsErrorInternal> errors, TextDocumentBatchStatistics statistics)
        {
            Errors = errors;
            Statistics = statistics;
        }

        public IReadOnlyList<TextAnalyticsErrorInternal> Errors { get; }
        /// <summary> if showStats=true was specified in the request this field will contain information about the request payload. </summary>
        public TextDocumentBatchStatistics Statistics { get; }
    }
}
