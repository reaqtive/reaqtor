// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    //
    // CONSIDER: Implementation of this interface on types implementing ISchedulerTask can be useful
    //           to account execution for these as well. When doing so, we may need to extend the
    //           interface a bit to also capture lifetime (equivalent to uptime), paused time, and
    //           maybe more. For now, this interface is mostly usable for LogicalScheduler.
    //

    /// <summary>
    /// Interface representing an object that supports being charged as part of an accounting
    /// operation. This interface is typically implemented by scheduling entities such as
    /// schedulers and tasks.
    /// </summary>
    internal interface IAccountable
    {
        /// <summary>
        /// Charges the cost of executing a task.
        /// </summary>
        /// <param name="cycleTime">The number of CPU clock cycles used to execute the task.</param>
        /// <param name="ticks">The number of 100ns ticks used to execute the task.</param>
        void ChargeTaskExecution(ulong cycleTime, long ticks);

        /// <summary>
        /// Charges a timer tick upon expiration of a timer.
        /// </summary>
        void ChargeTimerTick();
    }
}
