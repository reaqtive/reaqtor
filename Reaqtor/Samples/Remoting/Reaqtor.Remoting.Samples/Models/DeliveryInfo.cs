// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;

    using Nuqleon.DataModel;

    /// <summary>
    /// Data definition for a Delivery event
    /// </summary>
    public class DeliveryInfo
    {
        /// <summary>
        /// Delivery status enumeration
        /// </summary>
        public enum DeliveryStatus
        {
            [Mapping("bing://deliveryinfo/deliverystatus/unknown")]
            Unknown = 0,
            [Mapping("bing://deliveryinfo/deliverystatus/notshipped")]
            NotShipped = 1,
            [Mapping("bing://deliveryinfo/deliverystatus/shipped")]
            Shipped = 2,
            [Mapping("bing://deliveryinfo/deliverystatus/delivered")]
            Delivered = 3
        }

        /// <summary>
        /// Gets or sets the delivery status.
        /// </summary>
        [Mapping("bing://deliveryinfo/status")]
        public DeliveryStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the carrier.
        /// </summary>
        [Mapping("bing://deliveryinfo/carrier")]
        public DeliveryParameters.Carriers Carrier { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        [Mapping("bing://deliveryinfo/trackingnumber")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the expected delivery date.
        /// </summary>
        [Mapping("bing://deliveryinfo/expecteddeliverydate")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the date the package was shipped.
        /// </summary>
        [Mapping("bing://deliveryinfo/shipdate")]
        public DateTime? ShipDate { get; set; }
    }
}