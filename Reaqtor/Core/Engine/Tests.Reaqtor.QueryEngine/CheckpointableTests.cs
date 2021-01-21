// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class CheckpointableTests
    {
        [TestMethod]
        public void Checkpointable_ArgumentChecking()
        {
            Run((_, sr, sw, ct, pr) =>
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.CheckpointAsync(null, sw), ex => Assert.AreEqual("this", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.CheckpointAsync(null, sw, ct), ex => Assert.AreEqual("this", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.CheckpointAsync(null, sw, pr), ex => Assert.AreEqual("this", ex.ParamName));

                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.RecoverAsync(null, sr), ex => Assert.AreEqual("this", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.RecoverAsync(null, sr, ct), ex => Assert.AreEqual("this", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.RecoverAsync(null, sr, pr), ex => Assert.AreEqual("this", ex.ParamName));

                AssertEx.ThrowsException<ArgumentNullException>(() => CheckpointableExtensions.UnloadAsync(null), ex => Assert.AreEqual("this", ex.ParamName));
            });
        }

        [TestMethod]
        public void Checkpointable_CheckpointAsync1()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.CheckpointAsync(sw).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Checkpoint, Tuple.Create(sw, CancellationToken.None, default(IProgress<int>))),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_CheckpointAsync2()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.CheckpointAsync(sw, ct).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Checkpoint, Tuple.Create(sw, ct, default(IProgress<int>))),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_CheckpointAsync3()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.CheckpointAsync(sw, pr).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Checkpoint, Tuple.Create(sw, CancellationToken.None, pr)),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_RecoverAsync1()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.RecoverAsync(sr).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Recover, Tuple.Create(sr, CancellationToken.None, default(IProgress<int>))),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_RecoverAsync2()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.RecoverAsync(sr, ct).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Recover, Tuple.Create(sr, ct, default(IProgress<int>))),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_RecoverAsync3()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.RecoverAsync(sr, pr).Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Recover, Tuple.Create(sr, CancellationToken.None, pr)),
                    }));
                }
            );
        }

        [TestMethod]
        public void Checkpointable_UnloadAsync1()
        {
            Run(
                (mc, sr, sw, ct, pr) =>
                {
                    mc.UnloadAsync().Wait();
                },
                (msgs, sr, sw, ct, pr) =>
                {
                    Assert.IsTrue(msgs.SequenceEqual(new[]
                    {
                        Tuple.Create<CheckpointableRequest, object>(CheckpointableRequest.Unload, Tuple.Create(default(IProgress<int>))),
                    }));
                }
            );
        }

        private static void Run(Action<ICheckpointable, IStateReader, IStateWriter, CancellationToken, IProgress<int>> action, Action<IEnumerable<Tuple<CheckpointableRequest, object>>, IStateReader, IStateWriter, CancellationToken, IProgress<int>> assert = null)
        {
            var mc = new MyCheckpointable();

            GetStore(out var sr, out var sw);

            var ct = CancellationToken.None;
            var pr = new Progress<int>(x => { });

            action(mc, sr, sw, ct, pr);

            assert?.Invoke(mc.Requests, sr, sw, ct, pr);
        }

        private static void GetStore(out IStateReader reader, out IStateWriter writer)
        {
            var store = new InMemoryStateStore(Guid.NewGuid().ToString());
            reader = new InMemoryStateReader(store);
            writer = new InMemoryStateWriter(store, CheckpointKind.Full);
        }

        private sealed class MyCheckpointable : ICheckpointable
        {
            public List<Tuple<CheckpointableRequest, object>> Requests = new();

            public Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
            {
                Requests.Add(new Tuple<CheckpointableRequest, object>(CheckpointableRequest.Checkpoint, Tuple.Create(writer, token, progress)));
                return Task.FromResult(true);
            }

            public Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
            {
                Requests.Add(new Tuple<CheckpointableRequest, object>(CheckpointableRequest.Recover, Tuple.Create(reader, token, progress)));
                return Task.FromResult(true);
            }

            public Task UnloadAsync(IProgress<int> progress)
            {
                Requests.Add(new Tuple<CheckpointableRequest, object>(CheckpointableRequest.Unload, Tuple.Create(progress)));
                return Task.FromResult(true);
            }
        }

        private enum CheckpointableRequest
        {
            Checkpoint,
            Recover,
            Unload,
        }
    }
}
