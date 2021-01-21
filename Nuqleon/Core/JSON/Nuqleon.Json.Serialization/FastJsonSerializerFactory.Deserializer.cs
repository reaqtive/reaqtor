// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

namespace Nuqleon.Json.Serialization
{
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// JSON deserializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        private sealed class Deserializer<T> : DeserializerBase<T>
        {
            private readonly ParserContext _context;

#if !NO_IO
            /// <summary>
            /// Creates a new deserializer given the specified parser implementation.
            /// </summary>
            /// <param name="parseString">The parser to use to deserialize JSON payloads from string inputs.</param>
            /// <param name="parseReader">The parser to use to deserialize JSON payloads from text reader inputs.</param>
            public Deserializer(ParseStringFunc<T> parseString, ParseReaderFunc<T> parseReader)
                : base(parseString, parseReader)
            {
                _context = new ParserContext();
            }
#else
            /// <summary>
            /// Creates a new deserializer given the specified parser implementation.
            /// </summary>
            /// <param name="parseString">The parser to use to deserialize JSON payloads from string inputs.</param>
            public Deserializer(ParseStringFunc<T> parseString)
                : base(parseString)
            {
                _context = new ParserContext();
            }
#endif

            /// <summary>
            /// Deserializes the specified JSON string to an instance of type <typeparamref name="T"/>.
            /// </summary>
            /// <param name="json">The JSON string to deserialize.</param>
            /// <param name="length">The length of the input string.</param>
            /// <param name="index">The index in the input string at which to start deserializing JSON.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            protected override T DeserializeCore(string json, int length, ref int index) => _parseString(json, length, ref index, _context);

#if !NO_IO
            /// <summary>
            /// Deserializes the JSON payload from a text reader to an instance of type <typeparamref name="T"/>.
            /// </summary>
            /// <param name="reader">The text reader containing the JSON payload to deserialize.</param>
            /// <returns>An instance of type <typeparamref name="T"/> containing the deserialized JSON payload.</returns>
            protected override T DeserializeCore(System.IO.TextReader reader) => _parseReader(reader, _context);
#endif
        }
    }
}
