// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;

    using Nuqleon.DataModel;

    public class DeliveryV2Info
    {
        /// <summary>
        /// Status of the package
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryinfo/status")]
        public DeliveryV2Status Status { get; set; }

        /// <summary>
        /// Gets or sets the detailed status.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryinfo/statusdetail")]
        public string StatusDetail { get; set; }

        /// <summary>
        /// Gets or sets the expected delivery date.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryinfo/expecteddeliverydate")]
        public DateTime? ExpectedDeliveryDate { get; set; }
    }
}
