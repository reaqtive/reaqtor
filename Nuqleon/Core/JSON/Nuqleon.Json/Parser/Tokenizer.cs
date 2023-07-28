// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

#if NET6_0 || NETSTANDARD2_1
using System;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuqleon.Json.Parser
{
    /// <summary>
    /// Tokenizer for JSON code.
    /// </summary>
    internal sealed class Tokenizer
    {
        #region Private fields

        /// <summary>
        /// Interned strings for commonly used negative integer values of length 1.
        /// </summary>
        private static readonly string[] s_intsM1 =
            new[] { "-0", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9" };

        /// <summary>
        /// Interned strings for commonly used positive integer values of length 1.
        /// </summary>
        private static readonly string[] s_ints1 =
            new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        /// <summary>
        /// Interned strings for commonly used positive integer values of length 2.
        /// </summary>
        private static readonly string[][] s_ints2 = new[]
        {
            new[] { "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" },
            new[] { "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" },
            new[] { "30", "31", "32", "33", "34", "35", "36", "37", "38", "39" },
            new[] { "40", "41", "42", "43", "44", "45", "46", "47", "48", "49" },
            new[] { "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" },
            new[] { "60", "61", "62", "63", "64", "65", "66", "67", "68", "69" },
            new[] { "70", "71", "72", "73", "74", "75", "76", "77", "78", "79" },
            new[] { "80", "81", "82", "83", "84", "85", "86", "87", "88", "89" },
            new[] { "90", "91", "92", "93", "94", "95", "96", "97", "98", "99" },
        };

        /// <summary>
        /// JSON code text.
        /// </summary>
        private readonly string _input;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new tokenizer for JSON code.
        /// </summary>
        /// <param name="input">JSON code text.</param>
        public Tokenizer(string input) => _input = input;

        #endregion

        #region Methods

        /// <summary>
        /// Tokenizes the input.
        /// </summary>
        /// <param name="includeWhite">Indicates whether to include whitespace tokens or not.</param>
        /// <returns>Sequence of tokens.</returns>
        public IEnumerator<Token> Tokenize(bool includeWhite = true)
        {
            // PERF: This iterator method returns IEnumerator<Token> rather than IEnumerable<Token>
            //       in order to avoid unnecessary calls to Environment.CurrentManagedThreadId used
            //       by the emitted code to detect the presence of multiple enumerations over the
            //       sequence. Given that we only enumerate the token sequence once, changing to an
            //       IEnumerator <Token> is just fine.

            var i = 0;

            while (i < _input.Length)
            {
                var c = _input[i];
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        {
                            var b = i++;
                            var h = true;
                            while (h && i < _input.Length)
                            {
                                var d = _input[i];
                                switch (d)
                                {
                                    case ' ':
                                    case '\t':
                                    case '\r':
                                    case '\n':
                                        i++;
                                        break;
                                    default:
                                        h = false;
                                        break;
                                }
                            }

                            if (includeWhite)
                            {
                                yield return Token.White(b);
                            }
                        }
                        break;
                    case '{':
                        yield return Token.LeftCurly(i++);
                        break;
                    case '}':
                        yield return Token.RightCurly(i++);
                        break;
                    case '[':
                        yield return Token.LeftBracket(i++);
                        break;
                    case ']':
                        yield return Token.RightBracket(i++);
                        break;
                    case ',':
                        yield return Token.Comma(i++);
                        break;
                    case ':':
                        yield return Token.Colon(i++);
                        break;
                    case 'f':
                        if (i + 5 /*"false".Length*/ <= _input.Length && SubstringEquals(_input, i + 1, "alse"))
                        {
                            yield return Token.False(i += 5 /*"false".Length*/);
                        }
                        else
                        {
                            throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                        }
                        break;
                    case 't':
                        if (i + 4 /*"true".Length*/ <= _input.Length && SubstringEquals(_input, i + 1, "rue"))
                        {
                            yield return Token.True(i += 4 /*"true".Length*/);
                        }
                        else
                        {
                            throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                        }
                        break;
                    case 'n':
                        if (i + 4 /*"null".Length*/ <= _input.Length && SubstringEquals(_input, i + 1, "ull"))
                        {
                            yield return Token.Null(i += 4 /*"null".Length*/);
                        }
                        else
                        {
                            throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                        }
                        break;
                    case '\"':
                        {
                            var sb = default(StringBuilder);

                            var b = i++;
                            var h = true;
                            while (h && i < _input.Length)
                            {
                                var d = _input[i];
                                switch (d)
                                {
                                    case '\"':
                                        h = false;
                                        i++;
                                        break;
                                    case '\\':
                                        if (i + 1 >= _input.Length)
                                        {
                                            throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                                        }
                                        else
                                        {
                                            if (sb == null)
                                            {
                                                var len = 2 * (i - b);
                                                if (len < 16)
                                                    len = 16;

                                                sb = new StringBuilder(len);
                                                sb.Append(_input, b + 1, i - b - 1);
                                            }

                                            i++;
                                            var n = _input[i++];
                                            switch (n)
                                            {
                                                case '\"':
                                                    sb.Append('\"');
                                                    break;
                                                case '\\':
                                                    sb.Append('\\');
                                                    break;
                                                case '/':
                                                    sb.Append('/');
                                                    break;
                                                case 'b':
                                                    sb.Append('\b');
                                                    break;
                                                case 'f':
                                                    sb.Append('\f');
                                                    break;
                                                case 'n':
                                                    sb.Append('\n');
                                                    break;
                                                case 'r':
                                                    sb.Append('\r');
                                                    break;
                                                case 't':
                                                    sb.Append('\t');
                                                    break;
                                                case 'u':
                                                    if (i + 4 > _input.Length)
                                                    {
                                                        throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                                                    }
                                                    else
                                                    {
#if NET6_0 || NETSTANDARD2_1
                                                        if (!int.TryParse(_input.AsSpan(i, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int val))
#else
                                                        if (!int.TryParse(_input.Substring(i, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int val))
#endif
                                                        {
                                                            throw new ParseException("Unrecognized Unicode escape sequence.", i, ParseError.InvalidToken);
                                                        }

                                                        var u = char.ConvertFromUtf32(val);
                                                        sb.Append(u);
                                                        i += 4;
                                                    }
                                                    break;
                                                default:
                                                    throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                                            }
                                        }
                                        break;
                                    default:
                                        if (IsControl(d))
                                        {
                                            throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                                        }
                                        sb?.Append(d);
                                        i++;
                                        break;
                                }
                            }

                            if (h)
                            {
                                throw new ParseException("Missing string terminator.", i, ParseError.PrematureEndOfInput);
                            }

                            var s = sb != null ? sb.ToString() : _input.Substring(b + 1, i - b - 2);
                            yield return Token.String(b, s);
                        }
                        break;
                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        yield return Number(ref i);
                        break;
                    case '\0':
                        yield return Token.Eof(i++);
                        break;
                    default:
                        throw new ParseException("Unrecognized token.", i, ParseError.InvalidToken);
                }
            }
        }

        private static bool SubstringEquals(string s, int i, string value)
        {
            var n = value.Length;
            for (int j = 0; j < n; j++)
            {
                if (s[i + j] != value[j])
                {
                    return false;
                }
            }

            return true;
        }

        private Token Number(ref int i)
        {
            var b = i;

            var c = _input[i];
            if (c == '-')
            {
                var d = Peek(i);
                if (d == (char)0)
                {
                    throw new ParseException("Unrecognized token.", b, ParseError.InvalidToken);
                }
                else
                {
                    i++;
                }
            }

            c = _input[i];
            if (c == '0')
            {
                var d = Peek(i);
                i++;

                return d switch
                {
                    '.' => Fraction(b, ref i),
                    'e' or 'E' => Exponent(b, ref i),
                    _ => Integer(b, i),
                };
            }

            if (c is not (>= '1' and <= '9'))
            {
                throw new ParseException("Unrecognized token.", b, ParseError.InvalidToken);
            }

            var h = true;
            while (h && i < _input.Length)
            {
                var n = _input[i];
                switch (n)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        i++;
                        break;
                    case '.':
                        return Fraction(b, ref i);
                    case 'e':
                    case 'E':
                        return Exponent(b, ref i);
                    default:
                        h = false;
                        break;
                }
            }

            return Integer(b, i);
        }

        private Token Integer(int b, int i)
        {
            var len = i - b;

            string str;

            switch (len)
            {
                case 1:
                    {
                        var c0 = _input[b];
                        Debug.Assert(c0 >= '0' & c0 <= '9');

                        str = s_ints1[c0 - '0'];
                    }
                    break;
                case 2:
                    {
                        var c0 = _input[b];
                        var c1 = _input[b + 1];

                        if (c0 == '-')
                        {
                            Debug.Assert(c1 >= '0' & c1 <= '9');

                            str = s_intsM1[c1 - '0'];
                        }
                        else
                        {
                            Debug.Assert(c0 >= '1' & c0 <= '9');
                            Debug.Assert(c1 >= '0' & c1 <= '9');

                            str = s_ints2[c0 - '1'][c1 - '0'];
                        }
                    }
                    break;
                default:
                    str = _input.Substring(b, len);
                    break;
            }

            return Token.Number(b, str);
        }

        private Token Fraction(int b, ref int i)
        {
            Debug.Assert(_input[i] == '.');

            var n = Peek(i);
            if (n is not (>= '0' and <= '9'))
                throw new ParseException("Unrecognized token.", b, ParseError.InvalidToken);

            i++;

            while (i < _input.Length)
            {
                var c = _input[i];
                if (c is >= '0' and <= '9')
                {
                    i++;
                    continue;
                }
                else if (c is 'e' or 'E')
                {
                    return Exponent(b, ref i);
                }
                else
                {
                    break;
                }
            }

#if NET6_0 || NETSTANDARD2_1
            return Token.Number(b, _input[b..i]);
#else
            return Token.Number(b, _input.Substring(b, i - b));
#endif
        }

        private Token Exponent(int b, ref int i)
        {
            Debug.Assert(_input[i] is 'e' or 'E');

            var n = Peek(i);
            if (n is '+' or '-')
                n = Peek(++i);
            if (n is not (>= '0' and <= '9'))
                throw new ParseException("Unrecognized token.", b, ParseError.InvalidToken);

            i++;

            while (i < _input.Length)
            {
                var c = _input[i];
                if (c is >= '0' and <= '9')
                {
                    i++;
                    continue;
                }
                else
                {
                    break;
                }
            }

#if NET6_0 || NETSTANDARD2_1
            return Token.Number(b, _input[b..i]);
#else
            return Token.Number(b, _input.Substring(b, i - b));
#endif
        }

        private char Peek(int i) => i + 1 >= _input.Length ? (char)0 : _input[i + 1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsControl(char c)
        {
            // PERF: Use of char.IsControl leads to calls to JIT_GetSharedNonGCStaticBase__PatchTLSLabel
            //       which adds up to ~5% of CPU usage when parsing 68K Bonsai trees. We inline some of
            //       the implementation here to eliminate this cost. For commonly used Latin1 characters,
            //       performance increases up to 3x. For other characters, the path length is longer and
            //       leads to a 25% performance reduction. Analysis of common inputs (e.g. Bonsai trees)
            //       shows that non-Latin1 characters are used for less than 1% of input characters. We
            //       would break even with char.IsControl at:
            //
            //         0.35 * Latin1 = 1.25 * non-Latin1
            //
            //       which amounts to approximately a ratio of 1 non-Latin1 to 3.6 Latin1. If the ratio
            //       ever changes significantly, we can decide on specializations of the implementation.

            if (c <= '\x00ff')
            {
                return c is <= '\x001f' or >= '\x007f' and <= '\x009f';
            }

            return IsControlSlow(c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsControlSlow(char c)
        {
            // PERF: This method should not be inlined in order to avoid the cost associated with calling
            //       the static method on char described above.
            return char.GetUnicodeCategory(c) == UnicodeCategory.Control;
        }

        #endregion
    }
}
