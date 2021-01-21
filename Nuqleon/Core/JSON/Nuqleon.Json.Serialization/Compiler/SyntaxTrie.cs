// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace Nuqleon.Json.Serialization
{
    //
    // NB: These data structures are not particularly optimized for size under the assumption that not that many JSON deserializers
    //     will be constructed. This type is used for the compilation of such deserializers and can be further optimized in case many
    //     such compilations are expected.
    //

    /// <summary>
    /// Represents a syntax trie where each terminal production has an associated object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the objects associated with terminal productions.</typeparam>
    internal sealed class SyntaxTrie<T>
        where T : class
    {
        /// <summary>
        /// Creates a new empty syntax trie.
        /// </summary>
        public SyntaxTrie() => Root = new SyntaxTrieNode<T>('\0');

        /// <summary>
        /// Gets the root node of the syntax trie.
        /// </summary>
        public SyntaxTrieNode<T> Root { get; }

        /// <summary>
        /// Adds a terminal production to the syntax trie.
        /// </summary>
        /// <param name="value">The value of the string to add to the syntax trie.</param>
        /// <param name="action">The terminal production to associate with the string.</param>
        public void Add(string value, T action)
        {
            var node = Root;

            foreach (var c in value)
            {
                if (!node.Children.TryGetValue(c, out SyntaxTrieNode<T> next))
                {
                    next = new SyntaxTrieNode<T>(c);
                    node.Children[c] = next;
                }

                node = next;
            }

            if (node.Terminal != null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An entry for '{0}' already exists.", value));
            }

            node.Terminal = action;
        }

        /// <summary>
        /// Compiles the syntax trie into a delegate for efficient evaluation on string input.
        /// </summary>
        /// <returns>A compiled syntax trie consisting of an evaluation delegate and a switch table for productions.</returns>
        public CompiledTrieString<T> CompileString()
        {
            //
            // Switch table of terminal states to productions.
            //
            var terminals = new List<T>();

            //
            // Parameters for the lambda expression with signature (string str, int len, int b, ref int i, out int res)
            // where the res out parameter will correspond to a production in the switch table.
            //
            var str = Expression.Parameter(typeof(string), "str");
            var len = Expression.Parameter(typeof(int), "len");
            var bgn = Expression.Parameter(typeof(int), "b");
            var idx = Expression.Parameter(typeof(int).MakeByRefType(), "i");
            var chr = Expression.Parameter(typeof(char), "c");
            var res = Expression.Parameter(typeof(int).MakeByRefType(), "res");

            //
            // Exit label to return a Boolean indicating whether a match was found or not.
            //
            var ret = Expression.Label(typeof(bool));

            //
            // The compiled syntax trie which will emit a jump to the exit label if a match is found. The switch
            // table gets populated by this call to Compile.
            //
            var trieEval = Root.CompileString(str, len, bgn, idx, res, ret, chr, terminals);

            //
            // Top-level body of the EvalTrie lambda. Falls through to the exit label with a false value in case
            // no match is found.
            //
            var body =
                Expression.Block(
                    new[] { chr },
                    trieEval,
                    Expression.Label(ret, ExpressionUtils.ConstantFalse)
                );

            //
            // Compile the lambda into an EvalTrie and return the delegate together with the switch table for the
            // caller to use.
            //
            var expr = Expression.Lambda<EvalTrieString>(body, str, len, bgn, idx, res);
            var eval = expr.Compile();

            return new CompiledTrieString<T> { Terminals = terminals, Eval = eval };
        }

#if !NO_IO
        /// <summary>
        /// Compiles the syntax trie into a delegate for efficient evaluation on text reader input.
        /// </summary>
        /// <returns>A compiled syntax trie consisting of an evaluation delegate and a switch table for productions.</returns>
        public CompiledTrieReader<T> CompileReader()
        {
            //
            // Switch table of terminal states to productions.
            //
            var terminals = new List<T>();

            //
            // Parameters for the lambda expression with signature (string str, int len, int b, ref int i, out int res)
            // where the res out parameter will correspond to a production in the switch table.
            //
            var rdr = Expression.Parameter(typeof(System.IO.TextReader), "reader");
            var chr = Expression.Parameter(typeof(int), "c");
            var res = Expression.Parameter(typeof(int).MakeByRefType(), "res");

            //
            // Exit label to return a Boolean indicating whether a match was found or not.
            //
            var ret = Expression.Label(typeof(bool));

            //
            // The compiled syntax trie which will emit a jump to the exit label if a match is found. The switch
            // table gets populated by this call to Compile.
            //
            var trieEval = Root.CompileReader(rdr, res, ret, chr, terminals);

            //
            // Top-level body of the EvalTrieReader lambda. Falls through to the exit label with a false value in case
            // no match is found.
            //
            var body =
                Expression.Block(
                    new[] { chr },
                    trieEval,
                    Expression.Label(ret, ExpressionUtils.ConstantFalse)
                );

            //
            // Compile the lambda into an EvalTrieReader and return the delegate together with the switch table for the
            // caller to use.
            //
            var expr = Expression.Lambda<EvalTrieReader>(body, rdr, res);
            var eval = expr.Compile();

            return new CompiledTrieReader<T> { Terminals = terminals, Eval = eval };
        }
#endif
    }
}
