// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Analytics.Synapse.Artifacts.Models
{
    /// <summary> A copy activity snowflake sink. </summary>
    public partial class SnowflakeSink : CopySink
    {
        /// <summary> Initializes a new instance of SnowflakeSink. </summary>
        public SnowflakeSink()
        {
            Type = "SnowflakeSink";
        }

        /// <summary> Initializes a new instance of SnowflakeSink. </summary>
        /// <param name="type"> Copy sink type. </param>
        /// <param name="writeBatchSize"> Write batch size. Type: integer (or Expression with resultType integer), minimum: 0. </param>
        /// <param name="writeBatchTimeout"> Write batch timeout. Type: string (or Expression with resultType string), pattern: ((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9])). </param>
        /// <param name="sinkRetryCount"> Sink retry count. Type: integer (or Expression with resultType integer). </param>
        /// <param name="sinkRetryWait"> Sink retry wait. Type: string (or Expression with resultType string), pattern: ((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9])). </param>
        /// <param name="maxConcurrentConnections"> The maximum concurrent connection count for the sink data store. Type: integer (or Expression with resultType integer). </param>
        /// <param name="additionalProperties"> Additional Properties. </param>
        /// <param name="preCopyScript"> SQL pre-copy script. Type: string (or Expression with resultType string). </param>
        /// <param name="importSettings"> Snowflake import settings. </param>
        internal SnowflakeSink(string type, object writeBatchSize, object writeBatchTimeout, object sinkRetryCount, object sinkRetryWait, object maxConcurrentConnections, IDictionary<string, object> additionalProperties, object preCopyScript, SnowflakeImportCopyCommand importSettings) : base(type, writeBatchSize, writeBatchTimeout, sinkRetryCount, sinkRetryWait, maxConcurrentConnections, additionalProperties)
        {
            PreCopyScript = preCopyScript;
            ImportSettings = importSettings;
            Type = type ?? "SnowflakeSink";
        }

        /// <summary> SQL pre-copy script. Type: string (or Expression with resultType string). </summary>
        public object PreCopyScript { get; set; }
        /// <summary> Snowflake import settings. </summary>
        public SnowflakeImportCopyCommand ImportSettings { get; set; }
    }
}
