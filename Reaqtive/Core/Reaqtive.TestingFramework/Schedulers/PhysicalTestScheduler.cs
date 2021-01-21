// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Virtual time test scheduler that can execute tasks.
    /// </summary>
    internal class PhysicalTestScheduler : VirtualTimePhysicalScheduler<long, long>
    {
        /// <summary>
        /// Adds a relative virtual time to an absolute virtual time value.
        /// </summary>
        /// <param name="absolute">Absolute virtual time value.</param>
        /// <param name="relative">Relative virtual time value to add.</param>
        /// <returns>Resulting absolute virtual time sum value.</returns>
        public override long Add(long absolute, long relative)
        {
            return absolute + relative;
        }

        /// <summary>
        /// Converts the absolute virtual time value to a DateTimeOffset value.
        /// </summary>
        /// <param name="absolute">Absolute virtual time value to convert.</param>
        /// <returns>Corresponding DateTimeOffset value.</returns>
        public override DateTimeOffset ToDateTimeOffset(long absolute)
        {
            return new DateTimeOffset(absolute, TimeSpan.Zero);
        }

        /// <summary>
        /// Converts the TimeSpan value to a relative virtual time value.
        /// </summary>
        /// <param name="timeSpan">TimeSpan value to convert.</param>
        /// <returns>Corresponding relative virtual time value.</returns>
        public override long ToRelative(TimeSpan timeSpan)
        {
            return timeSpan.Ticks;
        }
    }
}
