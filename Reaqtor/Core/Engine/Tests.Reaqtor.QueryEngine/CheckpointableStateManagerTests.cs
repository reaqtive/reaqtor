// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class CheckpointableStateManagerTests
    {
        [TestMethod]
        public void CheckpointableStateManager_RecoverCheckpointUnload()
        {
            var res = Test(async e =>
            {
                await e.RecoverAsync(new TestReader());
                await e.CheckpointAsync(new TestWriter());
                await e.UnloadAsync();
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Recover,
                Actions.Checkpoint,
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_RecoverManyTimesSupportedForMergingEngines()
        {
            var res = Test(async e =>
            {
                await e.RecoverAsync(new TestReader());
                await e.CheckpointAsync(new TestWriter());
                await e.RecoverAsync(new TestReader());
                await e.CheckpointAsync(new TestWriter());
                await e.CheckpointAsync(new TestWriter());
                await e.UnloadAsync();
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Recover,
                Actions.Checkpoint,
                Actions.Recover,
                Actions.Checkpoint,
                Actions.Checkpoint,
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_CheckpointThenUnload()
        {
            var res = Test(async e =>
            {
                await e.CheckpointAsync(new TestWriter());
                await e.CheckpointAsync(new TestWriter());
                await e.CheckpointAsync(new TestWriter());
                await e.UnloadAsync();
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Checkpoint,
                Actions.Checkpoint,
                Actions.Checkpoint,
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadMultipleTimesIsIdempotent()
        {
            var res = Test(async e =>
            {
                await e.CheckpointAsync(new TestWriter());
                await e.UnloadAsync();
                await e.UnloadAsync();
                await e.UnloadAsync();
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Checkpoint,
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_CantCheckpointAfterUnload()
        {
            var res = Test(async e =>
            {
                await e.UnloadAsync();
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.CheckpointAsync(new TestWriter()));
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.CheckpointAsync(new TestWriter()));
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_CantRecoverAfterUnload()
        {
            var res = Test(async e =>
            {
                await e.UnloadAsync();
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.RecoverAsync(new TestReader()));
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => e.RecoverAsync(new TestReader()));
            });

            Assert.IsTrue(res.SequenceEqual(new[]
            {
                Actions.Unload,
            }));
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringCheckpointQueuesUp()
        {
            var checkpointDone = new TaskCompletionSource<bool>();

            var checkpointing = false;
            var unloading = false;

            var e = new TestEngine(
                (_, ct, p) => { checkpointing = true; return checkpointDone.Task; },
                (_, ct, p) => Task.FromResult(true),
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.CheckpointAsync(new TestWriter());
            Assert.IsTrue(checkpointing);

            var u1 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            var u2 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            checkpointDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u1.Wait();
            u2.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringRecoverQueuesUp()
        {
            var recoveryDone = new TaskCompletionSource<bool>();

            var recovering = false;
            var unloading = false;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                (_, ct, p) => { recovering = true; return recoveryDone.Task; },
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.RecoverAsync(new TestReader());
            Assert.IsTrue(recovering);

            var u1 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            var u2 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            recoveryDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u1.Wait();
            u2.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringCheckpointCausesCancellation()
        {
            var checkpointDone = new TaskCompletionSource<bool>();

            var checkpointing = false;
            var unloading = false;
            var cancelled = false;

            var e = new TestEngine(
                (_, ct, p) => { checkpointing = true; ct.Register(() => cancelled = true); return checkpointDone.Task; },
                (_, ct, p) => Task.FromResult(true),
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.CheckpointAsync(new TestWriter());
            Assert.IsTrue(checkpointing);

            var u1 = s.UnloadAsync();
            Assert.IsFalse(unloading);
            Assert.IsTrue(cancelled);

            var u2 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            checkpointDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u1.Wait();
            u2.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringRecoverCausesCancellation()
        {
            var recoveryDone = new TaskCompletionSource<bool>();

            var recovering = false;
            var unloading = false;
            var cancelled = false;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                (_, ct, p) => { recovering = true; ct.Register(() => cancelled = true); return recoveryDone.Task; },
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.RecoverAsync(new TestReader());
            Assert.IsTrue(recovering);

            var u1 = s.UnloadAsync();
            Assert.IsFalse(unloading);
            Assert.IsTrue(cancelled);

            var u2 = s.UnloadAsync();
            Assert.IsFalse(unloading);

            recoveryDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u1.Wait();
            u2.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringCheckpointCausesWriterShutdown()
        {
            var checkpointDone = new TaskCompletionSource<bool>();
            var unloadStarted = new TaskCompletionSource<bool>();

            var checkpointing = false;
            var unloading = false;

            var e = new TestEngine(
                async (w, ct, p) =>
                {
                    checkpointing = true;
                    await unloadStarted.Task;
                    Assert.ThrowsException<EngineUnloadedException>(() => w.DeleteItem("foo", "bar"));
                    Assert.ThrowsException<EngineUnloadedException>(() => w.GetItemWriter("foo", "bar"));
                    Assert.ThrowsException<EngineUnloadedException>(() => w.Rollback());
                    await Assert.ThrowsExceptionAsync<EngineUnloadedException>(() => w.CommitAsync());
                    await checkpointDone.Task;
                },
                (_, ct, p) => Task.FromResult(true),
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.CheckpointAsync(new TestWriter());
            Assert.IsTrue(checkpointing);

            var u = s.UnloadAsync();
            Assert.IsFalse(unloading);
            unloadStarted.SetResult(true);

            checkpointDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadDuringRecoverCausesReaderShutdown()
        {
            var recoveryDone = new TaskCompletionSource<bool>();
            var unloadStarted = new TaskCompletionSource<bool>();

            var recovering = false;
            var unloading = false;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                async (r, ct, p) =>
                {
                    recovering = true;
                    await unloadStarted.Task;
                    Assert.ThrowsException<EngineUnloadedException>(() => { r.GetCategories(); });
                    Assert.ThrowsException<EngineUnloadedException>(() => { r.TryGetItemKeys("bar", out _); });
                    Assert.ThrowsException<EngineUnloadedException>(() => { r.TryGetItemReader("bar", "foo", out _); });
                    await recoveryDone.Task;
                },
                (p) => { unloading = true; return Task.FromResult(true); }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var c = s.RecoverAsync(new TestReader());
            Assert.IsTrue(recovering);

            var u = s.UnloadAsync();
            Assert.IsFalse(unloading);
            unloadStarted.SetResult(true);

            recoveryDone.SetResult(true);
            c.Wait();

            Assert.IsTrue(unloading);
            u.Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_CheckpointCanBeCancelled()
        {
            var busy = new TaskCompletionSource<bool>();
            var done = new TaskCompletionSource<bool>();
            var cancelled = false;

            var e = new TestEngine(
                async (_, ct, p) => { ct.Register(() => cancelled = true); busy.SetResult(true); await done.Task; },
                (_, ct, p) => Task.FromResult(true),
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var cts = new CancellationTokenSource();

            var t = s.CheckpointAsync(new TestWriter(), cts.Token);

            busy.Task.Wait();

            cts.Cancel();

            done.SetResult(true);
            t.Wait();

            Assert.IsTrue(cancelled);
        }

        [TestMethod]
        public void CheckpointableStateManager_CheckpointCanBeCancelledImmediately()
        {
            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                (_, ct, p) => Task.FromResult(true),
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.ThrowsExceptionAsync<OperationCanceledException>(() => s.CheckpointAsync(new TestWriter(), cts.Token)).Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_RecoverCanBeCancelled()
        {
            var busy = new TaskCompletionSource<bool>();
            var done = new TaskCompletionSource<bool>();
            var cancelled = false;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                async (_, ct, p) => { ct.Register(() => cancelled = true); busy.SetResult(true); await done.Task; },
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var cts = new CancellationTokenSource();

            var t = s.RecoverAsync(new TestReader(), cts.Token);

            busy.Task.Wait();

            cts.Cancel();

            done.SetResult(true);
            t.Wait();

            Assert.IsTrue(cancelled);
        }

        [TestMethod]
        public void CheckpointableStateManager_RecoverCanBeCancelledImmediately()
        {
            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                (_, ct, p) => Task.FromResult(true),
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.ThrowsExceptionAsync<OperationCanceledException>(() => s.RecoverAsync(new TestReader(), cts.Token)).Wait();
        }

        [TestMethod]
        public void CheckpointableStateManager_UnloadCanBeRetried()
        {
            var i = 0;
            var fail = true;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                (_, ct, p) => Task.FromResult(true),
                async p =>
                {
                    if (Volatile.Read(ref fail))
                    {
                        var j = Interlocked.Increment(ref i) - 1;
                        throw new Exception("Oops! " + j);
                    }

                    await Task.Yield();
                }
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            try
            {
                s.UnloadAsync().Wait();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                ex.Handle(err => err.Message == "Oops! 0");
            }

            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.CheckpointAsync(new TestWriter())).Wait();
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.RecoverAsync(new TestReader())).Wait();

            try
            {
                s.UnloadAsync().Wait();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                ex.Handle(err => err.Message == "Oops! 1");
            }

            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.CheckpointAsync(new TestWriter())).Wait();
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.RecoverAsync(new TestReader())).Wait();

            Volatile.Write(ref fail, false);

            s.UnloadAsync().Wait();
            Assert.AreEqual(2, Volatile.Read(ref i));

            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.CheckpointAsync(new TestWriter())).Wait();
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => s.RecoverAsync(new TestReader())).Wait();

            s.UnloadAsync().Wait();
            Assert.AreEqual(2, Volatile.Read(ref i));
        }

        [TestMethod]
        public void CheckpointableStateManager_ErrorDuringCheckpointKeepsEngineRunning()
        {
            var fail = true;

            var e = new TestEngine(
                async (_, ct, p) =>
                {
                    if (Volatile.Read(ref fail))
                        throw new ArithmeticException("Oops!");

                    await Task.Yield();
                },
                (_, ct, p) => Task.FromResult(true),
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Assert.ThrowsExceptionAsync<ArithmeticException>(() => s.CheckpointAsync(new TestWriter())).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Assert.ThrowsExceptionAsync<ArithmeticException>(() => s.CheckpointAsync(new TestWriter())).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Volatile.Write(ref fail, false);

            s.CheckpointAsync(new TestWriter()).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);
        }

        [TestMethod]
        public void CheckpointableStateManager_ErrorDuringRecoverKeepsEngineRunning()
        {
            var fail = true;

            var e = new TestEngine(
                (_, ct, p) => Task.FromResult(true),
                async (_, ct, p) =>
                {
                    if (Volatile.Read(ref fail))
                        throw new ArithmeticException("Oops!");

                    await Task.Yield();
                },
                (p) => Task.FromResult(true)
            );

            var s = new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null);

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Assert.ThrowsExceptionAsync<ArithmeticException>(() => s.RecoverAsync(new TestReader())).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Assert.ThrowsExceptionAsync<ArithmeticException>(() => s.RecoverAsync(new TestReader())).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);

            Volatile.Write(ref fail, false);

            s.RecoverAsync(new TestReader()).Wait();

            Assert.AreEqual(QueryEngineStatus.Running, s.Status);
        }

        [TestMethod]
        public void CheckpointableStateManager_StateWriterWrapper()
        {
            var engine = new WriterTestEngine();
            var writer = new TestWriter();

            var s = new CheckpointableStateManager(engine, new Uri("qe:/test"), traceSource: null);

            s.CheckpointAsync(writer).Wait();

            Assert.IsTrue(writer.Log.SequenceEqual(new[]
            {
                "GetItemWriter(foo, bar)",
                "DeleteItem(qux, baz)",
                "Rollback()",
                "CommitAsync()",
                "Dispose()",
            }));
        }

        private class WriterTestEngine : ICheckpointable
        {
            public Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
            {
                Assert.AreEqual((CheckpointKind)99, writer.CheckpointKind);

                writer.GetItemWriter("foo", "bar");
                writer.DeleteItem("qux", "baz");
                writer.Rollback();
                writer.CommitAsync().Wait();

                writer.Dispose();

                Assert.ThrowsException<ObjectDisposedException>(() => { writer.GetItemWriter("xyz", "bar"); });
                Assert.ThrowsException<ObjectDisposedException>(() => { writer.DeleteItem("abc", "baz"); });
                Assert.ThrowsException<ObjectDisposedException>(() => { writer.Rollback(); });
                Assert.ThrowsException<ObjectDisposedException>(() => { writer.CommitAsync().Wait(); });

                return Task.FromResult(true);
            }

            public Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
            {
                throw new NotImplementedException();
            }

            public Task UnloadAsync(IProgress<int> progress)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class TestWriter : IStateWriter
        {
            public List<string> Log = new();

            public CheckpointKind CheckpointKind => (CheckpointKind)99;

            public Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                Log.Add("CommitAsync()");
                return Task.FromResult(true);
            }

            public void Rollback()
            {
                Log.Add("Rollback()");
            }

            public Stream GetItemWriter(string category, string key)
            {
                Log.Add("GetItemWriter(" + category + ", " + key + ")");
                return null;
            }

            public void DeleteItem(string category, string key)
            {
                Log.Add("DeleteItem(" + category + ", " + key + ")");
            }

            public void Dispose()
            {
                Log.Add("Dispose()");
            }
        }

        [TestMethod]
        public void CheckpointableStateManager_StateReaderWrapper()
        {
            var engine = new ReaderTestEngine();
            var reader = new TestReader();

            var s = new CheckpointableStateManager(engine, new Uri("qe:/test"), traceSource: null);

            s.RecoverAsync(reader).Wait();

            Assert.IsTrue(reader.Log.SequenceEqual(new[]
            {
                "GetCategories()",
                "TryGetItemKeys(bar)",
                "TryGetItemReader(bar, foo)",
                "Dispose()",
            }));
        }

        private sealed class ReaderTestEngine : ICheckpointable
        {
            public Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
            {
                throw new NotImplementedException();
            }

            public Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
            {
                reader.GetCategories();

                reader.TryGetItemKeys("bar", out _);

                reader.TryGetItemReader("bar", "foo", out _);

                reader.Dispose();

                Assert.ThrowsException<ObjectDisposedException>(() => { reader.GetCategories(); });
                Assert.ThrowsException<ObjectDisposedException>(() => { reader.TryGetItemKeys("bar", out _); });
                Assert.ThrowsException<ObjectDisposedException>(() => { reader.TryGetItemReader("bar", "foo", out _); });

                return Task.FromResult(true);
            }

            public Task UnloadAsync(IProgress<int> progress)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class TestReader : IStateReader
        {
            public List<string> Log = new();

            public IEnumerable<string> GetCategories()
            {
                Log.Add("GetCategories()");
                return null;
            }

            public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
            {
                Log.Add("TryGetItemKeys(" + category + ")");
                keys = null;
                return true;
            }

            public bool TryGetItemReader(string category, string key, out Stream stream)
            {
                Log.Add("TryGetItemReader(" + category + ", " + key + ")");
                stream = null;
                return true;
            }

            public void Dispose()
            {
                Log.Add("Dispose()");
            }
        }

        private static List<Actions> Test(Func<ICheckpointable, Task> f)
        {
            var log = new List<Actions>();

            var e = new TestEngine(
                (_, ct, p) => { log.Add(Actions.Checkpoint); return Task.FromResult(true); },
                (_, ct, p) => { log.Add(Actions.Recover); return Task.FromResult(true); },
                (p) => { log.Add(Actions.Unload); return Task.FromResult(true); }
            );

            f(new CheckpointableStateManager(e, new Uri("qe:/test"), traceSource: null)).Wait();

            return log;
        }
    }

    internal enum Actions
    {
        Checkpoint,
        Recover,
        Unload,
    }

    internal class TestEngine : ICheckpointable
    {
        private readonly Func<IStateWriter, CancellationToken, IProgress<int>, Task> _checkpoint;
        private readonly Func<IStateReader, CancellationToken, IProgress<int>, Task> _recover;
        private readonly Func<IProgress<int>, Task> _unload;

        public TestEngine(Func<IStateWriter, CancellationToken, IProgress<int>, Task> checkpoint, Func<IStateReader, CancellationToken, IProgress<int>, Task> recover, Func<IProgress<int>, Task> unload)
        {
            _checkpoint = checkpoint;
            _recover = recover;
            _unload = unload;
        }

        public Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
        {
            return _checkpoint(writer, token, progress);
        }

        public Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
        {
            return _recover(reader, token, progress);
        }

        public Task UnloadAsync(IProgress<int> progress)
        {
            return _unload(progress);
        }
    }
}
