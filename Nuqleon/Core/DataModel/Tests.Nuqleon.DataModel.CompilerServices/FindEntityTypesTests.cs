// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices.TypeSystem
{
    [TestClass]
    public class FindEntityTypesTests
    {
        [TestMethod]
        public void FindEntityTypes_Simple()
        {
            var e = (Expression<Func<Bar>>)(() => new Bar());

            FindEntitiesAndAssert(e, new[] {
                typeof(Bar),
                typeof(Foo), // transitive
                typeof(Qux), // closure
            });
        }

        [TestMethod]
        public void FindEntityTypes_Cyclic()
        {
            var e = (Expression<Func<Baz, Baz>>)(b => b);

            FindEntitiesAndAssert(e, new[] {
                typeof(Baz),
            });
        }

        [TestMethod]
        public void FindEntityTypes_TypeIs()
        {
            var e = (Expression<Func<object, bool>>)(o => o is Bar);

            FindEntitiesAndAssert(e, new[] {
                typeof(Bar),
                typeof(Foo), // transitive
                typeof(Qux), // closure
            });
        }

        [TestMethod]
        public void FindEntityTypes_SubtleHiddenType()
        {
            var e = (Expression<Action>)(() => FooIt<Bar>());

            FindEntitiesAndAssert(e, new[] {
                typeof(Bar),
                typeof(Foo), // transitive
                typeof(Qux), // closure
            });
        }

        private static void FooIt<T>()
        {
        }

        [TestMethod]
        public void FindEntityTypes_ManOrBoy()
        {
#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

            var e = (from x in Enumerable.Empty<List<Bar>>().AsQueryable()
                     from y in x
                     from z in y.Foos
                     let a = x
                     let b = y
                     let c = z
                     where c.Qux.Item2
                     select new { a, b, c }).Expression;

#pragma warning restore IDE0050 // Convert to tuple

            FindEntitiesAndAssert(e, new[] {
                typeof(Bar),
                typeof(Foo),
                typeof(Qux),
            });
        }

        [TestMethod]
        public void FindEntityTypes_KnownTypesIgnored()
        {
            var e = (Expression<Action>)(() => FooIt<KnownFoo>());

            FindEntitiesAndAssert(e, new[] { typeof(Qux) }, new[] { typeof(KnownFoo) });
        }

        [TestMethod]
        public void FindEntityTypes_Enums1()
        {
            var e = (from x in Enumerable.Empty<BarWithEnum>().AsQueryable()
                     select x).Expression;

            FindEnumsAndAssert(e, new[] {
                typeof(BarEnum),
            });
        }

        [TestMethod]
        public void FindEntityTypes_Enums2()
        {
            var e = (from x in Enumerable.Empty<BarWithEnum>().AsQueryable()
                     where x.Bar == BarEnum.Foo
                     select x).Expression;

            FindEnumsAndAssert(e, new[] {
                typeof(BarEnum),
            });
        }

        [TestMethod]
        public void FindEntityTypes_Enums3()
        {
            var e = (from x in Enumerable.Empty<int>().AsQueryable()
                     where new List<BarEnum>().Contains(BarEnum.Foo)
                     select x).Expression;

            FindEnumsAndAssert(e, new[] {
                typeof(BarEnum),
            });
        }

        [TestMethod]
        public void FindEntityTypes_Enums4()
        {
            var e = (from x in Enumerable.Empty<FooWithEnum>().AsQueryable()
                     select x).Expression;

            FindEnumsAndAssert(e, new[] {
                typeof(BarEnum),
            });
        }

        [TestMethod]
        public void FindEntityTypes_Enums5()
        {
            var e = (from x in Enumerable.Empty<FooWithEnum>().AsQueryable()
                     where x.Bar == BarEnum.Foo
                     select x).Expression;

            FindEnumsAndAssert(e, new[] {
                typeof(BarEnum),
            });
        }

        private static void FindEntitiesAndAssert(Expression expression, params Type[] types)
        {
            var res = FindEntityTypes.Apply(expression);

            foreach (var t in types)
            {
                Assert.IsTrue(res.Entities.ContainsKey(t));
            }
        }

        private static void FindEntitiesAndAssert(Expression expression, IEnumerable<Type> types, IEnumerable<Type> ignored)
        {
            var res = FindEntityTypes.Apply(expression);

            foreach (var t in types)
            {
                Assert.IsTrue(res.Entities.ContainsKey(t));
            }

            foreach (var t in ignored)
            {
                Assert.IsFalse(res.Entities.ContainsKey(t));
            }
        }

        private static void FindEnumsAndAssert(Expression expression, params Type[] types)
        {
            var res = FindEntityTypes.Apply(expression);

            foreach (var t in types)
            {
                Assert.IsTrue(res.Enumerations.ContainsKey(t));
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

        private class Qux
        {
            [Mapping("baz")]
            public int Baz { get; set; }
        }

        private class Baz
        {
            [Mapping("this")]
            public Baz Self { get; set; }
        }

        private class BarWithEnum
        {
            [Mapping("bar")]
            public BarEnum Bar { get; set; }
        }

        [KnownType]
        private class KnownFoo
        {
            [Mapping("qux")]
            public Tuple<Func<List<Qux>, long>, bool> Qux { get; set; }
        }

        [KnownType]
        private class KnownSimple
        {
            [Mapping("x")]
            public int X { get; set; }
        }

        private class InnerKnown
        {
            [Mapping("foo")]
            public KnownFoo KnownFoo { get; set; }

            [Mapping("simple")]
            public KnownSimple KnownSimple { get; set; }
        }

        private class FooWithEnum
        {
            [Mapping("bar")]
            public BarEnum? Bar { get; set; }
        }

        private enum BarEnum
        {
            [Mapping("foo")]
            Foo,

            [Mapping("qux")]
            Qux,
        }
    }
}
