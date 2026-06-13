// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// Traffic parameters
    /// </summary>
    [KnownType]
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
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/alternatevia")]
        public string AlternateVia { get; set; }

        /// <summary>
        /// Gets or sets the route's end address. It can be a lat/long
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>monroe wa</c></example>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/endaddress")]
        public string EndAddress { get; set; }

        /// <summary>
        /// Gets or sets a date when to stop monitoring the route. If null, the subscription will
        /// last for one year.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/endtime")]
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
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/routeid")]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the route's start address. It can be a lat/long
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>everett wa</c></example>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/startaddress")]
        public string StartAddress { get; set; }

        /// <summary>
        /// Gets or sets a date when to start monitoring the route. If null, the Now time is used.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/starttime")]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the culture info of the client (referring to StartAddress/EndAddress)
        /// </summary>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/cultureinfo")]
        public string CultureInfo { get; set; }
        #endregion

        #region Input Parameters Specific To Flow
        /// <summary>
        /// Gets or sets specific parameters required for flow subscriptions
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping(TrafficConstants.TrafficParametersMappingUriPrefix + "/flow")]
        public TrafficFlowParameters FlowParameters { get; set; }
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.AppendFormat("AlternateVia:{0},", AlternateVia);
            sb.AppendFormat("CultureInfo:{0},", CultureInfo);
            sb.AppendFormat("EndAddress:{0},", EndAddress);
            sb.AppendFormat("EndTime:{0},", EndTime);
            sb.AppendFormat("FlowParameters:{0},", FlowParameters);
            sb.AppendFormat("RouteId:{0},", RouteId);
            sb.AppendFormat("StartAddress:{0},", StartAddress);
            sb.AppendFormat("StartTime:{0},", StartTime);
            sb.Append(']');

            return sb.ToString();
        }
    }
}
