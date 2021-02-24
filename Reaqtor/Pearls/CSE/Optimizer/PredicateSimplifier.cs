// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Trivial simplifier for predicates.
//
// BD - September 2014
//
//
// Design notes:
//
//   In order to increase the likelihood for two expressions to be recognized as the same
//   so sharing can occur, we simplify predicates based on algebraic identities. Examples
//   of such rewrites include:
//
//     S(!!x)          ~>  S(x)
//     S(!(l == r))    ~>  S(l) != S(r)
//     S(!(l || r))    ~>  !S(l) && !S(r)
//

using System;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Simplifier for predicates using algebraic identities.
    /// </summary>
    internal class PredicateSimplifier : ExpressionVisitor
    {
        /// <summary>
        /// Simplifies the specified expression using algebraic identities.
        /// </summary>
        /// <param name="expression">Expression to simplify.</param>
        /// <returns>Simplified expression.</returns>
        public Expression Simplify(Expression expression)
        {
            // Monotonicity guaranteed; rewrites either keep the number of nodes, or reduce the number of nodes by 1, or make forward progress (e.g. push down of NOT in De Morgan's rule).

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
        /// Visits unary expression nodes to rewrite negations to a simpler form, if possible.
        /// </summary>
        /// <param name="node">Unary expression to analyze and simplify, if possible.</param>
        /// <returns>The original expression if no simplifcation was applied; otherwise, the simplified expression.</returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            // TODO: omitted more rigorous checks to ensure that rewrites are possible

            if (node.NodeType == ExpressionType.Not && node.Method == null)
            {
                var o = node.Operand;
                switch (o.NodeType)
                {
                    case ExpressionType.Not:
                        {
                            var u = (UnaryExpression)o;
                            if (u.Method == null)
                            {
                                return Visit(u.Operand);
                            }
                        }
                        break;
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                        {
                            var b = (BinaryExpression)o;
                            if (b.Method == null && b.Conversion == null)
                            {
                                var l = Visit(b.Left);
                                var r = Visit(b.Right);
                                return Expression.MakeBinary(Invert(b.NodeType), l, r);
                            }
                        }
                        break;
                    case ExpressionType.Or:
                    case ExpressionType.OrElse: // assume pure functions so short-circuiting effect is irrelevant
                        {
                            var b = (BinaryExpression)o;
                            if (b.Method == null && b.Conversion == null)
                            {
                                var l = Visit(Expression.Not(b.Left));
                                var r = Visit(Expression.Not(b.Right));
                                return Expression.MakeBinary(Invert(b.NodeType), l, r);
                            }
                        }
                        break;
                }
            }

            return base.VisitUnary(node);
        }

        /// <summary>
        /// Inverts an expression type under application of NOT.
        /// </summary>
        /// <param name="t">Expression type to invert.</param>
        /// <returns>Inverted expression type.</returns>
        private static ExpressionType Invert(ExpressionType t)
        {
            return t switch
            {
                ExpressionType.Equal => ExpressionType.NotEqual,
                ExpressionType.NotEqual => ExpressionType.Equal,
                ExpressionType.LessThan => ExpressionType.GreaterThanOrEqual,
                ExpressionType.LessThanOrEqual => ExpressionType.GreaterThan,
                ExpressionType.GreaterThan => ExpressionType.LessThanOrEqual,
                ExpressionType.GreaterThanOrEqual => ExpressionType.LessThan,
                ExpressionType.Or => ExpressionType.And, // arbitrary; see short-circuiting remarks
                ExpressionType.OrElse => ExpressionType.AndAlso, // arbitrary; see short-circuiting remarks
                _ => throw new NotImplementedException(),
            };
        }
    }
}
