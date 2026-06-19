// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reaqtive.Operators
{
    internal sealed class SequenceEqual<TSource> : SubscribableBase<bool>
    {
        private readonly ISubscribable<TSource> _left;
        private readonly ISubscribable<TSource> _right;
        private readonly IEqualityComparer<TSource> _comparer;

        public SequenceEqual(ISubscribable<TSource> left, ISubscribable<TSource> right, IEqualityComparer<TSource> comparer)
        {
            _left = left;
            _right = right;
            _comparer = comparer;
        }

        protected override ISubscription SubscribeCore(IObserver<bool> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulOperator<SequenceEqual<TSource>, bool>
        {
            private const string MAXQUEUESIZESETTING = "rx://operators/sequenceEqual/settings/maxQueueSize";
            private int _maxQueueSize;

            private readonly object _gate = new();
            private O _l;
            private O _r;

            public _(SequenceEqual<TSource> parent, IObserver<bool> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SequenceEqual";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXQUEUESIZESETTING, out _maxQueueSize);
            }

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _l = new L(this);
                _r = new R(this);

                _l.Other = _r;
                _r.Other = _l;

                return new[]
                {
                    _l.Subscription = Params._left.Subscribe(_l),
                    _r.Subscription = Params._right.Subscribe(_r)
                };
            }

            protected override void OnStart()
            {
                base.OnStart();

                _l.OnStart();
                _r.OnStart();
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _l.LoadState(reader);
                _r.LoadState(reader);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _l.SaveState(writer);
                _r.SaveState(writer);
            }

            private abstract class O : IObserver<TSource>
            {
                protected readonly _ _parent;

                private Queue<TSource> _queue;
                private bool _done;

                public O(_ parent)
                {
                    _parent = parent;
                }

                public ISubscription Subscription { get; set; }
                public O Other { get; set; }

                public void OnStart()
                {
                    if (_queue == null)
                    {
                        _queue = new Queue<TSource>();
                        _parent.StateChanged = true;
                    }
                }

                public void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        _done = true;
                        _parent.StateChanged = true;

                        if (_queue.Count == 0)
                        {
                            if (Other._queue.Count > 0)
                            {
                                _parent.Output.OnNext(false);
                                _parent.Output.OnCompleted();
                                _parent.Dispose();
                            }
                            else if (Other._done)
                            {
                                _parent.Output.OnNext(true);
                                _parent.Output.OnCompleted();
                                _parent.Dispose();
                            }
                        }
                    }

                    Subscription.Dispose();
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent.Output.OnError(error);
                        _parent.Dispose();
                    }

                    Subscription.Dispose();
                }

                public void OnNext(TSource value)
                {
                    lock (_parent._gate)
                    {
                        if (Other._queue.Count > 0)
                        {
                            var other = Other._queue.Dequeue();
                            _parent.StateChanged = true;

                            bool equal;

                            try
                            {
                                equal = Equal(value, other);
                            }
                            catch (Exception ex)
                            {
                                _parent.Output.OnError(ex);
                                _parent.Dispose();
                                return;
                            }

                            if (!equal)
                            {
                                _parent.Output.OnNext(false);
                                _parent.Output.OnCompleted();
                                _parent.Dispose();
                            }
                        }
                        else if (Other._done)
                        {
                            _parent.Output.OnNext(false);
                            _parent.Output.OnCompleted();
                            _parent.Dispose();
                        }
                        else
                        {
                            if (_queue.Count >= _parent._maxQueueSize)
                            {
                                _parent.Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of elements accumulated by a comparison queue in the SequenceEqual operator exceeded {0} items. Please apply the SequenceEqual operator to sequences with comparable event rates.", _parent._maxQueueSize)));
                                _parent.Dispose();

                                return;
                            }

                            _queue.Enqueue(value);
                            _parent.StateChanged = true;
                        }
                    }
                }

                protected abstract bool Equal(TSource value, TSource other);

                public void LoadState(IOperatorStateReader reader)
                {
                    _done = reader.Read<bool>();

                    var n = reader.Read<int>();
                    _queue = new Queue<TSource>(n);

                    for (var i = 0; i < n; i++)
                    {
                        _queue.Enqueue(reader.Read<TSource>());
                    }
                }

                public void SaveState(IOperatorStateWriter writer)
                {
                    writer.Write<bool>(_done);

                    var n = _queue.Count;
                    writer.Write<int>(n);

                    foreach (var item in _queue)
                    {
                        writer.Write<TSource>(item);
                    }
                }
            }

            private class L : O
            {
                public L(_ parent)
                    : base(parent)
                {
                }

                protected override bool Equal(TSource value, TSource other)
                {
                    // Compat with Rx behavior: order is always left, right
                    return _parent.Params._comparer.Equals(value, other);
                }
            }

            private class R : O
            {
                public R(_ parent)
                    : base(parent)
                {
                }

                protected override bool Equal(TSource value, TSource other)
                {
                    // Compat with Rx behavior: order is always left, right
                    return _parent.Params._comparer.Equals(other, value);
                }
            }
        }
    }
}
