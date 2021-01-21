// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System;
using System.Globalization;

namespace Nuqleon.Json.Serialization
{
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// Base class for JSON deserializers for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        private abstract class DeserializerBase<T> : IFastJsonDeserializer<T>
        {
#if !NO_IO
            protected readonly ParseStringFunc<T> _parseString;
            protected readonly ParseReaderFunc<T> _parseReader;

            /// <summary>
            /// Creates a new deserializer given the specified parser implementations.
            /// </summary>
            /// <param name="parseString">The parser to use to deserialize JSON payloads from string inputs.</param>
            /// <param name="parseReader">The parser to use to deserialize JSON payloads from text reader inputs.</param>
            public DeserializerBase(ParseStringFunc<T> parseString, ParseReaderFunc<T> parseReader)
            {
                _parseString = parseString;
                _parseReader = parseReader;
            }
#else
            protected readonly ParseStringFunc<T> _parseString;

            /// <summary>
            /// Creates a new deserializer given the specified parser implementation.
            /// </summary>
            /// <param name="parseString">The parser to use to deserialize JSON payloads from string inputs.</param>
            public DeserializerBase(ParseStringFunc<T> parseString)
            {
                _parseString = parseString;
            }
#endif

            /// <summary>
            /// Deserializes the specified JSON string to an instance of type <typeparamref name="T"/>. This method skips leading and whitespace.
            /// </summary>
            /// <param name="json">The JSON string to deserialize.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            public T Deserialize(string json)
            {
                if (json == null)
                    throw new ArgumentNullException(nameof(json));

                //
                // CONSIDER: Change the signature to take in an index and a length as well.
                // CONSIDER: Add an overload or extension method for deserialization of a StringSegment.
                //

                var str = json;

                var len = str.Length;
                var i = 0;

                Parser.SkipWhiteSpace(str, len, ref i);

                var res = DeserializeCore(str, len, ref i);

                Parser.SkipWhiteSpace(str, len, ref i);

                if (i < len)
                {
                    throw new Nuqleon.Json.Parser.ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' found at position '{1}' after deserializing a value of type '{2}'.", str[i], i, typeof(T)), i, Json.Parser.ParseError.UnexpectedToken);
                }

                return res;
            }

#if !NO_IO
            /// <summary>
            /// Deserializes the JSON payload from a text reader to an instance of type <typeparamref name="T"/>. This method skips leading and whitespace.
            /// </summary>
            /// <param name="reader">The text reader containing the JSON payload to deserialize.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            public T Deserialize(System.IO.TextReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                //
                // CONSIDER: Change the signature to take in an index and a length as well.
                // CONSIDER: Add an overload or extension method for deserialization of a StringSegment.
                //

                Parser.SkipWhiteSpace(reader);

                var res = DeserializeCore(reader);

                Parser.SkipWhiteSpace(reader);

                //
                // NB: We don't require the whole reader input to be consumed.
                //

                return res;
            }
#endif

            /// <summary>
            /// Deserializes the specified JSON string to an instance of type <typeparamref name="T"/>.
            /// </summary>
            /// <param name="json">The JSON string to deserialize.</param>
            /// <param name="length">The length of the input string.</param>
            /// <param name="index">The index in the input string at which to start deserializing JSON.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            protected abstract T DeserializeCore(string json, int length, ref int index);

#if !NO_IO
            /// <summary>
            /// Deserializes the JSON payload from a text reader to an instance of type <typeparamref name="T"/>.
            /// </summary>
            /// <param name="reader">The text reader containing the JSON payload to deserialize.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            protected abstract T DeserializeCore(System.IO.TextReader reader);
#endif
        }
    }
}
