// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Syntax trie implementation used to look up identifiers.
    /// </summary>
    public sealed class SyntaxTrie
    {
        private readonly Dictionary<char, SyntaxTrie> _trie;
        private readonly SyntaxTrie _parent;
        private readonly bool _isTerminal;

        /// <summary>
        /// Creates a new empty syntax trie.
        /// </summary>
        public SyntaxTrie()
            : this(parent: null, isTerminal: false)
        {
        }

        private SyntaxTrie(SyntaxTrie parent, bool isTerminal)
        {
            _trie = new Dictionary<char, SyntaxTrie>();
            _parent = parent;
            _isTerminal = isTerminal;
        }

        /// <summary>
        /// Adds an identifier to the syntax trie.
        /// </summary>
        /// <param name="identifier">Identifier to add.</param>
        public void Add(string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            var next = this;

            for (int i = 0; i < identifier.Length; i++)
            {
                var c = identifier[i];

                if (!next._trie.TryGetValue(c, out SyntaxTrie t))
                {
                    t = new SyntaxTrie(next, i == identifier.Length - 1);
                    next._trie[c] = t;
                }

                next = t;
            }
        }

        /// <summary>
        /// Checks whether the specified identifier exist in the syntax tree as a terminal symbol.
        /// </summary>
        /// <param name="identifier">Identifier to check for.</param>
        /// <returns>true if the specified identifier exists; otherwise, false.</returns>
        public bool Contains(string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            var next = this;

            for (int i = 0; i < identifier.Length; i++)
            {
                var c = identifier[i];

                if (!next._trie.TryGetValue(c, out SyntaxTrie t))
                {
                    return false;
                }

                next = t;
            }

            return next._isTerminal;
        }

        /// <summary>
        /// Removes the specified identifier from the syntax trie.
        /// </summary>
        /// <param name="identifier">Identifier to remove.</param>
        /// <returns>true if the specified identifier was found and removed; otherwise, false.</returns>
        public bool Remove(string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            var next = this;

            for (int i = 0; i < identifier.Length; i++)
            {
                var c = identifier[i];

                if (!next._trie.TryGetValue(c, out SyntaxTrie t))
                {
                    return false;
                }

                next = t;
            }

            if (next._isTerminal)
            {
                var parent = next._parent;

                for (int i = identifier.Length - 1; i >= 0; i--)
                {
                    var c = identifier[i];

                    parent._trie.Remove(c);

                    if (parent._trie.Count > 0)
                    {
                        break;
                    }

                    parent = next._parent;
                }

                return true;
            }

            return false;
        }
    }
}
