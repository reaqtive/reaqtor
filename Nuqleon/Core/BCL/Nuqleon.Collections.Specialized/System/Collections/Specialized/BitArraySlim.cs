// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

namespace System.Collections.Specialized
{
    internal struct BitArraySlim<T> : IBitArray
        where T : struct, IByteArray
    {
        private T _array;
        private readonly byte _size;

        public BitArraySlim(int size)
        {
            _array = default;

            if (size > _array.Length * 8 || size < 0 || size > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            _size = (byte)size;
        }

        public readonly int Count => _size;

        public bool this[int index]
        {
            readonly get
            {
                if (index >= _size || index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return ((_array[index / 8] >> (index % 8)) & 0x1) != 0x0;
            }
            set
            {
                if (index >= _size || index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (value)
                {
                    _array[index / 8] |= (byte)(1u << (index % 8));
                }
                else
                {
                    _array[index / 8] &= (byte)~(1u << (index % 8));
                }
            }
        }

        public void SetAll(bool value)
        {
            if (!value)
            {
                _array = default;
            }
            else
            {
                // This could be faster since 64 bit machines can operate on 8 bytes at a time,
                // but it leads to a more complicated interface and/or implementation for IByteArray
                for (var i = 0; i <= _size / 8; i++)
                {
                    _array[i] = 0xff;
                }
            }
        }
    }
}
