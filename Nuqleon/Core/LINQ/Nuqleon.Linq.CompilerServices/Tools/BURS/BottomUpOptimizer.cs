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
    /// <summary>
    /// Defines a bottom-up optimizer for TTree trees.
    /// </summary>
    /// <typeparam name="TTree">Type of the source trees. Needs to implement ITree&lt;TTree&gt;.</typeparam>
    /// <typeparam name="TTreeNodeType">Node type of the source trees.</typeparam>
    /// <typeparam name="TWildcardFactory">Wildcard factory type for source tree wildcard nodes.</typeparam>
    public class BottomUpOptimizer<TTree, TTreeNodeType, TWildcardFactory> : BottomUpRewriter<TTree, TTreeNodeType, TTree, TWildcardFactory>
        where TTree : ITree<TTreeNodeType>
        where TWildcardFactory : IWildcardFactory<TTree>, new()
    {
        private readonly IEqualityComparer<TTree> _comparer;

        /// <summary>
        /// Creates a new bottom-up optimizer instance with no initial rewrite rule definitions.
        /// Collection initializer syntax can be used for the Leaves, Rules, and Fallbacks properties.
        /// </summary>
        public BottomUpOptimizer() => _comparer = EqualityComparer<TTree>.Default;

        /// <summary>
        /// Creates a new bottom up rewriter instance with no initial rewrite rule definitions.
        /// Collection initializer syntax can be used for the Leaves, Rules, and Fallbacks properties.
        /// </summary>
        /// <param name="sourceTreeComparer">Equality comparer used to compare source trees in order to detect forward progress during optimization.</param>
        /// <param name="sourceNodeComparer">Equality comparer used to compare source tree nodes during construction of the rule table and during tree matching.</param>
        public BottomUpOptimizer(IEqualityComparer<TTree> sourceTreeComparer, IEqualityComparer<TTreeNodeType> sourceNodeComparer)
            : base(sourceNodeComparer)
        {
            _comparer = sourceTreeComparer ?? throw new ArgumentNullException(nameof(sourceTreeComparer));
        }

        /// <summary>
        /// Optimizes the given tree by repeatedly applying rewrite rules.
        /// </summary>
        /// <param name="tree">Tree to optimize.</param>
        /// <returns>Optimized tree.</returns>
        public TTree Optimize(TTree tree)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));

            var res = tree;

            while (true)
            {
                var optimized = Rewrite(res);
                if (_comparer.Equals(optimized, res)) // TODO: alternatively, we could "leak" the cost of the tree and use repeated Label/Reduce calls
                    break;

                res = optimized;
            }

            return res;
        }
    }
}
