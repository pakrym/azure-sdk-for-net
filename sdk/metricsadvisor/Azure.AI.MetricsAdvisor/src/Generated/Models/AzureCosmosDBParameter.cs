// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.AI.MetricsAdvisor.Models
{
    /// <summary> The AzureCosmosDBParameter. </summary>
    internal partial class AzureCosmosDBParameter
    {
        /// <summary> Initializes a new instance of AzureCosmosDBParameter. </summary>
        /// <param name="connectionString"> Azure CosmosDB connection string. </param>
        /// <param name="sqlQuery"> Query script. </param>
        /// <param name="database"> Database name. </param>
        /// <param name="collectionId"> Collection id. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="connectionString"/>, <paramref name="sqlQuery"/>, <paramref name="database"/>, or <paramref name="collectionId"/> is null. </exception>
        public AzureCosmosDBParameter(string connectionString, string sqlQuery, string database, string collectionId)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            if (sqlQuery == null)
            {
                throw new ArgumentNullException(nameof(sqlQuery));
            }
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }
            if (collectionId == null)
            {
                throw new ArgumentNullException(nameof(collectionId));
            }

            ConnectionString = connectionString;
            SqlQuery = sqlQuery;
            Database = database;
            CollectionId = collectionId;
        }

        /// <summary> Azure CosmosDB connection string. </summary>
        public string ConnectionString { get; set; }
        /// <summary> Query script. </summary>
        public string SqlQuery { get; set; }
        /// <summary> Database name. </summary>
        public string Database { get; set; }
        /// <summary> Collection id. </summary>
        public string CollectionId { get; set; }
    }
}