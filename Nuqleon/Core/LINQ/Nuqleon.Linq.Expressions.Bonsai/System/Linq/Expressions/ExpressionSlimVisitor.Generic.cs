// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Visitor for lightweight expression trees.
    /// </summary>
    /// <typeparam name="TExpression">Result type for ExpressionSlim nodes.</typeparam>
    /// <typeparam name="TMemberBinding">Result type for member binding nodes.</typeparam>
    /// <typeparam name="TElementInit">Result type for element initializer nodes.</typeparam>
    public abstract class ExpressionSlimVisitor<TExpression, TMemberBinding, TElementInit>
        : ExpressionSlimVisitor<TExpression, TExpression, TExpression, TExpression, TElementInit, TMemberBinding, TMemberBinding, TMemberBinding, TMemberBinding>
    {
    }

    /// <summary>
    /// Visitor for lightweight expression trees supporting statement nodes.
    /// </summary>
    /// <typeparam name="TExpression">Target type for expressions.</typeparam>
    /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
    /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
    /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
    /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
    /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
    public abstract class ExpressionSlimVisitor<TExpression, TMemberBinding, TElementInit, TCatchBlock, TSwitchCase, TLabelTarget>
        : ExpressionSlimVisitor<TExpression, TExpression, TExpression, TExpression, TElementInit, TMemberBinding, TMemberBinding, TMemberBinding, TMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
    {
    }

    /// <summary>
    /// Visitor for lightweight expression trees.
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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding>
        : ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, object, object, object>
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
        /// <summary>
        /// Makes an expression representing a BlockExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="variables">Variable expressions.</param>
        /// <param name="expressions">Body expressions.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override TExpression MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<TParameterExpression> variables, ReadOnlyCollection<TExpression> expressions)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes a catch block object representing a CatchBlock with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="variable">Variable expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="filter">Filter expression.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override object MakeCatchBlock(CatchBlockSlim node, TParameterExpression variable, TExpression body, TExpression filter)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes an expression representing a GotoExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="target">Target label.</param>
        /// <param name="value">Value expression.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override TExpression MakeGoto(GotoExpressionSlim node, object target, TExpression value)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Visits the children of the LabelExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <param name="target">Target label.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override TExpression MakeLabel(LabelExpressionSlim node, object target, TExpression defaultValue)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes a label target object representing a LabelTarget with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override object MakeLabelTarget(LabelTargetSlim node)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes an expression representing a LoopExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="breakLabel">Break label.</param>
        /// <param name="continueLabel">Continue label.</param>
        /// <returns>ALways throws exception.</returns>
        protected sealed override TExpression MakeLoop(LoopExpressionSlim node, TExpression body, object breakLabel, object continueLabel)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes an expression representing a SwitchExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="switchValue">Switch value expression.</param>
        /// <param name="cases">Switch cases.</param>
        /// <param name="defaultBody">Default body expressions.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override TExpression MakeSwitch(SwitchExpressionSlim node, TExpression switchValue, TExpression defaultBody, ReadOnlyCollection<object> cases)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes a switch case object representing a SwitchCase with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="testValues">Test value expressions.</param>
        /// <param name="body">Body expression.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override object MakeSwitchCase(SwitchCaseSlim node, TExpression body, ReadOnlyCollection<TExpression> testValues)
        {
            throw StatementNodesNotSupported();
        }

        /// <summary>
        /// Makes an expression representing a TryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="handlers">Handler expressions.</param>
        /// <param name="finally">Finally expression.</param>
        /// <param name="fault">Fault expression.</param>
        /// <returns>Always throws exception.</returns>
        protected sealed override TExpression MakeTry(TryExpressionSlim node, TExpression body, TExpression @finally, TExpression fault, ReadOnlyCollection<object> handlers)
        {
            throw StatementNodesNotSupported();
        }

        private static Exception StatementNodesNotSupported()
        {
            return new InvalidOperationException("This visitor does not support statement nodes.");
        }
    }
}
