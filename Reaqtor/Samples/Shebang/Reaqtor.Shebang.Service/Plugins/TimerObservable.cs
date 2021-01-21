// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Reaqtive;

namespace Reaqtor.Shebang.Extensions
{
    //
    // Example of a stateless observable, using the ISubscribable<T> interface which is the Reaqtive
    // equivalent of IObservable<T>. The main difference is that a subscription created through this
    // interface supports more than just IDisposable; it also provides a mechanism to traverse artifacts
    // using a visitor pattern for purposes of state saving and loading.
    //

    internal sealed class TimerObservable : SubscribableBase<DateTimeOffset>
    {
        private readonly TimeSpan _period;

        public TimerObservable(TimeSpan period) => _period = period;

        protected override ISubscription SubscribeCore(IObserver<DateTimeOffset> observer) => new Subscription(_period, observer);

        //
        // Use of ContextSwitchOperator base class such that calls to OnNext are correctly switched to
        // the engine's scheduler, which is critical to support checkpoint/recovery.
        //

        private sealed class Subscription : ContextSwitchOperator<TimeSpan, DateTimeOffset>, IUnloadableOperator
        {
            private Timer _timer;

            public Subscription(TimeSpan parent, IObserver<DateTimeOffset> observer) : base(parent, observer)
            {
            }

            public override string Name => "sb:Timer";

            public override Version Version => new(1, 0, 0, 0);

            protected override void OnStart()
            {
                base.OnStart();

                _timer = new Timer(Tick, null, 0, (int)Params.TotalMilliseconds);
            }

            protected override void OnDispose()
            {
                base.OnDispose();

                _timer?.Dispose();
            }

            private void Tick(object state) => OnNext(DateTimeOffset.Now);

            public void Unload() => _timer?.Dispose();
        }
    }
}
