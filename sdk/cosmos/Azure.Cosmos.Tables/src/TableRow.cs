// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Azure.Cosmos.Tables
{
    /// <summary>
    /// Represents a single row from the table
    /// </summary>
    public class TableEntity
    {
        private readonly IDictionary<string, object> _values;

        internal TableEntity(IDictionary<string, object> values)
        {
            _values = values;
        }
        public TableEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset Timeptamp { get; set; }

        public object this[string property]
        {
            get { return _values[property]; }
            set { _values[property] = value; }
        }

        public PropertyType GetPropertyType(string value)
        {
            return default;
        }

        internal IDictionary<string, object> ToValueDictionary()
        {
            return _values;
        }
    }
}