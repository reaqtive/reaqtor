// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        // CONSIDER: Relocate this code to a common library.

        /// <summary>
        /// Provides a set of helpers to deal with expressions coming in to the engine.
        /// </summary>
        private static class ExpressionHelpers
        {
            public static Expression Detupletize(Expression expression)
            {
                var detupletized = new InvocationDetupletizer().Visit(expression);

                // TODO: Check if the lambda parameter is tuple.
                if (detupletized is LambdaExpression lambda)
                {
                    detupletized = ExpressionTupletizer.Unpack(lambda);
                }

                return detupletized;
            }

            private sealed class InvocationDetupletizer : ScopedExpressionVisitor<ParameterExpression>
            {
                protected override Expression VisitInvocation(InvocationExpression node)
                {
                    if (node.Expression is ParameterExpression function && IsUnboundParameter(function) && node.Arguments.Count == 1 && IsTuple(node.Arguments[0]))
                    {
                        var args = ExpressionTupletizer.Unpack(Visit(node.Arguments[0]));

                        var newFunctionType = Expression.GetDelegateType(args.Select(a => a.Type).Concat(new[] { node.Type }).ToArray());

                        var newFunction = Expression.Parameter(newFunctionType, function.Name);

                        return Expression.Invoke(newFunction, args);
                    }

                    return base.VisitInvocation(node);
                }

                protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

                private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);

                private static bool IsTuple(Expression expression)
                {
                    if (expression.NodeType == ExpressionType.New)
                    {
                        //
                        // CONSIDER: All of this code really should move up, closer to where we know we're using tuples. We may want to use
                        //           different calling conventions, e.g. curried functions, or functions with records as parameters (which
                        //           could be data model types, thus easy to persist), or combinations thereof. Tuples have just turned out
                        //           to be a pragmatic first choice to squeeze N-ary functions through a unary function.
                        //

                        if (expression.Type.FullName.StartsWith("System.Tuple`", StringComparison.Ordinal)
                            && expression.Type.Assembly == typeof(Tuple<>).Assembly)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }
    }
}
