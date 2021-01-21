// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Interface for schedulers that keep a log of scheduled actions.
    /// </summary>
    /// <typeparam name="TAbsolute">Type of the absolute time values used to schedule actions.</typeparam>
    public interface ILoggingScheduler<TAbsolute> : IScheduler
    {
        /// <summary>
        /// Gets a sequence of times at which actions are scheduled.
        /// </summary>
        IEnumerable<TAbsolute> ScheduledTimes { get; }
    }
}
