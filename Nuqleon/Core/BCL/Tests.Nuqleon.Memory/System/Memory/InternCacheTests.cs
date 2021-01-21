// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/04/2015 - Adding intern caches.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Memory;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class InternCacheTests
    {
        [TestMethod]
        public void InternCache_Create_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => InternCache.CreateInternCache<string>(cacheFactory: null));
        }

        [TestMethod]
        public void InternCache_Create_Simple1()
        {
            var cache = InternCache.CreateInternCache<string>(MemoizationCacheFactory.Unbounded);

            var arg1 = "bar".ToUpper();
            var res1 = cache.Intern(arg1);
            Assert.AreEqual("BAR", res1);

            Assert.AreSame(arg1, res1);

            var arg2 = "bar".ToUpper();
            var res2 = cache.Intern(arg2);
            Assert.AreEqual("BAR", res2);

            Assert.AreNotSame(arg2, res2);
            Assert.AreSame(res1, res2);

            Assert.IsFalse(string.IsNullOrEmpty(cache.DebugView));
        }

        [TestMethod]
        public void InternCache_Create_Simple2()
        {
            var cache = InternCache.CreateInternCache<string>(MemoizationCacheFactory.Unbounded);

            for (var n = 0; n < 2; n++)
            {
                for (var i = 0; i < 100; i++)
                {
                    Assert.AreEqual("BAR", cache.Intern("bar".ToUpper()));
                }

                Assert.AreEqual(1, cache.Count);

                cache.Clear();
                Assert.AreEqual(0, cache.Count);
            }
        }

        [TestMethod]
        public void InternCache_Create_Dispose()
        {
            var cache = InternCache.CreateInternCache<string>(MemoizationCacheFactory.Unbounded);

            cache.Intern("bar".ToUpper());
            cache.Intern("bar".ToUpper());

            cache.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => cache.Intern("bar".ToUpper()));
        }

        [TestMethod]
        public void InternCache_Create_Service()
        {
            var cache = InternCache.CreateInternCache<string>(MemoizationCacheFactory.Unbounded);

            var trim = cache.GetService<ITrimmable<KeyValuePair<string, string>>>();
            Assert.IsNotNull(trim);

            cache.Intern("bar".ToUpper());

            Assert.AreEqual(1, cache.Count);

            trim.Trim(kv => kv.Key == "BAR");

            Assert.AreEqual(0, cache.Count);
        }

        [TestMethod]
        public void InternCache_CreateWeak_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => InternCache.CreateWeakInternCache<string>(default(IMemoizationCacheFactory), Copy));
            Assert.ThrowsException<ArgumentNullException>(() => InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, default(Func<string, string>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void InternCache_CreateWeak_BadClone()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, s => s);

            Assert.ThrowsException<InvalidOperationException>(() => cache.Intern("foo"));
        }

        [TestMethod]
        public void InternCache_CreateWeak_Simple1()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, Copy);

            var arg1 = "bar".ToUpper();
            var res1 = cache.Intern(arg1);
            Assert.AreEqual("BAR", res1);

            Assert.AreNotSame(arg1, res1); // cloning behavior in the value space

            var arg2 = "bar".ToUpper();
            var res2 = cache.Intern(arg2);
            Assert.AreEqual("BAR", res2);

            Assert.AreNotSame(arg2, res2);
            Assert.AreSame(res1, res2);

            Assert.IsFalse(string.IsNullOrEmpty(cache.DebugView));
        }

        [TestMethod]
        public void InternCache_CreateWeak_Simple2()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, Copy);

            for (var n = 0; n < 2; n++)
            {
                for (var i = 0; i < 100; i++)
                {
                    Assert.AreEqual("BAR", cache.Intern("bar".ToUpper()));
                }

                Assert.AreEqual(1, cache.Count);

                cache.Clear();
                Assert.AreEqual(0, cache.Count);
            }
        }

        [TestMethod]
        public void InternCache_CreateWeak_Dispose()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, Copy);

            cache.Intern("bar".ToUpper());
            cache.Intern("bar".ToUpper());

            cache.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => cache.Intern("bar".ToUpper()));
        }

        [TestMethod]
        public void InternCache_CreateWeak_Trim()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Unbounded, Copy);

            var set = new HashSet<string>();

            for (var n = 0; n < 2; n++)
            {
                InternSome(cache, set); // Ensures lifetime of locals is not extended in DEBUG mode

                Assert.AreEqual(1, cache.Count);

                cache.Trim();
                Assert.AreEqual(1, cache.Count);

                set.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                InternSome(cache, set); // Resurrects weak references

                Assert.AreEqual(1, cache.Count);

                cache.Trim();
                Assert.AreEqual(1, cache.Count);

                set.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                cache.Trim();
                Assert.AreEqual(0, cache.Count);
            }
        }

        [TestMethod]
        public void InternCache_CreateWeak_Trim_NotSupported()
        {
            var cache = InternCache.CreateWeakInternCache<string>(MemoizationCacheFactory.Nop, Copy);

            Assert.ThrowsException<NotSupportedException>(() => cache.Trim());
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void InternSome(IWeakInternCache<string> cache, HashSet<string> set)
        {
            for (var i = 0; i < 100; i++)
            {
                var res = cache.Intern("bar".ToUpper());
                set.Add(res);

                Assert.AreEqual("BAR", res);
            }
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable CS0618 // string.Copy(string) is obsolete
        private static string Copy(string s) => string.Copy(s);
#pragma warning restore CS0618
#pragma warning restore IDE0079
    }
}
