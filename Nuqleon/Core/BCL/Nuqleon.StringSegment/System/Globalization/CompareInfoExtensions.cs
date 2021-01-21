// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

namespace System.Globalization
{
    /// <summary>
    /// Contains helper methods for StringSegment-based comparisons.
    /// </summary>
    internal static class CompareInfoExtensions
    {
        /// <summary>
        /// Compares two specified string segments using the specified comparison options and culture-specific information to influence the comparison, and returns an integer that indicates the relationship of the two strings to each other in the sort order.
        /// </summary>
        /// <param name="info">Object representing the type of comparison to carry out.</param>
        /// <param name="strA">The first string segment to compare.</param>
        /// <param name="strB">The second string segment to compare.</param>
        /// <param name="options">Options to use when performing the comparison (such as ignoring case or symbols).</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(this CompareInfo info, StringSegment strA, StringSegment strB, CompareOptions options) => info.Compare(strA.String, strA.StartIndex, strA.Length, strB.String, strB.StartIndex, strB.Length, options);
    }
}
