// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests.System.Memory.Diagnostics
{
    [TestClass]
    public class ObjectSetTests
    {
        [TestMethod]
        public void ObjectSet_Basics()
        {
            var set = new ObjectSet();

            var objs = Enumerable.Range(0, 1000).Select(o => new object()).ToArray();

            //
            // IsReadOnly
            //

            Assert.IsFalse(((ICollection<object>)set).IsReadOnly);

            //
            // Count
            //

            Assert.AreEqual(0, set.Count);

            //
            // Contains
            //

            Assert.IsFalse(set.Contains(item: null));

            foreach (var obj in objs)
            {
                Assert.IsFalse(set.Contains(obj));
            }

            //
            // IEnumerable
            //

            var arr = set.ToArray();

            Assert.AreEqual(0, arr.Length);

            //
            // Remove
            //

            Assert.IsFalse(set.Remove(item: null));

            foreach (var obj in objs)
            {
                Assert.IsFalse(set.Remove(obj));
            }

            //
            // Add
            //

            for (var i = 0; i < objs.Length; i++)
            {
                var obj = objs[i];

                //
                // Add
                //

                if (i % 2 == 0)
                {
                    Assert.IsTrue(set.Add(obj));
                    Assert.IsFalse(set.Add(obj));
                }
                else
                {
                    ((ICollection<object>)set).Add(obj);
                }

                //
                // Count
                //

                Assert.AreEqual(i + 1, set.Count);

                //
                // Contains
                //

                Assert.IsFalse(set.Contains(item: null));

                for (var j = 0; j <= i; j++)
                {
                    Assert.IsTrue(set.Contains(objs[j]));
                }

                //
                // IEnumerable
                //

                Assert.IsTrue(new HashSet<object>(set).SetEquals(new HashSet<object>(objs.Take(i + 1))));
            }

            //
            // Null - Add
            //

            Assert.IsTrue(set.Add(item: null));
            Assert.IsFalse(set.Add(item: null));

            //
            // Null - Count
            //

            Assert.AreEqual(objs.Length + 1, set.Count);

            //
            // Null - Contains
            //

            Assert.IsTrue(set.Contains(item: null));

            //
            // Null - IEnumerable
            //

            Assert.IsTrue(new HashSet<object>(set).SetEquals(new HashSet<object>(objs.Concat(new object[] { null }))));

            //
            // Remove
            //

            var rand = new Random(1983);
            var remObjs = objs.OrderBy(_ => rand.Next()).ToArray();

            for (var i = 0; i < remObjs.Length; i++)
            {
                var obj = remObjs[i];

                //
                // Remove
                //

                Assert.IsTrue(set.Remove(obj));
                Assert.IsFalse(set.Remove(obj));

                //
                // Count
                //

                Assert.AreEqual(objs.Length - i, set.Count);

                //
                // Contains
                //

                Assert.IsFalse(set.Contains(obj));

                foreach (var exp in objs.Except(remObjs.Take(i + 1)))
                {
                    Assert.IsTrue(set.Contains(exp));
                }

                //
                // IEnumerable
                //

                var test = new HashSet<object>(objs.Concat(new object[] { null }));
                test.ExceptWith(remObjs.Take(i + 1));
                Assert.IsTrue(new HashSet<object>(set).SetEquals(test));

                //
                // TrimExcess
                //

                if (i == remObjs.Length / 2)
                {
                    set.TrimExcess();
                }
            }

            //
            // Null - Remove
            //

            Assert.IsTrue(set.Remove(item: null));
            Assert.IsFalse(set.Remove(item: null));

            //
            // Null - Count
            //

            Assert.AreEqual(0, set.Count);

            //
            // Clear
            //

            set.Clear();

            //
            // Clear - Count
            //

            Assert.AreEqual(0, set.Count);

            //
            // TrimExcess
            //

            set.TrimExcess();

            //
            // TrimExcess - Count
            //

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ObjectSet(collection: null));
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_Empty()
        {
            var set = new ObjectSet(Array.Empty<object>());

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_Array()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });

            Assert.AreEqual(2, set.Count);

            Assert.IsTrue(set.Contains(o1));
            Assert.IsTrue(set.Contains(o2));
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_Enumerable()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 }.Select(x => x));

            Assert.AreEqual(2, set.Count);

            Assert.IsTrue(set.Contains(o1));
            Assert.IsTrue(set.Contains(o2));
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_ObjectSet_Empty()
        {
            var set1 = new ObjectSet();
            var set2 = new ObjectSet(set1);

            Assert.AreEqual(0, set2.Count);
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_ObjectSet_NonEmpty1()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(set1);

            Assert.AreEqual(2, set2.Count);

            Assert.IsTrue(set2.Contains(o1));
            Assert.IsTrue(set2.Contains(o2));
        }

        [TestMethod]
        public void ObjectSet_Construct_With_Collection_ObjectSet_NonEmpty2()
        {
            //
            // NB: This tests another code path in CopyFrom where elements are copied
            //     from a set with high capacity but low count (i.e. sparse).
            //

            var objs = new object[10];

            for (var i = 0; i < objs.Length; i++)
            {
                objs[i] = new object();
            }

            var set1 = new ObjectSet();

            for (var i = 0; i < objs.Length; i++)
            {
                set1.Add(objs[i]);
            }

            set1.Clear();

            set1.Add(objs[0]);
            set1.Add(objs[1]);

            var set2 = new ObjectSet(set1);

            Assert.AreEqual(2, set2.Count);

            Assert.IsTrue(set2.Contains(objs[0]));
            Assert.IsTrue(set2.Contains(objs[1]));
        }

        [TestMethod]
        public void ObjectSet_UnionWith()
        {
            var set = new ObjectSet();

            Assert.ThrowsException<ArgumentNullException>(() => set.UnionWith(other: null));

            var objs = Enumerable.Range(0, 10).Select(_ => new object()).ToArray();

            set.UnionWith(objs);

            Assert.IsTrue(objs.All(obj => set.Contains(obj)));
        }

        [TestMethod]
        public void ObjectSet_IntersectWith()
        {
            var set = new ObjectSet();

            Assert.ThrowsException<ArgumentNullException>(() => set.IntersectWith(other: null));

            set.IntersectWith(Array.Empty<object>());

            Assert.AreEqual(0, set.Count);

            var objs = Enumerable.Range(0, 10).Select(_ => new object()).ToArray();

            set.IntersectWith(objs);

            Assert.AreEqual(0, set.Count);

            for (int i = 0; i < objs.Length; i++)
            {
                if (i % 2 == 0)
                {
                    set.Add(objs[i]);
                }
            }

            Assert.AreEqual(objs.Length / 2, set.Count);

            set.IntersectWith(objs);

            Assert.AreEqual(objs.Length / 2, set.Count);

            set.IntersectWith(new ObjectSet(objs));

            Assert.AreEqual(objs.Length / 2, set.Count);

            set.IntersectWith(set);

            Assert.AreEqual(objs.Length / 2, set.Count);

            set.IntersectWith(objs.Take(objs.Length - 2));

            Assert.AreEqual(objs.Length / 2 - 1, set.Count);

            set.IntersectWith(Array.Empty<object>());

            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        public void ObjectSet_CopyTo_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ObjectSet().CopyTo(null));
            Assert.ThrowsException<ArgumentNullException>(() => new ObjectSet().CopyTo(null, 0));
            Assert.ThrowsException<ArgumentNullException>(() => new ObjectSet().CopyTo(null, 0, 1));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ObjectSet().CopyTo(new object[8], -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ObjectSet().CopyTo(new object[8], -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ObjectSet().CopyTo(new object[8], 0, -1));

            Assert.ThrowsException<ArgumentException>(() => new ObjectSet().CopyTo(new object[8], 8, 1));
            Assert.ThrowsException<ArgumentException>(() => new ObjectSet().CopyTo(new object[8], 0, 9));
        }

        [TestMethod]
        public void ObjectSet_CopyTo1()
        {
            const int N = 10;

            var set = new ObjectSet();

            for (var i = 0; i < N; i++)
            {
                set.Add(new object());
            }

            var arr = new object[N];

            set.CopyTo(arr);

            CollectionAssert.AreEquivalent(set.ToList(), arr);
        }

        [TestMethod]
        public void ObjectSet_CopyTo2()
        {
            const int N = 10;

            var set = new ObjectSet();

            for (var i = 0; i < N; i++)
            {
                set.Add(new object());
            }

            const int P = 2;

            var arr = new object[N + 2 * P];

            set.CopyTo(arr, P);

            CollectionAssert.AreEquivalent(set.ToList(), arr.Skip(P).Take(N).ToList());
        }

        [TestMethod]
        public void ObjectSet_CopyTo3()
        {
            const int N = 10;

            var set = new ObjectSet();

            for (var i = 0; i < N - 1; i++)
            {
                set.Add(new object());
            }

            set.Add(null);

            var arr = new object[N];

            set.CopyTo(arr);

            CollectionAssert.AreEquivalent(set.ToList(), arr);
        }

        [TestMethod]
        public void ObjectSet_Enumerate_Pattern()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });
            var res = new ObjectSet();

            foreach (var o in set)
            {
                res.Add(o);
            }

            Assert.IsTrue(set.SetEquals(res));
        }

        [TestMethod]
        public void ObjectSet_Enumerate_Generic()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });
            var res = new ObjectSet();

            var genericSet = (IEnumerable<object>)set;

            foreach (var o in genericSet)
            {
                res.Add(o);
            }

            Assert.IsTrue(set.SetEquals(res));
        }

        [TestMethod]
        public void ObjectSet_Enumerate_NonGeneric()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });
            var res = new ObjectSet();

            var nonGenericSet = (IEnumerable)set;

            foreach (var o in nonGenericSet)
            {
                res.Add(o);
            }

            Assert.IsTrue(set.SetEquals(res));
        }

        [TestMethod]
        public void ObjectSet_Enumerate_NonGeneric_Manual()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });
            var res = new ObjectSet();

            var nonGenericSet = (IEnumerable)set;

            var e = nonGenericSet.GetEnumerator();

            Assert.ThrowsException<InvalidOperationException>(() => e.Current);

            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);

            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);

            Assert.IsFalse(e.MoveNext());
            Assert.ThrowsException<InvalidOperationException>(() => e.Current);

            Assert.IsTrue(set.SetEquals(res));

            res.Clear();
            e.Reset();

            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);

            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);

            Assert.IsFalse(e.MoveNext());
            Assert.ThrowsException<InvalidOperationException>(() => e.Current);

            Assert.IsTrue(set.SetEquals(res));
        }

        [TestMethod]
        public void ObjectSet_Enumerate_ModifyDuringEnumeration()
        {
            var o1 = new object();
            var o2 = new object();

            var set = new ObjectSet(new[] { o1, o2 });

            using var e = set.GetEnumerator();

            Assert.IsTrue(e.MoveNext());

            set.Add(new object());

            Assert.ThrowsException<InvalidOperationException>(() => e.MoveNext());
            Assert.ThrowsException<InvalidOperationException>(() => ((IEnumerator)e).Reset());
        }

        [TestMethod]
        public void ObjectSet_SetEquals()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet(new[] { o1, o2, new object() });
            var set4 = new ObjectSet();
            var set5 = new ObjectSet(new[] { o1, new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.SetEquals(null));

            AssertEquals(set1, set1);

            AssertEquals(set1, set2);
            AssertEquals(set2, set1);

            AssertNotEquals(set1, set3);
            AssertNotEquals(set3, set1);
            AssertNotEquals(set1, set4);
            AssertNotEquals(set4, set1);
            AssertNotEquals(set1, set5);
            AssertNotEquals(set5, set1);

            static void AssertEquals(ObjectSet set, ObjectSet other) => AssertEquality(set, other, Assert.IsTrue);
            static void AssertNotEquals(ObjectSet set, ObjectSet other) => AssertEquality(set, other, Assert.IsFalse);

            static void AssertEquality(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.SetEquals(other));
                assert(other.SetEquals(set));

                assert(set.SetEquals(other.ToList()));
                assert(other.SetEquals(set.ToList()));

                assert(set.SetEquals(other.Select(x => x)));
                assert(other.SetEquals(set.Select(x => x)));
            }
        }

        [TestMethod]
        public void ObjectSet_IsSubsetOf()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet();
            var set4 = new ObjectSet(new[] { o1 });
            var set5 = new ObjectSet(new[] { o1, o2, new object() });
            var set6 = new ObjectSet(new[] { o1, new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.IsSubsetOf(null));

            AssertIsSubsetOf(set1, set1);

            AssertIsSubsetOf(set1, set2);
            AssertIsSubsetOf(set2, set1);

            AssertIsSubsetOf(set1, set5);

            AssertIsSubsetOf(set3, set3);
            AssertIsSubsetOf(set3, set1);
            AssertIsSubsetOf(set3, set4);

            AssertIsNotSubsetOf(set1, set3);
            AssertIsNotSubsetOf(set1, set4);
            AssertIsNotSubsetOf(set5, set1);
            AssertIsNotSubsetOf(set1, set6);

            static void AssertIsSubsetOf(ObjectSet set, ObjectSet other) => AssertSubsetOf(set, other, Assert.IsTrue);
            static void AssertIsNotSubsetOf(ObjectSet set, ObjectSet other) => AssertSubsetOf(set, other, Assert.IsFalse);

            static void AssertSubsetOf(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.IsSubsetOf(other));
                assert(set.IsSubsetOf(other.ToList()));
                assert(set.IsSubsetOf(other.Select(x => x)));
            }
        }

        [TestMethod]
        public void ObjectSet_IsProperSubsetOf()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet();
            var set4 = new ObjectSet(new[] { o1 });
            var set5 = new ObjectSet(new[] { o1, o2, new object() });
            var set6 = new ObjectSet(new[] { o1, new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.IsProperSubsetOf(null));

            AssertIsNotProperSubsetOf(set1, set1);

            AssertIsNotProperSubsetOf(set1, set2);
            AssertIsNotProperSubsetOf(set2, set1);

            AssertIsProperSubsetOf(set1, set5);

            AssertIsNotProperSubsetOf(set3, set3);
            AssertIsProperSubsetOf(set3, set1);
            AssertIsProperSubsetOf(set3, set4);

            AssertIsNotProperSubsetOf(set1, set3);
            AssertIsNotProperSubsetOf(set1, set4);
            AssertIsNotProperSubsetOf(set5, set1);
            AssertIsNotProperSubsetOf(set1, set6);

            static void AssertIsProperSubsetOf(ObjectSet set, ObjectSet other) => AssertProperSubsetOf(set, other, Assert.IsTrue);
            static void AssertIsNotProperSubsetOf(ObjectSet set, ObjectSet other) => AssertProperSubsetOf(set, other, Assert.IsFalse);

            static void AssertProperSubsetOf(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.IsProperSubsetOf(other));
                assert(set.IsProperSubsetOf(other.ToList()));
                assert(set.IsProperSubsetOf(other.Select(x => x)));
            }
        }

        [TestMethod]
        public void ObjectSet_IsSupersetOf()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet();
            var set4 = new ObjectSet(new[] { o1 });
            var set5 = new ObjectSet(new[] { o1, o2, new object() });
            var set6 = new ObjectSet(new[] { o1, new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.IsSupersetOf(null));

            AssertIsSupersetOf(set1, set1);

            AssertIsSupersetOf(set1, set2);
            AssertIsSupersetOf(set2, set1);

            AssertIsNotSupersetOf(set1, set5);

            AssertIsSupersetOf(set3, set3);
            AssertIsNotSupersetOf(set3, set1);
            AssertIsNotSupersetOf(set3, set4);

            AssertIsSupersetOf(set1, set3);
            AssertIsSupersetOf(set1, set4);
            AssertIsSupersetOf(set5, set1);
            AssertIsNotSupersetOf(set1, set6);

            static void AssertIsSupersetOf(ObjectSet set, ObjectSet other) => AssertSupersetOf(set, other, Assert.IsTrue);
            static void AssertIsNotSupersetOf(ObjectSet set, ObjectSet other) => AssertSupersetOf(set, other, Assert.IsFalse);

            static void AssertSupersetOf(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.IsSupersetOf(other));
                assert(set.IsSupersetOf(other.ToList()));
                assert(set.IsSupersetOf(other.Select(x => x)));
            }
        }

        [TestMethod]
        public void ObjectSet_IsProperSupersetOf()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet();
            var set4 = new ObjectSet(new[] { o1 });
            var set5 = new ObjectSet(new[] { o1, o2, new object() });
            var set6 = new ObjectSet(new[] { o1, new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.IsProperSupersetOf(null));

            AssertIsNotProperSupersetOf(set1, set1);

            AssertIsNotProperSupersetOf(set1, set2);
            AssertIsNotProperSupersetOf(set2, set1);

            AssertIsNotProperSupersetOf(set1, set5);

            AssertIsNotProperSupersetOf(set3, set3);
            AssertIsNotProperSupersetOf(set3, set1);
            AssertIsNotProperSupersetOf(set3, set4);

            AssertIsProperSupersetOf(set1, set3);
            AssertIsProperSupersetOf(set1, set4);
            AssertIsProperSupersetOf(set5, set1);
            AssertIsNotProperSupersetOf(set1, set6);

            static void AssertIsProperSupersetOf(ObjectSet set, ObjectSet other) => AssertProperSupersetOf(set, other, Assert.IsTrue);
            static void AssertIsNotProperSupersetOf(ObjectSet set, ObjectSet other) => AssertProperSupersetOf(set, other, Assert.IsFalse);

            static void AssertProperSupersetOf(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.IsProperSupersetOf(other));
                assert(set.IsProperSupersetOf(other.ToList()));
                assert(set.IsProperSupersetOf(other.Select(x => x)));
            }
        }

        [TestMethod]
        public void ObjectSet_Overlaps()
        {
            var o1 = new object();
            var o2 = new object();

            var set1 = new ObjectSet(new[] { o1, o2 });
            var set2 = new ObjectSet(new[] { o1, o2 });
            var set3 = new ObjectSet();
            var set4 = new ObjectSet(new[] { o1 });
            var set5 = new ObjectSet(new[] { o1, o2, new object() });
            var set6 = new ObjectSet(new[] { o1, new object() });
            var set7 = new ObjectSet(new[] { new object() });

            Assert.ThrowsException<ArgumentNullException>(() => set1.Overlaps(null));

            AssertOverlaps(set1, set1);
            AssertOverlaps(set1, set2);
            AssertOverlaps(set1, set4);
            AssertOverlaps(set1, set5);
            AssertOverlaps(set1, set6);

            AssertDoesNotOverlap(set1, set3);
            AssertDoesNotOverlap(set1, set7);

            static void AssertOverlaps(ObjectSet set, ObjectSet other) => AssertOverlapsCore(set, other, Assert.IsTrue);
            static void AssertDoesNotOverlap(ObjectSet set, ObjectSet other) => AssertOverlapsCore(set, other, Assert.IsFalse);

            static void AssertOverlapsCore(ObjectSet set, ObjectSet other, Action<bool> assert)
            {
                assert(set.Overlaps(other));
                assert(set.Overlaps(other.ToList()));
                assert(set.Overlaps(other.Select(x => x)));

                assert(other.Overlaps(set));
                assert(other.Overlaps(set.ToList()));
                assert(other.Overlaps(set.Select(x => x)));
            }
        }
    }
}
