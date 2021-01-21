// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Defines a bottom-up rewriter for LINQ expression trees to TTarget trees.
    /// </summary>
    /// <typeparam name="TTarget">Type of the target trees. Needs to implement ITree.</typeparam>
    public class BottomUpExpressionRewriter<TTarget> : BottomUpRewriter<ExpressionTreeBase, ExpressionTreeNode, TTarget, ExpressionTreeWildcardFactory>
        where TTarget : ITree
    {
        /// <summary>
        /// Creates a new bottom up rewriter instance with no initial rewrite rule definitions.
        /// Collection initializer syntax can be used for the Leaves, Rules, and Fallbacks properties.
        /// </summary>
        public BottomUpExpressionRewriter()
            : base(ShallowExpressionTreeNodeEqualityComparer.Instance)
        {
            Leaves = new ExpressionRewriterLeafCollection<TTarget>(base.Leaves);
            Rules = new ExpressionRewriterRuleCollection<TTarget>(base.Rules);
            Fallbacks = new ExpressionRewriterFallbackCollection<TTarget>(base.Fallbacks);
        }

        /// <summary>
        /// Gets the collection of leaves recognized by the rewriter.
        /// </summary>
        public new ExpressionRewriterLeafCollection<TTarget> Leaves { get; }

        /// <summary>
        /// Gets the collection of rewrite pattern rules recognized by the rewriter.
        /// </summary>
        public new ExpressionRewriterRuleCollection<TTarget> Rules { get; }

        /// <summary>
        /// Gets the collection of fallback rules used by the rewriter.
        /// </summary>
        public new ExpressionRewriterFallbackCollection<TTarget> Fallbacks { get; }

        /// <summary>
        /// Rewrites the given expression tree to a target tree using the rules in the table.
        /// </summary>
        /// <param name="expression">Expression tree to rewrite.</param>
        /// <returns>Target tree after applying the rewrite rules in the table.</returns>
        public TTarget Rewrite(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var visitor = new ExpressionTreeConversionWithDeBruijn();
            var expressionTree = visitor.Visit(expression);
            return base.Rewrite(expressionTree);
        }
    }
}
