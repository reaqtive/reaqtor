// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Xml.Serialization;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.FlightStatus
{
    /// <summary>
    /// Represents a segment in a Flight Status event
    /// </summary>
    [KnownType]
    public class FlightStatusSegment
    {
        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        [Mapping("bing://flightstatus/segment/destinationairport")]
        [XmlElement]
        public string DestinationAirport { get; set; }

        /// <summary>
        /// Gets or sets the flight history id.
        /// </summary>
        [Mapping("bing://flightstatus/segment/flighthistoryid")]
        [XmlElement("FlightHistoryID")]
        public string FlightHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        [Mapping("bing://flightstatus/segment/originairport")]
        [XmlElement]
        public string OriginAirport { get; set; }
    }
}
