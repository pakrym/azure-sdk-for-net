// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.HybridCompute.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Describes a hybrid machine Update.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class MachineUpdate : UpdateResource
    {
        /// <summary>
        /// Initializes a new instance of the MachineUpdate class.
        /// </summary>
        public MachineUpdate()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MachineUpdate class.
        /// </summary>
        /// <param name="tags">Resource tags</param>
        /// <param name="type">The identity type.</param>
        /// <param name="principalId">The identity's principal id.</param>
        /// <param name="tenantId">The identity's tenant id.</param>
        public MachineUpdate(IDictionary<string, string> tags = default(IDictionary<string, string>), string type = default(string), string principalId = default(string), string tenantId = default(string), LocationData locationData = default(LocationData))
            : base(tags)
        {
            Type = type;
            PrincipalId = principalId;
            TenantId = tenantId;
            LocationData = locationData;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the identity type.
        /// </summary>
        [JsonProperty(PropertyName = "identity.type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets the identity's principal id.
        /// </summary>
        [JsonProperty(PropertyName = "identity.principalId")]
        public string PrincipalId { get; private set; }

        /// <summary>
        /// Gets the identity's tenant id.
        /// </summary>
        [JsonProperty(PropertyName = "identity.tenantId")]
        public string TenantId { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "properties.locationData")]
        public LocationData LocationData { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (LocationData != null)
            {
                LocationData.Validate();
            }
        }
    }
}
