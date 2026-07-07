// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nerdbank.Streams;

using StreamJsonRpc;

namespace Reaqtor.Remoting.Rpc.Demo
{
    //
    // Prototype + demos of StreamJsonRpc as an alternative RPC transport. Each test showcases a capability that the
    // gRPC port of the spike could NOT express, and maps it to the archived .NET Remoting primitive
    // (Nuqleon.Runtime.Remoting.Tasks) it is the modern, MarshalByRefObject-free equivalent of.
    //
    // Wiring: two JsonRpc endpoints over a single in-memory full-duplex pipe (Nerdbank.Streams.FullDuplexStream).
    // Because JSON-RPC is symmetric, EACH endpoint can both serve and call — so the server can invoke methods on the
    // client and receive client objects (callbacks/observers) by reference. That bidirectional, callback-marshaling
    // shape is exactly what .NET Remoting MBR gave and what gRPC's unary/one-way model does not.
    //
    [TestClass]
    public class StreamJsonRpcDemoTests
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20);

        // ----- Demo A: the Reply<T> analog (server pushes into a client-supplied MARSHALED observer) -----
        [TestMethod]
        public async Task A_Server_Pushes_Results_Into_Marshaled_Client_Observer()
        {
            using var harness = new RpcHarness(new NoopClientCallback());
            using var observer = new RecordingObserver();

            await harness.Server.RunIntoObserverAsync([1, 2, 3], observer, CancellationToken.None);

            Assert.IsTrue(await observer.Completed.Task.WaitAsync(Timeout), "observer should have been completed by the server");
            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, observer.Values.ToArray(),
                "the server pushed transformed results into the client's observer by calling it back across the connection (Reply<T>)");
            Assert.IsNull(observer.Error);
        }

        // ----- Demo B: IProgress<T> marshaling (server reports progress to the client) -----
        [TestMethod]
        public async Task B_Server_Reports_Progress_To_Client()
        {
            using var harness = new RpcHarness(new NoopClientCallback());
            var reports = new ConcurrentQueue<int>();
            var progress = new Progress<int>(p => reports.Enqueue(p));

            var sum = await harness.Server.SumWithProgressAsync([5, 10, 15], progress, CancellationToken.None);

            Assert.AreEqual(30, sum);
            // Progress<T> callbacks are delivered asynchronously; wait until the final report (one per input) arrives.
            Assert.IsTrue(await WaitUntilAsync(() => reports.Contains(3), Timeout), "progress reports should reach the client");
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, reports.OrderBy(x => x).ToArray());
        }

        // ----- Demo C: cancellation of an in-flight server op (RemoteCancellationDisposable / ICancellationProvider) -----
        [TestMethod]
        public async Task C_Client_Cancels_InFlight_Server_Operation()
        {
            using var harness = new RpcHarness(new NoopClientCallback());
            using var cts = new CancellationTokenSource();

            var call = harness.Server.SlowEchoAsync(42, TimeSpan.FromSeconds(30), cts.Token);
            cts.CancelAfter(TimeSpan.FromMilliseconds(200));

            Exception captured = null;
            try
            {
                await call;
            }
#pragma warning disable CA1031 // The test captures whatever the cancelled call throws to assert its shape.
            catch (Exception ex)
            {
                captured = ex;
            }
#pragma warning restore CA1031

            Assert.IsInstanceOfType(captured, typeof(OperationCanceledException),
                "cancelling the client token aborts the in-flight server operation (first-class CancellationToken)");
        }

        // ----- Demo D: IAsyncEnumerable<T> server streaming -----
        [TestMethod]
        public async Task D_Server_Streams_Via_IAsyncEnumerable()
        {
            using var harness = new RpcHarness(new NoopClientCallback());

            var items = new List<int>();
            await foreach (var x in harness.Server.CountAsync(5, CancellationToken.None))
            {
                items.Add(x);
            }

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, items.ToArray());
        }

        // ----- Demo E: full-duplex — the SERVER calls back into the CLIENT (ClientAction / §3.4) -----
        [TestMethod]
        public async Task E_Server_Calls_Back_Into_Client_Mid_Request()
        {
            var callback = new DoublingClientCallback();
            using var harness = new RpcHarness(callback);

            // Outer client->server call; inside it the server makes nested server->client calls on the SAME connection.
            var total = await harness.Server.AggregateViaClientAsync([1, 2, 3, 4], CancellationToken.None);

            Assert.AreEqual(20, total, "(1+2+3+4) doubled by the client-side callback = 20");
            Assert.AreEqual(4, callback.Calls, "the server invoked the client callback once per key");
        }

        // ----- Demo F: prove it is REAL RPC over the pipe by tapping the wire in both directions -----
        [TestMethod]
        public async Task F_Wire_Trace_Proves_Real_Bidirectional_Client_Server_Interaction()
        {
            // Tap both ends of the duplex pipe so we capture the actual bytes that cross it. With FullDuplexStream,
            // writes to the client's stream are client->server traffic; writes to the server's stream are
            // server->client traffic. If these were in-process method calls, NOTHING would cross the wire.
            var (clientStream, serverStream) = FullDuplexStream.CreatePair();
            var clientToServer = new StringBuilder();
            var serverToClient = new StringBuilder();
            var callback = new DoublingClientCallback();

            using var clientTap = new TappingStream(clientStream, clientToServer);
            using var serverTap = new TappingStream(serverStream, serverToClient);

            var serverImpl = new ComputeService();
            using var serverRpc = new JsonRpc(serverTap);
            serverRpc.AddLocalRpcTarget(serverImpl);
            serverImpl.Client = serverRpc.Attach<IClientCallback>();
            serverRpc.StartListening();

            using var clientRpc = new JsonRpc(clientTap);
            clientRpc.AddLocalRpcTarget(callback);
            var server = clientRpc.Attach<IComputeService>();
            clientRpc.StartListening();

            // The bidirectional scenario: a client->server call inside which the server calls back into the client.
            var total = await server.AggregateViaClientAsync([1, 2, 3, 4], CancellationToken.None);
            Assert.AreEqual(20, total);
            Assert.AreEqual(4, callback.Calls);

            var c2s = clientToServer.ToString();
            var s2c = serverToClient.ToString();

            // Both directions carried genuine JSON-RPC envelopes (not in-process calls)...
            StringAssert.Contains(c2s, "jsonrpc", "client->server traffic should be JSON-RPC framed");
            StringAssert.Contains(s2c, "jsonrpc", "server->client traffic should be JSON-RPC framed");
            // ...the client invoked the server (request method name on the wire)...
            StringAssert.Contains(c2s, "Aggregate", "the client's request to the server should be on the wire");
            // ...and the server invoked the client BACK over the same connection (the bidirectional proof).
            StringAssert.Contains(s2c, "Resolve", "the server's callbacks to the client should be on the wire");

            // Emit a human-readable transcript next to the test assembly for inspection.
            var transcriptPath = Path.Combine(AppContext.BaseDirectory, "sjr-wire-transcript.txt");
            File.WriteAllText(
                transcriptPath,
                "=== CLIENT -> SERVER ===" + Environment.NewLine + c2s + Environment.NewLine +
                "=== SERVER -> CLIENT ===" + Environment.NewLine + s2c + Environment.NewLine);
        }

        private static async Task<bool> WaitUntilAsync(Func<bool> predicate, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                if (predicate())
                {
                    return true;
                }
                await Task.Delay(25);
            }
            return predicate();
        }

        //
        // Two JsonRpc endpoints over one in-memory full-duplex pipe. The server end registers the ComputeService and
        // a proxy back to the client; the client end registers its callback and a proxy to the server.
        //
        private sealed class RpcHarness : IDisposable
        {
            private readonly JsonRpc _serverRpc;
            private readonly JsonRpc _clientRpc;

            public IComputeService Server { get; }

            public RpcHarness(IClientCallback clientCallback)
            {
                var (clientStream, serverStream) = FullDuplexStream.CreatePair();

                var serverImpl = new ComputeService();
                _serverRpc = new JsonRpc(serverStream);
                _serverRpc.AddLocalRpcTarget(serverImpl);
                serverImpl.Client = _serverRpc.Attach<IClientCallback>(); // server -> client proxy (full-duplex)
                _serverRpc.StartListening();

                _clientRpc = new JsonRpc(clientStream);
                _clientRpc.AddLocalRpcTarget(clientCallback);
                Server = _clientRpc.Attach<IComputeService>(); // client -> server proxy
                _clientRpc.StartListening();
            }

            public void Dispose()
            {
                _clientRpc.Dispose();
                _serverRpc.Dispose();
            }
        }

        private sealed class NoopClientCallback : IClientCallback
        {
            public Task<int> ResolveAsync(int key) => Task.FromResult(0);
        }

        private sealed class DoublingClientCallback : IClientCallback
        {
            private int _calls;

            public int Calls => Volatile.Read(ref _calls);

            public Task<int> ResolveAsync(int key)
            {
                Interlocked.Increment(ref _calls);
                return Task.FromResult(key * 2);
            }
        }

        // The client-side observer the server pushes into — the concrete Reply<T> recipient.
        private sealed class RecordingObserver : IResultObserver
        {
            public ConcurrentQueue<int> Values { get; } = new();
            public string Error { get; private set; }
            public TaskCompletionSource<bool> Completed { get; } = new();

            public Task OnNextAsync(int value)
            {
                Values.Enqueue(value);
                return Task.CompletedTask;
            }

            public Task OnErrorAsync(string error)
            {
                Error = error;
                Completed.TrySetResult(false);
                return Task.CompletedTask;
            }

            public Task OnCompletedAsync()
            {
                Completed.TrySetResult(true);
                return Task.CompletedTask;
            }

            public void Dispose()
            {
            }
        }

        //
        // A pass-through Stream that records a UTF-8 copy of every byte WRITTEN through it, so a test can inspect the
        // actual JSON-RPC envelopes crossing the pipe. Reads and all other members delegate to the inner stream.
        //
        private sealed class TappingStream : Stream
        {
            private readonly Stream _inner;
            private readonly StringBuilder _writes;
            private bool _disposed;

            public TappingStream(Stream inner, StringBuilder writes)
            {
                _inner = inner;
                _writes = writes;
            }

            private void Capture(ReadOnlySpan<byte> data)
            {
                var text = Encoding.UTF8.GetString(data);
                lock (_writes)
                {
                    _writes.Append(text);
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                Capture(buffer.AsSpan(offset, count));
                _inner.Write(buffer, offset, count);
            }

            public override void Write(ReadOnlySpan<byte> buffer)
            {
                Capture(buffer);
                _inner.Write(buffer);
            }

            public override void WriteByte(byte value)
            {
                Span<byte> one = [value];
                Capture(one);
                _inner.WriteByte(value);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                Capture(buffer.AsSpan(offset, count));
                return _inner.WriteAsync(buffer, offset, count, cancellationToken);
            }

            public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            {
                Capture(buffer.Span);
                return _inner.WriteAsync(buffer, cancellationToken);
            }

            public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _inner.ReadAsync(buffer, offset, count, cancellationToken);

            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => _inner.ReadAsync(buffer, cancellationToken);

            public override void Flush() => _inner.Flush();

            public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

            public override bool CanRead => _inner.CanRead;
            public override bool CanSeek => _inner.CanSeek;
            public override bool CanWrite => _inner.CanWrite;
            public override long Length => _inner.Length;
            public override long Position { get => _inner.Position; set => _inner.Position = value; }
            public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
            public override void SetLength(long value) => _inner.SetLength(value);

            protected override void Dispose(bool disposing)
            {
                if (disposing && !_disposed)
                {
                    _disposed = true;
                    _inner.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
