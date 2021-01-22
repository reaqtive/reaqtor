// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Optimizer for query operator applications using algebraic identities.
//
// BD - September 2014
//
//
// Design notes:
//
//   This optimizer leverages algebraic identities for query operators to perform domain-
//   specific optimizations. Example include:
//
//     xs.Take(n).Take(m)       ==   xs.Take(min(n, m))
//     xs.Where(f).Where(g)     ==   xs.Where(x => f(x) && g(x))
//     xs.Select(p).Select(q)   ==   xs.Select(x => q(p(x)))        [not implemented yet]
//

using System;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Optimizer for query expressions using algebraic identities of query operators.
    /// </summary>
    internal class QueryOperatorOptimizer : ExpressionVisitor
    {
        // NOTE: some of this could be done using BURS

        /// <summary>
        /// Optimizes the specified query expression using algebraic identities of query operators.
        /// </summary>
        /// <param name="expression">Expression to optimize.</param>
        /// <returns>Optimized query expression.</returns>
        public Expression Optimize(Expression expression)
        {
            // Monotonicity guaranteed because the rewrite only reduces the number of nodes.

            var newExpr = expression;
            Expression oldExpr;
            do
            {
                oldExpr = newExpr;
                newExpr = Visit(oldExpr);
            } while (oldExpr != newExpr);

            return newExpr;
        }

        /// <summary>
        /// Analyzes invocation expressions for the typical pattern of invoking a known resource in order to apply query optimizations based on algebraic identities of query operators.
        /// </summary>
        /// <param name="node">Expression to analyze.</param>
        /// <returns>Expression after applying query operator optimizations based on algebraic identities of query operators, if any apply.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            if (node.Expression is ParameterExpression p) // omitted unbound parameter check
            {
                switch (p.Name)
                {
                    //
                    // xs.Take(n).Take(m) == xs.Take(Min(n, m))
                    //
                    case "rx://operators/take":
                        {
                            if (node.Arguments[0] is InvocationExpression i && node.Arguments[1] is ConstantExpression c)
                            {
                                if (i.Expression is ParameterExpression q) // TODO: omitted unbound parameter check; use scoped visitor
                                {
                                    if (q.Name == "rx://operators/take")
                                    {
                                        if (i.Arguments[1] is ConstantExpression d)
                                        {
                                            var res = Visit(i.Arguments[0]);
                                            var z = Math.Min((int)c.Value, (int)d.Value);
                                            return Expression.Invoke(p, res, Expression.Constant(z));
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    //
                    // xs.Where(f).Where(g) == xs.Where(x => f(x) && g(x))
                    //
                    case "rx://operators/filter":
                        {
                            if (node.Arguments[0] is InvocationExpression i && node.Arguments[1] is LambdaExpression f1)
                            {
                                if (i.Expression is ParameterExpression q) // TODO: omitted unbound parameter check; use scoped visitor
                                {
                                    if (q.Name == "rx://operators/filter")
                                    {
                                        if (i.Arguments[1] is LambdaExpression f2)
                                        {
                                            var par1 = f1.Parameters[0];
                                            var par2 = f2.Parameters[0];

                                            var body1 = f1.Body;
                                            var body2 = new SimpleSubst(par2, par1).Visit(f2.Body);

                                            var body = Expression.AndAlso(body1, body2);
                                            var filter = Expression.Lambda(body, par1);

                                            var res = Visit(i.Arguments[0]);

                                            return Expression.Invoke(p, res, filter);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            return base.VisitInvocation(node);
        }

        /// <summary>
        /// Expression visitor to carry out simple substitutions of parameter expressions.
        /// </summary>
        private class SimpleSubst : ExpressionVisitor
        {
            /// <summary>
            /// Parameter expression to find and substitute.
            /// </summary>
            private readonly ParameterExpression _p;

            /// <summary>
            /// Parameter expression to put in place of occurrences of the original parameter expression occurrences.
            /// </summary>
            private readonly ParameterExpression _q;

            /// <summary>
            /// Creates a new parameter substitution visitor.
            /// </summary>
            /// <param name="p">Parameter expression to find and substitute.</param>
            /// <param name="q">Parameter expression to put in place of occurrences of the original parameter expression occurrences.</param>
            public SimpleSubst(ParameterExpression p, ParameterExpression q)
            {
                _p = p;
                _q = q;
            }

            /// <summary>
            /// Visits parameter expressions to find and replace occurrences of the specified parameter expression.
            /// </summary>
            /// <param name="node">Parameter expression to analyze.</param>
            /// <returns>The original parameter expression if it doesn't correspond to the parameter expression to find and substitute; otherwise, the new parameter expression used as the target of the substitution.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                // TODO: omitted scope checks; use scoped visitor

                if (node == _p)
                {
                    return _q;
                }

                return base.VisitParameter(node);
            }
        }
    }
}
