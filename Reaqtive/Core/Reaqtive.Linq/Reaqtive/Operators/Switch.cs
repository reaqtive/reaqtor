// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive.Operators
{
    internal sealed class Switch<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<ISubscribable<TSource>> _sources;

        public Switch(ISubscribable<ISubscribable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : HigherOrderInputStatefulOperator<Switch<TSource>, TSource>, IObserver<ISubscribable<TSource>>
        {
            private object _lock;
            private ISubscription _subscription;
            private SerialSubscription _innerSubscription;
            private ulong _latest;
            private bool _isStopped;
            private bool _hasLatest;
            private IOperatorContext _context;

            public _(Switch<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Switch";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _lock = new object();
                _innerSubscription = new SerialSubscription();
                _isStopped = false;
                _latest = 0UL;
                _hasLatest = false;
                _subscription = Params._sources.Subscribe(this);

                return new ISubscription[]
                {
                    _subscription,
                    _innerSubscription
                };
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasLatest);
                writer.Write(_latest);
                writer.Write(_isStopped);

                var hasInner = _innerSubscription.Subscription != null;
                writer.Write(hasInner);

                if (hasInner)
                {
                    SaveInner(_innerSubscription.Subscription, writer);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasLatest = reader.Read<bool>();
                _latest = reader.Read<ulong>();
                _isStopped = reader.Read<bool>();

                var hasInner = reader.Read<bool>();

                if (hasInner)
                {
                    var observer = new i(this, _latest);

                    var subscription = LoadInner<TSource>(reader, observer);
                    observer.Subscription = subscription;
                    _innerSubscription.Subscription = subscription;
                }
            }

            public void OnCompleted()
            {
                lock (_lock)
                {
                    _subscription.Dispose();

                    _isStopped = true;

                    if (!_hasLatest)
                    {
                        Output.OnCompleted();
                        Dispose();
                    }
                }
            }

            public void OnError(Exception error)
            {
                lock (_lock)
                {
                    Output.OnError(error);
                    Dispose();
                }
            }

            public void OnNext(ISubscribable<TSource> source)
            {
                var id = default(ulong);
                lock (_lock)
                {
                    id = unchecked(++_latest);
                    _hasLatest = true;
                }

                try
                {
                    var observer = new i(this, id);

                    var subscription = SubscribeInner<TSource>(source, observer);
                    observer.Subscription = subscription;

                    SubscriptionInitializeVisitor.Subscribe(subscription);
                    SubscriptionInitializeVisitor.SetContext(subscription, _context);

                    _innerSubscription.Subscription = subscription;

                    SubscriptionInitializeVisitor.Start(subscription);
                }
                catch (Exception ex)
                {
                    lock (_lock)
                    {
                        Output.OnError(ex);
                        Dispose();
                    }
                }
            }

            private sealed class i : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly ulong _id;
                private ISubscription _self;

                public i(_ parent, ulong id)
                {
                    _parent = parent;
                    _id = id;
                }

                public ISubscription Subscription
                {
                    set => _self = value;
                }

                public void OnCompleted()
                {
                    lock (_parent._lock)
                    {
                        _self.Dispose();

                        if (_parent._latest == _id)
                        {
                            _parent._hasLatest = false;

                            if (_parent._isStopped)
                            {
                                _parent.Output.OnCompleted();
                                _parent.Dispose();
                            }
                        }
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._lock)
                    {
                        if (_parent._latest == _id)
                        {
                            _parent.Output.OnError(error);
                            _parent.Dispose();
                        }
                    }
                }

                public void OnNext(TSource value)
                {
                    lock (_parent._lock)
                    {
                        if (_parent._latest == _id)
                        {
                            _parent.Output.OnNext(value);
                        }
                    }
                }
            }
        }
    }
}
