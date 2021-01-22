// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

using Reaqtive;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// Minimal Http Observer implementation
    /// </summary>
    /// <typeparam name="TSource">Type of the elements received by the observer.</typeparam>
    public class HttpObserver<TSource> : Observer<TSource>
    {
        /// <summary>
        /// Request timeout
        /// </summary>
        private static readonly int Timeout = (int)new TimeSpan(hours: 0, minutes: 0, seconds: 10).TotalMilliseconds;

        /// <summary>
        /// Cached copy of an empty headers array
        /// </summary>
        private static readonly Tuple<string, string>[] EmptyHeaders = Array.Empty<Tuple<string, string>>();

        /// <summary>
        /// Cached copy of the no-cache policy for http web requests
        /// </summary>
        private static readonly HttpRequestCachePolicy NoCacheNoStoreCaching = new(HttpRequestCacheLevel.NoCacheNoStore);

        /// <summary>
        /// Serialization helper to serialize onnext payloads
        /// </summary>
        private static readonly SerializationHelpers SerializationHelpers = new();

        /// <summary>
        /// The Uri to be pinged on completion.
        /// </summary>
        private readonly Uri _endpoint;

        /// <summary>
        /// The headers
        /// </summary>
        private readonly Tuple<string, string>[] _headers = EmptyHeaders;

        /// <summary>
        /// The retry data
        /// </summary>
        private readonly RetryData _retryData;

        /// <summary>
        /// The serialization protocol for outgoing messages
        /// </summary>
        private readonly SerializationProtocol _serializationProtocol;

        /// <summary>
        /// The HTTP verb to invoke on this request.
        /// </summary>
        private readonly string _method;

        private Uri _subscriptionId;

        public HttpObserver(
            int method,
            Uri endpoint,
            Tuple<string, string>[] headers,
            int serializationProtocol,
            RetryData retryData)
            : this((HttpVerb)method, endpoint, headers, (SerializationProtocol)serializationProtocol, retryData)
        {
        }

        public HttpObserver(
            HttpVerb method,
            Uri endpoint,
            Tuple<string, string>[] headers,
            SerializationProtocol serializationProtocol,
            RetryData retryData)
        {
            if (!method.AsString(out _method))
            {
                throw new ArgumentException("Not an expected HTTP method: " + method, nameof(method));
            }

            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _headers = headers ?? EmptyHeaders;

            switch (serializationProtocol)
            {
                case SerializationProtocol.Default:
                case SerializationProtocol.ToString:
                    break;
                default:
                    throw new ArgumentException("The serialization protocol specified is not supported: " + serializationProtocol, nameof(serializationProtocol));
            }

            _serializationProtocol = serializationProtocol;

            if (retryData == null)
            {
                throw new ArgumentNullException(nameof(retryData));
            }

            if (retryData.Variant is < ((int)Variant.None) or > ((int)Variant.Progressive))
            {
                throw new ArgumentOutOfRangeException(nameof(retryData), retryData.Variant, "The retry variant does not correspond to a known retry policy.");
            }

            _retryData = retryData;
        }

        public override void SetContext(IOperatorContext context)
        {
            // NB: Legacy Reactor deployments used a TraceSource.
            //_tracer = context.TraceSource;
            _subscriptionId = context.InstanceId;
            base.SetContext(context);
        }

        protected override void OnCompletedCore() => SendNotification(NotificationKind.OnCompleted, string.Empty);

        protected override void OnErrorCore(Exception error) => SendNotification(NotificationKind.OnError, error.Message);

        protected override void OnNextCore(TSource value) => SendNotification(NotificationKind.OnNext, Serialize(value));

        private void SendNotification(NotificationKind kind, string body)
        {
            var notificationHeaders = MakeHeaders(kind);
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_endpoint);
                request.Method = _method;
                request.CachePolicy = NoCacheNoStoreCaching;
                request.Timeout = Timeout;

                if (_headers != null)
                {
                    AddHeaders(request, notificationHeaders);
                }

                if (request.Method.ToLower() != "get")
                {
                    using var outputStream = request.GetRequestStream();

                    var bytes = Encoding.UTF8.GetBytes(body);

                    outputStream.Write(bytes, 0, bytes.Length);
                    outputStream.Flush();
                }

                using var response = (HttpWebResponse)request.GetResponse();

                var statusCode = (int)response.StatusCode;

                if (statusCode >= 400)
                {
                    // NB: Original Reactor code supports error handling, retry policies, etc. here. Not needed for testing in isolation.
                    _ = _retryData;
                }
            }
            catch (Exception)
            {
            }
        }

        private string Serialize<TSerialize>(TSerialize value)
        {
            switch (_serializationProtocol)
            {
                case SerializationProtocol.ToString:
                    return value == null ? string.Empty : value.ToString();
                case SerializationProtocol.Default:
                    {
                        using var stream = new MemoryStream();

                        SerializationHelpers.Serialize(value, stream);

                        stream.Seek(0, SeekOrigin.Begin);

                        using var reader = new StreamReader(stream);

                        return reader.ReadToEnd();
                    }

                default:
                    throw new NotSupportedException("Serialization protocol not supported: " + _serializationProtocol);
            }
        }

        private static void AddHeaders(HttpWebRequest httpWebRequest, Tuple<string, string>[] headers)
        {
            foreach (var header in headers)
            {
                switch (header.Item1.ToLowerInvariant())
                {
                    case "accept":
                        httpWebRequest.Accept = header.Item2;
                        break;
                    case "connection":
                        httpWebRequest.Connection = header.Item2;
                        break;
                    case "content-length":
                        httpWebRequest.ContentLength = long.Parse(header.Item2);
                        break;
                    case "content-type":
                        httpWebRequest.ContentType = header.Item2;
                        break;
                    case "date":
                        httpWebRequest.Date = DateTime.Parse(header.Item2);
                        break;
                    case "expect":
                        httpWebRequest.Expect = header.Item2;
                        break;
                    case "host":
                        httpWebRequest.Host = header.Item2;
                        break;
                    case "if-modified-since":
                        httpWebRequest.IfModifiedSince = DateTime.Parse(header.Item2);
                        break;
                    case "range":
                        httpWebRequest.AddRange(long.Parse(header.Item2));
                        break;
                    case "referer":
                        httpWebRequest.Referer = header.Item2;
                        break;
                    case "transfer-encoding":
                        httpWebRequest.SendChunked = true;
                        httpWebRequest.TransferEncoding = header.Item2;
                        break;
                    case "user-agent":
                        httpWebRequest.UserAgent = header.Item2;
                        break;
                    default:
                        httpWebRequest.Headers.Add(header.Item1, header.Item2);
                        break;
                }
            }
        }

        private Tuple<string, string>[] MakeHeaders(NotificationKind kind)
        {
            Tuple<string, string>[] hs;
            var diagnostics = kind != NotificationKind.OnNext;
            hs = new Tuple<string, string>[
                    1 +                         // Notification kind
                    1 +                         // Subscription id
                    (diagnostics ? 3 : 0) +     // Reactor headers
                    _headers.Length         // User specified headers
                ];

            var idx = 0;
            switch (kind)
            {
                case NotificationKind.OnNext:
                    hs[idx++] = Tuple.Create("RIPP-Provider-NotificationKind", "OnNext");
                    break;
                case NotificationKind.OnCompleted:
                    hs[idx++] = Tuple.Create("RIPP-Provider-NotificationKind", "OnCompleted");
                    break;
                case NotificationKind.OnError:
                    hs[idx++] = Tuple.Create("RIPP-Provider-NotificationKind", "OnError");
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            hs[idx++] = Tuple.Create("RIPP-Subscription-Id", _subscriptionId.ToCanonicalString());

            if (diagnostics)
            {
                hs[idx++] = Tuple.Create("X-AIS-AuthToken", "dummy ais");
                hs[idx++] = Tuple.Create("RIPP-Service-MachineName", Environment.MachineName);
                hs[idx++] = Tuple.Create("RIPP-QueryEvaluator-Name", "remoting:/mockqe");
            }

            Array.Copy(_headers, 0, hs, idx, _headers.Length);

            return hs;
        }
    }
}
