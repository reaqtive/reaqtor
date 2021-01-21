// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// data model used to deserialize the payload received via ZeroMQ from the TaRN service
    /// </summary>
    public class ZmqTrafficPayload
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
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/alternatevia")]
        public string AlternateVia { get; set; }

        /// <summary>
        /// Gets or sets the route's end address. It can be a lat/long
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>monroe wa</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/endaddress")]
        public string EndAddress { get; set; }

        /// <summary>
        /// Gets or sets when to stop monitoring the route. If null, the subscription will
        /// last for one year.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/endtime")]
        public DateTimeOffset EndTime { get; set; }

        /// <summary>
        /// Gets or sets the route Id. Subscribe to the route by its id. Once you negotiate with the
        /// routing service you get a routeid that describes the exact route.
        /// This can be used instead of the StartAddress/EndAddress.
        /// </summary>
        /// <remarks>
        /// The routeId should take the value returned in “id” top-level JSON
        /// field described at <see cref="http://msdn.microsoft.com/en-us/library/ff701718.aspx"/>
        /// for Bing Maps Route Services.
        /// </remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/routeid")]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the route's start address. It can be a lat/long
        /// comma-delimited pair or an address or a landmark.
        /// </summary>
        /// <example>Example: <c>everett wa</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/startaddress")]
        public string StartAddress { get; set; }

        /// <summary>
        /// Gets or sets when to start monitoring the route. If null, the Now time is used.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/starttime")]
        public DateTimeOffset StartTime { get; set; }

        #endregion

        #region Input Parameters Specific To Flow

        /// <summary>
        /// Gets or sets a value indicating whether the Notification/Renotification thresholds apply
        /// to the delay observed over THRU or HOV lanes.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/ishov")]
        public bool IsHov { get; set; }

        /// <summary>
        /// Gets or sets a timespan in seconds that means “notify the subscriber”
        /// if the “delay” in travel time over the subscribed route exceeds that value
        /// </summary>
        /// <example>Example: <c>50</c></example>
        /// <remarks>Must be a positive number. Zero will throw a 400 BadRequest.</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationthresholdinseconds")]
        public int NotificationThresholdInSeconds { get; set; }

        /// <summary>
        /// Gets or sets a timespan in seconds that means “renotify the subscriber” if:
        /// - the delay in travel time has previously exceeded “Notificationthreshold” and
        /// - the “delay” in travel time has changed again up or down by the renotification
        /// threshold value
        /// </summary>
        /// <example>Example: <c>50</c></example>
        /// <remarks>Must be a positive number. Zero will throw a 400 BadRequest.</remarks>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/renotificationthresholdinseconds")]
        public int RenotificationThresholdInSeconds { get; set; }

        #endregion

        #region Output Commmon To Both Flow And Incident

        /// <summary>
        /// Gets or sets the type of this notification.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/notificationtype")]
        public int NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/subscriptionid")]
        public string SubscriptionId { get; set; }

        #endregion

        #region Output for Flow

        /// <summary>
        /// Gets or sets the total number of seconds of the observed
        /// delay along the entire route.
        /// </summary>
        /// <example>Example: <c>1073</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/delayinseconds")]
        public long DelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the total number of seconds of the observed
        /// delay along the entire route in the HOV lane.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/hovdelayinseconds")]
        public int HovDelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the route's duration. The route duration is
        /// the route duration with no traffic. It is expressed in seconds.
        /// </summary>
        /// <example>Example: <c>7902.0</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/freeflowtraveldurationinseconds")]
        public int FreeFlowTravelDurationInSeconds { get; set; }

        #endregion

        #region Output for Incident

        /// <summary>Gets or sets a description of the congestion.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>generally slow</description></item>
        ///     <item><description>sluggish</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/congestioninfo")]
        public string CongestionInfo { get; set; }

        /// <summary>Gets or sets a description of the incident.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>W 95th St between Switzer Rd and Bluejacket Dr - construction</description></item>
        ///     <item><description>WB Johnson Dr at I-435 - bridge repair</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/description")]
        public string Description { get; set; }

        /// <summary>Gets or sets a description of a detour.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>Take 63rd St to Roe Ave and head south to 67th St</description></item>
        ///     <item><description>take US-40 to Blue Ridge Cut-Off</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/detourinfo")]
        public string DetourInfo { get; set; }

        /// <summary>
        /// Gets or sets the time that the traffic incident will end.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentendtimeutc")]
        public DateTime IncidentEndTimeUtc { get; set; }

        /// <summary>Gets or sets a unique Id for the incident.</summary>
        /// <example>Example: <c>384336120</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentid")]
        public string IncidentId { get; set; }

        /// <summary>
        /// Gets or sets the time that the incident occurred.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstarttimeutc")]
        public DateTime IncidentStartTimeUtc { get; set; }

        /// <summary>Gets or sets a description specific to lanes, such as lane closures.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>All lanes blocked</description></item>
        ///     <item><description>Left land blocked</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/laneinfo")]
        public string LaneInfo { get; set; }

        /// <summary>
        /// Gets or sets the time the incident information was last updated.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/lastmodifiedutc")]
        public DateTime LastModifiedUtc { get; set; }

        /// <summary>Gets or sets the incident latitude.</summary>
        /// <example>Example: <c>47.9603</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/latitude")]
        public double Latitude { get; set; }

        /// <summary>Gets or sets the incident longitude.</summary>
        /// <example>Longitude is an angular measurement in degrees ranging
        /// between +180 and -180 such as:<c> -122.12193 </c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a road closure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [road closed]; otherwise, <c>false</c>.
        /// </value>
        /// <example>Example: <c>true</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/roadclosed")]
        public bool RoadClosed { get; set; }

        /// <summary>Gets or sets the importance of the incident.</summary>
        /// <example>Example: <c>4</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentseverity")]
        public int Severity { get; set; }

        /// <summary>Gets or sets the incident status. Updated when processing</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus")]
        public int IncidentStatus { get; set; }

        /// <summary>Gets or sets the type of the incident.</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype")]
        public int IncidentType { get; set; }

        /// <summary>Gets or sets the Traffic Message Channel codes.</summary>
        /// <example>The message is coded according to the Alert C standard
        /// and looks like: <c>114-09870;114+05414</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/tmccodes")]
        public string TmcCodes { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the "to" point. The "to" point
        /// describes the the end of a traffic incident, such as the end of a
        /// construction zone.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/topointlatitude")]
        public double ToPointLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the "to" point. The "to" point
        /// describes the the end of a traffic incident, such as the end of a
        /// construction zone.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/topointlongitude")]
        public double ToPointLongitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TrafficInfo"/>
        /// incident has been visually verified or otherwise officially
        /// confirmed by a source such as the local police department.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verified; otherwise, <c>false</c>.
        /// </value>
        /// <example>Example: <c>false</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/verified")]
        public bool Verified { get; set; }

        #endregion

        #region Output for Status Notification

        /// <summary>Gets or sets the Status notification code</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/statuscode")]
        public int StatusCode { get; set; }

        /// <summary>Gets or sets the Status notification detail code</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/statusdetailcode")]
        public int StatusDetailCode { get; set; }

        /// <summary>Gets or sets the Description of the status</summary>
        /// <example>Example: <c>accident</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/statusmessage")]
        public string StatusMessage { get; set; }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return NotificationType switch
            {
                (int)NotificationTypeEnum.StatusReport => string.Format(
                        "[{0}] to [{1}] -> StatusReport AlternateVia: {2}, EndTime: {3}, RouteId: {4}, StartTime: {5}, StatusCode: {6}, StatusDetailCode: {7}, StatusMessage: {8}, SubscriptionId: {9}, NotificationThresholdInSeconds {10}, RenotificationThresholdInSeconds {11}",
                        StartAddress,
                        EndAddress,
                        AlternateVia,
                        EndTime,
                        RouteId,
                        StartTime,
                        StatusCode,
                        StatusDetailCode,
                        StatusMessage,
                        SubscriptionId,
                        NotificationThresholdInSeconds,
                        RenotificationThresholdInSeconds),
                (int)NotificationTypeEnum.Flow => string.Format(
                        "[{0}] to [{1}] -> Flow AlternateVia: {2}, DelayInSeconds: {3}, EndTime: {4}, FreeFlowTravelDurationInSeconds: {5}, HovDelayInSeconds: {6}, RouteId: {7}, StartTime: {8}, SubscriptionId: {9}, NotificationThresholdInSeconds {10}, RenotificationThresholdInSeconds {11}",
                        StartAddress,
                        EndAddress,
                        AlternateVia,
                        DelayInSeconds,
                        EndTime,
                        FreeFlowTravelDurationInSeconds,
                        HovDelayInSeconds,
                        RouteId,
                        StartTime,
                        SubscriptionId,
                        NotificationThresholdInSeconds,
                        RenotificationThresholdInSeconds),
                (int)NotificationTypeEnum.Incident => string.Format(
                        "[{0}] to [{1}] -> Incident AlternateVia: {2}, CongestionInfo: {3}, Description: {4}, DetourInfo: {5}, EndTime: {6}, IncidentId: {7}, LaneInfo: {8}, LastModifiedUtc: {9}, Latitude: {10}, Longitude: {11}, RoadClosed: {12}, RouteId: {13}, Severity: {14}, StartTime: {15}, IncidentStatus: {16}, SubscriptionId: {17}, IncidentType: {18}, TmcCodes: {19}, ToPointLatitude: {20}, ToPointLongitude: {21}, verified: {22}, NotificationThresholdInSeconds {23}, RenotificationThresholdInSeconds {24}",
                        StartAddress,
                        EndAddress,
                        AlternateVia,
                        CongestionInfo,
                        Description,
                        DetourInfo,
                        EndTime,
                        IncidentId,
                        LaneInfo,
                        LastModifiedUtc,
                        Latitude,
                        Longitude,
                        RoadClosed,
                        RouteId,
                        Severity,
                        StartTime,
                        IncidentStatus,
                        SubscriptionId,
                        IncidentType,
                        TmcCodes,
                        ToPointLatitude,
                        ToPointLongitude,
                        Verified,
                        NotificationThresholdInSeconds,
                        RenotificationThresholdInSeconds),
                _ => throw new NotImplementedException(string.Format("Value {0} for this.Notification not recognized", NotificationType.ToString())),
            };
        }
    }
}
