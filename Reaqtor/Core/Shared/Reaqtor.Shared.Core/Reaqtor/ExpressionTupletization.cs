// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this code in the Remoting sample's client library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor;

/// <summary>
/// Provides expression rewriting helpers to convert between n-ary invocations of unbound parameters
/// (representing known resources) and their unary tupletized forms, as used on the wire between
/// reactive service clients and services.
/// </summary>
public static class ExpressionTupletization
{
    /// <summary>
    /// Converts all invocation expressions of unbound parameters and the root level lambda expression,
    /// if any, to a form using unary lambdas by tupletizing the arguments, e.g. rewriting an invocation
    /// <c>f(x, y)</c> to <c>f((x, y))</c>.
    /// </summary>
    /// <param name="expression">The expression to tupletize.</param>
    /// <returns>The tupletized expression.</returns>
    /// <exception cref="NotSupportedException">
    /// The root level lambda expression has a tuple-typed parameter. The packed form cannot represent
    /// such lambdas unambiguously: after packing, the parameter's own tuple shape merges with the
    /// packing tuple, and <see cref="Detupletize"/> on the receiving side cannot distinguish the two,
    /// which would silently produce an incorrect expression.
    /// </exception>
    public static Expression Tupletize(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var result = new InvocationTupletizer().Visit(expression);

        if (result is LambdaExpression lambda)
        {
            foreach (var parameter in lambda.Parameters)
            {
                if (ExpressionTupletizer.IsTuple(parameter.Type))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Parameter '{0}' of the root level lambda expression has tuple type '{1}'. Lambda expressions with tuple-typed parameters cannot be represented unambiguously in the tupletized form and would not round-trip through Detupletize correctly.", parameter.Name, parameter.Type));
                }
            }

            result = ExpressionTupletizer.Pack(lambda);
        }

        return result;
    }

    /// <summary>
    /// Converts all invocation expressions of unbound parameters and the root level lambda expression,
    /// if any, from a form using unary lambdas with tuple arguments to n-ary lambdas with the tuple
    /// arguments unpacked, e.g. rewriting an invocation <c>f((x, y))</c> to <c>f(x, y)</c>.
    /// </summary>
    /// <param name="expression">The expression to detupletize.</param>
    /// <returns>The detupletized expression.</returns>
    /// <remarks>
    /// A root level lambda expression is only unpacked when it has exactly one parameter whose type is
    /// a tuple type; other lambda expressions pass through unchanged. Note that a unary lambda whose
    /// parameter is genuinely tuple-typed is indistinguishable from the packed form and is therefore
    /// always interpreted as packed; <see cref="Tupletize"/> refuses to produce such lambdas for the
    /// same reason.
    /// </remarks>
    public static Expression Detupletize(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var result = new InvocationDetupletizer().Visit(expression);

        if (result is LambdaExpression lambda && lambda.Parameters.Count == 1 && ExpressionTupletizer.IsTuple(lambda.Parameters[0].Type))
        {
            result = ExpressionTupletizer.Unpack(lambda);
        }

        return result;
    }

    /// <summary>
    /// Tupletizer for unbound parameter invocation sites.
    /// </summary>
    private sealed class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
    {
        /// <summary>
        /// Visits the children of the <see cref="InvocationExpression" />.
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

    /// <summary>
    /// Detupletizer for unbound parameter invocation sites.
    /// </summary>
    private sealed class InvocationDetupletizer : ScopedExpressionVisitor<ParameterExpression>
    {
        /// <summary>
        /// Visits the children of the <see cref="InvocationExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            // Turns f((x, y, z)) into f(x, y, z) when f is an unbound parameter, i.e. representing a known resource.
            if (node.Expression is ParameterExpression function && IsUnboundParameter(function) && node.Arguments.Count == 1 && IsTupleNew(node.Arguments[0]))
            {
                var args = ExpressionTupletizer.Unpack(Visit(node.Arguments[0]));

                var newFunctionType = Expression.GetDelegateType([.. args.Select(a => a.Type), node.Type]);

                var newFunction = Expression.Parameter(newFunctionType, function.Name);

                return Expression.Invoke(newFunction, args);
            }

            return base.VisitInvocation(node);
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

        /// <summary>
        /// Determines whether the specified expression is a tuple creation expression.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression creates a tuple; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTupleNew(Expression expression) => expression.NodeType == ExpressionType.New && ExpressionTupletizer.IsTuple(expression.Type);
    }
}
