// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Delegate to parse string input to an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to parse input into.</typeparam>
    /// <param name="str">The string to parse from.</param>
    /// <param name="len">The length of the string to parse from.</param>
    /// <param name="i">The start index in the string where to start parsing from.</param>
    /// <param name="ctx">The parser context used to thread state through the entire deserialization.</param>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
    /// <exception cref="Nuqleon.Json.Parser.ParseException">Thrown when the parser encounters an error.</exception>
    internal delegate T ParseStringFunc<out T>(string str, int len, ref int i, ParserContext ctx);

#if !NO_IO
    /// <summary>
    /// Delegate to parse string input to an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to parse input into.</typeparam>
    /// <param name="reader">The text reader to parse from.</param>
    /// <param name="ctx">The parser context used to thread state through the entire deserialization.</param>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
    /// <exception cref="Nuqleon.Json.Parser.ParseException">Thrown when the parser encounters an error.</exception>
    internal delegate T ParseReaderFunc<out T>(System.IO.TextReader reader, ParserContext ctx);
#endif
}
