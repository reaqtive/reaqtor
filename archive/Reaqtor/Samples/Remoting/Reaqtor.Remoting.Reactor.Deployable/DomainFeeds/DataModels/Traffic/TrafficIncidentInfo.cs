// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// The possible levels of incident importance.
    /// </summary>
    public enum IncidentSeverityEnum
    {
        /// <summary>Low Severity</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentseverity/low")]
        Low = 1,

        /// <summary>Minor Severity</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentseverity/minor")]
        Minor = 2,

        /// <summary>Moderate Severity</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentseverity/moderate")]
        Moderate = 3,

        /// <summary>Serious Severity</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentseverity/serious")]
        Serious = 4
    }

    /// <summary>
    /// The possible status of a traffic incident.
    /// </summary>
    public enum IncidentStatusEnum
    {
        /// <summary>Unknown status</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentstatus/unknown")]
        Unknown = 0,

        /// <summary>Incident error</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentstatus/error")]
        Error = 1,

        /// <summary>New incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentstatus/newincident")]
        NewIncident = 2,

        /// <summary>
        /// Incident has been updated. For instance the severity has been
        /// changed.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentstatus/updated")]
        Updated = 3,

        /// <summary>Incident has been resolved</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidentstatus/resolved")]
        Resolved = 4
    }

    /// <summary>
    /// The possible types of incidents.
    /// </summary>
    /// <cref>http://msdn.microsoft.com/en-us/library/hh441730.aspx</cref>
    public enum IncidentTypeEnum
    {
        /// <summary>Accident Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/unknown")]
        Accident = 1,

        /// <summary>Congestion Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/congestion")]
        Congestion = 2,

        /// <summary>Disabled Vehicle Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/disabledvehicle")]
        DisabledVehicle = 3,

        /// <summary>Mass Transit Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/masstransit")]
        MassTransit = 4,

        /// <summary>Miscellaneous Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/miscellaneous")]
        Miscellaneous = 5,

        /// <summary>Other News Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/othernews")]
        OtherNews = 6,

        /// <summary>Planned Event Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/plannedevent")]
        PlannedEvent = 7,

        /// <summary>Road Hazard Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/roadhazard")]
        RoadHazard = 8,

        /// <summary>Construction Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/construction")]
        Construction = 9,

        /// <summary>Alert Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/alert")]
        Alert = 10,

        /// <summary>Weather Incident</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incidenttype/weather")]
        Weather = 11
    }

    /// <summary>
    /// Incident information
    /// </summary>
    [KnownType]
    public class TrafficIncidentInfo
    {
        /// <summary>Gets or sets the type of the incident.</summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/type")]
        public IncidentTypeEnum Type { get; set; }

        /// <summary>Gets or sets a description of the incident.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>W 95th St between Switzer Rd and Bluejacket Dr - construction</description></item>
        ///     <item><description>WB Johnson Dr at I-435 - bridge repair</description></item>
        /// </list>
        /// </example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/description")]
        public string Description { get; set; }

        /// <summary>Gets or sets a unique Id for the incident.</summary>
        /// <example>Example: <c>384336120</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the time the incident information was last updated.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/lastmodifiedutc")]
        public DateTime LastModifiedUtc { get; set; }

        /// <summary>Gets or sets the incident latitude.</summary>
        /// <example>Example: <c>47.9603</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/latitude")]
        public double Latitude { get; set; }

        /// <summary>Gets or sets the incident longitude.</summary>
        /// <example>Longitude is an angular measurement in degrees ranging
        /// between +180 and -180 such as:<c> -122.12193 </c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a road closure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [road closed]; otherwise, <c>false</c>.
        /// </value>
        /// <example>Example: <c>true</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/roadclosed")]
        public bool RoadClosed { get; set; }

        /// <summary>Gets or sets the importance of the incident.</summary>
        /// <example>Example: <c>4</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/severity")]
        public IncidentSeverityEnum Severity { get; set; }

        /// <summary>Gets or sets the incident status. Updated when processing</summary>
        /// <example>Example: <c>2</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/status")]
        public IncidentStatusEnum Status { get; set; }

        /// <summary>Gets or sets the Traffic Message Channel codes.</summary>
        /// <example>The message is coded according to the Alert C standard
        /// and looks like: <c>114-09870;114+05414</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/tmccodes")]
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
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/verified")]
        public bool Verified { get; set; }

        #region Other optional fields that might be returned from the API, although not seen so far
        /// <summary>Gets or sets a description of the congestion.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>generally slow</description></item>
        ///     <item><description>sluggish</description></item>
        /// </list>
        /// </example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/congestioninfo")]
        public string CongestionInfo { get; set; }

        /// <summary>Gets or sets a description of a detour.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>Take 63rd St to Roe Ave and head south to 67th St</description></item>
        ///     <item><description>take US-40 to Blue Ridge Cut-Off</description></item>
        /// </list>
        /// </example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/detourinfo")]
        public string DetourInfo { get; set; }

        /// <summary>
        /// Gets or sets the time that the incident occurred.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/starttimeutc")]
        public DateTime StartTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the time that the traffic incident will end.
        /// </summary>
        /// <example>The last modified utc value is a DateTime value expressed
        /// with the SortableDateTimePattern: <c>2013-02-25T22:22:46</c></example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/endtimeutc")]
        public DateTime EndTimeUtc { get; set; }

        /// <summary>Gets or sets a description specific to lanes, such as lane closures.</summary>
        /// <example>
        /// <list type="bullet">
        ///     <item><description>All lanes blocked</description></item>
        ///     <item><description>Left land blocked</description></item>
        /// </list>
        /// </example>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/laneinfo")]
        public string LaneInfo { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the "to" point. The "to" point
        /// describes the the end of a traffic incident, such as the end of a
        /// construction zone.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/topointlatitude")]
        public double ToPointLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the "to" point. The "to" point
        /// describes the the end of a traffic incident, such as the end of a
        /// construction zone.
        /// </summary>
        [Mapping(TrafficConstants.TrafficInfoMappingUriPrefix + "/incident/topointlongitude")]
        public double ToPointLongitude { get; set; }
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.AppendFormat("Id:{0},", Id);
            sb.AppendFormat("Type:{0},", Type);
            sb.AppendFormat("Verified:{0},", Verified);
            sb.AppendFormat("CongestionInfo:{0},", CongestionInfo);
            sb.AppendFormat("Description:{0},", Description);
            sb.AppendFormat("DetourInfo:{0},", DetourInfo);
            sb.AppendFormat("EndTimeUtc:{0},", EndTimeUtc);
            sb.AppendFormat("LaneInfo:{0},", LaneInfo);
            sb.AppendFormat("LastModifiedUtc:{0},", LastModifiedUtc);
            sb.AppendFormat("Latitude:{0},", Latitude);
            sb.AppendFormat("Longitude:{0},", Longitude);
            sb.AppendFormat("RoadClosed:{0},", RoadClosed);
            sb.AppendFormat("Severity:{0},", Severity);
            sb.AppendFormat("StartTimeUtc:{0},", StartTimeUtc);
            sb.AppendFormat("Status:{0},", Status);
            sb.AppendFormat("TmcCodes:{0},", TmcCodes);
            sb.AppendFormat("ToPointLatitude:{0},", ToPointLatitude);
            sb.AppendFormat("ToPointLongitude:{0},", ToPointLongitude);
            sb.Append(']');

            return sb.ToString();
        }
    }
}
