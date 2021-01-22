// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of an unconditional jump. This includes return statements, break and continue statements, and other jumps.
    /// </summary>
    public sealed class GotoExpressionSlim : ExpressionSlim
    {
        internal GotoExpressionSlim(GotoExpressionKind kind, LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            Kind = kind;
            Value = value;
            Target = target;
            Type = type;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Goto;

        /// <summary>
        /// Gets the value passed to the target, or null if the target is of type System.Void.
        /// </summary>
        public ExpressionSlim Value { get; }

        /// <summary>
        /// Gets the target label where this node jumps to.
        /// </summary>
        public LabelTargetSlim Target { get; }

        /// <summary>
        /// Gets the kind of the goto. For information purposes only.
        /// </summary>
        public GotoExpressionKind Kind { get; }

        /// <summary>
        /// Gets the type of the value of the goto.
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="target">The <see cref="Target" /> property of the result.</param>
        /// <param name="value">The <see cref="Value" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public GotoExpressionSlim Update(LabelTargetSlim target, ExpressionSlim value)
        {
            if (target == Target && value == Value)
            {
                return this;
            }

            return new GotoExpressionSlim(Kind, target, value, Type);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitGoto(this);
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
            return visitor.VisitGoto(this);
        }
    }
}
