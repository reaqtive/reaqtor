// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public enum DeliveryV2Status
    {
        [Mapping("bing://reactiveprocessingentity/real/deliverystatus/unknown")]
        Unknown = 0,

        [Mapping("bing://reactiveprocessingentity/real/deliverystatus/processed")]
        Processed = 1,

        [Mapping("bing://reactiveprocessingentity/real/deliverystatus/intransit")]
        InTransit = 2,

        [Mapping("bing://reactiveprocessingentity/real/deliverystatus/delivered")]
        Delivered = 3,

        [Mapping("bing://reactiveprocessingentity/real/deliverystatus/other")]
        Other = 4
    }
}
