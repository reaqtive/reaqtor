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
    /// Provides a set of extension methods for clocks.
    /// </summary>
    public static class ClockExtensions
    {
        /// <summary>
        /// Asserts that the specified clock is monotonic. When the resulting clock's time is read and it is less than the previous time reading, an exception is thrown.
        /// </summary>
        /// <param name="clock">The clock to assert monotonicity for.</param>
        /// <returns>A clock instance that asserts monotonicity of the specified clock.</returns>
        public static IClock AssertMonotonic(this IClock clock)
        {
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            //
            // NB: Some optimizations could be made to coalesce various [Assert|Ensure]Monotonic applications.
            //
            return new Monotonic(clock, adjust: false);
        }

        /// <summary>
        /// Ensures that the specified clock is monotonic. When the resulting clock's time is read and it is less than the previous time reading, the previous reading is used.
        /// </summary>
        /// <param name="clock">The clock to ensure monotonicity for.</param>
        /// <returns>A clock instance that ensure monotonicity of the specified clock.</returns>
        public static IClock EnsureMonotonic(this IClock clock)
        {
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            //
            // NB: Some optimizations could be made to coalesce various [Assert|Ensure]Monotonic applications.
            //
            return new Monotonic(clock, adjust: true);
        }

        private sealed class Monotonic : IClock
        {
            private readonly bool _adjust;
            private readonly IClock _clock;
            private long _lastValue;

            public Monotonic(IClock clock, bool adjust)
            {
                _clock = clock;
                _lastValue = clock.Now;
                _adjust = adjust;
            }

            public long Now
            {
                get
                {
                    var now = _clock.Now;

                    if (now < _lastValue)
                    {
                        if (!_adjust)
                        {
                            throw new InvalidOperationException("Clock has gone back in time.");
                        }
                        else
                        {
                            now = _lastValue;
                        }
                    }

                    _lastValue = now;

                    return now;
                }
            }
        }
    }
}
