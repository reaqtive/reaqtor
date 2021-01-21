// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    ///     A user signal containing GeoCoordinate information
    /// </summary>
    public class GeoCoordinateUserSignal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinateUserSignal"/> class.
        /// </summary>
        public GeoCoordinateUserSignal()
        {
        }

        #region Public Properties

        /// <summary>
        ///     The type.
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/type")]
        public string Type { get; set; }

        /// <summary>
        ///     The timestamp.
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        ///     The ClientRequestId.
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/clientrequestid")]
        public string ClientRequestId { get; set; }

        /// <summary>
        ///     The AgentInstanceId
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/agentinstanceid")]
        public string AgentInstanceId { get; set; }

        /// <summary>
        ///     Gets or sets the value of this signal
        /// </summary>
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/value")]
        public GeoCoordinateValue Value { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }

        #endregion
    }
}
