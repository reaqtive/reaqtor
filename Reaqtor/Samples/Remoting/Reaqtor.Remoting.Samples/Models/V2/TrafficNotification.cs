// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using System;
    using Nuqleon.DataModel;

    public sealed class TrafficNotification
    {
        [Mapping("traffic://notification/timestamp")]
        public DateTime TimeStamp { get; set; }

        [Mapping("traffic://notification/subscriptionId")]
        public string SubscriptionId { get; set; }

        [Mapping("traffic://notification/travelDurationWithTraffic")]
        public TimeSpan TravelDurationWithTraffic { get; set; }

        [Mapping("traffic://notification/arriveAt")]
        public DateTimeOffset ArriveAt { get; set; }

        [Mapping("traffic://notification/notificationThreshold")]
        public TimeSpan NotificationThreshold { get; set; }

        [Mapping("traffic://notification/routeId")]
        public string RouteId { get; set; }
    }
}
