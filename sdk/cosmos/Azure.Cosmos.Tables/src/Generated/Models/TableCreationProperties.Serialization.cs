// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Azure.Core;

namespace Azure.Cosmos.Tables
{
    internal partial class TableCreationProperties : IUtf8JsonSerializable, IXmlSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (TableName != null)
            {
                writer.WritePropertyName("TableName");
                writer.WriteStringValue(TableName);
            }
            writer.WriteEndObject();
        }
        internal static TableCreationProperties DeserializeTableCreationProperties(JsonElement element)
        {
            TableCreationProperties result = new TableCreationProperties();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("TableName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    result.TableName = property.Value.GetString();
                    continue;
                }
            }
            return result;
        }
        void IXmlSerializable.Write(XmlWriter writer, string nameHint)
        {
            writer.WriteStartElement(nameHint ?? "TableProperties");
            if (TableName != null)
            {
                writer.WriteStartElement("TableName");
                writer.WriteValue(TableName);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        internal static TableCreationProperties DeserializeTableCreationProperties(XElement element)
        {
            TableCreationProperties result = default;
            result = new TableCreationProperties(); string value = default;
            var tableName = element.Element("TableName");
            if (tableName != null)
            {
                value = (string)tableName;
            }
            result.TableName = value;
            return result;
        }
    }
}
