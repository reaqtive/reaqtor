// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

namespace System.Memory
{
    /// <summary>
    /// Interface for data structures that can be trimmed.
    /// </summary>
    /// <typeparam name="T">The type of the elements held by the data structure.</typeparam>
    public interface ITrimmable<out T>
    {
        /// <summary>
        /// Trims the data structure by examining its elements.
        /// </summary>
        /// <param name="shouldTrim">Function to determine whether an element should be trimmed.</param>
        /// <returns>The number of elements that were trimmed.</returns>
        int Trim(Func<T, bool> shouldTrim);
    }
}
