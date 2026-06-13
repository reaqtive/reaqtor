// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Reactor.Client
{
    public static class Constants
    {
        /// <summary>Resource identifiers for the various data streams.</summary>
        public static class Stream
        {
            /// <summary>Resource identifiers for the various real data streams.</summary>
            public static class Real
            {
                /// <summary>Resource identifier for the real Diagnostic data stream.</summary>
                public static class DiagnosticData
                {
                    /// <summary>Resource identifier for the Diagnostic data stream as a String.</summary>
                    public const string String = "reactor://platform.bing.com/streams/real/diagnosticdata";

                    /// <summary>Resource identifier for the Diagnostic data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>Resource identifier for the real Geo Coordinate Signal data stream.</summary>
                public static class GeoCoordinateSignal
                {
                    /// <summary>Resource identifier for the real Geo Coordinate Signal data stream as a String.</summary>
                    public const string String = "reactor://platform.bing.com/streams/real/geocoordinatesignal";

                    /// <summary>Resource identifier for the real Geo Coordinate Signal data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>Resource identifier for the real Time data stream.</summary>
                public static class Time
                {
                    /// <summary>Resource identifier for the real Time data stream as a String.</summary>
                    public const string String = "reactor://platform.bing.com/streams/real/time";

                    /// <summary>Resource identifier for the real Time data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>Resource identifier for the real Traffic Info data stream.</summary>
                public static class TrafficInfo
                {
                    /// <summary>Resource identifier for the real Traffic Info data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/streams/real/trafficinfo";

                    /// <summary>Resource identifier for the real Traffic Info data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }
        }

        /// <summary>
        /// Resource identifiers for various observables defined in the system
        /// </summary>
        public static class Observable
        {
            /// <summary>
            /// Synthetic observables (ones that push mocked/synthetic data)
            /// </summary>
            public static class Synthetic
            {
                /// <summary>
                /// Resource identifier for the mock Flight Status data stream.
                /// </summary>
                public static class FlightStatus
                {
                    /// <summary>Resource identifier for the mock Flight Status data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/synthetic/flightstatus";

                    /// <summary>Resource identifier for the mock Flight Status data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// News feed
                /// </summary>
                public static class NewsInfo
                {
                    /// <summary>Resource identifier for the mock News Info data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/synthetic/newsinfo";

                    /// <summary>Resource identifier for the mock News Info data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// Traffic feed
                /// </summary>
                public static class TrafficInfo
                {
                    /// <summary>Resource identifier for the synthetic Traffic Info observable as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/synthetic/trafficinfo";

                    /// <summary>Resource identifier for the synthetic Traffic Info observable as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>Resource identifier for the real Weather Alert data stream.</summary>
                public static class WeatherAlert
                {
                    /// <summary>Resource identifier for the real Weather Alert data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/synthetic/weatheralert";

                    /// <summary>Resource identifier for the real Weather Alert data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }

            /// <summary>
            /// Real observables (the ones that push real data from the feeds)
            /// </summary>
            public static class Real
            {
                /// <summary>
                /// Resource identifier for the real Flight Status data stream.
                /// </summary>
                public static class FlightStatus
                {
                    /// <summary>Resource identifier for the real Flight Status data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/real/flightstatus";

                    /// <summary>Resource identifier for the real Flight Status data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// News feed
                /// </summary>
                public static class NewsInfo
                {
                    /// <summary>Resource identifier for the mock News Info data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/real/newsinfo";

                    /// <summary>Resource identifier for the mock News Info data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// Traffic feed
                /// </summary>
                public static class TrafficInfo
                {
                    /// <summary>Resource identifier for the real Traffic Info Observable as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/real/trafficinfo";

                    /// <summary>Resource identifier for the real Traffic Info Observable as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// Resource identifier for the real Weather Alert data stream.
                /// </summary>
                public static class WeatherAlert
                {
                    /// <summary>Resource identifier for the real Weather Alert data stream as a String.</summary>
                    public const string String = "bing://platform.bing.com/observable/real/weatheralert";

                    /// <summary>Resource identifier for the real Weather Alert data stream as a Uri.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }
        }

        /// <summary>
        /// Resource identifiers for various observers defined in the system
        /// </summary>
        public static class Observer
        {
            /// <summary>
            /// Resource identifiers for Actions. These are shared between the
            /// client and server to uniquely identify an action. They will
            /// ultimately be URIs into the knowledge store graph where they
            /// will be described, e.g. which parameters they take.
            /// </summary>
            public static class Action
            {
                /// <summary>The Http action.</summary>
                public static class Http
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/action/http";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The Http Post action.</summary>
                public static class HttpPost
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/action/http-post";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The notification hub action.</summary>
                public static class NotificationHub
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/action/notification-hub";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The Trace action.</summary>
                public static class Trace
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/action/trace";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The ZMQ Publish action.</summary>
                public static class ZmqPublish
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/action/zmq-publish";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }

            /// <summary>
            /// Resource identifiers for observers to be called on
            /// subscription completion.
            /// </summary>
            public static class Final
            {
                /// <summary>
                /// Resource identifier for HTTP observer to be called on
                /// subscription completion.
                /// </summary>
                public static class Http
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/final/http";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// Resource identifiers for HTTP POST observers to be called
                /// on subscription completion.
                /// </summary>
                public static class HttpPost
                {
                    /// <summary>
                    /// Resource identifier for the http post observer to be called
                    /// on subscription completion that takes no headers.
                    /// </summary>
                    public const string NoHeaders = "reactor://platform.bing.com/observer/final/httppost/noheaders";

                    /// <summary>
                    /// Resource identifier for the http post observer to be called
                    /// on subscription completion that takes headers.
                    /// </summary>
                    public const string WithHeaders = "reactor://platform.bing.com/observer/final/httppost/withheaders";
                }
            }
        }

        /// <summary>
        /// Other identifiers.
        /// </summary>
        public static class Identifiers
        {
            /// <summary>
            /// Observable identifiers.
            /// </summary>
            public static class Observable
            {
                /// <summary>
                /// The person observable.
                /// </summary>
                public static class Person
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/person";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// The range observable.
                /// </summary>
                public static class Range
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/range";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }
        }
    }
}
