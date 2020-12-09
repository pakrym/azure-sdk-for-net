// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.Communication.Administration.Models
{
    internal partial class PhonePlanGroups
    {
        internal static PhonePlanGroups DeserializePhonePlanGroups(JsonElement element)
        {
            Optional<IReadOnlyList<PhonePlanGroup>> phonePlanGroups = default;
            Optional<string> nextLink = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("phonePlanGroups"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<PhonePlanGroup> array = new List<PhonePlanGroup>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(PhonePlanGroup.DeserializePhonePlanGroup(item));
                    }
                    phonePlanGroups = array;
                    continue;
                }
                if (property.NameEquals("nextLink"))
                {
                    nextLink = property.Value.GetString();
                    continue;
                }
            }
            return new PhonePlanGroups(Optional.ToList(phonePlanGroups), nextLink.Value);
        }
    }
}
