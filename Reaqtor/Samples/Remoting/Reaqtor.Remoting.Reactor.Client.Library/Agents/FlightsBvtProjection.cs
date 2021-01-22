// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Type of entity for the the Flight BVT subscription.
    /// </summary>
    public class FlightsBvtProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsBvtProjection"/> class.
        /// </summary>
        public FlightsBvtProjection()
        {
        }

        /// <summary>
        /// Gets or sets the airline code.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/AirlineCode")]
        public string AirlineCode { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/FlightNumber")]
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the scheduled departure date time.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/ScheduledDepartureDateTime")]
        public DateTime ScheduledDepartureDateTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduled arrival date time.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/ScheduledArrivalDateTime")]
        public DateTime ScheduledArrivalDateTime { get; set; }

        /// <summary>
        /// Gets or sets the origin airport code.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/OriginAirportCode")]
        public string OriginAirportCode { get; set; }

        /// <summary>
        /// Gets or sets the destination airport code.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/DestinationAirportCode")]
        public string DestinationAirportCode { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/StatusCode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the departure delay in minutes.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/DepartureDelayInMinutes")]
        public double DepartureDelayInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the arrival delay in minutes.
        /// </summary>
        [Mapping("reactor://platform.bing.com/BVT/AgentsM2/Flight/ArrivalDelayInMinutes")]
        public double ArrivalDelayInMinutes { get; set; }
    }
}
