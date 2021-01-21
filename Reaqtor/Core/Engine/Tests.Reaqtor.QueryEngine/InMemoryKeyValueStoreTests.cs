// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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

                table.Add("A", new byte[] { 1 });

                var one = table["A"];

                CollectionAssert.AreEqual(new byte[] { 1 }, one);

                table.Update("A", new byte[] { 2 });

                Assert.IsTrue(table.Contains("A"));

                var two = table["A"];

                CollectionAssert.AreEqual(new byte[] { 2 }, two);

                var contents = table.ToList();

                Assert.AreEqual(contents.Count, 1);
                Assert.AreEqual(contents[0].Key, "A");
                CollectionAssert.AreEquivalent(contents[0].Value, new byte[] { 2 });

                table.Remove("A");

                Assert.IsFalse(table.Contains("A"));

                tx.CommitAsync().Wait();
            }

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                Assert.AreEqual(table.Count(), 0);
            }
        }

        [TestMethod]
        public void InMemoryKeyValueStore_Extensions()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TransactedKeyValueTable.Clear<string, byte[]>(null));
            Assert.ThrowsException<ArgumentNullException>(() => TransactedKeyValueTable.TryRemove<string, byte[]>(null, "aa"));
            Assert.ThrowsException<ArgumentNullException>(() => TransactedKeyValueTable.TryGet<string, byte[]>(null, "aa", out _));

            var kvs = new InMemoryKeyValueStore();

            var subtable = kvs.GetTable("MyTable");

            using (var tx = kvs.CreateTransaction())
            {
                var table = subtable.Enter(tx);

                // Clear

                table.Add("A", new byte[] { 1 });

                table.Clear();

                Assert.IsFalse(table.Contains("A"));

                // TryRemove

                Assert.IsFalse(table.TryRemove("A"));

                table.Add("A", new byte[] { 1 });

                Assert.IsTrue(table.TryRemove("A"));
                Assert.IsFalse(table.Contains("A"));

                // TryGet

                Assert.IsFalse(table.TryGet("A", out var value));
                table.Add("A", new byte[] { 1 });
                Assert.IsTrue(table.TryGet("A", out value));
                CollectionAssert.AreEquivalent(value, new byte[] { 1 });
            }

            Assert.ThrowsException<ArgumentNullException>(() => Transaction.CommitAsync(null));
        }

        [TestMethod]
        public void InMemoryKeyValueStore_AllOperations_MultipleTransaction() => InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(false);

        [TestMethod]
        public void InMemoryKeyValueStore_AllOperations_MultipleTransaction_SaveAndLoad() => InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(true);

        private void InMemoryKeyValueStore_AllOperations_MultipleTransaction_Core(bool saveAndLoad)
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

                table.Add("A", new byte[] { 1 });

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

                table.Update("A", new byte[] { 2 });

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

                Assert.AreEqual(contents.Count, 1);
                Assert.AreEqual(contents[0].Key, "A");
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

                Assert.AreEqual(table.Count(), 0);
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

                writer1.Add("A", new byte[] { 1 });
                writer2.Add("B", new byte[] { 1 });

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

                writer1.Add("C", new byte[] { 1 });
                writer2.Add("C", new byte[] { 2 });

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

                writer1.Add("D", new byte[] { 1 });
                writer2.Add("D", new byte[] { 2 });

                tx1.CommitAsync().Wait();

                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx2.CommitAsync()).Wait();
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
                writer.Add("A", Array.Empty<byte>());

                tx2.CommitAsync().Wait();

                // Throw because contains was false before and true now
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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

                writer.Add("A", Array.Empty<byte>());

                tx2.CommitAsync().Wait();

                // Throw because Get threw before and won't now (since the writer has added the key)
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Update

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                try
                {
                    writer1.Update("A", Array.Empty<byte>());
                    Assert.Fail();
                }
                catch
                {
                    // We leaked that "A" is not in the kvs
                }

                writer2.Add("A", Array.Empty<byte>());

                tx2.CommitAsync().Wait();

                // Throw because Updaete threw before and won't now (since the writer has added the key)
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }

            // Add

            using (var tx1 = kvs.CreateTransaction())
            using (var tx2 = kvs.CreateTransaction())
            {
                var writer1 = subtable1.Enter(tx1);
                var writer2 = subtable1.Enter(tx2);

                try
                {
                    writer1.Add("A", Array.Empty<byte>());
                    Assert.Fail();
                }
                catch
                {
                    // We leaked that "A" is already in the kvs
                }

                writer2.Remove("A");

                tx2.CommitAsync().Wait();

                // Throw because Add threw before and won't now (since the writer has removed the key)
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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

                writer2.Add("A", Array.Empty<byte>());

                tx2.CommitAsync().Wait();

                // Throw because Remove threw before and won't now (since the writer has added the key)
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
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
                Assert.ThrowsExceptionAsync<WriteConflictException>(() => tx1.CommitAsync()).Wait();
            }
        }
    }
}
