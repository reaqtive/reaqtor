// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Storage;

namespace Tests
{
    [TestClass]
    public class TransactionalDictionaryTests
    {
        [TestMethod]
        public void Basics_Empty()
        {
            var d = new TransactionalDictionary<int, int>();

            Assert.IsFalse(d.ContainsKey(0));
            Assert.IsFalse(d.ContainsKey(1));
            Assert.IsFalse(d.ContainsKey(2));

            Assert.IsFalse(d.TryGetValue(0, out _));
            Assert.IsFalse(d.TryGetValue(1, out _));
            Assert.IsFalse(d.TryGetValue(2, out _));

            Assert.ThrowsException<KeyNotFoundException>(() => _ = d[0]);
            Assert.ThrowsException<KeyNotFoundException>(() => _ = d[1]);
            Assert.ThrowsException<KeyNotFoundException>(() => _ = d[2]);

            Assert.IsFalse(d.Any());

            Assert.IsFalse(d.Remove(0));
            Assert.IsFalse(d.Remove(1));
            Assert.IsFalse(d.Remove(2));
        }

        [TestMethod]
        public void Basics_Add()
        {
            var d = new TransactionalDictionary<int, int>
            {
                { 42, 43 } // Add
            };

            Assert.ThrowsException<InvalidOperationException>(() => d.Add(42, -1));

            Assert.IsTrue(d.ContainsKey(42));

            Assert.IsTrue(d.TryGetValue(42, out var x));
            Assert.AreEqual(43, x);

            Assert.AreEqual(43, d[42]);

            Assert.IsTrue(d.SequenceEqual(new[] { new KeyValuePair<int, int>(42, 43) }));
        }

        [TestMethod]
        public void Basics_Remove()
        {
            var d = new TransactionalDictionary<int, int>
            {
                { 42, 43 } // Add
            };

            Assert.IsTrue(d.Remove(42));

            Assert.IsFalse(d.ContainsKey(42));

            Assert.IsFalse(d.TryGetValue(42, out var _));

            Assert.ThrowsException<KeyNotFoundException>(() => _ = d[42]);

            Assert.IsFalse(d.Any());
        }

        [TestMethod]
        public void Basics_Assign()
        {
            var d = new TransactionalDictionary<int, int>
            {
                [42] = 43 // indexer assignment
            };

            Assert.ThrowsException<InvalidOperationException>(() => d.Add(42, -1));

            Assert.IsTrue(d.ContainsKey(42));

            Assert.IsTrue(d.TryGetValue(42, out var x));
            Assert.AreEqual(43, x);

            Assert.AreEqual(43, d[42]);

            Assert.IsTrue(d.SequenceEqual(new[] { new KeyValuePair<int, int>(42, 43) }));
        }

        [TestMethod]
        public void Basics_Edit()
        {
            var d = new TransactionalDictionary<int, int>
            {
                { 42, 41 } // Add
            };

            d[42] = 43;

            Assert.ThrowsException<InvalidOperationException>(() => d.Add(42, -1));

            Assert.IsTrue(d.ContainsKey(42));

            Assert.IsTrue(d.TryGetValue(42, out var x));
            Assert.AreEqual(43, x);

            Assert.AreEqual(43, d[42]);

            Assert.IsTrue(d.SequenceEqual(new[] { new KeyValuePair<int, int>(42, 43) }));
        }

        [TestMethod]
        public void Snapshot_Full()
        {
            var d = new TransactionalDictionary<int, char>
            {
                { 0, '0' },
                { 1, 'a' },
                { 2, 'b' },
                { 4, '4' },
                { 6, 'c' },
            };

            d[1] = '1';

            d.Remove(2);
            d[2] = '2';

            d.Add(5, '5');

            d.Remove(6);
            d.Add(6, 'd');

            d.Remove(6);
            d[6] = '6';

            d.Add(3, '3');

            var s = d.CreateSnapshot(differential: false);

            var res = new Dictionary<int, char>();

            var v = new DictionarySnapshotVisitor<int, char>(res);
            s.Accept(v);

            var e = new Dictionary<int, char>
            {
                { 0, '0' },
                { 1, '1' },
                { 2, '2' },
                { 3, '3' },
                { 4, '4' },
                { 5, '5' },
                { 6, '6' },
            };

            AssertEqual(e, d);
            AssertEqual(res, d);
        }

        [TestMethod]
        public void Snapshot_Delta()
        {
            var res = new Dictionary<int, char>();
            var v = new DictionarySnapshotVisitor<int, char>(res);

            var d = new TransactionalDictionary<int, char>
            {
                { 0, '0' },
                { 1, '1' },
                { 2, '2' },
                { 3, '3' },
                { 4, '4' },
                { 6, '6' },
            };

            CheckpointAndAssert(delta: false);

            d.Add(5, '5');

            CheckpointAndAssert(delta: true);

            d.Remove(2);

            CheckpointAndAssert(delta: true);

            d[0] = '-';

            CheckpointAndAssert(delta: true);

            d.Remove(5);
            d.Add(5, 'x');

            CheckpointAndAssert(delta: true);

            d.Remove(4);
            d.Add(4, 'x');
            d.Remove(4);

            CheckpointAndAssert(delta: true);

            d.Remove(3);
            d.Add(3, 'x');
            d.Remove(3);
            d.Add(3, 'y');

            CheckpointAndAssert(delta: true);

            d.Add(7, 'a');
            d.Remove(7);

            CheckpointAndAssert(delta: true);

            d.Add(8, 'b');
            d.Remove(8);
            d.Add(8, 'c');

            CheckpointAndAssert(delta: true);

            d.Add(9, 'd');
            d.Remove(9);
            d.Add(9, 'e');
            d.Remove(9);

            CheckpointAndAssert(delta: true);

            var e = new Dictionary<int, char>
            {
                { 0, '-' },
                { 1, '1' },
                { 3, 'y' },
                { 5, 'x' },
                { 6, '6' },
                { 8, 'c' },
            };

            AssertEqual(e, d);

            void CheckpointAndAssert(bool delta)
            {
                var s1 = d.CreateSnapshot(delta);
                s1.Accept(v);
                s1.OnCommitted();

                AssertEqual(res, d);
            }
        }

        private static void AssertEqual<K, V>(Dictionary<K, V> expected, TransactionalDictionary<K, V> actual)
        {
            Assert.IsTrue(expected.OrderBy(kv => kv.Key).SequenceEqual(actual.OrderBy(kv => kv.Key)));

            foreach (var kv in expected)
            {
                var key = kv.Key;
                var value = kv.Value;

                Assert.IsTrue(actual.ContainsKey(key));

                Assert.IsTrue(actual.TryGetValue(key, out var v));
                Assert.AreEqual(value, v);

                Assert.AreEqual(value, actual[key]);
            }
        }
    }
}
