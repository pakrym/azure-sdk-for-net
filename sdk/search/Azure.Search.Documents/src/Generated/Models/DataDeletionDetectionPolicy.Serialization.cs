// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Search.Documents.Models
{
    public partial class DataDeletionDetectionPolicy : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("@odata.type");
            writer.WriteStringValue(ODataType);
            writer.WriteEndObject();
        }

        internal static DataDeletionDetectionPolicy DeserializeDataDeletionDetectionPolicy(JsonElement element)
        {
            if (element.TryGetProperty("@odata.type", out JsonElement discriminator))
            {
                switch (discriminator.GetString())
                {
                    case "#Microsoft.Azure.Search.SoftDeleteColumnDeletionDetectionPolicy": return SoftDeleteColumnDeletionDetectionPolicy.DeserializeSoftDeleteColumnDeletionDetectionPolicy(element);
                }
            }
            string odataType = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("@odata.type"))
                {
                    odataType = property.Value.GetString();
                    continue;
                }
            }
            return new DataDeletionDetectionPolicy(odataType);
        }
    }
}
