// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

#pragma warning disable CA1711 // Rename type so it doesn't end with Delegate. (Use of Delegate is appropriate here.)
#pragma warning disable CA1716 // Identifiers should not match keywords. (Use of Delegate is appropriate here.)

namespace System.Memory
{
    /// <summary>
    /// Representation of a memoized delegate including a reference to the memoization cache.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    public interface IMemoizedDelegate<out TDelegate>
    {
        /// <summary>
        /// Gets the memoization cache.
        /// </summary>
        IMemoizationCache Cache
        {
            get;
        }

        /// <summary>
        /// Gets the memoized delegate.
        /// </summary>
        TDelegate Delegate
        {
            get;
        }
    }
}
