// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Normalizer for predicates in order to increase sharing likelihood.
//
// BD - September 2014
//
//
// Design notes:
//
//   Given a predicate with parameter 'x', we want to move bound occurrences of the parameter
//   to the left-hand side of binary operators, whenever possible. For example:
//
//     x => 0 < x   becomes   x => x > 0
//
//   When bound occurrences are found on both sides of the operator, no change occurs.
//
//   Notice that prior rewrites can increase the likelihood for success of this rewrite. For
//   example, consider the following predicate:
//
//     xs.Where(x => 0 < x && 5 > x)
//
//   This rewriter can't be applied to this predicate given the top-level node type is AndAlso
//   rather than a comparison operator. By transforming this using algebraic identities for
//   the Where operator, we can obtain:
//
//     xs.Where(x => 0 < x).Where(x => 5 > x)
//
//   At this point, both predicates can be normalized using this utility, to obtain:
//
//     xs.Where(x => x > 0).Where(x => x < 5)
//

using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Normalizes predicates such that occurrences of bound parameters appear on the left-hand side of binary operators whenever possible.
    /// </summary>
    internal class PredicateNormalizer
    {
        /// <summary>
        /// Range variable for the predicates that get normalized.
        /// </summary>
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Creates a new predicate normalizer using the specified range variable for predicates.
        /// </summary>
        /// <param name="parameter">Range variable for the predicates that get normalized.</param>
        public PredicateNormalizer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        /// <summary>
        /// Normalizes the given expression by moving occurrences of the range variable to the left-hand side of a binary operator whenever possible.
        /// </summary>
        /// <param name="node">Expression to normalize.</param>
        /// <returns>Normalized expression.</returns>
        public Expression Normalize(Expression node)
        {
            if (node is BinaryExpression b)
            {
                var mirror = default(ExpressionType?);

                switch (b.NodeType)
                {
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                        mirror = b.NodeType;
                        break;
                    case ExpressionType.LessThan:
                        mirror = ExpressionType.GreaterThan;
                        break;
                    case ExpressionType.LessThanOrEqual:
                        mirror = ExpressionType.GreaterThanOrEqual;
                        break;
                    case ExpressionType.GreaterThan:
                        mirror = ExpressionType.LessThan;
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        mirror = ExpressionType.LessThanOrEqual;
                        break;
                }

                if (mirror != null)
                {
                    var fvs1 = new BoundParameterScanner(_parameter);
                    fvs1.Visit(b.Left);
                    var lb = fvs1.Bound;

                    var fvs2 = new BoundParameterScanner(_parameter);
                    fvs2.Visit(b.Right);
                    var rb = fvs2.Bound;

                    if (rb && !lb)
                    {
                        // TODO: omitted checks to ensure that the resulting operation is possible (e.g. checking for Int types)
                        return Expression.MakeBinary(mirror.Value, b.Right, b.Left);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Expression visitor to detect whether a given parameter is bound.
        /// </summary>
        private class BoundParameterScanner : ExpressionVisitor
        {
            /// <summary>
            /// Parameter to check for bound occurrences in visited expressions.
            /// </summary>
            private readonly ParameterExpression _p;

            /// <summary>
            /// Creates a new bound parameter scanner to check whether the specified parameter is bound.
            /// </summary>
            /// <param name="p">Parameter to check for bound occurrences in visited expressions.</param>
            public BoundParameterScanner(ParameterExpression p)
            {
                _p = p;
            }

            /// <summary>
            /// Gets a Boolean indicating whether the specified parameter was found to be bound in the visited expression.
            /// </summary>
            /// <remarks>
            /// <c>true</c> if the parameter was found to be bound; otherwise, <c>false</c>.
            /// </remarks>
            public bool Bound { get; private set; }

            /// <summary>
            /// Visits a parameter expression node to check whether it's bound.
            /// </summary>
            /// <param name="node">Parameter expression to check.</param>
            /// <returns>The original parameter expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                // TODO: omitted scope checking; replace using ScopedExpressionVisitor

                if (node == _p)
                {
                    Bound = true;
                }

                return base.VisitParameter(node);
            }
        }
    }
}
