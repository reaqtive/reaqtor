// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    /// <summary>
    /// Data definition for the parameters to the Flight Status parameterized Observable. Clients must specify either the FlightName,
    /// or the FlightNumber + one of either AirlineCode or AirlineName.
    /// </summary>
    public class FlightStatusParameters
    {
        /// <summary>
        /// Gets or sets the name of the flight, which is typically the AirlineCode+FlightNumber, e.g. "aa192"
        /// </summary>
        [Mapping("bing://flightstatusparameters/flightname")]
        public string FlightName { get; set; }

        /// <summary>
        /// Gets or sets the flight number, e.g. "192"
        /// </summary>
        [Mapping("bing://flightstatusparameters/flightnumber")]
        public string FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the airline, e.g. "American Airlines"
        /// </summary>
        [Mapping("bing://flightstatusparameters/airline/name")]
        public string AirlineName { get; set; }

        /// <summary>
        /// Gets or sets the airline code, e.g. "AA" = "American Airlines"
        /// </summary>
        [Mapping("bing://flightstatusparameters/airlinecode")]
        public string AirlineCode { get; set; }

        /// <summary>
        /// Gets or sets the market, e.g. "zh-CN"
        /// </summary>
        [Mapping("bing://flightstatusparameters/market")]
        public string Market { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// Note that the object won't be serialized using this method.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("AirlineCode: {0}, AirlineName: {1}, FlightName: {2}, FlightNumber: {3}", AirlineCode, AirlineName, FlightName, FlightNumber);
        }
    }
}
