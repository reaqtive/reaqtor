// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using global::Grpc.Net.Client;

using MagicOnion.Client;
using MagicOnion.Server;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.Remoting.Rpc.MagicOnionDemo;

//
// Prototype + demos of MagicOnion. Each test hosts a real in-process Kestrel/h2c MagicOnion server and connects a
// gRPC client — genuine client/server over HTTP/2 (the same transport stack the spike already uses for gRPC), not
// an in-memory pipe. Two facets:
//   * Unary  — request/response (the command-plane shape).
//   * StreamingHub — bidirectional: the client calls hub methods and the SERVER pushes back into the client's
//     receiver. This is the gRPC-native equivalent of the StreamJsonRpc Reply<T>/ClientAction demos (server→client
//     calls) without hand-rolling a duplex-stream message protocol.
//
[TestClass]
public class MagicOnionDemoTests
{
    static MagicOnionDemoTests()
    {
        // h2c (cleartext HTTP/2) on loopback — same switch the gRPC client side of the spike sets.
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Unary_Request_Response_Over_Grpc()
    {
        await using var host = await MagicOnionHost.StartAsync();
        using var channel = GrpcChannel.ForAddress(host.Address);

        var client = MagicOnionClient.Create<IGreeterService>(channel);
        var sum = await client.SumAsync(20, 22);

        Assert.AreEqual(42, sum);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task StreamingHub_Client_To_Server_And_Server_Pushes_To_Client()
    {
        await using var host = await MagicOnionHost.StartAsync();
        using var channel = GrpcChannel.ForAddress(host.Address);

        var receiver = new RecordingReceiver();
        var hub = await StreamingHubClient.ConnectAsync<IComputeHub, IComputeHubReceiver>(channel, receiver);
        try
        {
            // client -> server (returns a value)
            var sum = await hub.SumAsync([1, 2, 3]);
            Assert.AreEqual(6, sum);

            // client -> server, which triggers server -> client pushes into the receiver
            await hub.StartFeedAsync(count: 5, factor: 10);

            Assert.IsTrue(await receiver.Completed.Task.WaitAsync(TimeSpan.FromSeconds(20)), "the server should have completed the feed");
            CollectionAssert.AreEqual(new[] { 10, 20, 30, 40, 50 }, receiver.Values.ToArray(),
                "the server pushed transformed results back into the client's receiver over the hub (server→client)");
        }
        finally
        {
            await hub.DisposeAsync();
        }
    }

    private sealed class RecordingReceiver : IComputeHubReceiver
    {
        public ConcurrentQueue<int> Values { get; } = new();
        public TaskCompletionSource<bool> Completed { get; } = new();

        public void OnValue(int value) => Values.Enqueue(value);

        public void OnCompleted() => Completed.TrySetResult(true);
    }

    // An in-process Kestrel/h2c MagicOnion server on an ephemeral loopback port.
    private sealed class MagicOnionHost : IAsyncDisposable
    {
        private readonly WebApplication _app;

        public string Address { get; }

        private MagicOnionHost(WebApplication app, int port)
        {
            _app = app;
            Address = string.Create(System.Globalization.CultureInfo.InvariantCulture, $"http://localhost:{port}");
        }

        public static async Task<MagicOnionHost> StartAsync()
        {
            // The ephemeral port from GetFreePort can be taken between selection and bind (TOCTOU); retry on a
            // fresh port, disposing the half-built app on each failed attempt so it is never leaked (review #11/#12).
            for (var attempt = 0; ; attempt++)
            {
                var port = GetFreePort();

                var builder = WebApplication.CreateBuilder();
                builder.WebHost.ConfigureKestrel(options =>
                    options.ListenLocalhost(port, listen => listen.Protocols = HttpProtocols.Http2));
                builder.Logging.ClearProviders();
                builder.Services.AddMagicOnion();

                var app = builder.Build();
                app.MapMagicOnionService();

                try
                {
                    await app.StartAsync();
                    return new MagicOnionHost(app, port);
                }
#pragma warning disable CA1031 // Transient bind failures (port stolen after selection) are retried on a fresh port.
                catch (Exception) when (attempt < 4)
                {
                    await app.DisposeAsync();
                }
#pragma warning restore CA1031
            }
        }

        private static int GetFreePort()
        {
            using var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public async ValueTask DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
