// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// The possible types of route status.
    /// </summary>
    public enum StatusCodeEnum
    {
        /// <summary>The route status is being monitored.</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statuscode/active")]
        Active = 0,

        /// <summary>
        /// The route status is waiting for the monitoring start time.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statuscode/waiting")]
        Waiting = 1,

        /// <summary>
        /// Monitoring end time has already passed or unsubscribe request 
        /// is received.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statuscode/completed")]
        Completed = 2,

        /// <summary>
        /// Check status details for reasons it was dropped - StatusDetails 
        /// started with the prefix Error.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statuscode/dropped")]
        Dropped = 3,

        /// <summary>
        /// Check status details for warnings - StatusDetails started with 
        /// the prefix Warning.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statuscode/activewithwarnings")]
        ActiveWithWarnings = 4
    }

    /// <summary>
    /// The possible types of route status details.
    /// </summary>
    public enum StatusDetailCodeEnum
    {
        /// <summary>
        /// Not defined for status active, completed, waiting.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statusdetailcode/notavailable")]
        NotAvailable = 0,

        /// <summary>Possible value for Dropped status</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statusdetailcode/externalinternalservererror")]
        ErrorInternalServerError = 1,

        /// <summary>
        /// Possible value for Dropped status: the route has changed due 
        /// to changes in the underlying road network.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statusdetailcode/errorroutechange")]
        ErrorRouteChange = 2,

        /// <summary>
        /// Possible value for ActiveWithWarning status: no traffic flow 
        /// info is being received in the region as of now - traffic info 
        /// is expected to resume later.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statusdetailcode/warningtemporarilynoflowinfo")]
        WarningTemporarilyNoFlowInfo = 3,

        /// <summary>
        /// Communication error - no listener to the output feed.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/statusdetailcode/warningunabletoreachlistener")]
        WarningUnableToReachListener = 4
    }

    /// <summary>
    /// Traffic Status information
    /// </summary>
    [KnownType]
    public class TrafficStatusInfo
    {
        /// <summary>Gets or sets the Status notification code</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/status/code")]
        public StatusCodeEnum Code { get; set; }

        /// <summary>Gets or sets the Status notification detail code</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/status/details")]
        public StatusDetailCodeEnum Details { get; set; }

        /// <summary>Gets or sets the Description of the status</summary>
        /// <example>Example: <c>accident</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/status/message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("[Code:{0}, Details:{1}, Message:{2}]", Code, Details, Message);
        }
    }
}
