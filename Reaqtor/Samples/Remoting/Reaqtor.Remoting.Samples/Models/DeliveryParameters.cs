// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    /// <summary>
    /// Data definition for the parameters to the Delivery parameterized Observable.
    /// Clients must specify the carrier and the tracking number
    /// </summary>
    public class DeliveryParameters
    {
        /// <summary>
        /// Delivery Carriers
        /// </summary>
        public enum Carriers
        {
            [Mapping("bing://deliveryparameters/carriers/ups")]
            UPS = 1,
            [Mapping("bing://deliveryparameters/carriers/fedex")]
            FedEx = 2,
            [Mapping("bing://deliveryparameters/carriers/usps")]
            USPS = 3,
            [Mapping("bing://deliveryparameters/carriers/dhl")]
            DHL = 4,
        }

        /// <summary>
        /// Gets or sets the carrier
        /// </summary>
        [Mapping("bing://deliveryparameters/carrier")]
        public Carriers Carrier { get; set; }

        /// <summary>
        /// Gets or sets the flight number, e.g. "192"
        /// </summary>
        [Mapping("bing://deliveryparameters/trackingnumber")]
        public string TrackingNumber { get; set; }
    }
}
