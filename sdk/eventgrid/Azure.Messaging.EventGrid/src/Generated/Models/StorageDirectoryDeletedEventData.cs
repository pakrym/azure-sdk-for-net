// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.SystemEvents
{
    /// <summary> Schema of the Data property of an EventGridEvent for a Microsoft.Storage.DirectoryDeleted event. </summary>
    public partial class StorageDirectoryDeletedEventData
    {
        /// <summary> Initializes a new instance of StorageDirectoryDeletedEventData. </summary>
        internal StorageDirectoryDeletedEventData()
        {
        }

        /// <summary> Initializes a new instance of StorageDirectoryDeletedEventData. </summary>
        /// <param name="api"> The name of the API/operation that triggered this event. </param>
        /// <param name="clientRequestId"> A request id provided by the client of the storage API operation that triggered this event. </param>
        /// <param name="requestId"> The request id generated by the storage service for the storage API operation that triggered this event. </param>
        /// <param name="url"> The path to the deleted directory. </param>
        /// <param name="recursive"> Is this event for a recursive delete operation. </param>
        /// <param name="sequencer"> An opaque string value representing the logical sequence of events for any particular directory name. Users can use standard string comparison to understand the relative sequence of two events on the same directory name. </param>
        /// <param name="identity"> The identity of the requester that triggered this event. </param>
        /// <param name="storageDiagnostics"> For service use only. Diagnostic data occasionally included by the Azure Storage service. This property should be ignored by event consumers. </param>
        internal StorageDirectoryDeletedEventData(string api, string clientRequestId, string requestId, string url, bool? recursive, string sequencer, string identity, object storageDiagnostics)
        {
            Api = api;
            ClientRequestId = clientRequestId;
            RequestId = requestId;
            Url = url;
            Recursive = recursive;
            Sequencer = sequencer;
            Identity = identity;
            StorageDiagnostics = storageDiagnostics;
        }

        /// <summary> The name of the API/operation that triggered this event. </summary>
        public string Api { get; }
        /// <summary> A request id provided by the client of the storage API operation that triggered this event. </summary>
        public string ClientRequestId { get; }
        /// <summary> The request id generated by the storage service for the storage API operation that triggered this event. </summary>
        public string RequestId { get; }
        /// <summary> The path to the deleted directory. </summary>
        public string Url { get; }
        /// <summary> Is this event for a recursive delete operation. </summary>
        public bool? Recursive { get; }
        /// <summary> An opaque string value representing the logical sequence of events for any particular directory name. Users can use standard string comparison to understand the relative sequence of two events on the same directory name. </summary>
        public string Sequencer { get; }
        /// <summary> The identity of the requester that triggered this event. </summary>
        public string Identity { get; }
        /// <summary> For service use only. Diagnostic data occasionally included by the Azure Storage service. This property should be ignored by event consumers. </summary>
        public object StorageDiagnostics { get; }
    }
}
