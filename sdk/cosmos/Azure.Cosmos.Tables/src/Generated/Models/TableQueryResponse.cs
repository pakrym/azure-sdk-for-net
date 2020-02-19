// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

using System.Collections.Generic;
using Azure.Cosmos.Tables;

namespace Azure.Cosmos.Tables.Models
{
    /// <summary> The properties for the table query response. </summary>
    public partial class TableQueryResponse
    {
        /// <summary> The metadata response of the table. </summary>
        public string OdataMetadata { get; set; }
        /// <summary> List of tables. </summary>
        public ICollection<TableItem> Value { get; set; }
    }
}
