// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class ExpressionEntityTypeAnonymizerTests
    {
        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_ArgumentChecking()
        {
            var eta = new ExpressionEntityTypeAnonymizer();

            AssertEx.ThrowsException<ArgumentNullException>(() => eta.Apply(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Simple()
        {
            var netrc = new NoEntityTypesRemainingChecker();

            foreach (var e in new Expression[]
            {
                (Expression<Func<Qux, int>>)(q => q.Baz),
                (Expression<Func<Qux>>)(() => new Qux()),
                (Expression<Func<Qux>>)(() => new Qux() { Baz = 1 }),
                (Expression<Func<Qux>>)(() => new Qux(1) { Foo = "bar" }),
                (Expression<Func<Bar>>)(() => new Bar { Foos = new Foo[] { new Foo { Qux = new Tuple<Func<List<Qux>, long>, bool>(t => t.Count, false) } } }),
                (Expression<Func<IQueryable<Bar>, IQueryable<long>>>)(xs => from x in xs from y in x.Foos where y.Qux.Item2 select y.Qux.Item1(new List<Qux> { new Qux(1) { Foo = "bar" } })),
                (Expression<Func<IQueryable<Bar>, IQueryable<Foo>>>)(xs => from x in xs from y in x.Foos select y),
                (Expression<Func<Qux>>)(() => Activator.CreateInstance<Qux>()),
                (Expression<Action>)(() => FooIt<Qux>()),
                (Expression<Func<Qux[]>>)(() => new Qux[1]),
                (Expression<Func<Qux[]>>)(() => new Qux[] { new Qux(1), new Qux { Baz = 1 } }),
                (Expression<Func<Tuple<Qux, int>>>)(() => new Tuple<Qux, int>(new Qux(1), 2)),
                (Expression<Func<Holder<Qux>>>)(() => new Holder<Qux> { Value = new Qux(42) }),
            })
            {
                var eta = new ExpressionEntityTypeAnonymizer();

                var res = eta.Apply(e);

                netrc.Visit(res); // TODO: semantic equivalence checks
            }
        }

#pragma warning disable CA1822 // Mark static. (Used in expression tree)
        private void FooIt<T>()
        {
        }
#pragma warning restore CA1822

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Errors()
        {
            foreach (var e in new Expression[]
            {
                (Expression<Func<Qux>>)(() => new Qux(1) { Baz = 1 }),
                (Expression<Func<MissingMapping>>)(() => new MissingMapping(1)),
                (Expression<Func<NonMatchingMapping>>)(() => new NonMatchingMapping(1)),
                (Expression<Func<DuplicateMapping>>)(() => new DuplicateMapping(1, 2)),
            })
            {
                var eta = new ExpressionEntityTypeAnonymizer();

                Assert.ThrowsException<InvalidOperationException>(() => eta.Apply(e));
            }
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_Simple()
        {
            var recType = RuntimeCompiler.CreateRecordType(new[] {
                new KeyValuePair<string, Type>("a", typeof(int)),
                new KeyValuePair<string, Type>("b", typeof(Qux))
            }, valueEquality: true);

            var rec = Activator.CreateInstance(recType);
            recType.GetProperty("a").SetValue(rec, 42);
            recType.GetProperty("b").SetValue(rec, new Qux(43));

            foreach (var o in new object[]
            {
                new Qux(),
                new Qux(1),
                new Qux(1) { Foo = "bar" },
                new Qux[1],
                new Qux[] { new Qux(1), new Qux(2) },
                new List<Qux> { new Qux(1), new Qux(2) },
                new Func<int, int>(x => x),
                42,
                "bar",
                (Expression<Func<Qux, Qux>>)(x => x),
                (Expression<Func<Qux[]>>)(() => new Qux[] { new Qux(123) }),
                new Tuple<Qux, int>(new Qux(), 42),
                new { a = 1, b = new Qux(42), c = "bar" },
                rec,
            })
            {
                var eta = new ExpressionEntityTypeAnonymizer();

                var res = eta.Apply(Expression.Constant(o));

                Assert.IsNotNull(res); // TODO: semantic equivalence checks
            }
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_Null()
        {
            var eta = new ExpressionEntityTypeAnonymizer();

            var res = eta.Apply(Expression.Constant(value: null, typeof(Qux)));

            Assert.IsNotNull(res); // TODO: semantic equivalence checks
            Assert.IsTrue(res is ConstantExpression);
            Assert.IsNull(((ConstantExpression)res).Value);
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_ManOrBoy1()
        {
            var c = Expression.Constant(new A { B = new B { Cs = new C[] { new C { D = 42 } } } });

            var eta = new ExpressionEntityTypeAnonymizer();

            var res = eta.Apply(c);

            Assert.AreNotEqual(res.Type, c.Type);

            dynamic d = ((ConstantExpression)res).Value;

            int x = d.b.cs[0].d;

            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_ManOrBoy2()
        {
            Expression<Func<IEnumerable<A>, IEnumerable<int>>> f = xs => from x in xs
                                                                         let b = x.B
                                                                         from c in b.Cs
                                                                         let d = c.D
                                                                         where d > 0
                                                                         let e = new A { B = new B { Cs = new C[] { c, new C { D = d + 1 } } } }
                                                                         select 7 * e.B.Cs.Sum(y => y.D);

            var g = f.Compile();

            var res = g(new[] {
                new A { B = new B { Cs = new C[] { new C { D = 23 } } } },
                new A { B = new B { Cs = new C[] { new C { D = -1 } } } },
                new A { B = new B { Cs = new C[] { new C { D = 87 } } } },
            });

            var eta = new ExpressionEntityTypeAnonymizer();

            var h = (LambdaExpression)eta.Apply(f);

            var anonA = h.Parameters[0].Type.GetGenericArguments()[0];
            var anonB = anonA.GetProperty("b").PropertyType;
            var anonC = anonB.GetProperty("cs").PropertyType.GetElementType();

            var c23 = Activator.CreateInstance(anonC, 23);
            var cm1 = Activator.CreateInstance(anonC, -1);
            var c87 = Activator.CreateInstance(anonC, 87);

            var cs23 = (IList)Array.CreateInstance(anonC, 1);
            cs23[0] = c23;
            var csm1 = (IList)Array.CreateInstance(anonC, 1);
            csm1[0] = cm1;
            var cs87 = (IList)Array.CreateInstance(anonC, 1);
            cs87[0] = c87;

            var b23 = Activator.CreateInstance(anonB, cs23);
            var bm1 = Activator.CreateInstance(anonB, csm1);
            var b87 = Activator.CreateInstance(anonB, cs87);

            var a23 = Activator.CreateInstance(anonA, b23);
            var am1 = Activator.CreateInstance(anonA, bm1);
            var a87 = Activator.CreateInstance(anonA, b87);

            var inp = (IList)Array.CreateInstance(anonA, 3);
            inp[0] = a23;
            inp[1] = am1;
            inp[2] = a87;

            var output = (IEnumerable<int>)h.Compile().DynamicInvoke(new object[] { inp });

            Assert.IsTrue(res.SequenceEqual(output));
        }

        private class A
        {
            [Mapping("b")]
            public B B { get; set; }
        }

        private class B
        {
            [Mapping("cs")]
            public C[] Cs { get; set; }
        }

        private class C
        {
            [Mapping("d")]
            public int D { get; set; }
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_ReferenceEquality()
        {
            var q = new Qux { Baz = 42 };
            var t = new Tuple<Qux, Qux>(q, q);

            var eta = new ExpressionEntityTypeAnonymizer();

            var res = eta.Apply(Expression.Constant(t));

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Type.IsGenericType);
            Assert.AreEqual(typeof(Tuple<,>), res.Type.GetGenericTypeDefinition());

            var a1 = res.Type.GetGenericArguments()[0];
            var a2 = res.Type.GetGenericArguments()[1];
            Assert.AreEqual(a1, a2);

            Assert.AreEqual(ExpressionType.Constant, res.NodeType);
            var c = (ConstantExpression)res;
            var o = c.Value;

            var t1 = res.Type.GetProperty("Item1").GetValue(o);
            var t2 = res.Type.GetProperty("Item2").GetValue(o);

            Assert.AreSame(t1, t2);

            Assert.AreEqual(42, t1.GetType().GetProperty("baz").GetValue(t1));
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_Errors1()
        {
            foreach (var o in new object[]
            {
                new Func<Qux, Qux>(x => x),
            })
            {
                var eta = new ExpressionEntityTypeAnonymizer();

                Assert.ThrowsException<InvalidOperationException>(() => eta.Apply(Expression.Constant(o)));
            }
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Constants_Errors2()
        {
            var mur = new MessedUpRewriter();
            mur.Test();
        }

        [TestMethod]
        public void ExpressionEntityTypeAnonymizer_Unchanged()
        {
            var eta = new ExpressionEntityTypeAnonymizer();

            var exp = (Expression<Func<D>>)(() => new D { X = 1, E = { Y = 2 }, Ys = { 3, 4, 5 } });

            var res = eta.Apply(exp);

            Assert.AreSame(res, exp);
        }

        private class MessedUpRewriter : EntityTypeSubstitutor
        {
            public MessedUpRewriter()
                : base(new Dictionary<Type, Type>())
            {
            }

            protected override Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments)
            {
                throw new NotImplementedException();
            }

            protected override object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
            {
                throw new NotImplementedException();
            }

            public void Test()
            {
                Assert.ThrowsException<InvalidOperationException>(() => ConvertConstant(1, DataType.FromType(typeof(int)), DataType.FromType(typeof(int[]))));
                Assert.ThrowsException<InvalidOperationException>(() => ConvertConstant(1, DataType.FromType(typeof(int)), DataType.FromType(typeof(long))));
                Assert.ThrowsException<NotImplementedException>(() => ConvertConstant(1, new MyDataType(), new MyDataType()));
                Assert.ThrowsException<NotSupportedException>(() => ConvertConstant(1, new InvalidDataType(), new InvalidDataType()));
            }

            private class MyDataType : DataType
            {
                public MyDataType()
                    : base(typeof(int))
                {
                }

                public override DataTypeKinds Kind => DataTypeKinds.Custom;

                public override object CreateInstance(params object[] arguments)
                {
                    throw new NotImplementedException();
                }
            }

            private class InvalidDataType : DataType
            {
                public InvalidDataType()
                    : base(typeof(int))
                {
                }

                public override DataTypeKinds Kind => (DataTypeKinds)123;

                public override object CreateInstance(params object[] arguments)
                {
                    throw new NotImplementedException();
                }
            }
        }

        private class Bar
        {
            [Mapping("foos")]
            public Foo[] Foos { get; set; }
        }

        private class Foo
        {
            [Mapping("qux")]
            public Tuple<Func<List<Qux>, long>, bool> Qux { get; set; }
        }

#pragma warning disable IDE0060 // Remove unused parameter (test code)
        public class Qux
        {
            public Qux()
            {
            }

            public Qux([Mapping("baz")] int myBaz)
            {
            }

            [Mapping("baz")]
            public int Baz { get; set; }

            [Mapping("foo")]
            public string Foo { get; set; }
        }

        private class MissingMapping
        {
            public MissingMapping(int x)
            {
            }

            [Mapping("foo")]
            public int X { get; set; }
        }

        private class NonMatchingMapping
        {
            public NonMatchingMapping([Mapping("bar")] int x)
            {
            }

            [Mapping("foo")]
            public int X { get; set; }
        }

        private class DuplicateMapping
        {
            public DuplicateMapping([Mapping("foo")] int x, [Mapping("foo")] int y)
            {
            }

            [Mapping("foo")]
            public int X { get; set; }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        private class Holder<T>
        {
            public T Value { get; set; }
        }

        private class D
        {
            public D()
            {
                E = new E();
                Ys = new List<int>();
            }

            public int X { get; set; }
            public E E { get; private set; }
            public List<int> Ys { get; private set; }
        }

        private class E
        {
            public int Y { get; set; }
        }
    }
}
