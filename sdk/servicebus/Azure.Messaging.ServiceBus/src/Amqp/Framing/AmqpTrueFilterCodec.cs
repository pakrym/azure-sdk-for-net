﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.ServiceBus.Amqp.Framing
{
    using Microsoft.Azure.Amqp;

    internal sealed class AmqpTrueFilterCodec : AmqpFilterCodec
    {
        public static readonly string Name = AmqpConstants.Vendor + ":true-filter:list";
        public const ulong Code = 0x000001370000007;

        public AmqpTrueFilterCodec() : base(Name, Code) { }

        public override string ToString()
        {
            return "true()";
        }
    }
}
