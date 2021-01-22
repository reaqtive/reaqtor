// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    internal static class BottomUpExpressionRewriterUtils
    {
        public static void GetConvertAndPredicate<TLeaf, TTarget>(Expression<Func<TLeaf, TTarget>> convert, Expression<Func<TLeaf, bool>> predicate, out Expression<Func<ExpressionTree, TTarget>> convertLambda, out Func<ExpressionTreeBase, TTarget> convertFunction, out Expression<Func<ExpressionTree, bool>> predicateLambda, out Func<ExpressionTreeBase, bool> predicateFunction)
            where TLeaf : Expression
        {
            var expressionProperty = (PropertyInfo)ReflectionHelpers.InfoOf((ExpressionTree et) => et.Expression);

            var convertParameter = Expression.Parameter(typeof(ExpressionTree), convert.Parameters[0].Name);
            var convertLambdaRaw = Expression.Lambda<Func<ExpressionTree, TTarget>>(
                Expression.Invoke(
                    convert,
                    Expression.Convert(
                        Expression.Property(
                            convertParameter,
                            expressionProperty
                        ),
                        typeof(TLeaf)
                    )
                ),
                convertParameter
            );

            convertLambda = (Expression<Func<ExpressionTree, TTarget>>)BetaReducer.Reduce(convertLambdaRaw, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);

            var convertFunctionStrong = convertLambda.Compile();

            convertFunction = new Func<ExpressionTreeBase, TTarget>(etb =>
            {
                return convertFunctionStrong((ExpressionTree)etb);
            });

            var predicateParameter = Expression.Parameter(typeof(ExpressionTree), predicate.Parameters[0].Name);
            var predicateLambdaRaw = Expression.Lambda<Func<ExpressionTree, bool>>(
                Expression.AndAlso(
                    Expression.TypeIs(
                        Expression.Property(
                            predicateParameter,
                            expressionProperty
                        ),
                        typeof(TLeaf)
                    ),
                    Expression.Invoke(
                        predicate,
                        Expression.Convert(
                            Expression.Property(
                                predicateParameter,
                                expressionProperty
                            ),
                            typeof(TLeaf)
                        )
                    )
                ),
                predicateParameter
            );

            predicateLambda = (Expression<Func<ExpressionTree, bool>>)BetaReducer.Reduce(predicateLambdaRaw, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);

            var predicateFunctionStrong = predicateLambda.Compile();

            predicateFunction = new Func<ExpressionTreeBase, bool>(etb =>
            {
                return etb is ExpressionTree et && predicateFunctionStrong(et);
            });
        }
    }
}
