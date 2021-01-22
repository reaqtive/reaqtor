// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// The possible levels of incident importance.
    /// </summary>
    public enum IncidentSeverityEnum
    {
        /// <summary>Low Severity</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentseverity/low")]
        Low = 1,

        /// <summary>Minor Severity</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentseverity/minor")]
        Minor = 2,

        /// <summary>Moderate Severity</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentseverity/moderate")]
        Moderate = 3,

        /// <summary>Serious Severity</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentseverity/serious")]
        Serious = 4
    }

    /// <summary>
    /// The possible status of a traffic incident.
    /// </summary>
    public enum IncidentStatusEnum
    {
        /// <summary>Unknown status</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus/unknown")]
        Unknown = 0,

        /// <summary>Incident error</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus/error")]
        Error = 1,

        /// <summary>New incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus/newincident")]
        NewIncident = 2,

        /// <summary>
        /// Incident has been updated. For instance the severity has been 
        /// changed.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus/updated")]
        Updated = 3,

        /// <summary>Incident has been resolved</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidentstatus/resolved")]
        Resolved = 4
    }

    /// <summary>
    /// The possible types of incidents.
    /// </summary>
    /// <cref>http://msdn.microsoft.com/en-us/library/hh441730.aspx</cref>
    public enum IncidentTypeEnum
    {
        /// <summary>Accident Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/unknown")]
        Accident = 1,

        /// <summary>Congestion Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/congestion")]
        Congestion = 2,

        /// <summary>Disabled Vehicle Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/disabledvehicle")]
        DisabledVehicle = 3,

        /// <summary>Mass Transit Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/masstransit")]
        MassTransit = 4,

        /// <summary>Miscellaneous Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/miscellaneous")]
        Miscellaneous = 5,

        /// <summary>Other News Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/othernews")]
        OtherNews = 6,

        /// <summary>Planned Event Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/plannedevent")]
        PlannedEvent = 7,

        /// <summary>Road Hazard Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/roadhazard")]
        RoadHazard = 8,

        /// <summary>Construction Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/construction")]
        Construction = 9,

        /// <summary>Alert Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/alert")]
        Alert = 10,

        /// <summary>Weather Incident</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incidenttype/weather")]
        Weather = 11
    }

    /// <summary>
    /// Incident information
    /// </summary>
    public class TrafficIncidentInfo
    {
        /// <summary>Gets or sets the type of the incident.</summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/type")]
        public IncidentTypeEnum Type { get; set; }

        /// <summary>Gets or sets a description of the incident.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>W 95th St between Switzer Rd and Bluejacket Dr - construction</description></item>
        ///     <item><description>WB Johnson Dr at I-435 - bridge repair</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/description")]
        public string Description { get; set; }

        /// <summary>Gets or sets a unique Id for the incident.</summary>
        /// <example>Example: <c>384336120</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the time the incident information was last updated.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/lastmodifiedutc")]
        public DateTime LastModifiedUtc { get; set; }

        /// <summary>Gets or sets the incident latitude.</summary>
        /// <example>Example: <c>47.9603</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/latitude")]
        public double Latitude { get; set; }

        /// <summary>Gets or sets the incident longitude.</summary>
        /// <example>Longitude is an angular measurement in degrees ranging 
        /// between +180 and -180 such as:<c> -122.12193 </c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a road closure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [road closed]; otherwise, <c>false</c>.
        /// </value>
        /// <example>Example: <c>true</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/roadclosed")]
        public bool RoadClosed { get; set; }

        /// <summary>Gets or sets the importance of the incident.</summary>
        /// <example>Example: <c>4</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/severity")]
        public IncidentSeverityEnum Severity { get; set; }

        /// <summary>Gets or sets the incident status. Updated when processing</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/status")]
        public IncidentStatusEnum Status { get; set; }

        /// <summary>Gets or sets the Traffic Message Channel codes.</summary>
        /// <example>The message is coded according to the Alert C standard
        /// and looks like: <c>114-09870;114+05414</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/tmccodes")]
        public string TmcCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TrafficInfo"/> 
        /// incident has been visually verified or otherwise officially 
        /// confirmed by a source such as the local police department.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verified; otherwise, <c>false</c>.
        /// </value>
        /// <example>Example: <c>false</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/verified")]
        public bool Verified { get; set; }

        #region Other optional fields that might be returned from the API, although not seen so far
        /// <summary>Gets or sets a description of the congestion.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>generally slow</description></item>
        ///     <item><description>sluggish</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/congestioninfo")]
        public string CongestionInfo { get; set; }

        /// <summary>Gets or sets a description of a detour.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>Take 63rd St to Roe Ave and head south to 67th St</description></item>
        ///     <item><description>take US-40 to Blue Ridge Cut-Off</description></item>
        /// </list>
        /// </example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/detourinfo")]
        public string DetourInfo { get; set; }

        /// <summary>
        /// Gets or sets the time that the incident occurred.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/starttimeutc")]
        public DateTime StartTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the time that the traffic incident will end.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/endtimeutc")]
        public DateTime EndTimeUtc { get; set; }

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
        /// Gets or sets the latitude of the "to" point. The "to" point 
        /// describes the the end of a traffic incident, such as the end of a 
        /// construction zone.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/topointlatitude")]
        public double ToPointLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the "to" point. The "to" point 
        /// describes the the end of a traffic incident, such as the end of a 
        /// construction zone.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/trafficinfo/incident/topointlongitude")]
        public double ToPointLongitude { get; set; }
        #endregion
    }
}
