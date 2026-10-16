// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using global::Grpc.Core;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server;

//
// Server side of the Storage adapter (plan §4.4 / §4.4.2 / Milestone 4): a gRPC facade over the engine's real
// flat metadata storage connection (EngineHost.StorageConnection). Only the flat CRUD surface is remoted; the
// metadata IQueryable layer runs engine-side over this connection (Cosmos-free, §2.6). Faulting ops (e.g. a
// duplicate AddEntity / absent DeleteEntity) translate to an RpcException with §6.1 ErrorInfo trailers via
// GrpcFault; TryGetEntity projects the bool+out onto {found, entity}; GetEntities server-streams the snapshot.
//
/// <summary>Code-first implementation of <see cref="IStorageService"/> over the in-host metadata store.</summary>
public sealed class StorageGrpcAdapter : IStorageService
{
    private readonly EngineHost _engine;

    public StorageGrpcAdapter(EngineHost engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
    }

    private IReactiveStorageConnection Connection => _engine.StorageConnection;

    public Task<Empty> AddEntityAsync(AddEntityRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return FaultGuard(() => Connection.AddEntity(request.Collection, request.Key, StorageEntityConverter.FromWire(request.Entity)));
    }

    public Task<Empty> DeleteEntityAsync(DeleteEntityRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return FaultGuard(() => Connection.DeleteEntity(request.Collection, request.Key));
    }

    public Task<TryGetEntityResponse> TryGetEntityAsync(GetEntityRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var found = Connection.TryGetEntity(request.Collection, request.Key, out var entity);
        return Task.FromResult(new TryGetEntityResponse
        {
            Found = found,
            Entity = found ? StorageEntityConverter.ToWire(entity) : null,
        });
    }

    public async IAsyncEnumerable<StorageEntityData> GetEntitiesAsync(GetEntitiesRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // The snapshot is materialized inside a fault guard (a yield cannot live in a try/catch) so a CLR fault is
        // translated to an RpcException with §6.2 trailers like the unary paths, not leaked as a bare Unknown.
        await Task.Yield();

        foreach (var entity in SnapshotCollection(request))
        {
            yield return entity;
        }
    }

    private List<StorageEntityData> SnapshotCollection(GetEntitiesRequest request)
    {
        try
        {
            var entities = new List<StorageEntityData>();
            foreach (var entity in Connection.GetEntities(request.Collection))
            {
                entities.Add(StorageEntityConverter.ToWire(entity));
            }

            return entities;
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

    public Task<Empty> PingAsync(Empty request, CallContext context = default)
    {
        Connection.Ping();
        return Task.FromResult(Empty.Instance);
    }

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
#pragma warning disable CA1031 // Faithful fault marshaling: the CLR exception is reconstructed client-side (§6.1).
        catch (Exception ex)
        {
            throw GrpcFault.ToRpcException(ex);
        }
#pragma warning restore CA1031
    }
}
