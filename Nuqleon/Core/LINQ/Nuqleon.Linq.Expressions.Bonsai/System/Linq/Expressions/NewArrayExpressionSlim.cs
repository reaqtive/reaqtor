// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of array creation expression tree nodes.
    /// </summary>
    public abstract class NewArrayExpressionSlim : ExpressionSlim
    {
        internal NewArrayExpressionSlim(TypeSlim elementType, ReadOnlyCollection<ExpressionSlim> expressions)
        {
            ElementType = elementType;
            Expressions = expressions;
        }

        /// <summary>
        /// Gets the type of the elements in the array.
        /// </summary>
        public TypeSlim ElementType { get; }

        /// <summary>
        /// Gets the expressions used by the array initializer. Depending on the node type, these expressions either represent the array bounds or the elements being initialized.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Expressions { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="expressions">The <see cref="Expressions"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public abstract NewArrayExpressionSlim Update(ReadOnlyCollection<ExpressionSlim> expressions);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree APIs.

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitNewArray(this);
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
            return visitor.VisitNewArray(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }

    internal sealed class NewArrayBoundsExpressionSlim : NewArrayExpressionSlim
    {
        public NewArrayBoundsExpressionSlim(TypeSlim elementType, ReadOnlyCollection<ExpressionSlim> expressions)
            : base(elementType, expressions)
        {
        }

        public override ExpressionType NodeType => ExpressionType.NewArrayBounds;

        public override NewArrayExpressionSlim Update(ReadOnlyCollection<ExpressionSlim> expressions)
        {
            if (expressions == Expressions)
            {
                return this;
            }

            return new NewArrayBoundsExpressionSlim(ElementType, expressions);
        }
    }

    internal sealed class NewArrayInitExpressionSlim : NewArrayExpressionSlim
    {
        public NewArrayInitExpressionSlim(TypeSlim elementType, ReadOnlyCollection<ExpressionSlim> expressions)
            : base(elementType, expressions)
        {
        }

        public override ExpressionType NodeType => ExpressionType.NewArrayInit;

        public override NewArrayExpressionSlim Update(ReadOnlyCollection<ExpressionSlim> expressions)
        {
            if (expressions == Expressions)
            {
                return this;
            }

            return new NewArrayInitExpressionSlim(ElementType, expressions);
        }
    }
}
