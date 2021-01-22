// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Specific traffic flow parameters required when doing the subscription
    /// </summary>
    public class TrafficFlowParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Notification/Renotification thresholds apply 
        /// to the delay observed over THRU or HOV lanes.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/flow/ishov")]
        public bool? IsHov { get; set; }

        /// <summary>
        /// Gets or sets a timespan in seconds that means “notify the subscriber” 
        /// if the “delay” in travel time over the subscribed route exceeds that value
        /// </summary>
        /// <example>Example: <c>50</c></example>
        /// <remarks>Must be a positive number. Zero will throw a 400 BadRequest.</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/flow/notificationthresholdinseconds")]
        public int NotificationThresholdInSeconds { get; set; }

        /// <summary>
        /// Gets or sets a timespan in seconds that means “renotify the subscriber” if:
        /// - the delay in travel time has previously exceeded “Notificationthreshold” and 
        /// - the “delay” in travel time has changed again up or down by the renotification 
        /// threshold value
        /// </summary>
        /// <example>Example: <c>50</c></example>
        /// <remarks>Must be a positive number. Zero will throw a 400 BadRequest.</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/flow/renotificationthresholdinseconds")]
        public int RenotificationThresholdInSeconds { get; set; }
    }
}
