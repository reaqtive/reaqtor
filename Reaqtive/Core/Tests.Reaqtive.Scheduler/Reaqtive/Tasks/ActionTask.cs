// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive.Scheduler;

namespace Test.Reaqtive.Scheduler.Tasks
{
    internal class ActionTask : ISchedulerTask
    {
        private readonly Func<IScheduler, bool> _action;

        public static ActionTask Create(Func<IScheduler, bool> action, long priority)
        {
            return new ActionTask(action, priority);
        }

        private ActionTask(Func<IScheduler, bool> action, long priority)
        {
            _action = action;
            Priority = priority;
            IsRunnable = true;
        }

        public long Priority { get; }

        public bool IsRunnable { get; set; }

        public bool Execute(IScheduler scheduler)
        {
            return _action(scheduler);
        }

        public void RecalculatePriority()
        {
        }
    }
}
