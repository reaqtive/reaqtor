// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.FlightStatus;

    /// <summary>
    /// An Observable for retrieving flight status data
    /// </summary>
    [KnownType]
    public class FlightStatusObservable : SubscribableBase<FlightStatus>
    {
#pragma warning disable IDE0052 // Remove unread private members (placeholder)
        private readonly FlightStatusParameters _parameters;
#pragma warning restore IDE0052 // Remove unread private members

        public FlightStatusObservable(FlightStatusParameters parameters)
        {
            _parameters = parameters;
        }

        protected override ISubscription SubscribeCore(IObserver<FlightStatus> observer) => new _(this, observer);

        private sealed class _ : Operator<FlightStatusObservable, FlightStatus>
        {
            public _(FlightStatusObservable parent, IObserver<FlightStatus> observer)
                : base(parent, observer)
            {
            }
        }
    }
}
