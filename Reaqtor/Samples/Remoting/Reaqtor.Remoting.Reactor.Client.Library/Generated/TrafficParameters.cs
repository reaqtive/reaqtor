// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Traffic parameters
    /// </summary>
    public class TrafficParameters
    {
        #region Input Parameters Common To Both Flow And Incident
        /// <summary>
        /// Gets or sets the route's alternate via. It can be a lat/long 
        /// comma-delimited pair. Used as a point along the route to 
        /// differentiate between multiple routes.
        /// </summary>
        /// <example>Example: <c>47.9603,-122.12193</c>. SA1631 makes me add 
        /// enough text to go below its punctuation density threshold.</example>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/alternatevia")]
        public string AlternateVia { get; set; }

        /// <summary>
        /// Gets or sets the route's end address. It can be a lat/long 
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>monroe wa</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/endaddress")]
        public string EndAddress { get; set; }

        /// <summary>
        /// Gets or sets a date when to stop monitoring the route. If null, the subscription will 
        /// last for one year.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/endtime")]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the route id. Subscribe to the route by its id. Once you negotiate with the 
        /// routing service you get a routeid that describes the exact route.
        /// This can be used instead of the StartAddress/EndAddress.
        /// </summary>
        /// <remarks>
        /// The routeId should take the value returned in “id” top-level JSON 
        /// field described at <see cref="http://msdn.microsoft.com/en-us/library/ff701718.aspx"/>
        /// for Bing Maps Route Services.  
        /// </remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/routeid")]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the route's start address. It can be a lat/long 
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>everett wa</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/startaddress")]
        public string StartAddress { get; set; }

        /// <summary>
        /// Gets or sets a date when to start monitoring the route. If null, the Now time is used.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/starttime")]
        public DateTimeOffset? StartTime { get; set; }
        #endregion

        #region Input Parameters Specific To Flow
        /// <summary>
        /// Gets or sets specific parameters required for flow subscriptions
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficparameters/flow")]
        public TrafficFlowParameters FlowParameters { get; set; }
        #endregion
    }
}
