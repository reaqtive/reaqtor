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
        /// Visits a new expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The new expression to visit.</param>
        /// <returns>The result of optimizing the new expression.</returns>
        protected override Expression VisitNewCore(NewExpression node)
        {
            var res = (NewExpression)base.VisitNewCore(node);

            AssertTypes(node, res);

            var analysis = AnalyzeLeftToRight(first: null, res.Arguments);

            if (analysis.AllConstant && CanConstantFold(node))
            {
                if (node.Constructor != null)
                {
                    if (IsPure(node.Constructor))
                    {
                        return EvaluateNew(res);
                    }
                }
                else
                {
                    // NB: We need the constant folding check here as well (see above) because the result of
                    //     evaluating a value type creation can result in a singleton boxed instance which
                    //     could be mutated if the ConstantExpression.Value property is passed to reflection
                    //     APIs which can perform in-place mutations.

                    if (IsPure(node.Type))
                    {
                        return EvaluateNew(res);
                    }
                }
            }
            else if (analysis.Throw != null)
            {
                return ChangeType(analysis.Throw, res.Type);
            }

            return res;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Evaluates the constructor to perform constant folding.
        /// </summary>
        /// <param name="node">The new expression to evaluate.</param>
        /// <returns>An expression containing the result of evaluating the specified new expression.</returns>
        protected virtual Expression EvaluateNew(NewExpression node)
        {
            var evaluator = node.Constructor != null ? GetEvaluator(node.Constructor) : GetEvaluator(node.Type);

            return Evaluate(node, evaluator, instance: null, node.Arguments);
        }

#pragma warning restore CA1711
#pragma warning restore IDE0079
    }
}
