// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        public void ObjectSet_Construct_With_Collection_ObjectSet_NonEmpty()
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
    }
}
