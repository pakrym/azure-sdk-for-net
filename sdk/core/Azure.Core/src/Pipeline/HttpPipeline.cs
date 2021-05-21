// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline
{
    /// <summary>
    /// Represents a primitive for sending HTTP requests and receiving responses extensible by adding <see cref="HttpPipelinePolicy"/> processing steps.
    /// </summary>
    public class HttpPipeline
    {
        private const string DefaultFailureMessage = "Service request failed.";

        private static readonly AsyncLocal<HttpMessagePropertiesScope?> CurrentHttpMessagePropertiesScope = new AsyncLocal<HttpMessagePropertiesScope?>();

        private readonly HttpPipelineTransport _transport;

        private readonly ReadOnlyMemory<HttpPipelinePolicy> _pipeline;
        private readonly HttpMessageSanitizer _sanitizer;

        /// <summary>
        /// Creates a new instance of <see cref="HttpPipeline"/> with the provided transport, policies and response classifier.
        /// </summary>
        /// <param name="transport">The <see cref="HttpPipelineTransport"/> to use for sending the requests.</param>
        /// <param name="policies">Policies to be invoked as part of the pipeline in order.</param>
        /// <param name="responseClassifier">The response classifier to be used in invocations.</param>
        public HttpPipeline(HttpPipelineTransport transport, HttpPipelinePolicy[]? policies = null, ResponseClassifier? responseClassifier = null):
            this(transport, policies, responseClassifier, null)
        {
        }

        internal HttpPipeline(HttpPipelineTransport transport, HttpPipelinePolicy[]? policies, ResponseClassifier? responseClassifier, ClientOptions? options)
        {
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            ResponseClassifier = responseClassifier ?? new ResponseClassifier();

            policies = policies ?? Array.Empty<HttpPipelinePolicy>();

            var all = new HttpPipelinePolicy[policies.Length + 1];
            all[policies.Length] = new HttpPipelineTransportPolicy(_transport);
            policies.CopyTo(all, 0);

            _pipeline = all;
            _sanitizer = new HttpMessageSanitizer(
                options?.Diagnostics.LoggedQueryParameters.ToArray() ?? Array.Empty<string>(),
                options?.Diagnostics.LoggedHeaderNames.ToArray() ?? Array.Empty<string>());
        }

        /// <summary>
        /// Creates a new <see cref="Request"/> instance.
        /// </summary>
        /// <returns>The request.</returns>
        public Request CreateRequest()
            => _transport.CreateRequest();

        /// <summary>
        /// Creates a new <see cref="HttpMessage"/> instance.
        /// </summary>
        /// <returns>The message.</returns>
        public HttpMessage CreateMessage()
        {
            return new HttpMessage(CreateRequest(), ResponseClassifier);
        }

        /// <summary>
        /// The <see cref="ResponseClassifier"/> instance used in this pipeline invocations.
        /// </summary>
        public ResponseClassifier ResponseClassifier { get; }

        /// <summary>
        /// Invokes the pipeline asynchronously. After the task completes response would be set to the <see cref="HttpMessage.Response"/> property.
        /// </summary>
        /// <param name="message">The <see cref="HttpMessage"/> to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public ValueTask SendAsync(HttpMessage message, CancellationToken cancellationToken)
        {
            message.CancellationToken = cancellationToken;
            AddHttpMessageProperties(message);
            return _pipeline.Span[0].ProcessAsync(message, _pipeline.Slice(1));
        }

        /// <summary>
        /// Invokes the pipeline synchronously. After the task completes response would be set to the <see cref="HttpMessage.Response"/> property.
        /// </summary>
        /// <param name="message">The <see cref="HttpMessage"/> to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        public void Send(HttpMessage message, CancellationToken cancellationToken)
        {
            message.CancellationToken = cancellationToken;
            AddHttpMessageProperties(message);
            _pipeline.Span[0].Process(message, _pipeline.Slice(1));
        }
        /// <summary>
        /// Invokes the pipeline asynchronously with the provided request.
        /// </summary>
        /// <param name="request">The <see cref="Request"/> to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The <see cref="ValueTask{T}"/> representing the asynchronous operation.</returns>
        public async ValueTask<Response> SendRequestAsync(Request request, CancellationToken cancellationToken)
        {
            HttpMessage message = new HttpMessage(request, ResponseClassifier);
            await SendAsync(message, cancellationToken).ConfigureAwait(false);
            return message.Response;
        }

        /// <summary>
        /// Invokes the pipeline synchronously with the provided request.
        /// </summary>
        /// <param name="request">The <see cref="Request"/> to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The <see cref="Response"/> from the server.</returns>
        public Response SendRequest(Request request, CancellationToken cancellationToken)
        {
            HttpMessage message = new HttpMessage(request, ResponseClassifier);
            Send(message, cancellationToken);
            return message.Response;
        }

        /// <summary>
        /// Creates a scope in which all outgoing requests would use the provided
        /// </summary>
        /// <param name="clientRequestId">The client request id value to be sent with request.</param>
        /// <returns>The <see cref="IDisposable"/> instance that needs to be disposed when client request id shouldn't be sent anymore.</returns>
        /// <example>
        /// Sample usage:
        /// <code snippet="Snippet:ClientRequestId">
        /// var secretClient = new SecretClient(new Uri(&quot;http://example.com&quot;), new DefaultAzureCredential());
        ///
        /// using (HttpPipeline.CreateClientRequestIdScope(&quot;&lt;custom-client-request-id&gt;&quot;))
        /// {
        ///     // The HTTP request resulting from the client call would have x-ms-client-request-id value set to &lt;custom-client-request-id&gt;
        ///     secretClient.GetSecret(&quot;&lt;secret-name&gt;&quot;);
        /// }
        /// </code>
        /// </example>
        public static IDisposable CreateClientRequestIdScope(string? clientRequestId)
        {
            return CreateHttpMessagePropertiesScope(new Dictionary<string, object?>() { { ReadClientRequestIdPolicy.MessagePropertyKey, clientRequestId } });
        }

        /// <summary>
        /// Creates a scope in which all <see cref="HttpMessage"/>s would have provided properties.
        /// </summary>
        /// <param name="messageProperties">Properties to be added to <see cref="HttpMessage"/>s</param>
        /// <returns>The <see cref="IDisposable"/> instance that needs to be disposed when properties shouldn't be used anymore.</returns>
        public static IDisposable CreateHttpMessagePropertiesScope(IDictionary<string, object?> messageProperties)
        {
            Argument.AssertNotNull(messageProperties, nameof(messageProperties));
            CurrentHttpMessagePropertiesScope.Value = new HttpMessagePropertiesScope(messageProperties, CurrentHttpMessagePropertiesScope.Value);
            return CurrentHttpMessagePropertiesScope.Value;
        }

        private static void AddHttpMessageProperties(HttpMessage message)
        {
            if (CurrentHttpMessagePropertiesScope.Value != null)
            {
                foreach (KeyValuePair<string, object?> kvp in CurrentHttpMessagePropertiesScope.Value.Properties)
                {
                    if (kvp.Value != null)
                    {
                        message.SetProperty(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        private class HttpMessagePropertiesScope : IDisposable
        {
            private readonly HttpMessagePropertiesScope? _parent;
            private bool _disposed;

            internal HttpMessagePropertiesScope(IDictionary<string, object?> messageProperties, HttpMessagePropertiesScope? parent)
            {
                if (parent != null)
                {
                    Properties = new Dictionary<string, object?>(parent.Properties);
                    foreach (var kvp in messageProperties)
                    {
                        Properties[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    Properties = new Dictionary<string, object?>(messageProperties);
                }
                _parent = parent;
            }

            public Dictionary<string, object?> Properties { get; }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }
                CurrentHttpMessagePropertiesScope.Value = _parent;
                _disposed = true;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="RequestFailedException"/> for the provided failed <see cref="HttpMessage"/>.
        /// </summary>
        /// <param name="httpMessage"></param>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="data"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public async ValueTask<RequestFailedException> CreateRequestFailedExceptionAsync(
            HttpMessage httpMessage,
            string? message = null,
            string? errorCode = null,
            IDictionary<object, object?>? data = null,
            Exception? innerException = null)
        {
            var content = await ReadContentAsync(httpMessage.Response, true).ConfigureAwait(false);
            return CreateRequestFailedExceptionWithContent(httpMessage, message, content, errorCode, data, innerException);
        }

        /// <summary>
        /// Creates an instance of <see cref="RequestFailedException"/> for the provided failed <see cref="HttpMessage"/>.
        /// </summary>
        /// <param name="httpMessage"></param>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="data"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public RequestFailedException CreateRequestFailedException(
            HttpMessage httpMessage,
            string? message = null,
            string? errorCode = null,
            IDictionary<object, object?>? data = null,
            Exception? innerException = null)
        {
            string? content = ReadContentAsync(httpMessage.Response, false).EnsureCompleted();
            return CreateRequestFailedExceptionWithContent(httpMessage, message, content, errorCode, data, innerException);
        }

        /// <summary>
        /// Partial method that can optionally be defined to extract the error
        /// message, code, and details in a service specific manner.
        /// </summary>
        /// <param name="message">The response headers.</param>
        /// <param name="textContent">The extracted text content</param>
        protected virtual ErrorResponseDetails? ExtractFailureContent(HttpMessage message, string? textContent)
        {
            try
            {
                // Optimistic check for JSON object we expect
                if (textContent == null ||
                    !textContent.StartsWith("{", StringComparison.OrdinalIgnoreCase)) return null;

                var extractFailureContent = new ErrorResponseDetails();

                using JsonDocument document = JsonDocument.Parse(textContent);
                if (document.RootElement.TryGetProperty("error", out var errorProperty))
                {
                    if (errorProperty.TryGetProperty("code", out var codeProperty))
                    {
                        extractFailureContent.ErrorCode = codeProperty.GetString();
                    }
                    if (errorProperty.TryGetProperty("message", out var messageProperty))
                    {
                        extractFailureContent.Message = messageProperty.GetString();
                    }
                }

                return extractFailureContent;
            }
            catch (Exception)
            {
                // Ignore any failures - unexpected content will be
                // included verbatim in the detailed error message
            }

            return null;
        }

        private RequestFailedException CreateRequestFailedExceptionWithContent(
            HttpMessage httpMessage,
            string? message = null,
            string? content = null,
            string? errorCode = null,
            IDictionary<object, object?>? data = null,
            Exception? innerException = null)
        {
            var errorInformation = ExtractFailureContent(httpMessage, content);
            var response = httpMessage.Response;

            message ??= errorInformation?.Message ?? DefaultFailureMessage;
            errorCode ??= errorInformation?.ErrorCode;

            if (errorInformation?.Data != null)
            {
                if (data == null)
                {
                    data = errorInformation.Data;
                }
                else
                {
                    foreach (var pair in errorInformation.Data)
                    {
                        data[pair.Key] = pair.Value;
                    }
                }
            }

            StringBuilder messageBuilder = new StringBuilder()
                .AppendLine(message)
                .Append("Status: ")
                .Append(response.Status.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(response.ReasonPhrase))
            {
                messageBuilder.Append(" (")
                    .Append(response.ReasonPhrase)
                    .AppendLine(")");
            }
            else
            {
                messageBuilder.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                messageBuilder.Append("ErrorCode: ")
                    .Append(errorCode)
                    .AppendLine();
            }

            if (data != null && data.Count > 0)
            {
                messageBuilder
                    .AppendLine()
                    .AppendLine("Additional Information:");
                foreach (KeyValuePair<object, object?> info in data)
                {
                    messageBuilder
                        .Append(info.Key)
                        .Append(": ")
                        .Append(info.Value)
                        .AppendLine();
                }
            }

            if (content != null)
            {
                messageBuilder
                    .AppendLine()
                    .AppendLine("Content:")
                    .AppendLine(content);
            }

            messageBuilder
                .AppendLine()
                .AppendLine("Headers:");

            foreach (HttpHeader responseHeader in response.Headers)
            {
                string headerValue = _sanitizer.SanitizeHeader(responseHeader.Name, responseHeader.Value);
                messageBuilder.AppendLine($"{responseHeader.Name}: {headerValue}");
            }

            var exception = new RequestFailedException(response.Status, messageBuilder.ToString(), errorCode, innerException);

            if (data != null)
            {
                foreach (KeyValuePair<object, object?> keyValuePair in data)
                {
                    exception.Data.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return exception;
        }

        private static async ValueTask<string?> ReadContentAsync(Response response, bool async)
        {
            string? content = null;

            if (response.ContentStream != null &&
                ContentTypeUtilities.TryGetTextEncoding(response.Headers.ContentType, out var encoding))
            {
                using var streamReader = new StreamReader(response.ContentStream, encoding);
                content = async ? await streamReader.ReadToEndAsync().ConfigureAwait(false) : streamReader.ReadToEnd();
            }

            return content;
        }
    }
}
