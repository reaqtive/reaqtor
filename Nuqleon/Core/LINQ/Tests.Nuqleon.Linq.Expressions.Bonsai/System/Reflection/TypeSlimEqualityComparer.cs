// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Memory;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class TypeSlimEqualityComparerTests : TestBase
    {
        #region Equals Tests

        [TestMethod]
        public void TypeSlimEqualityComparer_Equals()
        {
            var eq = TypeSlimEqualityComparer.Default;
            Assert.IsTrue(eq.Equals(null, null));
            Assert.IsFalse(eq.Equals(null, SlimType));
            Assert.IsFalse(eq.Equals(SlimType, null));
            Assert.IsFalse(eq.Equals(SlimType, TypeSlim.Array(SlimType)));
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsSimple()
        {
            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(assembly: null, "Foo"), TypeSlim.Simple(assembly: null, "Foo")),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(new AssemblySlim("Foo"), "Foo"), TypeSlim.Simple(new AssemblySlim("Foo"), "Foo")),
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(assembly: null, "Foo"), TypeSlim.Simple(assembly: null, "Bar")),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(new AssemblySlim("Bar"), "Foo"), TypeSlim.Simple(new AssemblySlim("Bar"), "Bar")),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(new AssemblySlim("Foo"), "Bar"), TypeSlim.Simple(new AssemblySlim("Bar"), "Bar")),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Simple(new AssemblySlim("Foo"), "Bar"), TypeSlim.Simple(new AssemblySlim("Bar"), "Foo")),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal);
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsArray()
        {
            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Array(SlimType), TypeSlim.Array(SlimType)),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Array(SlimType, 2), TypeSlim.Array(SlimType, 2))
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Array(TypeSlim.Simple(assembly: null, "Qux")), TypeSlim.Array(SlimType)),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Array(SlimType), TypeSlim.Array(SlimType, 2)),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Array(SlimType, 1), TypeSlim.Array(SlimType, 2)),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal);
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsStructural()
        {
            var p1 = SlimType.GetProperty("Foo", SlimType, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var p2 = SlimType.GetProperty("Foo", SlimType, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: false);
            var p3 = SlimType.GetProperty("Foo", SlimType, new TypeSlim[] { SlimType }.ToReadOnly(), canWrite: true);
            var p4 = SlimType.GetProperty("Bar", SlimType, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var p5 = SlimType.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var s1 = TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s2 = TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var empty = EmptyReadOnlyCollection<PropertyInfoSlim>.Instance;

            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>(s1, s2),
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p2 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record)
                ),
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p3 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record)
                ),
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p4 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record)
                ),
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(new List<PropertyInfoSlim> { p5 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record)
                ),
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(empty, hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(empty, hasValueEqualitySemantics: false, StructuralTypeSlimKind.Record)
                ),
                new KeyValuePair<TypeSlim, TypeSlim>(
                    TypeSlim.Structural(empty, hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record),
                    TypeSlim.Structural(empty, hasValueEqualitySemantics: true, StructuralTypeSlimKind.Anonymous)
                ),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal);
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsStructural_RecursiveReference()
        {
            var s1 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            s1.AddProperty(s1.GetProperty("Foo", s1, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true));
            var s2 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            s2.AddProperty(s2.GetProperty("Foo", s2, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true));

            var equal = new Dictionary<TypeSlim, TypeSlim>
            {
                { s1, s2 }
            };

            AssertAllEqual(equal.AsEnumerable());
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsGeneric()
        {
            var genericArguments = new List<TypeSlim> { SlimType, SlimType }.AsReadOnly();
            var definition1 = TypeSlim.GenericDefinition(new AssemblySlim("Foo"), "Bar");
            var definition2 = TypeSlim.GenericDefinition(new AssemblySlim("Bar"), "Foo");

            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Generic(definition1, genericArguments), TypeSlim.Generic(definition1, genericArguments)),
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Generic(definition1, genericArguments), TypeSlim.Generic(definition2, genericArguments)),
                new KeyValuePair<TypeSlim, TypeSlim>
                    (TypeSlim.Generic(definition1, genericArguments), TypeSlim.Generic(definition1, genericArguments.Take(1).ToReadOnly())),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal);
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsGenericDefinition()
        {
            var definition1 = TypeSlim.GenericDefinition(new AssemblySlim("Foo"), "Bar");
            var definition2 = TypeSlim.GenericDefinition(new AssemblySlim("Bar"), "Foo");

            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (definition1, definition1),
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>
                    (definition1, definition2),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal);
        }

        [TestMethod]
        public void TypeSlimEqualityComparer_EqualsGenericParameter()
        {
            var t1 = TypeSlim.GenericParameter("T");
            var t2 = TypeSlim.GenericParameter("T");

            var equal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>(t1, t1),
            };

            var inequal = new[]
            {
                new KeyValuePair<TypeSlim, TypeSlim>(t1, t2),
            };

            AssertAllEqual(equal);
            AssertAllNotEqual(inequal, tolerateEqualHashCode: true);
        }

        [TestMethod]
        public void TypeSlimEqualityComparator_Misc()
        {
            var comp = new TypeSlimEqualityComparator();

            _ = comp.GetHashCode(null);

            // did not throw
            Assert.IsTrue(true);
        }

#if NET5_0 || NETCOREAPP3_1
        [Ignore] // See NB comment below; the implementation detail changed in .NET Core 3.1 and above.
#endif
        [TestMethod]
        public void TypeSlimEqualityComparator_Pooling()
        {
            // NB: This relies on some implementation details of collection APIs in .NET. The TypeSlimEqualityComparator
            //     used a HashSet<T> and a Dictionary<K, V> which both have a version field we can use to assert progress
            //     in a white-box manner.

            var fields = (from f in typeof(TypeSlimEqualityComparator).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          where typeof(ICollection).IsAssignableFrom(f.FieldType)
                          let v = f.FieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Single(i => i.FieldType == typeof(int) && i.Name.Contains("version"))
                          select (Collection: f, Version: v))
                         .ToArray();

            Assert.IsTrue(fields.Length > 0);

            var p1 = SlimType.GetProperty("Foo", SlimType, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var s1 = TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s2 = TypeSlim.Structural(new List<PropertyInfoSlim> { p1 }.AsReadOnly(), hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);

            var pool = new ObjectPool<TypeSlimEqualityComparator>(() => new TypeSlimEqualityComparator(), 1);

            var obj = default(TypeSlimEqualityComparator);
            int[] v1, v2, v3;

            using (var eq = pool.New())
            {
                obj = eq.Object;

                v1 = GetVersions(obj);

                Assert.IsTrue(obj.Equals(s1, s2));

                v2 = GetVersions(obj); // NB: Equals for structural types will touch the tracking data structures.
            }

            v3 = GetVersions(obj); // NB: The Clear method should clear all tracking data structures.

            AssertVersionsChanged(v1, v2);
            AssertVersionsChanged(v2, v3);

            int[] GetVersions(TypeSlimEqualityComparator eq)
            {
                return (from f in fields
                        let collection = f.Collection.GetValue(eq)
                        let version = (int)f.Version.GetValue(collection)
                        select version).ToArray();
            }

            static void AssertVersionsChanged(int[] versions1, int[] versions2)
            {
                Assert.IsTrue(versions1.Zip(versions2, (l, r) => l != r).All(b => b));
            }
        }

        [TestMethod]
        public void TypeSlimEqualityComparator_Extensions()
        {
            var comp = new TypeSlimEqualityComparator();

            var fts1 = new FakeTypeSlim();
            var fts2 = new FakeTypeSlim();
            Assert.ThrowsException<NotImplementedException>(() => comp.GetHashCode(fts1));
            Assert.ThrowsException<NotImplementedException>(() => comp.Equals(fts1, fts2));

            var fsts1 = new FakeStructuralTypeSlim();
            var fsts2 = new FakeStructuralTypeSlim();
            Assert.ThrowsException<NotSupportedException>(() => comp.GetHashCode(fsts1));
            Assert.ThrowsException<NotSupportedException>(() => comp.Equals(fsts1, fsts2));
        }

        [TestMethod]
        public void TypeSlimEqualityComparator_Properties()
        {
            new PropertyComparator().Test();
        }

        private sealed class FakeTypeSlim : TypeSlim
        {
            public override TypeSlimKind Kind => (TypeSlimKind)1023;
        }

        private sealed class FakeStructuralTypeSlim : StructuralTypeSlim
        {
            public override bool HasValueEqualitySemantics => false;

            public override ReadOnlyCollection<PropertyInfoSlim> Properties { get; } = global::System.Array.Empty<PropertyInfoSlim>().ToReadOnly();

            public override StructuralTypeSlimKind StructuralKind => (StructuralTypeSlimKind)1023;
        }

        private sealed class PropertyComparator : TypeSlimEqualityComparator
        {
            public void Test()
            {
                var tsInt32 = typeof(int).ToTypeSlim();
                var tsInt64 = typeof(long).ToTypeSlim();

                var ps1 = PropertyInfoSlim.Make(tsInt32, "foo", tsInt32, Array.Empty<TypeSlim>().ToReadOnly(), canWrite: false);
                var ps2 = PropertyInfoSlim.Make(tsInt32, "bar", tsInt32, Array.Empty<TypeSlim>().ToReadOnly(), canWrite: false);
                var ps3 = PropertyInfoSlim.Make(tsInt32, "qux", tsInt32, new TypeSlim[] { tsInt32 }.ToReadOnly(), canWrite: false);
                var ps4 = PropertyInfoSlim.Make(tsInt32, "qux", tsInt32, new TypeSlim[] { tsInt64 }.ToReadOnly(), canWrite: false);

                Assert.IsTrue(base.EqualsProperty(null, null));
                Assert.IsFalse(base.EqualsProperty(ps1, null));
                Assert.IsFalse(base.EqualsProperty(null, ps1));
                Assert.IsTrue(base.EqualsProperty(ps1, ps1));

                Assert.IsFalse(base.EqualsProperty(ps1, ps2));
                Assert.IsFalse(base.EqualsProperty(ps2, ps1));

                Assert.IsFalse(base.EqualsProperty(ps1, ps3));
                Assert.IsFalse(base.EqualsProperty(ps3, ps1));

                Assert.IsFalse(base.EqualsProperty(ps3, ps4));
                Assert.IsFalse(base.EqualsProperty(ps4, ps3));
            }
        }

        #endregion

        #region Test Helpers

        private static void AssertAllEqual(IEnumerable<KeyValuePair<TypeSlim, TypeSlim>> pairs)
        {
            foreach (var kv in pairs)
            {
                Assert.AreEqual(kv.Key, kv.Value, string.Format(CultureInfo.InvariantCulture, "Type slim '{0}' should equal type slim '{1}'.", kv.Key, kv.Value));
                Assert.AreEqual(kv.Key.GetHashCode(), kv.Value.GetHashCode(), string.Format(CultureInfo.InvariantCulture, "Type slim '{0}' hash code '{1}' should equal type slim '{2}' hash code '{3}'.", kv.Key, kv.Key.GetHashCode(), kv.Value, kv.Value.GetHashCode()));
            }
        }

        private static void AssertAllNotEqual(IEnumerable<KeyValuePair<TypeSlim, TypeSlim>> pairs, bool tolerateEqualHashCode = false)
        {
            foreach (var kv in pairs)
            {
                Assert.AreNotEqual(kv.Key, kv.Value, string.Format(CultureInfo.InvariantCulture, "Type slim '{0}' should not equal '{1}'.", kv.Key, kv.Value));

                // REVIEW: Clashes are possible here because there's no guarantee about the uniqueness
                //         of hash codes (though it should be extremely unlikely). We may want to
                //         consider more stability of hash codes across repeated runs by using a stable
                //         hash code for strings, which are pretty much the only external hash codes we
                //         rely on to compute hash codes of TypeSlim instances.

                if (!tolerateEqualHashCode)
                {
                    Assert.AreNotEqual(kv.Key.GetHashCode(), kv.Value.GetHashCode(), string.Format(CultureInfo.InvariantCulture, "Type slim '{0}' hash code '{1}' should not equal type slim '{2}' hash code '{3}'.", kv.Key, kv.Key.GetHashCode(), kv.Value, kv.Value.GetHashCode()));
                }
            }
        }

        #endregion
    }
}
