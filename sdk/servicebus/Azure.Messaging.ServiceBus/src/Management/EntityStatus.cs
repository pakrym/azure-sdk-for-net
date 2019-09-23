﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.ServiceBus.Management
{
    using System.Runtime.Serialization;

    /// <summary>The status of the messaging entity.</summary>
    public enum EntityStatus
    {
        /// <summary>The status of the messaging entity is active.</summary>
        [EnumMember]
        Active = 0,

        /// <summary>The status of the messaging entity is disabled.</summary>
        [EnumMember]
        Disabled = 1,

        /// <summary>The sending status of the messaging entity is disabled.</summary>
        [EnumMember]
        SendDisabled = 3,

        /// <summary>The receiving status of the messaging entity is disabled.</summary>
        [EnumMember]
        ReceiveDisabled = 4,

        /// <summary>The status of the messaging entity is unknown.</summary>
        [EnumMember]
        Unknown = 99
    }
}
