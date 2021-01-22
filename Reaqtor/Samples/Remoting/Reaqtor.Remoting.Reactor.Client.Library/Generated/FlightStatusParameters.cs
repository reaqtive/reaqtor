// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Data definition for the parameters to the Flight Status parameterized Observable. Clients must specify either the FlightName,
    /// or the FlightNumber + one of either AirlineCode or AirlineName.
    /// </summary>
    public class FlightStatusParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightStatusParameters"/> class.
        /// </summary>
        public FlightStatusParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightStatusParameters" /> class.
        /// TODO: make constructor parameters optional. introduce specialized observables per flight lookup type.
        /// </summary>
        /// <param name="flightName">The name of the flight being tracked, e.g. "aa112". If this is specified then no other parameters need to be provided.</param>
        /// <param name="flightNumber">The number of the flight being tracked, e.g. "112". If this is specified without flightName, then one of either airlineName or
        /// airlineCode must be provided.</param>
        /// <param name="airlineName">The name of the airline hosting the flight being tracked, e.g. "united airlines". Only needs to be specified with flightNumber
        /// when flightName and airlineCode are not specified.</param>
        /// <param name="airlineCode">The code for the airline hosting the flight being tracked, e.g. "ua". Only needs to be specified with flightNumber when flightName
        /// and airlineCode are not specified.</param>
        public FlightStatusParameters(
            [Mapping("bing://flightstatusparameters/flightname")] string flightName,
            [Mapping("bing://flightstatusparameters/flightnumber")] string flightNumber,
            [Mapping("bing://flightstatusparameters/airline/name")] string airlineName,
            [Mapping("bing://flightstatusparameters/airlinecode")] string airlineCode)
        {
            FlightName = flightName;
            FlightNumber = flightNumber;
            AirlineName = airlineName;
            AirlineCode = airlineCode;
        }

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
