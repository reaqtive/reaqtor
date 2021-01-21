// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0050 // Convert to tuple. (This facility deals with anonymous types.)

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class AnonymousTypeTupletizerTests
    {
        [TestMethod]
        public void AnonymousTypeTupletizer_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => AnonymousTypeTupletizer.Tupletize(expression: null, Expression.Constant(1)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => AnonymousTypeTupletizer.Tupletize(Expression.Constant(1), unitValue: null), ex => Assert.AreEqual("unitValue", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => AnonymousTypeTupletizer.Tupletize(expression: null, Expression.Constant(1), excludeVisibleTypes: true), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => AnonymousTypeTupletizer.Tupletize(Expression.Constant(1), unitValue: null, excludeVisibleTypes: true), ex => Assert.AreEqual("unitValue", ex.ParamName));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_NoChange()
        {
            foreach (var e in new Expression[]
            {
                Expression.Constant(1),
                (Expression<Func<int>>)(() => DateTime.Now.Year),
                (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
                (Expression<Func<IEnumerable<int>, IEnumerable<string>>>)(xs => from x in xs where x % 2 == 0 select x.ToString())
            })
            {
                var t = AnonymousTypeTupletizer.Tupletize(e, Expression.Constant(value: null));
                Assert.AreSame(e, t);
            }
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple0()
        {
            Expression<Func<object>> f = () => new { };
            var a = f.Body;

            var b = Expression.Constant(value: null);
            var t = AnonymousTypeTupletizer.Tupletize(a, b);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple1()
        {
            Expression<Func<object>> f = () => new { a = 42 };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int>(42);
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple1_Accessor()
        {
            Expression<Func<int>> f = () => new { a = 42 }.a;
            var a = f.Body;

            Expression<Func<int>> g = () => new Tuple<int>(42).Item1;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple2()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar" };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string>(42, "bar");
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple2_Equals()
        {
            Expression<Func<bool>> f = () => new { a = 42, b = "bar" }.Equals(new { a = 42, b = "bar" });
            var a = f.Body;

            Expression<Func<bool>> g = () => new Tuple<int, string>(42, "bar").Equals(new Tuple<int, string>(42, "bar"));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));

            Assert.IsTrue(a.Evaluate<bool>());
            Assert.IsTrue(b.Evaluate<bool>());
            Assert.IsTrue(t.Evaluate<bool>());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple2_GetHashCode()
        {
            Expression<Func<bool>> f = () => new { a = 42, b = "bar" }.GetHashCode() == new { a = 42, b = "bar" }.GetHashCode();
            var a = f.Body;

            Expression<Func<bool>> g = () => new Tuple<int, string>(42, "bar").GetHashCode() == new Tuple<int, string>(42, "bar").GetHashCode();
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));

            Assert.IsTrue(a.Evaluate<bool>());
            Assert.IsTrue(b.Evaluate<bool>());
            Assert.IsTrue(t.Evaluate<bool>());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple2_ToString()
        {
            Expression<Func<string>> f = () => new { a = 42, b = "bar" }.ToString();
            var a = f.Body;

            Expression<Func<string>> g = () => new Tuple<int, string>(42, "bar").ToString();
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));

            Assert.AreEqual("{ a = 42, b = bar }", a.Evaluate<string>());
            Assert.AreEqual("(42, bar)", b.Evaluate<string>());
            Assert.AreEqual("(42, bar)", t.Evaluate<string>());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple7()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3) };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple7_Accessor()
        {
            Expression<Func<bool>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3) }.d;
            var a = f.Body;

            Expression<Func<bool>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3)).Item4;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple8()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid() };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid>(Guid.NewGuid()));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple8_Accessor1()
        {
            Expression<Func<TimeSpan>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid() }.g;
            var a = f.Body;

            Expression<Func<TimeSpan>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid>(Guid.NewGuid())).Item7;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple8_Accessor2()
        {
            Expression<Func<Guid>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid() }.h;
            var a = f.Body;

            Expression<Func<Guid>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid>(Guid.NewGuid())).Rest.Item1;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple10()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float>(Guid.NewGuid(), 128, 12.34f));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple13()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo" };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo"));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple14()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo", n = (object)null };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string, object>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string, object>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo", null));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple15()
        {
            Expression<Func<object>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo", n = (object)null, o = 'a' };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo", null, new Tuple<char>('a')));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple15_Accessor1()
        {
            Expression<Func<decimal>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo", n = (object)null, o = 'a' }.e;
            var a = f.Body;

            Expression<Func<decimal>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo", null, new Tuple<char>('a'))).Item5;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple15_Accessor2()
        {
            Expression<Func<byte>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo", n = (object)null, o = 'a' }.i;
            var a = f.Body;

            Expression<Func<byte>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo", null, new Tuple<char>('a'))).Rest.Item2;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Simple15_Accessor3()
        {
            Expression<Func<char>> f = () => new { a = 42, b = "bar", c = 3.14, d = false, e = 49.95m, f = DateTime.Now, g = new TimeSpan(1, 2, 3), h = Guid.NewGuid(), i = (byte)128, j = 12.34f, k = 24, l = true, m = "foo", n = (object)null, o = 'a' }.o;
            var a = f.Body;

            Expression<Func<char>> g = () => new Tuple<int, string, double, bool, decimal, DateTime, TimeSpan, Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>>(42, "bar", 3.14, false, 49.95m, DateTime.Now, new TimeSpan(1, 2, 3), new Tuple<Guid, byte, float, int, bool, string, object, Tuple<char>>(Guid.NewGuid(), 128, 12.34f, 24, true, "foo", null, new Tuple<char>('a'))).Rest.Rest.Item1;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_TransparentIdentifiers()
        {
            Expression<Func<IEnumerable<int>, IEnumerable<string>, IEnumerable<bool>>> f = (xs, ys) => from x in xs
                                                                                                       from y in ys
                                                                                                       let l = y.Length
                                                                                                       let k = x * l
                                                                                                       where l > 0
                                                                                                       where k != 1
                                                                                                       let b = x + l < 42
                                                                                                       select !b;

            //Expression<Func<IEnumerable<int>, IEnumerable<string>, IEnumerable<bool>>> g = (xs, ys) => xs
            //                                                                                           .SelectMany(x => ys, (x, y) => new { x = x, y = y })
            //                                                                                           .Select(t => new { t, l = t.y.Length })
            //                                                                                           .Select(t => new { t, k = t.t.x * t.l })
            //                                                                                           .Where(t => t.t.l > 0)
            //                                                                                           .Where(t => t.k != 1)
            //                                                                                           .Select(t => new { t, b = t.t.t.x + t.t.l < 42 })
            //                                                                                           .Select(t => !t.b);

            Expression<Func<IEnumerable<int>, IEnumerable<string>, IEnumerable<bool>>> g = (xs, ys) => xs
                                                                                                       .SelectMany(x => ys, (x, y) => new Tuple<int, string>(x, y))
                                                                                                       .Select(t => new Tuple<Tuple<int, string>, int>(t, t.Item2.Length))
                                                                                                       .Select(t => new Tuple<Tuple<Tuple<int, string>, int>, int>(t, t.Item1.Item1 * t.Item2))
                                                                                                       .Where(t => t.Item1.Item2 > 0)
                                                                                                       .Where(t => t.Item2 != 1)
                                                                                                       .Select(t => new Tuple<Tuple<Tuple<Tuple<int, string>, int>, int>, bool>(t, t.Item1.Item1.Item1 + t.Item1.Item2 < 42))
                                                                                                       .Select(t => !t.Item2);

            var te = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(g, te));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_RecursiveType()
        {
            var rtc = new RuntimeCompiler();

            var atb1 = rtc.GetNewAnonymousTypeBuilder();
            var atb2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineAnonymousType(atb1, new[]
            {
                new KeyValuePair<string, Type>("Qux", typeof(int)),
                new KeyValuePair<string, Type>("Bar", atb2),
            }, Array.Empty<string>());

            rtc.DefineAnonymousType(atb2, new[]
            {
                new KeyValuePair<string, Type>("Baz", typeof(int)),
                new KeyValuePair<string, Type>("Foo", atb1),
            }, Array.Empty<string>());

            var foo = atb1.CreateType();
            var bar = atb2.CreateType();

            var e = Expression.New(foo.GetConstructors().Single(), Expression.Constant(1), Expression.Constant(value: null, bar));

            Assert.ThrowsException<NotSupportedException>(() => AnonymousTypeTupletizer.Tupletize(e, Expression.Constant(value: null)));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Nested1()
        {
            Expression<Func<object>> f = () => new { a = 42, b = new { c = "bar", d = false }, e = 12.34, f = new { g = 'a' } };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<int, Tuple<string, bool>, double, Tuple<char>>(42, new Tuple<string, bool>("bar", false), 12.34, new Tuple<char>('a'));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Nested2()
        {
            Expression<Func<object>> f = () => new { a = new { b = new { a = new { b = 42 } } } };
            var a = f.Body;

            Expression<Func<object>> g = () => new Tuple<Tuple<Tuple<Tuple<int>>>>(new Tuple<Tuple<Tuple<int>>>(new Tuple<Tuple<int>>(new Tuple<int>(42))));
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Nested3()
        {
            Expression<Func<int>> f = () => new { a = new { b = new { a = new { b = 42 } } } }.a.b.a.b;
            var a = f.Body;

            Expression<Func<int>> g = () => new Tuple<Tuple<Tuple<Tuple<int>>>>(new Tuple<Tuple<Tuple<int>>>(new Tuple<Tuple<int>>(new Tuple<int>(42)))).Item1.Item1.Item1.Item1;
            var b = g.Body;

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Null()
        {
            var x = new { a = 42 };

            var a = Expression.Constant(value: null, x.GetType());
            var b = Expression.Constant(value: null, typeof(Tuple<int>));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple0()
        {
            var x = new { };

            var a = Expression.Constant(x);
            var b = Expression.Constant("bar");

            var t = AnonymousTypeTupletizer.Tupletize(a, b);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple1()
        {
            var x = new { a = 42 };

            var a = Expression.Constant(x);
            var b = Expression.Constant(new Tuple<int>(42));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple2()
        {
            var x = new { a = 42, b = "bar" };

            var a = Expression.Constant(x);
            var b = Expression.Constant(new Tuple<int, string>(42, "bar"));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple7()
        {
            var x = new { a = 42, b = "bar", c = 12.34, d = false, e = 'a', f = 49.95m, g = 0.1f };

            var a = Expression.Constant(x);
            var b = Expression.Constant(new Tuple<int, string, double, bool, char, decimal, float>(42, "bar", 12.34, false, 'a', 49.95m, 0.1f));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple8()
        {
            var x = new { a = 42, b = "bar", c = 12.34, d = false, e = 'a', f = 49.95m, g = 0.1f, h = (byte)128 };

            var a = Expression.Constant(x);
            var b = Expression.Constant(new Tuple<int, string, double, bool, char, decimal, float, Tuple<byte>>(42, "bar", 12.34, false, 'a', 49.95m, 0.1f, new Tuple<byte>(128)));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Constant_Simple9()
        {
            var x = new { a = 42, b = "bar", c = 12.34, d = false, e = 'a', f = 49.95m, g = 0.1f, h = (byte)128, i = new TimeSpan(1, 2, 3) };

            var a = Expression.Constant(x);
            var b = Expression.Constant(new Tuple<int, string, double, bool, char, decimal, float, Tuple<byte, TimeSpan>>(42, "bar", 12.34, false, 'a', 49.95m, 0.1f, new Tuple<byte, TimeSpan>(128, new TimeSpan(1, 2, 3))));

            var t = AnonymousTypeTupletizer.Tupletize(a, Expression.Constant(value: null));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b, t));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc1()
        {
            var f = Expression.Property(Expression.Constant(new { x = 42, y = "Hello", z = 12.34 }), "x");
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("(42, Hello, 12.34).Item1", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc2()
        {
            var f = Expression.Property(Expression.Constant(new { x = 42, y = "Hello", z = 12.34 }), "z");
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("(42, Hello, 12.34).Item3", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc3()
        {
            var f = Expression.Property(Expression.Constant(new { x = 42, y = "Hello", z = 12.34 }), "z");
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("(42, Hello, 12.34).Item3", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc4()
        {
            var f = Expression.Property(Expression.Property(Expression.Constant(new { a = new { x = 42, y = "Hello" }, c = 12.34 }), "a"), "x");
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("((42, Hello), 12.34).Item1.Item1", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc5()
        {
            var f = Expression.Property(Expression.Property(Expression.Constant(new { a = new { x = 42, y = "Hello" }, c = new { z = 12.34 } }), "c"), "z");
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("((42, Hello), (12.34)).Item2.Item1", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Adhoc6()
        {
            var f = (from x in new[] { 1, 2 }.AsQueryable() let y = x + 1 select x * y).Expression;
            f = CompilerGeneratedNameEliminator.Prettify(f);
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null));
            Assert.AreEqual("System.Int32[].Select(x => new Tuple`2(x, (x + 1))).Select(t => (t.Item1 * t.Item2))", t.ToString());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Unit1()
        {
            var f = (Expression<Func<bool>>)(() => new { }.Equals(new { }));
            var t = (Expression<Func<bool>>)AnonymousTypeTupletizer.Tupletize(f, Expression.New(typeof(Unit)));

            var u = (Expression<Func<bool>>)(() => new Unit().Equals(new Unit()));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(u, t));

            Assert.IsTrue(f.Compile()());
            Assert.IsTrue(t.Compile()());
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_Unit2()
        {
            var f = (Expression<Func<bool>>)(() => new { }.Equals(new { }));
            AssertEx.ThrowsException<ArgumentException>(() => AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(false)), ex => Assert.AreEqual("unitValue", ex.ParamName));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_ExcludeVisibleTypes()
        {
            var f = (from x in new[] { 1, 2, 3 }.AsQueryable() let y = x * 2 select new { x, y, res = new { sum = x + y, product = x * y } }).Expression;
            var t = AnonymousTypeTupletizer.Tupletize(f, Expression.Constant(value: null), excludeVisibleTypes: true);

            Assert.AreEqual(f.Type, t.Type);

            var e = (MethodCallExpression)t;
            var s = e.Arguments[1].Unquote();
            var p = s.Parameters[0];

            Assert.IsFalse(p.Type.IsAnonymousType());
            Assert.IsTrue(p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(Tuple<,>));
        }

        [TestMethod]
        public void AnonymousTypeTupletizer_NewExpression_ValueType_DefaultConstructor()
        {
            var e = Expression.Lambda(Expression.New(typeof(int)));
            var t = AnonymousTypeTupletizer.Tupletize(e, Expression.Constant(value: null));
            Assert.AreSame(e, t);
        }

        private sealed class Unit
        {
            public override bool Equals(object obj) => obj is Unit;

            public override int GetHashCode() => 0;
        }
    }
}
