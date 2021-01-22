// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of member initializer expression tree nodes.
    /// </summary>
    public sealed class MemberInitExpressionSlim : ExpressionSlim
    {
        internal MemberInitExpressionSlim(NewExpressionSlim newExpression, ReadOnlyCollection<MemberBindingSlim> bindings)
        {
            NewExpression = newExpression;
            Bindings = bindings;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.MemberInit;

        /// <summary>
        /// Gets the object creation expression used by the list initializer.
        /// </summary>
        public NewExpressionSlim NewExpression { get; }

        /// <summary>
        /// Gets the member bindings used by the member initializer.
        /// </summary>
        public ReadOnlyCollection<MemberBindingSlim> Bindings { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="newExpression">The <see cref="NewExpression"/> child node of the result.</param>
        /// <param name="bindings">The <see cref="Bindings"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public MemberInitExpressionSlim Update(NewExpressionSlim newExpression, ReadOnlyCollection<MemberBindingSlim> bindings)
        {
            if (newExpression == NewExpression && bindings == Bindings)
            {
                return this;
            }

            return new MemberInitExpressionSlim(newExpression, bindings);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitMemberInit(this);
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
            return visitor.VisitMemberInit(this);
        }
    }
}
