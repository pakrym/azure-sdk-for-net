﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.ServiceBus
{
    using System.Xml.Linq;
    using Azure.Messaging.ServiceBus.Filters;
    using Azure.Messaging.ServiceBus.Management;

    internal static class CorrelationFilterExtensions
    {
        public static Filter ParseFromXElement(XElement xElement)
        {
            var correlationFilter = new CorrelationFilter();
            foreach (var element in xElement.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "CorrelationId":
                        correlationFilter.CorrelationId = element.Value;
                        break;
                    case "MessageId":
                        correlationFilter.MessageId = element.Value;
                        break;
                    case "To":
                        correlationFilter.To = element.Value;
                        break;
                    case "ReplyTo":
                        correlationFilter.ReplyTo = element.Value;
                        break;
                    case "Label":
                        correlationFilter.Label = element.Value;
                        break;
                    case "SessionId":
                        correlationFilter.SessionId = element.Value;
                        break;
                    case "ReplyToSessionId":
                        correlationFilter.ReplyToSessionId = element.Value;
                        break;
                    case "ContentType":
                        correlationFilter.ContentType = element.Value;
                        break;
                    case "Properties":
                        foreach (var prop in element.Elements(XName.Get("KeyValueOfstringanyType", ManagementClientConstants.SbNs)))
                        {
                            var key = prop.Element(XName.Get("Key", ManagementClientConstants.SbNs))?.Value;
                            var value = XmlObjectConvertor.ParseValueObject(prop.Element(XName.Get("Value", ManagementClientConstants.SbNs)));
                            correlationFilter.Properties.Add(key, value);
                        }
                        break;
                    default:
                        MessagingEventSource.Log.ManagementSerializationException(
                            $"{nameof(CorrelationFilterExtensions)}_{nameof(ParseFromXElement)}",
                            element.ToString());
                        break;
                }
            }

            return correlationFilter;
        }

        public static XElement Serialize(this CorrelationFilter filter)
        {
            XElement parameterElement = null;
            if (filter.properties != null)
            {
                parameterElement = new XElement(XName.Get("Properties", ManagementClientConstants.SbNs));
                foreach (var param in filter.properties)
                {
                    parameterElement.Add(
                        new XElement(XName.Get("KeyValueOfstringanyType", ManagementClientConstants.SbNs),
                            new XElement(XName.Get("Key", ManagementClientConstants.SbNs), param.Key),
                            XmlObjectConvertor.SerializeObject(param.Value)));
                }
            }

            return new XElement(
                XName.Get("Filter", ManagementClientConstants.SbNs),
                new XAttribute(XName.Get("type", ManagementClientConstants.XmlSchemaInstanceNs), nameof(CorrelationFilter)),
                string.IsNullOrWhiteSpace(filter.CorrelationId) ? null :
                    new XElement(XName.Get("CorrelationId", ManagementClientConstants.SbNs), filter.CorrelationId),
                string.IsNullOrWhiteSpace(filter.MessageId) ? null :
                    new XElement(XName.Get("MessageId", ManagementClientConstants.SbNs), filter.MessageId),
                string.IsNullOrWhiteSpace(filter.To) ? null :
                    new XElement(XName.Get("To", ManagementClientConstants.SbNs), filter.To),
                string.IsNullOrWhiteSpace(filter.ReplyTo) ? null :
                    new XElement(XName.Get("ReplyTo", ManagementClientConstants.SbNs), filter.ReplyTo),
                string.IsNullOrWhiteSpace(filter.Label) ? null :
                    new XElement(XName.Get("Label", ManagementClientConstants.SbNs), filter.Label),
                string.IsNullOrWhiteSpace(filter.SessionId) ? null :
                    new XElement(XName.Get("SessionId", ManagementClientConstants.SbNs), filter.SessionId),
                string.IsNullOrWhiteSpace(filter.ReplyToSessionId) ? null :
                    new XElement(XName.Get("ReplyToSessionId", ManagementClientConstants.SbNs), filter.ReplyToSessionId),
                string.IsNullOrWhiteSpace(filter.ContentType) ? null :
                    new XElement(XName.Get("ContentType", ManagementClientConstants.SbNs), filter.ContentType),
                parameterElement);
        }
    }
}