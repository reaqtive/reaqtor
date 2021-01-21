// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Represents an airport in a Flight Status event
    /// </summary>
    public class FlightStatusAirport
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [Mapping("bing://flightstatus/airport/code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Mapping("bing://flightstatus/airport/name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset.
        /// </summary>
        [Mapping("bing://flightstatus/airport/timezoneoffset")]
        public string TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets the bing map venue id.
        /// </summary>
        [Mapping("bing://flightstatus/airport/bingmapvenueid")]
        public string BingMapVenueId { get; set; }
    }
}
