// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Wrote these tests.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Memory
{
    [TestClass]
    public partial class CacheTests
    {
        [TestMethod]
        public void Cache_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new Cache<string>(storage: null), ex => Assert.AreEqual("storage", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache(innerCache: null), ex => Assert.AreEqual("innerCache", ex.ParamName));
        }

        [TestMethod]
        public void Cache_Null()
        {
            Assert.IsNull(new Cache<string>().Create(value: null).Value);
            Assert.IsNull(new TestCache().Create(value: null).Value);
            new TestCache().Create(value: null).Dispose(); // Does not throw
        }

        [TestMethod]
        public void Cache_CheckShared()
        {
            var cache = new TestCache();
            var shared = "The quick brown fox jumped over the lazy dog.";
            var unshared1 = new object();
            var unshared2 = new object();

            var foo1 = new Foo { Shared = shared, Unshared = unshared1 };
            var foo2 = new Foo { Shared = Copy(shared), Unshared = unshared2 };

            var ref1 = cache.Create(foo1);
            var ref2 = cache.Create(foo2);

            var rc1 = ref1.Value;
            var rc2 = ref2.Value;

            Assert.AreNotSame(foo1, rc1);
            Assert.AreSame(foo1.Shared, rc1.Shared);
            Assert.AreSame(shared, rc1.Shared);
            Assert.AreSame(unshared1, rc1.Unshared);

            Assert.AreNotSame(foo2, rc2);
            Assert.AreNotSame(foo2.Shared, rc2.Shared);
            Assert.AreSame(shared, rc2.Shared);
            Assert.AreSame(unshared2, rc2.Unshared);
        }

        [TestMethod]
        public void Cache_GarbageCollected()
        {
            var cache = new TestCache();
            var shared = "The quick brown fox jumped over the lazy dog.";

            DoFirst(cache, shared);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            DoSecond(cache, shared);

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            static void DoFirst(TestCache cache, string shared)
            {
                var foo1 = new Foo { Shared = shared };

                var rt1 = RoundtripCache(cache, foo1);
                Assert.AreNotSame(foo1, rt1);
                Assert.AreSame(shared, rt1.Shared);
                Assert.AreSame(foo1.Shared, rt1.Shared);
            }

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            static void DoSecond(TestCache cache, string shared)
            {
                var foo2 = new Foo { Shared = Copy(shared) };

                var rt2 = RoundtripCache(cache, foo2);
                Assert.AreNotSame(foo2, rt2);
                Assert.AreNotSame(shared, rt2.Shared);
                Assert.AreSame(foo2.Shared, rt2.Shared);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Foo RoundtripCache(TestCache cache, Foo item)
        {
            return cache.Create(item).Value;
        }

        [TestMethod]
        public void Cache_Gauntlet_RemoveByFinalizer()
        {
            var uniqueCount = 128;
            var rootedCount = 16;
            var repeat = 10;

            var cache = new TestCache(new Cache<string>());
            var strings = Enumerable.Range(1, uniqueCount).Select(x => "string" + x).ToArray();
            var refs = new IDiscardable<Foo>[rootedCount];
            var rand = new Random(17);
            var tests = Enumerable.Repeat(strings, rootedCount * repeat).SelectMany(ss => ss).Select(s => new Foo { Shared = Copy(s) }).OrderBy(_ => rand.Next());

            var count = 0;
            Parallel.ForEach(
                tests,
                foo =>
                {
                    var r = cache.Create(foo);
                    refs[Interlocked.Increment(ref count) % rootedCount] = r;
                    Assert.AreEqual(foo.Shared, r.Value.Shared);
                }
            );
        }

        [TestMethod]
        public void Cache_AddWhileReleasing()
        {
            var repeat = 1000;

            var cache = new Cache<string>();
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
        public void Cache_GetValueAfterDispose()
        {
            var cache = new Cache<string>();
            var str = "foo";
            var ref1 = cache.Create(str);
            ref1.Dispose();
            var unused = default(string);
            Assert.ThrowsException<ObjectDisposedException>(() => unused = ref1.Value);

            var testCache = new TestCache();
            var foo = new Foo { Shared = "shared" };
            var ref2 = testCache.Create(foo);
            ref2.Dispose();
            var unused2 = default(Foo);
            Assert.ThrowsException<ObjectDisposedException>(() => unused2 = ref2.Value);
        }

        [TestMethod]
        public void Cache_NullSharedValue()
        {
            var testCache = new TestCache();
            var foo = new Foo();
            var ref2 = testCache.Create(foo);
            ref2.Dispose();
            var unused2 = default(Foo);
            Assert.ThrowsException<ObjectDisposedException>(() => unused2 = ref2.Value);
        }

        [TestMethod]
        public void Cache_StorageReturnsNull_ThrowsInvalidOperation()
        {
            var cache = new Cache<string>(new BadStorage<string>());
            Assert.ThrowsException<InvalidOperationException>(() => cache.Create("foo"));

            var testCache = new TestCache(cache);
            Assert.ThrowsException<InvalidOperationException>(() => testCache.Create(new Foo { Shared = "foo" }));
        }

        private sealed class FaultyComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => false;

            public int GetHashCode(string obj) => obj.GetHashCode();
        }

        [TestMethod]
        public void Cache_TestOfTests()
        {
            var str = "foo";
            var copy1 = Copy(str);
            var copy2 = Copy(str);

            Assert.AreSame(str, str);
            Assert.AreNotSame(str, copy1);
            Assert.AreNotSame(str, copy2);
            Assert.AreNotSame(copy1, copy2);
        }

        [TestMethod]
        public void Cache_CollectAfterRemove()
        {
            var storage = new LruCacheStorage<string>(1);
            var e1 = storage.GetEntry("foo"); // "foo" in cache
            var e2 = storage.GetEntry("bar"); // "bar" in cache
            var e3 = storage.GetEntry(Copy("foo")); // different "foo" in cache
            Assert.AreNotSame(e1, e3);

            storage.ReleaseEntry(e1); // If cache entries were not checked for reference equality,
            storage.ReleaseEntry(e2); // the release operation on `e1` would decrement the count
            storage.ReleaseEntry(e3); // and this final release operation would fail an assert.
        }

        [TestMethod]
        public void Cache_Dummy()
        {
            var dummyCache = new DummyCache<string>();
            var cache = new TestCache(dummyCache);

            var foo = new Foo { Shared = "Bar", Unshared = new object() };
            var ref1 = cache.Create(foo);
            Assert.AreEqual(foo.Shared, ref1.Value.Shared);
        }

        private static string Copy(string value)
        {
            var sb = new StringBuilder();
            sb.Append(value);
            return sb.ToString();
        }

        private sealed class Foo
        {
            public string Shared;
            public object Unshared;
        }

        private sealed class TestCache : Cache<Foo, string, object>
        {
            public TestCache() { }

            public TestCache(ICache<string> innerCache)
                : base(innerCache)
            {
            }

            protected override Deconstructed<string, object> Deconstruct(Foo item)
            {
                return Deconstructed.Create(item.Shared, item.Unshared);
            }

            protected override Foo Reconstruct(Deconstructed<string, object> deconstructed)
            {
                return new Foo { Shared = deconstructed.Cached, Unshared = deconstructed.NonCached };
            }
        }

        private sealed class TestComparer<T> : IEqualityComparer<T>
        {
            public int EqualsCalls;
            public int GetHashCodeCalls;

            public bool Equals(T x, T y)
            {
                EqualsCalls++;
                return EqualityComparer<T>.Default.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                GetHashCodeCalls++;
                return EqualityComparer<T>.Default.GetHashCode(obj);
            }
        }

        private sealed class DummyCache<T> : ICache<T>
        {
            public IDiscardable<T> Create(T value) => new Ref(value);

            private sealed class Ref : IDiscardable<T>
            {
                public Ref(T value) => Value = value;

                public T Value { get; }

                public void Dispose() { }
            }
        }

        private sealed class BadStorage<T> : ICacheStorage<T>
        {
            public int Count => 0;

            public IReference<T> GetEntry(T value) => null;

            public void ReleaseEntry(IReference<T> entry) { }
        }
    }
}
