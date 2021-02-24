// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Wrote these tests.
//

using System;
using System.Linq;
using System.Memory;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Memory
{
    [TestClass]
    public class LruCacheStorageTests
    {
        [TestMethod]
        public void LruCacheStorage_ArgumentChecks()
        {
            var cacheStorage = new LruCacheStorage<string>(1);
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => new LruCacheStorage<string>(-1), ex => Assert.AreEqual("size", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new LruCacheStorage<string>(1, comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cacheStorage.GetEntry(value: null), ex => Assert.AreEqual("value", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cacheStorage.ReleaseEntry(entry: null), ex => Assert.AreEqual("entry", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => cacheStorage.ReleaseEntry(new NullEntry()), ex => Assert.AreEqual("entry", ex.ParamName));
        }

        [TestMethod]
        public void LruCacheStorage_PolicyWithMaxSize()
        {
            var cache = new Cache<string>(new LruCacheStorage<string>(2));
            var string1 = "string1";
            var string2 = "string2";
            var string3 = "string3";
            var string1Copy = Copy(string1);
            var string3Copy = Copy(string3);

            var ref1 = cache.Create(string1); // `string1` is cached
            var ref2 = cache.Create(string2); // `string2` is cached
            var ref3 = cache.Create(string3); // `string3` is cached, `string1` is ejected
            var ref4 = cache.Create(string1Copy); // `string1` is cached, `string2` is ejected
            var ref5 = cache.Create(string3Copy); // `string3` is re-used

            var rc1 = ref1.Value;
            var rc2 = ref2.Value;
            var rc3 = ref3.Value;
            var rc4 = ref4.Value;
            var rc5 = ref5.Value;

            Assert.AreSame(string1, rc1);

            Assert.AreSame(string2, rc2);

            Assert.AreSame(string3, rc3);

            Assert.AreNotSame(string1, rc4);
            Assert.AreSame(string1Copy, rc4);

            Assert.AreNotSame(string3Copy, rc5);
            Assert.AreSame(string3, rc5);
        }

        [TestMethod]
        public void LruCacheStorage_Gauntlet_RemoveByPolicy()
        {
            var cacheSize = 16;
            var uniqueCount = 32;
            var rootedCount = 256;
            var repeat = 10;

            var cache = new Cache<string>(new LruCacheStorage<string>(cacheSize));
            var strings = Enumerable.Range(1, uniqueCount).Select(x => "string" + x).ToArray();
            var refs = new IDiscardable<string>[rootedCount];
            var rand = new Random(17);
            var tests = Enumerable.Repeat(strings, rootedCount * repeat).SelectMany(ss => ss).Select(s => Copy(s)).OrderBy(_ => rand.Next());

            var count = 0;
            Parallel.ForEach(
                tests,
                test =>
                {
                    var r = cache.Create(test);
                    refs[Interlocked.Increment(ref count) % rootedCount] = r;
                    Assert.AreEqual(test, r.Value);
                }
            );
        }

        [TestMethod]
        public void LruCacheStorage_AddWhileReleasing()
        {
            var repeat = 1000;

            var cache = new Cache<string>(new LruCacheStorage<string>(2));
            for (var i = 0; i < repeat; ++i)
            {
                var foo = "foo";
                var copy = Copy(foo);
                Task.WaitAll(
                    Task.Factory.StartNew(() => Assert.AreEqual(foo, cache.Create(foo).Value)),
                    Task.Factory.StartNew(() => Assert.AreEqual(copy, cache.Create(copy).Value)),
                    Task.Factory.StartNew(() => GC.Collect())
                );
            }
        }

        [TestMethod]
        public void LruCacheStorage_Count()
        {
            var cacheStorage = new LruCacheStorage<string>(1);
            Assert.AreEqual(0, cacheStorage.Count);

            var e1 = cacheStorage.GetEntry("foo");
            var e2 = cacheStorage.GetEntry("bar");
            Assert.AreEqual(1, cacheStorage.Count);

            cacheStorage.ReleaseEntry(e1);
            Assert.AreEqual(1, cacheStorage.Count);

            cacheStorage.ReleaseEntry(e2);
            Assert.AreEqual(0, cacheStorage.Count);
        }

        [TestMethod]
        public void LruCacheStorage_Dispose()
        {
            var storage = new LruCacheStorage<int>(1);
            var entry = storage.GetEntry(42);
            storage.ReleaseEntry(entry);
            storage.Dispose();
            Assert.ThrowsException<ObjectDisposedException>(() => storage.GetEntry(42));
            storage.Dispose();
        }

        [TestMethod]
        public void LruCacheStorage_ReleaseAfterEviction()
        {
            var storage = new LruCacheStorage<string>(1);
            var e1 = storage.GetEntry("foo"); // "foo" is in cache
            var e2 = storage.GetEntry("bar"); // "bar" is in cache; "foo" is evicted
            var e3 = storage.GetEntry("foo"); // "foo" is in cache; "bar" is evicted
            var e4 = storage.GetEntry("foo"); // "foo" cache hit
            Assert.AreNotSame(e1, e3);
            Assert.AreSame(e3, e4);

            storage.ReleaseEntry(e1); // first "foo" is released, no-op (not in cache)
            storage.ReleaseEntry(e2); // "bar" is released, no-op
            storage.ReleaseEntry(e3); // "foo" is released, decremented

            var e5 = storage.GetEntry("foo");
            Assert.AreSame(e4, e5); // "foo" cache hit
            storage.ReleaseEntry(e4);
            storage.ReleaseEntry(e5);
        }

        private static string Copy(string value)
        {
            var sb = new StringBuilder();
            sb.Append(value);
            return sb.ToString();
        }

        private sealed class NullEntry : IReference<string>
        {
            public string Value => null;
        }
    }
}
