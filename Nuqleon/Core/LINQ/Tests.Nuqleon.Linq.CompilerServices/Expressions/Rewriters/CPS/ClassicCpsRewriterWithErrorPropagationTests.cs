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
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ClassicCpsRewriterWithErrorPropagationTests
    {
        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_ArgumentChecking()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var e1 = Expression.Constant(42);

            var f1 = (Expression<Func<int>>)(() => 42);
            var c1 = (Expression<Action<int>>)(x => Console.WriteLine(x));
            var d1 = (Expression<Action<Exception>>)(x => Console.WriteLine(x.Message));

            var f2 = (Expression<Action>)(() => Console.WriteLine("bar"));
            var c2 = (Expression<Action>)(() => Console.WriteLine("foo"));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(LambdaExpression)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int>(default(Expression<Func<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int>(default(Expression<Func<int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int, int>(default(Expression<Func<int, int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression<Action>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Action<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int>(default(Expression<Action<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int>(default(Expression<Action<int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression<Action>), c2, d1), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(f2, default(Expression<Action>), d1), ex => Assert.AreEqual("success", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(f2, c2, default(Expression<Action<Exception>>)), ex => Assert.AreEqual("failure", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Func<int>>), c1, d1), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(f1, default(Expression<Action<int>>), d1), ex => Assert.AreEqual("success", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(f1, c1, default(Expression<Action<Exception>>)), ex => Assert.AreEqual("failure", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression), e1, e1), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(e1, default(Expression), e1), ex => Assert.AreEqual("success", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(e1, e1, default(Expression)), ex => Assert.AreEqual("failure", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Return_Func1()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var f = (Expression<Func<int>>)(() => Calculator.Return(42));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Return_Func2()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var f = (Expression<Func<int>>)(() => Calculator.Return(42));

            var g = (Expression<Action<Action<int>, Action<Exception>>>)cps.Rewrite((LambdaExpression)f);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Return_Func3()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var f = (Expression<Func<int>>)(() => Calculator.Return(42));

            var g = (Expression<Action<Action<int>, Action<Exception>>>)cps.Rewrite(f.Body);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Return_Func4()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var res = default(int);
            var set = new Action<int>(x => res = x);
            var onNext = (Expression<Action<int>>)(x => set(x));
            var onError = (Expression<Action<Exception>>)(ex => Assert.Fail());

            var f = (Expression<Func<int>>)(() => Calculator.Return(42));

            var g = Expression.Lambda<Action>(cps.Rewrite(f.Body, onNext, onError));

            g.Compile()();

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Return_Func5()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var res = default(int);
            var set = new Action<int>(x => res = x);
            var onNext = (Expression<Action<int>>)(x => set(x));
            var onError = (Expression<Action<Exception>>)(ex => Assert.Fail());

            var f = (Expression<Func<int>>)(() => Calculator.Return(42));

            var g = Expression.Lambda<Action>(cps.Rewrite(f, onNext, onError));

            g.Compile()();

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Throw_Action1()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var ex = new Exception();
            var f = (Expression<Action>)(() => Calculator.Throw(ex));

            var g = cps.Rewrite(f);

            var err = default(Exception);
            g.Compile()(() => Assert.Fail(), e => err = e);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Throw_Action2()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var ex = new Exception();
            var f = (Expression<Action>)(() => Calculator.Throw(ex));

            var g = (Expression<Action<Action, Action<Exception>>>)cps.Rewrite((LambdaExpression)f);

            var err = default(Exception);
            g.Compile()(() => Assert.Fail(), e => err = e);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Throw_Action3()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var ex = new Exception();
            var f = (Expression<Action>)(() => Calculator.Throw(ex));

            var g = (Expression<Action<Action, Action<Exception>>>)cps.Rewrite(f.Body);

            var err = default(Exception);
            g.Compile()(() => Assert.Fail(), e => err = e);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Throw_Action4()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var err = default(Exception);
            var set = new Action<Exception>(e => err = e);
            var onNext = (Expression<Action>)(() => Assert.Fail());
            var onError = (Expression<Action<Exception>>)(e => set(e));

            var ex = new Exception();
            var f = (Expression<Action>)(() => Calculator.Throw(ex));

            var g = Expression.Lambda<Action>(cps.Rewrite(f.Body, onNext, onError));

            g.Compile()();

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Throw_Action5()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var err = default(Exception);
            var set = new Action<Exception>(e => err = e);
            var onNext = (Expression<Action>)(() => Assert.Fail());
            var onError = (Expression<Action<Exception>>)(e => set(e));

            var ex = new Exception();
            var f = (Expression<Action>)(() => Calculator.Throw(ex));

            var g = Expression.Lambda<Action>(cps.Rewrite(f.Body, onNext, onError));

            g.Compile()();

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics1()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var f = (Expression<Func<int>>)(() => Calculator.Add(Calculator.Add(1, Calculator.Div(4, 2)), 3));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(6, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics2()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var done = false;
            var complete = new Action(() => done = true);

            var bar = new Bar();

            var f = (Expression<Action>)(() => bar.Foo());
            var s = (Expression<Action>)(() => complete());
            var e = (Expression<Action<Exception>>)(err => Assert.Fail());

            var g = Expression.Lambda<Action>(cps.Rewrite(f, s, e));

            g.Compile()();

            Assert.IsTrue(done);
            Assert.IsTrue(bar.HasFood);
        }


        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics3()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var bar = new Bar();

            var f = (Expression<Func<int, int>>)(x => bar.I(x));

            var g = (Expression<Action<int, Action<int>, Action<Exception>>>)cps.Rewrite((LambdaExpression)f);

            var res = default(int);
            g.Compile()(42, x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics4()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var bar = new Bar();

            var f = (Expression<Action<int>>)(x => bar.J(x));

            var g = (Expression<Action<int, Action, Action<Exception>>>)cps.Rewrite((LambdaExpression)f);

            var h = g.Compile();

            var done = default(bool);
            var err = default(Exception);
            h(1, () => done = true, ex => Assert.Fail());
            h(0, () => Assert.Fail(), ex => err = ex);

            Assert.IsTrue(done);
            Assert.IsTrue(err is DivideByZeroException);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics5()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var log = new List<int>();

            var getX = new Func<bool>(() =>
            {
                log.Add(1);
                return true;
            });

            var getY = new Func<string>(() =>
            {
                log.Add(2);
                return "Hello";
            });

            var getZ = new Func<int>(() =>
            {
                log.Add(3);
                return 42;
            });

            var bar = new Bar();

            var f = (Expression<Func<long>>)(() => bar.K(getX(), getY(), getZ()));

            var g = cps.Rewrite(f);

            var res = default(long);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(1 + 42 / 5, res);

            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(log));
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics6()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var log = new List<int>();

            var getX = new Func<bool>(() =>
            {
                log.Add(1);
                return true;
            });

            var getY = new Func<string>(() =>
            {
                log.Add(2);
                return "Hello";
            });

            var getZ = new Func<int>(() =>
            {
                log.Add(3);
                return 42;
            });

            var f = (Expression<Func<int>>)(() => Calculator.Add(getX() ? 1 : 0, Calculator.Div(getZ(), getY().Length)));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(1 + 42 / 5, res);

            Assert.IsTrue(new[] { 1, 3, 2 }.SequenceEqual(log));
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Basics7()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var log = new List<int>();

            var getX = new Func<bool>(() =>
            {
                log.Add(1);
                return true;
            });

            var getY = new Func<string>(() =>
            {
                log.Add(2);
                return "Hello";
            });

            var ex = new Exception();

            var getZ = new Func<int>(() =>
            {
                log.Add(3);
                throw ex;
            });

            var f = (Expression<Func<int>>)(() => Calculator.Add(getX() ? 1 : 0, Calculator.Div(getZ(), getY().Length)));

            var g = cps.Rewrite(f);

            var err = default(Exception);
            g.Compile()(x => Assert.Fail(), e => err = e);

            Assert.AreSame(ex, err);

            Assert.IsTrue(new[] { 1, 3 }.SequenceEqual(log));
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_ThrowExpression()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var ex = new Exception();
            var f = Expression.Throw(Expression.Constant(ex), typeof(int));

            var g = (Expression<Action<Action<int>, Action<Exception>>>)cps.Rewrite(f);

            var err = default(Exception);
            g.Compile()(x => Assert.Fail(), e => err = e);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Func1()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Func<int>>)(() => foo.Bar());

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Func2()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Func<int, int>>)(x => foo.Bar(x));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(42, x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Func3()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Func<int, int, int>>)((x, y) => foo.Bar(x, y));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(19, 23, x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Func4()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Func<int, int, int, int>>)((x, y, z) => foo.Bar(x, y, z));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(19, 16, 7, x => res = x, ex => Assert.Fail());

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Action1()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Action>)(() => foo.Qux());

            var g = cps.Rewrite(f);

            var done = default(bool);
            g.Compile()(() => done = true, ex => Assert.Fail());

            Assert.IsTrue(done);
            Assert.AreEqual(42, foo.Qux0);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Action2()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Action<int>>)(x => foo.Qux(x));

            var g = cps.Rewrite(f);

            var done = default(bool);
            g.Compile()(42, () => done = true, ex => Assert.Fail());

            Assert.IsTrue(done);
            Assert.AreEqual(42, foo.Qux1);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Action3()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Action<int, int>>)((x, y) => foo.Qux(x, y));

            var g = cps.Rewrite(f);

            var done = default(bool);
            g.Compile()(19, 23, () => done = true, ex => Assert.Fail());

            Assert.IsTrue(done);
            Assert.AreEqual(42, foo.Qux2);
        }

        [TestMethod]
        public void ClassicCpsRewriterWithErrorPropagation_Action4()
        {
            var cps = new ClassicCpsRewriterWithErrorPropagation();

            var foo = new Foo();

            var f = (Expression<Action<int, int, int>>)((x, y, z) => foo.Qux(x, y, z));

            var g = cps.Rewrite(f);

            var done = default(bool);
            g.Compile()(19, 16, 7, () => done = true, ex => Assert.Fail());

            Assert.IsTrue(done);
            Assert.AreEqual(42, foo.Qux3);
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark as static
        private sealed class Calculator
        {
            [UseAsyncMethod]
            public static int Return(int value)
            {
                throw new NotImplementedException();
            }

            public static void Return(int value, Action<int> @return, Action<Exception> @throw)
            {
                @return(value);
            }

            [UseAsyncMethod]
            public static void Throw(Exception ex)
            {
                throw new NotImplementedException();
            }

            public static void Throw(Exception ex, Action @return, Action<Exception> @throw)
            {
                @throw(ex);
            }

            [UseAsyncMethod]
            public static int Add(int x, int y)
            {
                throw new NotImplementedException();
            }

            public static void Add(int x, int y, Action<int> @return, Action<Exception> @throw)
            {
                @return(x + y);
            }

            [UseAsyncMethod]
            public static int Div(int x, int y)
            {
                throw new NotImplementedException();
            }

            public static void Div(int x, int y, Action<int> @return, Action<Exception> @throw)
            {
                int res;
                try
                {
                    res = x / y;
                }
                catch (Exception ex)
                {
                    @throw(ex);
                    return;
                }

                @return(res);
            }
        }

        private sealed class Bar
        {
            public bool HasFood;

            [UseAsyncMethod]
            public void Foo()
            {
                throw new NotImplementedException();
            }

            public void Foo(Action @return, Action<Exception> @throw)
            {
                HasFood = true;
                @return();
            }

            [UseAsyncMethod]
            public int I(int x)
            {
                throw new NotImplementedException();
            }

            public void I(int x, Action<int> @return, Action<Exception> @throw)
            {
                @return(x);
            }

            [UseAsyncMethod]
            public void J(int x)
            {
                throw new NotImplementedException();
            }

            public void J(int x, Action @return, Action<Exception> @throw)
            {
                if (x != 0)
                    @return();
                else
                    @throw(new DivideByZeroException());
            }

            [UseAsyncMethod]
            public long K(bool x, string y, int z)
            {
                throw new NotImplementedException();
            }

            public void K(bool x, string y, int z, Action<long> @return, Action<Exception> @throw)
            {
                @return((x ? 1 : 0) + z / y.Length);
            }
        }

        private sealed class Foo
        {
            [UseAsyncMethod]
            public int Bar()
            {
                throw new NotImplementedException();
            }

            public void Bar(Action<int> success, Action<Exception> failure)
            {
                success(42);
            }

            [UseAsyncMethod]
            public int Bar(int x)
            {
                throw new NotImplementedException();
            }

            public void Bar(int x, Action<int> success, Action<Exception> failure)
            {
                success(x);
            }

            [UseAsyncMethod]
            public int Bar(int x, int y)
            {
                throw new NotImplementedException();
            }

            public void Bar(int x, int y, Action<int> success, Action<Exception> failure)
            {
                success(x + y);
            }

            [UseAsyncMethod]
            public int Bar(int x, int y, int z)
            {
                throw new NotImplementedException();
            }

            public void Bar(int x, int y, int z, Action<int> success, Action<Exception> failure)
            {
                success(x + y + z);
            }

            public int Qux0;

            [UseAsyncMethod]
            public void Qux()
            {
                throw new NotImplementedException();
            }

            public void Qux(Action success, Action<Exception> failure)
            {
                Qux0 = 42;
                success();
            }

            public int Qux1;

            [UseAsyncMethod]
            public void Qux(int x)
            {
                throw new NotImplementedException();
            }

            public void Qux(int x, Action success, Action<Exception> failure)
            {
                Qux1 = 42;
                success();
            }

            public int Qux2;

            [UseAsyncMethod]
            public void Qux(int x, int y)
            {
                throw new NotImplementedException();
            }

            public void Qux(int x, int y, Action success, Action<Exception> failure)
            {
                Qux2 = x + y;
                success();
            }

            public int Qux3;

            [UseAsyncMethod]
            public void Qux(int x, int y, int z)
            {
                throw new NotImplementedException();
            }

            public void Qux(int x, int y, int z, Action success, Action<Exception> failure)
            {
                Qux3 = x + y + z;
                success();
            }
        }
#pragma warning restore CA1822 // Mark as static
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
