// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

namespace Azure.Cosmos.Tables.Models
{
    /// <summary> signed identifier. </summary>
    public partial class SignedIdentifier
    {
        /// <summary> a unique id. </summary>
        public string Id { get; set; }
        /// <summary> An Access policy. </summary>
        public AccessPolicy AccessPolicy { get; set; } = new AccessPolicy();
    }
}
