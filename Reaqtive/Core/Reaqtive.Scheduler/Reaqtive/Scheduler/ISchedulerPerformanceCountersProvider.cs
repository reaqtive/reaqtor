// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Interface implemented by schedulers that support reporting performance counters.
    /// </summary>
    public interface ISchedulerPerformanceCountersProvider
    {
        /// <summary>
        /// Queries the current value of the performance counters.
        /// </summary>
        /// <param name="includeChildren">
        /// <c>true</c> to aggregate counters of the current object and all children;
        /// <c>false</c> to just return the counters for the current object.
        /// </param>
        /// <returns>The current value of the performance counters.</returns>
        SchedulerPerformanceCounters QueryPerformanceCounters(bool includeChildren);
    }
}
