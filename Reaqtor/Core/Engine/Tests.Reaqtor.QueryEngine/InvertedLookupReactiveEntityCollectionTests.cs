// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class InvertedLookupReactiveEntityCollectionTests
    {
        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_ArgumentChecks()
        {
            var inner = new ReactiveEntityCollection<string, string>(StringComparer.Ordinal);
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new InvertedLookupReactiveEntityCollection<string, string>(default(IReactiveEntityCollection<string, string>), StringComparer.Ordinal), ex => Assert.AreEqual("collection", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new InvertedLookupReactiveEntityCollection<string, string>(inner, null), ex => Assert.AreEqual("valueComparer", ex.ParamName));
        }

        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_Simple()
        {
            var collection = new InvertedLookupReactiveEntityCollection<string, int>(
                new ReactiveEntityCollection<string, int>(StringComparer.Ordinal),
                EqualityComparer<int>.Default)
            {
                { "foo", 1 },
                { "bar", 2 }
            };

            Assert.IsTrue(collection.TryGetKey(1, out var key));
            Assert.AreEqual("foo", key);

            Assert.IsFalse(collection.TryGetKey(3, out key));

            Assert.IsTrue(collection.TryRemove("foo", out var value));
            Assert.AreEqual(1, value);

            Assert.IsFalse(collection.TryGetKey(1, out key));
            Assert.IsFalse(collection.TryGetValue("foo", out value));

            Assert.IsTrue(collection.ContainsKey("bar"));
            Assert.IsTrue(collection.TryGetValue("bar", out value));
            Assert.AreEqual(2, value);
            Assert.IsTrue(collection.TryGetKey(2, out key));
            Assert.AreEqual("bar", key);

            Assert.IsTrue(collection.RemovedKeys.SequenceEqual(new[] { "foo" }));
            collection.ClearRemovedKeys(new[] { "foo" });
            Assert.AreEqual(0, collection.RemovedKeys.Count());

            collection.Add("foo", 1);
            collection.Add("qux", 3);
            collection.Add("baz", 4);

            Assert.IsTrue(collection.Values.OrderBy(x => x).SequenceEqual(new[] { 1, 2, 3, 4 }));
            Assert.IsTrue(collection.OrderBy(kv => kv.Value)
                .SequenceEqual(new Dictionary<string, int> { { "foo", 1 }, { "bar", 2 }, { "qux", 3 }, { "baz", 4 } }.OrderBy(kv => kv.Value)));

            collection.Clear();
            Assert.AreEqual(0, collection.Count());
        }

        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_DuplicateKey()
        {
            var l = new AutoResetEvent(false);
            var collection = new TestCollection();

            Task.Run(() => { collection.Add("foo", 1); l.Set(); });
            collection.AddLock.Set();
            l.WaitOne();

            var key = default(string);
            for (var i = 0; i < 1000; ++i)
            {
                collection.AddLock.Set();
                Task.WaitAll(new[]
                {
                    Task.Run(() => Assert.IsFalse(collection.TryAdd("foo", 2))),
                    Task.Run(() => Assert.IsFalse(collection.TryGetKey(2, out key))),
                });
            }
        }

        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_DuplicateValue()
        {
            var l = new AutoResetEvent(false);
            var collection = new TestCollection();

            Task.Run(() => { collection.Add("foo", 42); l.Set(); });
            collection.AddLock.Set();
            l.WaitOne();

            var key = default(string);
            var value = default(int);
            for (var i = 0; i < 1000; ++i)
            {
                collection.AddLock.Set();
                Task.WaitAll(new[]
                {
                    Task.Run(() => Assert.IsFalse(collection.TryAdd("bar", 42))),
                    Task.Run(() => Assert.IsTrue(collection.TryGetKey(42, out key))),
                    Task.Run(() => Assert.IsFalse(collection.TryGetValue("bar", out value))),
                });
                Assert.AreEqual(key, "foo");
            }
        }

        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_LookupKeyWhileRemoving()
        {
            var l = new AutoResetEvent(false);
            var collection = new TestCollection();

            var key = default(string);
            var value = default(int);
            for (var i = 0; i < 1000; ++i)
            {
                collection.AddLock.Set();
                collection.Add("foo", 1);
                Task.Run(() => { Assert.IsTrue(collection.TryRemove("foo", out value)); l.Set(); });
                Assert.IsTrue(collection.TryGetKey(1, out key));
                Assert.AreEqual("foo", key);
                collection.RemoveLock.Set();
                l.WaitOne();
            }
        }

        [TestMethod]
        public void InvertedLookupReactiveEntityCollection_RemoveWhileAdding()
        {
            var l = new AutoResetEvent(false);
            var collection = new TestCollection();

            var key = default(string);
            var value = default(int);
            for (var i = 0; i < 1000; ++i)
            {
                Task.Run(() => { collection.Add("foo", 1); l.Set(); });
                collection.RemoveLock.Set();
                Assert.IsFalse(collection.TryRemove("foo", out value));
                collection.AddLock.Set();
                l.WaitOne();
                Assert.IsTrue(collection.TryGetKey(1, out key));
                Assert.AreEqual("foo", key);
                collection.RemoveLock.Set();
                Assert.IsTrue(collection.TryRemove("foo", out value));
                Assert.AreEqual(1, value);
            }
        }

        private sealed class TestCollection : InvertedLookupReactiveEntityCollection<string, int>
        {
            public TestCollection()
                : this(new AutoResetEvent(false), new AutoResetEvent(false))
            {
            }

            private TestCollection(EventWaitHandle addLock, EventWaitHandle removeLock)
                : base(new BlockingCollection<string, int>(StringComparer.Ordinal, addLock, removeLock), EqualityComparer<int>.Default)
            {
                AddLock = addLock;
                RemoveLock = removeLock;
            }

            public EventWaitHandle AddLock { get; private set; }

            public EventWaitHandle RemoveLock { get; private set; }
        }

        private sealed class BlockingCollection<TKey, TValue> : IReactiveEntityCollection<TKey, TValue>
        {
            private readonly IReactiveEntityCollection<TKey, TValue> _inner;
            private readonly EventWaitHandle _blockAdd;
            private readonly EventWaitHandle _blockRemove;

            public BlockingCollection(IEqualityComparer<TKey> comparer, EventWaitHandle blockAdd, EventWaitHandle blockRemove)
            {
                _inner = new ReactiveEntityCollection<TKey, TValue>(comparer);
                _blockAdd = blockAdd;
                _blockRemove = blockRemove;
            }

            public bool ContainsKey(TKey key)
            {
                return _inner.ContainsKey(key);
            }

            public bool TryAdd(TKey key, TValue value)
            {
                _blockAdd.WaitOne();
                return _inner.TryAdd(key, value);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return _inner.TryGetValue(key, out value);
            }

            public bool TryRemove(TKey key, out TValue value)
            {
                _blockRemove.WaitOne();
                return _inner.TryRemove(key, out value);
            }

            public ICollection<TValue> Values => _inner.Values;

            public IEnumerable<TKey> RemovedKeys => _inner.RemovedKeys;

            public void ClearRemovedKeys(IEnumerable<TKey> keys)
            {
                _inner.ClearRemovedKeys(keys);
            }

            public void Clear()
            {
                _inner.Clear();
            }

            public void Dispose()
            {
                _inner.Dispose();
                _blockAdd.Dispose();
                _blockRemove.Dispose();
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _inner.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
