// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

using System.Diagnostics;

namespace System.Time
{
    /// <summary>
    /// Exposes commonly used stopwatch factories and methods to create stopwatch factories.
    /// </summary>
    public static class StopwatchFactory
    {
        /// <summary>
        /// Gets a factory for stopwatches that use the System.Diagnostics.Stopwatch type.
        /// </summary>
        public static IStopwatchFactory Diagnostics { get; } = new SystemTimeStopwatchFactory();

        /// <summary>
        /// Creates a stopwatch factory that uses the specified clock to measure time.
        /// </summary>
        /// <param name="clock">Clock used to measure time.</param>
        /// <returns>Stopwatch factory to create stopwatches based on the specified clock.</returns>
        public static IStopwatchFactory FromClock(IClock clock)
        {
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            return new ClockStopwatchFactory(clock);
        }

        private sealed class SystemTimeStopwatchFactory : IStopwatchFactory
        {
            public IStopwatch Create() => new Impl();

            private sealed class Impl : IStopwatch
            {
                private readonly Stopwatch _stopwatch;

                public Impl() => _stopwatch = new Stopwatch();

                public TimeSpan Elapsed => _stopwatch.Elapsed;

                public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

                public long ElapsedTicks => _stopwatch.ElapsedTicks;

                public bool IsRunning => _stopwatch.IsRunning;

                public void Reset() => _stopwatch.Reset();

                public void Restart() => _stopwatch.Restart();

                public void Start() => _stopwatch.Start();

                public void Stop() => _stopwatch.Stop();

                public long GetTimestamp() => Stopwatch.GetTimestamp();
            }
        }

        private sealed class ClockStopwatchFactory : IStopwatchFactory
        {
            private readonly IClock _clock;

            public ClockStopwatchFactory(IClock clock) => _clock = clock;

            public IStopwatch Create() => new Impl(_clock);

            private sealed class Impl : StopwatchBase
            {
                private readonly IClock _clock;

                public Impl(IClock clock) => _clock = clock;

                protected override bool IsHighResolution => false;

                protected override double TickFrequency => throw new NotSupportedException();

                public override long GetTimestamp() => _clock.Now;
            }
        }
    }
}
