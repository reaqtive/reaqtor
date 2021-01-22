// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Data definition for a flight status event
    /// </summary>
    public class FlightStatus
    {
        // TODO: Use Satori Ontology for all Mapping URIs throughout this code.

        /// <summary>
        /// Gets or sets the airline code.
        /// </summary>
        [Mapping("bing://flightstatus/airlinecode")]
        public string AirlineCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the airline.
        /// </summary>
        [Mapping("bing://flightstatus/airlinename")]
        public string AirlineName { get; set; }

        /// <summary>
        /// Gets or sets the arrival gate.
        /// </summary>
        [Mapping("bing://flightstatus/arrivalgate")]
        public string ArrivalGate { get; set; }

        /// <summary>
        /// Gets or sets the arrival terminal.
        /// </summary>
        [Mapping("bing://flightstatus/arrivalterminal")]
        public string ArrivalTerminal { get; set; }

        /// <summary>
        /// Gets or sets the data freshness.
        /// </summary>
        [Mapping("bing://flightstatus/datafreshness")]
        public string DataFreshness { get; set; }

        /// <summary>
        /// Gets or sets the departure gate.
        /// </summary>
        [Mapping("bing://flightstatus/departuregate")]
        public string DepartureGate { get; set; }

        /// <summary>
        /// Gets or sets the departure terminal.
        /// </summary>
        [Mapping("bing://flightstatus/departureterminal")]
        public string DepartureTerminal { get; set; }

        /// <summary>
        /// Gets or sets the airport's code.
        /// </summary>
        [Mapping("bing://flightstatus/destinationairport")]
        public FlightStatusAirport DestinationAirport { get; set; }

        /// <summary>
        /// Gets or sets the name of the flight.
        /// </summary>
        [Mapping("bing://flightstatus/flightname")]
        public string FlightName { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        [Mapping("bing://flightstatus/flightnumber")]
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the segment's flight history ID.
        /// </summary>
        [Mapping("bing://flightstatus/nextsegment")]
        public FlightStatusSegment NextSegment { get; set; }

        /// <summary>
        /// Gets or sets the on time status
        /// </summary>
        [Mapping("bing://flightstatus/ontime")]
        public string OnTime { get; set; }

        /// <summary>
        /// Gets or sets the airport's code.
        /// </summary>
        [Mapping("bing://flightstatus/originairport")]
        public FlightStatusAirport OriginAirport { get; set; }

        /// <summary>
        /// Gets or sets the scheduled arrival date time.
        /// </summary>
        [Mapping("bing://flightstatus/scheduledarrivaldatetime")]
        public DateTime ScheduledArrivalDateTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduled departure date time.
        /// </summary>
        [Mapping("bing://flightstatus/scheduleddeparturedatetime")]
        public DateTime ScheduledDepartureDateTime { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [Mapping("bing://flightstatus/statuscode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [Mapping("bing://flightstatus/status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Mapping("bing://flightstatus/title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the updated arrival date time.
        /// </summary>
        [Mapping("bing://flightstatus/updatedarrivaldatetime")]
        public DateTime UpdatedArrivalDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated departure date time.
        /// </summary>
        [Mapping("bing://flightstatus/updateddeparturedatetime")]
        public DateTime UpdatedDepartureDateTime { get; set; }
    }
}
