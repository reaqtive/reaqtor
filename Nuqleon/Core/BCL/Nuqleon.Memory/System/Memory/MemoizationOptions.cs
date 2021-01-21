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
    /// Options to control memoization behavior.
    /// </summary>
    [Flags]
    public enum MemoizationOptions
    {
        /// <summary>
        /// Default memoization behavior.
        /// </summary>
        None = 0,

        /// <summary>
        /// Cache exceptional function evaluation outcome rather than invoking the function each time.
        /// </summary>
        CacheException = 1,
    }
}
