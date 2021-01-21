// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitted null checks for protected overrides.)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Visitor to rewrite the types of expression tree nodes while also unquoting unary expressions representing quotes.
    /// </summary>
    public class SubstituteAndUnquoteRewriter : TypeSubstitutionExpressionVisitor
    {
        /// <summary>
        /// Creates a new type substitution and unquoting visitor using the specified type map.
        /// </summary>
        /// <param name="map">Dictionary mapping original types to their rewrite targets.</param>
        public SubstituteAndUnquoteRewriter(IDictionary<Type, Type> map)
            : base(new Impl(map))
        {
        }

        /// <summary>
        /// Visit invocation expressions to strip out invocations of the identity function.
        /// </summary>
        /// <param name="node">Expression to analyze and rewrite.</param>
        /// <returns>The original node if the invocation does not apply an identity function; otherwise, the operand of the identity function application.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            var visited = base.VisitInvocation(node);

            if (visited is InvocationExpression invoke && invoke.Expression.NodeType == ExpressionType.Parameter)
            {
                var target = (ParameterExpression)invoke.Expression;

                if (target.Name == Constants.IdentityFunctionUri)
                {
                    if (HasIdentitySignature(target.Type))
                    {
                        return invoke.Arguments[0];
                    }

                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Received invocation target type '{0}' in expression '{1}'; cannot call the identity function when argument type does not equal result type.",
                            target.Type.ToCSharpStringPretty(),
                            node.ToCSharpString(true)));
                }
            }

            return visited;
        }

        /// <summary>
        /// Visits unary expression nodes in order to unquote quote nodes.
        /// </summary>
        /// <param name="node">Expression to analyze and rewrite.</param>
        /// <returns>The original node if it doesn't represent a quote; otherwise, the unquoted node.</returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Quote)
            {
                return Visit(node.Operand);
            }
            else
            {
                return base.VisitUnary(node);
            }
        }

        private static bool HasIdentitySignature(Type type)
        {
            var funcType = type.FindGenericType(typeof(Func<,>));

            if (funcType != null)
            {
                var genericArguments = funcType.GetGenericArguments();
                var inputType = genericArguments[0];
                var outputType = genericArguments[1];
                return inputType == outputType;
            }

            return false;
        }

        private sealed class Impl : TypeSubstitutor
        {
            public Impl(IDictionary<Type, Type> map)
                : base(map)
            {
            }

            protected override Type VisitGenericClosed(Type type)
            {
                if (type.GetGenericTypeDefinition() == typeof(Expression<>))
                {
                    return Visit(type.GetGenericArguments()[0]);
                }
                else
                {
                    return base.VisitGenericClosed(type);
                }
            }
        }
    }
}
