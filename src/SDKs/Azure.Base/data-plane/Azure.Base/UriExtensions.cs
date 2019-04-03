﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;

namespace Azure
{

    public class HttpPipelineUriBuilder
    {
        private readonly StringBuilder _pathAndQuery = new StringBuilder();

        private int _queryIndex = -1;

        private Uri _uri;

        private int _port;

        private string _host;

        private string _scheme;

        public string Scheme
        {
            get => _scheme;
            set
            {
                ResetUri();
                _scheme = value;
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                ResetUri();
                _host = value;
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                ResetUri();
                _port = value;
            }
        }

        public string Query
        {
            get => HasQuery ? _pathAndQuery.ToString(_queryIndex, _pathAndQuery.Length - _queryIndex) : string.Empty;
            set
            {
                ResetUri();
                if (HasQuery)
                {
                    _pathAndQuery.Remove(_queryIndex, _pathAndQuery.Length - _queryIndex);
                    _queryIndex = -1;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    _queryIndex = _pathAndQuery.Length;
                    _pathAndQuery.Append('?');
                    _pathAndQuery.Append(value);
                }
            }
        }

        public string Path
        {
            get => HasQuery ? _pathAndQuery.ToString(0, _queryIndex) : _pathAndQuery.ToString();
            set
            {
                if (HasQuery)
                {
                    _pathAndQuery.Remove(0, _queryIndex);
                    _pathAndQuery.Insert(0, value);
                    _queryIndex = value.Length;
                }
                else
                {
                    _pathAndQuery.Remove(0, _pathAndQuery.Length);
                    _pathAndQuery.Append(value);
                }
            }
        }

        private bool HasQuery => _queryIndex != -1;

        public string PathAndQuery => _pathAndQuery.ToString();

        public Uri Uri
        {
            get
            {
                if (_uri == null)
                {
                    _uri = new Uri(ToString());
                }
                return _uri;
            }
            set
            {
                Scheme = value.Scheme;
                Host = value.Host;
                Port = value.Port;
                Path = value.AbsolutePath;
                Query = value.Query;
                _uri = value;
            }
        }

        public void AppendQuery(string name, string value)
        {

            if (!HasQuery)
            {
                _pathAndQuery.Append('?');
                _queryIndex = _pathAndQuery.Length;
            }
            else
            {
                _pathAndQuery.Append('&');
            }

            _pathAndQuery.Append(name);
            _pathAndQuery.Append('=');
            _pathAndQuery.Append(value);
        }

        public void AppendPath(string value)
        {
            if (HasQuery)
            {
                _pathAndQuery.Insert(_queryIndex, value);
                _queryIndex += value.Length;
            }
            else
            {
                _pathAndQuery.Append(value);
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Scheme);
            stringBuilder.Append("://");
            stringBuilder.Append(Host);
            if (!HasDefaultPortForScheme)
            {
                stringBuilder.Append(':');
                stringBuilder.Append(Port);
            }
            stringBuilder.Append(_pathAndQuery);
            return stringBuilder.ToString();
        }

        private bool HasDefaultPortForScheme =>
            (Port == 80 && Scheme.Equals("http", StringComparison.InvariantCultureIgnoreCase)) ||
            (Port == 443 && Scheme.Equals("https", StringComparison.InvariantCultureIgnoreCase));

        private void ResetUri()
        {
            _uri = null;
        }
    }
}
