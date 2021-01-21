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
    /// Representation of a memoized delegate including a reference to the memoization cache.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    internal sealed class MemoizedDelegate<TDelegate> : IMemoizedDelegate<TDelegate>
    {
        /// <summary>
        /// Creates a new memoized delegate representation.
        /// </summary>
        /// <param name="function">The memoized delegate.</param>
        /// <param name="cache">The memoization cache.</param>
        public MemoizedDelegate(TDelegate function, IMemoizationCache cache)
        {
            Delegate = function;
            Cache = cache;
        }

        /// <summary>
        /// Gets the memoization cache.
        /// </summary>
        public IMemoizationCache Cache { get; }

        /// <summary>
        /// Gets the memoized delegate.
        /// </summary>
        public TDelegate Delegate { get; }
    }
}
