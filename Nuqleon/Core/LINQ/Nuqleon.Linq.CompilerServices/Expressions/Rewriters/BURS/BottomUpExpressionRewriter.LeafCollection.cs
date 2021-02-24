// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a collection of leaf rules.
    /// </summary>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public class ExpressionRewriterLeafCollection<TTarget> : IEnumerable<Leaf<ExpressionTreeBase, TTarget>>
        where TTarget : ITree
    {
        private readonly LeafCollection<ExpressionTreeBase, TTarget> _leaves;

        internal ExpressionRewriterLeafCollection(LeafCollection<ExpressionTreeBase, TTarget> leaves) => _leaves = leaves;

        /// <summary>
        /// Adds a new leaf rule.
        /// </summary>
        /// <typeparam name="TLeaf">Type of the source tree leaf node. This type should be a subtype of Expression.</typeparam>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, int cost)
                where TLeaf : Expression
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, _ => true, cost);
        }

        /// <summary>
        /// Adds a new leaf rule.
        /// </summary>
        /// <typeparam name="TLeaf">Type of the source tree leaf node. This type should be a subtype of Expression.</typeparam>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="predicate">Lambda expression representing the predicate to apply to evaluate applicability of the rule to a source tree leaf node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, Expression<Func<TLeaf, bool>> predicate, int cost)
            where TLeaf : Expression
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, predicate, cost);
        }

        /// <summary>
        /// Gets an enumerator to iterate over the leaf rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the leaf rules in the collection.</returns>
        public IEnumerator<Leaf<ExpressionTreeBase, TTarget>> GetEnumerator() => _leaves.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the leaf rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the leaf rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void AddCore<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, Expression<Func<TLeaf, bool>> predicate, int cost)
            where TLeaf : Expression
        {
            BottomUpExpressionRewriterUtils.GetConvertAndPredicate<TLeaf, TTarget>(
                convert,
                predicate,
                out Expression<Func<ExpressionTree, TTarget>> convertLambda,
                out Func<ExpressionTreeBase, TTarget> convertFunction,
                out Expression<Func<ExpressionTree, bool>> predicateLambda,
                out Func<ExpressionTreeBase, bool> predicateFunction
            );

            var leaf = new Leaf<ExpressionTreeBase, TTarget>(convertLambda, convertFunction, predicateLambda, predicateFunction, cost);
            _leaves.AddCore(leaf);
        }
    }
}
