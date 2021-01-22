// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    internal sealed class ApplySubscribable<T> : MockSubscribable<T>
    {
        private readonly Recorded<SubscriptionAction>[] _actions;
        private readonly ISubscribable<T> _source;

        public ApplySubscribable(ISubscribable<T> source, TestScheduler scheduler, params Recorded<SubscriptionAction>[] actions)
            : base(scheduler)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _actions = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public override ISubscription Subscribe(IObserver<T> observer) => new _(this, base.Subscribe(observer));

        public override IList<Recorded<Notification<T>>> ObserverMessages => throw new NotImplementedException();

        private sealed class _ : ISubscription, IObserver<T>, IOperator
        {
            private readonly ApplySubscribable<T> _parent;
            private readonly ISubscription _sub;
            private readonly ISubscription _source;
            private TestScheduler _scheduler;

            public _(ApplySubscribable<T> parent, ISubscription sub)
            {
                _parent = parent;
                _source = _parent._source.Subscribe(this);
                _sub = sub;
            }

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

            public void Dispose()
            {
                _source.Dispose();
                _sub.Dispose();
            }

            public void OnCompleted()
            {
                _parent.TheObserver.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                _parent.TheObserver.OnError(error);
                Dispose();
            }

            public void OnNext(T value)
            {
                _parent.TheObserver.OnNext(value);
            }

            public IEnumerable<ISubscription> Inputs => new[] { _source };

            public void Subscribe()
            {
            }

            public void SetContext(IOperatorContext context)
            {
                // Throws if scheduler is not testable.
                _scheduler = (TestScheduler)context.Scheduler;
            }

            public void Start()
            {
                foreach (var action in _parent._actions)
                {
                    _scheduler.ScheduleAbsolute(action.Time, () => action.Value.Accept(_source));
                }
            }
        }
    }
}
