// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive.Scheduler;

namespace Reaqtive.Tasks
{
    /// <summary>
    /// Simple action task with yielding support.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the task.</typeparam>
    public sealed class YieldableActionTask<T> : ISchedulerTask, IYieldableSchedulerTask
    {
        /// <summary>
        /// The action to execute.
        /// </summary>
        private readonly Func<T, YieldToken, bool> _action;

        /// <summary>
        /// The state to pass to the action.
        /// </summary>
        private readonly T _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="state">The state to pass to the action.</param>
        /// <remarks>
        /// The return value of the <paramref name="action"/> indicates whether the task has been completed. If
        /// a value of <c>true</c> is returned, the task won't be scheduled again. If a value of <c>false</c> is
        /// returned, the task will be scheduled again. This is usable for tasks that support yielding and can
        /// interrupt their work for it to be resumed later.
        /// </remarks>
        public YieldableActionTask(Func<T, YieldToken, bool> action, T state)
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
        /// <returns><c>true</c> if the task has been completed; otherwise, <c>false</c>.</returns>
        public bool Execute(IScheduler scheduler)
        {
            Debug.Assert(scheduler != null, "Scheduler should not be null.");

            return _action(_state, default);
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="yieldToken">Token to observe for yield requests.</param>
        /// <returns><c>true</c> if the task has been completed; otherwise, <c>false</c>.</returns>
        public bool Execute(IScheduler scheduler, YieldToken yieldToken)
        {
            Debug.Assert(scheduler != null, "Scheduler should not be null.");

            return _action(_state, yieldToken);
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
