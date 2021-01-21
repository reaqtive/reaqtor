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
    /// Lightweight representation of an infinite loop. It can be exited with "break".
    /// </summary>
    public sealed class LoopExpressionSlim : ExpressionSlim
    {
        internal LoopExpressionSlim(ExpressionSlim body, LabelTargetSlim @break, LabelTargetSlim @continue)
        {
            Body = body;
            BreakLabel = @break;
            ContinueLabel = @continue;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Loop;

        /// <summary>
        /// Gets the <see cref="ExpressionSlim"/> that is the body of the loop.
        /// </summary>
        public ExpressionSlim Body { get; }

        /// <summary>
        /// Gets the <see cref="LabelTargetSlim"/> that is used by the loop body as a break statement target.
        /// </summary>
        public LabelTargetSlim BreakLabel { get; }

        /// <summary>
        /// Gets the <see cref="LabelTargetSlim"/> that is used by the loop body as a continue statement target.
        /// </summary>
        public LabelTargetSlim ContinueLabel { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="breakLabel">The <see cref="BreakLabel" /> property of the result.</param>
        /// <param name="continueLabel">The <see cref="ContinueLabel" /> property of the result.</param>
        /// <param name="body">The <see cref="Body" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public LoopExpressionSlim Update(LabelTargetSlim breakLabel, LabelTargetSlim continueLabel, ExpressionSlim body)
        {
            if (breakLabel == BreakLabel && continueLabel == ContinueLabel && body == Body)
            {
                return this;
            }

            return new LoopExpressionSlim(body, breakLabel, continueLabel);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitLoop(this);
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
            return visitor.VisitLoop(this);
        }
    }
}
