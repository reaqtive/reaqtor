// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Nuqleon.Json.Parser;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        /// <summary>
        /// Lexes and parses a JSON value as a System.Object in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse an object from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the JSON value, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Object value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON value.
        /// </remarks>
        internal static object ParseAny(string str, int len, ref int i, ParserContext context)
        {
            if (i >= len)
                throw new ParseException("Unexpected end of stream when parsing a JSON value.", i, ParseError.PrematureEndOfInput);

            var c = str[i];

            switch (c)
            {
                case 'n':
                    if (!TryParseNullSkipN(str, len, ref i))
                    {
                        throw new ParseException("Expected null literal after lexing 'n' character.", i, ParseError.UnexpectedToken);
                    }
                    return null;
                case 't':
                case 'f':
                    return ParseBoolean(str, len, ref i, context);
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

                    //
                    // CONSIDER: This could be made configurable to return a float, double, or decimal. There could even be a mode that
                    //           looks ahead to detect the absence of '.' (and possibly '-') and allow an (unsigned) integer value to be
                    //           returned. We skip adding these modes for now and pick a meaningful default, which also makes the resulting
                    //           object easier to consume because only a few type checks need to be made.
                    //

                    return ParseDouble(str, len, ref i, context);
                case '\"':
                    return ParseString(str, len, ref i, context);
                case '[':
                    return ParseAnyArray(str, len, ref i, context);
                case '{':
                    return ParseAnyObject(str, len, ref i, context, ParseAny);
                default:
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' when deserializing a JSON value.", c), i, ParseError.UnexpectedToken);
            }
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON value as a System.Object in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse an object from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Object value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON value.
        /// </remarks>
        internal static object ParseAny(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c < 0)
                throw new ParseException("Unexpected end of stream when parsing a JSON value.", -1, ParseError.PrematureEndOfInput);

            switch (c)
            {
                case 'n':
                    if (!TryParseNullSkipN(reader, context))
                    {
                        throw new ParseException("Expected null literal after lexing 'n' character.", -1, ParseError.UnexpectedToken);
                    }
                    return null;
                case 't':
                case 'f':
                    return ParseBoolean(reader, context);
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

                    //
                    // CONSIDER: This could be made configurable to return a float, double, or decimal. There could even be a mode that
                    //           looks ahead to detect the absence of '.' (and possibly '-') and allow an (unsigned) integer value to be
                    //           returned. We skip adding these modes for now and pick a meaningful default, which also makes the resulting
                    //           object easier to consume because only a few type checks need to be made.
                    //

                    return ParseDouble(reader, context);
                case '\"':
                    return ParseString(reader, context);
                case '[':
                    return ParseAnyArray(reader, context);
                case '{':
                    return ParseAnyObject(reader, context, ParseAny);
                default:
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' when deserializing a JSON value.", (char)c), -1, ParseError.UnexpectedToken);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON object as a hash table in the specified string starting from the specified index.
        /// </summary>
        /// <typeparam name="TValue">The type of the values to deserialize.</typeparam>
        /// <param name="str">The string to lex and parse an object from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the JSON value, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <param name="parseValue">The parser to use for values.</param>
        /// <returns>A hash table, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON object.
        /// </remarks>
        internal static Dictionary<string, TValue> ParseAnyObject<TValue>(string str, int len, ref int i, ParserContext context, ParseStringFunc<TValue> parseValue)
        {
            Debug.Assert(str[i] == '{');

            //
            // CONSIDER: Add support for different representations of objects, e.g. ExpandoObject from the DLR. For
            //           now, we'll go with the most obvious choice and users can wrap the resulting value in case
            //           they want to support dynamic behavior.
            //

            var res = new Dictionary<string, TValue>();

            i++;

            SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the { token.

            if (i == len)
            {
                throw new ParseException("Unexpected end of stream when parsing object expecting an object body or '}' terminator.", i, ParseError.PrematureEndOfInput);
            }

            if (str[i] == '}')
            {
                i++;
                SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the } token.
                return res;
            }

            while (i < len)
            {
                var key = ParseStringNonNull(str, len, ref i);

                SkipWhiteSpace(str, len, ref i); // NB: This skips leading whitespace before the : token; we can't rely on the trie to do it.

                if (i == len)
                {
                    throw new ParseException("Unexpected end of stream when parsing object expecting a ':' separator between a key name and a value.", i, ParseError.PrematureEndOfInput);
                }

                if (str[i] != ':')
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting a ':' token to separate key name and a value.", str[i]), i, ParseError.UnexpectedToken);
                }

                i++;

                SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the : token.

                //
                // NB: We're not using Add here in order to allow for duplicate keys. The last one in lexical order wins, which is
                //     consistent with behavior of other popular JSON frameworks and the behavior of our strongly typed object
                //     deserializer.
                //

                res[key] = parseValue(str, len, ref i, context);

                //
                // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                //           arrays which already guarantee trimming trailing whitespace.
                //

                SkipWhiteSpace(str, len, ref i);

                if (i == len)
                {
                    throw new ParseException("Unexpected end of stream when parsing object.", i, ParseError.PrematureEndOfInput);
                }

                switch (str[i])
                {
                    case ',':
                        i++;
                        SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the , token.
                        break;
                    case '}':
                        i++;
                        SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the } token.
                        return res;
                    default:
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting either ',' or '}}'.", str[i]), i, ParseError.UnexpectedToken);
                }
            }

            Debug.Assert(i == len);

            throw new ParseException("Unexpected end of stream when parsing object and expecting a key.", i, ParseError.PrematureEndOfInput);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON object as a hash table in the specified text reader.
        /// </summary>
        /// <typeparam name="TValue">The type of the values to deserialize.</typeparam>
        /// <param name="reader">The text reader to lex and parse an object from.</param>
        /// <param name="context">The parser context.</param>
        /// <param name="parseValue">The parser to use for values.</param>
        /// <returns>A hash table, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON object.
        /// </remarks>
        internal static Dictionary<string, TValue> ParseAnyObject<TValue>(System.IO.TextReader reader, ParserContext context, ParseReaderFunc<TValue> parseValue)
        {
            Debug.Assert(reader.Peek() == '{');

            //
            // CONSIDER: Add support for different representations of objects, e.g. ExpandoObject from the DLR. For
            //           now, we'll go with the most obvious choice and users can wrap the resulting value in case
            //           they want to support dynamic behavior.
            //

            var res = new Dictionary<string, TValue>();

            reader.Read();

            SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the { token.

            var c = reader.Peek();

            if (c < 0)
            {
                throw new ParseException("Unexpected end of stream when parsing object expecting an object body or '}' terminator.", -1, ParseError.PrematureEndOfInput);
            }

            if (c == '}')
            {
                reader.Read();
                SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the } token.
                return res;
            }

            while (c >= 0)
            {
                var key = ParseStringNonNull(reader);

                SkipWhiteSpace(reader); // NB: This skips leading whitespace before the : token; we can't rely on the trie to do it.

                c = reader.Peek();

                if (c < 0)
                {
                    throw new ParseException("Unexpected end of stream when parsing object expecting a ':' separator between a key name and a value.", -1, ParseError.PrematureEndOfInput);
                }

                if (c != ':')
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting a ':' token to separate key name and a value.", (char)c), -1, ParseError.UnexpectedToken);
                }

                reader.Read();

                SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the : token.

                //
                // NB: We're not using Add here in order to allow for duplicate keys. The last one in lexical order wins, which is
                //     consistent with behavior of other popular JSON frameworks and the behavior of our strongly typed object
                //     deserializer.
                //

                res[key] = parseValue(reader, context);

                //
                // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                //           arrays which already guarantee trimming trailing whitespace.
                //

                SkipWhiteSpace(reader);

                c = reader.Peek();

                if (c < 0)
                {
                    throw new ParseException("Unexpected end of stream when parsing object.", -1, ParseError.PrematureEndOfInput);
                }

                switch (c)
                {
                    case ',':
                        reader.Read();
                        SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the , token.
                        break;
                    case '}':
                        reader.Read();
                        SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the } token.
                        return res;
                    default:
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting either ',' or '}}'.", (char)c), -1, ParseError.UnexpectedToken);
                }

                c = reader.Peek();
            }

            throw new ParseException("Unexpected end of stream when parsing object and expecting a key.", -1, ParseError.PrematureEndOfInput);
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON array as a System.Object[] in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse an array from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the JSON value, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>An array, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON array.
        /// </remarks>
        private static object[] ParseAnyArray(string str, int len, ref int i, ParserContext context)
        {
            Debug.Assert(str[i] == '[');

            i++;

            SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the [ token.

            if (i == len)
            {
                throw new ParseException("Unexpected end of stream when parsing array.", i, ParseError.PrematureEndOfInput);
            }

            if (str[i] == ']')
            {
                i++;
                SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the ] token.
                return ArrayBuilder<object>.Empty;
            }

            var builder = ArrayBuilder.Create<object>();

            while (i < len)
            {
                builder.Add(ParseAny(str, len, ref i, context));

                //
                // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                //           arrays which already guarantee trimming trailing whitespace.
                //

                SkipWhiteSpace(str, len, ref i);

                if (i == len)
                {
                    throw new ParseException("Unexpected end of stream when parsing array.", i, ParseError.PrematureEndOfInput);
                }

                switch (str[i])
                {
                    case ',':
                        i++;
                        SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the , token.
                        break;
                    case ']':
                        i++;
                        SkipWhiteSpace(str, len, ref i); // NB: We *have* to skip trailing whitespace after the ] token.
                        return builder.ToArray();
                    default:
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing array expecting either ',' or ']'.", str[i]), i, ParseError.UnexpectedToken);
                }
            }

            Debug.Assert(i == len);

            throw new ParseException("Unexpected end of stream when parsing array.", i, ParseError.PrematureEndOfInput);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON array as a System.Object[] in the specified string starting from the specified index.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse an array from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>An array, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON array.
        /// </remarks>
        private static object[] ParseAnyArray(System.IO.TextReader reader, ParserContext context)
        {
            Debug.Assert(reader.Peek() == '[');

            reader.Read();

            SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the [ token.

            var c = reader.Peek();

            if (c < 0)
            {
                throw new ParseException("Unexpected end of stream when parsing array.", -1, ParseError.PrematureEndOfInput);
            }

            if (c == ']')
            {
                reader.Read();
                SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the ] token.
                return ArrayBuilder<object>.Empty;
            }

            var builder = ArrayBuilder.Create<object>();

            while (c >= 0)
            {
                builder.Add(ParseAny(reader, context));

                //
                // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                //           arrays which already guarantee trimming trailing whitespace.
                //

                SkipWhiteSpace(reader);

                c = reader.Peek();

                if (c < 0)
                {
                    throw new ParseException("Unexpected end of stream when parsing array.", -1, ParseError.PrematureEndOfInput);
                }

                switch (c)
                {
                    case ',':
                        reader.Read();
                        SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the , token.
                        break;
                    case ']':
                        reader.Read();
                        SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the ] token.
                        return builder.ToArray();
                    default:
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing array expecting either ',' or ']'.", (char)c), -1, ParseError.UnexpectedToken);
                }

                c = reader.Peek();
            }

            throw new ParseException("Unexpected end of stream when parsing array.", -1, ParseError.PrematureEndOfInput);
        }
#endif
    }
}
