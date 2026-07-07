// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using global::Grpc.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 0b scaffold proof (plan §10): the QueryEvaluatorHost Kestrel exe boots and answers
// ReactiveServiceControl.Ping over cleartext HTTP/2 (h2c) on a loopback port, launched out-of-process by
// GrpcProcessRunnable. This exercises the whole gRPC transport: code-first contracts (protobuf-net.Grpc),
// the host, the h2c channel, and the client — end to end over the wire (not in-process).
//
[TestClass]
public class GrpcPingTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);

    private static string HostAssembly =>
        typeof(GrpcPingTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Host_Answers_Ping_And_Control_Over_The_Wire()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var channel = GrpcConnectionFactory.CreateChannel(host.Address);
        var control = GrpcConnectionFactory.CreateControlClient(channel);

        // Ping (the readiness deliverable).
        await control.PingAsync(Empty.Instance);

        // The rest of the control surface round-trips: Configure (Remoting) then Start.
        await control.ConfigureAsync(new PlatformConfiguration { StorageType = MetadataStorageType.Remoting });
        await control.StartAsync(Empty.Instance);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Configure_Rejects_NonRemoting_StorageType_With_InvalidArgument()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var channel = GrpcConnectionFactory.CreateChannel(host.Address);
        var control = GrpcConnectionFactory.CreateControlClient(channel);

        // The Azure metadata backend is not supported on net10.0 (§2.6); Configure fails fast over the wire.
        var ex = await Assert.ThrowsExactlyAsync<RpcException>(
            () => control.ConfigureAsync(new PlatformConfiguration { StorageType = MetadataStorageType.Azure }));

        Assert.AreEqual(StatusCode.InvalidArgument, ex.StatusCode);
    }
}
