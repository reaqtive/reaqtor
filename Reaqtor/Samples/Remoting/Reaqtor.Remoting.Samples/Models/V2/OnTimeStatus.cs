// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using Nuqleon.DataModel;

    /// <summary>
    /// Enum that describe the status of the flight
    /// </summary>
    public enum OnTimeStatus
    {
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/undefined")]
        UnDefined = 0,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/scheduledontime")]
        ScheduledOnTime = 1,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/scheduledearly")]
        ScheduledEarly = 2,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/scheduledlate")]
        ScheduledLate = 3,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/enrouteontime")]
        EnRouteOnTime = 4,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/enrouteearly")]
        EnRouteEarly = 5,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/enroutelate")]
        EnRouteLate = 6,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/landedontime")]
        LandedOnTime = 7,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/landedearly")]
        LandedEarly = 8,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/landedlate")]
        LandedLate = 9,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/scheduledcanceled")]
        ScheduledCanceled = 10,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/landedrerouted")]
        LandedRerouted = 11,
        [Mapping("reactor://platform.bing.com/flight/ontimestatus/enroutererouted")]
        EnRouteRerouted = 12,
    }
}
