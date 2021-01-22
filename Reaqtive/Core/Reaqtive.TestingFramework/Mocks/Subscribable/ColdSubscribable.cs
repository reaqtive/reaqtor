// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    internal class ColdSubscribable<T> : MockSubscribable<T>
    {
        public ColdSubscribable(TestScheduler scheduler, params Recorded<Notification<T>>[] actions)
            : base(scheduler)
        {
            ObserverMessages = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public override IList<Recorded<Notification<T>>> ObserverMessages { get; }

        public override ISubscription Subscribe(IObserver<T> observer) => new _(this, base.Subscribe(observer));

        private sealed class _ : StatefulUnaryOperator<ColdSubscribable<T>, T>
        {
            private readonly ColdSubscribable<T> _parent;
            private readonly ISubscription _sub;
            private int _currentItem;
            private TestScheduler _scheduler;

            public _(ColdSubscribable<T> parent, ISubscription sub) : base(parent, parent.TheObserver)
            {
                _parent = parent;
                _sub = sub;
                _currentItem = 0;
            }

            public override string Name => "rct:ColdSubscribable";

            public override Version Version => new(1, 0, 0, 0);

            protected override void OnDispose()
            {
                _sub.Dispose();
            }

            protected override void OnStart()
            {
                for (int i = _currentItem; i < _parent.ObserverMessages.Count; ++i)
                {
                    var notification = _parent.ObserverMessages[i].Value;
                    _scheduler.ScheduleRelative(_parent.ObserverMessages[i].Time,
                        () =>
                        {
                            notification.Accept(_parent.TheObserver);
                            _currentItem++;
                            StateChanged = true;
                        });
                }
            }

            public override void SetContext(IOperatorContext context)
            {
                // Throws if not a test scheduler is used.
                _scheduler = (TestScheduler)context.Scheduler;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                _currentItem = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                writer.Write(_currentItem);
            }
        }
    }
}
