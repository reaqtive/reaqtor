// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using global::Grpc.Core;
using global::Grpc.Net.Client;

using ProtoBuf.Grpc.Client;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Client;

//
// Client-side synchronous adapter (plan §4.4 / Milestone 2): implements the engine-facing
// IReactiveStateStoreConnection over the async gRPC StateStore service. Each per-item method is a unary RPC
// bridged sync-over-async (§5.2); the bool+out methods read {found, …} response DTOs back into out-parameters.
// AddOrUpdateItems is the §4.4.1 client-streamed checkpoint batch (N staged items → one call); GetCallStats
// reads the server's round-trip counters back over the wire for the per-item-vs-batch measurement.
//
/// <summary>A gRPC-backed <see cref="IReactiveStateStoreConnection"/> over the StateStore service.</summary>
public sealed class GrpcStateStoreConnection : ReactiveConnectionBase, IReactiveStateStoreConnection, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly bool _ownsChannel;
    private readonly IStateStoreService _service;

    /// <summary>Connects to a state-store host address, owning the underlying channel.</summary>
    public GrpcStateStoreConnection(string address)
        : this(GrpcConnectionFactory.CreateChannel(address), ownsChannel: true)
    {
    }

    /// <summary>Connects over an existing channel.</summary>
    public GrpcStateStoreConnection(GrpcChannel channel, bool ownsChannel = false)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _ownsChannel = ownsChannel;
        _service = channel.CreateGrpcService<IStateStoreService>();
    }

    public IEnumerable<string> GetCategories()
        => Invoke(() => _service.GetCategoriesAsync(Empty.Instance)).Items ?? [];

    public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
    {
        var response = Invoke(() => _service.TryGetItemKeysAsync(new Category { Name = category }));
        keys = response.Found ? (response.Keys ?? []) : null;
        return response.Found;
    }

    public bool TryGetItem(string category, string key, out byte[] value)
    {
        var response = Invoke(() => _service.TryGetItemAsync(new ItemKey { Category = category, Key = key }));
        value = response.Found ? response.Value : null;
        return response.Found;
    }

    public void AddOrUpdateItem(string category, string key, byte[] value)
        => Invoke(() => _service.AddOrUpdateItemAsync(new ItemValue { Category = category, Key = key, Value = value }));

    public void RemoveItem(string category, string key)
        => Invoke(() => _service.RemoveItemAsync(new ItemKey { Category = category, Key = key }));

    public void Clear()
        => Invoke(() => _service.ClearAsync(Empty.Instance));

    public byte[] SerializeStateStore()
        => Invoke(() => _service.SerializeAsync(Empty.Instance)).Data;

    public void DeserializeStateStore(byte[] bytes)
        => Invoke(() => _service.DeserializeAsync(new Bytes { Data = bytes }));

    /// <summary>Round-trips the readiness probe to the state-store host (re-implements the base no-op).</summary>
    public new void Ping()
        => Invoke(() => _service.PingAsync(Empty.Instance));

    /// <summary>
    /// Checkpoint commit as a single client-streamed batch (§4.4.1): the writer's per-item loop feeds one
    /// streamed call instead of N unary <see cref="AddOrUpdateItem"/> round-trips.
    /// </summary>
    public void AddOrUpdateItems(IEnumerable<(string Category, string Key, byte[] Value)> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        Invoke(() => _service.AddOrUpdateItemsAsync(ToStream(items)));

        static async IAsyncEnumerable<ItemValue> ToStream(IEnumerable<(string Category, string Key, byte[] Value)> source)
        {
            foreach (var (category, key, value) in source)
            {
                yield return new ItemValue { Category = category, Key = key, Value = value };
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }

    /// <summary>Reads the server's round-trip counters (unary item writes vs batched calls/items) — §4.4.1.</summary>
    public CallStats GetCallStats()
        => Invoke(() => _service.GetCallStatsAsync(Empty.Instance));

    public void Dispose()
    {
        if (_ownsChannel)
        {
            _channel.Dispose();
        }
    }

    // Sync-over-async bridge (§5.2) with fault re-raise (§6.2): an RpcException carrying ErrorInfo trailers is
    // converted back into the original CLR exception before it reaches the engine — consistent with the KeyValue
    // and Storage client adapters.
    private static T Invoke<T>(Func<Task<T>> call)
    {
        try
        {
            return call().GetAwaiter().GetResult();
        }
        catch (RpcException ex)
        {
            throw GrpcFault.ToException(ex);
        }
    }
}
