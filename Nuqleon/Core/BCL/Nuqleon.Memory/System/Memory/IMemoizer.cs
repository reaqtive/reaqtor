// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// Interface for function memoizers.
    /// </summary>
    public interface IMemoizer
    {
#pragma warning disable CA1716 // Identifiers should not match keywords. (Use of function is appropriate here.)

        /// <summary>
        /// Memoizes the specified <paramref name="function"/>.
        /// </summary>
        /// <typeparam name="T">Type of the function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer">Comparer to compare the function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        IMemoizedDelegate<Func<T, TResult>> Memoize<T, TResult>(Func<T, TResult> function, MemoizationOptions options = MemoizationOptions.None, IEqualityComparer<T> comparer = null);

#pragma warning restore CA1716
    }
}
