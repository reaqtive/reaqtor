// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/04/2015 - Adding intern caches.
//

namespace System.Memory
{
    /// <summary>
    /// Interface for caches used to intern reused values of the specified type.
    /// This type of cache support trimming of entries for values that are no longer used.
    /// </summary>
    /// <typeparam name="T">Type of the values to intern.</typeparam>
    public interface IWeakInternCache<T> : IInternCache<T>
    {
        /// <summary>
        /// Trims the intern cache to remove unused values.
        /// </summary>
        /// <returns>The number of elements that were trimmed from the intern cache.</returns>
        int Trim();
    }
}
