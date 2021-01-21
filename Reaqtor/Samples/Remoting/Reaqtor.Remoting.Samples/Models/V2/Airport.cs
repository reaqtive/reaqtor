// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using Nuqleon.DataModel;

    /// <summary>
    /// The airport data for flight status
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// the airport code for this airport
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airport/code")]
        public string AirportCode { get; set; }

        /// <summary>
        /// The localized name for this airport
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airport/name")]
        public string Name { get; set; }

        /// <summary>
        /// The localized city name for this airport
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airport/cityname")]
        public string CityName { get; set; }

        /// <summary>
        /// The bing maps venue id
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airport/bingmapsvenueid")]
        public string BingMapsVenueId { get; set; }

        /// <summary>
        /// The timezone offset for this airport
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airport/timezoneoffset")]
        public int TimeZoneOffset { get; set; }
    }
}
