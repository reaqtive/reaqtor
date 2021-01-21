// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Tasks;
using System;

namespace Reaqtive.Operators
{
    internal sealed class DelaySubscription<T> : SubscribableBase<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly TimeSpan? _relative;
        private readonly DateTimeOffset? _absolute;

        public DelaySubscription(ISubscribable<T> source, TimeSpan dueTime)
        {
            _source = source;
            _relative = dueTime;
        }

        public DelaySubscription(ISubscribable<T> source, DateTimeOffset dueTime)
        {
            _source = source;
            _absolute = dueTime;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            if (_relative.HasValue)
            {
                return new Relative(this, observer);
            }
            else
            {
                return new Absolute(this, observer);
            }
        }

        private abstract class _ : StatefulUnaryOperator<DelaySubscription<T>, T>, IObserver<T>
        {
            private readonly SingleAssignmentSubscription _subscription = new();
            private IOperatorContext _context;
            private bool _subscribed;

            public _(DelaySubscription<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public abstract DateTimeOffset SubscribeAt
            {
                get;
            }

            protected override ISubscription OnSubscribe()
            {
                return _subscription;
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void OnStart()
            {
                base.OnStart();

                if (!_subscribed)
                {
                    _context.Scheduler.Schedule(SubscribeAt, new ActionTask(DoSubscribeTask));
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _subscribed = reader.Read<bool>();
                if (_subscribed)
                {
                    DoSubscribeTask(true);
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_subscribed);
            }

            private void DoSubscribeTask()
            {
                DoSubscribeTask(false);
            }

            private void DoSubscribeTask(bool recovery)
            {
                _context.TraceSource.DelaySubscription_Subscribing(_context.InstanceId);

                var subscription = default(ISubscription);
                try
                {
                    subscription = Params._source.Subscribe(this);

                    if (recovery)
                    {
                        SubscriptionInitializeVisitor.Subscribe(subscription);
                        SubscriptionInitializeVisitor.SetContext(subscription, _context);
                    }
                    else
                    {
                        SubscriptionInitializeVisitor.Initialize(subscription, _context);
                    }

                    _subscription.Subscription = subscription;
                }
                catch (Exception ex)
                {
                    subscription?.Dispose();

                    _context.Scheduler.Schedule(new ActionTask(() =>
                    {
                        Output.OnError(ex);
                        Dispose();
                    }));

                    return;
                }

                _subscribed = true;
                StateChanged = true;
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

            public void OnNext(T value)
            {
                Output.OnNext(value);
            }
        }

        private sealed class Relative : _
        {
            private DateTimeOffset _calculated;

            public Relative(DelaySubscription<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rx:DelaySubscription+Relative";

            public override Version Version => Versioning.v1;

            public override DateTimeOffset SubscribeAt => _calculated;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _calculated = context.Scheduler.Now + Params._relative.Value;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _calculated = reader.Read<DateTimeOffset>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_calculated);
            }
        }

        private sealed class Absolute : _
        {
            public Absolute(DelaySubscription<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override DateTimeOffset SubscribeAt => Params._absolute.Value;

            public override string Name => "rx:DelaySubscription+Absolute";

            public override Version Version => Versioning.v1;
        }
    }
}
