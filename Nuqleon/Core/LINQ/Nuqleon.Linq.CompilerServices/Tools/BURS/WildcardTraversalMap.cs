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
    internal sealed class WildcardTraversalMap<TSource>
    {
        private readonly Dictionary<TSource, WildcardTraversal<TSource>> _map;

        public WildcardTraversalMap() => _map = new Dictionary<TSource, WildcardTraversal<TSource>>();

        public WildcardTraversal<TSource> this[TSource node]
        {
            get => _map[node];
            set => _map[node] = value;
        }

        public bool TryGetValue(TSource node, out WildcardTraversal<TSource> traversal) => _map.TryGetValue(node, out traversal);

        public void PushPathSegment(int childIndex)
        {
            foreach (var traversal in _map)
            {
                traversal.Value.Push(childIndex);
            }
        }

        public void Merge(WildcardTraversalMap<TSource> map)
        {
            foreach (var traversal in map._map)
            {
                if (_map.ContainsKey(traversal.Key))
                    throw new InvalidOperationException("Wildcard used multiple times in pattern: " + traversal.Key);

                _map[traversal.Key] = traversal.Value;
            }
        }

        public override string ToString() => "{ " + string.Join(", ", _map.Select(kv => kv.Key + ": " + kv.Value)) + " }";
    }
}
