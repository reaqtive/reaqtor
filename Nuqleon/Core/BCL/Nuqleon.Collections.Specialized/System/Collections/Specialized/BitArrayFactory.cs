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
    /// <summary>
    /// A factory to create a bit array of a desired size.
    /// </summary>
    public static class BitArrayFactory
    {
        /// <summary>
        /// A factory method to create a bit array of a desired size. The returned bit
        /// array will have exactly <paramref name="size"/> number of addressable bits.
        /// </summary>
        /// <param name="size">The size of the bit array to be created.</param>
        /// <returns>A bit array of the desired size.</returns>
        public static IBitArray Create(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            return (size / 8) switch
            {
                0 => new BitArraySlim<ByteArray1>(size),
                1 => new BitArraySlim<ByteArray2>(size),
                2 => new BitArraySlim<ByteArray3>(size),
                3 => new BitArraySlim<ByteArray4>(size),
                4 => new BitArraySlim<ByteArray5>(size),
                5 => new BitArraySlim<ByteArray6>(size),
                6 => new BitArraySlim<ByteArray7>(size),
                7 => new BitArraySlim<ByteArray8>(size),
                8 => new BitArraySlim<ByteArray9>(size),
                9 => new BitArraySlim<ByteArray10>(size),
                10 => new BitArraySlim<ByteArray11>(size),
                _ => new DecoratedBitArray(size),
            };
        }
    }
}
