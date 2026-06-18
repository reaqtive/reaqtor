// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents a work item using <see cref="DateTimeOffset"/> to represent due time.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="WorkItem" /> class.
    /// </remarks>
    /// <param name="scheduler">The logical scheduler that owns the work item.</param>
    /// <param name="task">The task to execute.</param>
    /// <param name="dueTime">The due time at which to execute the task.</param>
    internal sealed class WorkItem(LogicalScheduler scheduler, ISchedulerTask task, DateTimeOffset dueTime) : WorkItemBase<DateTimeOffset>(scheduler, task, dueTime, s_nopDisposable)
    {
        private static readonly IDisposable s_nopDisposable = Disposable.Create(() => { });

        /// <summary>
        /// Gets or sets the worker.
        /// </summary>
        public Worker Worker { get; set; }

        /// <summary>
        /// Called when the work item completes.
        /// </summary>
        protected override void OnCompleted()
        {
            base.OnCompleted();

            ((LogicalScheduler)Scheduler).Remove(this);
        }
    }
}
