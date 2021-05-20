// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using Azure.Core.Pipeline;
using Azure.Core.TestFramework;
using Moq;
using NUnit.Framework;

namespace Azure.Core.Tests
{
    public class HttpPipelineMessageTest
    {
        [Test]
        public void DisposeNoopsForNullResponse()
        {
            var requestMock = new Mock<Request>();
            HttpMessage message = new HttpMessage(requestMock.Object, new ResponseClassifier());
            message.Dispose();
            requestMock.Verify(r=>r.Dispose(), Times.Once);
        }

        [Test]
        public void DisposingMessageDisposesTheRequestAndResponse()
        {
            var requestMock = new Mock<Request>();
            var responseMock = new Mock<Response>();
            HttpMessage message = new HttpMessage(requestMock.Object, new ResponseClassifier());
            message.Response = responseMock.Object;
            message.Dispose();
            requestMock.Verify(r=>r.Dispose(), Times.Once);
            responseMock.Verify(r=>r.Dispose(), Times.Once);
        }

        [Test]
        public void PreserveReturnsResponseStream()
        {
            var mockStream = new Mock<Stream>();
            var response = new MockResponse(200);
            response.ContentStream = mockStream.Object;

            HttpMessage message = new HttpMessage(new MockRequest(), new ResponseClassifier());
            message.Response = response;

            Stream stream = message.ExtractResponseContent();
            Stream stream2 = message.ExtractResponseContent();

            Assert.AreSame(mockStream.Object, stream);
            Assert.AreSame(stream2, stream);
        }

        [Test]
        public void PreserveReturnsNullWhenContentIsNull()
        {
            var response = new MockResponse(200);
            response.ContentStream = null;

            HttpMessage message = new HttpMessage(new MockRequest(), new ResponseClassifier());
            message.Response = response;

            Stream stream = message.ExtractResponseContent();

            Assert.AreSame(null, stream);
        }

        [Test]
        public void PreserveSetsResponseContentToThrowingStream()
        {
            var mockStream = new Mock<Stream>();
            var response = new MockResponse(200);
            response.ContentStream = mockStream.Object;

            HttpMessage message = new HttpMessage(new MockRequest(), new ResponseClassifier());
            message.Response = response;

            Stream stream = message.ExtractResponseContent();

            Assert.AreSame(mockStream.Object, stream);
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => response.ContentStream.Read(Array.Empty<byte>(), 0, 0));
            Assert.AreEqual("The operation has called ExtractResponseContent and will provide the stream as part of its response type.", exception.Message);
        }

        [Test]
        public void ContentPropertyThrowsResponseIsExtracted()
        {
            var memoryStream = new MemoryStream();
            var response = new MockResponse(200);
            response.ContentStream = memoryStream;

            HttpMessage message = new HttpMessage(new MockRequest(), new ResponseClassifier());
            message.Response = response;

            Assert.AreEqual(memoryStream.ToArray(), message.Response.Content.ToArray());

            Stream stream = message.ExtractResponseContent();

            Assert.AreSame(memoryStream, stream);
            Assert.Throws<InvalidOperationException>(() => { var x = response.Content; });
        }

        [Test]
        public void SetNetworkTimeoutProperty()
        {
            HttpMessage message = new HttpMessage(new MockRequest(), new ResponseClassifier());

            message.NetworkTimeout = TimeSpan.FromHours(2);
            // + Easy to find
            // ~ Instance property ties implementation to abstraction
            // - How do I know if it works? (custom pipelines)

            message.SetNetworkTimeout(TimeSpan.FromHours(2));
            // + Easy to find
            // + Is an extension and not an instance property
            // - How do I know if it works? (custom pipelines)

            message.SetProperty("NetworkTimeout", TimeSpan.FromHours(2));
            // + Doesn't pollute the abstraction
            // - Hard to find
            // - How do I know if it works? (custom pipelines)

            message.SetProperty(ResponseBodyPolicy.NetworkTimeoutProperty, TimeSpan.FromHours(2));
            // ~ Somewhat easy to find
            // + Doesn't pollute the abstraction
            // + Specifies which policy is required
            // - Requires a public policy type
            // - Doesn't work well for properties shared between policies (ClientRequestId)

            message.SetProperty<ResponseBodyPolicy>("NetworkTimeout", TimeSpan.FromHours(2));
            // ~ Somewhat easy to find
            // - Hard to find
            // + Specifies which policy is required
            // - Requires a public policy type
            // - Doesn't work well for properties shared between policies (ClientRequestId)
        }
    }

    public static class MessageExtensions
    {
        public static void SetNetworkTimeout(this HttpMessage message, TimeSpan span)
        {
            message.SetProperty("NetworkTimeout", span);
        }
    }
}
