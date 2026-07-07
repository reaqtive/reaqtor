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
// Client-side synchronous adapter (plan §4.4 / §4.4.2 / Milestone 4): implements the engine-facing flat
// IReactiveStorageConnection over the async gRPC Storage service. Only flat CRUD crosses the wire — the metadata
// IQueryable layer stays engine-side over this connection (§2.6). Faulting ops re-raise via GrpcFault (§6.1);
// TryGetEntity reads {found, entity} back into the out-parameter; GetEntities drains the server-streamed snapshot.
//
/// <summary>A gRPC-backed <see cref="IReactiveStorageConnection"/> over the Storage service.</summary>
public sealed class GrpcStorageConnection : ReactiveConnectionBase, IReactiveStorageConnection, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly bool _ownsChannel;
    private readonly IStorageService _service;

    /// <summary>Connects to a storage host address, owning the underlying channel.</summary>
    public GrpcStorageConnection(string address)
        : this(GrpcConnectionFactory.CreateChannel(address), ownsChannel: true)
    {
    }

    /// <summary>Connects over an existing channel.</summary>
    public GrpcStorageConnection(GrpcChannel channel, bool ownsChannel = false)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _ownsChannel = ownsChannel;
        _service = channel.CreateGrpcService<IStorageService>();
    }

    public void AddEntity(string collection, string key, StorageEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Invoke(() => _service.AddEntityAsync(new AddEntityRequest
        {
            Collection = collection,
            Key = key,
            Entity = StorageEntityConverter.ToWire(entity),
        }));
    }

    public void DeleteEntity(string collection, string key)
        => Invoke(() => _service.DeleteEntityAsync(new DeleteEntityRequest { Collection = collection, Key = key }));

    public bool TryGetEntity(string collection, string key, out StorageEntity entity)
    {
        var response = Invoke(() => _service.TryGetEntityAsync(new GetEntityRequest { Collection = collection, Key = key }));
        entity = response.Found ? StorageEntityConverter.FromWire(response.Entity) : null;
        return response.Found;
    }

    public IList<StorageEntity> GetEntities(string collection)
    {
        var entities = new List<StorageEntity>();
        var request = new GetEntitiesRequest { Collection = collection };

        try
        {
            DrainAsync().GetAwaiter().GetResult();
        }
        catch (RpcException ex)
        {
            throw GrpcFault.ToException(ex);
        }

        return entities;

        async Task DrainAsync()
        {
            await foreach (var entity in _service.GetEntitiesAsync(request).ConfigureAwait(false))
            {
                entities.Add(StorageEntityConverter.FromWire(entity));
            }
        }
    }

    /// <summary>Round-trips the readiness probe to the storage host (re-implements the base no-op).</summary>
    public new void Ping()
        => Invoke(() => _service.PingAsync(Empty.Instance));

    public void Dispose()
    {
        if (_ownsChannel)
        {
            _channel.Dispose();
        }
    }

    // Sync-over-async bridge (§5.2) with fault re-raise (§6.1): an RpcException carrying ErrorInfo trailers is
    // converted back into the original CLR exception before it reaches the engine.
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
