// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Interface representing a scheduler work item.
    /// </summary>
    /// <typeparam name="TTime">The type used to represent the due time.</typeparam>
    public interface IWorkItem<out TTime> : IWorkItem
        where TTime : IComparable<TTime>
    {
        /// <summary>
        /// Gets the due time when the work item should run.
        /// </summary>
        TTime DueTime { get; }
    }

    /// <summary>
    /// Interface representing a scheduler work item.
    /// </summary>
    public interface IWorkItem
    {
        /// <summary>
        /// Gets the item's priority.
        /// </summary>
        long Priority { get; }

        /// <summary>
        /// Gets the task to run.
        /// </summary>
        ISchedulerTask Task { get; }

        /// <summary>
        /// Gets the logical scheduler that owns the work item.
        /// </summary>
        IScheduler Scheduler { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is paused.
        /// </summary>
        bool IsPaused { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is runnable.
        /// </summary>
        bool IsRunnable { get; }

        /// <summary>
        /// Invokes the item.
        /// </summary>
        /// <returns><c>true</c> if the task has completed and should not be rescheduled; otherwise, <c>false</c>.</returns>
        bool Invoke();

        /// <summary>
        /// Recalculates the priority.
        /// </summary>
        void RecalculatePriority();
    }
}
