// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting.ReificationFramework
{
    internal static class ExpressionHelpers
    {
        public static Expression BetaReduce(this Expression expression)
        {
            return BetaReducer.ReduceEager(
                expression,
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
        }

        public static Expression<T> BetaReduce<T>(this Expression<T> expression)
        {
            return (Expression<T>)BetaReduce((Expression)expression);
        }

        public static Expression<T> InlineClosures<T>(this Expression<T> expression)
        {
            return new ClosureEliminator().VisitAndConvert<Expression<T>>(expression, "InlineClosures");
        }

        private sealed class ClosureEliminator : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                var expression = Visit(node.Expression);
                if (expression != null && expression.Type.IsClosureClass())
                {
                    var value = Evaluate(node.Update(expression));
                    return Expression.Constant(value, node.Type);
                }

                return base.VisitMember(node);
            }

            private static object Evaluate(MemberExpression node)
            {
                var expression = node.Expression;
                if (node.Member is FieldInfo field && expression.NodeType == ExpressionType.Constant)
                {
                    return field.GetValue(((ConstantExpression)expression).Value);
                }

                return node.Evaluate();
            }
        }
    }
}
