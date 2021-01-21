// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;

    using Nuqleon.DataModel;

    /// <summary>
    /// Data definition for a weather alert event
    /// </summary>
    public class WeatherAlert
    {
        /// <summary>
        /// Gets or sets the event ID
        /// </summary>
        [Mapping("bing://weatheralert/eventid")]
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the title for the weather alert event
        /// </summary>
        [Mapping("mso:meteorology.weather_forecast.alert")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the weather alert event
        /// </summary>
        [Mapping("mso:meteorology.weather_forecast.start_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the weather alert event
        /// </summary>
        [Mapping("mso:meteorology.weather_forecast.end_time")]
        public DateTime ExpirationTime { get; set; }
    }
}
