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
    /// Delegate for syntax trie evaluation given string input.
    /// </summary>
    /// <param name="str">The string to match the syntax trie against.</param>
    /// <param name="len">The length of the string.</param>
    /// <param name="b">The start index of the string literal denoting a key in a JSON object being parsed; used for error reporting.</param>
    /// <param name="i">The index in the string to start matching from. This value gets updated to the index position of the " terminating the JSON object key, if found.</param>
    /// <param name="res">The switch table index of the terminal production that was matched, if found.</param>
    /// <returns>true if the syntax trie matched the input string; otherwise, false.</returns>
    internal delegate bool EvalTrieString(string str, int len, int b, ref int i, out int res);

#if !NO_IO
    /// <summary>
    /// Delegate for syntax trie evaluation given text reader input.
    /// </summary>
    /// <param name="reader">The reader to match the syntax trie against.</param>
    /// <param name="res">The switch table index of the terminal production that was matched, if found.</param>
    /// <returns>true if the syntax trie matched the input string; otherwise, false.</returns>
    internal delegate bool EvalTrieReader(System.IO.TextReader reader, out int res);
#endif
}
