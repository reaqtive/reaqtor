// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.News;

    /// <summary>
    /// Mock Observable implementation for the 'news' feed
    /// </summary>
    [KnownType]
    public class MockNewsObservable : NewsObservable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockNewsObservable"/> class.
        /// </summary>
        /// <param name="parameters">The parameters for this observable</param>
        public MockNewsObservable(NewsParameters parameters)
            : base(parameters)
        {
        }
    }
}
