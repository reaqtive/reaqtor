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
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Visits a conditional expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The conditional expression to visit.</param>
        /// <returns>The result of optimizing the conditional expression.</returns>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            // REVIEW: Should we visit some nodes as `const`?

            var res = (ConditionalExpression)base.VisitConditional(node);

            AssertTypes(node, res);

            var test = res.Test;
            var ifTrue = res.IfTrue;
            var ifFalse = res.IfFalse;

            if (AlwaysThrows(test))
            {
                return ChangeType(test, res.Type);
            }

            // REVIEW: Omitting a check for constant folding here; the node returns either of
            //         the child expression, so this node can't introduce a new mutable value.

            if (/* CanConstantFold(node) && */ HasConstantValue(test))
            {
                var testValue = (bool)GetConstantValue(test);

                return ChangeType(testValue ? ifTrue : ifFalse, res.Type);
            }

            return FlattenConditional(res);
        }

        /// <summary>
        /// Flattens nested conditional expressions by using <see cref="ExpressionType.AndAlso"/> nodes
        /// to combine conditions.
        /// </summary>
        /// <param name="node">The node to flatten.</param>
        /// <returns>The result of flattening the conditional expression.</returns>
        private static Expression FlattenConditional(ConditionalExpression node)
        {
            if (node.IfFalse.NodeType == ExpressionType.Default)
            {
                var current = node;

                var test = current.Test;
                var ifTrue = current.IfTrue;

                while (IsIfThen(ifTrue, out current))
                {
                    test = Expression.AndAlso(test, current.Test);
                    ifTrue = current.IfTrue;
                }

                return node.Update(test, ifTrue, node.IfFalse);
            }

            return node;
        }

        /// <summary>
        /// Checks if the specified <paramref name="node"/> is a conditional expression with a default
        /// value for <see cref="ConditionalExpression.IfFalse"/>.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <param name="next">The expression converted to a <see cref="ConditionalExpression"/>, if possible.</param>
        /// <returns>
        /// <c>true</c> if the specified <paramref name="node"/> is a conditional with a default value;
        /// otherwise, <c>false</c>.
        /// </returns>
        private static bool IsIfThen(Expression node, out ConditionalExpression next)
        {
            next = node as ConditionalExpression;
            return next != null && next.IfFalse.NodeType == ExpressionType.Default;
        }
    }
}
