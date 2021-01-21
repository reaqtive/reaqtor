// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    ///     The data inside a GeoCoordinateUserSignal
    /// </summary>
    public class GeoCoordinateValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinateValue"/> class.
        /// </summary>
        public GeoCoordinateValue()
        {
        }

        #region Public Properties

        /// <summary>
        /// The status of the GeoCoordinate information. Valid values include Valid, Disabled, and NoData, per the spec.
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/status")]
        public string Status { get; set; }

        /// <summary>
        /// The accuracy reading of the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/accuracy")]
        public string Accuracy { get; set; }

        /// <summary>
        /// The altitude as given by GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/altitude")]
        public string Altitude { get; set; }

        /// <summary>
        /// The accuracy of the altitude as given by the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/altitudeaccuracy")]
        public string AltitudeAccuracy { get; set; }

        /// <summary>
        /// The heading read by the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/heading")]
        public string Heading { get; set; }

        /// <summary>
        /// The latitude read by the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude read by the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// The source information provided by the GPS. Valid values include Cellular, Satellite, Wi-Fi, and Unknown, per the spec.
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/positionsource")]
        public string PositionSource { get; set; }

        /// <summary>
        /// The speed read by the GPS
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/speed")]
        public string Speed { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Status={0}", Status);
        }
        #endregion
    }
}
