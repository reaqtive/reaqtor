// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.Traffic;

    /// <summary>
    /// Synthetic observable implementation for the traffic feed. It pushes dummy data
    /// </summary>
    public class SyntheticTrafficObservable : TrafficObservable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyntheticTrafficObservable"/> class.
        /// </summary>
        /// <param name="parameters">The parameters for this observable</param>
        public SyntheticTrafficObservable(TrafficParameters parameters)
            : base(parameters, new TrafficConfiguration())
        {
        }
    }
}
