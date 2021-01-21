// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// The possible types of route traffic info notifications.
    /// </summary>
    public enum NotificationTypeEnum
    {
        /// <summary>
        /// The notification is due to a change in the traffic flow.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/notificationtype/flow")]
        Flow = 0,

        /// <summary>
        /// The notification is due to traffic incident flow.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/notificationtype/incident")]
        Incident = 1,

        /// <summary>
        /// Notification status report.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/notificationtype/statusreport")]
        StatusReport = 2
    }

    /// <summary>
    /// A Route object that expresses the route duration and delay between
    /// two points.
    /// Event Source: GeoSpatial
    /// </summary>
    [KnownType]
    public class TrafficInfo
    {
        /// <summary>
        /// Gets or sets the subscription parameters
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/subscription")]
        public TrafficParameters Subscription { get; set; }

        /// <summary>
        /// Gets or sets the type of this notification.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/notificationtype")]
        public NotificationTypeEnum NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/subscriptionid")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the UTC datetime of the feed item
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/timestamp")]
        public DateTime TimestampUTC { get; set; }

        /// <summary>
        /// Gets or sets the traffic flow information.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/flowinfo")]
        public TrafficFlowInfo FlowInfo { get; set; }

        /// <summary>
        /// Gets or sets the traffic incident information.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident")]
        public TrafficIncidentInfo IncidentInfo { get; set; }

        /// <summary>
        /// Gets or sets the traffic status information.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/status")]
        public TrafficStatusInfo StatusInfo { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.AppendFormat("NotificationType:{0},", NotificationType);
            sb.AppendFormat("Subscription:{0},", Subscription);
            sb.AppendFormat("CultureInfo:{0},", SubscriptionId);
            sb.AppendFormat("TimestampUTC:{0},", TimestampUTC);
            sb.AppendFormat("IncidentInfo:{0},", IncidentInfo);
            sb.AppendFormat("FlowInfo:{0},", FlowInfo);
            sb.AppendFormat("StatusInfo:{0},", StatusInfo);
            sb.Append(']');

            return sb.ToString();
        }
    }
}
