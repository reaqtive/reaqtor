// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System;
using System.Diagnostics;
using System.Memory;

namespace Nuqleon.Json.Interop.Newtonsoft
{
    /// <summary>
    /// Specialized stack implementation to hold JSON tokens or expressions.
    /// </summary>
    /// <remarks>
    /// A specialized implementation is preferred over <see cref="System.Collections.Generic.Stack{TokenStack}"/>
    /// because of
    /// <list type="bullet">
    /// <item>No need to version the collection to protect against concurrent enumerations.</item>
    /// <item>Expose the underlying array directly to avoid having to pop and reverse segments of the stack.</item>
    /// <item>Support Push(n) to avoid repeated capacity checks and dynamic array growth.</item>
    /// <item>Support Pop(n) to avoid fine-grained array element clearing.</item>
    /// <item>Implement <see cref="IClearable"/> directly for object pooling support.</item>
    /// </list>
    /// </remarks>
    internal sealed class TokenStack : IClearable
    {
        private Token[] _tokens;

        public TokenStack() => _tokens = new Token[16];

        public int Count { get; private set; }

        public Token this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Count);

                return _tokens[index];
            }

            set
            {
                Debug.Assert(index >= 0 && index < Count);

                _tokens[index] = value;
            }
        }

        public void Push(Token token)
        {
            EnsureCapacity(1);
            _tokens[Count++] = token;
        }

        public void Push(int n)
        {
            Debug.Assert(n >= 0);

            EnsureCapacity(n);
            Count += n;
        }

        public void Pop(int n)
        {
            Debug.Assert(n >= 0);

            Array.Clear(_tokens, Count - n, n);
            Count -= n;
        }

        private void EnsureCapacity(int n)
        {
            var size = Count + n;

            if (size > _tokens.Length)
            {
                var newSize = _tokens.Length * 2;

                while (newSize < size)
                {
                    newSize *= 2;
                }

                var newTokens = new Token[newSize];
                Array.Copy(_tokens, newTokens, Count);
                _tokens = newTokens;
            }
        }

        public bool TryPop(out Token token)
        {
            if (Count == 0)
            {
                token = default;
                return false;
            }

            var n = --Count;
            token = _tokens[n];
            _tokens[n] = default;
            return true;
        }

        public void Clear()
        {
            Array.Clear(_tokens, 0, Count);
            Count = 0;
        }
    }
}
