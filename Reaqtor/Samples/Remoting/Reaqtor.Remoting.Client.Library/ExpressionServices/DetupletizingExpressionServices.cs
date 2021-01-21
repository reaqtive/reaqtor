// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Expression services, which, after normalization, convert all invocation expressions
    /// and the root level lambda expression from a form using unary lambdas with tuple
    /// arguments to n-ary lambdas with tuple arguments unpacked.
    /// </summary>
    public class DetupletizingExpressionServices : ReactiveExpressionServices
    {
        /// <summary>
        /// Instantiates the expression services.
        /// </summary>
        public DetupletizingExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
        }

        /// <summary>
        /// Normalizes the specified expression prior to submission to the service.
        /// </summary>
        /// <param name="expression">The expression to normalize.</param>
        /// <returns>The normalized expression.</returns>
        public override Expression Normalize(Expression expression)
        {
            var normalized = base.Normalize(expression);
            return ExpressionHelpers.Detupletize(normalized);
        }

        // TODO: Relocate this code to a common library.

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
                        // TODO: All of this code really should move up, closer to where we know we're using tuples.

                        if (ExpressionTupletizer.IsTuple(expression.Type))
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
