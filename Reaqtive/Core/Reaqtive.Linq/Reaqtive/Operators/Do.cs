// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Do<T, N> : SubscribableBase<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly Func<T, N> _selector;
        private readonly IObserver<N> _observer;

        public Do(ISubscribable<T> source, Func<T, N> selector, IObserver<N> observer)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);
            Debug.Assert(observer != null);

            _source = source;
            _selector = selector;
            _observer = observer;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Do<T, N>, T>, IObserver<T>
        {
            private readonly IObserver<N> _doer;

            public _(Do<T, N> parent, IObserver<T> observer)
                : base(parent, observer)
            {
                _doer = Params._observer;
            }

            public override string Name => "rc:Do";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                try
                {
                    _doer.OnCompleted();
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                try
                {
                    _doer.OnError(error);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                Output.OnError(error);
                Dispose();
            }

            public void OnNext(T value)
            {
                try
                {
                    _doer.OnNext(Params._selector(value));
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                Output.OnNext(value);
            }

            protected override ISubscription OnSubscribe()
            {
                var src = Params._source.Subscribe(this);

                if (Params._observer is ISubscription o)
                {
                    return StaticCompositeSubscription.Create(src, o);
                }
                else
                {
                    return src;
                }
            }
        }
    }
}
