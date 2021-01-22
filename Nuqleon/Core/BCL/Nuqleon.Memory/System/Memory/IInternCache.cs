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
    /// </summary>
    /// <typeparam name="T">Type of the values to intern.</typeparam>
    public interface IInternCache<T> : IMemoizationCache
    {
        /// <summary>
        /// Interns the specified <paramref name="value"/>, returning an equivalent value.
        /// </summary>
        /// <param name="value">The value to intern.</param>
        /// <returns>The original value if no interned equivalent value was found; otherwise, an equivalent value.</returns>
        T Intern(T value);
    }
}
