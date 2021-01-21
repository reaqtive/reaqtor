// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Scheduler task.
    /// </summary>
    public interface ISchedulerTask
    {
        /// <summary>
        /// Gets the task priority.
        /// </summary>
        long Priority { get; }

        /// <summary>
        /// Gets a value indicating whether the task is runnable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is runnable; otherwise, <c>false</c>.
        /// </value>
        bool IsRunnable { get; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns><c>true</c> if the task has been completed; otherwise, <c>false</c>.</returns>
        bool Execute(IScheduler scheduler);

        /// <summary>
        /// Recalculates the priority of the task. The task can become runnable
        /// as the result of this operation.
        /// </summary>
        void RecalculatePriority();
    }
}
