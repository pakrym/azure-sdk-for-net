// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Azure.Cosmos.Tables
{
    /// <summary>
    /// Represents a single row from the table
    /// </summary>
    public class TableRow
    {
        internal TableRow(IDictionary<string, object> values)
        {

        }
        public TableRow(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset Timeptamp { get; set; }

        public object this[string value]
        {
            get { return default; }
            set { return; }
        }

        public ColumnType GetColumnType(string value)
        {
            return default;
        }

        internal IDictionary<string, object> ToValueDictionary()
        {

        }
    }
}