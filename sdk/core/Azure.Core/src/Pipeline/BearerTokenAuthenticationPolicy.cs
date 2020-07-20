// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Pipeline
{
    /// <summary>
    /// A policy that sends an <see cref="AccessToken"/> provided by a <see cref="TokenCredential"/> as an Authentication header.
    /// </summary>
    public class BearerTokenAuthenticationPolicy : HttpPipelinePolicy
    {
        private readonly AccessTokenCache _accessTokenCache;

        /// <summary>
        /// Creates a new instance of <see cref="BearerTokenAuthenticationPolicy"/> using provided token credential and scope to authenticate for.
        /// </summary>
        /// <param name="credential">The token credential to use for authentication.</param>
        /// <param name="scope">The scope to authenticate for.</param>
        public BearerTokenAuthenticationPolicy(TokenCredential credential, string scope) : this(credential, new[] { scope }) { }

        /// <summary>
        /// Creates a new instance of <see cref="BearerTokenAuthenticationPolicy"/> using provided token credential and scopes to authenticate for.
        /// </summary>
        /// <param name="credential">The token credential to use for authentication.</param>
        /// <param name="scopes">Scopes to authenticate for.</param>
        public BearerTokenAuthenticationPolicy(TokenCredential credential, IEnumerable<string> scopes)
            : this(credential, scopes, TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(30)) { }

        internal BearerTokenAuthenticationPolicy(TokenCredential credential, IEnumerable<string> scopes, TimeSpan tokenRefreshOffset, TimeSpan tokenRefreshRetryTimeout) {
            Argument.AssertNotNull(credential, nameof(credential));
            Argument.AssertNotNull(scopes, nameof(scopes));

            _accessTokenCache = new AccessTokenCache(tokenRefreshOffset, tokenRefreshRetryTimeout, credential, scopes.ToArray());
        }

        /// <inheritdoc />
        public override ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            return ProcessAsync(message, pipeline, true);
        }

        /// <inheritdoc />
        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            ProcessAsync(message, pipeline, false).EnsureCompleted();
        }

        /// <inheritdoc />
        private async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, bool async)
        {
            if (message.Request.Uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new InvalidOperationException("Bearer token authentication is not permitted for non TLS protected (https) endpoints.");
            }

            string headerValue = await _accessTokenCache.GetHeaderValueAsync(message, async);

            message.Request.SetHeader(HttpHeader.Names.Authorization, headerValue);

            if (async)
            {
                await ProcessNextAsync(message, pipeline).ConfigureAwait(false);
            }
            else
            {
                ProcessNext(message, pipeline);
            }
        }


        private class AccessTokenCache
        {

            private readonly object _syncObj = new object();
            private readonly TimeSpan _tokenRefreshOffset;
            private readonly TimeSpan _tokenRefreshRetryTimeout;
            private readonly TokenCredential _credential;
            private readonly string[] _scopes;

            private ValueTask<TokenInfo> _headerTask;

            public AccessTokenCache(TimeSpan tokenRefreshOffset, TimeSpan tokenRefreshRetryTimeout, TokenCredential credential, string[] scopes)
            {
                _tokenRefreshOffset = tokenRefreshOffset;
                _tokenRefreshRetryTimeout = tokenRefreshRetryTimeout;
                _credential = credential;
                _scopes = scopes;
            }

            private async ValueTask<TokenInfo> GetHeaderValueFromCredentialAsync(HttpMessage message, bool async)
            {
                var requestContext = new TokenRequestContext(_scopes, message.Request.ClientRequestId);

                AccessToken token = async
                    ? await _credential.GetTokenAsync(requestContext, message.CancellationToken).ConfigureAwait(false)
                    : _credential.GetToken(requestContext, message.CancellationToken);

                return new TokenInfo("Bearer " + token.Token, token.ExpiresOn, DateTimeOffset.Now + _tokenRefreshOffset) ;
            }

            public async ValueTask<string> GetHeaderValueAsync(HttpMessage message, bool async)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;

                ValueTask<TokenInfo> pendingTask;
                TaskCompletionSource<TokenInfo>? inlineRefreshTaskCompletionSource = null;
                bool backgroundRefresh = false;

                lock (_syncObj)
                {
                    // Are we the first request or expired token start a new TSC
                    if (_headerTask == null ||
                        (_headerTask.IsCompleted && now >= _headerTask.Result.ExpiresOn))
                    {
                        inlineRefreshTaskCompletionSource = new TaskCompletionSource<TokenInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
                        _headerTask = new ValueTask<TokenInfo>(inlineRefreshTaskCompletionSource.Task);
                    }
                    // If it's time to try a background refresh update the next refresh time
                    else if (_headerTask.IsCompleted && now >= _headerTask.Result.RefreshOn)
                    {
                        backgroundRefresh = true;
                        _headerTask = new ValueTask<TokenInfo>(_headerTask.Result.WithNewRefreshTime(_tokenRefreshRetryTimeout));
                    }

                    pendingTask = _headerTask;
                }

                // if taskCompletionSource is not null refresh needs to happen inline
                if (inlineRefreshTaskCompletionSource != null)
                {
                    try
                    {
                        var tokenToken = await GetHeaderValueFromCredentialAsync(message, async).ConfigureAwait(false);
                        inlineRefreshTaskCompletionSource.SetResult(tokenToken);
                    }
                    catch (Exception e)
                    {
                        inlineRefreshTaskCompletionSource.SetException(e);
                    }
                }

                // It's time to try and refresh the token in the background
                if (backgroundRefresh)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var getTokenTask = GetHeaderValueFromCredentialAsync(message, async);
                            await getTokenTask.ConfigureAwait(false);
                            UpdateHeaderValue(getTokenTask);
                        }
                        catch (Exception)
                        {
                            // Log and suppress
                        }
                    });
                }

                // Fast path
                if (pendingTask.IsCompleted)
                {
                    return pendingTask.GetAwaiter().GetResult().HeaderValue;
                }

                // Wait with cancellation token or block for the result
                TokenInfo tokenInfo;
                if (async)
                {
                    tokenInfo = await pendingTask.AwaitWithCancellation(message.CancellationToken);
                }
                else
                {
                    try
                    {
                        pendingTask.AsTask().Wait(message.CancellationToken);
                    }
                    catch (AggregateException) { } // ignore exception here to rethrow it with EnsureCompleted

                    tokenInfo = pendingTask.EnsureCompleted();
                }

                return tokenInfo.HeaderValue;
            }

            private void UpdateHeaderValue(ValueTask<TokenInfo> headerTask)
            {
                lock (_syncObj)
                {
                    _headerTask = headerTask;
                }
            }

            private readonly struct TokenInfo
            {
                public string HeaderValue { get; }
                public DateTimeOffset ExpiresOn { get; }
                public DateTimeOffset RefreshOn { get; }

                public TokenInfo(string headerValue, DateTimeOffset expiresOn, DateTimeOffset refreshOn)
                {
                    HeaderValue = headerValue;
                    ExpiresOn = expiresOn;
                    RefreshOn = refreshOn;
                }

                public TokenInfo WithNewRefreshTime(TimeSpan tokenRefreshRetryTimeout)
                {
                    return new TokenInfo(HeaderValue, ExpiresOn, RefreshOn + tokenRefreshRetryTimeout);
                }
            }
        }
    }
}
