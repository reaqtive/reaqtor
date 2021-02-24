// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace System.Collections.Generic
{
    /// <summary>
    /// Interface representing a sorted set.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
    public interface ISortedSet<T> : ISet<T>
    {
        /// <summary>
        /// Gets the maximum value in the set.
        /// </summary>
        T Max { get; }

        /// <summary>
        /// Gets the minimum value in the set.
        /// </summary>
        T Min { get; }

        /// <summary>
        /// Returns a view of a subset in the set.
        /// </summary>
        /// <param name="lowerValue">The lowest desired value in the view.</param>
        /// <param name="upperValue">The highest desired value in the view.</param>
        /// <returns>A subset view that contains only the values in the specified range.</returns>
        /// <exception cref="ArgumentException"><paramref name="lowerValue" /> is more than <paramref name="upperValue" /> according to the set's comparer.</exception>
        /// <exception cref="ArgumentOutOfRangeException">A tried operation on the view was outside the range specified by <paramref name="lowerValue" /> and <paramref name="upperValue" />.</exception>
        ISortedSet<T> GetViewBetween(T lowerValue, T upperValue);

        /// <summary>
        /// Returns an enumerable sequence that enumerates over the set in reverse order.
        /// </summary>
        /// <returns>An enumerator that iterates over the set in reverse order.</returns>
        IEnumerable<T> Reverse();

        // REVIEW: Do we want RemoveWhere as well?
    }
}
