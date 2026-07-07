// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using ProtoBuf.Grpc;

namespace Reaqtor.Remoting.Grpc.Contracts
{
    //
    // Code-first gRPC service contracts (plan §4). protobuf-net.Grpc maps these WCF-style [ServiceContract]/
    // [OperationContract] interfaces onto HTTP/2 gRPC methods; the Name pins the wire service name so the proto
    // form is stable. The same IReactiveServiceConnectionService is hosted by both the QC (client → QC) and the
    // QE (QC → QE) — the command channel is symmetric (§4.1).
    //

    /// <summary>The command channel: execute a verb/noun/text command and get a result string (plan §4.1).</summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.ReactiveServiceConnection")]
    public interface IReactiveServiceConnectionService
    {
        [OperationContract]
        Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request, CallContext context = default);
    }

    /// <summary>Lifecycle/control surface hosted by both QC and QE (plan §4.2).</summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.ReactiveServiceControl")]
    public interface IReactiveServiceControl
    {
        /// <summary>Configures the service from addresses + scalar options. Rejects non-Remoting storage with InvalidArgument (§3.3).</summary>
        [OperationContract]
        Task<Empty> ConfigureAsync(PlatformConfiguration request, CallContext context = default);

        [OperationContract]
        Task<Empty> StartAsync(Empty request, CallContext context = default);

        /// <summary>Readiness probe (replaces the archived Tcp Ping retry loop).</summary>
        [OperationContract]
        Task<Empty> PingAsync(Empty request, CallContext context = default);
    }

    /// <summary>Query-evaluator-only control: checkpoint/recovery (plan §4.2). Scheduler is NOT remoted (§3.4).</summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.QueryEvaluatorControl")]
    public interface IQueryEvaluatorControl
    {
        [OperationContract]
        Task<Empty> CheckpointAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Empty> UnloadAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Empty> RecoverAsync(Empty request, CallContext context = default);
    }

    /// <summary>The stateful fan-out broker: unary Publish (inject) + server-streaming Subscribe (receive) (plan §4.3).</summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.Messaging")]
    public interface IMessagingService
    {
        [OperationContract]
        Task<Empty> PublishAsync(PublishRequest request, CallContext context = default);

        [OperationContract]
        IAsyncEnumerable<Notification> SubscribeAsync(SubscribeRequest request, CallContext context = default);
    }

    /// <summary>
    /// The checkpoint state store (<c>IReactiveStateStoreConnection</c>) over gRPC (plan §4.4). The engine's
    /// per-item sync interface maps onto unary RPCs (the <c>bool TryGet…</c> + <c>out</c> pairs become
    /// found+value response DTOs), plus the §4.4.1 client-streamed batch commit that collapses the checkpoint
    /// writer's per-item loop into one call. <see cref="GetCallStatsAsync"/> reports the round-trip counts the
    /// batch path is designed to reduce.
    /// </summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.StateStore")]
    public interface IStateStoreService
    {
        [OperationContract]
        Task<Categories> GetCategoriesAsync(Empty request, CallContext context = default);

        /// <summary><c>bool TryGetItemKeys(category, out keys)</c> → {found, keys}.</summary>
        [OperationContract]
        Task<ItemKeys> TryGetItemKeysAsync(Category request, CallContext context = default);

        /// <summary><c>bool TryGetItem(category, key, out value)</c> → {found, value}.</summary>
        [OperationContract]
        Task<TryGetItemResponse> TryGetItemAsync(ItemKey request, CallContext context = default);

        [OperationContract]
        Task<Empty> AddOrUpdateItemAsync(ItemValue request, CallContext context = default);

        [OperationContract]
        Task<Empty> RemoveItemAsync(ItemKey request, CallContext context = default);

        [OperationContract]
        Task<Empty> ClearAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Bytes> SerializeAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Empty> DeserializeAsync(Bytes request, CallContext context = default);

        [OperationContract]
        Task<Empty> PingAsync(Empty request, CallContext context = default);

        /// <summary>Checkpoint commit as a client-streamed batch — N staged items in one call (plan §4.4.1).</summary>
        [OperationContract]
        Task<Empty> AddOrUpdateItemsAsync(IAsyncEnumerable<ItemValue> request, CallContext context = default);

        /// <summary>Round-trip counters (unary item writes vs batched-stream calls/items) for §4.4.1 measurement.</summary>
        [OperationContract]
        Task<CallStats> GetCallStatsAsync(Empty request, CallContext context = default);
    }

    /// <summary>
    /// The transactional key-value store (<c>ITransactionalKeyValueStoreConnection</c>) over gRPC (plan §4.4). The
    /// transaction id (a server-side <c>long</c>) is created by <see cref="CreateTransactionAsync"/> and carried on
    /// every subsequent call. The indexer getter (<see cref="GetAsync"/>) and the mutators surface the engine's
    /// exception contract over the wire — absent/duplicate-key faults are re-raised client-side with their original
    /// CLR type via the §6.2 fault trailers; <see cref="EnumerateAsync"/> server-streams the table snapshot.
    /// </summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.KeyValueStore")]
    public interface IKeyValueStoreService
    {
        [OperationContract]
        Task<Txn> CreateTransactionAsync(Empty request, CallContext context = default);

        /// <summary><c>this[txId, table, key]</c> getter; an absent key faults (re-raised client-side, §6.2).</summary>
        [OperationContract]
        Task<Bytes> GetAsync(TxnKey request, CallContext context = default);

        [OperationContract]
        Task<Empty> AddAsync(TxnKeyValue request, CallContext context = default);

        [OperationContract]
        Task<ContainsResponse> ContainsAsync(TxnKey request, CallContext context = default);

        [OperationContract]
        Task<Empty> UpdateAsync(TxnKeyValue request, CallContext context = default);

        [OperationContract]
        Task<Empty> RemoveAsync(TxnKey request, CallContext context = default);

        /// <summary><c>GetEnumerator(txId, table)</c> snapshot as a server stream.</summary>
        [OperationContract]
        IAsyncEnumerable<KvpEntry> EnumerateAsync(TxnTable request, CallContext context = default);

        [OperationContract]
        Task<Empty> CommitAsync(Txn request, CallContext context = default);

        [OperationContract]
        Task<Empty> RollbackAsync(Txn request, CallContext context = default);

        [OperationContract]
        Task<Empty> DisposeTransactionAsync(Txn request, CallContext context = default);

        [OperationContract]
        Task<Bytes> SerializeAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Empty> DeserializeAsync(Bytes request, CallContext context = default);

        [OperationContract]
        Task<Empty> ClearAsync(Empty request, CallContext context = default);

        [OperationContract]
        Task<Empty> PingAsync(Empty request, CallContext context = default);
    }

    /// <summary>
    /// The flat metadata storage connection (<c>IReactiveStorageConnection</c>) over gRPC (plan §4.4 / §4.4.2). Only
    /// the flat CRUD surface crosses the wire — the metadata IQueryable/expression layer runs engine-side over this
    /// connection (Cosmos-free, §2.6). <see cref="TryGetEntityAsync"/> projects the engine's <c>bool TryGetEntity(out)</c>
    /// onto a {found, entity} response; <see cref="GetEntitiesAsync"/> server-streams a collection snapshot.
    /// </summary>
    [ServiceContract(Name = "reaqtor.remoting.v1.Storage")]
    public interface IStorageService
    {
        [OperationContract]
        Task<Empty> AddEntityAsync(AddEntityRequest request, CallContext context = default);

        [OperationContract]
        Task<Empty> DeleteEntityAsync(DeleteEntityRequest request, CallContext context = default);

        /// <summary><c>bool TryGetEntity(collection, key, out entity)</c> → {found, entity}.</summary>
        [OperationContract]
        Task<TryGetEntityResponse> TryGetEntityAsync(GetEntityRequest request, CallContext context = default);

        /// <summary><c>GetEntities(collection)</c> snapshot as a server stream.</summary>
        [OperationContract]
        IAsyncEnumerable<StorageEntityData> GetEntitiesAsync(GetEntitiesRequest request, CallContext context = default);

        [OperationContract]
        Task<Empty> PingAsync(Empty request, CallContext context = default);
    }
}
