// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Retry<TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly int? _count;

        public Retry(ISubscribable<TResult> source)
        {
            Debug.Assert(source != null);

            _source = source;
        }

        public Retry(ISubscribable<TResult> source, int count)
        {
            Debug.Assert(source != null);
            Debug.Assert(count >= 0);

            _source = source;
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            if (_count == null)
            {
                return new i(this, observer);
            }
            else
            {
                return new _(this, observer);
            }
        }

        private sealed class _ : StatefulUnaryOperator<Retry<TResult>, TResult>, IObserver<TResult>
        {
            private readonly SerialSubscription _subscription = new();
            private int _retryCount;
            private IOperatorContext _context;

            public _(Retry<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
                _retryCount = parent._count.Value;
            }

            public override string Name => "rc:Retry+Count";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                if (_retryCount <= 1)
                {
                    Output.OnError(error);
                    Dispose();
                }
                else
                {
                    _retryCount--;
                    StateChanged = true;

                    _context.TraceSource.Retry_Retrying_Count(_context.InstanceId, _retryCount, error);

                    var subscription = default(ISubscription);
                    try
                    {
                        subscription = Params._source.Subscribe(this);
                        _subscription.Subscription = subscription;

                        SubscriptionInitializeVisitor.Initialize(subscription, _context);
                        _context.TraceSource.Retry_Retrying(_context.InstanceId, error);
                    }
                    catch (Exception ex)
                    {
                        subscription?.Dispose();

                        Output.OnError(ex);
                        Dispose();
                        return;
                    }
                }
            }

            public void OnNext(TResult value)
            {
                Output.OnNext(value);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_retryCount);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _retryCount = reader.Read<int>();

                if (IsDisposed)
                {
                    _subscription.Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                _subscription.Subscription = Params._source.Subscribe(this);
                return _subscription;
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }
        }

        private sealed class i : StatefulUnaryOperator<Retry<TResult>, TResult>, IObserver<TResult>
        {
            private readonly SerialSubscription _subscription = new();
            private IOperatorContext _context;

            public i(Retry<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Retry";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                _context.TraceSource.Retry_Retrying(_context.InstanceId, error);

                var subscription = default(ISubscription);
                try
                {
                    subscription = Params._source.Subscribe(this);
                    _subscription.Subscription = subscription;

                    SubscriptionInitializeVisitor.Initialize(subscription, _context);
                }
                catch (Exception ex)
                {
                    subscription?.Dispose();

                    Output.OnError(ex);
                    Dispose();
                    return;
                }
            }

            public void OnNext(TResult value)
            {
                Output.OnNext(value);
            }

            protected override ISubscription OnSubscribe()
            {
                _subscription.Subscription = Params._source.Subscribe(this);
                return _subscription;
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                if (IsDisposed)
                {
                    _subscription.Dispose();
                }
            }
        }
    }
}
