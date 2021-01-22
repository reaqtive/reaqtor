// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Scheduler
{
    using static HashHelpers;
    using static TimingHelpers;

    /// <summary>
    /// Represents performance counters retrieved from a scheduler.
    /// </summary>
    public struct SchedulerPerformanceCounters : IEquatable<SchedulerPerformanceCounters>
    {
        //
        // CONSIDER: The following counters may make sense to add in future iterations.
        //
        //           - The number of [tasks|timers] currently in the scheduler.
        //           - The count of the number of [tasks|timers] that have been scheduled during the scheduler lifetime.
        //           - The total number of pause operations.
        //           - The number of exceptions thrown.
        //           - The number of unhandled exceptions.
        //

        //
        // NB: This is an *internally* mutable struct, allowing the scheduler instructure to perform efficient
        //     updates to the fields. Upon reporting the value to a caller, a copy is made which will effectively
        //     be immutable because no public setters are made available.
        //

        /// <summary>
        /// The number of <see cref="Stopwatch.ElapsedTicks"/> representing the uptime of the scheduler.
        /// </summary>
        internal long _uptime;

        /// <summary>
        /// The cumulative number of <see cref="Stopwatch.ElapsedTicks"/> while the scheduler was paused.
        /// </summary>
        internal long _pausedTime;

        /// <summary>
        /// The cumulative duration spent executing user work.
        /// </summary>
        internal Duration _userTime;

        /// <summary>
        /// The cumulative duration spent performing scheduler infrastructure work.
        /// </summary>
        internal Duration _kernelTime;

        /// <summary>
        /// The total number of calls to <see cref="ISchedulerTask.Execute(IScheduler)"/>.
        /// </summary>
        internal long _taskExecutionCount;

        /// <summary>
        /// The total number of times a time-based task was moved to the ready queue.
        /// </summary>
        internal long _timerTickCount;

        /// <summary>
        /// Gets the total uptime of the scheduler.
        /// </summary>
        public TimeSpan Uptime => ToTimeSpan(_uptime);

        /// <summary>
        /// Gets the total time the scheduler was in a paused state.
        /// </summary>
        public TimeSpan PausedTime => ToTimeSpan(_pausedTime);

        /// <summary>
        /// Gets the total time spent on running user work.
        /// </summary>
        public Duration UserTime => _userTime;

        /// <summary>
        /// Gets the total time spent performing scheduler infrastructure work.
        /// </summary>
        public Duration KernelTime => _kernelTime;

        /// <summary>
        /// Gets the total number of task executions that have been performed by the scheduler.
        /// </summary>
        /// <remarks>
        /// This value represents the total number of tasks executions that have been performed, including the
        /// execution of timers, and repeated executions of the same tasks in case of recurring tasks. This
        /// number can be used as a proxy to the amount of work running on the scheduler, provided the distribution
        /// of the granularity of work is relatively uniform.
        /// </remarks>
        public long TaskExecutionCount => _taskExecutionCount;

        /// <summary>
        /// Gets the total number of timer ticks that have been invoked by the scheduler.
        /// </summary>
        /// <remarks>
        /// This value represents the total number of expired timer tasks that were made runnable by the scheduler,
        /// including repeated expirations for periodic timer tasks. This number can be used as a proxy to the
        /// amount of time-based task execution activity running on the scheduler.
        /// </remarks>
        public long TimerTickCount => _timerTickCount;

        /// <summary>
        /// Checks whether the specified counters are equal.
        /// </summary>
        /// <param name="left">The first counter to compare.</param>
        /// <param name="right">The second counter to compare.</param>
        /// <returns><c>true</c> if the counters are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(SchedulerPerformanceCounters left, SchedulerPerformanceCounters right) => left.Equals(right);

        /// <summary>
        /// Checks whether the specified counters are not equal.
        /// </summary>
        /// <param name="left">The first counter to compare.</param>
        /// <param name="right">The second counter to compare.</param>
        /// <returns><c>true</c> if the counters are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(SchedulerPerformanceCounters left, SchedulerPerformanceCounters right) => !left.Equals(right);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable format // (Formatted as a table.)

        /// <summary>
        /// Adds two performance counter instances using pairwise addition of counter values.
        /// </summary>
        /// <param name="left">The left performance counter to add.</param>
        /// <param name="right">The right performance counter to add.</param>
        /// <returns>A performance counter object representing the sum of the given performance counters.</returns>
        public static SchedulerPerformanceCounters operator +(SchedulerPerformanceCounters left, SchedulerPerformanceCounters right) => new()
        {
            _uptime             = left._uptime             + right._uptime            ,
            _pausedTime         = left._pausedTime         + right._pausedTime        ,
            _kernelTime         = left._kernelTime         + right._kernelTime        ,
            _userTime           = left._userTime           + right._userTime          ,
            _taskExecutionCount = left._taskExecutionCount + right._taskExecutionCount,
            _timerTickCount     = left._timerTickCount     + right._timerTickCount    ,
        };

        /// <summary>
        /// Returns a <see cref="SchedulerPerformanceCounters"/> whose value represents the sum of the specified <paramref name="counters"/> and the current value.
        /// </summary>
        /// <param name="counters">The performance counter to add.</param>
        /// <returns>A performance counter object representing the sum of the given performance counters.</returns>
        public SchedulerPerformanceCounters Add(SchedulerPerformanceCounters counters) => this + counters;

        /// <summary>
        /// Subtracts two performance counter instances using pairwise subtraction of counter values.
        /// </summary>
        /// <param name="left">The left performance counter to subtract.</param>
        /// <param name="right">The right performance counter to subtract.</param>
        /// <returns>A performance counter object representing the subtraction of the given performance counters.</returns>
        public static SchedulerPerformanceCounters operator -(SchedulerPerformanceCounters left, SchedulerPerformanceCounters right) => new()
        {
            _uptime             = left._uptime             - right._uptime            ,
            _pausedTime         = left._pausedTime         - right._pausedTime        ,
            _kernelTime         = left._kernelTime         - right._kernelTime        ,
            _userTime           = left._userTime           - right._userTime          ,
            _taskExecutionCount = left._taskExecutionCount - right._taskExecutionCount,
            _timerTickCount     = left._timerTickCount     - right._timerTickCount    ,
        };

        /// <summary>
        /// Returns a <see cref="SchedulerPerformanceCounters"/> whose value represents the subtraction of the specified <paramref name="counters"/> and the current value.
        /// </summary>
        /// <param name="counters">The performance counter to subtract.</param>
        /// <returns>A performance counter object representing the subtraciton of the given performance counters.</returns>
        public SchedulerPerformanceCounters Subtract(SchedulerPerformanceCounters counters) => this - counters;

        /// <summary>
        /// Checks whether the current counters and the specified counters are equal.
        /// </summary>
        /// <param name="other">The counters to check the current counters against for equality.</param>
        /// <returns><c>true</c> if this counters instance is equal to the specified counters instance; otherwise, <c>false</c>.</returns>
        public bool Equals(SchedulerPerformanceCounters other) =>
            _uptime             == other._uptime             &&
            _pausedTime         == other._pausedTime         &&
            _kernelTime         == other._kernelTime         &&
            _userTime           == other._userTime           &&
            _taskExecutionCount == other._taskExecutionCount &&
            _timerTickCount     == other._timerTickCount     ;

        /// <summary>
        /// Checks whether the current object and the specified object are equal.
        /// </summary>
        /// <param name="obj">The object to check the current object against for equality.</param>
        /// <returns><c>true</c> if this object is equal to the specified object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is Duration duration && Equals(duration);

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Combine(
            _uptime            .GetHashCode(),
            _pausedTime        .GetHashCode(),
            _kernelTime        .GetHashCode(),
            _userTime          .GetHashCode(),
            _taskExecutionCount.GetHashCode(),
            _timerTickCount    .GetHashCode()
        );

#pragma warning restore format
#pragma warning restore IDE0079
    }
}
