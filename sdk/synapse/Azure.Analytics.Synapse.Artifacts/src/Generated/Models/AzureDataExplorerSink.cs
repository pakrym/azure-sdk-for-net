// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Analytics.Synapse.Artifacts.Models
{
    /// <summary> A copy activity Azure Data Explorer sink. </summary>
    public partial class AzureDataExplorerSink : CopySink
    {
        /// <summary> Initializes a new instance of AzureDataExplorerSink. </summary>
        public AzureDataExplorerSink()
        {
            Type = "AzureDataExplorerSink";
        }

        /// <summary> Initializes a new instance of AzureDataExplorerSink. </summary>
        /// <param name="type"> Copy sink type. </param>
        /// <param name="writeBatchSize"> Write batch size. Type: integer (or Expression with resultType integer), minimum: 0. </param>
        /// <param name="writeBatchTimeout"> Write batch timeout. Type: string (or Expression with resultType string), pattern: ((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9])). </param>
        /// <param name="sinkRetryCount"> Sink retry count. Type: integer (or Expression with resultType integer). </param>
        /// <param name="sinkRetryWait"> Sink retry wait. Type: string (or Expression with resultType string), pattern: ((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9])). </param>
        /// <param name="maxConcurrentConnections"> The maximum concurrent connection count for the sink data store. Type: integer (or Expression with resultType integer). </param>
        /// <param name="additionalProperties"> Additional Properties. </param>
        /// <param name="ingestionMappingName"> A name of a pre-created csv mapping that was defined on the target Kusto table. Type: string. </param>
        /// <param name="ingestionMappingAsJson"> An explicit column mapping description provided in a json format. Type: string. </param>
        /// <param name="flushImmediately"> If set to true, any aggregation will be skipped. Default is false. Type: boolean. </param>
        internal AzureDataExplorerSink(string type, object writeBatchSize, object writeBatchTimeout, object sinkRetryCount, object sinkRetryWait, object maxConcurrentConnections, IDictionary<string, object> additionalProperties, object ingestionMappingName, object ingestionMappingAsJson, object flushImmediately) : base(type, writeBatchSize, writeBatchTimeout, sinkRetryCount, sinkRetryWait, maxConcurrentConnections, additionalProperties)
        {
            IngestionMappingName = ingestionMappingName;
            IngestionMappingAsJson = ingestionMappingAsJson;
            FlushImmediately = flushImmediately;
            Type = type ?? "AzureDataExplorerSink";
        }

        /// <summary> A name of a pre-created csv mapping that was defined on the target Kusto table. Type: string. </summary>
        public object IngestionMappingName { get; set; }
        /// <summary> An explicit column mapping description provided in a json format. Type: string. </summary>
        public object IngestionMappingAsJson { get; set; }
        /// <summary> If set to true, any aggregation will be skipped. Default is false. Type: boolean. </summary>
        public object FlushImmediately { get; set; }
    }
}
