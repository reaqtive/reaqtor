// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Xml.Serialization;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.FlightStatus
{
    /// <summary>
    /// Data definition for a flight status event.
    /// </summary>
    [KnownType]
    [XmlRoot("FlightStatusResponse", Namespace = "http://schemas.microsoft.com/bing/bdi/generic/flightstatus/1")]
    public class FlightStatus
    {
        // TODO: Use **** Ontology for all Mapping URIs throughout this code.

        /// <summary>
        /// Gets or sets the airline code.
        /// </summary>
        [Mapping("bing://flightstatus/airlinecode")]
        [XmlElement]
        public string AirlineCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the airline.
        /// </summary>
        [Mapping("bing://flightstatus/airlinename")]
        [XmlElement]
        public string AirlineName { get; set; }

        /// <summary>
        /// Gets or sets the arrival gate.
        /// </summary>
        [Mapping("bing://flightstatus/arrivalgate")]
        [XmlElement]
        public string ArrivalGate { get; set; }

        /// <summary>
        /// Gets or sets the arrival terminal.
        /// </summary>
        [Mapping("bing://flightstatus/arrivalterminal")]
        [XmlElement]
        public string ArrivalTerminal { get; set; }

        /// <summary>
        /// Gets or sets the data freshness.
        /// </summary>
        [Mapping("bing://flightstatus/datafreshness")]
        [XmlElement]
        public string DataFreshness { get; set; }

        /// <summary>
        /// Gets or sets the departure gate.
        /// </summary>
        [Mapping("bing://flightstatus/departuregate")]
        [XmlElement]
        public string DepartureGate { get; set; }

        /// <summary>
        /// Gets or sets the departure terminal.
        /// </summary>
        [Mapping("bing://flightstatus/departureterminal")]
        [XmlElement]
        public string DepartureTerminal { get; set; }

        /// <summary>
        /// Gets or sets the airport's code.
        /// </summary>
        [Mapping("bing://flightstatus/destinationairport")]
        [XmlElement]
        public FlightStatusAirport DestinationAirport { get; set; }

        /// <summary>
        /// Gets or sets the name of the flight.
        /// </summary>
        [Mapping("bing://flightstatus/flightname")]
        [XmlElement]
        public string FlightName { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        [Mapping("bing://flightstatus/flightnumber")]
        [XmlElement]
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the segment's flight history ID.
        /// </summary>
        [Mapping("bing://flightstatus/nextsegment")]
        [XmlElement]
        public FlightStatusSegment NextSegment { get; set; }

        /// <summary>
        /// Gets or sets the on time status
        /// </summary>
        [Mapping("bing://flightstatus/ontime")]
        [XmlElement]
        public string OnTime { get; set; }

        /// <summary>
        /// Gets or sets the airport's code.
        /// </summary>
        [Mapping("bing://flightstatus/originairport")]
        [XmlElement]
        public FlightStatusAirport OriginAirport { get; set; }

        /// <summary>
        /// Gets or sets the scheduled arrival date time.
        /// </summary>
        [Mapping("bing://flightstatus/scheduledarrivaldatetime")]
        [XmlElement]
        public DateTime ScheduledArrivalDateTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduled departure date time.
        /// </summary>
        [Mapping("bing://flightstatus/scheduleddeparturedatetime")]
        [XmlElement]
        public DateTime ScheduledDepartureDateTime { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [Mapping("bing://flightstatus/statuscode")]
        [XmlElement]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [Mapping("bing://flightstatus/status")]
        [XmlElement]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Mapping("bing://flightstatus/title")]
        [XmlElement]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the updated arrival date time.
        /// </summary>
        [Mapping("bing://flightstatus/updatedarrivaldatetime")]
        [XmlElement]
        public DateTime UpdatedArrivalDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated departure date time.
        /// </summary>
        [Mapping("bing://flightstatus/updateddeparturedatetime")]
        [XmlElement]
        public DateTime UpdatedDepartureDateTime { get; set; }
    }
}
