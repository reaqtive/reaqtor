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
    /// Represents a clock that uses virtual time.
    /// </summary>
    public class VirtualTimeClock : IClock
    {
        private long _now;

        /// <summary>
        /// Creates a new virtual time clock with an initial time set to zero ticks.
        /// </summary>
        public VirtualTimeClock() => _now = 0L;

        /// <summary>
        /// Creates a new virtual time clock with the specified initial time.
        /// </summary>
        /// <param name="initialTime">Initial time to set the clock to.</param>
        public VirtualTimeClock(long initialTime)
        {
            if (initialTime < 0L)
                throw new ArgumentOutOfRangeException(nameof(initialTime));

            _now = initialTime;
        }

        /// <summary>
        /// Gets or sets the current time in ticks.
        /// </summary>
        public long Now
        {
            get => _now;

            set
            {
                if (value < 0L)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _now = value;
            }
        }
    }
}
