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
    /// Interface representing a cache to store memoization entries for a memoized function.
    /// </summary>
    public interface IMemoizationCache : IClearable, IDisposable
    {
        /// <summary>
        /// Gets a debug view on the memoization cache.
        /// </summary>
        string DebugView { get; }

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        int Count { get; }
    }
}
