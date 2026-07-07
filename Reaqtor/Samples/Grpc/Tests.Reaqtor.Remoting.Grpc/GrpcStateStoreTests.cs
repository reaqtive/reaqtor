// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.StateStore;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 2: the StateStore adapter + host (plan §4.4). GrpcStateStoreConnection implements the engine-facing
// IReactiveStateStoreConnection over the async gRPC StateStore service, backed by the engine's REAL checkpoint
// store (EngineHost.StateStoreConnection). Two facets are asserted:
//   1. Round-trip parity: every operation (incl. the bool+out TryGet pairs, Serialize/Deserialize) matches an
//      in-proc oracle StateStoreConnection run side-by-side with the same calls.
//   2. §4.4.1 checkpoint batch: the client-streamed AddOrUpdateItems collapses N per-item commits into one call
//      (measured over the wire via the server's round-trip counters), with identical resulting state.
//
[TestClass]
public class GrpcStateStoreTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);

    private static string HostAssembly =>
        typeof(GrpcStateStoreTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_StateStore_RoundTrip_Parity_With_InProc_Oracle()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcStateStoreConnection(host.Address);
        var oracle = new StateStoreConnection();

        // Both start from a known-empty store.
        grpc.Clear();
        oracle.Clear();

        // Readiness probe round-trips over the wire.
        grpc.Ping();

        AssertParity(grpc, oracle);

        // Add items across two categories.
        var items = new (string Category, string Key, byte[] Value)[]
        {
            ("cat-a", "k1", new byte[] { 1, 2, 3 }),
            ("cat-a", "k2", new byte[] { 4, 5 }),
            ("cat-b", "k1", new byte[] { 6 }),
        };
        foreach (var (cat, key, val) in items)
        {
            grpc.AddOrUpdateItem(cat, key, val);
            oracle.AddOrUpdateItem(cat, key, val);
        }

        AssertParity(grpc, oracle);
        foreach (var (cat, key, val) in items)
        {
            Assert.IsTrue(grpc.TryGetItem(cat, key, out var got), FormattableString.Invariant($"missing {cat}/{key}"));
            CollectionAssert.AreEqual(val, got, FormattableString.Invariant($"value {cat}/{key}"));
        }

        // Update an existing item (overwrite).
        grpc.AddOrUpdateItem("cat-a", "k1", [9, 9, 9]);
        oracle.AddOrUpdateItem("cat-a", "k1", [9, 9, 9]);
        AssertParity(grpc, oracle);

        // bool+out semantics on absent items/categories match the oracle (false, null out).
        Assert.AreEqual(
            oracle.TryGetItem("cat-a", "absent", out _),
            grpc.TryGetItem("cat-a", "absent", out var absentValue));
        Assert.IsFalse(grpc.TryGetItem("cat-a", "absent", out _));
        Assert.IsNull(absentValue);

        Assert.AreEqual(
            oracle.TryGetItemKeys("absent-cat", out _),
            grpc.TryGetItemKeys("absent-cat", out var absentKeys));
        Assert.IsFalse(grpc.TryGetItemKeys("absent-cat", out _));
        Assert.IsNull(absentKeys);

        // Remove an item.
        grpc.RemoveItem("cat-a", "k2");
        oracle.RemoveItem("cat-a", "k2");
        AssertParity(grpc, oracle);
        Assert.IsFalse(grpc.TryGetItem("cat-a", "k2", out _));

        // Serialize on the wire, clear, then deserialize back — the rehydrated store matches the oracle again.
        var blob = grpc.SerializeStateStore();
        Assert.IsTrue(blob is { Length: > 0 }, "serialized state-store blob was empty");

        grpc.Clear();
        Assert.IsFalse(grpc.GetCategories().Any(), "store should be empty after Clear");

        grpc.DeserializeStateStore(blob);
        AssertParity(grpc, oracle);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_StateStore_Checkpoint_Batch_Collapses_RoundTrips()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcStateStoreConnection(host.Address);
        grpc.Clear();

        const int N = 25;

        // Per-item path: one unary AddOrUpdateItem RPC per item.
        var before = grpc.GetCallStats();
        for (var i = 0; i < N; i++)
        {
            grpc.AddOrUpdateItem("per-item", "k" + i.ToString(CultureInfo.InvariantCulture), [(byte)i]);
        }

        var afterUnary = grpc.GetCallStats();
        Assert.AreEqual(before.UnaryAddOrUpdateCount + N, afterUnary.UnaryAddOrUpdateCount, "per-item path should issue one RPC per item");
        Assert.AreEqual(before.BatchCount, afterUnary.BatchCount, "per-item path should issue no batch calls");

        // Batched checkpoint path (§4.4.1): N items committed in ONE client-streamed call.
        var batch = Enumerable.Range(0, N)
            .Select(i => ("batch", "k" + i.ToString(CultureInfo.InvariantCulture), new byte[] { (byte)i }))
            .ToList();
        grpc.AddOrUpdateItems(batch);

        var afterBatch = grpc.GetCallStats();
        Assert.AreEqual(afterUnary.BatchCount + 1, afterBatch.BatchCount, "batch path should be a single streamed RPC");
        Assert.AreEqual(afterUnary.BatchItemCount + N, afterBatch.BatchItemCount, "the one batch call should carry all N items");
        Assert.AreEqual(afterUnary.UnaryAddOrUpdateCount, afterBatch.UnaryAddOrUpdateCount, "batch path should issue no per-item unary calls");

        // Both paths produced identical, retrievable state.
        Assert.IsTrue(grpc.TryGetItemKeys("batch", out var keys));
        Assert.AreEqual(N, keys.Count());
        for (var i = 0; i < N; i++)
        {
            Assert.IsTrue(grpc.TryGetItem("batch", "k" + i.ToString(CultureInfo.InvariantCulture), out var value));
            CollectionAssert.AreEqual(new byte[] { (byte)i }, value);
        }
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_StateStore_NullArgument_ReRaises_Typed_Exception()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        using var grpc = new GrpcStateStoreConnection(host.Address);
        var oracle = new StateStoreConnection();

        // The in-proc store throws ArgumentNullException on a null category; the gRPC adapter must re-raise the
        // SAME CLR type over the wire (the StateStore client now routes through GrpcFault like the other stores).
        Assert.ThrowsExactly<ArgumentNullException>(() => oracle.AddOrUpdateItem(null, "k", [1]));
        Assert.ThrowsExactly<ArgumentNullException>(() => grpc.AddOrUpdateItem(null, "k", [1]));
    }

    //
    // Logical parity over both connections: same categories, same keys per category, same value bytes. Compared
    // order-insensitively (the in-memory store does not promise enumeration order).
    //
    private static void AssertParity(GrpcStateStoreConnection grpc, StateStoreConnection oracle)
    {
        var grpcCategories = grpc.GetCategories().OrderBy(c => c, StringComparer.Ordinal).ToList();
        var oracleCategories = oracle.GetCategories().OrderBy(c => c, StringComparer.Ordinal).ToList();
        CollectionAssert.AreEqual(oracleCategories, grpcCategories, "categories");

        foreach (var category in oracleCategories)
        {
            var grpcFound = grpc.TryGetItemKeys(category, out var grpcKeysEnumerable);
            var oracleFound = oracle.TryGetItemKeys(category, out var oracleKeysEnumerable);
            Assert.AreEqual(oracleFound, grpcFound, FormattableString.Invariant($"TryGetItemKeys({category}).found"));

            var grpcKeys = (grpcKeysEnumerable ?? []).OrderBy(k => k, StringComparer.Ordinal).ToList();
            var oracleKeys = (oracleKeysEnumerable ?? []).OrderBy(k => k, StringComparer.Ordinal).ToList();
            CollectionAssert.AreEqual(oracleKeys, grpcKeys, FormattableString.Invariant($"keys({category})"));

            foreach (var key in oracleKeys)
            {
                var grpcHas = grpc.TryGetItem(category, key, out var grpcValue);
                var oracleHas = oracle.TryGetItem(category, key, out var oracleValue);
                Assert.AreEqual(oracleHas, grpcHas, FormattableString.Invariant($"TryGetItem({category},{key}).found"));
                CollectionAssert.AreEqual(oracleValue, grpcValue, FormattableString.Invariant($"value({category},{key})"));
            }
        }
    }
}
