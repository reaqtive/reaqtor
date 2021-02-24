// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    internal class HotSubscribable<T> : MockSubscribable<T>
    {
        public HotSubscribable(TestScheduler scheduler, params Recorded<Notification<T>>[] actions)
            : base(scheduler)
        {
            ObserverMessages = actions ?? throw new ArgumentNullException(nameof(actions));

            foreach (Recorded<Notification<T>> action in actions)
            {
                var notification = action.Value;
                scheduler.ScheduleAbsolute(
                    default(object),
                    action.Time,
                    (scheduler1, state1) => notification.Accept(TheObserver));
            }
        }

        public override IList<Recorded<Notification<T>>> ObserverMessages { get; }
    }
}
