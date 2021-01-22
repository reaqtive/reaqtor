// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Linq.Expressions
{
    /// <summary>
    /// Optimizer for expression trees.
    /// </summary>
    public partial class ExpressionOptimizer : LvalExpressionVisitor
    {
        /// <summary>
        /// Creates a new expression optimizer instance using the specified semantic <paramref name="semanticProvider"/>
        /// and the specified evaluator <paramref name="evaluatorFactory"/>.
        /// </summary>
        /// <param name="semanticProvider">The semantic provider to use when performing various checks against expressions and reflection objects.</param>
        /// <param name="evaluatorFactory">The evaluator factory to use when evaluating a member or an expression at compile time.</param>
        public ExpressionOptimizer(ISemanticProvider semanticProvider, IEvaluatorFactory evaluatorFactory)
        {
            SemanticProvider = semanticProvider;
            EvaluatorFactory = evaluatorFactory;
        }

        /// <summary>
        /// Gets the semantic provider to use when performing various checks against expressions and reflection objects.
        /// </summary>
        public ISemanticProvider SemanticProvider { get; }

        /// <summary>
        /// Gets the evaluator factory to use when evaluating a member or an expression at compile time.
        /// </summary>
        public IEvaluatorFactory EvaluatorFactory { get; }

        /// <summary>
        /// Visits and expression tree and returns the optimized result.
        /// </summary>
        /// <param name="node">The expression tree to visit.</param>
        /// <returns>An optimized rewritten expression, or the original expression if nothing was optimized.</returns>
        public override Expression Visit(Expression node)
        {
            if (node != null && ShouldOptimize(node))
            {
                var exp = VisitPreOptimize(node);

                AssertTypes(node, exp);

                var opt = base.Visit(exp);

                AssertTypes(opt, exp);

                if (node != opt)
                {
                    var res = VisitPostOptimize(node, opt);

                    AssertTypes(res, opt);

                    return res;
                }
                else
                {
                    return node;
                }
            }

            return node;
        }

        /// <summary>
        /// Checks if the specified expression should be optimized.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <returns>true if the expression should be optimized; otherwise, false.</returns>
        /// <remarks>
        /// This method supports the suppression of optimizations to various nodes where optimization
        /// may be undesirable. An example is a node of type <see cref="ExpressionType.Quote"/>.
        /// </remarks>
        protected virtual bool ShouldOptimize(Expression node) => true;

        /// <summary>
        /// Visits the specified expression prior to applying optimizations.
        /// </summary>
        /// <param name="node">The expression to visit prior to applying optimizations.</param>
        /// <returns>The result of visiting the specified expression prior to optimization.</returns>
        /// <remarks>
        /// This method supports plugging in pre-optimization rewrites or logging steps.
        /// </remarks>
        protected virtual Expression VisitPreOptimize(Expression node) => node;

        /// <summary>
        /// Visits an expression after optimizations have been applied.
        /// </summary>
        /// <param name="original">The original expression returned from <see cref="VisitPreOptimize"/>.</param>
        /// <param name="optimized">The result of applying optimizations to the <paramref name="original"/> expression.</param>
        /// <returns>The expression to return from the visitor.</returns>
        /// <remarks>
        /// This method supports plugging in post-optimization rewrites or logging steps. It can also
        /// be used to undo an optimization.
        /// </remarks>
        protected virtual Expression VisitPostOptimize(Expression original, Expression optimized)
        {
            AssertTypes(original, optimized);

            return optimized;
        }
    }
}
