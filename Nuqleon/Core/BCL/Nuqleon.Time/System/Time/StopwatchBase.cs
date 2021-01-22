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
    /// Base class for stopwatches.
    /// </summary>
    public abstract class StopwatchBase : IStopwatch
    {
        private long _elapsed;
        private long _startTimeStamp;

        /// <summary>
        /// Gets the total elapsed time measured by the current instance.
        /// </summary>
        public TimeSpan Elapsed => new(GetElapsedDateTimeTicks());

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        public long ElapsedMilliseconds => GetElapsedDateTimeTicks() / 10000L;

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in timer ticks.
        /// </summary>
        public long ElapsedTicks => GetRawElapsedTicks();

        /// <summary>
        /// Gets a value indicating whether the stopwatch timer is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        public void Reset()
        {
            _elapsed = 0L;
            IsRunning = false;
            _startTimeStamp = 0L;
        }

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        public void Restart()
        {
            _elapsed = 0L;
            _startTimeStamp = GetTimestamp();
            IsRunning = true;
        }

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                _startTimeStamp = GetTimestamp();
                IsRunning = true;
            }
        }

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                var delta = GetTimestamp() - _startTimeStamp;

                _elapsed += delta;
                IsRunning = false;

                if (_elapsed < 0L)
                {
                    _elapsed = 0L;
                }
            }
        }

        /// <summary>
        /// Gets the current number of ticks in the timer mechanism.
        /// </summary>
        /// <returns>A long integer representing the tick counter value of the underlying timer mechanism.</returns>
        public abstract long GetTimestamp();

        /// <summary>
        /// Gets a value indicating whether the stopwatch supports high resolution timing.
        /// </summary>
        protected abstract bool IsHighResolution { get; }

        /// <summary>
        /// Gets the tick frequency if the stopwatch supports high resolution timing.
        /// </summary>
        protected abstract double TickFrequency { get; }

        /// <summary>
        /// Gets the total elapsed time, including the time since the stopwatch was last started.
        /// </summary>
        /// <returns>The total elapsed time, including the time since the stopwatch was last started.</returns>
        protected long GetRawElapsedTicks()
        {
            var elapsed = _elapsed;

            if (IsRunning)
            {
                var delta = GetTimestamp() - _startTimeStamp;
                elapsed += delta;
            }

            return elapsed;
        }

        /// <summary>
        /// Gets the high-resolution ticks as DateTime.Ticks if the stopwatch is using a high-resolution counter.
        /// </summary>
        /// <returns>High-resolution ticks converted to DateTime.Ticks.</returns>
        protected virtual long GetElapsedDateTimeTicks()
        {
            var rawElapsedTicks = GetRawElapsedTicks();

            if (IsHighResolution)
            {
                return (long)(rawElapsedTicks * TickFrequency);
            }

            return rawElapsedTicks;
        }
    }
}
