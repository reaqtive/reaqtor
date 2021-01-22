// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents the status of a logical scheduler.
    /// </summary>
    public enum SchedulerStatus
    {
        /// <summary>
        /// The scheduler is running.
        /// </summary>
        Running,

        /// <summary>
        /// The scheduler is in the process of pausing.
        /// </summary>
        Pausing,

        /// <summary>
        /// The sheduler has been paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The scheduler has been disposed.
        /// </summary>
        Disposed,
    }
}
