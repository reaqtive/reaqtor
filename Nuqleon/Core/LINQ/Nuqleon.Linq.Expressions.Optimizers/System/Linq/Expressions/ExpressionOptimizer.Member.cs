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
        /// Visits a member expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The member expression to visit.</param>
        /// <param name="isLval">Indicates whether the node occurs in an lval position.</param>
        /// <returns>The result of optimizing the member expression.</returns>
        protected override Expression VisitMember(MemberExpression node, bool isLval)
        {
            var res = (MemberExpression)base.VisitMember(node, isLval);

            AssertTypes(node, res);

            var expression = res.Expression;

            if (expression == null)
            {
                if (IsPure(node.Member))
                {
                    return EvaluateMember(res);
                }
            }
            else
            {
                if (AlwaysThrows(expression))
                {
                    return ChangeType(expression, res.Type);
                }

                if (IsAlwaysNull(expression))
                {
                    return Throw(res, NullReferenceException, res.Type);
                }

                if (CanConstantFold(node) && HasConstantValue(expression) && IsPure(node.Member))
                {
                    return EvaluateMember(res);
                }
            }

            return res;
        }

        /// <summary>
        /// Evaluates the member to perform constant folding.
        /// </summary>
        /// <param name="node">The member expression to evaluate.</param>
        /// <returns>An expression containing the result of evaluating the specified member expression.</returns>
        protected virtual Expression EvaluateMember(MemberExpression node)
        {
            var evaluator = GetEvaluator(node.Member);

            return Evaluate(node, evaluator, node.Expression, arguments: null);
        }
    }
}
