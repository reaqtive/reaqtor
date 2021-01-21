// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//
#if USE_STRINGPOOL
using System.Runtime.CompilerServices;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Pool for string buffers with a length in a predefined range. This type is not thread-safe.
    /// </summary>
    /// <remarks>
    /// The intended usage pattern for strings retrieved from the pool is sequential, i.e. at most one string should be used at a time.
    /// This restriction allows the implementation to be free of synchronization and is well-suited for single-threaded lexers and parsers.
    /// If multiple threads are to use a string pool, the user is responsible to use a string pool that's unique per thread. Alternatively, the ConcurrentStringPool can be used.
    /// </remarks>
    internal class StringPool
    {
        private readonly string[] _pool;

        /// <summary>
        /// Creates a new pool for strings with a specified maximum length.
        /// </summary>
        /// <param name="maxLength">The maximum length for strings in the pool.</param>
        public StringPool(int maxLength) => _pool = new string[maxLength];

        /// <summary>
        /// Returns a string of the specified length. If the pool doesn't contain a string of the specified length, a null reference is returned.
        /// </summary>
        /// <param name="length">The length of the string to allocate.</param>
        /// <returns>A string of the specified length, or null if the pool doesn't contain a string of the specified length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Allocate(int length)
        {
            //
            // NB: Intentionally no checking here.
            //

            if (length < _pool.Length)
            {
                return _pool[length] ?? new string('\0', length); // NB: This overload does a fast allocation.
            }

            return null;
        }

        /// <summary>
        /// Returns a string to the pool.
        /// </summary>
        /// <param name="value">The string to return to the pool.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Free(string value)
        {
            //
            // NB: We trust the (internal) user to use the pool correctly.
            //

            if (value != null)
            {
                var len = value.Length;
                if (len < _pool.Length)
                {
                    _pool[len] = value;
                }
            }
        }
    }
}
#endif
