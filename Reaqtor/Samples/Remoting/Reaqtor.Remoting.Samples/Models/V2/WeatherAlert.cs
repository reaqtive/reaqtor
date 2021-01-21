// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using System;
    using Nuqleon.DataModel;

    /// <summary>
    /// Data contract for weather alert data provider (push mechanism)
    /// </summary>
    public class WeatherAlert
    {
        /// <summary>
        /// Gets or sets the Alert id
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Alert title
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Alert start time
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/starttime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the Alert end time
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/endtime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the Alert type
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the tile id for which alert is valid
        /// </summary>
        [Mapping("reactor://platform.bing.com/weather/alert/tileid")]
        public string TileId { get; set; }
    }
}
