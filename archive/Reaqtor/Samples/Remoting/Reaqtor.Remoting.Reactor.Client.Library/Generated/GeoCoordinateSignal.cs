// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    ///     <para>Defines a user device geocoordinate signal, based on the data returned by the User Device Signal Gateway.</para>
    ///     <para>Note that while the User Device Signal Gateway spec as of 2/19/2013 states that they will send an array of different user signals,
    ///     in practice we've agreed that the gateway will send only one signal type at a time. For now, only Geocoordinate is supported. This
    ///     Entity type assumes that just one Signal is being sent and that its "Type" value is
    ///     "com.bing.merino.device.geolocation.geocoordinate".</para>
    /// </summary>
    public class GeoCoordinateSignal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinateSignal"/> class.
        /// </summary>
        public GeoCoordinateSignal()
        {
        }

        #region Public Properties

        /// <summary>
        ///     Gets or sets the ID of the request (internal to the feed)
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/requestid")]
        public Guid RequestId { get; set; }

        /// <summary>
        ///     Gets or sets the ID of the user generating the signal
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/userid")]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the ID of the device generating the signal
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/deviceid")]
        public string DeviceId { get; set; }

        /// <summary>
        ///     Gets or sets the structure containing the GeoCoordinate information for this signal
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/usersignal")]
        public GeoCoordinateUserSignal UserSignal { get; set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("RequestId={0} UserId={1} DeviceId={2} UserSignal={3}", RequestId, UserId, DeviceId, UserSignal);
        }

        #endregion
    }
}
