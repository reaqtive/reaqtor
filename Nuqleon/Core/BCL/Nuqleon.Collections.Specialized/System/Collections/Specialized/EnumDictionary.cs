// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

#pragma warning disable CA1711 // Rename type so it doesn't end with Dictionary. (Preserved for compat.)

using System.Collections.Generic;

namespace System.Collections.Specialized
{
    /// <summary>
    /// A factory to create an IDictionary for enums.
    /// </summary>
    public static class EnumDictionary
    {
        /// <summary>
        /// Creates a dictionary for the given enum. The enum must be a non-flags enum with underlying type of int, short, ushort, byte or sbyte.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys for the dictionary - must be a non-flags enum with underlying type of 
        /// int, short, ushort, byte or sbyte.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <returns>A dictionary whose key is TKey and value is TValue.</returns>
        [CLSCompliant(false)] // NB: IConvertible constraint is not CLS compliant
        public static IDictionary<TKey, TValue> Create<TKey, TValue>()
            where TKey : struct, IConvertible
        {
            int size;
            try
            {
                size = EnumDictionary<TKey>.enumSize.Value;
            }
            catch (EnumSizeResolutionException e)
            {
                switch (e.ErrorCode)
                {
                    case EnumSizeResolutionError.TypeIsNotEnum:
                        throw new ArgumentException("TKey needs to be an enum.", nameof(TKey), e);
                    case EnumSizeResolutionError.UnderlyingTypeIsNotIntOrSmaller:
                        throw new ArgumentException("The underlying type for enum TKey needs to be an int.", nameof(TKey), e);
                    case EnumSizeResolutionError.EnumContainsNegativeValues:
                        throw new ArgumentException("The enum contains negative values.", nameof(TKey), e);
                    case EnumSizeResolutionError.EnumHasFlagAttribute:
                        throw new ArgumentException("Flags enums are not supported.", nameof(TKey), e);
                    case EnumSizeResolutionError.Unknown:
                    default:
                        throw;
                }
            }

            return (size / 8) switch
            {
                0 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray1>>(new BitArraySlim<ByteArray1>(size)),
                1 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray2>>(new BitArraySlim<ByteArray2>(size)),
                2 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray3>>(new BitArraySlim<ByteArray3>(size)),
                3 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray4>>(new BitArraySlim<ByteArray4>(size)),
                4 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray5>>(new BitArraySlim<ByteArray5>(size)),
                5 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray6>>(new BitArraySlim<ByteArray6>(size)),
                6 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray7>>(new BitArraySlim<ByteArray7>(size)),
                7 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray8>>(new BitArraySlim<ByteArray8>(size)),
                8 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray9>>(new BitArraySlim<ByteArray9>(size)),
                9 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray10>>(new BitArraySlim<ByteArray10>(size)),
                10 => new EnumDictionary<TKey, TValue, BitArraySlim<ByteArray11>>(new BitArraySlim<ByteArray11>(size)),
                _ => new EnumDictionary<TKey, TValue, DecoratedBitArray>(new DecoratedBitArray(size)),
            };
        }
    }
}
