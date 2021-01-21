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
    /// Provides a set of extension methods for stopwatch factories.
    /// </summary>
    public static class StopwatchFactoryExtensions
    {
        /// <summary>
        /// Creates a new stopwatch that gets started immediately before being returned to the called.
        /// </summary>
        /// <param name="factory">The factory used to create the stopwatch.</param>
        /// <returns>New stopwach instance.</returns>
        public static IStopwatch StartNew(this IStopwatchFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var sw = factory.Create();
            sw.Start();
            return sw;
        }
    }
}
