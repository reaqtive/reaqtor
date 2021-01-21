// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using System.Buffers;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Implementation of <see cref="IArrayPool{T}"/> using <see cref="ArrayPool{T}"/>.
    /// </summary>
    internal sealed class CharArrayPool : IArrayPool<char>
    {
        /// <summary>
        /// The shared singleton instance of the array pool, using defaults of <c>System.Buffers</c>.
        /// </summary>
        public static readonly CharArrayPool Instance = new(ArrayPool<char>.Shared);

        /// <summary>
        /// The underlying array pool to use.
        /// </summary>
        private readonly ArrayPool<char> _pool;

        /// <summary>
        /// Creates a new instance of <see cref="CharArrayPool"/> using the specified underlying <paramref name="pool"/>.
        /// </summary>
        /// <param name="pool">The underlying array pool to use.</param>
        public CharArrayPool(ArrayPool<char> pool) => _pool = pool;

        /// <summary>
        /// Rents an array from the pool. This array must be returned when it is no longer needed.
        /// </summary>
        /// <param name="minimumLength">The minimum required length of the array. The returned array may be longer.</param>
        /// <returns>The rented array from the pool. This array must be returned when it is no longer needed.</returns>
        public char[] Rent(int minimumLength) => _pool.Rent(minimumLength);

        /// <summary>
        /// Returns the specified <paramref name="array"/> to the pool.
        /// </summary>
        /// <param name="array">The array that is being returned.</param>
        public void Return(char[] array) => _pool.Return(array, clearArray: false);
    }
}
