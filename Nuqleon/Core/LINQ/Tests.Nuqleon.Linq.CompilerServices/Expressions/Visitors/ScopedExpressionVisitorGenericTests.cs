// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
#if USE_SLIM
using System.Linq.CompilerServices.Bonsai;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Bonsai
#else
namespace Tests.System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using BinaryExpression = global::System.Linq.Expressions.BinaryExpressionSlim;
    using BlockExpression = global::System.Linq.Expressions.BlockExpressionSlim;
    using ConditionalExpression = global::System.Linq.Expressions.ConditionalExpressionSlim;
    using ConstantExpression = global::System.Linq.Expressions.ConstantExpressionSlim;
    using DefaultExpression = global::System.Linq.Expressions.DefaultExpressionSlim;
    using Expression = global::System.Linq.Expressions.ExpressionSlim;
    using GotoExpression = global::System.Linq.Expressions.GotoExpressionSlim;
    using IndexExpression = global::System.Linq.Expressions.IndexExpressionSlim;
    using InvocationExpression = global::System.Linq.Expressions.InvocationExpressionSlim;
    using LabelExpression = global::System.Linq.Expressions.LabelExpressionSlim;
    using LambdaExpression = global::System.Linq.Expressions.LambdaExpressionSlim;
    using ListInitExpression = global::System.Linq.Expressions.ListInitExpressionSlim;
    using LoopExpression = global::System.Linq.Expressions.LoopExpressionSlim;
    using MemberExpression = global::System.Linq.Expressions.MemberExpressionSlim;
    using MemberInitExpression = global::System.Linq.Expressions.MemberInitExpressionSlim;
    using MethodCallExpression = global::System.Linq.Expressions.MethodCallExpressionSlim;
    using NewArrayExpression = global::System.Linq.Expressions.NewArrayExpressionSlim;
    using NewExpression = global::System.Linq.Expressions.NewExpressionSlim;
    using ParameterExpression = global::System.Linq.Expressions.ParameterExpressionSlim;
    using SwitchExpression = global::System.Linq.Expressions.SwitchExpressionSlim;
    using TryExpression = global::System.Linq.Expressions.TryExpressionSlim;
    using TypeBinaryExpression = global::System.Linq.Expressions.TypeBinaryExpressionSlim;
    using UnaryExpression = global::System.Linq.Expressions.UnaryExpressionSlim;

    using ElementInit = global::System.Linq.Expressions.ElementInitSlim;
    using MemberAssignment = global::System.Linq.Expressions.MemberAssignmentSlim;
    using MemberBinding = global::System.Linq.Expressions.MemberBindingSlim;
    using MemberListBinding = global::System.Linq.Expressions.MemberListBindingSlim;
    using MemberMemberBinding = global::System.Linq.Expressions.MemberMemberBindingSlim;

    using CatchBlock = global::System.Linq.Expressions.CatchBlockSlim;
    using LabelTarget = global::System.Linq.Expressions.LabelTargetSlim;
    using SwitchCase = global::System.Linq.Expressions.SwitchCaseSlim;

    using Type = global::System.Reflection.TypeSlim;

    #endregion
#endif

    [TestClass]
    public class ScopedExpressionVisitorGenericTests
    {
        [TestMethod]
        public void ScopedExpressionVisitorGeneric_ProtectedNulls()
        {
            new ProtectedNullVisitor().Do();
        }

        [TestMethod]
        public void ScopedExpressionVisitorGeneric_ParametersScoped()
        {
            var p0 = Expression.Parameter(ToType(typeof(int)), "p0");
            var p1 = Expression.Parameter(ToType(typeof(int)), "p1");
            var add = Expression.Add(p0, p1);
            var d = Expression.Default(ToType(typeof(int)));

            var exprs = new Expression[]
            {
                Expression.Lambda(Expression.Lambda(add, p1), p0),
                Expression.Block(new[] { p0 }, Expression.Block(new[] { p1 }, add)),
                Expression.TryCatch(d, Expression.Catch(p0, Expression.TryCatch(d, Expression.Catch(p1, add)))),
            };

            foreach (var e in exprs)
            {
                new ScopeAsserter().Visit(e); // Assertions inside visitor
            }
        }

        [TestMethod]
        public void ScopedExpressionVisitorGeneric_Globals()
        {
            var g = Expression.Parameter(ToType(typeof(int)), "g");
            var p0 = Expression.Parameter(ToType(typeof(int)), "p0");
            var add = Expression.Add(g, p0);
            var expr = Expression.Lambda(add, p0);

            var a1 = new GlobalScopeAsserter();
            Assert.ThrowsException<UnboundVariableException>(() => a1.Visit(expr));

            var a2 = new GlobalScopeAsserter();
            a2.GlobalScope.Add(g, g);
            a2.Visit(expr); // does not throw

            var a3 = new GlobalScopeAsserter();
            a3.PushGlobal(g);
            a3.Visit(expr); // does not throw
        }

        private static Type ToType(global::System.Type type)
        {
#if USE_SLIM
            return type.ToTypeSlim();
#else
            return type;
#endif
        }

#if USE_SLIM
        private abstract class TestVisitorBase<TState> : ScopedExpressionSlimVisitor<TState, Expression, LambdaExpression, ParameterExpression, NewExpression, ElementInit, MemberBinding, MemberAssignment, MemberListBinding, MemberMemberBinding, CatchBlock, SwitchCase, LabelTarget>
#else
        private abstract class TestVisitorBase<TState> : ScopedExpressionVisitor<TState, Expression, LambdaExpression, ParameterExpression, NewExpression, ElementInit, MemberBinding, MemberAssignment, MemberListBinding, MemberMemberBinding, CatchBlock, SwitchCase, LabelTarget>
#endif
        {
            protected override Expression MakeBinary(BinaryExpression node, Expression left, LambdaExpression conversion, Expression right) => node;

            protected override Expression MakeBlock(BlockExpression node, ReadOnlyCollection<ParameterExpression> variables, ReadOnlyCollection<Expression> expressions) => node;

            protected override CatchBlock MakeCatchBlock(CatchBlock node, ParameterExpression variable, Expression body, Expression filter) => node;

            protected override Expression MakeConditional(ConditionalExpression node, Expression test, Expression ifTrue, Expression ifFalse) => node;

            protected override Expression MakeConstant(ConstantExpression node) => node;

#if !USE_SLIM
            protected override Expression MakeDebugInfo(DebugInfoExpression node) => node;
#endif

            protected override Expression MakeDefault(DefaultExpression node) => node;

#if !USE_SLIM
            protected override Expression MakeDynamic(DynamicExpression node, ReadOnlyCollection<Expression> arguments) => node;
#endif

            protected override ElementInit MakeElementInit(ElementInit node, ReadOnlyCollection<Expression> arguments) => node;

            protected override Expression MakeGoto(GotoExpression node, LabelTarget target, Expression value) => node;

            protected override Expression MakeIndex(IndexExpression node, Expression @object, ReadOnlyCollection<Expression> arguments) => node;

            protected override Expression MakeInvocation(InvocationExpression node, Expression expression, ReadOnlyCollection<Expression> arguments) => node;

            protected override Expression MakeLabel(LabelExpression node, LabelTarget target, Expression defaultValue) => node;

            protected override LabelTarget MakeLabelTarget(LabelTarget node) => node;

#if USE_SLIM
            protected override LambdaExpression MakeLambda(LambdaExpression node, Expression body, ReadOnlyCollection<ParameterExpression> parameters) => node;
#else
            protected override LambdaExpression MakeLambda<T>(Expression<T> node, Expression body, ReadOnlyCollection<ParameterExpression> parameters) => node;
#endif

            protected override Expression MakeListInit(ListInitExpression node, NewExpression newExpression, ReadOnlyCollection<ElementInit> initializers) => node;

            protected override Expression MakeLoop(LoopExpression node, Expression body, LabelTarget breakLabel, LabelTarget continueLabel) => node;

            protected override Expression MakeMember(MemberExpression node, Expression expression) => node;

            protected override MemberAssignment MakeMemberAssignment(MemberAssignment node, Expression expression) => node;

            protected override Expression MakeMemberInit(MemberInitExpression node, NewExpression newExpression, ReadOnlyCollection<MemberBinding> bindings) => node;

            protected override MemberListBinding MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<ElementInit> initializers) => node;

            protected override MemberMemberBinding MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<MemberBinding> bindings) => node;

            protected override Expression MakeMethodCall(MethodCallExpression node, Expression @object, ReadOnlyCollection<Expression> arguments) => node;

            protected override Expression MakeNew(NewExpression node, ReadOnlyCollection<Expression> arguments) => node;

            protected override Expression MakeNewArray(NewArrayExpression node, ReadOnlyCollection<Expression> expressions) => node;

            protected override ParameterExpression MakeParameter(ParameterExpression node) => node;

#if !USE_SLIM
            protected override Expression MakeRuntimeVariables(RuntimeVariablesExpression node, ReadOnlyCollection<ParameterExpression> variables) => node;
#endif

            protected override Expression MakeSwitch(SwitchExpression node, Expression switchValue, Expression defaultBody, ReadOnlyCollection<SwitchCase> cases) => node;

            protected override SwitchCase MakeSwitchCase(SwitchCase node, Expression body, ReadOnlyCollection<Expression> testValues) => node;

            protected override Expression MakeTry(TryExpression node, Expression body, Expression @finally, Expression fault, ReadOnlyCollection<CatchBlock> handlers) => node;

            protected override Expression MakeTypeBinary(TypeBinaryExpression node, Expression expression) => node;

            protected override Expression MakeUnary(UnaryExpression node, Expression operand) => node;
        }

        private sealed class ProtectedNullVisitor : TestVisitorBase<ParameterExpression>
        {
            public void Do()
            {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitBlock(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitBlockCore(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitCatchBlock(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitCatchBlockCore(node: null), ex => Assert.AreEqual("node", ex.ParamName));
#if USE_SLIM
                AssertEx.Throws<ArgumentNullException>(() => base.VisitLambda(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.Throws<ArgumentNullException>(() => base.VisitLambdaCore(node: null), ex => Assert.AreEqual("node", ex.ParamName));
#else
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLambda<Action>(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLambdaCore<Action>(node: null), ex => Assert.AreEqual("node", ex.ParamName));
#endif
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Push(default(IEnumerable<ParameterExpression>)), ex => Assert.AreEqual("parameters", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Push(default(IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>>)), ex => Assert.AreEqual("scope", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => { base.TryLookup(parameter: null, out _); }, ex => Assert.AreEqual("parameter", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }

        private sealed class ScopeAsserter : TestVisitorBase<int>
        {
            private int scope = 0;

            protected override Expression MakeBlock(BlockExpression node, ReadOnlyCollection<ParameterExpression> variables, ReadOnlyCollection<Expression> expressions)
            {
                foreach (var p in node.Variables)
                {
                    Assert.IsTrue(TryLookup(p, out var scope));
                    Assert.AreEqual("p" + scope, p.Name);
                }

                return base.MakeBlock(node, variables, expressions);
            }

            protected override CatchBlock MakeCatchBlock(CatchBlock node, ParameterExpression variable, Expression body, Expression filter)
            {
                if (node.Variable != null)
                {
                    Assert.IsTrue(TryLookup(node.Variable, out var scope));
                    Assert.AreEqual("p" + scope, node.Variable.Name);
                }

                return base.MakeCatchBlock(node, variable, body, filter);
            }

#if USE_SLIM
            protected override LambdaExpression MakeLambda(LambdaExpression node, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
#else
            protected override LambdaExpression MakeLambda<T>(Expression<T> node, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
#endif
            {
                var scope = default(int);
                foreach (var p in node.Parameters)
                {
                    Assert.IsTrue(TryLookup(p, out scope));
                    Assert.AreEqual("p" + scope, p.Name);
                }

#if USE_SLIM
                return base.MakeLambda(node, body, parameters);
#else
                return base.MakeLambda<T>(node, body, parameters);
#endif
            }

            protected override void Push(IEnumerable<ParameterExpression> parameters)
            {
                base.Push(parameters);
                scope++;
            }

            protected override void Pop()
            {
                base.Pop();
                scope--;
            }

            protected override int GetState(ParameterExpression parameter) => scope;
        }

        private sealed class GlobalScopeAsserter : TestVisitorBase<ParameterExpression>
        {
            public void PushGlobal(ParameterExpression global)
            {
                Push(new[] { new KeyValuePair<ParameterExpression, ParameterExpression>(global, global) });
            }

#if USE_SLIM
            protected internal override Expression VisitParameter(ParameterExpression node)
#else
            protected override Expression VisitParameter(ParameterExpression node)
#endif
            {
                if (!TryLookup(node, out _))
                {
                    throw new UnboundVariableException();
                }

                return base.VisitParameter(node);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }

        private sealed class UnboundVariableException : Exception { }
    }
}
