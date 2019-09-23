﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.ServiceBus.Amqp
{
    using System;
    using Microsoft.Azure.Amqp;
    using Microsoft.Azure.Amqp.Encoding;
    using Microsoft.Azure.Amqp.Framing;

    internal sealed class AmqpRequestMessage
    {
        private readonly AmqpMessage requestMessage;

        private AmqpRequestMessage(string operation, TimeSpan timeout, string trackingId)
        {
            this.Map = new AmqpMap();
            this.requestMessage = AmqpMessage.Create(new AmqpValue { Value = this.Map });
            this.requestMessage.ApplicationProperties.Map[ManagementConstants.Request.Operation] = operation;
            this.requestMessage.ApplicationProperties.Map[ManagementConstants.Properties.ServerTimeout] = (uint)timeout.TotalMilliseconds;
            this.requestMessage.ApplicationProperties.Map[ManagementConstants.Properties.TrackingId] = trackingId ?? Guid.NewGuid().ToString();
        }

        public AmqpMessage AmqpMessage => this.requestMessage;

        public AmqpMap Map { get; }

        public static AmqpRequestMessage CreateRequest(string operation, TimeSpan timeout, string trackingId)
        {
            return new AmqpRequestMessage(operation, timeout, trackingId);
        }
    }
}