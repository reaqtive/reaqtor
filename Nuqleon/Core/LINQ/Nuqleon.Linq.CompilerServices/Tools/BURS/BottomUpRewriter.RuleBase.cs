// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for rules.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    /// <remarks>
    /// Creates a new rule with the given cost.
    /// </remarks>
    /// <param name="cost">Cost to apply the rule.</param>
    public abstract class RuleBase<TSource, TTarget>(int cost)
    {
        private List<WildcardTraversal<TSource>> _paths;

        /// <summary>
        /// Gets the cost to apply the rule.
        /// </summary>
        public int Cost { get; } = cost;

        internal void SetWildcardTraversalPaths(List<WildcardTraversal<TSource>> paths)
        {
            Debug.Assert(_paths == null, "Traversal paths already set");
            _paths = paths;
        }

        internal IEnumerable<WildcardBinding<T>> RecurseOnWildcards<T>(ITree<T> tree)
        {
            if (_paths == null)
                return [];

            return _paths.Select(traversal => new WildcardBinding<T>(traversal.Wildcard, traversal.Get<T>(tree)));
        }

        internal readonly struct WildcardBinding<T>(TSource wildcard, ITree<T> tree)
        {
            public readonly TSource Wildcard = wildcard;
            public readonly ITree<T> Tree = tree;
        }
    }
}
