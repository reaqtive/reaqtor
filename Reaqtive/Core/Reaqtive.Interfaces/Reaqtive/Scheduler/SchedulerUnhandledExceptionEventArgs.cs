// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Provides data for the UnhandledException event on IScheduler.
    /// </summary>
    public sealed class SchedulerUnhandledExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new scheduler unhandled exception event argument object using the specified scheduler and exception.
        /// </summary>
        /// <param name="scheduler">Scheduler associated with this event.</param>
        /// <param name="exception">Exception that was raised when executing code on the scheduler.</param>
        public SchedulerUnhandledExceptionEventArgs(IScheduler scheduler, Exception exception)
        {
            Scheduler = scheduler;
            Exception = exception;
        }

        /// <summary>
        /// Gets the scheduler associated with this event.
        /// </summary>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Gets the exception that was raised when executing code on the scheduler.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets or sets whether the exception event has been handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
