// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System.Globalization;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal static class Helpers
    {
        private const int N = 64; // NB: The largest Bonsai trees we've seen to this date (December 2016) have 40-50 entries in reflection context tables.
        private static readonly Json.ConstantExpression[] s_integers = Enumerable.Range(0, N).Select(i => Json.Expression.Number(i.ToStringFast())).ToArray();

        public static string ToStringFast(this int i)
        {
            return i switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                6 => "6",
                7 => "7",
                8 => "8",
                9 => "9",
                _ => i.ToString(CultureInfo.InvariantCulture),
            };
        }

        public static Json.ConstantExpression ToJsonNumber(this int i)
        {
            if (i is >= 0 and < N)
            {
                return s_integers[i];
            }

            return Json.Expression.Number(i.ToString(CultureInfo.InvariantCulture));
        }

        public static int ParseInt32(string input)
        {
            const int maxTens = int.MaxValue / 10;
            const int maxUnitPos = int.MaxValue % 10;

            var res = default(int);

            var len = input.Length;
            if (len == 0)
                throw InvalidFormat;

            var i = 0;
            var c = input[i];

            var neg = false;

            if (c == '-')
            {
                neg = true;

                if (++i == len)
                    throw InvalidFormat;
            }

            var maxUnit = neg ? (maxUnitPos + 1) : maxUnitPos;

            while (i < len)
            {
                c = input[i++];

                var d = c - '0';
                if (d is < 0 or > 9)
                    throw InvalidFormat;

                if (res >= maxTens)
                {
                    if (res > maxTens)
                        throw InvalidFormat;

                    if (d > maxUnit)
                        throw InvalidFormat;
                }

                var next = res * 10 + d;

                res = next;
            }

            return neg ? -res : res;
        }

        // TODO: centralize all exceptions and message strings to an Error/Strings class pair

        private static FormatException InvalidFormat => new("Input string was not in a correct format.");
    }
}
