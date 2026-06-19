// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    internal class DeploymentReactiveServiceContext : ReactiveServiceContext
    {
        public DeploymentReactiveServiceContext(IReactiveEngineProvider provider)
            : base(new ExpressionServices(), provider)
        {
        }

        private sealed class ExpressionServices : ReactiveExpressionServices
        {
            public ExpressionServices()
                : base(typeof(IReactiveClient))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var normalized = base.Normalize(expression);

                var tupletized = new InvocationTupletizer().Visit(normalized);
                if (tupletized is LambdaExpression lambda)
                {
                    return ExpressionTupletizer.Pack(lambda);
                }

                return tupletized;
            }
        }

        // TODO: Move InvocationTupletizer to a common location.

        /// <summary>
        /// Tupletizer for unbound parameter invocation sites.
        /// </summary>
        private class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
        {
            /// <summary>
            /// Visits the children of the <see cref="System.Linq.Expressions.InvocationExpression" />.
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
            protected override ParameterExpression GetState(ParameterExpression parameter)
            {
                return parameter;
            }

            /// <summary>
            /// Determines whether the specified parameter is unbound.
            /// </summary>
            /// <param name="parameter">The parameter to check.</param>
            /// <returns>
            ///   <c>true</c> if the specified parameter is unbound; otherwise, <c>false</c>.
            /// </returns>
            private bool IsUnboundParameter(ParameterExpression parameter)
            {
                return !TryLookup(parameter, out _);
            }
        }
    }
}
