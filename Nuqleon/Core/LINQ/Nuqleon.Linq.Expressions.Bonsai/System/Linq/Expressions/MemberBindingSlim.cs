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
    /// Lightweight representation of a member binding.
    /// </summary>
    public abstract class MemberBindingSlim
    {
        internal MemberBindingSlim(MemberInfoSlim member)
        {
            Member = member;
        }

        /// <summary>
        /// Gets the type of the member binding.
        /// </summary>
        public abstract MemberBindingType BindingType { get; }

        /// <summary>
        /// Gets the member processed by the member binding.
        /// </summary>
        public MemberInfoSlim Member { get; }

        /// <summary>
        /// Accepts the member binding node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal abstract MemberBindingSlim Accept(ExpressionSlimVisitor visitor);

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
        protected internal abstract TMemberBinding Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
            where TLambdaExpression : TExpression
            where TParameterExpression : TExpression
            where TNewExpression : TExpression
            where TMemberAssignment : TMemberBinding
            where TMemberListBinding : TMemberBinding
            where TMemberMemberBinding : TMemberBinding;

        /// <summary>
        /// Gets a friendly string representation of the node.
        /// </summary>
        /// <returns>String representation of the node.</returns>
        public override string ToString()
        {
            using var sb = ExpressionSlimPrettyPrinter.StringBuilderPool.New();

            new ExpressionSlimPrettyPrinter(sb.StringBuilder).VisitMemberBinding(this);

            return sb.StringBuilder.ToString();
        }
    }
}
