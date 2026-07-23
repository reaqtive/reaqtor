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
// Client-side synchronous adapter (plan §4.4 / Milestone 3): implements the engine-facing
// ITransactionalKeyValueStoreConnection over the async gRPC KeyValueStore service. Every method bridges
// sync-over-async (§5.2); the transaction id is the server-side long. Faulting operations (the indexer getter
// and the mutators) translate an RpcException back into the original CLR exception type via GrpcFault, so the
// engine sees the same exception it would in-proc (§6.2 — e.g. an absent-key lookup). GetEnumerator drains the
// server-streamed snapshot into a list, matching the in-memory connection's snapshot semantics.
//
/// <summary>A gRPC-backed <see cref="ITransactionalKeyValueStoreConnection"/> over the KeyValueStore service.</summary>
public sealed class GrpcKeyValueStoreConnection : ReactiveConnectionBase, ITransactionalKeyValueStoreConnection, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly bool _ownsChannel;
    private readonly IKeyValueStoreService _service;

    /// <summary>Connects to a key-value-store host address, owning the underlying channel.</summary>
    public GrpcKeyValueStoreConnection(string address)
        : this(GrpcConnectionFactory.CreateChannel(address), ownsChannel: true)
    {
    }

    /// <summary>Connects over an existing channel.</summary>
    public GrpcKeyValueStoreConnection(GrpcChannel channel, bool ownsChannel = false)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _ownsChannel = ownsChannel;
        _service = channel.CreateGrpcService<IKeyValueStoreService>();
    }

    public long CreateTransaction()
        => Invoke(() => _service.CreateTransactionAsync(Empty.Instance)).Id;

#pragma warning disable CA1819 // Properties should not return arrays. (Matches the engine-facing interface.)
    public byte[] this[long transactionId, string tableName, string key]
        => Invoke(() => _service.GetAsync(new TxnKey { TransactionId = transactionId, TableName = tableName, Key = key })).Data;
#pragma warning restore CA1819

    public void Add(long transactionId, string tableName, string key, byte[] value)
        => Invoke(() => _service.AddAsync(new TxnKeyValue { TransactionId = transactionId, TableName = tableName, Key = key, Value = value }));

    public bool Contains(long transactionId, string tableName, string key)
        => Invoke(() => _service.ContainsAsync(new TxnKey { TransactionId = transactionId, TableName = tableName, Key = key })).Found;

    public void Update(long transactionId, string tableName, string key, byte[] value)
        => Invoke(() => _service.UpdateAsync(new TxnKeyValue { TransactionId = transactionId, TableName = tableName, Key = key, Value = value }));

    public void Remove(long transactionId, string tableName, string key)
        => Invoke(() => _service.RemoveAsync(new TxnKey { TransactionId = transactionId, TableName = tableName, Key = key }));

    public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator(long transactionId, string tableName)
    {
        var list = new List<KeyValuePair<string, byte[]>>();
        var request = new TxnTable { TransactionId = transactionId, TableName = tableName };

        try
        {
            DrainAsync().GetAwaiter().GetResult();
        }
        catch (RpcException ex)
        {
            throw GrpcFault.ToException(ex);
        }

        return list.GetEnumerator();

        async Task DrainAsync()
        {
            await foreach (var entry in _service.EnumerateAsync(request).ConfigureAwait(false))
            {
                list.Add(new KeyValuePair<string, byte[]>(entry.Key, entry.Value));
            }
        }
    }

    public void Commit(long transactionId)
        => Invoke(() => _service.CommitAsync(new Txn { Id = transactionId }));

    public void Rollback(long transactionId)
        => Invoke(() => _service.RollbackAsync(new Txn { Id = transactionId }));

    public void Dispose(long transactionId)
        => Invoke(() => _service.DisposeTransactionAsync(new Txn { Id = transactionId }));

    public byte[] SerializeStore()
        => Invoke(() => _service.SerializeAsync(Empty.Instance)).Data;

    public void DeserializeStore(byte[] bytes)
        => Invoke(() => _service.DeserializeAsync(new Bytes { Data = bytes }));

    public void Clear()
        => Invoke(() => _service.ClearAsync(Empty.Instance));

    /// <summary>Round-trips the readiness probe to the key-value-store host (re-implements the base no-op).</summary>
    public new void Ping()
        => Invoke(() => _service.PingAsync(Empty.Instance));

    public void Dispose()
    {
        if (_ownsChannel)
        {
            _channel.Dispose();
        }
    }

    // Sync-over-async bridge (§5.2) with fault re-raise (§6.2): an RpcException carrying §6.1 ErrorInfo trailers
    // is converted back into the original CLR exception type before it reaches the engine.
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
