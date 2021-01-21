// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//
#if FALSE
using System.Runtime.CompilerServices;
using System.Threading;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Pool for string buffers with a length in a predefined range. This type is thread-safe.
    /// </summary>
    internal class ConcurrentStringPool
    {
        private readonly string[] _pool;

        /// <summary>
        /// Creates a new pool for strings with a specified maximum length.
        /// </summary>
        /// <param name="maxLength">The maximum length for strings in the pool.</param>
        public ConcurrentStringPool(int maxLength) => _pool = new string[maxLength];

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
                var res = Interlocked.Exchange(ref _pool[length], null);
                return res ?? new string('\0', length); // NB: This overload does a fast allocation.
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
