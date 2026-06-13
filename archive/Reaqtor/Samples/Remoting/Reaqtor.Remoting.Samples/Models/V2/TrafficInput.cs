// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using System;

    using Nuqleon.DataModel;

    public sealed class TrafficInput
    {
        [Mapping("traffic://parameters/arrivalTime")]
        public DateTimeOffset ArrivalTime { get; set; }

        [Mapping("traffic://parameters/notificationThresholdInSeconds")]
        public int NotificationThresholdInSeconds { get; set; }

        [Mapping("traffic://parameters/routeWayPoints")]
        public WayPoint[] RouteWayPoints { get; set; }
    }
}
