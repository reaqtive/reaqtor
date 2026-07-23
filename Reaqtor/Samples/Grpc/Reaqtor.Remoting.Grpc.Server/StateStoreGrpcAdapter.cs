// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using global::Grpc.Core;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server;

//
// Server side of the StateStore adapter (plan §4.4 / Milestone 2): a thin gRPC facade over the engine's real
// checkpoint state store (EngineHost.StateStoreConnection — the SAME IReactiveStateStoreConnection the engine
// reads as config.StateStoreConnection for checkpoint/recover, §4.4/§3.3). The engine's synchronous bool+out
// methods are projected onto {found, …} response DTOs; the §4.4.1 client-streamed AddOrUpdateItems collapses
// the checkpoint writer's per-item commit loop into a single call. Round-trip counters back the §4.4.1
// per-item-vs-batch measurement (read over the wire via GetCallStats).
//
/// <summary>Code-first implementation of <see cref="IStateStoreService"/> over the in-host state store.</summary>
public sealed class StateStoreGrpcAdapter : IStateStoreService
{
    private readonly EngineHost _engine;

    private int _unaryAddOrUpdateCount;
    private int _batchCount;
    private int _batchItemCount;

    public StateStoreGrpcAdapter(EngineHost engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
    }

    private IReactiveStateStoreConnection Connection => _engine.StateStoreConnection;

    public Task<Categories> GetCategoriesAsync(Empty request, CallContext context = default)
        => Task.FromResult(new Categories { Items = [.. Connection.GetCategories()] });

    public Task<ItemKeys> TryGetItemKeysAsync(Category request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var found = Connection.TryGetItemKeys(request.Name, out var keys);
        return Task.FromResult(new ItemKeys
        {
            Found = found,
            Keys = found ? [.. keys] : [],
        });
    }

    public Task<TryGetItemResponse> TryGetItemAsync(ItemKey request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var found = Connection.TryGetItem(request.Category, request.Key, out var value);
        return Task.FromResult(new TryGetItemResponse
        {
            Found = found,
            Value = found ? value : null,
        });
    }

    public Task<Empty> AddOrUpdateItemAsync(ItemValue request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return FaultGuard(() =>
        {
            Connection.AddOrUpdateItem(request.Category, request.Key, request.Value);
            Interlocked.Increment(ref _unaryAddOrUpdateCount);
        });
    }

    public Task<Empty> RemoveItemAsync(ItemKey request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return FaultGuard(() => Connection.RemoveItem(request.Category, request.Key));
    }

    public Task<Empty> ClearAsync(Empty request, CallContext context = default)
    {
        Connection.Clear();
        return Task.FromResult(Empty.Instance);
    }

    public Task<Bytes> SerializeAsync(Empty request, CallContext context = default)
        => Task.FromResult(new Bytes { Data = Connection.SerializeStateStore() });

    public Task<Empty> DeserializeAsync(Bytes request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        Connection.DeserializeStateStore(request.Data);
        return Task.FromResult(Empty.Instance);
    }

    public Task<Empty> PingAsync(Empty request, CallContext context = default)
    {
        Connection.Ping();
        return Task.FromResult(Empty.Instance);
    }

    public async Task<Empty> AddOrUpdateItemsAsync(IAsyncEnumerable<ItemValue> request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var count = 0;
        await foreach (var item in request.ConfigureAwait(false))
        {
            Connection.AddOrUpdateItem(item.Category, item.Key, item.Value);
            count++;
        }

        Interlocked.Increment(ref _batchCount);
        Interlocked.Add(ref _batchItemCount, count);
        return Empty.Instance;
    }

    public Task<CallStats> GetCallStatsAsync(Empty request, CallContext context = default)
        => Task.FromResult(new CallStats
        {
            UnaryAddOrUpdateCount = Volatile.Read(ref _unaryAddOrUpdateCount),
            BatchCount = Volatile.Read(ref _batchCount),
            BatchItemCount = Volatile.Read(ref _batchItemCount),
        });

    // Translate a thrown CLR exception into an RpcException with §6.2 ErrorInfo trailers, consistent with the
    // KeyValueStore and Storage adapters (so the client re-raises the original type).
    private static Task<Empty> FaultGuard(Action action)
    {
        try
        {
            action();
            return Task.FromResult(Empty.Instance);
        }
        catch (RpcException)
        {
            throw;
        }
#pragma warning disable CA1031 // Faithful fault marshaling: the CLR exception is reconstructed by type client-side (§6.2).
        catch (Exception ex)
        {
            throw GrpcFault.ToRpcException(ex);
        }
#pragma warning restore CA1031
    }
}
