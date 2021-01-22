// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;

using MijnEntiteiten;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public partial class ExpressionEntityTypeRecordizerTests
    {
        [TestMethod]
        public void ExpressionEntityTypeRecordizer_ArgumentChecking()
        {
            var etr = new ExpressionEntityTypeRecordizer();

            AssertEx.ThrowsException<ArgumentNullException>(() => etr.Apply(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Simple()
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
                var eta = new ExpressionEntityTypeRecordizer();

                var res = eta.Apply(e);

                netrc.Visit(res);

                var expressionComparer = ObjectComparator.CreateInstance().ExpressionComparator;
                expressionComparer.Equals(e, res);
            }
        }

#pragma warning disable CA1822 // Mark static. (Used in expression tree)
        private void FooIt<T>()
        {
        }
#pragma warning restore CA1822

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Errors()
        {
            foreach (var e in new Expression[]
            {
                (Expression<Func<Qux>>)(() => new Qux(1) { Baz = 1 }),
                (Expression<Func<MissingMapping>>)(() => new MissingMapping(1)),
                (Expression<Func<NonMatchingMapping>>)(() => new NonMatchingMapping(1)),
                (Expression<Func<DuplicateMapping>>)(() => new DuplicateMapping(1, 2)),
            })
            {
                var eta = new ExpressionEntityTypeRecordizer();

                Assert.ThrowsException<InvalidOperationException>(() => eta.Apply(e));
            }
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_Simple()
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
                var eta = new ExpressionEntityTypeRecordizer();

                var res = (ConstantExpression)eta.Apply(Expression.Constant(o));

                Assert.IsTrue(ObjectComparator.CreateInstance().Equals(o, res.Value));
            }
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_Null()
        {
            var eta = new ExpressionEntityTypeRecordizer();

            var res = eta.Apply(Expression.Constant(value: null, typeof(Qux)));

            Assert.IsNotNull(res); // TODO: semantic equivalence checks
            Assert.IsTrue(res is ConstantExpression);
            Assert.IsNull(((ConstantExpression)res).Value);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_ManOrBoy1()
        {
            var c = Expression.Constant(new A { B = new B { Cs = new C[] { new C { D = 42, Es = new List<E> { new E { F = 42 } } } } } });

            var eta = new ExpressionEntityTypeRecordizer();

            var res = eta.Apply(c);

            Assert.AreNotEqual(res.Type, c.Type);

            dynamic d = ((ConstantExpression)res).Value;

            int x = d.b.cs[0].d;

            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_ManOrBoy2()
        {
            Expression<Func<IEnumerable<A>, IEnumerable<int>>> f = xs => from x in xs
                                                                         let b = x.B
                                                                         from c in b.Cs
                                                                         let d = c.D
                                                                         where d > 0
                                                                         let e = new A { B = new B { Cs = new C[] { c, new C { D = d + 1, Es = new List<E> { new E { F = d * d } } } } } }
                                                                         let z = new A { B = { Cs = new C[] { c, new C { D = d + 1, Es = { new E { F = d * d } } } } } }
                                                                         where e.B.Cs[1].Es[0].F == z.B.Cs[1].Es[0].F
                                                                         select 7 * e.B.Cs.Sum(y => y.D);

            var g = f.Compile();

            var res = g(new[] {
                new A { B = new B { Cs = new C[] { new C { D = 23 } } } },
                new A { B = new B { Cs = new C[] { new C { D = -1 } } } },
                new A { B = new B { Cs = new C[] { new C { D = 87 } } } },
            });

            var eta = new ExpressionEntityTypeRecordizer();

            var h = (LambdaExpression)eta.Apply(f);

            var anonA = h.Parameters[0].Type.GetGenericArguments()[0];
            var propB = anonA.GetProperty("b");
            var anonB = propB.PropertyType;
            var propCs = anonB.GetProperty("cs");
            var anonC = propCs.PropertyType.GetElementType();
            var propD = anonC.GetProperty("d");

            var c23 = Activator.CreateInstance(anonC);
            propD.SetValue(c23, 23);
            var cm1 = Activator.CreateInstance(anonC);
            propD.SetValue(cm1, -1);
            var c87 = Activator.CreateInstance(anonC);
            propD.SetValue(c87, 87);

            var cs23 = (IList)Array.CreateInstance(anonC, 1);
            cs23[0] = c23;
            var csm1 = (IList)Array.CreateInstance(anonC, 1);
            csm1[0] = cm1;
            var cs87 = (IList)Array.CreateInstance(anonC, 1);
            cs87[0] = c87;

            var b23 = Activator.CreateInstance(anonB);
            propCs.SetValue(b23, cs23);
            var bm1 = Activator.CreateInstance(anonB);
            propCs.SetValue(bm1, csm1);
            var b87 = Activator.CreateInstance(anonB);
            propCs.SetValue(b87, cs87);

            var a23 = Activator.CreateInstance(anonA);
            propB.SetValue(a23, b23);
            var am1 = Activator.CreateInstance(anonA);
            propB.SetValue(am1, bm1);
            var a87 = Activator.CreateInstance(anonA);
            propB.SetValue(a87, b87);

            var inp = (IList)Array.CreateInstance(anonA, 3);
            inp[0] = a23;
            inp[1] = am1;
            inp[2] = a87;

            var p = CompilerGeneratedNameEliminator.Prettify(h).ToCSharpString(allowCompilerGeneratedNames: true).Replace("<>a__RecordType", "rec").Replace("<>h__TransparentIdentifier", "__t");
            Assert.IsTrue(true, p);

            var output = (IEnumerable<int>)h.Compile().DynamicInvoke(new object[] { inp });

            Assert.IsTrue(res.SequenceEqual(output));
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_ManOrBoy3()
        {
            var c = new QuotedExpressionHolder { E = () => new E { F = 42 }.Equals(new E { F = 42 }) };

            var eta = new ExpressionEntityTypeRecordizer();

            var res = (ConstantExpression)eta.Apply(Expression.Constant(c));

            // The type `E` uses reference equality, as it does not overload `Equals`.
            Assert.IsFalse(c.E.Compile().Invoke());
            var property = res.Value.GetType().GetProperty("expression");

            // The generated record type for `E` does have value equality based on its properties.
            Assert.IsTrue(((Expression<Func<bool>>)property.GetValue(res.Value)).Compile().Invoke());
        }

        private class A
        {
            public A()
            {
                B = new B();
            }

            [Mapping("b")]
            public B B { get; set; }
        }

        private class B
        {
            [Mapping("cs")]
            public C[] Cs { get; set; }

            [Mapping("ds")]
            public int[] Ds { get; set; }
        }

        private class C
        {
            public C()
            {
                Es = new List<E>();
            }

            [Mapping("d")]
            public int D { get; set; }

            [Mapping("es")]
            public List<E> Es { get; set; }
        }

        private class E
        {
            [Mapping("f")]
            public int F { get; set; }
        }

        private class QuotedExpressionHolder
        {
            [Mapping("expression")]
            public Expression<Func<bool>> E { get; set; }
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Constants_ReferenceEquality()
        {
            var q = new Qux { Baz = 42 };
            var t = new Tuple<Qux, Qux>(q, q);

            var eta = new ExpressionEntityTypeRecordizer();

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
        public void ExpressionEntityTypeRecordizer_Constants_Errors1()
        {
            foreach (var o in new object[]
            {
                new Func<Qux, Qux>(x => x),
            })
            {
                var eta = new ExpressionEntityTypeRecordizer();

                Assert.ThrowsException<InvalidOperationException>(() => eta.Apply(Expression.Constant(o)));
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter (entities are purely here for tests and are reflected upon)
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

        public class Qux
        {
            public Qux()
            {
            }

            public Qux([Mapping("baz")] int myBaz)
            {
                Baz = myBaz;
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

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum1()
        {
            var e = (Expression<Func<Persoon1, bool>>)(p => p.Geslacht == Sex.Male);

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (LambdaExpression)eta.Apply(e);

            var m = (MemberExpression)((UnaryExpression)((BinaryExpression)r.Body).Left).Operand;

            Assert.AreEqual("contoso://entities/person/sex", m.Member.Name);
            Assert.AreEqual(typeof(int), ((PropertyInfo)m.Member).PropertyType);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum2()
        {
            var e = (Expression<Func<Persoon1, Persoon1>>)(x => x);

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (LambdaExpression)eta.Apply(e);

            var t = r.Parameters.Single().Type;
            Assert.IsTrue(t.IsRecordType());

            var ps = t.GetProperties();
            var s = ps.Single(p => p.Name == "contoso://entities/person/sex");
            Assert.AreEqual(typeof(int), s.PropertyType);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum3()
        {
            var e = (Expression<Func<Persoon2, bool>>)(p => p.Geslacht == Sex.Male);

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (LambdaExpression)eta.Apply(e);

            var m = (MemberExpression)((UnaryExpression)((BinaryExpression)r.Body).Left).Operand;

            Assert.AreEqual("contoso://entities/person/sex", m.Member.Name);
            Assert.AreEqual(typeof(int?), ((PropertyInfo)m.Member).PropertyType);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum3_Nullable1()
        {
            var p = Expression.Parameter(typeof(Persoon2));
            var e = Expression.Equal(Expression.Property(p, "Geslacht"), Expression.Constant(Sex.Male, typeof(Sex?)));

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (BinaryExpression)eta.Apply(e);

            var m = (MemberExpression)r.Left;
            var c = (ConstantExpression)r.Right;

            Assert.AreEqual("contoso://entities/person/sex", m.Member.Name);
            Assert.AreEqual(typeof(int?), ((PropertyInfo)m.Member).PropertyType);
            Assert.AreEqual(typeof(int?), c.Type);
            Assert.AreEqual(1, c.Value);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum3_Nullable2()
        {
            var p = Expression.Parameter(typeof(Persoon2));
            var e = Expression.Equal(Expression.Property(p, "Geslacht"), Expression.Constant(value: null, typeof(Sex?)));

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (BinaryExpression)eta.Apply(e);

            var m = (MemberExpression)r.Left;
            var c = (ConstantExpression)r.Right;

            Assert.AreEqual("contoso://entities/person/sex", m.Member.Name);
            Assert.AreEqual(typeof(int?), ((PropertyInfo)m.Member).PropertyType);
            Assert.AreEqual(typeof(int?), c.Type);
            Assert.IsNull(c.Value);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_Enum4()
        {
            var e = (Expression<Func<Persoon3, bool>>)(p => p.Geslacht == Sex.Male && p.Color == ConsoleColor.Blue && Console.ForegroundColor == ConsoleColor.Red);

            var eta = new ExpressionEntityTypeRecordizer();

            var r = (LambdaExpression)eta.Apply(e);

            var t = r.Parameters.Single().Type;
            Assert.IsTrue(t.IsRecordType());

            var ps = t.GetProperties();
            var s1 = ps.Single(p => p.Name == "contoso://entities/person/sex");
            var s2 = ps.Single(p => p.Name == "contoso://entities/person/color");
            Assert.AreEqual(typeof(int), s1.PropertyType);
            Assert.AreEqual(typeof(ConsoleColor), s2.PropertyType);
        }

        [TestMethod]
        public void ExpressionEntityTypeRecordizer_RecursiveType_Regression()
        {
            var helper = new SerializationHelper((lf, rf) => new RecordizingBonsaiSerializer(lf, rf));

            var e = Expression.Constant(RecursiveClass.Create());

            using var stream = new MemoryStream();

            helper.Serialize(e, stream);

            stream.Position = 0;

            var json = new StreamReader(stream).ReadToEnd();
            Assert.AreEqual(
                @"{""Context"":{""Types"":[[""::"",""System.String"",0],[""{;}"",[[""contoso://entities/parent/typestring"",0],[""contoso://entities/parent/recursive"",1]]]],""Assemblies"":[""STD""],""Version"":""0.9.0.0""},""Expression"":["":"",{""contoso://entities/parent/typestring"":""Hello"",""contoso://entities/parent/recursive"":{""contoso://entities/parent/typestring"":"","",""contoso://entities/parent/recursive"":{""contoso://entities/parent/typestring"":""World"",""contoso://entities/parent/recursive"":null}}},1]}",
                json
                    .Replace("System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", "STD")
                    .Replace("System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", "STD")
                    .Replace("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "STD")
            );
        }
    }
}

namespace MijnEntiteiten
{
    public class Persoon1
    {
        [Mapping("contoso://entities/person/name")]
        public string Naam { get; set; }

        [Mapping("contoso://entities/person/age")]
        public int Leeftijd { get; set; }

        [Mapping("contoso://entities/person/sex")]
        public Sex Geslacht { get; set; }
    }

    public class Persoon2
    {
        [Mapping("contoso://entities/person/name")]
        public string Naam { get; set; }

        [Mapping("contoso://entities/person/age")]
        public int Leeftijd { get; set; }

        [Mapping("contoso://entities/person/sex")]
        public Sex? Geslacht { get; set; }
    }

    public class Persoon3
    {
        [Mapping("contoso://entities/person/name")]
        public string Naam { get; set; }

        [Mapping("contoso://entities/person/age")]
        public int Leeftijd { get; set; }

        [Mapping("contoso://entities/person/sex")]
        public Sex Geslacht { get; set; }

        [Mapping("contoso://entities/person/color")]
        public ConsoleColor Color { get; set; }
    }

    public enum Sex
    {
        [Mapping("contoso://gender/male")]
        Male = 1,

        [Mapping("contoso://gender/female")]
        Female = 2,
    }
}
