// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Provides utilities to convert between integers and strings using a specified alphabet.
    /// </summary>
    internal sealed class IntegerKey
    {
        /// <summary>
        /// The base-62 integer key, using digits 0-9, lower case letters a-z, and upper case letters A-Z.
        /// </summary>
        public static readonly IntegerKey Base62 = new('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');

        /// <summary>
        /// Size of the cache in <see cref="_cache"/> used to store string representations of commonly used integers.
        /// </summary>
        private const int CacheCount = 1024;

        /// <summary>
        /// The alphabet used to represent integer values. The first character is the 'zero' element, the second character the 'one' element, and so on.
        /// </summary>
        private readonly char[] _alphabet;

        /// <summary>
        /// A map from alphabet characters to their integer value, used to speed up lookups. Each entry in this dictionary corresponds to a character and its index in <see cref="_alphabet"/>.
        /// </summary>
        private readonly Dictionary<char, int> _map;

        /// <summary>
        /// Cache of string representations of commonly used integers in the range [0, <see cref="CacheCount"/>).
        /// </summary>
        private readonly string[] _cache = new string[CacheCount];

        /// <summary>
        /// Creates a new instance of <see cref="IntegerKey"/> using the specified <paramref name="alphabet"/>.
        /// </summary>
        /// <param name="alphabet">The alphabet containing the characters used to represent integer values. The first character is the 'zero' element, the second character the 'one' element, and so on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="alphabet"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="alphabet"/> contains less than 2 characters, or <paramref name="alphabet"/> contains the reserved character <c>-</c>, or <paramref name="alphabet"/> contains one or more duplicate characters.</exception>
        public IntegerKey(params char[] alphabet)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));
            if (alphabet.Length <= 1)
                throw new ArgumentException("The alphabet should consist of at least two characters.", nameof(alphabet));

            _alphabet = alphabet;
            _map = new Dictionary<char, int>(alphabet.Length);

            var i = 0;

            foreach (var c in alphabet)
            {
                if (c == '-')
                    throw new ArgumentException("The alphabet cannot contain the reserved character '-'.", nameof(alphabet));

                if (_map.ContainsKey(c))
                    throw new ArgumentException("All letters in the alphabet should be distinct.", nameof(alphabet));

                _map.Add(c, i++);
            }
        }

        /// <summary>
        /// Converts the specified integer value <paramref name="i"/> to a string representation using the alphabet.
        /// </summary>
        /// <param name="i">The integer value to convert to a string.</param>
        /// <returns>The string representation of <paramref name="i"/> using the alphabet.</returns>
        public string ToString(long i)
        {
            //
            // PERF: Do we want to have -1 in the cache as well? It may be commonly used to denote the absence of a value.
            //

            //
            // Check if we want to cache the string representation.
            //

            if (i is >= 0 and < CacheCount)
            {
                //
                // Try to retrieve the value from the cache. If not found, call ToStringCore.
                //

                var res = _cache[i];

                if (res == null)
                {
                    res = ToStringCore(i);

                    //
                    // NB: There's no point in using locks, volatile reads, and/or interlocked instructions here. Worst case, we compute the same value more than once.
                    //

                    _cache[i] = res;
                }

                return res;
            }

            return ToStringCore(i);
        }

        /// <summary>
        /// Converts the specified integer value <paramref name="i"/> to a string representation using the alphabet.
        /// </summary>
        /// <param name="i">The integer value to convert to a string.</param>
        /// <returns>The string representation of <paramref name="i"/> using the alphabet.</returns>
        /// <remarks>This function always allocates a string.</remarks>
        private string ToStringCore(long i)
        {
            if (i == 0)
            {
                return _alphabet[0].ToString();
            }

            // PERF: Consider calculating the required length using log(i + 1, base) and allocate the required buffer size.
            // PERF: Consider using stackalloc and Span<char>.

            var sb = new StringBuilder();

            var isNeg = i < 0;

            if (isNeg)
            {
                sb.Append('-');
                i = -i;
            }

            while (i > 0)
            {
                var d = (int)(i % _alphabet.Length);
                sb.Append(_alphabet[d]);
                i /= _alphabet.Length;
            }

            var b = isNeg ? 1 : 0;
            var e = sb.Length - 1;

            while (b < e)
            {
                var t = sb[b];
                sb[b] = sb[e];
                sb[e] = t;

                b++;
                e--;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Parses the specified string representation <paramref name="s"/> to an integer value using the alphabet.
        /// </summary>
        /// <param name="s">The string representation to convert to an integer value.</param>
        /// <returns>The integer value of <paramref name="s"/> using the alphabet.</returns>
        public long Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length == 0)
                throw new ArgumentException("Input cannot be empty.", nameof(s));

            var isNeg = false;
            var i = 0;

            if (s[0] == '-')
            {
                isNeg = true;
                i = 1;

                if (s.Length == 1)
                    throw new ArgumentException("Unexpected end of input after '-' sign character.", nameof(s));
            }

            var res = 0L;

            while (i < s.Length)
            {
                var c = s[i];

                if (!_map.TryGetValue(c, out var v))
                    throw new ArgumentException(FormattableString.Invariant($"Unexpected character '{c}'."), nameof(s));

                res = checked(res * _alphabet.Length + v);

                i++;
            }

            return isNeg ? -res : res;
        }
    }
}
