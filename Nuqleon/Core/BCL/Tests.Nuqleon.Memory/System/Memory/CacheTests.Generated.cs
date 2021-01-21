// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Memory;

namespace Tests.System.Memory
{
    public partial class CacheTests
    {
        [TestMethod]
        public void Cache_2SharedComponents_ArgumentChecks()
        {
            var innerCache = new Cache<string>();
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache2(innerCache1: null, innerCache), ex => Assert.AreEqual("innerCache1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache2(innerCache, innerCache2: null), ex => Assert.AreEqual("innerCache2", ex.ParamName));
        }

        [TestMethod]
        public void Cache_2SharedComponents_Dummies()
        {
            var dummyCache = new DummyCache<string>();
            var cache = new TestCache2(dummyCache, dummyCache);

            var s = new[] { "s1", "s2", "s3", "s4" };
            var ref1 = cache.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref1.Value));
        }

        [TestMethod]
        public void Cache_2SharedComponents()
        {
            var cache1 = new TestCache2();

            Assert.IsNull(cache1.Create(value: null).Value);

            var s = new[] { "s1", "s2", "s3", "s4" };
            
            var ref0 = cache1.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref0.Value));
        }

        [TestMethod]
        public void Cache_2SharedComponents_GetValueAfterDispose()
        {
            var cache = new TestCache2();
            var s = new[] { "s1", "s2", "s3", "s4" };
            var ref0 = cache.Create(s);
            ref0.Dispose();
            var unused = default(string[]);
            Assert.ThrowsException<ObjectDisposedException>(() => unused = ref0.Value);
        }

        private sealed class TestCache2 : Cache<string[], string, string, string[]>
        {
            public TestCache2() { }

            public TestCache2(ICache<string> innerCache1, ICache<string> innerCache2)
                : base(innerCache1, innerCache2)
            {
            }

            protected override Deconstructed<string, string, string[]> Deconstruct(string[] item)
            {
                var rem = new string[item.Length - 2];
                Array.Copy(item, 2, rem, 0, item.Length - 2);
                return Deconstructed.Create(item[0], item[1], rem);
            }

            protected override string[] Reconstruct(Deconstructed<string, string, string[]> deconstructed)
            {
                var item = new string[deconstructed.NonCached.Length + 2];
                item[0] = deconstructed.Cached1;
                item[1] = deconstructed.Cached2;
                Array.Copy(deconstructed.NonCached, 0, item, 2, deconstructed.NonCached.Length);
                return item;
            }
        }

        [TestMethod]
        public void Cache_3SharedComponents_ArgumentChecks()
        {
            var innerCache = new Cache<string>();
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache3(innerCache1: null, innerCache, innerCache), ex => Assert.AreEqual("innerCache1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache3(innerCache, innerCache2: null, innerCache), ex => Assert.AreEqual("innerCache2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache3(innerCache, innerCache, innerCache3: null), ex => Assert.AreEqual("innerCache3", ex.ParamName));
        }

        [TestMethod]
        public void Cache_3SharedComponents_Dummies()
        {
            var dummyCache = new DummyCache<string>();
            var cache = new TestCache3(dummyCache, dummyCache, dummyCache);

            var s = new[] { "s1", "s2", "s3", "s4", "s5" };
            var ref1 = cache.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref1.Value));
        }

        [TestMethod]
        public void Cache_3SharedComponents()
        {
            var cache1 = new TestCache3();

            Assert.IsNull(cache1.Create(value: null).Value);

            var s = new[] { "s1", "s2", "s3", "s4", "s5" };
            
            var ref0 = cache1.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref0.Value));
        }

        [TestMethod]
        public void Cache_3SharedComponents_GetValueAfterDispose()
        {
            var cache = new TestCache3();
            var s = new[] { "s1", "s2", "s3", "s4", "s5" };
            var ref0 = cache.Create(s);
            ref0.Dispose();
            var unused = default(string[]);
            Assert.ThrowsException<ObjectDisposedException>(() => unused = ref0.Value);
        }

        private sealed class TestCache3 : Cache<string[], string, string, string, string[]>
        {
            public TestCache3() { }

            public TestCache3(ICache<string> innerCache1, ICache<string> innerCache2, ICache<string> innerCache3)
                : base(innerCache1, innerCache2, innerCache3)
            {
            }

            protected override Deconstructed<string, string, string, string[]> Deconstruct(string[] item)
            {
                var rem = new string[item.Length - 3];
                Array.Copy(item, 3, rem, 0, item.Length - 3);
                return Deconstructed.Create(item[0], item[1], item[2], rem);
            }

            protected override string[] Reconstruct(Deconstructed<string, string, string, string[]> deconstructed)
            {
                var item = new string[deconstructed.NonCached.Length + 3];
                item[0] = deconstructed.Cached1;
                item[1] = deconstructed.Cached2;
                item[2] = deconstructed.Cached3;
                Array.Copy(deconstructed.NonCached, 0, item, 3, deconstructed.NonCached.Length);
                return item;
            }
        }

        [TestMethod]
        public void Cache_4SharedComponents_ArgumentChecks()
        {
            var innerCache = new Cache<string>();
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache4(innerCache1: null, innerCache, innerCache, innerCache), ex => Assert.AreEqual("innerCache1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache4(innerCache, innerCache2: null, innerCache, innerCache), ex => Assert.AreEqual("innerCache2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache4(innerCache, innerCache, innerCache3: null, innerCache), ex => Assert.AreEqual("innerCache3", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TestCache4(innerCache, innerCache, innerCache, innerCache4: null), ex => Assert.AreEqual("innerCache4", ex.ParamName));
        }

        [TestMethod]
        public void Cache_4SharedComponents_Dummies()
        {
            var dummyCache = new DummyCache<string>();
            var cache = new TestCache4(dummyCache, dummyCache, dummyCache, dummyCache);

            var s = new[] { "s1", "s2", "s3", "s4", "s5", "s6" };
            var ref1 = cache.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref1.Value));
        }

        [TestMethod]
        public void Cache_4SharedComponents()
        {
            var cache1 = new TestCache4();

            Assert.IsNull(cache1.Create(value: null).Value);

            var s = new[] { "s1", "s2", "s3", "s4", "s5", "s6" };
            
            var ref0 = cache1.Create(s);
            Assert.IsTrue(s.SequenceEqual(ref0.Value));
        }

        [TestMethod]
        public void Cache_4SharedComponents_GetValueAfterDispose()
        {
            var cache = new TestCache4();
            var s = new[] { "s1", "s2", "s3", "s4", "s5", "s6" };
            var ref0 = cache.Create(s);
            ref0.Dispose();
            var unused = default(string[]);
            Assert.ThrowsException<ObjectDisposedException>(() => unused = ref0.Value);
        }

        private sealed class TestCache4 : Cache<string[], string, string, string, string, string[]>
        {
            public TestCache4() { }

            public TestCache4(ICache<string> innerCache1, ICache<string> innerCache2, ICache<string> innerCache3, ICache<string> innerCache4)
                : base(innerCache1, innerCache2, innerCache3, innerCache4)
            {
            }

            protected override Deconstructed<string, string, string, string, string[]> Deconstruct(string[] item)
            {
                var rem = new string[item.Length - 4];
                Array.Copy(item, 4, rem, 0, item.Length - 4);
                return Deconstructed.Create(item[0], item[1], item[2], item[3], rem);
            }

            protected override string[] Reconstruct(Deconstructed<string, string, string, string, string[]> deconstructed)
            {
                var item = new string[deconstructed.NonCached.Length + 4];
                item[0] = deconstructed.Cached1;
                item[1] = deconstructed.Cached2;
                item[2] = deconstructed.Cached3;
                item[3] = deconstructed.Cached4;
                Array.Copy(deconstructed.NonCached, 0, item, 4, deconstructed.NonCached.Length);
                return item;
            }
        }

    }
}
