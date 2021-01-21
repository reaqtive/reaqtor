// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System
{
    /// <summary>
    /// Provides a set of runtime assertion helpers.
    /// </summary>
    internal static class Invariant
    {
        /// <summary>
        /// Asserts that the specified <paramref name="condition"/> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The condition to assert.</param>
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException("Runtime invariant broken.");
            }
        }

        /// <summary>
        /// Returns an exception indicating that the code location should be unreachable.
        /// </summary>
        public static Exception Unreachable => new InvalidOperationException("Unreachable code.");
    }
}
