// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of type binary expression tree nodes.
    /// </summary>
    public abstract class TypeBinaryExpressionSlim : ExpressionSlim
    {
        internal TypeBinaryExpressionSlim(ExpressionSlim expression, TypeSlim typeOperand)
        {
            Expression = expression;
            TypeOperand = typeOperand;
        }

        /// <summary>
        /// Gets the expression to apply the type operation on.
        /// </summary>
        public ExpressionSlim Expression { get; }

        /// <summary>
        /// Gets the type operand used by the type operation.
        /// </summary>
        public TypeSlim TypeOperand { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public TypeBinaryExpressionSlim Update(ExpressionSlim expression)
        {
            if (expression == Expression)
            {
                return this;
            }

            return Make(NodeType, expression, TypeOperand);
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
            return visitor.VisitTypeBinary(this);
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
            return visitor.VisitTypeBinary(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        internal static TypeBinaryExpressionSlim Make(ExpressionType nodeType, ExpressionSlim expression, TypeSlim typeOperand)
        {
            if (nodeType == ExpressionType.TypeEqual)
            {
                return new TypeEqualExpressionSlim(expression, typeOperand);
            }
            else
            {
                Debug.Assert(nodeType == ExpressionType.TypeIs);
                return new TypeIsExpressionSlim(expression, typeOperand);
            }
        }
    }

    internal sealed class TypeEqualExpressionSlim : TypeBinaryExpressionSlim
    {
        public TypeEqualExpressionSlim(ExpressionSlim expression, TypeSlim typeOperand)
            : base(expression, typeOperand)
        {
        }

        public override ExpressionType NodeType => ExpressionType.TypeEqual;
    }

    internal sealed class TypeIsExpressionSlim : TypeBinaryExpressionSlim
    {
        public TypeIsExpressionSlim(ExpressionSlim expression, TypeSlim typeOperand)
            : base(expression, typeOperand)
        {
        }

        public override ExpressionType NodeType => ExpressionType.TypeIs;
    }
}
