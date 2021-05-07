// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Azure.Data.AppConfiguration
{
    /// <summary>
    /// Represents a configuration setting that references as KeyVault secret.
    /// </summary>
    public class SecretReferenceValue
    {
        /// <summary>
        ///
        /// </summary>
        public static string SecretReferenceContentType { get; } = "application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8";

        /// <summary>
        /// Creates a <see cref="SecretReferenceValue"/> referencing the provided KeyVault secret.
        /// </summary>
        /// <param name="secretId">The secret identifier to reference.</param>
        public SecretReferenceValue(Uri secretId)
        {
            SecretId = secretId;
        }

        /// <summary>
        /// The secret identifier.
        /// </summary>
        public Uri SecretId { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool TryParse(string value, out SecretReferenceValue secretReferenceValue)
        {
            secretReferenceValue = null;
            try
            {
                using var document = JsonDocument.Parse(value);
                var root = document.RootElement;

                if (!root.TryGetProperty("uri", out var uriProperty))
                {
                    return false;
                }

                if (!Uri.TryCreate(uriProperty.GetString(), UriKind.Absolute, out Uri uriValue))
                {
                    return false;
                }

                secretReferenceValue = new SecretReferenceValue(uriValue);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            using var memoryStream = new MemoryStream();
            using var writer = new Utf8JsonWriter(memoryStream);

            writer.WriteStartObject();
            writer.WriteString("uri", SecretId.AbsoluteUri);
            writer.WriteEndObject();
            writer.Flush();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}