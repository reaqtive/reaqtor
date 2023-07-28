// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.QueryEngine;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class StateWriterExtensionsTests
    {
        [TestMethod]
        public void StateWriterExtensions_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StateWriterExtensions.CommitAsync(null));
            Assert.ThrowsException<ArgumentNullException>(() => StateWriterExtensions.CommitAsync(null, CancellationToken.None));
            Assert.ThrowsException<ArgumentNullException>(() => StateWriterExtensions.CommitAsync(null, new Progress<int>()));
        }

        [TestMethod]
        public void StateWriterExtensions_CommitAsync1()
        {
            var writer = new MyWriter();

            var t = writer.CommitAsync();

            writer.CommitAsyncTask.SetResult("foo");
            t.Wait();

            var s = (Task<object>)t;
            Assert.AreEqual("foo", s.Result);
        }

        [TestMethod]
        public void StateWriterExtensions_CommitAsync2()
        {
            var writer = new MyWriter();

            var t = writer.CommitAsync(CancellationToken.None);

            writer.CommitAsyncTask.SetResult("foo");
            t.Wait();

            var s = (Task<object>)t;
            Assert.AreEqual("foo", s.Result);
        }

        [TestMethod]
        public void StateWriterExtensions_CommitAsync3()
        {
            var writer = new MyWriter();

            var res = default(int);
            var p = Progress.Create<int>(x => res = x);

            var t = writer.CommitAsync(p);

            writer.CommitAsyncTask.SetResult("foo");
            t.Wait();

            var s = (Task<object>)t;
            Assert.AreEqual("foo", s.Result);

            Assert.AreEqual(42, res);
        }

        private sealed class MyWriter : IStateWriter
        {
            public TaskCompletionSource<object> CommitAsyncTask = new();

            public CheckpointKind CheckpointKind => throw new NotImplementedException();

            public Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                progress?.Report(42);

                return CommitAsyncTask.Task;
            }

            public void Rollback()
            {
                throw new NotImplementedException();
            }

            public Stream GetItemWriter(string category, string key)
            {
                throw new NotImplementedException();
            }

            public void DeleteItem(string category, string key)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

    }
}
