// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Interface for test schedulers.
    /// </summary>
    public interface ITestScheduler : IScheduler, IClockable<long>
    {
        /// <summary>
        /// Schedules a task to run at the specified absolute time.
        /// </summary>
        /// <param name="dueTime">The absolute due time.</param>
        /// <param name="task">The task to run.</param>
        void ScheduleAbsolute(long dueTime, ISchedulerTask task);

        /// <summary>
        /// Starts execution of scheduled actions.
        /// </summary>
        void Start();
    }
}
