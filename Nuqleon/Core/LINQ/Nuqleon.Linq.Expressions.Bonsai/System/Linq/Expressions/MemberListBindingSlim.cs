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
    /// Lightweight representation of a member list binding.
    /// </summary>
    public sealed class MemberListBindingSlim : MemberBindingSlim
    {
        internal MemberListBindingSlim(MemberInfoSlim member, ReadOnlyCollection<ElementInitSlim> initializers)
            : base(member)
        {
            Initializers = initializers;
        }

        /// <summary>
        /// Gets the type of the member binding.
        /// </summary>
        public override MemberBindingType BindingType => MemberBindingType.ListBinding;

        /// <summary>
        /// Gets the element initializers processed by the member binding.
        /// </summary>
        public ReadOnlyCollection<ElementInitSlim> Initializers { get; }

        /// <summary>
        /// Creates a new member binding that is like this one, but using the supplied children. If all of the children are the same, it will return this member binding.
        /// </summary>
        /// <param name="initializers">The <see cref="Initializers"/> child node of the result.</param>
        /// <returns>This member binding if no children are changed or a member binding with the updated children.</returns>
        public MemberListBindingSlim Update(ReadOnlyCollection<ElementInitSlim> initializers)
        {
            if (initializers == Initializers)
            {
                return this;
            }

            return new MemberListBindingSlim(Member, initializers);
        }

        /// <summary>
        /// Accepts the member binding node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override MemberBindingSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitMemberListBinding(this);
        }

        /// <summary>
        /// Accepts the member binding node in the specified visitor.
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
        protected internal override TMemberBinding Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
        {
            return visitor.VisitMemberListBinding(this);
        }
    }
}
