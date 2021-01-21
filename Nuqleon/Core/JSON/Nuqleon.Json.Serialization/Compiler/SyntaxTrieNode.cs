// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Represents a node in a syntax trie.
    /// </summary>
    /// <typeparam name="T">Type of the objects associated with terminal productions.</typeparam>
    internal sealed class SyntaxTrieNode<T>
        where T : class
    {
        /// <summary>
        /// Creates a new syntax trie node with the specified character.
        /// </summary>
        /// <param name="value">The character represented by this node.</param>
        public SyntaxTrieNode(char value)
        {
            Value = value;
            Children = new Dictionary<char, SyntaxTrieNode<T>>();
        }

        /// <summary>
        /// Gets the character represented by this node.
        /// </summary>
        public char Value { get; }

        /// <summary>
        /// Gets the terminal production associated with this node, or null if the node is not a terminal.
        /// </summary>
        public T Terminal { get; set; }

        /// <summary>
        /// Gets the children of the current node.
        /// </summary>
        public IDictionary<char, SyntaxTrieNode<T>> Children { get; }

        /// <summary>
        /// Compiles the syntax trie node into an expression fragment for efficient lexing and parsing of JSON. This function is recursive and builds up the lexer for an entire syntax trie.
        /// </summary>
        /// <param name="str">Expression of type System.String representing the string to read from.</param>
        /// <param name="len">Expression of type System.Int32 representing the length fo the string.</param>
        /// <param name="b">Expression of type System.Int32 representing the start of the string being lexed, used for error logging purposes.</param>
        /// <param name="i">Expression of type System.Int32 representing the index where to start reading the string from.</param>
        /// <param name="res">Expression of type System.Int32&amp; representing the output parameter to write the switch table index to upon finding a match with a terminal.</param>
        /// <param name="e">Label target to exit the compiled syntax trie with a System.Boolean result indicating whether a match was found.</param>
        /// <param name="c">Expression of type System.Char representing a temporary storage location for a lexed character.</param>
        /// <param name="terminals">The switch table to keep terminal states in. This table gets appended to by calls to this method whenever a terminal node is encountered.</param>
        /// <returns>An expression fragment containing the logic to consume the input string and match it with the syntax trie node and its children.</returns>
        internal Expression CompileString(Expression str, Expression len, Expression b, Expression i, Expression res, LabelTarget e, Expression c, IList<T> terminals)
        {
            //
            // The index in the switch table. This value will remain set to -1 in case the node is not a
            // terminal production.
            //
            var id = -1;

            //
            // If the node is a terminal production, add an entry to the switch table and assign a unique
            // identifier for the entry.
            //
            if (Terminal != default(T))
            {
                id = terminals.Count;
                terminals.Add(Terminal);
            }

            var next = ExpressionUtils.Empty;

            if (Children.Count > 0)
            {
                if (Children.Count == 1)
                {
                    var node = this;

                    //
                    // If the node contains only a single child, continue scanning for single-child nodes
                    // and concatenate all the characters that lead up to the first node with more than
                    // one child or no children. This allows us to emit efficient code that simply performs
                    // a JSON-aware "StartsWith" call.
                    //

                    var prefix = default(string);

                    using (var psb = PooledStringBuilder.New())
                    {
                        var sb = psb.StringBuilder;

                        do
                        {
                            var kv = node.Children.Single();
                            sb.Append(kv.Key);
                            node = kv.Value;
                        } while (node.Children.Count == 1 && node.Terminal == default(T));

                        prefix = sb.ToString();
                    }

                    var prefixValue = Expression.Constant(prefix, typeof(string));

                    //
                    // NB: The Parser.StartsWith helper function always performs character-per-character JSON
                    //     escape decoding on the input. However, if the input does not contain any escape
                    //     sequences, we can be more efficient and rely on String.CompareOrdinal to perform a
                    //     quick check for a prefix match assuming that escape sequences in JSON Object keys
                    //     are rare. Worst case, we'll be scanning the string twice.
                    //
                    //     In order for this optimization to be safe, we need to check that the string we are
                    //     matching against can't underrun a JSON escape sequence or overrun a JSON string. For
                    //     examples, see StringHasNoEscapesOrTerminator.
                    //

                    var startsWithCheck = (Expression)Expression.Call(ReflectionCache.StartsWithString, str, len, b, i, prefixValue);

                    if (Parser.StringHasNoEscapesOrTerminator(prefix))
                    {
                        startsWithCheck =
                            Expression.OrElse(
                                Expression.Call(ReflectionCache.StartsWithStringFast, str, i, prefixValue),
                                startsWithCheck
                            );
                    }

                    //
                    // NB: A call to any StartsWith helper method causes the index to move beyond a match
                    //     of the prefix, if found.
                    //
                    // if (Parser.StartsWith(str, len, b, ref i, prefix))
                    //     ...
                    //
                    // -or-
                    //
                    // if (Parser.StartsWithFast(str, ref i, prefix) || Parser.StartsWith(str, len, b, ref i, prefix))
                    //     ...
                    //

                    next =
                        Expression.IfThen(
                            startsWithCheck,
                            node.CompileString(str, len, b, i, res, e, c, terminals)
                        );
                }
                else
                {
                    //
                    // NB: A call to the GetNextChar helper method causes the index to move beyond the next
                    //     lexed character, allowing for escape sequences.
                    //
                    // if (Parser.TryGetNextChar(str, len, ref i, ref c))
                    // {
                    //     switch (c)
                    //         ...
                    // }
                    //

                    next =
                        Expression.IfThen(
                            Expression.Call(ReflectionCache.TryGetNextCharString, str, len, i, c),
                            Expression.Switch(
                                c,
                                Children.Select(kv => Expression.SwitchCase(kv.Value.CompileString(str, len, b, i, res, e, c, terminals), Expression.Constant(kv.Key))).ToArray()
                            )
                        );
                }
            }

            if (id >= 0)
            {
                //
                // if (str[i] == '\"')
                //     ...
                // else
                //     ...
                //

                next =
                    Expression.IfThenElse(
                        Expression.Equal(Expression.MakeIndex(str, ReflectionCache.Chars, new[] { i }), Expression.Constant('\"')),
                        Expression.Block(
                            Expression.Assign(res, Expression.Constant(id)),
                            Expression.Return(e, ExpressionUtils.ConstantTrue)
                        ),
                        next
                    );
            }

            //
            // if (i < len)
            //     ...
            //

            var expr =
                Expression.IfThen(
                    Expression.LessThan(i, len),
                    next
                );

            return expr;
        }

#if !NO_IO
        /// <summary>
        /// Compiles the syntax trie node into an expression fragment for efficient lexing and parsing of JSON. This function is recursive and builds up the lexer for an entire syntax trie.
        /// </summary>
        /// <param name="reader">Expression of type System.String representing the text reader to read from.</param>
        /// <param name="res">Expression of type System.Int32&amp; representing the output parameter to write the switch table index to upon finding a match with a terminal.</param>
        /// <param name="e">Label target to exit the compiled syntax trie with a System.Boolean result indicating whether a match was found.</param>
        /// <param name="c">Expression of type System.Int32 representing a temporary storage location for a lexed character, or -1 if the end of the reader has been reached.</param>
        /// <param name="terminals">The switch table to keep terminal states in. This table gets appended to by calls to this method whenever a terminal node is encountered.</param>
        /// <returns>An expression fragment containing the logic to consume the input string and match it with the syntax trie node and its children.</returns>
        internal Expression CompileReader(Expression reader, Expression res, LabelTarget e, Expression c, IList<T> terminals)
        {
            //
            // The index in the switch table. This value will remain set to -1 in case the node is not a
            // terminal production.
            //
            var id = -1;

            //
            // If the node is a terminal production, add an entry to the switch table and assign a unique
            // identifier for the entry.
            //
            if (Terminal != default(T))
            {
                id = terminals.Count;
                terminals.Add(Terminal);
            }

            var next = ExpressionUtils.Empty;

            if (Children.Count > 0)
            {
                if (Children.Count == 1)
                {
                    var node = this;

                    //
                    // If the node contains only a single child, continue scanning for single-child nodes
                    // and concatenate all the characters that lead up to the first node with more than
                    // one child or no children. This allows us to emit efficient code that simply performs
                    // a JSON-aware "StartsWith" call.
                    //

                    var prefix = default(string);

                    using (var psb = PooledStringBuilder.New())
                    {
                        var sb = psb.StringBuilder;

                        do
                        {
                            var kv = node.Children.Single();
                            sb.Append(kv.Key);
                            node = kv.Value;
                        } while (node.Children.Count == 1 && node.Terminal == default(T));

                        prefix = sb.ToString();
                    }

                    var prefixValue = Expression.Constant(prefix, typeof(string));

                    //
                    // NB: The Parser.StartsWithReader helper function always performs character-per-character JSON
                    //     escape decoding on the input.
                    //

                    var startsWithCheck = (Expression)Expression.Call(ReflectionCache.StartsWithReader, reader, prefixValue);

                    //
                    // NB: A call to any StartsWithReader helper method causes the reader to consume input, even if
                    //     there's no match (due to it being limited to peek only one character ahead). When changing
                    //     the "if" statement below, this should be taken into account because an "else" won't read
                    //     from the original position prior to a failed StartsWithReader check.
                    //
                    // if (Parser.StartsWithReader(reader, prefix))
                    //     ...
                    //

                    next =
                        Expression.IfThen(
                            startsWithCheck,
                            node.CompileReader(reader, res, e, c, terminals)
                        );
                }
                else
                {
                    //
                    // NB: A call to the GetNextChar helper method causes the index to move beyond the next
                    //     lexed character, allowing for escape sequences.
                    //
                    // if (Parser.TryGetNextChar(reader, ref c))
                    // {
                    //     switch (c)
                    //         ...
                    // }
                    //

                    next =
                        Expression.IfThen(
                            Expression.Call(ReflectionCache.TryGetNextCharReader, reader, c),
                            Expression.Switch(
                                c,
                                Children.Select(kv => Expression.SwitchCase(kv.Value.CompileReader(reader, res, e, c, terminals), Expression.Constant((int)kv.Key))).ToArray()
                            )
                        );
                }
            }

            if (id >= 0)
            {
                //
                // if (reader.Peek() == '\"')
                //     ...
                // else
                //     ...
                //

                next =
                    Expression.IfThenElse(
                        Expression.Equal(
                            Expression.Call(reader, ReflectionCache.Peek),
                            Expression.Constant((int)'\"')
                        ),
                        Expression.Block(
                            Expression.Assign(res, Expression.Constant(id)),
                            Expression.Return(e, ExpressionUtils.ConstantTrue)
                        ),
                        next
                    );
            }

            return next;
        }
#endif
    }
}
