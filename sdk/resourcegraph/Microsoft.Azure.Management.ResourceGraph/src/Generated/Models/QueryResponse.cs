// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.ResourceGraph.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Query result.
    /// </summary>
    public partial class QueryResponse
    {
        /// <summary>
        /// Initializes a new instance of the QueryResponse class.
        /// </summary>
        public QueryResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryResponse class.
        /// </summary>
        /// <param name="totalRecords">Number of total records matching the
        /// query.</param>
        /// <param name="count">Number of records returned in the current
        /// response. In the case of paging, this is the number of records in
        /// the current page.</param>
        /// <param name="resultTruncated">Indicates whether the query results
        /// are truncated. Possible values include: 'true', 'false'</param>
        /// <param name="data">Query output in JObject array or Table
        /// format.</param>
        /// <param name="skipToken">When present, the value can be passed to a
        /// subsequent query call (together with the same query and scopes used
        /// in the current request) to retrieve the next page of data.</param>
        /// <param name="facets">Query facets.</param>
        public QueryResponse(long totalRecords, long count, ResultTruncated resultTruncated, object data, string skipToken = default(string), IList<Facet> facets = default(IList<Facet>))
        {
            TotalRecords = totalRecords;
            Count = count;
            ResultTruncated = resultTruncated;
            SkipToken = skipToken;
            Data = data;
            Facets = facets;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets number of total records matching the query.
        /// </summary>
        [JsonProperty(PropertyName = "totalRecords")]
        public long TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets number of records returned in the current response. In
        /// the case of paging, this is the number of records in the current
        /// page.
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public long Count { get; set; }

        /// <summary>
        /// Gets or sets indicates whether the query results are truncated.
        /// Possible values include: 'true', 'false'
        /// </summary>
        [JsonProperty(PropertyName = "resultTruncated")]
        public ResultTruncated ResultTruncated { get; set; }

        /// <summary>
        /// Gets or sets when present, the value can be passed to a subsequent
        /// query call (together with the same query and scopes used in the
        /// current request) to retrieve the next page of data.
        /// </summary>
        [JsonProperty(PropertyName = "$skipToken")]
        public string SkipToken { get; set; }

        /// <summary>
        /// Gets or sets query output in JObject array or Table format.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets query facets.
        /// </summary>
        [JsonProperty(PropertyName = "facets")]
        public IList<Facet> Facets { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Data == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Data");
            }
            if (Facets != null)
            {
                foreach (var element in Facets)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
