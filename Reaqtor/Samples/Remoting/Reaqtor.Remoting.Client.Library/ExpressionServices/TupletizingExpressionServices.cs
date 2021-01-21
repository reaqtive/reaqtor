// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Expression services, which, after normalization, convert all invocation expressions
    /// and the root level lambda expression to a form using unary lambdas by tupletizing
    /// the arguments.
    /// </summary>
    public class TupletizingExpressionServices : ExpressionServices
    {
        /// <summary>
        /// Instantiates the expression services.
        /// </summary>
        public TupletizingExpressionServices(Type reactiveClientInterfaceType)
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
            return Tupletize(normalized);
        }

        /// <summary>
        /// Tupletizes an expression.
        /// </summary>
        /// <param name="expression">The expression to tupletize.</param>
        /// <returns>The tupletized expression.</returns>
        private static Expression Tupletize(Expression expression)
        {
            var inv = new InvocationTupletizer();
            var result = inv.Visit(expression);
            if (result is LambdaExpression lambda)
            {
                result = ExpressionTupletizer.Pack(lambda);
            }

            return result;
        }

        /// <summary>
        /// Tupletizer for unbound parameter invocation sites.
        /// </summary>
        private sealed class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
        {
            /// <summary>
            /// Visits the children of the <see cref="T:System.Linq.Expressions.InvocationExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var expr = Visit(node.Expression);
                var args = Visit(node.Arguments);

                if (expr.NodeType == ExpressionType.Parameter)
                {
                    var parameter = (ParameterExpression)expr;

                    // Turns f(x, y, z) into f((x, y, z)) when f is an unbound parameter, i.e. representing a known resource.
                    if (IsUnboundParameter(parameter))
                    {
                        if (args.Count > 0)
                        {
                            var tuple = ExpressionTupletizer.Pack(args);
                            var funcType = Expression.GetDelegateType(tuple.Type, node.Type);
                            var function = Expression.Parameter(funcType, parameter.Name);
                            return Expression.Invoke(function, tuple);
                        }
                    }
                }

                return node.Update(expr, args);
            }

            /// <summary>
            /// Gets the state associated with the specified parameter declaration.
            /// </summary>
            /// <param name="parameter">The parameter to obtain associated state for.</param>
            /// <returns>State associated with the specified parameter declaration.</returns>
            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            /// <summary>
            /// Determines whether the specified parameter is unbound.
            /// </summary>
            /// <param name="parameter">The parameter to check.</param>
            /// <returns>
            ///   <c>true</c> if the specified parameter is unbound; otherwise, <c>false</c>.
            /// </returns>
            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
        }
    }
}
