// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive.Operators
{
    /// <summary>
    /// Ignores everything from a subscription unless a given "throttling observable" fires.
    /// For example, we might set the throttling observer to be a timer, and if the timer
    /// has not fired by the time we observe something from our subscription, then we (1)
    /// ignore it, (2) restart the timer, and (3) repeat.
    /// </summary>
    /// <typeparam name="TSource">Type being observed from subscription.</typeparam>
    /// <typeparam name="TThrottle">Type of result output by throttle.</typeparam>
    internal sealed class Throttle<TSource, TThrottle> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, ISubscribable<TThrottle>> _throttleSelector;

        public Throttle(ISubscribable<TSource> source, Func<TSource, ISubscribable<TThrottle>> throttleSelector)
        {
            _source = source;
            _throttleSelector = throttleSelector;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : HigherOrderInputStatefulOperator<Throttle<TSource, TThrottle>, TSource>, IObserver<TSource>
        {
            private SerialSubscription _cancelable;

            private TSource _value;
            private bool _hasValue;
            private readonly object _gate;
            private ulong _id;
            private IOperatorContext _context;

            public _(Throttle<TSource, TThrottle> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
                _gate = new object();
            }

            public override string Name => "rc:Throttle";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _cancelable = new SerialSubscription();
                _id = 0UL;

                return new[] { Params._source.Subscribe(this), _cancelable };
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            public void OnCompleted()
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    if (_hasValue)
                    {
                        Output.OnNext(_value);
                    }

                    Output.OnCompleted();
                    Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public void OnError(Exception error)
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    Output.OnError(error);
                    Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public void OnNext(TSource value)
            {
                var throttle = default(ISubscribable<TThrottle>);
                try
                {
                    throttle = Params._throttleSelector(value);
                }
                catch (Exception error)
                {
                    lock (_gate)
                    {
                        Output.OnError(error);
                        Dispose();
                    }

                    return;
                }

                ulong currentId;
                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
                    _id = unchecked(_id + 1);
                    currentId = _id;
                }

                try
                {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Observer will be owned by inner subscription.)

                    var observer = new ThrottleObserver(this, value, currentId);

                    var subscription = SubscribeInner<TThrottle>(throttle, observer);
                    observer.Subscription = subscription;

                    SubscriptionInitializeVisitor.Initialize(subscription, _context);

                    _cancelable.Subscription = subscription;

#pragma warning restore CA2000
#pragma warning restore IDE0079
                }
                catch (Exception error)
                {
                    lock (_gate)
                    {
                        Output.OnError(error);
                        Dispose();
                    }
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasValue);
                writer.Write(_value);
                writer.Write(_id);

                var hasInner = _cancelable.Subscription != null;
                writer.Write(hasInner);

                if (hasInner)
                {
                    SaveInner(_cancelable.Subscription, writer);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _value = reader.Read<TSource>();
                _id = reader.Read<ulong>();

                var hasInner = reader.Read<bool>();

                if (hasInner)
                {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Observer will be owned by inner subscription.)

                    var observer = new ThrottleObserver(this);

                    var subscription = LoadInner<TThrottle>(reader, observer);
                    observer.Subscription = subscription;
                    _cancelable.Subscription = subscription;

#pragma warning restore CA2000
#pragma warning restore IDE0079
                }
            }

            private sealed class ThrottleObserver : StatefulObserver<TThrottle>
            {
                private readonly _ _parent;
                private ISubscription _self;
                private TSource _value;
                private ulong _currentId;

                public ThrottleObserver(_ parent)
                {
                    _parent = parent;
                }

                public ThrottleObserver(_ parent, TSource value, ulong currentid)
                    : this(parent)
                {
                    _value = value;
                    _currentId = currentid;
                }

                public override string Name => "rc:Throttle/v";

                public override Version Version => Versioning.v1;

                public ISubscription Subscription
                {
                    set => _self = value;
                }

                protected override void OnCompletedCore()
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentId)
                        {
                            _parent.Output.OnNext(_value);
                        }

                        _parent._hasValue = false;
                        _self.Dispose();
                    }
                }

                protected override void OnErrorCore(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent.Output.OnError(error);
                        _parent.Dispose();
                    }
                }

                protected override void OnNextCore(TThrottle value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentId)
                        {
                            _parent.Output.OnNext(_value);
                            _parent._hasValue = false;
                        }

                        _self.Dispose();
                    }
                }

                protected override void LoadStateCore(IOperatorStateReader reader)
                {
                    base.LoadStateCore(reader);

                    _currentId = reader.Read<ulong>();
                    _value = reader.Read<TSource>();
                }

                protected override void SaveStateCore(IOperatorStateWriter writer)
                {
                    base.SaveStateCore(writer);

                    writer.Write(_currentId);
                    writer.Write(_value);
                }
            }
        }
    }
}
