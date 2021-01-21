// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Reaqtor.Shared.Core
{
    [TestClass]
    public class ReadOnlyChainedDictionaryTests
    {
        [TestMethod]
        public void ReadOnlyChainedDictionary_Contains()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.Contains(new KeyValuePair<string, int>("bar", 42)));
            Assert.IsTrue(ch.Contains(new KeyValuePair<string, int>("foo", 43)));
            Assert.IsTrue(ch.Contains(new KeyValuePair<string, int>("qux", 44)));

            Assert.IsFalse(ch.Contains(new KeyValuePair<string, int>("bar", 45)));
            Assert.IsFalse(ch.Contains(new KeyValuePair<string, int>("foo", 45)));
            Assert.IsFalse(ch.Contains(new KeyValuePair<string, int>("qux", 45)));
            Assert.IsFalse(ch.Contains(new KeyValuePair<string, int>("baz", 45)));
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_ContainsKey()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.ContainsKey("bar"));
            Assert.IsTrue(ch.ContainsKey("foo"));
            Assert.IsTrue(ch.ContainsKey("qux"));

            Assert.IsFalse(ch.ContainsKey("baz"));
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_Keys()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.Keys.OrderBy(x => x).SequenceEqual(new[] { "bar", "foo", "qux" }));
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_Values()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.Values.OrderBy(x => x).SequenceEqual(new[] { 42, 43, 44 }));
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_TryGetValue()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.TryGetValue("bar", out var res) && res == 42);
            Assert.IsTrue(ch.TryGetValue("foo", out res) && res == 43);
            Assert.IsTrue(ch.TryGetValue("qux", out res) && res == 44);
            Assert.IsFalse(ch.TryGetValue("baz", out _));
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_Get()
        {
            var ch = GetChainedDictionary();

            Assert.AreEqual(42, ch["bar"]);
            Assert.AreEqual(43, ch["foo"]);
            Assert.AreEqual(44, ch["qux"]);

            Assert.ThrowsException<KeyNotFoundException>(() => ch["baz"]);
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_Count()
        {
            var ch = GetChainedDictionary();

            Assert.AreEqual(3, ch.Count);
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_IsReadOnly()
        {
            var ch = GetChainedDictionary();

            Assert.IsTrue(ch.IsReadOnly);
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_GetEnumerator_Generic()
        {
            var ch = GetChainedDictionary();

            var e = new Enumerable<KeyValuePair<string, int>>(ch.GetEnumerator());
            AssertEnumeration(e);
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_GetEnumerator_NonGeneric()
        {
            var ch = GetChainedDictionary();

            var e = new Enumerable(((IEnumerable)ch).GetEnumerator());
            AssertEnumeration(e.Cast<KeyValuePair<string, int>>());
        }

        private static void AssertEnumeration(IEnumerable<KeyValuePair<string, int>> e)
        {
            Assert.IsTrue(e.OrderBy(kv => kv.Key).SequenceEqual(new[]
            {
                new KeyValuePair<string, int>("bar", 42),
                new KeyValuePair<string, int>("foo", 43),
                new KeyValuePair<string, int>("qux", 44),
            }));
        }

        private sealed class Enumerable : IEnumerable
        {
            private readonly IEnumerator _e;

            public Enumerable(IEnumerator e)
            {
                _e = e;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _e;
            }
        }

        private sealed class Enumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> _e;

            public Enumerable(IEnumerator<T> e)
            {
                _e = e;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _e;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _e;
            }
        }

        [TestMethod]
        public void ReadOnlyChainedDictionary_NotSupported()
        {
            var ch = GetChainedDictionary();

            Assert.ThrowsException<NotImplementedException>(() => ch["baz"] = 45);
            Assert.ThrowsException<NotImplementedException>(() => ch.Add("baz", 45));
            Assert.ThrowsException<NotImplementedException>(() => ch.Add(new KeyValuePair<string, int>("baz", 45)));
            Assert.ThrowsException<NotImplementedException>(() => ch.Clear());
            Assert.ThrowsException<NotImplementedException>(() => ch.CopyTo(null, 0));
            Assert.ThrowsException<NotImplementedException>(() => ch.Remove("baz"));
            Assert.ThrowsException<NotImplementedException>(() => ch.Remove(new KeyValuePair<string, int>("baz", 45)));
        }

        private static IDictionary<string, int> GetChainedDictionary()
        {
            var d1 = new Dictionary<string, int>
            {
                { "bar", 42 },
                { "foo", 43 },
            };

            var d2 = new Dictionary<string, int>
            {
                { "qux", 44 },
                { "bar", 45 },
            };

            return new ReadOnlyChainedDictionary<string, int>(d1, d2);
        }
    }
}
