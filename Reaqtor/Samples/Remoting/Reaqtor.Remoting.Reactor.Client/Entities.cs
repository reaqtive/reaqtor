// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using Nuqleon.DataModel;

namespace Reaqtor.Remoting
{
    public class Geocoordinate
    {
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/requestid")]
        public string RequestId { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/userid")]
        public string UserId { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/useridhashcode")]
        public int UserIdHashcode { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/deviceid")]
        public string DeviceId { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/datacenter")]
        public string DataCenter { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatesignal/usersignal")]
        public UserSignal UserSignal { get; set; }
    }

    public class UserSignal
    {
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/type")]
        public string Type { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/timestamp")]
        public DateTime Timestamp { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/clientrequestid")]
        public string ClientRequestid { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/agentinstanceid")]
        public string AgentInstanceId { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinateusersignal/value")]
        public GeocoordinateValue Value { get; set; }
    }

    public class GeocoordinateValue
    {
        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/status")]
        public string Status { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/accuracy")]
        public string Accuracy { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/altitude")]
        public string Altitude { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/altitudeaccuracy")]
        public string AltitudeAccuracy { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/heading")]
        public string Heading { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/latitude")]
        public double Latitude { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/longitude")]
        public double Longtitude { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/positionsource")]
        public string PositionSource { get; set; }

        [Mapping("reactor://platform.bing.com/reactiveprocessingentity/real/geocoordinatevalue/speed")]
        public string Speed { get; set; }
    }
}
