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
            var set = new ObjectSet<object>();

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

                Assert.IsTrue(set.Add(obj));
                Assert.IsFalse(set.Add(obj));

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
        }
    }
}
