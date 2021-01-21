// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.Weather;

    /// <summary>
    /// An Observable for retrieving weather alert data
    /// </summary>
    [KnownType]
    public class WeatherAlertObservable : SubscribableBase<WeatherAlert>
    {
#pragma warning disable IDE0052 // Remove unread private members (placeholder)
        private readonly WeatherAlertParameters _parameters;
#pragma warning restore IDE0052 // Remove unread private members

        public WeatherAlertObservable(WeatherAlertParameters parameters)
        {
            _parameters = parameters;
        }

        protected override ISubscription SubscribeCore(IObserver<WeatherAlert> observer) => new _(this, observer);

        private sealed class _ : Operator<WeatherAlertObservable, WeatherAlert>
        {
            public _(WeatherAlertObservable parent, IObserver<WeatherAlert> observer)
                : base(parent, observer)
            {
            }
        }
    }
}
