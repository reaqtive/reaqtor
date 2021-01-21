// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Interface representing a logical scheduler. Tasks scheduled through the same logical
    /// scheduler are managed as a group.
    /// </summary>
    public interface IScheduler : IDisposable
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// Creates a child scheduler.
        /// </summary>
        /// <returns>A child scheduler.</returns>
        IScheduler CreateChildScheduler();

        /// <summary>
        /// Schedules the specified task.
        /// </summary>
        /// <param name="task">The task to schedule.</param>
        void Schedule(ISchedulerTask task);

        /// <summary>
        /// Schedules the specified task at the specified relative due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="task">The task to schedule.</param>
        void Schedule(TimeSpan dueTime, ISchedulerTask task);

        /// <summary>
        /// Schedules the specified task at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="task">The task to schedule.</param>
        void Schedule(DateTimeOffset dueTime, ISchedulerTask task);

        /// <summary>
        /// Asynchronously pauses all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        /// <returns>A task representing the eventual completion of pausing all tasks on the scheduler.</returns>
        Task PauseAsync();

#pragma warning disable CA1716 // Identifiers should not match keywords. (Backwards compatibility. Resume isn't any better.)

        /// <summary>
        /// Continues all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        void Continue();

#pragma warning restore CA1716

        /// <summary>
        /// Recalculates the priority of all tasks in the scheduler.
        /// As the result of recalculation some tasks can become runnable/change priority.
        /// </summary>
        void RecalculatePriority();

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <returns><c>true</c> if the calling thread has access to the scheduler; otherwise, <c>false</c>.</returns>
        bool CheckAccess();

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this scheduler.</exception>
        void VerifyAccess();

        /// <summary>
        /// Event to observe unhandled exceptions and optionally mark them as handled.
        /// </summary>
        event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;
    }
}
