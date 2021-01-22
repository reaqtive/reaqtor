// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using Nuqleon.DataModel;

    public sealed class WayPoint
    {
        [Mapping("traffic://waypoint/address")]
        public string Address { get; set; }

        [Mapping("traffic://waypoint/isViaPoint")]
        public bool IsViaPoint { get; set; }
    }
}
