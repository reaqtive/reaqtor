// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Memory;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class WeakCacheDictionaryTests
    {
        [TestMethod]
        public void WeakCacheDictionary_Simple()
        {
            var cd = new WeakCacheDictionary<string, string>();

            var n = 0;

            Assert.AreEqual("FOO", cd.GetOrAdd("foo", s => { n++; return s.ToUpper(); }));
            Assert.AreEqual(1, n);

            Assert.AreEqual("FOO", cd.GetOrAdd("foo", s => { n++; return s.ToUpper(); }));
            Assert.AreEqual(1, n);

            Assert.AreEqual("BAR", cd.GetOrAdd("bar", s => { n++; return s.ToUpper(); }));
            Assert.AreEqual(2, n);

            Assert.IsTrue(cd.Remove("bar"));
            Assert.AreEqual(2, n);

            Assert.IsFalse(cd.Remove("qux"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("BAR", cd.GetOrAdd("bar", s => { n++; return s.ToUpper(); }));
            Assert.AreEqual(3, n);

#if DEBUG
            Assert.IsTrue(cd.Keys.OrderBy(x => x).SequenceEqual(new[] { "bar", "foo" }));
#endif
        }

        [TestMethod]
        public void WeakCacheDictionary_Null()
        {
            var cd = new WeakCacheDictionary<string, string>();

            var n = 0;

            Assert.AreEqual("NULL", cd.GetOrAdd(key: null, s => { n++; return (s ?? "null").ToUpper(); }));
            Assert.AreEqual(1, n);

            Assert.AreEqual("NULL", cd.GetOrAdd(key: null, s => { n++; return (s ?? "null").ToUpper(); }));
            Assert.AreEqual(1, n);

            Assert.IsTrue(cd.Remove(key: null));
            Assert.AreEqual(1, n);

            Assert.AreEqual("NULL", cd.GetOrAdd(key: null, s => { n++; return (s ?? "null").ToUpper(); }));
            Assert.AreEqual(2, n);

#if DEBUG
            Assert.IsTrue(cd.Keys.Any(x => x == null));
#endif
        }

        [TestMethod]
        public void WeakCacheDictionary_Finalizers()
        {
            var cd = new WeakCacheDictionary<Obj, string>();

            var initial = Obj.FinalizeCount;

            for (var i = 1; i <= 10; i++)
            {
                Assert.IsTrue(GetOrAdd(cd).Contains("Obj"));

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Assert.AreEqual(i, Obj.FinalizeCount - initial);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static string GetOrAdd(IWeakCacheDictionary<Obj, string> cache)
        {
            return cache.GetOrAdd(new Obj(), o => o.ToString());
        }

        private sealed class Obj
        {
            public static int FinalizeCount;

            ~Obj()
            {
                Interlocked.Increment(ref FinalizeCount);
            }
        }
    }
}
