// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of unary expression tree nodes.
    /// </summary>
    public abstract class UnaryExpressionSlim : ExpressionSlim
    {
        internal UnaryExpressionSlim(ExpressionType nodeType, ExpressionSlim operand)
        {
            NodeType = nodeType;
            Operand = operand;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType { get; }

        /// <summary>
        /// Gets the expression representing the operand of the unary operation.
        /// </summary>
        public ExpressionSlim Operand { get; }

        /// <summary>
        /// Gets the method implementing the unary operation.
        /// </summary>
        public virtual MethodInfoSlim Method => null;

        /// <summary>
        /// Gets the type used for conversion operations.
        /// </summary>
        public virtual TypeSlim Type => s_typeMissing;

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="operand">The <see cref="Operand"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public UnaryExpressionSlim Update(ExpressionSlim operand)
        {
            if (operand == Operand)
            {
                return this;
            }

            return UnaryExpressionSlim.Make(NodeType, operand, Method, Type);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree APIs.

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitUnary(this);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <typeparam name="TExpression">Target type for expressions.</typeparam>
        /// <typeparam name="TLambdaExpression">Target type for lambda expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TParameterExpression">Target type for parameter expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TNewExpression">Target type for new expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
        /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
        /// <typeparam name="TMemberAssignment">Target type for member assignments. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberListBinding">Target type for member list bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberMemberBinding">Target type for member member bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
        /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
        /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override TExpression Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
        {
            return visitor.VisitUnary(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        internal static UnaryExpressionSlim Make(ExpressionType nodeType, ExpressionSlim operand, MethodInfoSlim method, TypeSlim type)
        {
            if (type == s_typeMissing)
            {
                if (method == null)
                {
                    return new UntypedUnaryExpressionSlim(nodeType, operand);
                }
                else
                {
                    return new MethodBasedUntypedUnaryExpressionSlim(nodeType, operand, method);
                }
            }
            else
            {
                if (method == null)
                {
                    return new TypedUnaryExpressionSlim(nodeType, operand, type);
                }
                else
                {
                    return new MethodBasedTypedUnaryExpressionSlim(nodeType, operand, type, method);
                }
            }
        }
    }

    internal class UntypedUnaryExpressionSlim : UnaryExpressionSlim
    {
        internal UntypedUnaryExpressionSlim(ExpressionType nodeType, ExpressionSlim operand)
            : base(nodeType, operand)
        {
        }
    }

    internal sealed class MethodBasedUntypedUnaryExpressionSlim : UntypedUnaryExpressionSlim
    {
        internal MethodBasedUntypedUnaryExpressionSlim(ExpressionType nodeType, ExpressionSlim operand, MethodInfoSlim method)
            : base(nodeType, operand)
        {
            Method = method;
        }

        public override MethodInfoSlim Method { get; }
    }

    internal class TypedUnaryExpressionSlim : UnaryExpressionSlim
    {
        internal TypedUnaryExpressionSlim(ExpressionType nodeType, ExpressionSlim operand, TypeSlim type)
            : base(nodeType, operand)
        {
            Type = type;
        }

        public override TypeSlim Type { get; }
    }

    internal sealed class MethodBasedTypedUnaryExpressionSlim : TypedUnaryExpressionSlim
    {
        internal MethodBasedTypedUnaryExpressionSlim(ExpressionType nodeType, ExpressionSlim operand, TypeSlim type, MethodInfoSlim method)
            : base(nodeType, operand, type)
        {
            Method = method;
        }

        public override MethodInfoSlim Method { get; }
    }
}
