// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable
{
    public sealed class GarbageCollectorObservable : SubscribableBase<int>
    {
        private readonly ISubscribable<int> _source;

        public GarbageCollectorObservable(ISubscribable<int> source) => _source = source;

        protected override ISubscription SubscribeCore(IObserver<int> observer) => new _(this, observer);

        #region Nested

        private sealed class _ : UnaryOperator<GarbageCollectorObservable, int>, IObserver<int>
        {
            public _(GarbageCollectorObservable parent, IObserver<int> observer)
                : base(parent, observer)
            {
            }

            protected override ISubscription OnSubscribe() => Params._source.Subscribe(this);

            #region IObserver<int>

            public void OnCompleted() => Output.OnCompleted();

            public void OnError(Exception error) => Output.OnError(error);

            public void OnNext(int value)
            {
                if (value is >= 0 and <= 2)
                {
                    GC.Collect(value);
                    Output.OnNext(value);
                }
                else
                {
                    OnError(new ArgumentOutOfRangeException(nameof(value), "Expected generation value between 0 and 2, inclusive."));
                }
            }

            #endregion
        }

        #endregion
    }
}
