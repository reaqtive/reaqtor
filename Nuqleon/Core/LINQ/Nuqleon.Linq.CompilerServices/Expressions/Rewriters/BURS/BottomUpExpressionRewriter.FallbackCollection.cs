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
    /// Represents a collection of fallback rules.
    /// </summary>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public class ExpressionRewriterFallbackCollection<TTarget> : IEnumerable<Fallback<ExpressionTreeBase, TTarget>>
        where TTarget : ITree
    {
        private readonly FallbackCollection<ExpressionTreeBase, TTarget> _fallbacks;

        internal ExpressionRewriterFallbackCollection(FallbackCollection<ExpressionTreeBase, TTarget> fallbacks) => _fallbacks = fallbacks;

        /// <summary>
        /// Adds a new fallback rule.
        /// </summary>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<Expression, TTarget>> convert, int cost)
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, _ => true, cost);
        }

        /// <summary>
        /// Adds a new fallback rule.
        /// </summary>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="predicate">Lambda expression representing the predicate to apply to evaluate applicability of the rule to a source tree leaf node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<Expression, TTarget>> convert, Expression<Func<Expression, bool>> predicate, int cost)
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
        /// Gets an enumerator to iterate over the fallback rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the fallback rules in the collection.</returns>
        public IEnumerator<Fallback<ExpressionTreeBase, TTarget>> GetEnumerator() => _fallbacks.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the fallback rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the fallback rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void AddCore(Expression<Func<Expression, TTarget>> convert, Expression<Func<Expression, bool>> predicate, int cost)
        {
            BottomUpExpressionRewriterUtils.GetConvertAndPredicate<Expression, TTarget>(
                convert,
                predicate,
                out Expression<Func<ExpressionTree, TTarget>> convertLambda,
                out Func<ExpressionTreeBase, TTarget> convertFunction,
                out Expression<Func<ExpressionTree, bool>> predicateLambda,
                out Func<ExpressionTreeBase, bool> predicateFunction
            );

            var fallback = new Fallback<ExpressionTreeBase, TTarget>(convertLambda, convertFunction, predicateLambda, predicateFunction, cost);
            _fallbacks.AddCore(fallback);
        }
    }
}
