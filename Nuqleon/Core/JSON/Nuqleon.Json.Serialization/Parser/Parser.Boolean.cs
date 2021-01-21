// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using Nuqleon.Json.Parser;
using System.Globalization;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        /// <summary>
        /// Lexes and parses a JSON Boolean as a System.Boolean in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Boolean from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Boolean, if found.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.Boolean value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Boolean.
        /// For nullable Booleans, use th ParseNullableBoolean method instead.
        /// JSON Booleans are case-sensitive.
        /// </remarks>
        internal static bool ParseBoolean(string str, int len, ref int i, ParserContext _ = null)
        {
            // | "true"  -> true
            // | "false" -> false
            // | _       -> throw error

            if (i + 4 <= len)
            {
#if ALLOW_UNSAFE
                unsafe
                {
                    const long TRUE = ((long)'t' << 0) | ((long)'r' << 16) | ((long)'u' << 32) | ((long)'e' << 48);
                    const long FALS = ((long)'f' << 0) | ((long)'a' << 16) | ((long)'l' << 32) | ((long)'s' << 48);

                    fixed (char* s = str)
                    {
                        var c = s + i;

                        var val = *((long*)c);

                        if (val == TRUE)
                        {
                            i += 4;
                            return true;
                        }
                        else if (val == FALS && i + 5 <= len && *(c + 4) == 'e')
                        {
                            i += 5;
                            return false;
                        }
                    }
                }
#else
                switch (str[i])
                {
                    case 't':
                        if (str[i + 1] == 'r' && str[i + 2] == 'u' && str[i + 3] == 'e')
                        {
                            i += 4;
                            return true;
                        }
                        break;
                    case 'f':
                        if (i + 5 <= len)
                        {
                            if (str[i + 1] == 'a' && str[i + 2] == 'l' && str[i + 3] == 's' && str[i + 4] == 'e')
                            {
                                i += 5;
                                return false;
                            }
                        }
                        break;
                }
#endif
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected JSON Boolean literal at position '{0}'.", i), i, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Boolean as a System.Boolean in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Boolean from.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.Boolean value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Boolean.
        /// For nullable Booleans, use the ParseNullableBoolean method instead.
        /// JSON Booleans are case-sensitive.
        /// </remarks>
        internal static bool ParseBoolean(System.IO.TextReader reader, ParserContext _ = null)
        {
#if ALLOW_UNSAFE
            if (reader.Read(context.char4, 0, 4) == 4)
            {
                unsafe
                {
                    const long TRUE = ((long)'t' << 0) | ((long)'r' << 16) | ((long)'u' << 32) | ((long)'e' << 48);
                    const long FALS = ((long)'f' << 0) | ((long)'a' << 16) | ((long)'l' << 32) | ((long)'s' << 48);

                    fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                    {
                        var val = *((long*)s);

                        if (val == TRUE)
                        {
                            return true;
                        }
                        else if (val == FALS && reader.Read() == 'e')
                        {
                            return false;
                        }
                    }
                }
            }
#else
            switch ((char)reader.Read())
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0078 // Use pattern matching. (`reader.Read()` has a side-effect and should not get removed; see https://github.com/dotnet/roslyn-analyzers/issues/4690.)
                case 't':
                    if (reader.Read() == 'r' && reader.Read() == 'u' && reader.Read() == 'e')
                    {
                        return true;
                    }
                    break;
                case 'f':
                    if (reader.Read() == 'a' && reader.Read() == 'l' && reader.Read() == 's' && reader.Read() == 'e')
                    {
                        return false;
                    }
                    break;
#pragma warning restore IDE0078
#pragma warning disable IDE0079
            }
#endif

            throw new ParseException("Expected JSON Boolean literal.", -1, ParseError.InvalidToken);
        }
#endif
    }
}
