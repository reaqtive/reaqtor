// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Nuqleon.DataModel.CompilerServices;

using MijnEntiteiten;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    public partial class ExpressionEntityTypeRecordizerTests
    {
        private const int Repeat = 10;
        private static readonly Random r = new(42);

        [TestMethod]
        public void Recordizer_Concurrency_Expressions()
        {
            var recType = RuntimeCompiler.CreateRecordType(new[] {
                new KeyValuePair<string, Type>("a", typeof(int)),
                new KeyValuePair<string, Type>("b", typeof(Qux))
            }, valueEquality: true);

            var rec = Activator.CreateInstance(recType);
            recType.GetProperty("a").SetValue(rec, 42);
            recType.GetProperty("b").SetValue(rec, new Qux(43));

            var baseCases = new Expression[]
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

                Expression.Constant(new Qux()),
                Expression.Constant(new Qux(1)),
                Expression.Constant(new Qux(1) { Foo = "bar" }),

                // There is a known issue with the following test related to
                // collectible assemblies not being properly rooted when
                // instances of array types are created.
                //Expression.Constant(new Qux[1]),

                Expression.Constant(new Qux[] { new Qux(1), new Qux(2) }),
                Expression.Constant(new List<Qux> { new Qux(1), new Qux(2) }),
                Expression.Constant(42),
                Expression.Constant("bar"),
                Expression.Constant((Expression<Func<Qux, Qux>>)(x => x)),
                Expression.Constant((Expression<Func<Qux[]>>)(() => new Qux[] { new Qux(123) })),
                Expression.Constant(new Tuple<Qux, int>(new Qux(), 42)),

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)
                Expression.Constant(new { a = 1, b = new Qux(42), c = "bar" }),
#pragma warning restore IDE0050 // Convert to tuple

                Expression.Constant(rec),

                Expression.Constant(value: null, typeof(Qux)),

                Expression.Constant(new A { B = new B { Cs = new C[] { new C { D = 42, Es = new List<E> { new E { F = 42 } } } } } }),

                (Expression<Func<IEnumerable<A>, IEnumerable<int>>>)(xs => from x in xs
                                                                         let b = x.B
                                                                         from c in b.Cs
                                                                         let d = c.D
                                                                         where d > 0
                                                                         let e = new A { B = new B { Cs = new C[] { c, new C { D = d + 1, Es = new List<E> { new E { F = d * d } } } } } }
                                                                         let z = new A { B = { Cs = new C[] { c, new C { D = d + 1, Es = { new E { F = d * d } } } } } }
                                                                         where e.B.Cs[1].Es[0].F == z.B.Cs[1].Es[0].F
                                                                         select 7 * e.B.Cs.Sum(y => y.D)),

                Expression.Constant(new QuotedExpressionHolder { E = () => new E { F = 42 }.Equals(new E { F = 42 }) }),

                (Expression<Func<Persoon1, bool>>)(p => p.Geslacht == Sex.Male),

                (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1)),
                (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest { Foo = 1 }),
                (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1) { Bar = 1.0 }),
                (Expression<Func<FooSlim>>)(() => new FooSlim { Qux = { X = 1.0 } }),
                (Expression<Func<Ocean, Planet>>)(ocean => new Planet { OceanDepth = ocean.Depth }),
                (Expression<Func<ListPropertyTest>>)(() => new ListPropertyTest { List = { 1, 2, 3 } }),
                (Expression<Func<Star>>)(() => new Star(new List<int> { 1, 2, 3 }) { Planets = { new Planet(10) } }),
                (Expression<Func<UnchangedNewPropertyTest>>)(() => new UnchangedNewPropertyTest { Foo = new string("bar".ToArray()) }),
                (Expression<Func<MemberMemberListPropertyTest>>)(() => new MemberMemberListPropertyTest { Container = { List = new List<SimplePropertyTest> { new SimplePropertyTest(1) } } }),
                (Expression<Func<List<SimplePropertyTest>, MemberMemberListPropertyTest>>)(list => new MemberMemberListPropertyTest { Container = { List = list } }),

                Expression.Parameter(typeof(int)),
                Expression.Constant(RecursiveClass.Create()),
                (Expression<Func<string, int, Person>>)((name, age) => new Person { Name = name, Age = age }),
            };

            var testCases = Enumerable.Repeat(baseCases, Repeat).SelectMany(x => x).OrderBy(_ => r.Next());
            var helper = new SerializationHelper((lf, rf) => new RecordizingBonsaiSerializer(lf, rf));
            Parallel.ForEach(
                testCases,
                testCase => AssertRoundtrip(testCase, helper));
        }

        private static void AssertRoundtrip(Expression expected, SerializationHelper helper)
        {
            var comparer = new ExpressionEqualityComparer(() => new RecordTreeComparator(new TypeComparator()));

            // No shared state in fat recordizer
            var etr = new ExpressionEntityTypeRecordizer();
            var fatRecordized = etr.Apply(expected);
            Assert.IsTrue(comparer.Equals(expected, fatRecordized), "(Fat) Expected: {0}\nActual: {1}", expected.ToCSharpString(allowCompilerGeneratedNames: true), fatRecordized.ToCSharpString(allowCompilerGeneratedNames: true));

            // No shared state in fat anonymization
            var eta = new ExpressionEntityTypeAnonymizer();
            var fatAnonymized = etr.Apply(expected);
            Assert.IsTrue(comparer.Equals(expected, fatAnonymized), "(Fat) Expected: {0}\nActual: {1}", expected.ToCSharpString(allowCompilerGeneratedNames: true), fatAnonymized.ToCSharpString(allowCompilerGeneratedNames: true));

            // No shared state in slim recordizer
            using var stream = new MemoryStream();

            helper.Serialize(expected, stream);

            stream.Position = 0;

            var slim = helper.Deserialize<Expression>(stream);
            Assert.IsTrue(comparer.Equals(expected, fatRecordized), "(Slim) Expected: {0}\nActual: {1}", expected.ToCSharpString(allowCompilerGeneratedNames: true), slim.ToCSharpString(allowCompilerGeneratedNames: true));
        }
    }
}
