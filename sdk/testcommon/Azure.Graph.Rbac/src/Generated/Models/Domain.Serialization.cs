// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.Graph.Rbac.Models
{
    public partial class Domain
    {
        internal static Domain DeserializeDomain(JsonElement element)
        {
            Optional<string> authenticationType = default;
            Optional<bool> isDefault = default;
            Optional<bool> isVerified = default;
            string name = default;
            IReadOnlyDictionary<string, object> additionalProperties = default;
            Dictionary<string, object> additionalPropertiesDictionary = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("authenticationType"))
                {
                    authenticationType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("isDefault"))
                {
                    isDefault = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("isVerified"))
                {
                    isVerified = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("name"))
                {
                    name = property.Value.GetString();
                    continue;
                }
                additionalPropertiesDictionary ??= new Dictionary<string, object>();
                additionalPropertiesDictionary.Add(property.Name, property.Value.GetObject());
            }
            additionalProperties = additionalPropertiesDictionary;
            return new Domain(authenticationType.Value, Optional.ToNullable(isDefault), Optional.ToNullable(isVerified), name, additionalProperties);
        }
    }
}
