// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class InMemoryKeyValueTableTests
    {
        [TestMethod]
        public void InMemoryKeyValueStore_AllOperations_SingleTransaction()
        {
            var kvs = new InMemoryKeyValueStore();

            var subtable = kvs.GetTable("MyTable");

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                table.Add("A", [1]);

                var one = table["A"];

                CollectionAssert.AreEqual(new byte[] { 1 }, one);

                table.Update("A", [2]);

                Assert.IsTrue(table.Contains("A"));

                var two = table["A"];

                CollectionAssert.AreEqual(new byte[] { 2 }, two);

                var contents = table.ToList();

                Assert.AreEqual(1, contents.Count);
                Assert.AreEqual("A", contents[0].Key);
                CollectionAssert.AreEquivalent(contents[0].Value, new byte[] { 2 });

                table.Remove("A");

                Assert.IsFalse(table.Contains("A"));

                tx.CommitAsync().Wait();
            }

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                Assert.AreEqual(0, table.Count());
            }
        }

        [TestMethod]
        public void InMemoryKeyValueStore_Extensions()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => TransactedKeyValueTable.Clear<string, byte[]>(null));
            Assert.ThrowsExactly<ArgumentNullException>(() => TransactedKeyValueTable.TryRemove<string, byte[]>(null, "aa"));
            Assert.ThrowsExactly<ArgumentNullException>(() => TransactedKeyValueTable.TryGet<string, byte[]>(null, "aa", out _));

            var kvs = new InMemoryKeyValueStore();

            var subtable = kvs.GetTable("MyTable");

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                // Clear

                table.Add("A", [1]);

                table.Clear();

                Assert.IsFalse(table.Contains("A"));

                // TryRemove

                Assert.IsFalse(table.TryRemove("A"));

                table.Add("A", [1]);

                Assert.IsTrue(table.TryRemove("A"));
                Assert.IsFalse(table.Contains("A"));

                // TryGet

                Assert.IsFalse(table.TryGet("A", out var value));
                table.Add("A", [1]);
                Assert.IsTrue(table.TryGet("A", out value));
                CollectionAssert.AreEquivalent(value, new byte[] { 1 });
            }

            Assert.ThrowsExactly<ArgumentNullException>(() => Transaction.CommitAsync(null));
        }

        [TestMethod]
        public void InMemoryKeyValueStore_AllOperations_MultipleTransaction() => InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(false);

        [TestMethod]
        public void InMemoryKeyValueStore_AllOperations_MultipleTransaction_SaveAndLoad() => InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(true);

        private static void InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(bool saveAndLoad)
        {
            var kvs = new InMemoryKeyValueStore();

            void SaveAndLoad()
            {
                if (!saveAndLoad)
                {
                    return;
                }

                using var ms = new MemoryStream();

                kvs.Save(ms);

                ms.Position = 0;

                kvs = InMemoryKeyValueStore.Load(ms);
            }

            var subtable = kvs.GetTable("MyTable");

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                table.Add("A", [1]);

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                var one = table["A"];

                CollectionAssert.AreEqual(new byte[] { 1 }, one);

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                table.Update("A", [2]);

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                Assert.IsTrue(table.Contains("A"));

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                var two = table["A"];

                CollectionAssert.AreEqual(new byte[] { 2 }, two);

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                var contents = table.ToList();

                Assert.AreEqual(1, contents.Count);
                Assert.AreEqual("A", contents[0].Key);
                CollectionAssert.AreEquivalent(contents[0].Value, new byte[] { 2 });

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                table.Remove("A");

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                Assert.IsFalse(table.Contains("A"));

                tx.CommitAsync().Wait();
            }

            SaveAndLoad();

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                Assert.AreEqual(0, table.Count());
            }
        }

        [TestMethod]
        public void InMemoryKeyValueStoreMultipleWriters()
        {
            var kvs = new InMemoryKeyValueStore();

            var subtable1 = kvs.GetTable("MyTable1");
            var subtable2 = kvs.GetTable("MyTable2");

            // Same table, different keys
            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                writer1.Add("A", [1]);
                writer2.Add("B", [1]);

                tx1.CommitAsync().Wait();
                tx2.CommitAsync().Wait();
            }

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable1.Enter(tx);

                Assert.IsTrue(table.Contains("A"));
                Assert.IsTrue(table.Contains("B"));
            }

            // Different table, same keys
            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable2.Enter(tx2);

                writer1.Add("C", [1]);
                writer2.Add("C", [2]);

                tx1.CommitAsync().Wait();
                tx2.CommitAsync().Wait();
            }

            using (var tx = kvs.CreateTransaction())
            {
                var table1 = subtable1.Enter(tx);
                var table2 = subtable2.Enter(tx);

                CollectionAssert.AreEquivalent(table1["C"], new byte[] { 1 });
                CollectionAssert.AreEquivalent(table2["C"], new byte[] { 2 });
            }

            // Same table, same keys
            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                writer1.Add("D", [1]);
                writer2.Add("D", [2]);

                tx1.CommitAsync().Wait();

                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx2.CommitAsync()).Wait();
            }
        }

        [TestMethod]
        public void InMemoryKeyValueStore_ConsistencyViolations()
        {
            var kvs = new InMemoryKeyValueStore();

            var subtable1 = kvs.GetTable("MyTable1");

            // Contains

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var reader = subtable1.Enter(tx1);
                var writer = subtable1.Enter(tx2);

                reader.Contains("A");
                writer.Add("A", []);

                tx2.CommitAsync().Wait();

                // Throw because contains was false before and true now
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var reader = subtable1.Enter(tx1);
                var writer = subtable1.Enter(tx2);

                reader.Contains("A");
                writer.Remove("A");

                tx2.CommitAsync().Wait();

                // Throw because contains was true before and false now
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Get

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var reader = subtable1.Enter(tx1);
                var writer = subtable1.Enter(tx2);

                try
                {
                    var CS0201 = reader["A"];
                    Assert.Fail();
                }
                catch
                {
                    // We leak the fact that the table doesn't contain "A"
                }

                writer.Add("A", []);

                tx2.CommitAsync().Wait();

                // Throw because Get threw before and won't now (since the writer has added the key)
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var reader = subtable1.Enter(tx1);
                var writer = subtable1.Enter(tx2);

                var CS0201 = reader["A"];
                writer.Update("A", new byte[1]);

                tx2.CommitAsync().Wait();

                // Throw because Get returned the original version before but will return the updated version now
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var reader = subtable1.Enter(tx1);
                var writer = subtable1.Enter(tx2);

                var CS0201 = reader["A"];
                writer.Remove("A");

                tx2.CommitAsync().Wait();

                // Throw because Get didn't throw before but will now (since the writer has added the key)
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Update

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                try
                {
                    writer1.Update("A", []);
                    Assert.Fail();
                }
                catch
                {
                    // We leaked that "A" is not in the kvs
                }

                writer2.Add("A", []);

                tx2.CommitAsync().Wait();

                // Throw because Updaete threw before and won't now (since the writer has added the key)
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Add

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                try
                {
                    writer1.Add("A", []);
                    Assert.Fail();
                }
                catch
                {
                    // We leaked that "A" is already in the kvs
                }

                writer2.Remove("A");

                tx2.CommitAsync().Wait();

                // Throw because Add threw before and won't now (since the writer has removed the key)
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Remove

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                try
                {
                    writer1.Remove("A");
                    Assert.Fail();
                }
                catch
                {
                    // We leaked that "A" is already in the kvs
                }

                writer2.Add("A", []);

                tx2.CommitAsync().Wait();

                // Throw because Remove threw before and won't now (since the writer has added the key)
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Enumerate

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                foreach (var item in writer1) ;

                writer2.Update("A", new byte[2]);

                tx2.CommitAsync().Wait();

                // Throw because iteration is not consistent with before
                Assert.ThrowsExactlyAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }
        }
    }
}
