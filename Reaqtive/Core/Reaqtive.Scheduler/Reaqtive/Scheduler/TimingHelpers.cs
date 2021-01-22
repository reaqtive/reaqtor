// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Provides a set of helper methods to perform and work with timings.
    /// </summary>
    internal static class TimingHelpers
    {
        /// <summary>
        /// The number of 100ns ticks (used for <see cref="TimeSpan"/> elapsed ticks) per <see cref="Stopwatch"/> tick.
        /// </summary>
        private static readonly double s_stopwatchTickFrequency =
            Stopwatch.IsHighResolution
            ? 10_000_000.0 /* 100 ns/s */ / Stopwatch.Frequency /* ticks/s */
            : 1.0;

        /// <summary>
        /// Converts the number of elapsed ticks measured by a <see cref="Stopwatch"/> instance to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="rawElapsedTicks">The number of elapsed ticks according to <see cref="Stopwatch.ElapsedTicks"/>.</param>
        /// <returns>The <see cref="TimeSpan"/> value.</returns>
        public static TimeSpan ToTimeSpan(long rawElapsedTicks) => TimeSpan.FromTicks(GetElapsedDateTimeTicks(rawElapsedTicks));

        /// <summary>
        /// Converts the number of elapsed ticks measured by a <see cref="Stopwatch"/> instance to 100ns ticks
        /// used by <see cref="TimeSpan"/> and <see cref="DateTime"/>.
        /// </summary>
        /// <param name="rawElapsedTicks">The number of elapsed ticks according to <see cref="Stopwatch.ElapsedTicks"/>.</param>
        /// <returns>The number of 100ns ticks for use by <see cref="TimeSpan"/> or <see cref="DateTime"/>.</returns>
        private static long GetElapsedDateTimeTicks(long rawElapsedTicks)
        {
            if (Stopwatch.IsHighResolution)
            {
                return (long)(rawElapsedTicks * s_stopwatchTickFrequency);
            }

            return rawElapsedTicks;
        }
    }
}
