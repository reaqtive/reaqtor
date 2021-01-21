// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using Nuqleon.DataModel;

    /// <summary>
    /// The status of this flight
    /// </summary>
    public enum FlightStatus
    {
        [Mapping("reactor://platform.bing.com/flight/statuscode/scheduled")]
        Scheduled,
        [Mapping("reactor://platform.bing.com/flight/statuscode/active")]
        Active,
        [Mapping("reactor://platform.bing.com/flight/statuscode/unknown")]
        Unknown,
        [Mapping("reactor://platform.bing.com/flight/statuscode/redirected")]
        Redirected,
        [Mapping("reactor://platform.bing.com/flight/statuscode/landed")]
        Landed,
        [Mapping("reactor://platform.bing.com/flight/statuscode/diverted")]
        Diverted,
        [Mapping("reactor://platform.bing.com/flight/statuscode/cancelled")]
        Cancelled,
        [Mapping("reactor://platform.bing.com/flight/statuscode/notoperational")]
        NotOperational,
    }
}