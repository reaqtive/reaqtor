// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Threading;

namespace Nuqleon.DataModel.Serialization.Binary
{
    /// <summary>
    /// Represents a pool of reusable arrays.
    /// </summary>
    /// <typeparam name="T">Element type of the arrays.</typeparam>
    internal sealed class ArrayPool<T>
    {
        /// <summary>
        /// Number of elements in each pooled array.
        /// </summary>
        private readonly int _size;

        /// <summary>
        /// Number of elements in the pool.
        /// </summary>
        private readonly int _max;

        /// <summary>
        /// Array of holders containing arrays that are available from the pool.
        /// </summary>
        private readonly Holder[] _pool;

        /// <summary>
        /// Creates a new array pool for arrays of the specified size and with the specified maximum pool capacity.
        /// </summary>
        /// <param name="size">Size of the arrays in the pool.</param>
        /// <param name="max">Maximum pool capacity.</param>
        public ArrayPool(int size, int max = 16)
        {
            _size = size;
            _max = max;
            _pool = new Holder[max];
        }

        /// <summary>
        /// Gets an array from the pool. After use, the array has to be returned to the pool using the <see cref="Release"/> method.
        /// </summary>
        /// <returns>Array obtained from the pool.</returns>
        /// <remarks>Array retrieved from the pool are not guaranteed to be cleared.</remarks>
        public T[] Get()
        {
            var n = _max;

            for (var i = 0; i < n; i++)
            {
                var arr = Interlocked.Exchange(ref _pool[i].Array, null);
                if (arr != null)
                {
                    return arr;
                }
            }

            return new T[_size];
        }

        /// <summary>
        /// Returns an array to the pool. The specified array should have been obtained using the <see cref="Get"/> method before.
        /// </summary>
        /// <param name="array">Array to return to the pool.</param>
        public void Release(T[] array)
        {
            var n = _max;

            for (var i = 0; i < n; i++)
            {
                if (_pool[i].Array == null)
                {
                    _pool[i].Array = array;
                    break;
                }
            }
        }

        /// <summary>
        /// Holder struct containing a pooled array.
        /// </summary>
        private struct Holder
        {
            /// <summary>
            /// Pooled array.
            /// </summary>
            public T[] Array;
        }
    }
}
