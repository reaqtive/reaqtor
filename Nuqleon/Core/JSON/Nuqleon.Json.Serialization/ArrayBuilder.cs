// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nuqleon.Json.Serialization
{
    internal static class ArrayBuilder
    {
        public static ArrayBuilder<T> Create<T>()
        {
            //
            // CONSIDER: Efficient pooling of array builders to reuse small arrays of size 4, 8, 16, etc. This would need a per-type
            //           cache, though all reference types could be clumped together in object arrays. For big value types, we'd have
            //           to be careful not to end up with huge non-reclaimable caches. Note that when we use a pooling strategy, we
            //           have to change ToArray below to make sure we don't return arrays to the pool if we hand them out to the caller.
            //

            return new ArrayBuilder<T>();
        }
    }

    internal sealed class ArrayBuilder<T>
    {
        public static readonly T[] Empty = Array.Empty<T>();

        private const int INITIAL_CAPACITY = 4;
        private const int GROW_FACTOR = 2;

        private T[] _values = Empty;
        private int _count;

        public void Add(T value)
        {
            var oldLen = _values.Length;

            if (oldLen == _count)
            {
                var newLen = oldLen == 0 ? INITIAL_CAPACITY : oldLen * GROW_FACTOR;

                var values = new T[newLen];
                Array.Copy(_values, 0, values, 0, oldLen);
                _values = values;
            }

            _values[_count++] = value;
        }

        public T[] ToArray()
        {
            //
            // NB: The builder should not get allocated if no elements will be added to it.
            //

            Debug.Assert(_count > 0);

            if (_count == _values.Length)
            {
                return _values;
            }
            else
            {
                var res = new T[_count];
                Array.Copy(_values, 0, res, 0, _count);
                return res;
            }
        }

        public List<T> ToList()
        {
            //
            // CONSIDER: Use a separate builder for lists and rely on its dynamic growth, so we don't have to perform this
            //           final copy at the end. The current approach would allow for the use of pooled array chunks though.
            //           Another benefit of the current approach is that the resulting lists won't have an excess capacity,
            //           so if they're kept alive for a long time, we're not wasting space on empty array slots.
            //

            //
            // NB: The builder should not get allocated if no elements will be added to it.
            //

            Debug.Assert(_count > 0);

            var res = new List<T>(_count);

            for (var i = 0; i < _count; i++)
                res.Add(_values[i]);

            return res;
        }
    }
}
