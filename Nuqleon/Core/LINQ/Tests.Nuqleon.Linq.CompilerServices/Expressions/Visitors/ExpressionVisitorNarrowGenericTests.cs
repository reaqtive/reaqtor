// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionVisitorNarrowGenericTests
    {
        [TestMethod]
        public void ExpressionVisitorNarrow_NotSupported()
        {
            new MyVisitor1().Test();
            new MyVisitor2().Test();
        }

        private class MyExt : Expression
        {
            public override ExpressionType NodeType => ExpressionType.Extension;
        }

        private class MyVisitor1 : ExpressionVisitorNarrow<string>
        {
            public void Test()
            {
                var block = Expression.Block(Expression.Constant(1));

                var add = Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                    Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                    ExpressionType.Add,
                    typeof(ExpressionEqualityComparerTests),
                    new[]
                    {
                        Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null),
                        Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null)
                    }
                );

                Assert.ThrowsException<NotSupportedException>(() => VisitBlock(block));
                Assert.ThrowsException<NotSupportedException>(() => VisitDebugInfo(Expression.DebugInfo(Expression.SymbolDocument("foo.txt"), 1, 1, 1, 1)));
                Assert.ThrowsException<NotSupportedException>(() => VisitDynamic(Expression.Dynamic(add, typeof(object), Expression.Constant(1), Expression.Constant(2))));
                Assert.ThrowsException<NotSupportedException>(() => VisitExtension(new MyExt()));
                Assert.ThrowsException<NotSupportedException>(() => VisitGoto(Expression.Return(Expression.Label(""))));
                Assert.ThrowsException<NotSupportedException>(() => VisitIndex(Expression.MakeIndex(Expression.Constant(new List<int>()), typeof(List<int>).GetProperty("Item"), new Expression[] { Expression.Constant(1) })));
                Assert.ThrowsException<NotSupportedException>(() => VisitLabel(Expression.Label(Expression.Label(""))));
                Assert.ThrowsException<NotSupportedException>(() => VisitLoop(Expression.Loop(block)));
                Assert.ThrowsException<NotSupportedException>(() => VisitRuntimeVariables(Expression.RuntimeVariables()));
                Assert.ThrowsException<NotSupportedException>(() => VisitSwitch(Expression.Switch(Expression.Constant(1), Expression.Constant(1), Expression.SwitchCase(block, Expression.Constant(1)))));
                Assert.ThrowsException<NotSupportedException>(() => VisitTry(Expression.TryFinally(block, block)));
            }

            protected override string VisitBinary(BinaryExpression node) => throw new NotImplementedException();

            protected override string VisitMethodCall(MethodCallExpression node) => throw new NotImplementedException();

            protected override string VisitConditional(ConditionalExpression node) => throw new NotImplementedException();

            protected override string VisitConstant(ConstantExpression node) => throw new NotImplementedException();

            protected override string VisitDefault(DefaultExpression node) => throw new NotImplementedException();

            protected override string VisitInvocation(InvocationExpression node) => throw new NotImplementedException();

            protected override string VisitLambda<T>(Expression<T> node) => throw new NotImplementedException();

            protected override string VisitListInit(ListInitExpression node) => throw new NotImplementedException();

            protected override string VisitMember(MemberExpression node) => throw new NotImplementedException();

            protected override string VisitMemberInit(MemberInitExpression node) => throw new NotImplementedException();

            protected override string VisitNew(NewExpression node) => throw new NotImplementedException();

            protected override string VisitNewArray(NewArrayExpression node) => throw new NotImplementedException();

            protected override string VisitParameter(ParameterExpression node) => throw new NotImplementedException();

            protected override string VisitTypeBinary(TypeBinaryExpression node) => throw new NotImplementedException();

            protected override string VisitUnary(UnaryExpression node) => throw new NotImplementedException();
        }

        private class MyVisitor2 : ExpressionVisitorNarrow<Expression, LambdaExpression, ParameterExpression, NewExpression, ElementInit, MemberBinding, MemberAssignment, MemberListBinding, MemberMemberBinding>
        {
            public void Test()
            {
                var block = Expression.Block(Expression.Constant(1));

                var add = Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                    Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                    ExpressionType.Add,
                    typeof(ExpressionEqualityComparerTests),
                    new[]
                    {
                        Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null),
                        Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null)
                    }
                );

                Assert.ThrowsException<NotSupportedException>(() => MakeBlock(block, variables: null, expressions: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeDebugInfo(Expression.DebugInfo(Expression.SymbolDocument("foo.txt"), 1, 1, 1, 1)));
                Assert.ThrowsException<NotSupportedException>(() => MakeDynamic(Expression.Dynamic(add, typeof(object), Expression.Constant(1), Expression.Constant(2)), arguments: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeGoto(Expression.Return(Expression.Label("")), target: null, value: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeIndex(Expression.MakeIndex(Expression.Constant(new List<int>()), typeof(List<int>).GetProperty("Item"), new Expression[] { Expression.Constant(1) }), @object: null, arguments: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeLabel(Expression.Label(Expression.Label("")), target: null, defaultValue: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeLoop(Expression.Loop(block), body: null, breakLabel: null, continueLabel: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeRuntimeVariables(Expression.RuntimeVariables(), variables: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeSwitch(Expression.Switch(Expression.Constant(1), Expression.Constant(1), Expression.SwitchCase(block, Expression.Constant(1))), switchValue: null, defaultBody: null, cases: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeTry(Expression.TryFinally(block, block), body: null, @finally: null, fault: null, handlers: null));

                Assert.ThrowsException<NotSupportedException>(() => MakeCatchBlock(Expression.MakeCatchBlock(typeof(Exception), Expression.Parameter(typeof(Exception)), block, filter: null), variable: null, body: null, filter: null));
                Assert.ThrowsException<NotSupportedException>(() => MakeLabelTarget(Expression.Label("")));
                Assert.ThrowsException<NotSupportedException>(() => MakeSwitchCase(Expression.SwitchCase(block, Expression.Constant(1)), body: null, testValues: null));
            }

            protected override Expression MakeBinary(BinaryExpression node, Expression left, LambdaExpression conversion, Expression right) => throw new NotImplementedException();

            protected override Expression MakeConditional(ConditionalExpression node, Expression test, Expression ifTrue, Expression ifFalse) => throw new NotImplementedException();

            protected override Expression MakeConstant(ConstantExpression node) => throw new NotImplementedException();

            protected override Expression MakeDefault(DefaultExpression node) => throw new NotImplementedException();

            protected override ElementInit MakeElementInit(ElementInit node, ReadOnlyCollection<Expression> arguments) => throw new NotImplementedException();

            protected override Expression MakeInvocation(InvocationExpression node, Expression expression, ReadOnlyCollection<Expression> arguments) => throw new NotImplementedException();

            protected override LambdaExpression MakeLambda<T>(Expression<T> node, Expression body, ReadOnlyCollection<ParameterExpression> parameters) => throw new NotImplementedException();

            protected override Expression MakeListInit(ListInitExpression node, NewExpression newExpression, ReadOnlyCollection<ElementInit> initializers) => throw new NotImplementedException();

            protected override Expression MakeMember(MemberExpression node, Expression expression) => throw new NotImplementedException();

            protected override MemberAssignment MakeMemberAssignment(MemberAssignment node, Expression expression) => throw new NotImplementedException();

            protected override Expression MakeMemberInit(MemberInitExpression node, NewExpression newExpression, ReadOnlyCollection<MemberBinding> bindings) => throw new NotImplementedException();

            protected override MemberListBinding MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<ElementInit> initializers) => throw new NotImplementedException();

            protected override MemberMemberBinding MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<MemberBinding> bindings) => throw new NotImplementedException();

            protected override Expression MakeMethodCall(MethodCallExpression node, Expression @object, ReadOnlyCollection<Expression> arguments) => throw new NotImplementedException();

            protected override Expression MakeNew(NewExpression node, ReadOnlyCollection<Expression> arguments) => throw new NotImplementedException();

            protected override Expression MakeNewArray(NewArrayExpression node, ReadOnlyCollection<Expression> expressions) => throw new NotImplementedException();

            protected override ParameterExpression MakeParameter(ParameterExpression node) => throw new NotImplementedException();

            protected override Expression MakeTypeBinary(TypeBinaryExpression node, Expression expression) => throw new NotImplementedException();

            protected override Expression MakeUnary(UnaryExpression node, Expression operand) => throw new NotImplementedException();
        }
    }
}
