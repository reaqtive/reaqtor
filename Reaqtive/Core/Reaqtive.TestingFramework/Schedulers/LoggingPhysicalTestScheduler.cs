// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    internal sealed class LoggingPhysicalTestScheduler : PhysicalTestScheduler
    {
        private readonly List<long> _scheduledTimes;

        public LoggingPhysicalTestScheduler()
        {
            _scheduledTimes = new List<long>();
        }

        public IEnumerable<long> ScheduledTimes => _scheduledTimes;

        public override IWorkItem<long> ScheduleAbsolute(long dueTime, ISchedulerTask task, IScheduler scheduler)
        {
            _scheduledTimes.Add(dueTime);
            return base.ScheduleAbsolute(dueTime, task, scheduler);
        }
    }
}
