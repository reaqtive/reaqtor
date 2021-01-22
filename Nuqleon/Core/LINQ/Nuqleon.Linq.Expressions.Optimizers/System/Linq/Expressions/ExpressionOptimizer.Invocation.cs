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
        /// Lazily instantiated beta reducer instance.
        /// </summary>
        private StrictBetaReducer _betaReducer;

        /// <summary>
        /// Visits an invocation to perform optimization steps.
        /// </summary>
        /// <param name="node">The invocation to visit.</param>
        /// <returns>The result of optimizing the invocation.</returns>
        protected override Expression VisitInvocationCore(InvocationExpression node)
        {
            var res = (InvocationExpression)base.VisitInvocationCore(node);

            AssertTypes(node, res);

            if (res.Arguments.Count == 1 && IsIdentityFunction(res.Expression))
            {
                return ChangeType(res.Arguments[0], res.Type);
            }

            if (res.Expression.NodeType == ExpressionType.Lambda)
            {
                return VisitLambdaInvocation(res);
            }

            var analysis = AnalyzeLeftToRight(res.Expression, res.Arguments);

            if (analysis.AllPure)
            {
                if (IsAlwaysNull(res.Expression))
                {
                    return Throw(res, NullReferenceException, res.Type);
                }
            }

            if (analysis.AllConstant)
            {
                if (CanConstantFold(node) && IsInvocationTargetPure(res))
                {
                    return EvaluateInvocation(res);
                }
            }

            if (analysis.Throw != null)
            {
                return ChangeType(analysis.Throw, res.Type);
            }

            return res;
        }

        /// <summary>
        /// Visits and invocation of a lambda expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The invocation to visit.</param>
        /// <returns>The result of optimizing the invocation.</returns>
        protected virtual Expression VisitLambdaInvocation(InvocationExpression node)
        {
            var lambda = (LambdaExpression)node.Expression;

            var analysis = AnalyzeLeftToRight(first: null, node.Arguments);

            if (analysis.AllConstant)
            {
                if (AlwaysThrows(lambda.Body))
                {
                    return ChangeType(lambda.Body, node.Type);
                }

                //
                // CONSIDER: Provide more control to the user in order to avoid the inlining of
                //           the same constant many times, which may result in a bigger expression
                //           (e.g. when it gets serialized).
                //

                if (_betaReducer == null)
                {
                    _betaReducer = new StrictBetaReducer(SemanticProvider);
                }

                if (_betaReducer.TryReduce(node, out var res))
                {
                    AssertTypes(node, res);

                    //
                    // NB: We should visit the result again in order to do more constant folding
                    //     that may have become possible due to the beta reduction.
                    //
                    //     E.g. ((x, y) => x + y)(1, 2)  -->  1 + 2
                    //
                    return Visit(res);
                }
            }
            else if (analysis.Throw != null)
            {
                return ChangeType(analysis.Throw, node.Type);
            }

            return node;
        }

        /// <summary>
        /// Evaluates an invocation expression where the invocation target is a delegate.
        /// </summary>
        /// <param name="node">The invocation expression to evaluate.</param>
        /// <returns>An expression containing the result of evaluating the specified invocation expression.</returns>
        protected virtual Expression EvaluateInvocation(InvocationExpression node)
        {
            var d = (Delegate)GetConstantValue(node.Expression);

            var evaluator = GetEvaluator(d.Method);

            return Evaluate(node, evaluator, instance: null, node.Arguments);
        }

        /// <summary>
        /// Checks if the invocation target of the specified expression is considered a pure function.
        /// </summary>
        /// <param name="node">The invocation expression whose invocation target to check for purity.</param>
        /// <returns><c>true</c> if the invocation target is pure; otherwise, <c>false</c>.</returns>
        private bool IsInvocationTargetPure(InvocationExpression node)
        {
            var d = (Delegate)GetConstantValue(node.Expression);

            foreach (var i in d.GetInvocationList())
            {
                //
                // NB: Multicast delegates can have multiple invocations that all have to be pure
                //     in order for the result to be pure. If any of them has an observable side-
                //     effect, we can't perform the evaluation at compile time. However, if any
                //     of the invocation targets throws, we're still okay (provided all preceding
                //     invocation targets are pure).
                //

                if (!IsPure(i.Method))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
