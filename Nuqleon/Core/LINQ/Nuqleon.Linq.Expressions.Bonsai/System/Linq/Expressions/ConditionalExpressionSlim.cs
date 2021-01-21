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
    // PERF: In case IfThen[Else] nodes become popular, we can introduce specialized nodes for void-returning
    //       conditionals and for those with a missing IfFalse branch.

    /// <summary>
    /// Lightweight representation of conditional expression tree nodes.
    /// </summary>
    public sealed class ConditionalExpressionSlim : ExpressionSlim
    {
        internal ConditionalExpressionSlim(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse, TypeSlim type)
        {
            Test = test;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
            Type = type;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Conditional;

        /// <summary>
        /// Gets the test expression used by the conditional expression.
        /// </summary>
        public ExpressionSlim Test { get; }

        /// <summary>
        /// Gets the expression representing the result of the conditional expression in case it evaluates to true.
        /// </summary>
        public ExpressionSlim IfTrue { get; }

        /// <summary>
        /// Gets the expression representing the result of the conditional expression in case it evaluates to false.
        /// </summary>
        public ExpressionSlim IfFalse { get; }

        /// <summary>
        /// Gets the type of the result of the conditional expression.
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="test">The <see cref="Test"/> child node of the result.</param>
        /// <param name="ifTrue">The <see cref="IfTrue"/> child node of the result.</param>
        /// <param name="ifFalse">The <see cref="IfFalse"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public ConditionalExpressionSlim Update(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            if (test == Test && ifTrue == IfTrue && ifFalse == IfFalse)
            {
                return this;
            }

            return new ConditionalExpressionSlim(test, ifTrue, ifFalse, Type);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitConditional(this);
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
            return visitor.VisitConditional(this);
        }
    }
}
