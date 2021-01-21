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
        /// Visits an index expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The index expression to visit.</param>
        /// <param name="isLval">Indicates whether the node occurs in an lval position.</param>
        /// <returns>The result of optimizing the index expression.</returns>
        protected override Expression VisitIndex(IndexExpression node, bool isLval)
        {
            var res = (IndexExpression)base.VisitIndex(node, isLval);

            AssertTypes(node, res);

            if (!isLval)
            {
                var analysis = AnalyzeLeftToRight(res.Object, res.Arguments);

                if (analysis.AllPure)
                {
                    if (IsAlwaysNull(res.Object))
                    {
                        return Throw(res, NullReferenceException, res.Type);
                    }
                }

                if (analysis.AllConstant)
                {
                    // NB: When the indexer is null, we're array indexing and there's no need to
                    //     check for constant folding because the node can't introduce a new value.

                    if (res.Indexer == null || (CanConstantFold(node) && IsPure(res.Indexer)))
                    {
                        return EvaluateIndex(res);
                    }
                }

                if (analysis.Throw != null)
                {
                    return ChangeType(analysis.Throw, res.Type);
                }

                // CONSIDER: We could support `new[] { x0, x1, ..., xn }[i] -> xi` and `(new T[n])[i] -> default(T)`.
            }

            return res;
        }

        /// <summary>
        /// Evaluates the indexer to perform constant folding.
        /// </summary>
        /// <param name="node">The index expression to evaluate.</param>
        /// <returns>An expression containing the result of evaluating the specified index expression.</returns>
        protected virtual Expression EvaluateIndex(IndexExpression node)
        {
            if (node.Indexer == null)
            {
                var array = (Array)GetConstantValue(node.Object);

                object value;

                var dimensions = node.Arguments.Count;

                if (dimensions == 1)
                {
                    var index = (int)GetConstantValue(node.Arguments[0]);

                    try
                    {
                        value = array.GetValue(index);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return Throw(node, IndexOutOfRangeException, node.Type);
                    }
                }
                else
                {
                    var indices = new int[dimensions];

                    for (var i = 0; i < dimensions; i++)
                    {
                        indices[i] = (int)GetConstantValue(node.Arguments[i]);
                    }

                    try
                    {
                        value = array.GetValue(indices);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return Throw(node, IndexOutOfRangeException, node.Type);
                    }
                }

                return Constant(node, value, node.Type);
            }
            else
            {
                var evaluator = GetEvaluator(node.Indexer);

                return Evaluate(node, evaluator, node.Object, node.Arguments);
            }
        }
    }
}
