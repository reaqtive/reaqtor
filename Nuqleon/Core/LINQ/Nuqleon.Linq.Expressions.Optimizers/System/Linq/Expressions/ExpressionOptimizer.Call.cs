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
        /// Visits a method call expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The method call expression to visit.</param>
        /// <returns>The result of optimizing the method call expression.</returns>
        protected override Expression VisitMethodCallCore(MethodCallExpression node)
        {
            var res = (MethodCallExpression)base.VisitMethodCallCore(node);

            AssertTypes(node, res);

            var analysis = AnalyzeLeftToRight(res.Object, res.Arguments);

            if (analysis.AllPure)
            {
                if (res.Object != null && IsAlwaysNull(res.Object))
                {
                    return Throw(res, NullReferenceException, res.Type);
                }
            }

            if (analysis.AllConstant)
            {
                if (IsPure(node.Method) && CanConstantFold(node))
                {
                    return EvaluateMethodCall(res);
                }
            }

            if (analysis.Throw != null)
            {
                return ChangeType(analysis.Throw, res.Type);
            }

            return res;
        }

        /// <summary>
        /// Evaluates the method call to perform constant folding.
        /// </summary>
        /// <param name="node">The method call expression to evaluate.</param>
        /// <returns>An expression containing the result of evaluating the specified method call expression.</returns>
        protected virtual Expression EvaluateMethodCall(MethodCallExpression node)
        {
            var evaluator = GetEvaluator(node.Method);

            return Evaluate(node, evaluator, node.Object, node.Arguments);
        }
    }
}
