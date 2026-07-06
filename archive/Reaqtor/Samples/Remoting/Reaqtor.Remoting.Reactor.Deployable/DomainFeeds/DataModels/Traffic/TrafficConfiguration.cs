// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.Traffic
{
    /// <summary>
    /// Holds the configuration details for running the traffic subscribable
    /// </summary>
    [KnownType]
    public class TrafficConfiguration
    {
        /// <summary>
        /// Gets or sets the TaRN service endpoint.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/traffic/configuration/serviceendpoint")]
        public string ServiceEndpoint { get; set; }

        public override string ToString()
        {
            return string.Format("[ServiceEndpoint:{0}]", ServiceEndpoint);
        }
    }
}
