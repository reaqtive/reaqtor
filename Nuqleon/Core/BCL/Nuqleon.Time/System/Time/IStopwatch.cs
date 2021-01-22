// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

namespace System.Time
{
    /// <summary>
    /// Interface for stopwatches.
    /// </summary>
    public interface IStopwatch
    {
        /// <summary>
        /// Gets a value indicating whether the stopwatch timer is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in timer ticks.
        /// </summary>
        long ElapsedTicks { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        long ElapsedMilliseconds { get; }

        /// <summary>
        /// Gets the total elapsed time measured by the current instance.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        void Start();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword. (Stop is an API on Stopwatch.)

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        void Stop();

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        void Reset();

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        void Restart();

        /// <summary>
        /// Gets the current number of ticks in the timer mechanism.
        /// </summary>
        /// <returns>A long integer representing the tick counter value of the underlying timer mechanism.</returns>
        long GetTimestamp();
    }
}
