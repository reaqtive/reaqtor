// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2014 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// An optimizer that coalesces adjacent operators if semantic equivalence can be maintained.
    /// </summary>
    public class CoalescingOptimizer : QueryVisitor, IOptimizer
    {
        /// <summary>
        /// Optimizes the given query expression tree by coalescing adjacent operators.
        /// </summary>
        /// <param name="queryTree">The query expression tree to optimize.</param>
        /// <returns>An optimized version of the input query tree.</returns>
        public QueryTree Optimize(QueryTree queryTree) => Visit(queryTree);

        /// <summary>
        /// Visits a <see cref="FirstOperator" /> and coalesces it with any possible child operators.
        /// </summary>
        /// <param name="op">The operator to visit.</param>
        /// <returns>The coalesced operator.</returns>
        protected internal override QueryOperator VisitFirst(FirstOperator op)
        {
            var source = VisitAndConvert<MonadMember>(op.Source);
            if (source.QueryNodeType == QueryNodeType.Operator)
            {
                var sourceOp = (QueryOperator)source;
                if (sourceOp.NodeType == OperatorType.Where)
                {
                    var where = (WhereOperator)sourceOp;
                    if (op.ElementType == where.ElementType) // new C[0].Where<B>(_ => B.b).First<A>() can't be optimized
                    {
                        return op.QueryExpressionFactory.First(
                            op.ElementType,
                            where.Source,
                            where.Predicate);
                    }
                }
            }

            return op.Update(source);
        }

        /// <summary>
        /// Visits a <see cref="WhereOperator" /> and coalesces it with any possible child operators.
        /// </summary>
        /// <param name="op">The operator to visit.</param>
        /// <returns>The coalesced operator.</returns>
        protected internal override QueryOperator VisitWhere(WhereOperator op)
        {
            var source = VisitAndConvert<MonadMember>(op.Source);
            var predicate = Visit(op.Predicate);

            var srcOperator = source.QueryNodeType == QueryNodeType.Operator ? (QueryOperator)source : null;
            var srcWhere = srcOperator != null && srcOperator.NodeType == OperatorType.Where ? (WhereOperator)srcOperator : null;
            if (srcWhere != null && op.ElementType == srcWhere.ElementType)
            {
                var p = Expression.Parameter(op.ElementType);
                if (TryExtractNullaryLambdaBody(srcWhere.Predicate, out Expression predicate1) && TryExtractNullaryLambdaBody(predicate, out Expression predicate2))
                {
                    var reduced1 = BetaReducer.Reduce(Expression.Invoke(predicate1, p));
                    var reduced2 = BetaReducer.Reduce(Expression.Invoke(predicate2, p));

                    return op.QueryExpressionFactory.Where(
                        op.ElementType,
                        srcWhere.Source,
                        DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Lambda(
                                    Expression.AndAlso(reduced1, reduced2),
                                    p
                                )
                            ),
                            Array.Empty<QueryTree>()
                        )
                    );
                }

                var predicateType = typeof(Func<,>).MakeGenericType(p.Type, typeof(bool));
                var p1 = Expression.Parameter(predicateType);
                var p2 = Expression.Parameter(predicateType);
                return op.QueryExpressionFactory.Where(
                    op.ElementType,
                    srcWhere.Source,
                    DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                        Expression.Lambda(
                            Expression.Lambda(
                                Expression.AndAlso(
                                    Expression.Invoke(p1, p),
                                    Expression.Invoke(p2, p)
                                ),
                                p
                            ),
                            p1,
                            p2
                        ),
                        new[] { srcWhere.Predicate, predicate }
                    )
                );
            }

            return op.Update(source, predicate);
        }

        /// <summary>
        /// Visits a <see cref="SelectOperator" /> and coalesces it with any possible child operators.
        /// </summary>
        /// <param name="op">The operator to visit.</param>
        /// <returns>The coalesced operator.</returns>
        protected internal override QueryOperator VisitSelect(SelectOperator op)
        {
            var source = VisitAndConvert<MonadMember>(op.Source);
            var selector = Visit(op.Selector);

            var srcOperator = source.QueryNodeType == QueryNodeType.Operator ? (QueryOperator)source : null;
            var srcSelect = srcOperator != null && srcOperator.NodeType == OperatorType.Select ? (SelectOperator)srcOperator : null;
            if (srcSelect != null && op.InputElementType == srcSelect.ElementType)
            {
                var p = Expression.Parameter(srcSelect.InputElementType);

                // Optimization when both lambda abstractions don't abstract over anything
                if (TryExtractNullaryLambdaBody(srcSelect.Selector, out Expression selector1) && TryExtractNullaryLambdaBody(selector, out Expression selector2))
                {
                    var reduced = BetaReducer.Reduce(Expression.Invoke(selector2, Expression.Invoke(selector1, p)));
                    return op.QueryExpressionFactory.Select(
                        op.ElementType,
                        srcSelect.InputElementType,
                        srcSelect.Source,
                        DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Lambda(
                                    reduced,
                                    p
                                )
                            ),
                            Array.Empty<QueryTree>()
                        )
                    );
                }

                var p1 = Expression.Parameter(typeof(Func<,>).MakeGenericType(srcSelect.InputElementType, srcSelect.ElementType));
                var p2 = Expression.Parameter(typeof(Func<,>).MakeGenericType(op.InputElementType, op.ElementType));
                return op.QueryExpressionFactory.Select(
                    op.ElementType,
                    srcSelect.InputElementType,
                    srcSelect.Source,
                    DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                        Expression.Lambda(
                            Expression.Lambda(
                                Expression.Invoke(p2, Expression.Invoke(p1, p)),
                                p
                            ),
                            p1,
                            p2
                        ),
                        new[] { srcSelect.Selector, selector }
                    )
                );
            }

            return op.Update(source, selector);
        }

        private static readonly MethodInfo s_Min = (MethodInfo)ReflectionHelpers.InfoOf(() => Math.Min(0, 0));

        /// <summary>
        /// Visits a <see cref="TakeOperator" /> operator and coalesces it with any possible child operators.
        /// </summary>
        /// <param name="op">The operator to visit.</param>
        /// <returns>The coalesced operator.</returns>
        protected internal override QueryOperator VisitTake(TakeOperator op)
        {
            var source = VisitAndConvert<MonadMember>(op.Source);
            var count = Visit(op.Count);

            // Take coalescing
            var srcOperator = source.QueryNodeType == QueryNodeType.Operator ? (QueryOperator)source : null;
            var srcTake = srcOperator != null && srcOperator.NodeType == OperatorType.Take ? (TakeOperator)srcOperator : null;
            if (srcTake != null)
            {
                if (TryExtractNullaryLambdaBody(count, out Expression count1) &&
                    count1.NodeType == ExpressionType.Constant &&
                    TryExtractNullaryLambdaBody(srcTake.Count, out Expression count2) &&
                    count2.NodeType == ExpressionType.Constant)
                {
                    var c1 = (int)((ConstantExpression)count1).Value;
                    var c2 = (int)((ConstantExpression)count2).Value;

                    return op.QueryExpressionFactory.Take(
                        op.ElementType,
                        srcTake.Source,
                        DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Constant(
                                    Math.Min(c1, c2)
                                )
                            ),
                            Array.Empty<QueryTree>()
                        )
                    );
                }

                var p1 = Expression.Parameter(typeof(int));
                var p2 = Expression.Parameter(typeof(int));
                return op.QueryExpressionFactory.Take(
                    op.ElementType,
                    srcTake.Source,
                    DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                        Expression.Lambda(
                            Expression.Call(s_Min, p1, p2),
                            p1,
                            p2
                        ),
                        new[] { srcTake.Count, count }
                    )
                );
            }

            return op.Update(source, count);
        }

        private static bool TryExtractNullaryLambdaBody(QueryTree queryTree, out Expression result)
        {
            if (queryTree.QueryNodeType == QueryNodeType.Lambda)
            {
                var lambda = (LambdaAbstraction)queryTree;
                if (lambda.Parameters.Count == 0)
                {
                    result = lambda.Body.Body;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
