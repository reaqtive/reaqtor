// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models.V2
{
    using Nuqleon.DataModel;

    /// <summary>
    /// Airline contains that metadata of the airline
    /// </summary>
    public class Airline
    {
        /// <summary>
        /// The airline code. eg: UA for United
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airline/code")]
        public string AirlineCode { get; set; }

        /// <summary>
        /// The name for the airline. United is United Airlines
        /// </summary>
        [Mapping("reactor://platform.bing.com/flight/airline/name")]
        public string Name { get; set; }
    }
}
