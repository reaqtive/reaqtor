// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// A Route object that expresses the route duration and delay between
    /// two points.
    /// Event Source: GeoSpatial
    /// </summary>
    public class TrafficInfo
    {
        /// <summary>
        /// Gets or sets the type of this notification.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationtype")]
        public NotificationTypeEnum NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/subscriptionid")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the subscription parameters
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/subscription")]
        public TrafficParameters Subscription { get; set; }

        /// <summary>
        /// Gets or sets the UTC datetime of the feed item
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/timestamp")]
        public DateTime TimestampUTC { get; set; }

        /// <summary>
        /// Gets or sets the traffic flow information.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/flowinfo")]
        public TrafficFlowInfo FlowInfo { get; set; }

        /// <summary>
        /// Gets or sets the traffic incident information.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident")]
        public TrafficIncidentInfo IncidentInfo { get; set; }
    }

    /// <summary>
    /// The possible types of route traffic info notifications.
    /// </summary>
    public enum NotificationTypeEnum
    {
        /// <summary>
        /// The notification is due to a change in the traffic flow.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationtype/flow")]
        Flow = 0,

        /// <summary>
        /// The notification is due to traffic incident flow.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationtype/incident")]
        Incident = 1,

        /// <summary>
        /// Notification status report.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationtype/statusreport")]
        StatusReport = 2
    }
}
