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
    /// <typeparam name="T">Type of the parameter passed to the task.</typeparam>
    public sealed class ActionTask<T> : ISchedulerTask
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<T> _action;

        /// <summary>
        /// The state to pass to the action.
        /// </summary>
        private readonly T _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="state">The state to pass to the action.</param>
        public ActionTask(Action<T> action, T state)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _state = state;
        }

        /// <summary>
        /// Gets task priority.
        /// </summary>
        public long Priority => ActionTask.TaskPriority;

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

            _action(_state);

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
