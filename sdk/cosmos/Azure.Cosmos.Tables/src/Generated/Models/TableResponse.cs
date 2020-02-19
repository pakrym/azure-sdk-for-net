// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

namespace Azure.Cosmos.Tables
{
    /// <summary> The response for a single table. </summary>
    internal partial class TableResponse : TableItem
    {
        /// <summary> The metadata response of the table. </summary>
        public string OdataMetadata { get; set; }
    }
}
