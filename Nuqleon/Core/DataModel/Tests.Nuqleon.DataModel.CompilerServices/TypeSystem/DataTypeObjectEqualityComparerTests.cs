// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Nuqleon.DataModel;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class DataTypeObjectEqualityComparerTests
    {
        #region Null

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Equals_Null()
        {
            var comparer = new DataTypeObjectEqualityComparer();

            Assert.IsTrue(comparer.Equals(x: null, y: null));
            Assert.IsFalse(comparer.Equals(x: null, y: 1));
            Assert.IsFalse(comparer.Equals(x: 1, y: null));
        }

        private class Foo
        {
        }

        [TestMethod]
        public void DataTypeObjectEqualityComparer_GetHashCode_Null()
        {
            var comparer = new DataTypeObjectEqualityComparer();

            Assert.AreEqual(comparer.GetHashCode(null), EqualityComparer<object>.Default.GetHashCode(null));
        }

        #endregion

        #region Primitive

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Primitive()
        {
            var equals = new Dictionary<object, object>
            {
                { 0, 0 },
                { 42, 42 },
                { "foo", "foo" },
                { ConsoleColor.Red, ConsoleColor.Red },
                { ConsoleColor.Blue, (int)ConsoleColor.Blue },
            };

            var inequals = new Dictionary<object, object>
            {
                { 0, 1 },
                { 42, "foo" },
                { 55, new[] { 42 } },
                { ConsoleColor.Red, ConsoleColor.Blue },
                { ConsoleColor.Blue, (int)ConsoleColor.Yellow },
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
                AssertInequal(pair.Value, pair.Key);
            }
        }

        #endregion

        #region Array

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Array()
        {
#pragma warning disable CA1825 // Avoid unnecessary zero-length array allocations. (Used as keys in dictionaries.)
            var equals = new Dictionary<object, object>
            {
                { new[] { 1, 2, 3 }, new[] { 1, 2, 3 } },
                { new string[0], Array.Empty<string>() },

                // These are unfortunate side effects of being type agnostic
                { new[] { 1, 2, 3 }, new List<int> { 1, 2, 3 } },
                { new string[0], Array.Empty<DateTime>() }
            };
#pragma warning restore CA1825

            var inequals = new Dictionary<object, object>
            {
                { new[] { 1, 2, 3 }, new[] { 1, 2 } },
                { new[] { 1, 2 }, new[] { 1, 2, 3 } },
                { new[] { 1, 2 }, new[] { 'a', 'b' } }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        #endregion

        #region Structural

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Structural()
        {
            var equals = new Dictionary<object, object>
            {
                { new Mapped { MyFoo = 1, MyBar = "the" }, new KT { Foo = 1, Bar = "the" } },
#if SUPPORT_SUBSET_TYPES
                { new Subset { Qux = new DateTime(10) }, new KT { Qux = new DateTime(10) } },
#endif
            };

            var inequals = new Dictionary<object, object>
            {
                { new KT { Qux = new DateTime(10) }, new Subset { Qux = new DateTime(10) } },
                { new KT { Foo = 2, Bar = "the" }, new Mapped { MyFoo = 1, MyBar = "the" } },
                { new Subset { Qux = new DateTime(10) }, new KT { Qux = new DateTime(10), Foo = 1 } },
                { new KT { Foo = 1, Bar = "the" }, new WrongMapped { WrongBar = 1, WrongFoo = "the" } }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        [KnownType]
        public class KT
        {
            [Mapping("foo")]
            public int Foo { get; set; }
            [Mapping("bar")]
            public string Bar { get; set; }
            [Mapping("qux")]
            public DateTime Qux { get; set; }

            public string Baz { get; set; }
        }

        public class Mapped
        {
            [Mapping("foo")]
            public int MyFoo { get; set; }
            [Mapping("bar")]
            public string MyBar { get; set; }
            [Mapping("qux")]
            public DateTime Qux { get; set; }
        }

        public class WrongMapped
        {
            [Mapping("foo")]
            public string WrongFoo { get; set; }
            [Mapping("bar")]
            public int WrongBar { get; set; }
        }

        public class Subset
        {
            [Mapping("qux")]
            public DateTime Qux { get; set; }
        }

        #endregion

        #region Function

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Function()
        {
            Func<int> f = () => 1;
            Func<int> g = () => 1;
            Func<string> h = () => "foo";

            var equals = new Dictionary<object, object>
            {
                { f, f }
            };

            var inequals = new Dictionary<object, object>
            {
                { f, g },
                { g, h }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        #endregion

        #region Expression

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Expression()
        {
            Expression f = Expression.Constant(0);
            Expression g = Expression.Constant(0);
            Expression h = Expression.Constant("foo");

            var equals = new Dictionary<object, object>
            {
                { f, f }
            };

            var inequals = new Dictionary<object, object>
            {
                { f, g },
                { g, h }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        #endregion

        #region Quotation

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Quotation()
        {
            Expression<Func<int>> f = () => 1;
            Expression<Func<int>> g = () => 1;
            Expression<Func<string>> h = () => "foo";

            var equals = new Dictionary<object, object>
            {
                { f, f }
            };

            var inequals = new Dictionary<object, object>
            {
                { f, g },
                { g, h }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        #endregion

        #region Man or Boy

        [TestMethod]
        public void DataTypeObjectEqualityComparer_ManOrBoy()
        {
            var ktnRecursive = new KTNested();
            var mnRecursive = new MappedNested();
            ktnRecursive.Self = ktnRecursive;
            mnRecursive.MySelf = mnRecursive;

            var equals = new Dictionary<object, object>
            {
                {
                    new Mapped2 { MyTuple = new Tuple<int, int, MappedNested>(1, 1, new MappedNested()), MyNested = { mnRecursive } },
                    new KT2 { Tuple = new Tuple<int, int, KTNested>(1, 1, new KTNested()), Nested = { ktnRecursive } }
                }
            };

            var inequals = new Dictionary<object, object>
            {
                {
                    new KT2 { Nested = { new KTNested { Value = 42 } } },
                    new ArrayMapped { ArrayNested = new[] { new KTNested { Value = 42 } } }
                }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        public class KT2
        {
            public KT2()
            {
                Nested = new List<KTNested>();
            }

            [Mapping("nested")]
            public List<KTNested> Nested { get; set; }
            [Mapping("tuple")]
            public Tuple<int, int, KTNested> Tuple { get; set; }
        }

        public class KTNested
        {
            [Mapping("value")]
            public int Value { get; set; }
            [Mapping("stuff")]
            public string Stuff { get; set; }
            [Mapping("self")]
            public KTNested Self { get; set; }
        }

        public class Mapped2
        {
            public Mapped2()
            {
                MyNested = new List<MappedNested>();
            }

            [Mapping("nested")]
            public List<MappedNested> MyNested { get; set; }
            [Mapping("tuple")]
            public Tuple<int, int, MappedNested> MyTuple { get; set; }
        }

        public class MappedNested
        {
            [Mapping("value")]
            public int Value { get; set; }
            [Mapping("stuff")]
            public string Stuff { get; set; }
            [Mapping("self")]
            public MappedNested MySelf { get; set; }
        }

        public class ArrayMapped
        {
            [Mapping("nested")]
            public KTNested[] ArrayNested { get; set; }
        }

        #endregion

        #region Strict

        [TestMethod]
        public void DataTypeObjectEqualityComparer_Strict()
        {
            var equals = new Dictionary<object, object>
            {
                { new Mapped { MyFoo = 1, MyBar = "the" }, new KT { Foo = 1, Bar = "the" } }
            };

            var inequals = new Dictionary<object, object>
            {
                { new Subset { Qux = new DateTime(10) }, new KT { Qux = new DateTime(10) } }
            };

            foreach (var pair in equals)
            {
                AssertEqual(pair.Key, pair.Value);
            }

            foreach (var pair in inequals)
            {
                AssertInequal(pair.Key, pair.Value);
            }
        }

        #endregion

        #region Helpers

        private static void AssertEqual(object x, object y)
        {
            var comparer = new DataTypeObjectEqualityComparer();
            Assert.IsTrue(comparer.Equals(x, y));
            Assert.AreEqual(comparer.GetHashCode(x), comparer.GetHashCode(y));

            var comparer2 = DataTypeEqualityComparer<object>.Default;
            Assert.IsTrue(comparer2.Equals(x, y));
            Assert.AreEqual(comparer2.GetHashCode(x), comparer2.GetHashCode(y));
        }

        private static void AssertInequal(object x, object y)
        {
            var comparer = new DataTypeObjectEqualityComparer();
            Assert.IsFalse(comparer.Equals(x, y));

            var comparer2 = DataTypeEqualityComparer<object>.Default;
            Assert.IsFalse(comparer2.Equals(x, y));
        }

        #endregion
    }
}
