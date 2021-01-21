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
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ClassicCpsRewriterTests
    {
        [TestMethod]
        public void ClassicCpsRewriter_ArgumentChecking()
        {
            var cps = new ClassicCpsRewriter();

            var e1 = Expression.Constant(42);

            var f1 = (Expression<Func<int>>)(() => 42);
            var c1 = (Expression<Action<int>>)(x => Console.WriteLine(x));

            var f2 = (Expression<Action>)(() => Console.WriteLine("bar"));
            var c2 = (Expression<Action>)(() => Console.WriteLine("foo"));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(LambdaExpression)), ex => Assert.AreEqual("expression", ex.ParamName));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int>(default(Expression<Func<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int>(default(Expression<Func<int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int, int>(default(Expression<Func<int, int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression<Action>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Action<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int>(default(Expression<Action<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int, int, int>(default(Expression<Action<int, int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression<Action>), c2), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(f2, default(Expression<Action>)), ex => Assert.AreEqual("continuation", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(default(Expression<Func<int>>), c1), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite<int>(f1, default(Expression<Action<int>>)), ex => Assert.AreEqual("continuation", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(default(Expression), e1), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cps.Rewrite(e1, default(Expression)), ex => Assert.AreEqual("continuation", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics1()
        {
            Expression<Func<int>> f = () => Calculator.Answer();

            var cb = Expression.Parameter(typeof(Action<int>));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action<int>>>(r, cb);

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics2()
        {
            Expression<Func<int>> f = () => Calculator.Add(19, 23);

            var cb = Expression.Parameter(typeof(Action<int>));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action<int>>>(r, cb);

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics3()
        {
            Expression<Func<int>> f = () => Calculator.Add(Calculator.Negate(-19), 23);

            var cb = Expression.Parameter(typeof(Action<int>));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action<int>>>(r, cb);

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_SideEffects()
        {
            var log = new List<int>();

            var s1 = new Func<int>(() =>
            {
                log.Add(1);
                return -7;
            });

            var s2 = new Func<int>(() =>
            {
                log.Add(2);
                return 12;
            });

            var s3 = new Func<int>(() =>
            {
                log.Add(3);
                return 23;
            });

            Expression<Func<int>> f = () => Calculator.Add(Calculator.Negate(s1()), Calculator.Add(s2(), s3()));

            var cb = Expression.Parameter(typeof(Action<int>));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action<int>>>(r, cb);

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);

            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(log));
        }

        private sealed class Calculator
        {
            [UseAsyncMethod]
            public static int Answer()
            {
                throw new NotImplementedException();
            }

            public static void Answer(Action<int> callback)
            {
                callback(42);
            }

            [UseAsyncMethod]
            public static int Negate(int x)
            {
                throw new NotImplementedException();
            }

            public static void Negate(int x, Action<int> callback)
            {
                callback(-x);
            }

            [UseAsyncMethod]
            public static int Add(int x, int y)
            {
                throw new NotImplementedException();
            }

            public static void Add(int x, int y, Action<int> callback)
            {
                callback(x + y);
            }
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics4()
        {
            Expression<Func<int>> f = () => new Bar().Foo(6, 7);

            var cb = Expression.Parameter(typeof(Action<int>));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action<int>>>(r, cb);

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics5()
        {
            var b = new Bar();

            Expression<Action> f = () => b.Qux(42);

            var cb = Expression.Parameter(typeof(Action));

            var r = new ClassicCpsRewriter().Rewrite(f.Body, cb);

            var g = Expression.Lambda<Action<Action>>(r, cb);

            var res = default(int);
            g.Compile()(() => res = b.QuxedValue);

            Assert.AreEqual(42, res);
        }

#pragma warning disable CA1822 // Mark static
        private sealed class Bar
        {
            [UseAsyncMethod]
            public int Foo(int x, int y)
            {
                throw new NotImplementedException();
            }

            public void Foo(int x, int y, Action<int> callback)
            {
                callback(x * y);
            }

            [UseAsyncMethod]
            public void Qux(int x)
            {
                throw new NotImplementedException();
            }

            public int QuxedValue;

            public void Qux(int x, Action callback)
            {
                QuxedValue = x;
                callback();
            }
        }
#pragma warning restore CA1822

        [TestMethod]
        public void ClassicCpsRewriter_Basics6_Func()
        {
            Expression<Func<int>> f = () => Calculator.Add(Calculator.Negate(-19), Calculator.Add(17, 6));

            var r = new ClassicCpsRewriter().Rewrite(f.Body);

            var g = (Expression<Action<Action<int>>>)r;

            var res = default(int);
            g.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics6_Action()
        {
            var b = new Bar();

            Expression<Action> f = () => b.Qux(42);

            var r = new ClassicCpsRewriter().Rewrite(f.Body);

            var g = (Expression<Action<Action>>)r;

            var res = default(int);
            g.Compile()(() => res = b.QuxedValue);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics7_Func()
        {
            Expression<Func<int, int>> f = x => Calculator.Add(Calculator.Negate(-19), Calculator.Add(x, 6));

            var r = new ClassicCpsRewriter().Rewrite((LambdaExpression)f);

            var g = (Expression<Action<int, Action<int>>>)r;

            var res = default(int);
            g.Compile()(17, x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics7_Action()
        {
            var b = new Bar();

            Expression<Action<int>> f = x => b.Qux(x * 7);

            var r = new ClassicCpsRewriter().Rewrite((LambdaExpression)f);

            var g = (Expression<Action<int, Action>>)r;

            var res = default(int);
            g.Compile()(6, () => res = b.QuxedValue);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics8_Func()
        {
            Expression<Func<int>> f = () => Calculator.Add(Calculator.Negate(-19), Calculator.Add(17, 6));

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(int);
            r.Compile()(x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics9_Func()
        {
            Expression<Func<int>> f = () => Calculator.Add(Calculator.Negate(-19), Calculator.Add(17, 6));

            var res = default(int);
            var set = new Action<int>(x => res = x);
            Expression<Action<int>> g = x => set(x);

            var r = new ClassicCpsRewriter().Rewrite(f, g);

            Expression.Lambda<Action>(r).Compile()();

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics10_Func()
        {
            Expression<Func<int, int>> f = x => Calculator.Add(Calculator.Negate(-19), Calculator.Add(x, 6));

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(int);
            r.Compile()(17, x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics11_Func()
        {
            Expression<Func<int, int, int>> f = (x, y) => Calculator.Add(Calculator.Negate(-19), Calculator.Add(x, y));

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(int);
            r.Compile()(17, 6, x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics12_Func()
        {
            Expression<Func<int, int, int, int>> f = (x, y, z) => Calculator.Add(Calculator.Negate(-z), Calculator.Add(x, y));

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(int);
            r.Compile()(17, 6, 19, x => res = x);

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics13()
        {
            Expression<Func<string>> f = () => Qux.Bar<string>("Hello, CPS!");

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(string);
            r.Compile()(s => res = s);

            Assert.AreEqual("Hello, CPS!", res);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class Qux
        {
            [UseAsyncMethod]
            internal static T Bar<T>(T x)
            {
                throw new NotImplementedException();
            }

            internal static void Bar<T>(T x, Action<T> callback)
            {
                callback(x);
            }

            internal static void Bar<T, R>(T x, R y, Action<R> callback)
            {
                throw new InvalidOperationException();
            }

            internal static void Bar(string s, Action<string> callback)
            {
                throw new InvalidOperationException();
            }

            internal static void Foo(string s)
            {
                throw new InvalidOperationException();
            }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        [TestMethod]
        public void ClassicCpsRewriter_Basics14()
        {
            Expression<Func<int>> f = () => Baz(42);

            Assert.ThrowsException<InvalidOperationException>(() => _ = new ClassicCpsRewriter().Rewrite(f));
        }

        [UseAsyncMethod]
        private static int Baz(int x)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics15()
        {
            Expression<Func<string>> f = () => "Bar";

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(string);
            r.Compile()(s => res = s);

            Assert.AreEqual("Bar", res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Basics16()
        {
            Expression<Func<string>> f = () => "Bar".ToLower();

            var r = new ClassicCpsRewriter().Rewrite(f);

            var res = default(string);
            r.Compile()(s => res = s);

            Assert.AreEqual("bar", res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Action1()
        {
            var cps = new ClassicCpsRewriter();

            var f = (Expression<Action>)(() => Actions.Foo());

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(() => res = Actions.Foo0);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Action2()
        {
            var cps = new ClassicCpsRewriter();

            var f = (Expression<Action<int>>)(x => Actions.Foo(x));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(1, () => res = Actions.Foo1);

            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Action3()
        {
            var cps = new ClassicCpsRewriter();

            var f = (Expression<Action<int, int>>)((x, y) => Actions.Foo(x, y));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(1, 2, () => res = Actions.Foo2);

            Assert.AreEqual(3, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Action4()
        {
            var cps = new ClassicCpsRewriter();

            var f = (Expression<Action<int, int, int>>)((x, y, z) => Actions.Foo(x, y, z));

            var g = cps.Rewrite(f);

            var res = default(int);
            g.Compile()(1, 2, 3, () => res = Actions.Foo3);

            Assert.AreEqual(6, res);
        }

        [TestMethod]
        public void ClassicCpsRewriter_Action5()
        {
            var cps = new ClassicCpsRewriter();

            var f = (Expression<Action>)(() => Actions.Foo());

            var res = default(int);
            var set = (Action<int>)(x => res = x);
            var c = (Expression<Action>)(() => set(Actions.Foo0));

            var g = Expression.Lambda<Action>(cps.Rewrite(f, c));

            g.Compile()();

            Assert.AreEqual(0, res);
        }

        private sealed class Actions
        {
            public static int Foo0;

            [UseAsyncMethod]
            public static void Foo()
            {
                throw new NotImplementedException();
            }

            public static void Foo(Action callback)
            {
                Foo0 = 0;
                callback();
            }

            public static int Foo1;

            [UseAsyncMethod]
            public static void Foo(int x)
            {
                throw new NotImplementedException();
            }

            public static void Foo(int x, Action callback)
            {
                Foo1 = x;
                callback();
            }

            public static int Foo2;

            [UseAsyncMethod]
            public static void Foo(int x, int y)
            {
                throw new NotImplementedException();
            }

            public static void Foo(int x, int y, Action callback)
            {
                Foo2 = x + y;
                callback();
            }

            public static int Foo3;

            [UseAsyncMethod]
            public static void Foo(int x, int y, int z)
            {
                throw new NotImplementedException();
            }

            public static void Foo(int x, int y, int z, Action callback)
            {
                Foo3 = x + y + z;
                callback();
            }
        }
    }
}
