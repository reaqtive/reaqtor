// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    internal class ColdSubscribable<T>(TestScheduler scheduler, params Recorded<Notification<T>>[] actions) : MockSubscribable<T>(scheduler)
    {
        public override IList<Recorded<Notification<T>>> ObserverMessages { get; } = actions ?? throw new ArgumentNullException(nameof(actions));

        public override ISubscription Subscribe(IObserver<T> observer) => new _(this, base.Subscribe(observer));

        private sealed class _(ColdSubscribable<T> parent, ISubscription sub) : StatefulUnaryOperator<ColdSubscribable<T>, T>(parent, parent.TheObserver)
        {
            private readonly ColdSubscribable<T> _parent = parent;
            private readonly ISubscription _sub = sub;
            private int _currentItem = 0;
            private TestScheduler _scheduler;

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
