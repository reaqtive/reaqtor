// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Reaqtive.Operators
{
    internal sealed class SelectMany<TSource, TCollection, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, ISubscribable<TCollection>> _collectionSelector;
        private readonly Func<TSource, TCollection, TResult> _resultSelector;

        public SelectMany(ISubscribable<TSource> source,
            Func<TSource, ISubscribable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            _source = source;
            _collectionSelector = collectionSelector;
            _resultSelector = resultSelector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : HigherOrderInputStatefulOperator<SelectMany<TSource, TCollection, TResult>, TResult>, IObserver<TSource>
        {
            private const string MAXINNERSUBCOUNTSETTING = "rx://operators/bind/settings/maxConcurrentInnerSubscriptionCount";
            private int _maxInnerCount;

            private readonly object _lock = new();
            private bool _isStopped;
            private ISubscription _sourceSubscription;
            private CompositeSubscription _innerSubscriptions;
            private IOperatorContext _context;

            public _(SelectMany<TSource, TCollection, TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SelectMany";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _isStopped = false;

                _innerSubscriptions = new CompositeSubscription();
                _sourceSubscription = Params._source.Subscribe(this);

                return new ISubscription[] { _sourceSubscription, _innerSubscriptions };
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXINNERSUBCOUNTSETTING, out _maxInnerCount);

                _context = context;
            }

            public void OnCompleted()
            {
                StateChanged = true;
                _isStopped = true;
                lock (_lock)
                {
                    if (_innerSubscriptions.Count == 0)
                    {
                        Output.OnCompleted();
                        Dispose();
                    }
                    else
                    {
                        _sourceSubscription.Dispose();
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

            public void OnNext(TSource value)
            {
                lock (_lock)
                {
                    if (_innerSubscriptions.Count >= _maxInnerCount)
                    {
                        Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of concurrent nested sequences produced by the SelectMany operator exceeded {0} items. Please review the SelectMany operator usage to avoid exceeding this limit.", _maxInnerCount)));
                        Dispose();

                        return;
                    }
                }

                ISubscribable<TCollection> collection;

                try
                {
                    collection = Params._collectionSelector(value);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Observer will be owned by inner subscription.)

                    var observer = new Observer(this, value);

                    var subscription = SubscribeInner<TCollection>(collection, observer);
                    observer.Subscription = subscription;

                    SubscriptionInitializeVisitor.Initialize(subscription, _context);

                    lock (_lock)
                    {
                        // No need to set dirty flag here, the SubscribeInner call returns
                        // an operator with the dirty flag set to true.
                        _innerSubscriptions.Add(subscription);
                    }

#pragma warning restore CA2000
#pragma warning restore IDE0079
                }
                catch (Exception ex)
                {
                    lock (_lock)
                    {
                        Output.OnError(ex);
                        Dispose();
                        return;
                    }
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_isStopped);
                writer.Write(_innerSubscriptions.Count);

                foreach (var innerSubscription in _innerSubscriptions)
                {
                    SaveInner(innerSubscription, writer);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _isStopped = reader.Read<bool>();
                var subscriptionCount = reader.Read<int>();

                Debug.Assert(_innerSubscriptions != null);
                Debug.Assert(_innerSubscriptions.Count == 0);

                if (subscriptionCount > 0)
                {
                    for (int i = 0; i < subscriptionCount; ++i)
                    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Observer will be owned by inner subscription.)

                        var observer = new Observer(this);

                        var subscription = LoadInner<TCollection>(reader, observer);
                        observer.Subscription = subscription;
                        _innerSubscriptions.Add(subscription);

#pragma warning restore CA2000
#pragma warning restore IDE0079
                    }
                }
            }

            private sealed class Observer : StatefulObserver<TCollection>
            {
                private readonly _ _parent;
                private ISubscription _subscription;
                private TSource _value;

                public Observer(_ parent)
                {
                    _parent = parent;
                }

                public Observer(_ parent, TSource value) : this(parent)
                {
                    _value = value;
                }

                public override string Name => "rc:SelectMany/v";

                public override Version Version => Versioning.v1;

                public ISubscription Subscription
                {
                    set => _subscription = value;
                }

                protected override void OnCompletedCore()
                {
                    lock (_parent._lock)
                    {
                        _parent.StateChanged = true;
                        _parent._innerSubscriptions.Remove(_subscription);
                        _subscription.Dispose();
                        if (_parent._isStopped && _parent._innerSubscriptions.Count == 0)
                        {
                            _parent.Output.OnCompleted();
                            _parent.Dispose();
                        }
                    }
                }

                protected override void OnErrorCore(Exception error)
                {
                    lock (_parent._lock)
                    {
                        _parent.Output.OnError(error);
                        _parent.Dispose();
                    }
                }

                protected override void OnNextCore(TCollection value)
                {
                    TResult res;

                    try
                    {
                        res = _parent.Params._resultSelector(_value, value);
                    }
                    catch (Exception ex)
                    {
                        lock (_parent._lock)
                        {
                            _parent.Output.OnError(ex);
                            _parent.Dispose();
                            return;
                        }
                    }

                    lock (_parent._lock)
                    {
                        _parent.Output.OnNext(res);
                    }
                }

                protected override void SaveStateCore(IOperatorStateWriter writer)
                {
                    base.SaveStateCore(writer);

                    writer.Write(_value);
                }

                protected override void LoadStateCore(IOperatorStateReader reader)
                {
                    base.LoadStateCore(reader);

                    _value = reader.Read<TSource>();
                }
            }
        }
    }
}
