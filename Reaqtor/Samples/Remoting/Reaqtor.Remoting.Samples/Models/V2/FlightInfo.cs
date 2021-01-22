// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using System;

    using Nuqleon.DataModel;

    /// <summary>
    /// The flight status information
    /// </summary>
    public class FlightInfo
    {
        /// <summary>
        /// The airline of the flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airline")]
        public Airline Airline { get; set; }

        /// <summary>
        /// the flight number of the flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/flightnumber")]
        public string FlightNumber { get; set; }

        /// <summary>
        /// the history of the flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/flighthistoryid")]
        public int FlightHistoryId { get; set; }

        /// <summary>
        /// The status code of the flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/statuscode")]
        public FlightStatus StatusCode { get; set; }

        /// <summary>
        /// The ontime status of the flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/ontimestatus")]
        public OnTimeStatus OnTimeStatus { get; set; }

        /// <summary>
        /// The image url for this flight status
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/relativeimageurl")]
        public string RelativeImageURL { get; set; }

        /// <summary>
        /// the scheduled depature time for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/scheduleddeparture")]
        public DateTime ScheduledDeparture { get; set; }

        /// <summary>
        /// the latest updated departure time for this flight 
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/updateddeparture")]
        public DateTime UpdatedDeparture { get; set; }

        /// <summary>
        /// the scheduled arrival for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/scheduledarrival")]
        public DateTime ScheduledArrival { get; set; }

        /// <summary>
        /// the latest updated arrival time for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/updatedarrival")]
        public DateTime UpdatedArrival { get; set; }

        /// <summary>
        /// The origin airport for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/originairport")]
        public Airport OriginAirport { get; set; }

        /// <summary>
        /// the destination airport for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/destinationairport")]
        public Airport DestinationAirport { get; set; }

        /// <summary>
        /// the diverted airport for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/divertedairport")]
        public Airport DivertedAirport { get; set; }

        /// <summary>
        /// the latest updated departure gate for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/departuregate")]
        public string DepartureGate { get; set; }

        /// <summary>
        /// the latest updated departure terminal for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/departureterminal")]
        public string DepartureTerminal { get; set; }

        /// <summary>
        /// the latest arrival gate for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/arrivalgate")]
        public string ArrivalGate { get; set; }

        /// <summary>
        /// the latest arrival terminal for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/arrivalterminal")]
        public string ArrivalTerminal { get; set; }

        /// <summary>
        /// the latest baggage claim for this flight
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/baggageclaim")]
        public string BaggageClaim { get; set; }

        /// <summary>
        /// the datetime when this flight status was recorded
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/datetimerecorded")]
        public DateTime DateTimeRecorded { get; set; }

        /// <summary>
        /// the title of this flight. Usually its airline code plus flight number Ie. UA 2
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/title")]
        public string Title { get; set; }
    }
}
