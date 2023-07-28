// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive.Scheduler
{
    using static HashHelpers;
    using static TimingHelpers;

    /// <summary>
    /// Representation of a duration using thread cycle time and wall clock time.
    /// </summary>
    public struct Duration : IEquatable<Duration>
    {
        //
        // NB: This is an *internally* mutable struct, allowing the scheduler infrastructure to perform efficient
        //     updates to the fields. Upon reporting the value to a caller, a copy is made which will effectively
        //     be immutable because no public setters are made available.
        //

        /// <summary>
        /// The thread cycle time, measured using the Win32 <c>QueryThreadCycleTime</c> function.
        /// </summary>
        internal long _cycleTime;

        /// <summary>
        /// The number of <see cref="Stopwatch.ElapsedTicks"/> spent.
        /// </summary>
        internal long _tickCount;

        /// <summary>
        /// Gets the thread cycle time spent.
        /// </summary>
        /// <remarks>
        /// This value is computed using the Win32 <c>QueryThreadCycleTime</c> function and includes cycles spent
        /// in user mode and in kernel mode. The measurement excludes cycles when the thread wasn't running, e.g.
        /// due to blocking or context switching. The value returned can only be used for relative measurements
        /// and comparisons with other scheduling entities. It cannot be converted to wall clock time.
        /// </remarks>
        public long CycleTime
        {
            get => Volatile.Read(ref _cycleTime);
            private set => Volatile.Write(ref _cycleTime, value);
        }

        /// <summary>
        /// Gets the elapsed wall clock time in ticks.
        /// </summary>
        public long TickCount
        {
            get => Volatile.Read(ref _tickCount);
            private set => Volatile.Write(ref _tickCount, value);
        }

        /// <summary>
        /// Gets the elapsed wall clock time.
        /// </summary>
        /// <remarks>
        /// This value is computed using the <see cref="Stopwatch"/> type and reports the total wall clock time
        /// spent on executing work. The measurement includes time when the thread wasn't running, e.g. due to
        /// blocking or context switching. While the value returned can be used to account for absolute values of
        /// wall clock time spent by a scheduler, it's recommended to use relative measurements and comparisons
        /// with other equal priority scheduling entities in order to distribute the cost of context switching
        /// and blocking which may be induced by the runtime environment.
        /// </remarks>
        public TimeSpan ElapsedTime => ToTimeSpan(TickCount);

        /// <summary>
        /// Checks whether the specified durations are equal.
        /// </summary>
        /// <param name="left">The first duration to compare.</param>
        /// <param name="right">The second duration to compare.</param>
        /// <returns><c>true</c> if the durations are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Duration left, Duration right) => left.Equals(right);

        /// <summary>
        /// Checks whether the specified durations are not equal.
        /// </summary>
        /// <param name="left">The first duration to compare.</param>
        /// <param name="right">The second duration to compare.</param>
        /// <returns><c>true</c> if the durations are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Duration left, Duration right) => !left.Equals(right);

        /// <summary>
        /// Adds two durations using pairwise addition of timing values.
        /// </summary>
        /// <param name="left">The first duration to add.</param>
        /// <param name="right">The second duration to add.</param>
        /// <returns>A duration representing the sum of the given durations.</returns>
        public static Duration operator +(Duration left, Duration right) => new()
        {
            CycleTime = left.CycleTime + right.CycleTime,
            TickCount = left.TickCount + right.TickCount,
        };

        /// <summary>
        /// Returns a <see cref="Duration"/> whose value represents the sum of the specified <paramref name="duration"/> and the current value.
        /// </summary>
        /// <param name="duration">The duration to add.</param>
        /// <returns>A duration representing the sum of the current duration and the specified <paramref name="duration"/>.</returns>
        public readonly Duration Add(Duration duration) => this + duration;

        /// <summary>
        /// Subtracts two durations using pairwise subtraction of timing values.
        /// </summary>
        /// <param name="left">The first duration to subtract.</param>
        /// <param name="right">The second duration to subtract.</param>
        /// <returns>A duration representing the subtraction of the given durations.</returns>
        public static Duration operator -(Duration left, Duration right) => new()
        {
            CycleTime = left.CycleTime - right.CycleTime,
            TickCount = left.TickCount - right.TickCount,
        };

        /// <summary>
        /// Returns a <see cref="Duration"/> whose value represents the subtraction of the specified <paramref name="duration"/> and the current value.
        /// </summary>
        /// <param name="duration">The duration to substract.</param>
        /// <returns>A duration representing the subtraction of the current duration and the specified <paramref name="duration"/>.</returns>
        public readonly Duration Subtract(Duration duration) => this - duration;

        /// <summary>
        /// Checks whether the current duration and the specified duration are equal.
        /// </summary>
        /// <param name="other">The duration to check the current duration against for equality.</param>
        /// <returns><c>true</c> if this duration is equal to the specified duration; otherwise, <c>false</c>.</returns>
        public bool Equals(Duration other) =>
            CycleTime == other.CycleTime &&
            TickCount == other.TickCount;

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
            TickCount.GetHashCode(),
            CycleTime.GetHashCode()
        );
    }

}
