// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Weather
{
    /// <summary>
    /// Data definition for the parameters to the weather alert parameterized Observable.
    /// </summary>
    [KnownType]
    public class WeatherAlertParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherAlertParameters"/> class.
        /// </summary>
        public WeatherAlertParameters()
        {
        }

        /// <summary>
        /// Gets or sets the latitude for the weather alert
        /// </summary>
        [Mapping("bing://weatheralert/latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude for the weather alert
        /// </summary>
        [Mapping("bing://weatheralert/longitude")]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the zip code for the weather alert
        /// </summary>
        [Mapping("bing://weatheralert/zipcode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city for the weather alert
        /// </summary>
        [Mapping("bing://weatheralert/city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the market that the user is in. Different market may have different data source.
        /// </summary>
        [Mapping("bing://weatheralert/market")]
        public string Market { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Latitude: {0}, Longitude: {1}, ZipCode: {2}, City: {3}, Market: {4}", Latitude, Longitude, ZipCode, City, Market);
        }
    }
}
