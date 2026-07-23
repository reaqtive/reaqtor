// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 4: the Metadata/Storage adapter + host (plan §4.4 / §4.4.2). GrpcStorageConnection implements the
// engine-facing flat IReactiveStorageConnection over the async gRPC Storage service, backed by the engine's REAL
// metadata store (EngineHost.StorageConnection). Only flat CRUD crosses the wire — the IQueryable metadata layer
// stays engine-side (§2.6). Asserted:
//   1. Round-trip parity: a CRUD scenario (TryGet absent → add → TryGet → enumerate → delete → enumerate) is
//      record-equal to an in-proc oracle StorageConnection.
//   2. Fault fidelity: a duplicate AddEntity faults over the wire carrying the same CLR type name (§6.1) the
//      in-proc store throws (a custom ReactiveProcessingStorageException — identity surfaced via RemoteTypeName).
//
[TestClass]
public class GrpcStorageTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(120);

    private static string HostAssembly =>
        typeof(GrpcStorageTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Storage_RoundTrip_Parity_With_InProc_Oracle()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcStorageConnection(host.Address);
        var oracle = new StorageConnection();

        // Readiness probe round-trips over the wire.
        grpc.Ping();

        var grpcResult = RunScenario(grpc);
        var oracleResult = RunScenario(oracle);

        Assert.AreEqual(oracleResult.AbsentBeforeAdd, grpcResult.AbsentBeforeAdd, nameof(StorageScenarioResult.AbsentBeforeAdd));
        Assert.AreEqual(oracleResult.FoundK1, grpcResult.FoundK1, nameof(StorageScenarioResult.FoundK1));
        Assert.AreEqual(oracleResult.K1Description, grpcResult.K1Description, nameof(StorageScenarioResult.K1Description));
        Assert.AreEqual(oracleResult.AllEntities, grpcResult.AllEntities, nameof(StorageScenarioResult.AllEntities));
        Assert.AreEqual(oracleResult.FoundK1AfterDelete, grpcResult.FoundK1AfterDelete, nameof(StorageScenarioResult.FoundK1AfterDelete));
        Assert.AreEqual(oracleResult.AllEntitiesAfterDelete, grpcResult.AllEntitiesAfterDelete, nameof(StorageScenarioResult.AllEntitiesAfterDelete));

        Assert.AreEqual(oracleResult, grpcResult);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Storage_DuplicateAdd_Faults_With_Same_Type_As_Oracle()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcStorageConnection(host.Address);
        var oracle = new StorageConnection();

        const string collection = "dupes";

        grpc.AddEntity(collection, "k", Entity(("Name", 6, "x")));
        oracle.AddEntity(collection, "k", Entity(("Name", 6, "x")));

        var oracleFault = Capture(() => oracle.AddEntity(collection, "k", Entity(("Name", 6, "y"))));
        var grpcFault = Capture(() => grpc.AddEntity(collection, "k", Entity(("Name", 6, "y"))));

        Assert.IsNotNull(oracleFault, "oracle should fault on a duplicate add");
        Assert.IsNotNull(grpcFault, "gRPC should fault on a duplicate add");

        // The custom CLR type can't be reconstructed (it relied on BinaryFormatter, §6.1), but its identity is
        // preserved as RemoteTypeName on the rehydrated GrpcRemoteException.
        var remote = grpcFault as GrpcRemoteException;
        Assert.IsNotNull(remote, "gRPC fault should rehydrate as GrpcRemoteException");
        Assert.AreEqual(oracleFault.GetType().FullName, remote.RemoteTypeName, "fault type name should round-trip (§6.1)");
    }

    private sealed record StorageScenarioResult(
        bool AbsentBeforeAdd,
        bool FoundK1,
        string K1Description,
        string AllEntities,
        bool FoundK1AfterDelete,
        string AllEntitiesAfterDelete);

    private static StorageScenarioResult RunScenario(IReactiveStorageConnection connection)
    {
        const string collection = "metadata-coll";

        var absentBeforeAdd = connection.TryGetEntity(collection, "k1", out _);

        connection.AddEntity(collection, "k1", Entity(("Name", 6, "alpha"), ("Count", 4, "1")));
        connection.AddEntity(collection, "k2", Entity(("Name", 6, "beta")));

        var foundK1 = connection.TryGetEntity(collection, "k1", out var k1);
        var k1Description = foundK1 ? Describe(k1) : "<none>";

        var allEntities = DescribeAll(connection.GetEntities(collection));

        connection.DeleteEntity(collection, "k1");
        var foundK1AfterDelete = connection.TryGetEntity(collection, "k1", out _);
        var allEntitiesAfterDelete = DescribeAll(connection.GetEntities(collection));

        return new StorageScenarioResult(absentBeforeAdd, foundK1, k1Description, allEntities, foundK1AfterDelete, allEntitiesAfterDelete);
    }

    private static StorageEntity Entity(params (string Name, int Type, string Data)[] properties)
    {
        var bag = new Dictionary<string, StorageEntityProperty>();
        foreach (var (name, type, data) in properties)
        {
            bag[name] = new StorageEntityProperty { Type = type, Data = data };
        }

        return new StorageEntity(bag);
    }

    private static string Describe(StorageEntity entity)
        => string.Join("|", entity.Properties
            .Select(p => p.Key + ":" + p.Value.Type.ToString(System.Globalization.CultureInfo.InvariantCulture) + ":" + p.Value.Data)
            .OrderBy(s => s, StringComparer.Ordinal));

    private static string DescribeAll(IEnumerable<StorageEntity> entities)
        => string.Join(";", entities.Select(Describe).OrderBy(s => s, StringComparer.Ordinal));

    private static Exception Capture(Action action)
    {
        try
        {
            action();
            return null;
        }
#pragma warning disable CA1031 // The test deliberately captures whatever the operation throws.
        catch (Exception ex)
        {
            return ex;
        }
#pragma warning restore CA1031
    }
}
