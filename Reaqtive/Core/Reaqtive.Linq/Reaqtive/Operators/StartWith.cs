// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Reaqtive.Tasks;

namespace Reaqtive.Operators
{
    internal sealed class StartWith<T> : SubscribableBase<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly T[] _values;

        public StartWith(ISubscribable<T> source, params T[] values)
        {
            Debug.Assert(source != null);

            _source = source;
            _values = values;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<StartWith<T>, T>, IObserver<T>
        {
            private volatile int _valuesIndex;
            private SingleAssignmentSubscription _subscription;
            private IOperatorContext _context;

            public _(StartWith<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:StartWith";

            public override Version Version => Versioning.v1;

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

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _valuesIndex = reader.Read<int>();

                if (_valuesIndex >= Params._values.Length)
                {
                    DoSubscribeTask(true);
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_valuesIndex);
            }

            protected override ISubscription OnSubscribe()
            {
                _subscription = new SingleAssignmentSubscription();
                return _subscription;
            }

            public override void SetContext(IOperatorContext context)
            {
                _context = context;
            }

            protected override void OnStart()
            {
                if (!_subscription.IsDisposed && _subscription.Subscription == null)
                {
                    DoSchedule(GetNextTask());
                }
            }

            private void DoSchedule(ActionTask task)
            {
                _context.Scheduler.Schedule(task);

                _context.TraceSource.StartWith_Scheduling(_context.InstanceId, _valuesIndex, Params._values.Length);
            }

            private ActionTask GetNextTask()
            {
                return new ActionTask(() =>
                {
                    var values = Params._values;
                    var count = values.Length;

                    _context.TraceSource.StartWith_Processing(_context.InstanceId, _valuesIndex, count);

                    if (_valuesIndex < count)
                    {
                        StateChanged = true;

                        OnNext(values[_valuesIndex]);

                        // NOTE: be sure that no more tail-scheduled tasks are scheduled
                        // after the final value is pushed, otherwise it will call
                        // subscribe to the upstream observable more than once.
                        if (++_valuesIndex < count)
                        {
                            DoSchedule(GetNextTask());
                        }
                    }

                    if (_valuesIndex == count)
                    {
                        DoSubscribeTask();
                    }
                });
            }

            private void DoSubscribeTask(bool recovery = false)
            {
                _context.TraceSource.StartWith_Subscribing(_context.InstanceId);

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
                }
                catch (Exception ex)
                {
                    if (subscription != null)
                    {
                        subscription.Dispose();
                    }

                    DoSchedule(new ActionTask(() =>
                    {
                        Output.OnError(ex);
                        Dispose();
                    }));

                    return;
                }

                _subscription.Subscription = subscription;
            }
        }
    }
}
