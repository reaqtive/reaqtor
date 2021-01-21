// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive.Operators
{
    internal sealed class Merge<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource>[] _sources;

        public Merge(params ISubscribable<TSource>[] sources)
        {
            _sources = sources;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulOperator<Merge<TSource>, TSource>
        {
            private readonly object _gate;
            private readonly bool[] _done;

            public _(Merge<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
                _gate = new object();
                _done = new bool[parent._sources.Length];
            }

            public override string Name => "rc:Merge-N";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                var sources = Params._sources;

                var n = sources.Length;
                var subscriptions = new ISubscription[n];

                for (var i = 0; i < n; i++)
                {
                    var observer = new Observer(this, i);
                    var subscription = sources[i].Subscribe(observer);
                    subscriptions[i] = observer.Subscription = subscription;
                }

                return new StaticCompositeSubscription(subscriptions /* no cloning */);
            }

            private void OnCompleted(int index)
            {
                lock (_gate)
                {
                    if (!_done[index])
                    {
                        _done[index] = true;
                        StateChanged = true;
                    }

                    // NB: optimizations are possible; we assume the number of sources is low for now

                    foreach (var done in _done)
                    {
                        if (!done)
                        {
                            return;
                        }
                    }

                    Output.OnCompleted();
                    Dispose();
                }
            }

            private void OnError(Exception error)
            {
                lock (_gate)
                {
                    Output.OnError(error);
                    Dispose();
                }
            }

            private void OnNext(TSource value)
            {
                lock (_gate)
                {
                    Output.OnNext(value);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                for (var i = 0; i < _done.Length; i++)
                {
                    _done[i] = reader.Read<bool>();
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                for (var i = 0; i < _done.Length; i++)
                {
                    writer.Write(_done[i]);
                }
            }

            private sealed class Observer : IObserver<TSource>
            {
                private readonly int _index;
                private readonly _ _parent;
                private ISubscription _subscription;

                public Observer(_ parent, int index)
                {
                    _parent = parent;
                    _index = index;
                }

                public ISubscription Subscription
                {
                    set => _subscription = value;
                }

                public void OnCompleted()
                {
                    _subscription.Dispose();
                    _parent.OnCompleted(_index);
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnNext(TSource value)
                {
                    _parent.OnNext(value);
                }
            }
        }
    }
}
