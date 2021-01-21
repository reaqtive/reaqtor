// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    // NB: This interface was introduced to preserve backwards compatibility for `IScheduler`
    //     where we can't simply add another property.

    /// <summary>
    /// Interface representing a logical scheduler exposing scheduler status.
    /// </summary>
    internal interface ISchedulerStatus : IScheduler
    {
        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        SchedulerStatus Status { get; }
    }
}
