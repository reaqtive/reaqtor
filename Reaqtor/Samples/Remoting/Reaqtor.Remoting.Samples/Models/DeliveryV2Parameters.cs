// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public class DeliveryV2Parameters
    {
        /// <summary>
        /// Agentds ID
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryparameters/anid")]
        public string AnId { get; set; }

        /// <summary>
        /// Gets or sets the carrier
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryparameters/carrier")]
        public string Carrier { get; set; }

        /// <summary>
        /// Gets or sets the tracking number, e.g. "1Z12345E1505270452"
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryparameters/trackingnumber")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the tracking url
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryparameters/trackingUrl")]
        public string TrackingUrl { get; set; }

        /// <summary>
        /// Gets or sets the api credentials for the carrier
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/deliveryparameters/apicredential")]
        public CarrierCredential ApiCredential { get; set; }
    }
}
