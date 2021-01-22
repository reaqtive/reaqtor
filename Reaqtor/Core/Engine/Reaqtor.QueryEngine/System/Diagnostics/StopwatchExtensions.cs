// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Diagnostics
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="Stopwatch"/>.
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Converts a stopwatch instance to an <see cref="IStopwatch"/>.
        /// </summary>
        /// <param name="stopwatch">Stopwatch to convert.</param>
        /// <returns>An <see cref="IStopwatch"/> wrapper around the specified stopwatch instance.</returns>
        public static IStopwatch ToStopwatch(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
                throw new ArgumentNullException(nameof(stopwatch));

            return new StopwatchImpl(stopwatch);
        }
    }
}
