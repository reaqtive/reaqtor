// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents a work item using <see cref="DateTimeOffset"/> to represent due time.
    /// </summary>
    internal sealed class WorkItem : WorkItemBase<DateTimeOffset>
    {
        private static readonly IDisposable s_nopDisposable = Disposable.Create(() => { });

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItem" /> class.
        /// </summary>
        /// <param name="scheduler">The logical scheduler that owns the work item.</param>
        /// <param name="task">The task to execute.</param>
        /// <param name="dueTime">The due time at which to execute the task.</param>
        public WorkItem(LogicalScheduler scheduler, ISchedulerTask task, DateTimeOffset dueTime)
            : base(scheduler, task, dueTime, s_nopDisposable)
        {
        }

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
