// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Interface for schedulers that support handling exceptions, either by using a built-in policy or by some user interaction.
    /// </summary>
    public interface ISchedulerExceptionHandler
    {
        /// <summary>
        /// Tries to handle an exception that was thrown by a work item running on the scheduler.
        /// </summary>
        /// <param name="exception">Exception to handle.</param>
        /// <param name="task">Task that threw the exception.</param>
        /// <returns>true if the exception was handled; otherwise, false.</returns>
        bool TryCatch(Exception exception, IWorkItem task);
    }
}
