// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.News;

    /// <summary>
    /// Subscribable implementation for the 'news' feed
    /// </summary>
    [KnownType]
    public class NewsObservable : SubscribableBase<NewsInfo>
    {
#pragma warning disable IDE0052 // Remove unread private members (placeholder)
        private readonly NewsParameters _parameters;
#pragma warning restore IDE0052 // Remove unread private members

        public NewsObservable(NewsParameters parameters)
        {
            _parameters = parameters;
        }

        protected override ISubscription SubscribeCore(IObserver<NewsInfo> observer) => new _(this, observer);

        private sealed class _ : Operator<NewsObservable, NewsInfo>
        {
            public _(NewsObservable parent, IObserver<NewsInfo> observer)
                : base(parent, observer)
            {
            }
        }
    }
}
