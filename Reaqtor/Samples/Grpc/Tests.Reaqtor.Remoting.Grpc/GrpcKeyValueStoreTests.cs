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
using Reaqtor.Remoting.KeyValueStore;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 3: the KeyValueStore adapter + host (plan §4.4). GrpcKeyValueStoreConnection implements the
// engine-facing ITransactionalKeyValueStoreConnection over the async gRPC KeyValueStore service, backed by the
// engine's REAL in-host KV store (EngineHost.KeyValueStoreConnection). Two facets are asserted:
//   1. Round-trip parity: a full transactional scenario (create tx, add, contains, indexer get, commit, re-read,
//      enumerate snapshot, update, remove, serialize/deserialize) produces results identical to an in-proc oracle.
//   2. §6.2 fault contract: an absent-key indexer lookup re-raises the SAME CLR exception type over the wire as
//      the in-proc store throws (KeyNotFoundException), via the GrpcFault ErrorInfo trailers.
//
[TestClass]
public class GrpcKeyValueStoreTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);

    private static string HostAssembly =>
        typeof(GrpcKeyValueStoreTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_KeyValueStore_RoundTrip_Parity_With_InProc_Oracle()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcKeyValueStoreConnection(host.Address);
        var oracle = new KeyValueStoreConnection();

        // Readiness probe round-trips over the wire.
        grpc.Ping();

        var grpcResult = RunScenario(grpc);
        var oracleResult = RunScenario(oracle);

        Assert.AreEqual(oracleResult.ContainsK1InTx, grpcResult.ContainsK1InTx, nameof(KvScenarioResult.ContainsK1InTx));
        Assert.AreEqual(oracleResult.ContainsMissingInTx, grpcResult.ContainsMissingInTx, nameof(KvScenarioResult.ContainsMissingInTx));
        Assert.AreEqual(oracleResult.V1InTx, grpcResult.V1InTx, nameof(KvScenarioResult.V1InTx));
        Assert.AreEqual(oracleResult.V1AfterCommit, grpcResult.V1AfterCommit, nameof(KvScenarioResult.V1AfterCommit));
        Assert.AreEqual(oracleResult.EnumerateSnapshot, grpcResult.EnumerateSnapshot, nameof(KvScenarioResult.EnumerateSnapshot));
        Assert.AreEqual(oracleResult.V1AfterUpdate, grpcResult.V1AfterUpdate, nameof(KvScenarioResult.V1AfterUpdate));
        Assert.AreEqual(oracleResult.ContainsK2AfterRemove, grpcResult.ContainsK2AfterRemove, nameof(KvScenarioResult.ContainsK2AfterRemove));
        Assert.AreEqual(oracleResult.V1AfterReload, grpcResult.V1AfterReload, nameof(KvScenarioResult.V1AfterReload));

        // Full-record parity as a backstop.
        Assert.AreEqual(oracleResult, grpcResult);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_KeyValueStore_AbsentKey_ReRaises_Typed_Exception()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcKeyValueStoreConnection(host.Address);
        var oracle = new KeyValueStoreConnection();

        const string table = "faults";

        var oracleType = CaptureAbsentKeyExceptionType(oracle, table);
        var grpcType = CaptureAbsentKeyExceptionType(grpc, table);

        // The in-proc store throws KeyNotFoundException on an absent-key indexer lookup; the gRPC adapter must
        // re-raise the SAME CLR type over the wire (§6.2 — engine callers depend on the exception contract).
        Assert.AreEqual(typeof(KeyNotFoundException), oracleType, "oracle absent-key exception type");
        Assert.AreEqual(oracleType, grpcType, "gRPC must re-raise the same exception type as the oracle (§6.2)");
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_KeyValueStore_Enumerate_BadTransaction_ReRaises_Typed_Exception()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcKeyValueStoreConnection(host.Address);
        var oracle = new KeyValueStoreConnection();

        const long absentTransactionId = 999999L;

        // The STREAMING Enumerate path must surface faults with the same fidelity as the unary paths: an absent
        // transaction id throws KeyNotFoundException in-proc, and the gRPC adapter must re-raise the same CLR type
        // over the server-streamed call (regression guard for the streaming fault-mapping gap).
        Assert.ThrowsExactly<KeyNotFoundException>(() => oracle.GetEnumerator(absentTransactionId, "t"));
        Assert.ThrowsExactly<KeyNotFoundException>(() => grpc.GetEnumerator(absentTransactionId, "t"));
    }

    private static Type CaptureAbsentKeyExceptionType(ITransactionalKeyValueStoreConnection connection, string table)
    {
        var tx = connection.CreateTransaction();
        try
        {
            _ = connection[tx, table, "absent-key"];
            throw new AssertFailedException("expected an exception for an absent-key lookup, but none was thrown");
        }
        catch (Exception ex) when (ex is not AssertFailedException)
        {
            return ex.GetType();
        }
        finally
        {
            connection.Dispose(tx);
        }
    }

    private sealed record KvScenarioResult(
        bool ContainsK1InTx,
        bool ContainsMissingInTx,
        string V1InTx,
        string V1AfterCommit,
        string EnumerateSnapshot,
        string V1AfterUpdate,
        bool ContainsK2AfterRemove,
        string V1AfterReload);

    //
    // A self-contained transactional scenario whose every observable output is captured into a comparable record.
    // Run against both transports, the records must be equal.
    //
    private static KvScenarioResult RunScenario(ITransactionalKeyValueStoreConnection connection)
    {
        const string table = "kvtable";

        connection.Clear();

        // Transaction 1: stage writes, observe uncommitted reads, commit.
        var tx1 = connection.CreateTransaction();
        connection.Add(tx1, table, "k1", [1, 1]);
        connection.Add(tx1, table, "k2", [2, 2]);
        var containsK1InTx = connection.Contains(tx1, table, "k1");
        var containsMissingInTx = connection.Contains(tx1, table, "missing");
        var v1InTx = Hex(connection[tx1, table, "k1"]);
        connection.Commit(tx1);
        connection.Dispose(tx1);

        // Transaction 2: read committed state, enumerate, update, remove, commit.
        var tx2 = connection.CreateTransaction();
        var v1AfterCommit = Hex(connection[tx2, table, "k1"]);
        var snapshot = EnumerateSorted(connection, tx2, table);
        connection.Update(tx2, table, "k1", [9, 9, 9]);
        var v1AfterUpdate = Hex(connection[tx2, table, "k1"]);
        connection.Remove(tx2, table, "k2");
        var containsK2AfterRemove = connection.Contains(tx2, table, "k2");
        connection.Commit(tx2);
        connection.Dispose(tx2);

        // Serialize the whole store, clear, deserialize — committed state survives the round-trip.
        var blob = connection.SerializeStore();
        connection.Clear();
        connection.DeserializeStore(blob);

        var tx3 = connection.CreateTransaction();
        var v1AfterReload = Hex(connection[tx3, table, "k1"]);
        connection.Dispose(tx3);

        return new KvScenarioResult(
            containsK1InTx,
            containsMissingInTx,
            v1InTx,
            v1AfterCommit,
            snapshot,
            v1AfterUpdate,
            containsK2AfterRemove,
            v1AfterReload);
    }

    private static string EnumerateSorted(ITransactionalKeyValueStoreConnection connection, long transactionId, string table)
    {
        var entries = new List<string>();
        var enumerator = connection.GetEnumerator(transactionId, table);
        while (enumerator.MoveNext())
        {
            entries.Add(enumerator.Current.Key + "=" + Hex(enumerator.Current.Value));
        }

        entries.Sort(StringComparer.Ordinal);
        return string.Join(";", entries);
    }

    private static string Hex(byte[] bytes) => bytes == null ? "<null>" : Convert.ToHexString(bytes);
}
