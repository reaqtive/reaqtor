// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace System.Linq.Expressions
{
    [TestClass]
    public class ExpressionSlimVisitorTests : TestBase
    {
        [TestMethod]
        public void ExpressionSlimVisitor_NullArgumentChecks()
        {
            var visitor = new ExpressionSlimVisitor();
            Assert.IsNull(visitor.Visit((ExpressionSlim)null));
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.Visit(nodes: null), ex => Assert.AreEqual("nodes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.VisitAndConvert<ExpressionSlim>(nodes: null), ex => Assert.AreEqual("nodes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionSlimVisitor.Visit<ExpressionSlim>(nodes: null, elementVisitor: null), ex => Assert.AreEqual("nodes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionSlimVisitor.Visit<ExpressionSlim>(new List<ExpressionSlim>().AsReadOnly(), elementVisitor: null), ex => Assert.AreEqual("elementVisitor", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitAndConvert_Fail()
        {
            var visitor = new A();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = Expression.Constant(0);
            var slim = slimifier.Visit(expression) as ConstantExpressionSlim;
            Assert.ThrowsException<InvalidOperationException>(() => visitor.VisitAndConvert<ConstantExpressionSlim>(slim));
        }

        private sealed class A : ExpressionSlimVisitor
        {
            protected internal override ExpressionSlim VisitConstant(ConstantExpressionSlim node)
            {
                return ExpressionSlim.LessThan(node, node);
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitAndConvert_Collection_CreateNewCollection()
        {
            var visitor = new B();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var slims = (new[] { Expression.Constant("foo"), Expression.Constant(0), Expression.Constant(1) }).Select(expr => slimifier.Visit(expr) as ConstantExpressionSlim);
            var readOnlySlims = slims.ToList().AsReadOnly();
            Assert.AreNotSame(readOnlySlims, visitor.VisitAndConvert<ConstantExpressionSlim>(readOnlySlims));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_Visit_Collection_CreateNewCollection()
        {
            var visitor = new B();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var slims = (new[] { Expression.Constant("foo"), Expression.Constant(0), Expression.Constant(1) }).Select(slimifier.Visit);
            var readOnlySlims = slims.ToList().AsReadOnly();
            Assert.AreNotSame(readOnlySlims, visitor.Visit(readOnlySlims));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_Visit_Static_CreateNewCollection()
        {
            var visitor = new B();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var slims = (new[] { Expression.Constant("foo"), Expression.Constant(0), Expression.Constant(1) }).Select(expr => slimifier.Visit(expr) as ConstantExpressionSlim);
            var readOnlySlims = slims.ToList().AsReadOnly();
            Assert.AreNotSame(readOnlySlims, B.Visit<ConstantExpressionSlim>(readOnlySlims, visitor.VisitAndConvert<ConstantExpressionSlim>));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_Visit_Static_AreSame()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var slims = new[] { slimifier.Visit(Expression.Constant(0)) as ConstantExpressionSlim };
            var readOnlySlims = slims.ToList().AsReadOnly();
            Assert.AreSame(readOnlySlims, ExpressionSlimVisitor.Visit<ConstantExpressionSlim>(readOnlySlims, visitor.VisitAndConvert<ConstantExpressionSlim>));
        }

        private sealed class B : ExpressionSlimVisitor
        {
            protected internal override ExpressionSlim VisitConstant(ConstantExpressionSlim node)
            {
                if (node.Type.Kind == TypeSlimKind.Simple)
                {
                    var simple = (SimpleTypeSlim)node.Type;
                    if (simple.Name.Contains("Int"))
                        return ExpressionSlim.Constant(node.Value, node.Type);
                }
                return base.VisitConstant(node);
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitor_Visit_Success1()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var parameters = new[] { Expression.Parameter(typeof(int)), Expression.Parameter(typeof(int)) };
            var expression = Expression.Invoke(
                Expression.Lambda(
                    Expression.Condition(
                        Expression.OrElse(
                            Expression.LessThan(
                                parameters[0],
                                Expression.Constant(5)
                            ),
                            Expression.TypeIs(
                                parameters[0],
                                typeof(int)
                            )
                        ),
                        Expression.Increment(parameters[0]),
                        Expression.Decrement(parameters[1])
                    ),
                    parameters
                ),
                Expression.Constant(0),
                Expression.Constant(2)
            );
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_Visit_Success2()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var fs = new[] { new MyFoo(), new MyFoo() };
            var expression = (Expression<Func<bool>>)(() => fs[0].Count() == fs[0][0] && fs[0].Qux == fs[1].Qux);
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitNew_Success1()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = (Expression<Func<TimeSpan>>)(() => new TimeSpan());
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitNew_Success2()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = (Expression<Func<TimeSpan>>)(() => new TimeSpan(42L));
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitMemberInit_Success()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = (Expression<Func<MyFoo>>)(() => new MyFoo { Qux = 1, Bar = { Baz = 2 }, List = { 1, 2 } });
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitNewArray_Success()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = (Expression<Func<MyFoo[]>>)(() => new[] { new MyFoo() });
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitListInit_Success()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = (Expression<Func<List<int>>>)(() => new List<int> { 1, 2, 3 });
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitDefault_Success()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = Expression.Default(typeof(int));
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitIndex()
        {
            var visitor = new ExpressionSlimVisitor();
            var slimifier = new ExpressionToExpressionSlimConverter();
            var expression = Expression.MakeIndex(Expression.Parameter(typeof(MyFoo)), typeof(MyFoo).GetProperty("Item"), new[] { Expression.Constant(0) });
            var slim = slimifier.Visit(expression);
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitElementInit()
        {
            Expression<Func<List<int>>> f = () => new List<int> { 43 };
            var expression = f.Body;

            var slimifier = new ExpressionToExpressionSlimConverter();
            var slim = slimifier.Visit(expression);

            var visitor = new MyConstSubstVisitor();
            var res = visitor.Visit(slim);

            var value = res.ToExpression().Evaluate<List<int>>();
            Assert.IsTrue(value.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitMemberBinding1()
        {
            Expression<Func<MyFoo>> f = () => new MyFoo { Qux = 43 };
            var expression = f.Body;

            var slimifier = new ExpressionToExpressionSlimConverter();
            var slim = slimifier.Visit(expression);

            var visitor = new MyConstSubstVisitor();
            var res = visitor.Visit(slim);

            var value = res.ToExpression().Evaluate<MyFoo>();
            Assert.AreEqual(42, value.Qux);
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitMemberBinding2()
        {
            Expression<Func<MyFoo>> f = () => new MyFoo(43) { Bar = { Baz = 41 } };
            var expression = f.Body;

            var slimifier = new ExpressionToExpressionSlimConverter();
            var slim = slimifier.Visit(expression);

            var visitor = new MyConstSubstVisitor();
            var res = visitor.Visit(slim);

            var value = res.ToExpression().Evaluate<MyFoo>();
            Assert.AreEqual(42, value.Qux);
            Assert.AreEqual(42, value.Bar.Baz);
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitLoop()
        {
            var @break = Expression.Label(typeof(int), "b");
            var @continue = Expression.Label("c");

            var slimifier = new ExpressionToExpressionSlimConverter();
            var substitutingVisitor = new MyConstSubstVisitor();
            var nopVisitor = new ExpressionSlimVisitor();

            {
                var loop = Expression.Loop(Expression.Constant(41));
                var slim = slimifier.Visit(loop);
                var fat = slim.ToExpression();
                Assert.AreEqual(slim, nopVisitor.Visit(slim));
                Assert.IsTrue(new ExpressionEqualityComparer().Equals(loop, fat));
            }

            {
                var loop = Expression.Loop(Expression.Break(@break, Expression.Constant(41), typeof(int)), @break);
                var slim = slimifier.Visit(loop);
                Assert.AreEqual(slim, nopVisitor.Visit(slim));
                var res = substitutingVisitor.Visit(slim);
                var value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);

                var breakFinder = new LabelTargetFinder(@break.Name);
                breakFinder.Visit(res);
                var breakSubstitutor = new LabelTargetSubstitutor(breakFinder.Target, ExpressionSlim.Label(typeof(int).ToTypeSlim()));
                res = breakSubstitutor.Visit(res);
                value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);
            }

            {
                var loop = Expression.Loop(Expression.Break(@break, Expression.Constant(41), typeof(int)), @break, @continue);
                var slim = slimifier.Visit(loop);
                Assert.AreEqual(slim, nopVisitor.Visit(slim));

                var res = substitutingVisitor.Visit(slim);
                var value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);

                var continueFinder = new LabelTargetFinder(@continue.Name);
                continueFinder.Visit(res);
                var continueSubstitutor = new LabelTargetSubstitutor(continueFinder.Target, ExpressionSlim.Label());
                res = continueSubstitutor.Visit(res);
                value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitBlock()
        {
            var slimifier = new ExpressionToExpressionSlimConverter();
            var substitutingVisitor = new MyConstSubstVisitor();
            var nopVisitor = new ExpressionSlimVisitor();

            var p = Expression.Variable(typeof(int), "p");
            var expr = Expression.Block(new[] { p }, Expression.Constant(41));
            var slim = slimifier.Visit(expr);
            Assert.AreEqual(slim, nopVisitor.Visit(slim));

            var res = substitutingVisitor.Visit(slim);
            var value = res.ToExpression().Evaluate<int>();
            Assert.AreEqual(42, value);

            var paramFinder = new ParameterFinder(p.Name);
            paramFinder.Visit(slim);
            var paramSubstitutor = new ParameterSubstitutor(paramFinder.Parameter, (ParameterExpressionSlim)Expression.Variable(typeof(int), "p").ToExpressionSlim());
            res = paramSubstitutor.Visit(slim);
            value = res.ToExpression().Evaluate<int>();
            Assert.AreEqual(41, value);
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitTryCatch()
        {
            var slimifier = new ExpressionToExpressionSlimConverter();
            var substitutingVisitor = new MyConstSubstVisitor();
            var nopVisitor = new ExpressionSlimVisitor();

            {
                var ex = (Expression<Func<Exception>>)(() => new Exception());
                var p = Expression.Parameter(typeof(Exception), "a");
                var expr =
                    Expression.TryCatch(
                        Expression.Throw(ex.Body, typeof(int)),
                        Expression.Catch(
                            p,
                            Expression.Constant(43)
                        )
                    );
                var slim = slimifier.Visit(expr);
                Assert.AreEqual(slim, nopVisitor.Visit(slim));

                var res = substitutingVisitor.Visit(slim);
                var value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);

                var paramFinder = new ParameterFinder(p.Name);
                paramFinder.Visit(slim);
                var paramSubstitutor = new ParameterSubstitutor(paramFinder.Parameter, (ParameterExpressionSlim)Expression.Variable(typeof(Exception), "p").ToExpressionSlim());
                res = paramSubstitutor.Visit(slim);
                value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(43, value);
            }

            {
                var e = Expression.TryFault(Expression.Constant(42), Expression.Constant(42));
                var s = slimifier.Visit(e);
                var f = s.ToExpression();

                Assert.AreEqual(s, nopVisitor.Visit(s));
                var c = new ExpressionEqualityComparer();
                Assert.IsTrue(c.Equals(e, f));
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitSwitchCase()
        {
            var slimifier = new ExpressionToExpressionSlimConverter();
            var substitutingVisitor = new MyConstSubstVisitor();
            var nopVisitor = new ExpressionSlimVisitor();

            {
                var ex = (Expression<Func<Exception>>)(() => new Exception());
                var expr =
                    Expression.Switch(
                        Expression.Constant(0),
                        Expression.Throw(ex.Body, typeof(int)),
                        Expression.SwitchCase(
                            Expression.Constant(43),
                            Expression.Constant(0)
                        )
                    );
                var slim = slimifier.Visit(expr);
                Assert.AreEqual(slim, nopVisitor.Visit(slim));

                var res = substitutingVisitor.Visit(slim);
                var value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);
            }

            {
                var ex = (Expression<Func<Exception>>)(() => new Exception());
                var method = ((MethodCallExpression)((Expression<Func<int, int, bool>>)((x, y) => Eq(x, y))).Body).Method;
                var expr =
                    Expression.Switch(
                        Expression.Constant(0),
                        Expression.Throw(ex.Body, typeof(int)),
                        method,
                        Expression.SwitchCase(
                            Expression.Constant(43),
                            Expression.Constant(0)
                        )
                    );
                var slim = slimifier.Visit(expr);
                Assert.AreEqual(slim, nopVisitor.Visit(slim));

                var res = substitutingVisitor.Visit(slim);
                var value = res.ToExpression().Evaluate<int>();
                Assert.AreEqual(42, value);
            }
        }

        private static bool Eq(int a, int b) { return a == b; }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitLabels()
        {
            var slimifier = new ExpressionToExpressionSlimConverter();
            var substitutingVisitor = new MyConstSubstVisitor();
            var nopVisitor = new ExpressionSlimVisitor();

            var ex = (Expression<Func<Exception>>)(() => new Exception());
            var target = Expression.Label(typeof(int));
            var expr =
                Expression.Block(
                    Expression.Goto(
                        target,
                        Expression.Constant(41)),
                    Expression.Throw(ex.Body),
                    Expression.Label(target, Expression.Constant(41))
                );
            var slim = slimifier.Visit(expr);
            Assert.AreEqual(slim, nopVisitor.Visit(slim));

            var res = substitutingVisitor.Visit(slim);
            var value = res.ToExpression().Evaluate<int>();
            Assert.AreEqual(42, value);

            var targetFinder = new LabelTargetFinder(target.Name);
            targetFinder.Visit(res);
            var targetSubstitutor = new LabelTargetSubstitutor(targetFinder.Target, ExpressionSlim.Label(typeof(int).ToTypeSlim()));
            res = targetSubstitutor.Visit(res);
            value = res.ToExpression().Evaluate<int>();
            Assert.AreEqual(42, value);
        }

        private class LabelTargetFinder : ExpressionSlimVisitor
        {
            private readonly string _name;

            public LabelTargetFinder(string name)
            {
                _name = name;
            }

            internal LabelTargetSlim Target
            {
                get;
                private set;
            }

            protected internal override LabelTargetSlim VisitLabelTarget(LabelTargetSlim node)
            {
                if (node != null && node.Name == _name)
                {
                    if (Target == null)
                    {
                        Target = node;
                    }
                    else if (Target != node)
                    {
                        throw new Exception("Multiple labels exist with the same name.");
                    }
                }

                return node;
            }
        }

        private class LabelTargetSubstitutor : ExpressionSlimVisitor
        {
            private readonly LabelTargetSlim _before;
            private readonly LabelTargetSlim _after;

            public LabelTargetSubstitutor(LabelTargetSlim before, LabelTargetSlim after)
            {
                _before = before;
                _after = after;
            }

            protected internal override LabelTargetSlim VisitLabelTarget(LabelTargetSlim node)
            {
                if (node == _before)
                    return _after;
                else
                    return node;
            }
        }

        private class ParameterFinder : ExpressionSlimVisitor
        {
            private readonly string _name;

            public ParameterFinder(string name)
            {
                _name = name;
            }

            internal ParameterExpressionSlim Parameter
            {
                get;
                private set;
            }

            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                if (node != null && node.Name != null && node.Name == _name)
                {
                    if (Parameter == null)
                    {
                        Parameter = node;
                    }
                    else if (Parameter != node)
                    {
                        throw new Exception("Multiple parameters exist with the same name.");
                    }
                }

                return node;
            }
        }

        private class ParameterSubstitutor : ExpressionSlimVisitor
        {
            private readonly ParameterExpressionSlim _before;
            private readonly ParameterExpressionSlim _after;

            public ParameterSubstitutor(ParameterExpressionSlim before, ParameterExpressionSlim after)
            {
                _before = before;
                _after = after;
            }

            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                if (node == _before)
                    return _after;
                else
                    return node;
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitor_VisitLoopComplex()
        {
            var counter = Expression.Variable(typeof(int), "counter");
            var @break = Expression.Label(typeof(int));

            var loop =
                Expression.Loop(
                    Expression.Block(
                        typeof(int),
                        new[] { counter },
                        Expression.AddAssign(counter, Expression.Constant(2)),
                        Expression.Condition(
                            Expression.GreaterThanOrEqual(
                                counter,
                                Expression.Constant(1337)
                            ),
                            Expression.Break(@break, counter, typeof(int)),
                            Expression.Constant(42)
                        )
                    ),
                    @break
                );

            var slimifier = new ExpressionToExpressionSlimConverter();
            var slim = slimifier.Visit(loop);

            var value = slim.ToExpression().Evaluate<int>();

            Assert.AreEqual(1338, value);
        }

        [TestMethod]
        public void ExpressionSlimVisitor_CountNodes()
        {
            Expression<Func<MyFoo>> f = () => new MyFoo { Bar = { Baz = 1 }, List = { 2, 3 }, Qux = 4 };
            var expression = f.Body;

            var slimifier = new ExpressionToExpressionSlimConverter();
            var slim = slimifier.Visit(expression);

            var visitor = new MyCountingVisitor();
            var res = visitor.Visit(slim);

            Assert.AreEqual(12, res); // MemberInit(New, MemberMember(MemberAssignment(Constant)), MemberList(ElementInit(Constant), ElementInit(Constant)), MemberAssignment(Constant))
        }

        private sealed class MyConstSubstVisitor : ExpressionSlimVisitor
        {
            protected internal override ExpressionSlim VisitConstant(ConstantExpressionSlim node)
            {
                return Expression.Constant(42).ToExpressionSlim();
            }
        }

        private sealed class MyCountingVisitor : ExpressionSlimVisitor<int, int, int, int, int, int>
        {
            protected override int MakeBinary(BinaryExpressionSlim node, int left, int conversion, int right)
            {
                return left + right + 1;
            }

            protected override int MakeConditional(ConditionalExpressionSlim node, int test, int ifTrue, int ifFalse)
            {
                return test + ifTrue + ifFalse + 1;
            }

            protected override int MakeConstant(ConstantExpressionSlim node)
            {
                return 1;
            }

            protected override int MakeDefault(DefaultExpressionSlim node)
            {
                return 1;
            }

            protected override int MakeElementInit(ElementInitSlim node, ReadOnlyCollection<int> arguments)
            {
                return arguments.Sum() + 1;
            }

            protected override int MakeIndex(IndexExpressionSlim node, int @object, ReadOnlyCollection<int> arguments)
            {
                return @object + arguments.Sum() + 1;
            }

            protected override int MakeInvocation(InvocationExpressionSlim node, int expression, ReadOnlyCollection<int> arguments)
            {
                return expression + arguments.Sum() + 1;
            }

            protected override int MakeLambda(LambdaExpressionSlim node, int body, ReadOnlyCollection<int> parameters)
            {
                return body + parameters.Sum() + 1;
            }

            protected override int MakeListInit(ListInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> initializers)
            {
                return newExpression + initializers.Sum() + 1;
            }

            protected override int MakeMember(MemberExpressionSlim node, int expression)
            {
                return expression + 1;
            }

            protected override int MakeMemberAssignment(MemberAssignmentSlim node, int expression)
            {
                return expression + 1;
            }

            protected override int MakeMemberInit(MemberInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> bindings)
            {
                return newExpression + bindings.Sum() + 1;
            }

            protected override int MakeMemberListBinding(MemberListBindingSlim node, ReadOnlyCollection<int> initializers)
            {
                return initializers.Sum() + 1;
            }

            protected override int MakeMemberMemberBinding(MemberMemberBindingSlim node, ReadOnlyCollection<int> bindings)
            {
                return bindings.Sum() + 1;
            }

            protected override int MakeMethodCall(MethodCallExpressionSlim node, int @object, ReadOnlyCollection<int> arguments)
            {
                return @object + arguments.Sum() + 1;
            }

            protected override int MakeNew(NewExpressionSlim node, ReadOnlyCollection<int> arguments)
            {
                return arguments.Sum() + 1;
            }

            protected override int MakeNewArray(NewArrayExpressionSlim node, ReadOnlyCollection<int> expressions)
            {
                return expressions.Sum() + 1;
            }

            protected override int MakeParameter(ParameterExpressionSlim node)
            {
                return 1;
            }

            protected override int MakeTypeBinary(TypeBinaryExpressionSlim node, int expression)
            {
                return expression + 1;
            }

            protected override int MakeUnary(UnaryExpressionSlim node, int operand)
            {
                return operand + 1;
            }

            protected override int MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<int> variables, ReadOnlyCollection<int> expressions)
            {
                return variables.Sum() + expressions.Sum() + 1;
            }

            protected override int MakeCatchBlock(CatchBlockSlim node, int variable, int body, int filter)
            {
                return variable + body + filter + 1;
            }

            protected override int MakeGoto(GotoExpressionSlim node, int target, int value)
            {
                return target + value + 1;
            }

            protected override int MakeLabel(LabelExpressionSlim node, int target, int defaultValue)
            {
                return target + defaultValue + 1;
            }

            protected override int MakeLabelTarget(LabelTargetSlim node)
            {
                return 1;
            }

            protected override int MakeLoop(LoopExpressionSlim node, int body, int @break, int @continue)
            {
                return @break + @continue + body + 1;
            }

            protected override int MakeSwitch(SwitchExpressionSlim node, int switchValue, int defaultBody, ReadOnlyCollection<int> cases)
            {
                return switchValue + cases.Sum() + defaultBody + 1;
            }

            protected override int MakeSwitchCase(SwitchCaseSlim node, int body, ReadOnlyCollection<int> testValues)
            {
                return testValues.Sum() + body + 1;
            }

            protected override int MakeTry(TryExpressionSlim node, int body, int @finally, int fault, ReadOnlyCollection<int> handlers)
            {
                return body + handlers.Sum() + @finally + fault + 1;
            }
        }

        private sealed class MyFoo
        {
            public MyFoo()
            {
                List = new List<int>();
                Bar = new MyBar();
            }

            public MyFoo(int qux)
                : this()
            {
                Qux = qux;
            }

            public MyBar Bar { get; set; }
            public int Qux { get; set; }
            public List<int> List { get; set; }
            public int this[int index] => List[index];

            public int Count() { return List.Count; }
        }

        private sealed class MyBar
        {
            public int Baz { get; set; }
        }

        [TestMethod]
        public void ExpressionSlimVisitorGeneric_Dispatch()
        {
            var exs = new ExpressionSlim[]
            {
                Expression.Add(Expression.Constant(1), Expression.Constant(2)).ToExpressionSlim(),
                Expression.Condition(Expression.Equal(Expression.Constant(1), Expression.Constant(1)), Expression.Constant(2), Expression.Constant(3)).ToExpressionSlim(),
                Expression.Constant(1).ToExpressionSlim(),
                Expression.Default(typeof(int)).ToExpressionSlim(),
                Expression.MakeIndex(Expression.Parameter(typeof(MyFoo)), typeof(MyFoo).GetProperty("Item"), new[] { Expression.Constant(0) }).ToExpressionSlim(),
                Expression.Invoke(Expression.Constant(new Func<int>(() => 42))).ToExpressionSlim(),
                Expression.Lambda<Func<int>>(Expression.Constant(1)).ToExpressionSlim(),
                Expression.Property(Expression.Constant("bar"), "Length").ToExpressionSlim(),
                Expression.Call(Expression.Constant("bar"), typeof(string).GetMethod("ToUpper", Type.EmptyTypes)).ToExpressionSlim(),
                Expression.New(typeof(MyFoo)).ToExpressionSlim(),
                Expression.NewArrayInit(typeof(int)).ToExpressionSlim(),
                Expression.NewArrayBounds(typeof(int), Expression.Constant(1)).ToExpressionSlim(),
                Expression.Negate(Expression.Constant(1)).ToExpressionSlim(),
                Expression.Parameter(typeof(int)).ToExpressionSlim(),
                Expression.ListInit(Expression.New(typeof(List<int>).GetConstructor(Type.EmptyTypes)), Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(42))).ToExpressionSlim(),
                Expression.MemberInit(Expression.New(typeof(MyBar).GetConstructor(Type.EmptyTypes)), Expression.Bind(typeof(MyBar).GetProperty("Baz"), Expression.Constant(42))).ToExpressionSlim(),
                Expression.TypeIs(Expression.Constant("foo"), typeof(string)).ToExpressionSlim(),
            };

            var v = new SimpleVisitor();

            foreach (var ex in exs)
            {
                Assert.AreEqual(ex.NodeType, v.Visit(ex));
            }
        }

        [TestMethod]
        public void ExpressionSlimVisitorGeneric_Null()
        {
            var v = new SimpleVisitor();

            var r = v.Visit(node: null);

            Assert.AreEqual(default, r);
        }

        [TestMethod]
        public void ExpressionSlimVisitorGeneric_Invalid()
        {
            var v = new SimpleVisitor();

            Assert.ThrowsException<NotSupportedException>(() => v.Visit(new FakeNode()));
        }

        private sealed class SimpleVisitor : ExpressionSlimVisitor<ExpressionType>
        {
            protected internal override ExpressionType VisitBinary(BinaryExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitBlock(BlockExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitConditional(ConditionalExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitConstant(ConstantExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitDefault(DefaultExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitExtension(ExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitGoto(GotoExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitIndex(IndexExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitInvocation(InvocationExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitLabel(LabelExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitLambda(LambdaExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitListInit(ListInitExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitLoop(LoopExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitMember(MemberExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitMemberInit(MemberInitExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitMethodCall(MethodCallExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitNew(NewExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitNewArray(NewArrayExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitParameter(ParameterExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitSwitch(SwitchExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitTry(TryExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitTypeBinary(TypeBinaryExpressionSlim node) => node.NodeType;

            protected internal override ExpressionType VisitUnary(UnaryExpressionSlim node) => node.NodeType;
        }

        [TestMethod]
        public void ExpressionSlimVisitorGeneric_ArgumentChecksInVisitMethods()
        {
            new CheckExceptions().Run();
        }

        private sealed class CheckExceptions : ExpressionSlimVisitor<int, int, int, int, int, int, int, int, int, int, int, int>
        {
            public void Run()
            {
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitBinary(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitBlock(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitCatchBlock(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitConditional(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitConstant(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitDefault(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitElementInit(node: null));
                Assert.ThrowsException<NotImplementedException>(() => base.VisitExtension(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitGoto(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitIndex(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitInvocation(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitLabel(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitLabelTarget(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitLambda(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitListInit(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitLoop(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMember(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMemberAssignment(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMemberBinding(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMemberInit(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMemberListBinding(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMemberMemberBinding(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitMethodCall(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitNew(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitNewArray(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitParameter(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitSwitch(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitSwitchCase(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitTry(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitTypeBinary(node: null));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitUnary(node: null));
            }

            protected override int MakeBinary(BinaryExpressionSlim node, int left, int conversion, int right) => throw new NotImplementedException();

            protected override int MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<int> variables, ReadOnlyCollection<int> expressions) => throw new NotImplementedException();

            protected override int MakeCatchBlock(CatchBlockSlim node, int variable, int body, int filter) => throw new NotImplementedException();

            protected override int MakeConditional(ConditionalExpressionSlim node, int test, int ifTrue, int ifFalse) => throw new NotImplementedException();

            protected override int MakeConstant(ConstantExpressionSlim node) => throw new NotImplementedException();

            protected override int MakeDefault(DefaultExpressionSlim node) => throw new NotImplementedException();

            protected override int MakeElementInit(ElementInitSlim node, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int MakeGoto(GotoExpressionSlim node, int target, int value) => throw new NotImplementedException();

            protected override int MakeIndex(IndexExpressionSlim node, int @object, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int MakeInvocation(InvocationExpressionSlim node, int expression, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int MakeLabel(LabelExpressionSlim node, int target, int defaultValue) => throw new NotImplementedException();

            protected override int MakeLabelTarget(LabelTargetSlim node) => throw new NotImplementedException();

            protected override int MakeLambda(LambdaExpressionSlim node, int body, ReadOnlyCollection<int> parameters) => throw new NotImplementedException();

            protected override int MakeListInit(ListInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> initializers) => throw new NotImplementedException();

            protected override int MakeLoop(LoopExpressionSlim node, int body, int breakLabel, int continueLabel) => throw new NotImplementedException();

            protected override int MakeMember(MemberExpressionSlim node, int expression) => throw new NotImplementedException();

            protected override int MakeMemberAssignment(MemberAssignmentSlim node, int expression) => throw new NotImplementedException();

            protected override int MakeMemberInit(MemberInitExpressionSlim node, int newExpression, ReadOnlyCollection<int> bindings) => throw new NotImplementedException();

            protected override int MakeMemberListBinding(MemberListBindingSlim node, ReadOnlyCollection<int> initializers) => throw new NotImplementedException();

            protected override int MakeMemberMemberBinding(MemberMemberBindingSlim node, ReadOnlyCollection<int> bindings) => throw new NotImplementedException();

            protected override int MakeMethodCall(MethodCallExpressionSlim node, int @object, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int MakeNew(NewExpressionSlim node, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int MakeNewArray(NewArrayExpressionSlim node, ReadOnlyCollection<int> expressions) => throw new NotImplementedException();

            protected override int MakeParameter(ParameterExpressionSlim node) => throw new NotImplementedException();

            protected override int MakeSwitch(SwitchExpressionSlim node, int switchValue, int defaultBody, ReadOnlyCollection<int> cases) => throw new NotImplementedException();

            protected override int MakeSwitchCase(SwitchCaseSlim node, int body, ReadOnlyCollection<int> testValues) => throw new NotImplementedException();

            protected override int MakeTry(TryExpressionSlim node, int body, int @finally, int fault, ReadOnlyCollection<int> handlers) => throw new NotImplementedException();

            protected override int MakeTypeBinary(TypeBinaryExpressionSlim node, int expression) => throw new NotImplementedException();

            protected override int MakeUnary(UnaryExpressionSlim node, int operand) => throw new NotImplementedException();
        }

        private sealed class FakeNode : ExpressionSlim
        {
            public FakeNode()
            {
            }

            public override ExpressionType NodeType => (ExpressionType)(-1);

            protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor) => throw new NotImplementedException();

            protected internal override TExpression Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor) => throw new NotImplementedException();
        }

        [TestMethod]
        public void ExpressionSlimVisitorGeneric_VisitAndConvert()
        {
            var c = new ConvertTests();

            c.Test1();
            c.Test2();
        }

        private sealed class ConvertTests : ExpressionSlimVisitor<Base>
        {
            public void Test1()
            {
                var res = VisitAndConvert<Derived>(Expression.Constant(42).ToExpressionSlim());

                Assert.AreEqual(42, res.Value);
            }

            public void Test2()
            {
                var xs = new List<ExpressionSlim>
                {
                    Expression.Constant(1).ToExpressionSlim(),
                    Expression.Constant(2).ToExpressionSlim(),
                }.AsReadOnly();

                var res = VisitAndConvert<ExpressionSlim, Derived>(xs);

                Assert.IsTrue(res.Select(x => (int)x.Value).SequenceEqual(new[] { 1, 2 }));
            }

            protected internal override Base VisitBinary(BinaryExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitBlock(BlockExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitConditional(ConditionalExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitConstant(ConstantExpressionSlim node)
            {
                return new Derived { Value = node.Value.Reduce(node.Type.ToType()) };
            }

            protected internal override Base VisitDefault(DefaultExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitExtension(ExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitGoto(GotoExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitIndex(IndexExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitInvocation(InvocationExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitLabel(LabelExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitLambda(LambdaExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitListInit(ListInitExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitLoop(LoopExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitMember(MemberExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitMemberInit(MemberInitExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitMethodCall(MethodCallExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitNew(NewExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitNewArray(NewArrayExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitParameter(ParameterExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitSwitch(SwitchExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitTry(TryExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitTypeBinary(TypeBinaryExpressionSlim node) => throw new NotImplementedException();

            protected internal override Base VisitUnary(UnaryExpressionSlim node) => throw new NotImplementedException();
        }

        private sealed class Derived : Base
        {
            public object Value { get; set; }
        }

        private class Base
        {
        }
    }
}
