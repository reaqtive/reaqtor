// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Traffic flow information
    /// </summary>
    public class TrafficFlowInfo
    {
        /// <summary>
        /// Gets or sets the total number of seconds of the observed
        /// delay along the entire route.
        /// </summary>
        /// <example>Example: <c>1073</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/flow/delayinseconds")]
        public long DelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the total number of seconds of the observed
        /// delay along the entire route in the HOV lane.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/flow/hovdelayinseconds")]
        public int? HovDelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the route's duration. The route duration is
        /// the route duration with no traffic. It is expressed in seconds.
        /// </summary>
        /// <example>Example: <c>7902.0</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/flow/freeflowtraveldurationinseconds")]
        public int FreeFlowTravelDurationInSeconds { get; set; }
    }
}
