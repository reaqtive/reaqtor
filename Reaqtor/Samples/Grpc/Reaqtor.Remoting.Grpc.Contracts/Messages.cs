// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using ProtoBuf;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Contracts
{
    //
    // Code-first message DTOs for the spike's gRPC contracts (plan §4). The CLR enums CommandVerb / CommandNoun /
    // NotificationKind / MetadataStorageType are reused verbatim from Reaqtor.Remoting.Core so client and server
    // agree on the wire values without a second source of truth; protobuf-net serializes them by their integer value.
    //

    /// <summary>An empty message (the code-first equivalent of <c>google.protobuf.Empty</c>).</summary>
    [ProtoContract]
    public sealed class Empty
    {
        /// <summary>A shared empty instance to avoid per-call allocations.</summary>
        public static readonly Empty Instance = new();
    }

    /// <summary>A command to execute against a reactive service (client → QC, or QC → QE — same contract).</summary>
    [ProtoContract]
    public sealed class ExecuteRequest
    {
        [ProtoMember(1)] public CommandVerb Verb { get; set; }
        [ProtoMember(2)] public CommandNoun Noun { get; set; }

        /// <summary>The command payload — Bonsai/DataModel JSON, opaque to the transport.</summary>
        [ProtoMember(3)] public string CommandText { get; set; }
    }

    /// <summary>The result of a command (a JSON string, or "OK").</summary>
    [ProtoContract]
    public sealed class ExecuteResponse
    {
        [ProtoMember(1)] public string Result { get; set; }
    }

    /// <summary>
    /// Platform wiring for a QC/QE host. The archived <c>[Serializable]</c> config graph carried live proxy
    /// connections; over gRPC it carries only address strings + scalar options (plan §3.3, §4.2).
    /// </summary>
    [ProtoContract]
    public sealed class PlatformConfiguration
    {
        [ProtoMember(1)] public string SchedulerType { get; set; }

        /// <summary>Scheduler thread count; 0 is treated as "unset" (null) by the host.</summary>
        [ProtoMember(2)] public int SchedulerThreadCount { get; set; }

        [ProtoMember(3)] public string TraceListenerType { get; set; }
        [ProtoMember(4)] public string TraceListenerFileName { get; set; }
        [ProtoMember(5)] public Dictionary<string, string> EngineOptions { get; set; }

        /// <summary>Only <see cref="MetadataStorageType.Remoting"/> is supported on net10.0 (plan §2.6/§4.2).</summary>
        [ProtoMember(6)] public MetadataStorageType StorageType { get; set; }

        /// <summary>Unused on net10.0 (the Azure metadata backend is not shipped — §2.6); reserved.</summary>
        [ProtoMember(7)] public string AzureConnectionString { get; set; }

        [ProtoMember(8)] public string MetadataAddress { get; set; }
        [ProtoMember(9)] public string MessagingAddress { get; set; }
        [ProtoMember(10)] public string StateStoreAddress { get; set; }
        [ProtoMember(11)] public string KeyValueStoreAddress { get; set; }
        [ProtoMember(12)] public List<string> QueryEvaluatorAddresses { get; set; }
    }

    /// <summary>Wire form of an exception crossing the command path or a firehose OnError (plan §6.1).</summary>
    [ProtoContract]
    public sealed class ErrorInfo
    {
        [ProtoMember(1)] public string TypeName { get; set; }
        [ProtoMember(2)] public string Message { get; set; }
        [ProtoMember(3)] public string StackTrace { get; set; }
        [ProtoMember(4)] public bool IsTransient { get; set; }
    }

    /// <summary>Wire form of an <c>INotification&lt;byte[]&gt;</c> on a messaging stream (plan §4.3).</summary>
    [ProtoContract]
    public sealed class Notification
    {
        [ProtoMember(1)] public NotificationKind Kind { get; set; }

        /// <summary>The payload for OnNext — SerializationHelpers bytes.</summary>
        [ProtoMember(2)] public byte[] Value { get; set; }

        /// <summary>The error for OnError.</summary>
        [ProtoMember(3)] public ErrorInfo Error { get; set; }
    }

    /// <summary>A publish onto a broker topic (inject leg).</summary>
    [ProtoContract]
    public sealed class PublishRequest
    {
        [ProtoMember(1)] public string Topic { get; set; }
        [ProtoMember(2)] public Notification Notification { get; set; }
    }

    /// <summary>A subscription to a broker topic (receive leg).</summary>
    [ProtoContract]
    public sealed class SubscribeRequest
    {
        [ProtoMember(1)] public string Topic { get; set; }
    }

    //
    // StateStore DTOs (plan §4.4). Names mirror the documented proto: Categories / Category / ItemKeys / ItemKey /
    // ItemValue / TryGetItemResponse / Bytes. The bool-returning engine methods (TryGetItemKeys / TryGetItem) are
    // modelled as {found, …} response messages because gRPC has no out-parameters.
    //

    /// <summary>Result of <c>GetCategories</c>.</summary>
    [ProtoContract]
    public sealed class Categories
    {
        [ProtoMember(1)] public List<string> Items { get; set; }
    }

    /// <summary>Argument carrying a single state-store category name.</summary>
    [ProtoContract]
    public sealed class Category
    {
        [ProtoMember(1)] public string Name { get; set; }
    }

    /// <summary>Result of <c>bool TryGetItemKeys(category, out keys)</c>.</summary>
    [ProtoContract]
    public sealed class ItemKeys
    {
        [ProtoMember(1)] public bool Found { get; set; }
        [ProtoMember(2)] public List<string> Keys { get; set; }
    }

    /// <summary>Addresses a single state-store item (category + key).</summary>
    [ProtoContract]
    public sealed class ItemKey
    {
        [ProtoMember(1)] public string Category { get; set; }
        [ProtoMember(2)] public string Key { get; set; }
    }

    /// <summary>A state-store item to add or update (category + key + value); also the batch-stream element.</summary>
    [ProtoContract]
    public sealed class ItemValue
    {
        [ProtoMember(1)] public string Category { get; set; }
        [ProtoMember(2)] public string Key { get; set; }
        [ProtoMember(3)] public byte[] Value { get; set; }
    }

    /// <summary>Result of <c>bool TryGetItem(category, key, out value)</c>.</summary>
    [ProtoContract]
    public sealed class TryGetItemResponse
    {
        [ProtoMember(1)] public bool Found { get; set; }
        [ProtoMember(2)] public byte[] Value { get; set; }
    }

    /// <summary>An opaque byte payload (the <c>Serialize</c>/<c>Deserialize</c> state-store blob).</summary>
    [ProtoContract]
    public sealed class Bytes
    {
        [ProtoMember(1)] public byte[] Data { get; set; }
    }

    /// <summary>Server-side round-trip counters for the §4.4.1 per-item-vs-batch measurement.</summary>
    [ProtoContract]
    public sealed class CallStats
    {
        /// <summary>Number of unary <c>AddOrUpdateItem</c> RPCs serviced (one per item).</summary>
        [ProtoMember(1)] public int UnaryAddOrUpdateCount { get; set; }

        /// <summary>Number of client-streamed batch commit calls serviced.</summary>
        [ProtoMember(2)] public int BatchCount { get; set; }

        /// <summary>Total items received across all batch commit calls.</summary>
        [ProtoMember(3)] public int BatchItemCount { get; set; }
    }

    //
    // KeyValueStore DTOs (plan §4.4). Names mirror the documented proto: Txn / TxnKey / TxnKeyValue /
    // ContainsResponse / TxnTable / KvpEntry. The transaction id is a server-side long created by CreateTransaction
    // and carried on every subsequent call. (Bytes is shared with the StateStore contract.)
    //

    /// <summary>A transaction handle (the server-side transaction id).</summary>
    [ProtoContract]
    public sealed class Txn
    {
        [ProtoMember(1)] public long Id { get; set; }
    }

    /// <summary>Addresses a key within a transaction + table.</summary>
    [ProtoContract]
    public sealed class TxnKey
    {
        [ProtoMember(1)] public long TransactionId { get; set; }
        [ProtoMember(2)] public string TableName { get; set; }
        [ProtoMember(3)] public string Key { get; set; }
    }

    /// <summary>A key/value to add or update within a transaction + table.</summary>
    [ProtoContract]
    public sealed class TxnKeyValue
    {
        [ProtoMember(1)] public long TransactionId { get; set; }
        [ProtoMember(2)] public string TableName { get; set; }
        [ProtoMember(3)] public string Key { get; set; }
        [ProtoMember(4)] public byte[] Value { get; set; }
    }

    /// <summary>Result of <c>bool Contains(txId, table, key)</c>.</summary>
    [ProtoContract]
    public sealed class ContainsResponse
    {
        [ProtoMember(1)] public bool Found { get; set; }
    }

    /// <summary>Addresses a table within a transaction (for enumeration).</summary>
    [ProtoContract]
    public sealed class TxnTable
    {
        [ProtoMember(1)] public long TransactionId { get; set; }
        [ProtoMember(2)] public string TableName { get; set; }
    }

    /// <summary>A single key/value pair streamed from <c>Enumerate</c>.</summary>
    [ProtoContract]
    public sealed class KvpEntry
    {
        [ProtoMember(1)] public string Key { get; set; }
        [ProtoMember(2)] public byte[] Value { get; set; }
    }

    //
    // Storage DTOs (plan §4.4 / §4.4.2). The CLR StorageEntity wraps a read-only property dictionary with no settable
    // members, so the wire form is a separate DTO (StorageEntityData) bridged by StorageEntityConverter. The property
    // Type is the local StorageEntityPropertyType ordinal (int) — no EdmType / Cosmos.Table (§2.6).
    //

    /// <summary>Wire form of a <c>StorageEntityProperty</c> (type ordinal + string data).</summary>
    [ProtoContract]
    public sealed class StoragePropertyData
    {
        [ProtoMember(1)] public int Type { get; set; }
        [ProtoMember(2)] public string Data { get; set; }
    }

    /// <summary>Wire form of a <c>StorageEntity</c> (a named property bag).</summary>
    [ProtoContract]
    public sealed class StorageEntityData
    {
        [ProtoMember(1)] public Dictionary<string, StoragePropertyData> Properties { get; set; }
    }

    /// <summary>Add a metadata entity into a collection under a key.</summary>
    [ProtoContract]
    public sealed class AddEntityRequest
    {
        [ProtoMember(1)] public string Collection { get; set; }
        [ProtoMember(2)] public string Key { get; set; }
        [ProtoMember(3)] public StorageEntityData Entity { get; set; }
    }

    /// <summary>Delete a metadata entity by collection + key.</summary>
    [ProtoContract]
    public sealed class DeleteEntityRequest
    {
        [ProtoMember(1)] public string Collection { get; set; }
        [ProtoMember(2)] public string Key { get; set; }
    }

    /// <summary>Look up a metadata entity by collection + key.</summary>
    [ProtoContract]
    public sealed class GetEntityRequest
    {
        [ProtoMember(1)] public string Collection { get; set; }
        [ProtoMember(2)] public string Key { get; set; }
    }

    /// <summary>Result of <c>bool TryGetEntity(collection, key, out entity)</c>.</summary>
    [ProtoContract]
    public sealed class TryGetEntityResponse
    {
        [ProtoMember(1)] public bool Found { get; set; }
        [ProtoMember(2)] public StorageEntityData Entity { get; set; }
    }

    /// <summary>Enumerate all entities in a collection.</summary>
    [ProtoContract]
    public sealed class GetEntitiesRequest
    {
        [ProtoMember(1)] public string Collection { get; set; }
    }
}
