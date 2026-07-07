// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Platform.Grpc;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 1 (MVP) keystone: a real reactive command runs end-to-end over gRPC. The host runs the SAME in-proc
// engine the oracle runs (InMemoryReactivePlatform + CoreDeployable) behind a gRPC Execute facade; the client is
// a RemotingClientContext over the gRPC command channel. This proves the full command path over the wire —
// client lowers an Expression to Bonsai/DataModel JSON, the JSON crosses as Execute.command_text, the in-host QC
// parses it and writes metadata — i.e. exactly the oracle's command path (0a.8) with a gRPC hop on client→QC.
//
[TestClass]
public class GrpcCommandPathTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);

    private static string HostAssembly =>
        typeof(GrpcCommandPathTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_CommandPath_DefineUndefine_Observable_Over_The_Wire()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var connection = new GrpcReactiveServiceConnection(host.Address);
        var ctx = new RemotingClientContext(connection);

        var uri = new Uri("reactor://grpc/test/observable");

        // Define an observable (aliasing the standard Empty operator deployed in-host) — a New/Observable command
        // serialized to Bonsai JSON and executed over the gRPC command channel against the in-host engine.
        var empty = ctx.GetObservable<int>(new Uri(Reaqtor.Remoting.Client.Constants.Observable.Empty.NoArgument));
        await ctx.DefineObservableAsync<int>(uri, empty, state: null, CancellationToken.None);

        // Undefine round-trips the Remove/Observable verb over the wire too.
        await ctx.UndefineObservableAsync(uri, CancellationToken.None);
    }
}
