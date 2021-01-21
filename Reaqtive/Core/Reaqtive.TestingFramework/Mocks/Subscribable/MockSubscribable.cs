// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    public abstract class MockSubscribable<T> : ITestableSubscribable<T>
    {
        protected MockSubscribable(IClockable<long> clock)
        {
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Subscriptions = new List<Subscription>();
            TheObserver = NopObserver<T>.Instance;
        }

        protected IClockable<long> Clock { get; }

        public IList<Subscription> Subscriptions { get; }

        public abstract IList<Recorded<Notification<T>>> ObserverMessages { get; }

        public virtual ISubscription Subscribe(IObserver<T> observer)
        {
            Subscriptions.Add(new Subscription(Clock.Clock));
            var index = Subscriptions.Count - 1;

            TheObserver = observer ?? throw new ArgumentNullException(nameof(observer));
            AfterSubscribe(observer);

            return new MockSubscription(
                () =>
                {
                    Subscriptions[index] = new Subscription(Subscriptions[index].Subscribe, Clock.Clock);
                    if (index == Subscriptions.Count - 1)
                    {
                        TheObserver = NopObserver<T>.Instance;
                    }
                });
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => Subscribe(observer);

        protected virtual void AfterSubscribe(IObserver<T> observer)
        {
        }

        protected IObserver<T> TheObserver { get; set; }
    }
}
