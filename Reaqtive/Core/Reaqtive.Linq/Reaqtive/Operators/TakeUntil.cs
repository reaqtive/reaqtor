// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class TakeUntil<TSource, TOther> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly ISubscribable<TOther> _other;

        public TakeUntil(ISubscribable<TSource> source, ISubscribable<TOther> other)
        {
            Debug.Assert(source != null);
            Debug.Assert(other != null);

            _source = source;
            _other = other;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulOperator<TakeUntil<TSource, TOther>, TSource>, IObserver<TSource>
        {
            /// <summary>
            /// Unlike RX, we are leaking the _ object, so we can't take a lock on that reference.
            /// </summary>
            private readonly object _syncLock = new();

            private ISubscription _otherSubscription;
            private volatile bool _gateOpened;

            public _(TakeUntil<TSource, TOther> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:TakeUntil";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                var firstObserver = new FirstObserver(this);
                var otherObserver = new OtherObserver(this);

                var firstSubscription = Params._source.Subscribe(firstObserver);
                _otherSubscription = Params._other.Subscribe(otherObserver);
                otherObserver.Subscription = _otherSubscription;

                return new[] { firstSubscription, _otherSubscription };
            }

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(TSource value)
            {
                Output.OnNext(value);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _gateOpened = reader.Read<bool>();

                if (_gateOpened)
                {
                    _otherSubscription.Dispose();
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_gateOpened);
            }

            public bool GateOpened
            {
                get => _gateOpened;

                set
                {
                    _gateOpened = value;

                    StateChanged = true;
                }
            }

            private class FirstObserver : IObserver<TSource>
            {
                private readonly _ _parent;

                public FirstObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnCompleted()
                {
                    lock (_parent._syncLock)
                    {
                        _parent.OnCompleted();
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._syncLock)
                    {
                        _parent.OnError(error);
                    }
                }

                public void OnNext(TSource value)
                {
                    if (_parent.GateOpened)
                    {
                        // to be consistent with SkipUntil, see below comment
                        _parent.OnNext(value);
                    }
                    else
                    {
                        // normal use case with concurrency of the second observer
                        lock (_parent._syncLock)
                        {
                            _parent.OnNext(value);
                        }
                    }
                }
            }

            private sealed class OtherObserver : IObserver<TOther>
            {
                private readonly _ _parent;
                private ISubscription _subscription;

                public OtherObserver(_ parent)
                {
                    _parent = parent;
                }

                public ISubscription Subscription
                {
                    set => _subscription = value;
                }

                public void OnCompleted()
                {
                    lock (_parent._syncLock)
                    {
                        // open the gate for the first observer so that in case of an empty second Observable we will push all messages 
                        // (we want to do this so that TakeUntil and SkipUntil are complimentary)
                        // xs.SkipUntil(empty).Concat(xs.TakeUntil(empty)) == xs
                        _parent.GateOpened = true;
                        _subscription.Dispose();
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._syncLock)
                    {
                        // end the pumping of notifications
                        _parent.OnError(error);
                    }
                }

                public void OnNext(TOther value)
                {
                    lock (_parent._syncLock)
                    {
                        // end the pumping of notifications
                        _parent.OnCompleted();
                    }
                }
            }
        }
    }
}
