// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System.Collections.Generic;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Value representing a compiled syntax trie for string input.
    /// </summary>
    /// <typeparam name="T">Type of the objects associated with terminal productions.</typeparam>
    internal struct CompiledTrieString<T>
        where T : class
    {
        /// <summary>
        /// Gets a dictionary mapping terminal production indexes onto terminal production objects.
        /// </summary>
        public IList<T> Terminals { get; set; }

        /// <summary>
        /// Gets the compiled delegate to evaluate the trie for a string input.
        /// </summary>
        public EvalTrieString Eval { get; set; }
    }

#if !NO_IO
    /// <summary>
    /// Value representing a compiled syntax trie for text reader input.
    /// </summary>
    /// <typeparam name="T">Type of the objects associated with terminal productions.</typeparam>
    internal struct CompiledTrieReader<T>
        where T : class
    {
        /// <summary>
        /// Gets a dictionary mapping terminal production indexes onto terminal production objects.
        /// </summary>
        public IList<T> Terminals { get; set; }

        /// <summary>
        /// Gets the compiled delegate to evaluate the trie for a text reader input.
        /// </summary>
        public EvalTrieReader Eval { get; set; }
    }
#endif
}
