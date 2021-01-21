// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    internal sealed class WildcardTraversal<TSource>
    {
        private readonly Stack<int> _path;

        public WildcardTraversal(TSource wildcard)
        {
            _path = new Stack<int>();
            Wildcard = wildcard;
        }

        public TSource Wildcard { get; }

        public void Push(int next) => _path.Push(next);

        public ITree<T> Get<T>(ITree<T> root)
        {
            var cur = root;
            foreach (var nxt in _path)
            {
                cur = cur.Children[nxt];
            }

            return cur;
        }

        public override string ToString() => string.Join(" -> ", _path.ToArray());
    }
}
