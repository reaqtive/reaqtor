// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.IO;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class InMemoryStorageTests
    {
        [TestMethod]
        public void InMemoryStorageBasicWritingReadingTest()
        {
            InMemoryStorageProvider provider = new InMemoryStorageProvider();

            string category = "someCategory";
            string itemKey = "someItem";
            string id = "someId";

            byte[] buffer1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            byte[] buffer2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0 };
            byte[][] buffers = new byte[][] { buffer1, buffer2 };

            for (int i = 0; i < 2; i++)
            {
                byte[] expectedBuffer = buffers[i];

                using (IStateWriter writer =
                    i == 0 ? provider.StartNewCheckpoint(id) : provider.UpdateCheckpoint(id))
                {
                    using (Stream stream = writer.GetItemWriter(category, itemKey))
                    {
                        stream.Write(expectedBuffer, 0, expectedBuffer.Length);
                    }

                    writer.CommitAsync().Wait();
                }

                IStateReader reader = null;
                try
                {
                    if (provider.TryReadCheckpoint(out id, out reader))
                    {
                        bool success = reader.TryGetItemReader(category, itemKey, out var stream);

                        Assert.IsTrue(success, "Could not get a reader for the requested item.");
                        Assert.AreEqual(expectedBuffer.Length, stream.Length);

                        byte[] actualBuffer = new byte[expectedBuffer.Length];

                        int byteRead = stream.Read(actualBuffer, 0, actualBuffer.Length);
                        Assert.AreEqual(byteRead, stream.Length);
                        CollectionAssert.AreEqual(expectedBuffer, actualBuffer);
                    }
                }
                finally
                {
                    reader?.Dispose();
                }
            }
        }

        [TestMethod]
        public void InMemoryStorageItemDeletionTest()
        {
            InMemoryStorageProvider provider = new InMemoryStorageProvider();

            string category = "someCategory";
            string itemKey = "someItem";
            string id = "someId";
            byte[] buffer = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            using (IStateWriter writer = provider.StartNewCheckpoint(id))
            {
                using (Stream stream = writer.GetItemWriter(category, itemKey))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }

                writer.CommitAsync().Wait();
            }

            IStateReader reader = null;
            try
            {
                if (provider.TryReadCheckpoint(out id, out reader))
                {
                    bool success = reader.TryGetItemReader(category, itemKey, out _);
                    Assert.IsTrue(success, "An stream could not be retrieved for the requested item.");
                }
                else
                {
                    Assert.Fail("Could not retrieve the checkpoint.");
                }
            }
            finally
            {
                reader?.Dispose();
            }

            using (IStateWriter writer = provider.UpdateCheckpoint(id))
            {
                writer.DeleteItem(category, itemKey);
                writer.CommitAsync().Wait();
            }

            reader = null;
            try
            {
                if (provider.TryReadCheckpoint(out id, out reader))
                {
                    bool success = reader.TryGetItemReader(category, itemKey, out _);
                    Assert.IsFalse(success, "An stream could be retrieved for the requested item.");
                }
                else
                {
                    Assert.Fail("Could not retrieve the checkpoint.");
                }
            }
            finally
            {
                reader?.Dispose();
            }
        }

        [TestMethod]
        public void InMemoryStorageCheckpointSequenceTest()
        {
            InMemoryStorageProvider provider = new InMemoryStorageProvider();

            string id = "someId";
            string category = "someCategory";
            string itemKey = "someItem";
            byte[] buffer = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (int i = 0; i < 10; i++)
            {
                if (i % 3 == 0)
                {
                    using IStateWriter writer = provider.StartNewCheckpoint(id);
                    Assert.IsNotNull(writer);

                    using (Stream stream = writer.GetItemWriter(category, itemKey))
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    writer.CommitAsync().Wait();
                }

                if (i % 3 == 1)
                {
                    using IStateWriter writer = provider.StartNewCheckpoint(id);
                    Assert.IsNotNull(writer);

                    using (Stream stream = writer.GetItemWriter(category, itemKey))
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    writer.Rollback();
                }

                if (i % 2 == 0)
                {
                    using IStateWriter writer = provider.UpdateCheckpoint(id);
                    Assert.IsNotNull(writer);

                    using (Stream stream = writer.GetItemWriter(category, itemKey))
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    writer.CommitAsync().Wait();
                }

                if (i % 5 == 0)
                {
                    using IStateWriter writer = provider.UpdateCheckpoint(id);
                    Assert.IsNotNull(writer);

                    using (Stream stream = writer.GetItemWriter(category, itemKey))
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    writer.Rollback();
                }
            }
        }
    }
}
