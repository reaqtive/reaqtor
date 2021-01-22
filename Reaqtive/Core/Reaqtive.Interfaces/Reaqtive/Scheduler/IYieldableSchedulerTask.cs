// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    // NB: This interface was introduced to preserve backwards compatibility for `ISchedulerTask`
    //     where we can't simply add another method.

    /// <summary>
    /// Scheduler task with yield support.
    /// </summary>
    public interface IYieldableSchedulerTask : ISchedulerTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="yieldToken">Token to observe for yield requests.</param>
        /// <returns><c>true</c> if the task has been completed; otherwise, <c>false</c>.</returns>
        bool Execute(IScheduler scheduler, YieldToken yieldToken);
    }
}
