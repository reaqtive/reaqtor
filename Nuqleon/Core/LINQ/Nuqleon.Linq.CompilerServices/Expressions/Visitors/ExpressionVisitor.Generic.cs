// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.ObjectModel;
#if USE_SLIM
using System.Linq.CompilerServices;
#else
using System.Linq.Expressions;
#endif

#if USE_SLIM
namespace System.Linq.Expressions
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using BinaryExpressionAlias = BinaryExpressionSlim;
    using BlockExpressionAlias = BlockExpressionSlim;
    using CatchBlockAlias = CatchBlockSlim;
    using ConditionalExpressionAlias = ConditionalExpressionSlim;
    using ConstantExpressionAlias = ConstantExpressionSlim;
    using DefaultExpressionAlias = DefaultExpressionSlim;
    using ExpressionAlias = ExpressionSlim;
    using GotoExpressionAlias = GotoExpressionSlim;
    using IndexExpressionAlias = IndexExpressionSlim;
    using InvocationExpressionAlias = InvocationExpressionSlim;
    using LabelTargetAlias = LabelTargetSlim;
    using LabelExpressionAlias = LabelExpressionSlim;
    using LambdaExpressionAlias = LambdaExpressionSlim;
    using ListInitExpressionAlias = ListInitExpressionSlim;
    using LoopExpressionAlias = LoopExpressionSlim;
    using MemberExpressionAlias = MemberExpressionSlim;
    using MemberInitExpressionAlias = MemberInitExpressionSlim;
    using MethodCallExpressionAlias = MethodCallExpressionSlim;
    using NewArrayExpressionAlias = NewArrayExpressionSlim;
    using NewExpressionAlias = NewExpressionSlim;
    using ParameterExpressionAlias = ParameterExpressionSlim;
    using SwitchCaseAlias = SwitchCaseSlim;
    using SwitchExpressionAlias = SwitchExpressionSlim;
    using TryExpressionAlias = TryExpressionSlim;
    using TypeBinaryExpressionAlias = TypeBinaryExpressionSlim;
    using UnaryExpressionAlias = UnaryExpressionSlim;

    using ElementInitAlias = ElementInitSlim;
    using MemberAssignmentAlias = MemberAssignmentSlim;
    using MemberBindingAlias = MemberBindingSlim;
    using MemberListBindingAlias = MemberListBindingSlim;
    using MemberMemberBindingAlias = MemberMemberBindingSlim;

    #endregion
#else
    #region Aliases

    using BinaryExpressionAlias = BinaryExpression;
    using BlockExpressionAlias = BlockExpression;
    using CatchBlockAlias = CatchBlock;
    using ConditionalExpressionAlias = ConditionalExpression;
    using ConstantExpressionAlias = ConstantExpression;
    using DefaultExpressionAlias = DefaultExpression;
    using ExpressionAlias = Expression;
    using GotoExpressionAlias = GotoExpression;
    using IndexExpressionAlias = IndexExpression;
    using InvocationExpressionAlias = InvocationExpression;
    using LabelTargetAlias = LabelTarget;
    using LabelExpressionAlias = LabelExpression;
    using ListInitExpressionAlias = ListInitExpression;
    using LoopExpressionAlias = LoopExpression;
    using MemberExpressionAlias = MemberExpression;
    using MemberInitExpressionAlias = MemberInitExpression;
    using MethodCallExpressionAlias = MethodCallExpression;
    using NewArrayExpressionAlias = NewArrayExpression;
    using NewExpressionAlias = NewExpression;
    using ParameterExpressionAlias = ParameterExpression;
    using SwitchCaseAlias = SwitchCase;
    using SwitchExpressionAlias = SwitchExpression;
    using TryExpressionAlias = TryExpression;
    using TypeBinaryExpressionAlias = TypeBinaryExpression;
    using UnaryExpressionAlias = UnaryExpression;

    using ElementInitAlias = ElementInit;
    using MemberAssignmentAlias = MemberAssignment;
    using MemberBindingAlias = MemberBinding;
    using MemberListBindingAlias = MemberListBinding;
    using MemberMemberBindingAlias = MemberMemberBinding;

    #endregion
#endif

    /// <summary>
    /// Expression visitor to rewrite an expression tree into a target type.
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
#if USE_SLIM
    public abstract class ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionSlimVisitor<TExpression>
#else
    public abstract class ExpressionVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionVisitor<TExpression>
#endif
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
#if USE_SLIM
        /// <summary>
        /// Visits the specified expression and rewrites it to the target expression type.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        public override TExpression Visit(ExpressionAlias node)
        {
            if (node != null)
            {
                return node.Accept(this);
            }

            return base.Visit(node);
        }
#endif

        /// <summary>
        /// Visits the children of the BinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitBinary(BinaryExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var l = Visit(node.Left);
            var c = VisitAndConvert<TLambdaExpression>(node.Conversion);
            var r = Visit(node.Right);
            return MakeBinary(node, l, c, r);
        }

        /// <summary>
        /// Makes an expression representing a BinaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="left">Left expression.</param>
        /// <param name="conversion">Conversion expression.</param>
        /// <param name="right">Right expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeBinary(BinaryExpressionAlias node, TExpression left, TLambdaExpression conversion, TExpression right);

        /// <summary>
        /// Visits the children of the BlockExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitBlock(BlockExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var v = VisitAndConvert<ParameterExpressionAlias, TParameterExpression>(node.Variables);
            var e = Visit(node.Expressions);
            return MakeBlock(node, v, e);
        }

        /// <summary>
        /// Makes an expression representing a BlockExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="variables">Variables in the block.</param>
        /// <param name="expressions">Expressions in the block.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeBlock(BlockExpressionAlias node, ReadOnlyCollection<TParameterExpression> variables, ReadOnlyCollection<TExpression> expressions);

        /// <summary>
        /// Visits the children of the CatchBlock.
        /// </summary>
        /// <param name="node">Catch block to visit.</param>
        /// <returns>Result of visiting the catch block.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TCatchBlock VisitCatchBlock(CatchBlockAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var v = VisitAndConvert<TParameterExpression>(node.Variable);
            var b = Visit(node.Body);
            var f = Visit(node.Filter);
            return MakeCatchBlock(node, v, b, f);
        }

        /// <summary>
        /// Makes a catch block object representing a CatchBlock with the given children.
        /// </summary>
        /// <param name="node">Original catch block.</param>
        /// <param name="variable">Variable expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="filter">Filter expression.</param>
        /// <returns>Representation of the original catch block.</returns>
        protected abstract TCatchBlock MakeCatchBlock(CatchBlockAlias node, TParameterExpression variable, TExpression body, TExpression filter);

        /// <summary>
        /// Visits the children of the ConditionalExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitConditional(ConditionalExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var t = Visit(node.Test);
            var p = Visit(node.IfTrue);
            var n = Visit(node.IfFalse);
            return MakeConditional(node, t, p, n);
        }

        /// <summary>
        /// Makes an expression representing a ConditionalExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="test">Test expression.</param>
        /// <param name="ifTrue">True branch expression.</param>
        /// <param name="ifFalse">False branch expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeConditional(ConditionalExpressionAlias node, TExpression test, TExpression ifTrue, TExpression ifFalse);

        /// <summary>
        /// Visits the children of the ConstantExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitConstant(ConstantExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return MakeConstant(node);
        }

        /// <summary>
        /// Makes an expression representing a ConstantExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeConstant(ConstantExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the DebugInfoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitDebugInfo(DebugInfoExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return MakeDebugInfo(node);
        }

        /// <summary>
        /// Makes an expression representing a DebugInfoExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeDebugInfo(DebugInfoExpression node);
#endif

        /// <summary>
        /// Visits the children of the DefaultExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitDefault(DefaultExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return MakeDefault(node);
        }

        /// <summary>
        /// Makes an expression representing a DefaultExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeDefault(DefaultExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the DynamicExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitDynamic(DynamicExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var a = Visit(node.Arguments);
            return MakeDynamic(node, a);
        }

        /// <summary>
        /// Makes an expression representing a DynamicExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeDynamic(DynamicExpression node, ReadOnlyCollection<TExpression> arguments);
#endif

        /// <summary>
        /// Visits the children of the ElementInit.
        /// </summary>
        /// <param name="node">Element initializer to visit.</param>
        /// <returns>Result of visiting the element initializer.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TElementInit VisitElementInit(ElementInitAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var a = Visit(node.Arguments);
            return MakeElementInit(node, a);
        }

        /// <summary>
        /// Makes an element initializer object representing a ElementInit object with the given children.
        /// </summary>
        /// <param name="node">Original element initializer.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original element initializer.</returns>
        protected abstract TElementInit MakeElementInit(ElementInitAlias node, ReadOnlyCollection<TExpression> arguments);

        /// <summary>
        /// Visits the children of the extension expression node.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitExtension(ExpressionAlias node)
        {
            throw new NotImplementedException("Left for implementation by subclasses.");
        }

        /// <summary>
        /// Visits the children of the GotoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitGoto(GotoExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var t = VisitIfNotNull(node.Target, VisitLabelTarget);
            var v = Visit(node.Value);
            return MakeGoto(node, t, v);
        }

        /// <summary>
        /// Makes an expression representing a GotoExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="target">Target label.</param>
        /// <param name="value">Value expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeGoto(GotoExpressionAlias node, TLabelTarget target, TExpression value);

        /// <summary>
        /// Visits the children of the IndexExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitIndex(IndexExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var o = Visit(node.Object);
            var a = Visit(node.Arguments);
            return MakeIndex(node, o, a);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)
#pragma warning disable CA1716 // Reserved language keyword 'object'. (Mirroring expression tree APIs.)

        /// <summary>
        /// Makes an expression representing a IndexExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="object">Object expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeIndex(IndexExpressionAlias node, TExpression @object, ReadOnlyCollection<TExpression> arguments);

#pragma warning restore CA1716
#pragma warning restore CA1720
#pragma warning restore IDE0079

        /// <summary>
        /// Visits the children of the InvocationExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitInvocation(InvocationExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var e = Visit(node.Expression);
#if USE_SLIM
            var a = VisitArguments(node);
#else
            var a = Visit(node.Arguments);
#endif
            return MakeInvocation(node, e, a);
        }

        /// <summary>
        /// Makes an expression representing a InvocationExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Function expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeInvocation(InvocationExpressionAlias node, TExpression expression, ReadOnlyCollection<TExpression> arguments);

        /// <summary>
        /// Visits the children of the LabelExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitLabel(LabelExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var t = VisitIfNotNull(node.Target, VisitLabelTarget);
            var d = Visit(node.DefaultValue);
            return MakeLabel(node, t, d);
        }

        /// <summary>
        /// Makes an expression representing a LabelExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="target">Target label.</param>
        /// <param name="defaultValue">Default value expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeLabel(LabelExpressionAlias node, TLabelTarget target, TExpression defaultValue);

        /// <summary>
        /// Visits the children of the LabelTarget.
        /// </summary>
        /// <param name="node">Label target to visit.</param>
        /// <returns>Result of visiting the label target.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TLabelTarget VisitLabelTarget(LabelTargetAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return MakeLabelTarget(node);
        }

        /// <summary>
        /// Makes a label target object representing a LabelTarget.
        /// </summary>
        /// <param name="node">Original label target.</param>
        /// <returns>Representation of the label target.</returns>
        protected abstract TLabelTarget MakeLabelTarget(LabelTargetAlias node);

#if USE_SLIM
        /// <summary>
        /// Visits the children of the LambdaExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected internal override TExpression VisitLambda(LambdaExpressionAlias node)
#else
        /// <summary>
        /// Visits the children of the LambdaExpression.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitLambda<T>(Expression<T> node)
#endif
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Body);
            var p = VisitAndConvert<ParameterExpressionAlias, TParameterExpression>(node.Parameters);
#if USE_SLIM
            return MakeLambda(node, b, p);
#else
            return MakeLambda(node, b, p);
#endif
        }

#if USE_SLIM
        /// <summary>
        /// Makes an expression representing a LambdaExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="parameters">Parameter expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TLambdaExpression MakeLambda(LambdaExpressionAlias node, TExpression body, ReadOnlyCollection<TParameterExpression> parameters);
#else
        /// <summary>
        /// Makes an expression representing a LambdaExpression with the given children.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="parameters">Parameter expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TLambdaExpression MakeLambda<T>(Expression<T> node, TExpression body, ReadOnlyCollection<TParameterExpression> parameters);
#endif

        /// <summary>
        /// Visits the children of the ListInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitListInit(ListInitExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var n = VisitAndConvert<TNewExpression>(node.NewExpression);
            var e = Visit(node.Initializers, VisitElementInit);
            return MakeListInit(node, n, e);
        }

        /// <summary>
        /// Makes an expression representing a ListInitExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="newExpression">New expression.</param>
        /// <param name="initializers">Element initializers.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeListInit(ListInitExpressionAlias node, TNewExpression newExpression, ReadOnlyCollection<TElementInit> initializers);

        /// <summary>
        /// Visits the children of the LoopExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitLoop(LoopExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Body);
            var k = VisitIfNotNull(node.BreakLabel, VisitLabelTarget);
            var c = VisitIfNotNull(node.ContinueLabel, VisitLabelTarget);
            return MakeLoop(node, b, k, c);
        }

        /// <summary>
        /// Makes an expression representing a LoopExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="breakLabel">Break label.</param>
        /// <param name="continueLabel">Continue label.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeLoop(LoopExpressionAlias node, TExpression body, TLabelTarget breakLabel, TLabelTarget continueLabel);

        /// <summary>
        /// Visits the children of the MemberExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitMember(MemberExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var e = Visit(node.Expression);
            return MakeMember(node, e);
        }

        /// <summary>
        /// Makes an expression representing a MemberExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Object expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeMember(MemberExpressionAlias node, TExpression expression);

        /// <summary>
        /// Visits the children of the MemberAssignment.
        /// </summary>
        /// <param name="node">Member assignment to visit.</param>
        /// <returns>Result of visiting the member assignment.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TMemberAssignment VisitMemberAssignment(MemberAssignmentAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var e = Visit(node.Expression);
            return MakeMemberAssignment(node, e);
        }

        /// <summary>
        /// Makes a member binding object representing a MemberAssignment with the given children.
        /// </summary>
        /// <param name="node">Original member assignment.</param>
        /// <param name="expression">Assigned expression.</param>
        /// <returns>Representation of the original member assignment.</returns>
        protected abstract TMemberAssignment MakeMemberAssignment(MemberAssignmentAlias node, TExpression expression);

        /// <summary>
        /// Visits the children of the MemberBinding.
        /// </summary>
        /// <param name="node">Member binding to visit.</param>
        /// <returns>Result of visiting the member binding.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TMemberBinding VisitMemberBinding(MemberBindingAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.BindingType switch
            {
                MemberBindingType.Assignment => VisitMemberAssignment((MemberAssignmentAlias)node),
                MemberBindingType.ListBinding => VisitMemberListBinding((MemberListBindingAlias)node),
                MemberBindingType.MemberBinding => VisitMemberMemberBinding((MemberMemberBindingAlias)node),
                _ => throw new NotSupportedException(),
            };
        }

        /// <summary>
        /// Visits the children of the MemberInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitMemberInit(MemberInitExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var n = VisitAndConvert<TNewExpression>(node.NewExpression);
            var b = Visit(node.Bindings, VisitMemberBinding);
            return MakeMemberInit(node, n, b);
        }

        /// <summary>
        /// Makes an expression representing a MemberInitExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="newExpression">New expression.</param>
        /// <param name="bindings">Member bindings.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeMemberInit(MemberInitExpressionAlias node, TNewExpression newExpression, ReadOnlyCollection<TMemberBinding> bindings);

        /// <summary>
        /// Visits the children of the MemberListBinding.
        /// </summary>
        /// <param name="node">Member list binding to visit.</param>
        /// <returns>Result of visiting the member list binding.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TMemberListBinding VisitMemberListBinding(MemberListBindingAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var i = Visit(node.Initializers, VisitElementInit);
            return MakeMemberListBinding(node, i);
        }

        /// <summary>
        /// Makes a member binding object representing a MemberListBinding with the given children.
        /// </summary>
        /// <param name="node">Original member list binding.</param>
        /// <param name="initializers">Element initializers.</param>
        /// <returns>Representation of the original member list binding.</returns>
        protected abstract TMemberListBinding MakeMemberListBinding(MemberListBindingAlias node, ReadOnlyCollection<TElementInit> initializers);

        /// <summary>
        /// Visits the children of the MemberMemberBinding.
        /// </summary>
        /// <param name="node">Member member binding to visit.</param>
        /// <returns>Result of visiting the member member binding.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TMemberMemberBinding VisitMemberMemberBinding(MemberMemberBindingAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Bindings, VisitMemberBinding);
            return MakeMemberMemberBinding(node, b);
        }

        /// <summary>
        /// Makes a member binding object representing a MemberMemberBinding with the given children.
        /// </summary>
        /// <param name="node">Original member member binding.</param>
        /// <param name="bindings">Member bindings.</param>
        /// <returns>Representation of the original member member binding.</returns>
        protected abstract TMemberMemberBinding MakeMemberMemberBinding(MemberMemberBindingAlias node, ReadOnlyCollection<TMemberBinding> bindings);

        /// <summary>
        /// Visits the children of the MethodCallExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitMethodCall(MethodCallExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var o = Visit(node.Object);
#if USE_SLIM
            var a = VisitArguments(node);
#else
            var a = Visit(node.Arguments);
#endif
            return MakeMethodCall(node, o, a);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)
#pragma warning disable CA1716 // Reserved language keyword 'object'. (Mirroring expression tree APIs.)

        /// <summary>
        /// Makes an expression representing a MethodCallExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="object">Object expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeMethodCall(MethodCallExpressionAlias node, TExpression @object, ReadOnlyCollection<TExpression> arguments);

#pragma warning restore CA1716
#pragma warning restore CA1720
#pragma warning restore IDE0079

        /// <summary>
        /// Visits the children of the NewExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitNew(NewExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

#if USE_SLIM
            var a = VisitArguments(node);
#else
            var a = Visit(node.Arguments);
#endif
            return MakeNew(node, a);
        }

        /// <summary>
        /// Makes an expression representing a NewExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="arguments">Argument expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeNew(NewExpressionAlias node, ReadOnlyCollection<TExpression> arguments);

        /// <summary>
        /// Visits the children of the NewArrayExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitNewArray(NewArrayExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var e = Visit(node.Expressions);
            return MakeNewArray(node, e);
        }

        /// <summary>
        /// Makes an expression representing a NewArrayExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expressions">Child expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeNewArray(NewArrayExpressionAlias node, ReadOnlyCollection<TExpression> expressions);

        /// <summary>
        /// Visits the children of the ParameterExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitParameter(ParameterExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return MakeParameter(node);
        }

        /// <summary>
        /// Makes an expression representing a ParameterExpression.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TParameterExpression MakeParameter(ParameterExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the RuntimeVariablesExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var v = VisitAndConvert<ParameterExpression, TParameterExpression>(node.Variables);
            return MakeRuntimeVariables(node, v);
        }

        /// <summary>
        /// Makes an expression representing a RuntimeVariablesExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="variables">Variable expressions.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeRuntimeVariables(RuntimeVariablesExpression node, ReadOnlyCollection<TParameterExpression> variables);
#endif

        /// <summary>
        /// Visits the children of the SwitchExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitSwitch(SwitchExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var s = Visit(node.SwitchValue);
            var d = Visit(node.DefaultBody);
            var c = Visit(node.Cases, VisitSwitchCase);
            return MakeSwitch(node, s, d, c);
        }

        /// <summary>
        /// Makes an expression representing a SwitchExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="switchValue">Switch value expression.</param>
        /// <param name="defaultBody">Default body expression.</param>
        /// <param name="cases">Switch cases.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeSwitch(SwitchExpressionAlias node, TExpression switchValue, TExpression defaultBody, ReadOnlyCollection<TSwitchCase> cases);

        /// <summary>
        /// Visits the children of the SwitchCase.
        /// </summary>
        /// <param name="node">Switch case to visit.</param>
        /// <returns>Result of visiting the switch case.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        virtual TSwitchCase VisitSwitchCase(SwitchCaseAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Body);
            var t = Visit(node.TestValues);
            return MakeSwitchCase(node, b, t);
        }

        /// <summary>
        /// Makes a switch case object representing a SwitchCase with the given children.
        /// </summary>
        /// <param name="node">Original switch case.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="testValues">Test value expressions.</param>
        /// <returns>Representation of the original switch case.</returns>
        protected abstract TSwitchCase MakeSwitchCase(SwitchCaseAlias node, TExpression body, ReadOnlyCollection<TExpression> testValues);

        /// <summary>
        /// Visits the children of the TryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitTry(TryExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Body);
            var i = Visit(node.Finally);
            var a = Visit(node.Fault);
            var h = Visit(node.Handlers, VisitCatchBlock);
            return MakeTry(node, b, i, a, h);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'finally'. (Mirroring expression tree APIs.)

        /// <summary>
        /// Makes an expression representing a TryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="finally">Finally expression.</param>
        /// <param name="fault">Fault expression.</param>
        /// <param name="handlers">Catch handlers.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeTry(TryExpressionAlias node, TExpression body, TExpression @finally, TExpression fault, ReadOnlyCollection<TCatchBlock> handlers);

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Visits the children of the TypeBinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitTypeBinary(TypeBinaryExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var e = Visit(node.Expression);
            return MakeTypeBinary(node, e);
        }

        /// <summary>
        /// Makes an expression representing a TypeBinaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="expression">Child expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeTypeBinary(TypeBinaryExpressionAlias node, TExpression expression);

        /// <summary>
        /// Visits the children of the UnaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        override TExpression VisitUnary(UnaryExpressionAlias node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var o = Visit(node.Operand);
            return MakeUnary(node, o);
        }

        /// <summary>
        /// Makes an expression representing a UnaryExpression with the given children.
        /// </summary>
        /// <param name="node">Original expression.</param>
        /// <param name="operand">Operand expression.</param>
        /// <returns>Representation of the original expression.</returns>
        protected abstract TExpression MakeUnary(UnaryExpressionAlias node, TExpression operand);

        /// <summary>
        /// Visits the elements in the specified input collection.
        /// </summary>
        /// <typeparam name="T">Element type in the input collection.</typeparam>
        /// <typeparam name="R">Element type in the result collection.</typeparam>
        /// <param name="input">Input collection whose elements to visit.</param>
        /// <param name="visit">Function to visit elements in the input collection.</param>
        /// <returns>Collection of visited input elements.</returns>
        private static ReadOnlyCollection<R> Visit<T, R>(ReadOnlyCollection<T> input, Func<T, R> visit)
        {
            var n = input.Count;

            if (n == 0)
            {
                return EmptyReadOnlyCollection<R>.Instance;
            }

            var res = new R[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = visit(input[i]);
            }

            return new TrueReadOnlyCollection<R>(/* transfer ownership */ res);
        }
    }
}
