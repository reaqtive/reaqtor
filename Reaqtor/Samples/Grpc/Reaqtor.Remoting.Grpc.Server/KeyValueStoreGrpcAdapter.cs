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

namespace Reaqtor.Remoting.Grpc.Server
{
    //
    // Server side of the KeyValueStore adapter (plan §4.4 / Milestone 3): a gRPC facade over the engine's real
    // transactional key-value store (EngineHost.KeyValueStoreConnection — the SAME ITransactionalKeyValueStoreConnection
    // the engine reads as config.KeyValueStoreConnection). The transaction id is a server-side long carried on every
    // call. Faulting operations (the indexer getter and the mutators) translate their CLR exception into an
    // RpcException with §6.1 ErrorInfo trailers via GrpcFault, so the client adapter can RE-RAISE the original type
    // (§6.2 — the absent-key fault must reach the engine as the same type it would in-proc). Enumerate server-streams
    // the table snapshot.
    //
    /// <summary>Code-first implementation of <see cref="IKeyValueStoreService"/> over the in-host KV store.</summary>
    public sealed class KeyValueStoreGrpcAdapter : IKeyValueStoreService
    {
        private readonly EngineHost _engine;

        public KeyValueStoreGrpcAdapter(EngineHost engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        private ITransactionalKeyValueStoreConnection Connection => _engine.KeyValueStoreConnection;

        public Task<Txn> CreateTransactionAsync(Empty request, CallContext context = default)
            => Task.FromResult(new Txn { Id = Connection.CreateTransaction() });

        public Task<Bytes> GetAsync(TxnKey request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                return Task.FromResult(new Bytes { Data = Connection[request.TransactionId, request.TableName, request.Key] });
            }
            catch (RpcException)
            {
                throw;
            }
#pragma warning disable CA1031 // Faithful fault marshaling: any CLR exception is re-raised by type client-side (§6.2).
            catch (Exception ex)
            {
                throw GrpcFault.ToRpcException(ex);
            }
#pragma warning restore CA1031
        }

        public Task<Empty> AddAsync(TxnKeyValue request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Add(request.TransactionId, request.TableName, request.Key, request.Value));
        }

        public Task<ContainsResponse> ContainsAsync(TxnKey request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return Task.FromResult(new ContainsResponse { Found = Connection.Contains(request.TransactionId, request.TableName, request.Key) });
        }

        public Task<Empty> UpdateAsync(TxnKeyValue request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Update(request.TransactionId, request.TableName, request.Key, request.Value));
        }

        public Task<Empty> RemoveAsync(TxnKey request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Remove(request.TransactionId, request.TableName, request.Key));
        }

        public async IAsyncEnumerable<KvpEntry> EnumerateAsync(TxnTable request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            // await Task.Yield keeps the method a well-formed async iterator. The snapshot is materialized inside a
            // fault guard (a yield cannot live in a try/catch), so a CLR fault — e.g. an absent transaction id throws
            // KeyNotFoundException — is translated to an RpcException with §6.2 trailers exactly like the unary paths,
            // instead of leaking as a bare Unknown that the client can only rehydrate as a generic GrpcRemoteException.
            await Task.Yield();

            foreach (var entry in SnapshotTable(request))
            {
                yield return entry;
            }
        }

        private List<KvpEntry> SnapshotTable(TxnTable request)
        {
            try
            {
                var entries = new List<KvpEntry>();
                var enumerator = Connection.GetEnumerator(request.TransactionId, request.TableName);
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    entries.Add(new KvpEntry { Key = current.Key, Value = current.Value });
                }

                return entries;
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

        public Task<Empty> CommitAsync(Txn request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Commit(request.Id));
        }

        public Task<Empty> RollbackAsync(Txn request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Rollback(request.Id));
        }

        public Task<Empty> DisposeTransactionAsync(Txn request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            return FaultGuard(() => Connection.Dispose(request.Id));
        }

        public Task<Bytes> SerializeAsync(Empty request, CallContext context = default)
            => Task.FromResult(new Bytes { Data = Connection.SerializeStore() });

        public Task<Empty> DeserializeAsync(Bytes request, CallContext context = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            Connection.DeserializeStore(request.Data);
            return Task.FromResult(Empty.Instance);
        }

        public Task<Empty> ClearAsync(Empty request, CallContext context = default)
        {
            Connection.Clear();
            return Task.FromResult(Empty.Instance);
        }

        public Task<Empty> PingAsync(Empty request, CallContext context = default)
        {
            // ITransactionalKeyValueStoreConnection has no Ping (unlike the state-store connection); touching the
            // connection getter confirms the in-host engine is started, which is all a readiness probe needs.
            _ = Connection;
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
#pragma warning disable CA1031 // Faithful fault marshaling: any CLR exception is re-raised by type client-side (§6.2).
            catch (Exception ex)
            {
                throw GrpcFault.ToRpcException(ex);
            }
#pragma warning restore CA1031
        }
    }
}
