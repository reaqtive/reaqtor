// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

using System.Linq;

namespace System.Collections.Specialized
{
    internal static class EnumDictionary<TKey>
        where TKey : struct, IConvertible
    {
        internal static Lazy<int> enumSize = new(() =>
        {
            var typ = typeof(TKey);
            if (!typ.IsEnum)
            {
                throw new EnumSizeResolutionException(EnumSizeResolutionError.TypeIsNotEnum);
            }

            var underlying = Enum.GetUnderlyingType(typ);
            if (underlying != typeof(int) &&
                underlying != typeof(ushort) &&
                underlying != typeof(short) &&
                underlying != typeof(byte) &&
                underlying != typeof(sbyte))
            {
                throw new EnumSizeResolutionException(EnumSizeResolutionError.UnderlyingTypeIsNotIntOrSmaller);
            }

            if (typ.GetCustomAttributes(typeof(FlagsAttribute), inherit: false).Any())
            {
                throw new EnumSizeResolutionException(EnumSizeResolutionError.EnumHasFlagAttribute);
            }

            var (max, min) = Enum.GetValues(typ).Cast<TKey>().Select(e => e.ToInt32(null)).Aggregate((Max: 0, Min: 0), (acc, curr) => (Max: Math.Max(acc.Max, curr + 1), Min: Math.Min(acc.Min, curr)));

            return min < 0 ? throw new EnumSizeResolutionException(EnumSizeResolutionError.EnumContainsNegativeValues) : max;
        });
    }
}
