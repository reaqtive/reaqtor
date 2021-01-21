// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive.Scheduler;

namespace Reaqtive.Tasks
{
    /// <summary>
    /// Simple action task.
    /// </summary>
    public sealed class ActionTask : ISchedulerTask
    {
        /// <summary>
        /// The action tasks have highest priority.
        /// </summary>
        internal const int TaskPriority = 1;

        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionTask(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Gets task priority.
        /// </summary>
        public long Priority => TaskPriority;

        /// <summary>
        /// Gets a value indicating whether the task is runnable.
        /// </summary>
        public bool IsRunnable => true;

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>
        /// True if the task has been completed.
        /// </returns>
        public bool Execute(IScheduler scheduler)
        {
            Debug.Assert(scheduler != null, "Scheduler should not be null.");

            _action();

            return true;
        }

        /// <summary>
        /// Recalculates the priority of the task. The task can become runnable
        /// as the result of this operation.
        /// </summary>
        public void RecalculatePriority()
        {
        }
    }
}
