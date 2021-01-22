// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Reactor.DomainFeeds
{
    /// <summary>
    /// Maintains the registry with all URIs that are used to Define the IRP Observables/Observers/Streams
    /// </summary>
    public static class MetadataRegistry
    {
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
                    /// <summary>Resource identifier for the real Traffic Info Observable as a Uri.</summary>
                    public static readonly Uri Uri = new(String);

                    /// <summary>Resource identifier for the configurable Traffic Info Observable as a Uri.</summary>
                    public static readonly Uri ConfigurableUri = new(ConfigurableString);

                    /// <summary>Resource identifier for the real Traffic Info Observable as a String.</summary>
                    private const string String = "bing://platform.bing.com/observable/real/trafficinfo";

                    /// <summary>Resource identifier for the configurable Traffic Info Observable as a String.</summary>
                    private const string ConfigurableString = "bing://platform.bing.com/observable/real/trafficinfo/generic";
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
    }
}
