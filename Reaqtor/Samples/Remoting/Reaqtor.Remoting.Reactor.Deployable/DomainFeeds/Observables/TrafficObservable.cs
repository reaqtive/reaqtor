// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.Traffic;

    /// <summary>
    /// An Observable for retrieving weather alert data
    /// </summary>
    [KnownType]
    public class TrafficObservable : SubscribableBase<TrafficInfo>
    {
#pragma warning disable IDE0052 // Remove unread private members (placeholder)
        private readonly TrafficParameters _parameters;
        private readonly TrafficConfiguration _configuration;
#pragma warning restore IDE0052 // Remove unread private members

        public TrafficObservable(TrafficParameters parameters, TrafficConfiguration configuration)
        {
            _parameters = parameters;
            _configuration = configuration;
        }

        protected override ISubscription SubscribeCore(IObserver<TrafficInfo> observer) => new _(this, observer);

        private sealed class _ : Operator<TrafficObservable, TrafficInfo>
        {
            public _(TrafficObservable parent, IObserver<TrafficInfo> observer)
                : base(parent, observer)
            {
            }
        }
    }
}
