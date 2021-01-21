// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.QueryEngine;

using Utilities;

namespace Tests
{
    [TestClass]
    public class LoggingStateWriterTests
    {
        [TestMethod]
        public void LoggingStateWriter_Basics()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using var w = new MyWriter();
                using var writer = new LoggingStateWriter(w, sw);

                Assert.AreEqual(CheckpointKind.Full, writer.CheckpointKind);

                var s = writer.GetItemWriter("bar", "foo");
                Assert.IsNotNull(s);

                writer.DeleteItem("bar", "foo");

                writer.Rollback();

                writer.CommitAsync().Wait();
            }

            var log = sb.ToString();

            foreach (var entry in new[]
            {
                "GetItemWriter(bar, foo)/Start",
                "GetItemWriter(bar, foo)/Stop",

                "DeleteItem(bar, foo)/Start",
                "DeleteItem(bar, foo)/Stop",

                "Rollback()/Start",
                "Rollback()/Stop",

                "CommitAsync()/Start",
                "CommitAsync()/Stop",

                "Dispose()/Start",
                "Dispose()/Stop",
            })
            {
                Assert.IsTrue(log.Contains(entry), "Not found: '" + entry + "'");
            }
        }

        [TestMethod]
        public void LoggingStateWriter_Errors()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using var w = new MyWriter { Throw = true };
                using var writer = new LoggingStateWriter(w, sw);

                Assert.ThrowsException<NotImplementedException>(() => _ = writer.GetItemWriter("bar", "foo"));
                Assert.ThrowsException<NotImplementedException>(() => writer.DeleteItem("bar", "foo"));
                Assert.ThrowsException<NotImplementedException>(() => writer.Rollback());
                Assert.ThrowsException<NotImplementedException>(() => writer.CommitAsync().GetAwaiter().GetResult());
                Assert.ThrowsException<NotImplementedException>(() => writer.Dispose());

                w.Throw = false;
            }

            var log = sb.ToString();

            foreach (var entry in new[]
            {
                "GetItemWriter(bar, foo)/Start",
                "GetItemWriter(bar, foo)/Error",
                "GetItemWriter(bar, foo)/Stop",

                "DeleteItem(bar, foo)/Start",
                "DeleteItem(bar, foo)/Error",
                "DeleteItem(bar, foo)/Stop",

                "Rollback()/Start",
                "Rollback()/Error",
                "Rollback()/Stop",

                "CommitAsync()/Start",
                "CommitAsync()/Error",
                "CommitAsync()/Stop",

                "Dispose()/Start",
                "Dispose()/Error",
                "Dispose()/Stop",
            })
            {
                Assert.IsTrue(log.Contains(entry), "Not found: '" + entry + "'");
            }
        }

        private sealed class MyWriter : IStateWriter
        {
            public bool Throw;

            public CheckpointKind CheckpointKind => CheckpointKind.Full;

            public Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                if (Throw)
                    throw new NotImplementedException();

                return Task.CompletedTask;
            }

            public void DeleteItem(string category, string key)
            {
                if (Throw)
                    throw new NotImplementedException();
            }

            public void Dispose()
            {
                if (Throw)
                    throw new NotImplementedException();
            }

            public Stream GetItemWriter(string category, string key)
            {
                if (Throw)
                    throw new NotImplementedException();

                return new MemoryStream();
            }

            public void Rollback()
            {
                if (Throw)
                    throw new NotImplementedException();
            }
        }
    }
}
