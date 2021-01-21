// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Provides helper methods to compute hash codes.
    /// </summary>
    internal static class HashHelpers
    {
        /// <summary>
        /// Combines two hash codes using a non-commutative approach.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <returns>A combined hash code.</returns>
        public static int Combine(int h1, int h2)
        {
            unchecked
            {
                // RyuJIT optimizes this to use the ROL instruction
                uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
                return ((int)rol5 + h1) ^ h2;
            }
        }

        /// <summary>
        /// Combines the specified hash codes using a non-commutative approach.
        /// </summary>
        /// <param name="h1">The first hash code.</param>
        /// <param name="h2">The second hash code.</param>
        /// <param name="h3">The third hash code.</param>
        /// <param name="h4">The fourth hash code.</param>
        /// <param name="h5">The fifth hash code.</param>
        /// <param name="h6">The sixth hash code.</param>
        /// <returns>A combined hash code.</returns>
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6) =>
            Combine(
                Combine(
                    Combine(h1, h2),
                    Combine(h3, h4)
                ),
                Combine(h5, h6)
            );
    }
}
