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
        /// Visits a new array expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The new array expression to visit.</param>
        /// <returns>The result of optimizing the new array expression.</returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            var res = (NewArrayExpression)base.VisitNewArray(node);

            AssertTypes(node, res);

            var analysis = AnalyzeLeftToRight(first: null, res.Expressions);

            if (analysis.Throw != null)
            {
                return ChangeType(analysis.Throw, res.Type);
            }
            else if (analysis.AllConstant && CanConstantFold(node))
            {
                return Evaluate(node);
            }

            return res;
        }

        /// <summary>
        /// Evaluates a new array expression.
        /// </summary>
        /// <param name="node">The new array expression to evaluate.</param>
        /// <returns>
        /// An expression representing the result of evaluating the expression.
        /// </returns>
        private Expression Evaluate(NewArrayExpression node)
        {
            var evaluator = GetEvaluator(node);

            // NB: Array creation should never throw, except for OutOfMemoryException which we can't do much
            //     about anyway.

            var obj = evaluator();

            return Constant(node, obj, node.Type);
        }
    }
}
