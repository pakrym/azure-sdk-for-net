// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

namespace Azure.Cosmos.Tables
{
    /// <summary> The properties for creating a table. </summary>
    internal partial class TableCreationProperties
    {
        /// <summary> The name of the table to create. </summary>
        public string TableName { get; set; }
    }
}
