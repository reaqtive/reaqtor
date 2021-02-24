// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // No null checks in visitor methods.

using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Expression visitor with restrictions to "pure" expression nodes (i.e. as supported in .NET 3.5).
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
    public abstract class ExpressionVisitorNarrow<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding> : ExpressionVisitorNarrow<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, object, object, object>
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="variables">Irrelevant.</param>
        /// <param name="expressions">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeBlock(BlockExpression node, ReadOnlyCollection<TParameterExpression> variables, ReadOnlyCollection<TExpression> expressions) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="variable">Irrelevant.</param>
        /// <param name="body">Irrelevant.</param>
        /// <param name="filter">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override object MakeCatchBlock(CatchBlock node, TParameterExpression variable, TExpression body, TExpression filter) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="target">Irrelevant.</param>
        /// <param name="value">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeGoto(GotoExpression node, object target, TExpression value) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="target">Irrelevant.</param>
        /// <param name="defaultValue">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeLabel(LabelExpression node, object target, TExpression defaultValue) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override object MakeLabelTarget(LabelTarget node) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="body">Irrelevant.</param>
        /// <param name="breakLabel">Irrelevant.</param>
        /// <param name="continueLabel">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeLoop(LoopExpression node, TExpression body, object breakLabel, object continueLabel) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="switchValue">Irrelevant.</param>
        /// <param name="defaultBody">Irrelevant.</param>
        /// <param name="cases">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeSwitch(SwitchExpression node, TExpression switchValue, TExpression defaultBody, ReadOnlyCollection<object> cases) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="body">Irrelevant.</param>
        /// <param name="testValues">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override object MakeSwitchCase(SwitchCase node, TExpression body, ReadOnlyCollection<TExpression> testValues) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="body">Irrelevant.</param>
        /// <param name="finally">Irrelevant.</param>
        /// <param name="fault">Irrelevant.</param>
        /// <param name="handlers">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeTry(TryExpression node, TExpression body, TExpression @finally, TExpression fault, ReadOnlyCollection<object> handlers) => throw Errors.NotSupportedByNarrowVisitor(node);
    }

    /// <summary>
    /// Expression visitor with restrictions to "pure" expression nodes (i.e. as supported in .NET 3.5).
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
    public abstract class ExpressionVisitorNarrow<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeDebugInfo(DebugInfoExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="arguments">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeDynamic(DynamicExpression node, ReadOnlyCollection<TExpression> arguments) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="object">Irrelevant.</param>
        /// <param name="arguments">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeIndex(IndexExpression node, TExpression @object, ReadOnlyCollection<TExpression> arguments) => throw Errors.NotSupportedByNarrowVisitor(node);

        /// <summary>
        /// Not supported by this visitor.
        /// </summary>
        /// <param name="node">Irrelevant.</param>
        /// <param name="variables">Irrelevant.</param>
        /// <returns>Always throws an exception.</returns>
        /// <exception cref="NotSupportedException">This node type is not supported by this visitor.</exception>
        protected override TExpression MakeRuntimeVariables(RuntimeVariablesExpression node, ReadOnlyCollection<TParameterExpression> variables) => throw Errors.NotSupportedByNarrowVisitor(node);
    }

    internal abstract class ExpressionVisitorNarrow<TExpression> : ExpressionVisitor<TExpression>
    {
        protected override TExpression VisitBlock(BlockExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitDebugInfo(DebugInfoExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitDynamic(DynamicExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitExtension(Expression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitGoto(GotoExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitIndex(IndexExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitLabel(LabelExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitLoop(LoopExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitRuntimeVariables(RuntimeVariablesExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitSwitch(SwitchExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);

        protected override TExpression VisitTry(TryExpression node) => throw Errors.NotSupportedByNarrowVisitor(node);
    }
}
