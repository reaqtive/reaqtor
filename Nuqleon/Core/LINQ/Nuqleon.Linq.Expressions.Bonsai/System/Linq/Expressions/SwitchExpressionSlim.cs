// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    // PERF: Consider introducing a specialization that omits the Comparison field.

    /// <summary>
    /// Lightweight representation of a control expression that handles multiple selections by passing control to <see cref="SwitchCaseSlim" />.
    /// </summary>
    public sealed class SwitchExpressionSlim : ExpressionSlim
    {
        internal SwitchExpressionSlim(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, ReadOnlyCollection<SwitchCaseSlim> cases)
        {
            Type = type;
            SwitchValue = switchValue;
            DefaultBody = defaultBody;
            Comparison = comparison;
            Cases = cases;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Switch;

        /// <summary>
        /// Gets the static type of the expression that this <see cref="ExpressionSlim" /> represents.
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Gets the test for the switch.
        /// </summary>
        public ExpressionSlim SwitchValue { get; }

        /// <summary>
        /// Gets the collection of <see cref="SwitchCaseSlim"/> objects for the switch.
        /// </summary>
        public ReadOnlyCollection<SwitchCaseSlim> Cases { get; }

        /// <summary>
        /// Gets the test for the switch.
        /// </summary>
        public ExpressionSlim DefaultBody { get; }

        /// <summary>
        /// Gets the equality comparison method, if any.
        /// </summary>
        public MethodInfoSlim Comparison { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="switchValue">The <see cref="SwitchValue" /> property of the result.</param>
        /// <param name="cases">The <see cref="Cases" /> property of the result.</param>
        /// <param name="defaultBody">The <see cref="DefaultBody" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public SwitchExpressionSlim Update(ExpressionSlim switchValue, ReadOnlyCollection<SwitchCaseSlim> cases, ExpressionSlim defaultBody)
        {
            if (switchValue == SwitchValue && cases == Cases && defaultBody == DefaultBody)
            {
                return this;
            }

            return new SwitchExpressionSlim(Type, switchValue, defaultBody, Comparison, cases);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitSwitch(this);
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
            return visitor.VisitSwitch(this);
        }
    }
}
