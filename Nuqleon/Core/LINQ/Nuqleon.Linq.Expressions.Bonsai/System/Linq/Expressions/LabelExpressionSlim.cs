// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lighweight representation of a label, which can be put in any <see cref="ExpressionSlim" /> context.
    /// If it is jumped to, it will get the value provided by the corresponding <see cref="GotoExpressionSlim" />.
    /// Otherwise, it receives the value in <see cref="DefaultValue" />.
    /// </summary>
    public sealed class LabelExpressionSlim : ExpressionSlim
    {
        internal LabelExpressionSlim(LabelTargetSlim label, ExpressionSlim defaultValue)
        {
            Target = label;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Label;

        /// <summary>
        /// The <see cref="LabelTargetSlim"/> which this label is associated with.
        /// </summary>
        public LabelTargetSlim Target { get; }

        /// <summary>
        /// The value of the <see cref="LabelExpressionSlim"/> when the label is reached through normal control flow (e.g. is not jumped to).
        /// </summary>
        public ExpressionSlim DefaultValue { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="target">The <see cref="Target" /> property of the result.</param>
        /// <param name="defaultValue">The <see cref="DefaultValue" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public LabelExpressionSlim Update(LabelTargetSlim target, ExpressionSlim defaultValue)
        {
            if (target == Target && defaultValue == DefaultValue)
            {
                return this;
            }

            return new LabelExpressionSlim(target, defaultValue);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitLabel(this);
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
            return visitor.VisitLabel(this);
        }
    }
}
