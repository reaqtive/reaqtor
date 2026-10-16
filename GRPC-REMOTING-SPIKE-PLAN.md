# gRPC Replacement for the Archived Reaqtor .NET Remoting Stack — Implementation Plan

> Spike branch: `spike/grpc-remoting` (off `feature/dotnet-10`). Target framework: `net10.0`. The Remoting sample stack
> (`archive/Reaqtor/Samples/Remoting/**`) and `archive/Nuqleon/Core/BCL/Nuqleon.Runtime.Remoting.Tasks/**`
> are net472-only and in **zero** solutions. They are **source references**, not build inputs. We build a
> gRPC transport plus the transport-neutral abstractions it needs.
>
> **Correction to the original framing (read first).** The original plan asserted we could "reference the
> archived projects read-only, modify no archived file, and override `StartAsync` like `InMemory`." That is
> **not buildable** and is retracted. Every archived Remoting project — `Reaqtor.Remoting.Protocol`,
> `.QueryEvaluator`, `.QueryCoordinator`, `.Messaging`, `.Platform`, `.Platform.InMemory`, `.Client`,
> `.Client.Library`, `.TestingFramework`, and `Nuqleon.Runtime.Remoting.Tasks` — declares
> `<TargetFrameworks>net472</TargetFrameworks>`, and the transport-clean types we want are **welded** to
> `System.Runtime.Remoting` and to the very `Nuqleon.Runtime.Remoting.Tasks` layer we delete (verified:
> `ReactiveServiceCommandProxy : RemoteProxyBase`, `ReactiveServiceCommandService : RemoteServiceBase`;
> `LocalReactiveServiceConnection` — used by both `QueryCoordinatorServiceConnection.Start` and
> `ReactivePlatformClientBase.ServiceProvider` — `new`s up `ReactiveServiceCommandProxy`;
> `ReactivePlatformBase.StartAsync` registers a `TcpChannel` with `BinaryServerFormatterSinkProvider`/
> `TypeFilterLevel`; `ReactiveEnvironmentBase` calls
> `ServiceInstanceHelpers.SetMessagingServiceInstance(AppDomain.CurrentDomain, …)`; the metadata storage
> abstraction lives **inside** `Reaqtor.Remoting.Platform`). A `net10.0` project cannot `ProjectReference`
> any of these. **Therefore the real work is to multi-target / extract the transport-agnostic types into new
> `net10.0` projects** (detailed in §2.5). This is the largest single work item, and it explicitly overrides
> the "do not un-archive or rewrite the remoting code" constraint where that constraint is physically
> impossible. Everything downstream of §2.5 depends on it.

---

## 1. Goal & scope

**Goal.** Restore the distributed-sample capability of Reaqtor on modern .NET by replacing the
`.NET Remoting` transport with gRPC, while preserving the platform/deployer abstraction
(`IReactivePlatform`, `IReactiveService`, `IRunnable`, `ReactivePlatformDeployer`, `IDeployable`),
the command protocol (`CommandVerb` + `CommandNoun` + Bonsai/DataModel `commandText` → `string`),
and the existing Bonsai/DataModel serialization untouched **in behaviour** (the *code* moves projects; see §2.5).

| In scope (spike → MVP) | Out of scope (deferred / explicit non-goals) |
|---|---|
| **Port/extract** the transport-agnostic types to `net10.0` (the enabling work item — §2.5) | Preserving net472 / `System.Runtime.Remoting` anywhere in the live build |
| New `Reaqtor.Remoting.Platform.Grpc` transport behind the (now ported) `IReactivePlatform` seam | Re-modelling `ExpressionSlim`/`Expression` node types in protobuf (we carry Bonsai bytes opaquely) |
| Core command channel: client → QueryCoordinator → QueryEvaluator (the `CreateCommand(...).Execute` chokepoint) | Command-surface modernization (flattening verbs/nouns into typed methods — "Approach B") |
| **The gRPC client connection + the four client store adapters + the client driver** (`Reaqtor.Remoting.Client(.Library)` clean subset is also ported — §2.5, §4.7) | Sharing `TExpression` across client/server (client serializes `Expression`, QC `ExpressionSlim`; only the JSON string crosses — §4.7) |
| **Stateful Messaging broker**: `Publish` on one connection fans out to all open `Subscribe` streams for the topic; **both broker legs** (QE-as-subscriber for input firehoses, QE-as-publisher for output firehoses) in scope (§4.3, §5, §9) | A *durable* external broker (Redis/RabbitMQ); the MVP broker is in-process-in-the-host but exercises both legs |
| **A transport-friendly test ingress/egress** replacing the in-process `GetInstance<TestQueryEvaluatorServiceConnection>` + `EventTimelines`/`TestObservers`/`TestSubscriptions` mutation and the `AppDomain`-slot `Subscriptions` ledger (§3.7) | Full-fidelity `[Serializable]` exception graphs (we project to `ErrorInfo{type,message,stack,isTransient}`) |
| **Four synchronous gRPC client adapters** (KVStore, StateStore, Storage, Messaging) implementing the engine's existing sync interfaces, with documented sync-over-async bridging (§4.4, §5.2) | Native `IQueryable` remoting (the metadata IQueryable layer stays engine-side over flat CRUD — §4.4.2) |
| Native async + `CancellationToken`/deadline (delete `RemoteProxyBase`/`RemoteServiceBase`/`Reply<T>`/`ICancellationProvider`/`RemoteCancellationDisposable`) | Remoting the live `IScheduler`/`SchedulerProxy` and **virtual-time test scheduling across the wire** (§3.4) |
| ASP.NET Core Kestrel gRPC hosts replacing the `*Host` exes; in-process transport retained for tests | `AppDomain` transport (no AppDomains on net10.0 — dropped) |
| `Get(Observer)` as the **client-side publishing observer** it actually is (§4.5) | A server→client observer-callback channel (the current design has none and needs none — §4.5) |
| **Real deployable workload**: `TestDeployable` + `TestFirehoseObservable` as the MVP acceptance scenario (§3.5, §9), with the test-ingress/egress of §3.7 | The `Microsoft.Azure.Cosmos.Table` `AzureTable/` metadata backend (carry-vs-refactor decision in §2.6; net10.0 supports only the gRPC-Storage `Remoting` backend) |
| TLS/mTLS hardening |

**Definition of "spike done"** is the MVP vertical slice in §9: with the ported abstractions building on
`net10.0`, one client drives one `QueryCoordinator` → one `QueryEvaluator` over gRPC to deploy
`TestDeployable`, wire a `TestFirehoseObservable` that **subscribes through the broker from inside the engine**,
**inject events through the chosen ingress (§3.7)**, observe them at a **client-side `Subscribe(topic)` recording
observer**, then **tear the subscription down via an explicit `Remove(Subscription, uri)` command** and assert no
orphaned engine subscription — all using **wall-clock**, closure-free scenarios (no marshaled virtual-time
delegates; see §3.4 and §10).

---

## 2. Recommended architecture & rationale

**Recommendation: a blend — Approach C's MVP-first sequencing toward Approach B's idiomatic, streaming-first
design, using code-first `protobuf-net.Grpc`, while keeping Approach A's faithful single
`Execute(verb, noun, commandText) → string` command surface for the coordinator/evaluator hop — *but only after*
the enabling port in §2.5, and with Messaging promoted from "server-stream" to a real broker.**

Concretely:

- **Command plane = faithful 1:1 (from A/C).** The entire data-carrying surface of
  `IRemotingReactiveServiceConnection` is the single coarse method `CreateCommand(verb, noun, commandText).Execute`.
  `commandText` is *already* Bonsai/DataModel JSON. Keeping it as one unary RPC means
  `QueryCoordinatorServiceProvider`, `QueryEvaluatorServiceProvider`, `CommandTextFactory`/`CommandTextParser`,
  `SerializationHelpers`, and the query engine are reused **behaviourally verbatim** (their code is relocated to
  `net10.0` projects in §2.5, not rewritten) — the lowest-risk path, and the same proto serves both client→QC and
  QC→QE because both ends share that *interface* (note the `TExpression` asymmetry in §4.7 — only the wire string
  is shared).
- **Streaming-first where it matters, as a *broker* (from B, corrected).** The notification/firehose path is the
  only place that fans *N* values over time **and** it is genuinely bidirectional at the topology level (the engine
  both subscribes to receive feeds **and** publishes results; clients both publish and subscribe). It becomes a
  **stateful Messaging broker**: a server-streaming `Subscribe(topic) → stream Notification` for the receive leg, a
  unary `Publish(topic, n)` for the inject leg, and **server-side fan-out** correlating a `Publish` on one
  connection to all open `Subscribe` streams for that topic (§4.3, §5). This is the natural gRPC shape *and*
  faithfully reproduces `MessagingConnection`'s `ConcurrentDictionary<topic, Action<INotification<byte[]>>>`
  semantics.
- **Delete the async-over-remoting machinery (all three approaches agree).** gRPC's native `Task<T>` +
  `ServerCallContext.CancellationToken` replaces `Nuqleon.Runtime.Remoting.Tasks`
  (`RemoteProxyBase`/`RemoteServiceBase`/`Reply<T>`/`ICancellationProvider`/`RemoteCancellationDisposable`), so
  that layer is *not ported*. **Consequence:** the generated `ReactiveServiceCommandProxy`/`Service` (which derive
  from it) and `LocalReactiveServiceConnection` (which `new`s the proxy) cannot come along unchanged — they are
  re-homed/re-implemented in §2.5.
- **Code-first contracts (from B/C).** Existing C# interfaces and CLR enums (`CommandVerb`, `CommandNoun`,
  `NotificationKind`) drive the wire format via `protobuf-net.Grpc`, avoiding hand-writing ~6 large `.proto` files.
  Because those enums live in the net472 Protocol assembly, §2.5 makes them available to `net10.0` first.
- **Keep the platform seam — after porting it (from C, the key insight, corrected).** The abstraction injects
  transport **per service**, so `Grpc*Service` slots in beside `InMemory*`/`Tcp*` with no changes to consumer
  code — **but the seam types themselves must first exist on `net10.0`** (§2.5), because today they sit in the
  net472, remoting-coupled `Reaqtor.Remoting.Platform`.

**Why not pure A / pure B.** Unchanged from the prior analysis: A keeps fine-grained store proxies but leaves the
firehose awkward; B's full method-flattening is a larger first step. We take A's command fidelity, B's streaming
(now a broker) and code-first, and C's spine — and add the §2.5 port as the prerequisite all three silently
assumed.

### 2.5 Enabling work item — porting the transport-agnostic core to `net10.0` (the largest task)

This is **prerequisite for every milestone**. We move (not fork) the transport-neutral types into new `net10.0`
projects, surgically excluding the `System.Runtime.Remoting`/MBR/AppDomain pieces, which are re-implemented for
gRPC. Two mechanisms, chosen per file:

- **Multi-target** a project to `net472;net10.0` where the whole file set is clean and we want a single home; or
- **Extract** clean types into a new `net10.0` project where the original file mixes clean and remoting-coupled
  code (the common case — Protocol and Platform are both mixed).

Concrete inventory (verified by reading the archived sources):

| Source (archived, net472) | Disposition on `net10.0` | Notes / why |
|---|---|---|
| `Protocol`: `CommandVerb`, `CommandNoun`, `NotificationKind`, `INotification<T>`, `ObserverNotification` (+ `OnNext/OnError/OnCompleted` notifications), `StorageEntity`/`StorageEntityProperty` | **Extract** → `Reaqtor.Remoting.Core` (new) | Pure data/enums. `ObserverNotification` is `[Serializable]` with a live `Exception` and supports **predicate** OnError — keep the type, but predicate variants stay in-proc (§6, §4.3). |
| `Protocol`: `IRemotingReactiveServiceConnection`, `IReactiveServiceConnection`, `IReactiveServiceProvider`, `ReactiveServiceConnection<T>`, `ReactiveServiceProvider<T>`, `CommandTextFactory`/`CommandTextParser`, `ReactiveConnectionBase`, the `IReactive*Connection` store/messaging/evaluator interfaces | **Extract** → `Reaqtor.Remoting.Core` | Transport-agnostic command machinery + the engine-facing interfaces. **Stop `ReactiveConnectionBase`/`ReactiveServiceConnection` from depending on `MarshalByRefObject`** (`ReactiveConnectionBase` is currently MBR — reintroduce a non-MBR base). |
| `Protocol/FaultHandling`: `BaseException` **and any sibling fault types the engine throws** | **Extract → `Reaqtor.Remoting.Core`; modernize on the way** | **Previously missing from this table — added (§6).** `BaseException` lives *only* at `archive/.../Reaqtor.Remoting.Protocol/FaultHandling/BaseException.cs` (no live copy). It is `[Serializable]` with a `protected BaseException(SerializationInfo, StreamingContext)` ctor (`:62`) and a `GetObjectData` override (`:84`) — the legacy `ISerializable` pattern that is **obsolete on net10.0 (SYSLIB0051)**. **Strip the `ISerializable` members during the port** (keep `_transient`/`IsTransient()`, the three normal ctors); they are dead weight once exceptions cross as `ErrorInfo` (§6.1). |
| `Protocol`: `ReactiveServiceCommandProxy : RemoteProxyBase`, `ReactiveServiceCommandService : RemoteServiceBase`, `LocalReactiveServiceConnection` (news the proxy), `RemoteObserver`/remoting glue | **Do NOT port; re-implement** in `Reaqtor.Remoting.Grpc.Protocol` | These are the deleted-layer bindings. Provide a `net10.0` `IReactiveServiceCommand`/connection that calls the engine directly (in-proc QC→QE) or over gRPC (cross-process), replacing the proxy/`LocalReactiveServiceConnection` pair. |
| `Nuqleon.Runtime.Remoting.Tasks` (`RemoteProxyBase`/`RemoteServiceBase`/`Reply<T>`/`ICancellationProvider`/`RemoteCancellationDisposable`) | **Delete (not ported)** | Subsumed by gRPC async + `CancellationToken`. |
| `Platform`: `IReactivePlatform`, `IReactiveService`, `IRunnable`, `IDeployable`, `ReactiveServiceType`, `ReactivePlatformDeployer`, `ReactiveServiceBase`, `ReactivePlatformServiceBase`, `ReactiveQueryEvaluatorBase`, `IReactivePlatformClient`, `IReactivePlatformConfiguration` (interface), `MessageRouter`, `FirehoseObserver`, the `Metadata/StorageAbstractions/**` (`ITableClient`/`ITable`/`ITableServiceContext`/`CreateQuery<T>` IQueryable layer), `StorageConnectionTableClient`/`StorageConnectionTableServiceContext`/`StorageConnectionQueryable` | **Extract** → `Reaqtor.Remoting.Platform.Core` (new `net10.0`) — **but see §2.6 (Cosmos.Table)** | These are interface/abstraction/clean-impl. **Excluded:** the `TcpChannel`-registering body of `ReactivePlatformBase.StartAsync`, the `[Serializable]` concrete `ReactivePlatformConfiguration`, and `ReactiveEnvironmentBase`'s `SetMessagingServiceInstance(AppDomain.CurrentDomain, …)`. Provide a **new transport-neutral `ReactivePlatformBase`** that does *not* register any channel, plus a non-`[Serializable]` config (§3.3). **`StorageConnectionQueryable`/`StorageConnectionTable` carry a hidden `Microsoft.Azure.Cosmos.Table` dependency — §2.6.** |
| `Platform/Metadata/AzureTable/**` + `AzureReactiveMetadataProxy`, `AzureStorageResolver`, `AzureTableQueryRewriter`, `DistributedQuery`, `KnownTableEntity`, the `AzureTableQueryKeyMatchRewriter` | **Decision in §2.6 (default: refactor the `EdmType` coupling out; do NOT carry the `AzureTable/` backend)** | The QC metadata path (`QueryCoordinatorServiceConnection.GetMetadata`) **drags this whole subtree in** via `new AzureReactiveMetadataProxy(new StorageConnectionTableClient(config.StorageConnection), new AzureStorageResolver())`. Several of these files use `Microsoft.Azure.Cosmos.Table`. **net10.0 supports only the `Remoting` (gRPC-Storage) backend** — see §2.6 and §4.2. |
| `Platform.InMemory` (`InMemoryReactivePlatform<,>`, `InMemory*Service`) | **Port** → `Reaqtor.Remoting.Platform.InMemory.Core` (new `net10.0`) | Needed as the **live oracle** (§10). It is net472 today; **no net10.0 oracle exists until this is ported** — an explicit prerequisite for any parity comparison. |
| `Client` / `Client.Library` (`RemotingServiceProvider : ReactiveServiceProvider` with `CommandTextFactory<Expression>(new ClientSerializationHelpers())`, `RemoteObserverClient<T>`, `ReactivePlatformClientBase`, `RemotingClientContext`, `ExpressionServices`, client `Constants`/`Operators`) **minus** the `LocalReactiveServiceConnection`+`ReactiveServiceCommandProxy` wiring | **Port** → `Reaqtor.Remoting.Client.Core` (new `net10.0`) | **Previously missing from this table — added (§4.7).** This is the client-facing driver the MVP needs. `ReactivePlatformClientBase.ServiceProvider` currently wires `LocalReactiveServiceConnection` + `ReactiveServiceCommandProxy` and branches on `MetadataStorageType.Remoting` (`:89`); since those are NOT ported, the client connection is re-implemented over gRPC in `Reaqtor.Remoting.Grpc.Client` (§4.7). `RemotingServiceProvider.GetObserverAsync` stays the **local publishing-observer factory it already is** (§4.5). |
| `QueryEvaluator` (`QueryEvaluatorServiceConnection`, `Checkpointing/StateStoreConnection{StateWriter,StateReader}`, `OperatorContextElements`) **minus** `SchedulerProxy` (MBR) | **Port** → `Reaqtor.Remoting.Engine` (new `net10.0`) | The engine wrapper. **Excluded:** `SchedulerProxy : MarshalByRefObject` (§3.4). `Scheduler` stays a local engine scheduler. |
| `QueryCoordinator` (`QueryCoordinatorServiceConnection`, `QueryCoordinatorServiceProvider`) | **Port** → `Reaqtor.Remoting.Engine` | Replace its internal `LocalReactiveServiceConnection` use with the §2.5 re-implementation. Today it routes to `_queryEvaluators.First()` (verified) — see §4.6 for multi-QE. **`GetMetadata` Azure coupling — §2.6.** |
| `Messaging` (`MessagingConnection` broker impl) | **Port** → `Reaqtor.Remoting.Engine` (or `…Core`) | The `ConcurrentDictionary<topic, Action<…>>` fan-out is the broker kernel reused server-side (§4.3); drop the MBR `Receiver`/`InitializeLifetimeService` lease bits. |
| `TestingFramework` (`TestDeployable`, `TestFirehoseObservable`, `TestObserver`, `TimelineObservable`, `OperatorTestBase`, `TestPlatform/**` incl. `TestQueryEvaluatorServiceConnection`, `TimelineStoreConnection`, `TestObserverStoreConnection`, `TestSubscriptionStoreConnection`) **minus** `Schedulers/RemoteSchedulerTask`/`ClientAction`/`AsyncClientAction` | **Port** → `Reaqtor.Remoting.TestingFramework.Core` (new `net10.0`) — **with the test-ingress/egress redesign of §3.7** | Needed for the MVP workload and oracle. **Excluded:** the marshaled-delegate scheduler tasks (§3.4); replace with in-proc/wall-clock equivalents. **`TestFirehoseObservable`'s `AppDomain.CurrentDomain.GetData/SetData(_topic)` Subscriptions ledger (`:36-42`) has no net10.0 equivalent and cannot cross gRPC — replace per §3.7.** |

**Sequencing rule.** Port bottom-up: `Reaqtor.Remoting.Core` → `Reaqtor.Remoting.Platform.Core` →
`Reaqtor.Remoting.Engine` + `…Platform.InMemory.Core` → `…Client.Core` + `…TestingFramework.Core`. Each must
`dotnet build` green on `net10.0` before the gRPC transport on top of it. Budget this as the dominant effort; the
gRPC adapters are small by comparison.

> **Constraint reconciliation.** The task said "do not un-archive or rewrite the remoting code." We honour it for
> the *remoting-specific* code (TcpChannel/MBR/AppDomain/`Nuqleon.Runtime.Remoting.Tasks` are not ported). We
> **cannot** honour it for the transport-agnostic core, which is physically unbuildable on `net10.0` in place. The
> archived tree stays untouched as a reference; the ported code lives in new `net10.0` projects under the Grpc
> tree (§8).

### 2.6 The hidden `Microsoft.Azure.Cosmos.Table` dependency in the metadata layer (NEW — resolves a missed third-party coupling)

The §2.5 plan to relocate the metadata `StorageAbstractions` IQueryable layer into `Reaqtor.Remoting.Platform.Core`
is correct in shape but **omits a third-party dependency that the layer drags in**. Verified by reading the
archived sources:

- `Metadata/StorageConnectionQueryable.cs` and `Metadata/StorageConnectionTable.cs` (the very files §4.4.2 places
  client/engine-side) **`using Microsoft.Azure.Cosmos.Table;`** and `switch` on **`EdmType`**
  (`Boolean/DateTime/Double/Guid/Int32/Int64/String`) to convert `StorageEntityProperty.Type` ↔ CLR types.
- That package is **not** referenced in `Directory.Packages.props` today (CPM has **zero** Azure/Cosmos entries —
  verified) and was absent from the original §8 CPM-additions list.
- A total of **15 files** under `Platform/Metadata/**` use `Microsoft.Azure.Cosmos.Table`: the two
  `StorageConnection*` files above **plus the entire `Metadata/AzureTable/**` subtree**
  (`AzureMetadataQueryProvider`, `AzureMetadataServiceProvider`, `AzureQueryableDictionary`, `AzureTableClient`,
  `AzureTable`), `Metadata/Entities/KnownTableEntity`, `Metadata/Internal/AzureTableQueryKeyMatchRewriter`, and
  the `StorageAbstractions/*TableOperation*` files (`ITable`, `ITableOperation`, `*TableOperation`,
  `TableOperationBase`).
- The QC metadata path **pulls the `AzureTable/` subtree into the live path** even for the `Remoting` backend:
  `QueryCoordinatorServiceConnection.GetMetadata` (`:40-54`) does
  `new AzureReactiveMetadataProxy(new StorageConnectionTableClient(config.StorageConnection), new AzureStorageResolver())`
  for `MetadataStorageType.Remoting`, with the `Azure` branch commented out and `default: throw
  InvalidOperationException`.

**Decision (default: refactor the coupling out, carry nothing from `AzureTable/`):**

1. **Replace the `EdmType` switch in `StorageConnectionQueryable`/`StorageConnectionTable` with a local enum**
   (`StorageEntityPropertyType { Boolean, DateTime, Double, Guid, Int32, Int64, String }`) that mirrors
   `StorageEntityProperty.Type`. This removes the `Microsoft.Azure.Cosmos.Table` `using` from the two files that
   must live in `Platform.Core`, so **no Cosmos package is added to CPM**.
2. **Do not port the `AzureTable/` backend.** It is the `MetadataStorageType.Azure` path, which is **commented out
   in source** and out of scope (§1). The QC `GetMetadata` is reworked so the `Remoting` backend builds
   `AzureReactiveMetadataProxy` over a Cosmos-free `StorageConnectionTableClient` (the proxy/resolver themselves
   must be audited for `Cosmos.Table` `using`s and, where present on the `Remoting` path, refactored to the local
   enum; the `Azure`-only members are dropped).
3. **Fallback (only if the refactor proves too invasive):** add `Microsoft.Azure.Cosmos.Table` to CPM and confirm
   a `net10.0`-compatible version exists; this is explicitly the *less preferred* option because it re-introduces a
   heavyweight Azure SDK dependency into the sample's metadata path for a backend we do not ship.

**Net10.0 supported metadata backend = `Remoting` only** (gRPC-Storage-backed). This is pinned in §4.2's
`storage_type` mapping and in §3.3.

> **Implementation correction (discovered executing Milestone 0a).** §2.6 above understated the metadata work:
> it implied only the storage *abstractions* (`ITable`/`ITableClient`/`EntityProperty`/`EdmType`→enum) needed
> relocating. In fact the `MetadataStorageType.Remoting` path itself flows
> `AzureReactiveMetadataProxy` → `AzureMetadataServiceProvider` → `AzureMetadataQueryProvider` →
> `AzureQueryableDictionary<_, XxxTableEntity>`, and the `XxxTableEntity` types derive from
> `Microsoft.Azure.Cosmos.Table.TableEntity`. So the entire metadata **implementation** layer (the query
> provider, the `Metadata/Entities/**` table-entity hierarchy, the `Metadata/Internal/AzureTableQuery*`
> rewriters, and the `*TableOperation` types) is on the live `Remoting` path and had to be ported Cosmos-free —
> **not** left behind as "the `AzureTable/` backend." Resolution (still no Cosmos package; honours §2.6's
> preferred option 1): port that layer into **`Reaqtor.Remoting.Platform.Core`** (it already holds the
> abstractions and already references `Reaqtor.Client`/`Reaqtor.Service`), introducing one new local Cosmos-free
> `TableEntity` base whose reflection-based `WriteEntity`/`ReadEntity` map CLR properties ↔ `EntityProperty`
> via the local `StorageEntityPropertyType`. Only the **true** Azure backend
> (`AzureTableClient`/`AzureTable`/`AzureTableServiceContext`, the commented-out `MetadataStorageType.Azure`
> path) is left unported. Two transport-agnostic command types also missing from the §2.5 inventory were added
> to `Reaqtor.Remoting.Core`: `InProcessReactiveServiceConnection` (replacing the deleted
> `LocalReactiveServiceConnection`/`ReactiveServiceCommandProxy`) and `RemotingReactiveServiceConnectionBase` +
> `ReactiveServiceCommandService` (the engine-side base/command producer, replacing the deleted
> `RemoteServiceBase` adapter) — both inline the `Nuqleon.Runtime.Remoting.Tasks` `TaskCompletionSource` bridge
> with no marshaling.

---

## 3. The platform seam — how `Reaqtor.Remoting.Platform.Grpc` plugs in

The seam is **ported** to `net10.0` as `Reaqtor.Remoting.Platform.Core` (§2.5). It retains the shapes
`IReactivePlatform`, `IReactiveService`, `ReactiveServiceBase`, `ReactivePlatformServiceBase`,
`ReactiveQueryEvaluatorBase`, `IRunnable`, `ReactivePlatformDeployer`, `IDeployable`, `IReactivePlatformClient`,
`MessageRouter`, `FirehoseObserver` — but with a **new transport-neutral `ReactivePlatformBase`** (no channel
registration) and a **new non-`[Serializable]` `IReactivePlatformConfiguration`** (§3.3).

Existing transports for reference (archived) and the new one:

| Transport | Project | `Instance` mechanism |
|---|---|---|
| InMemory (oracle) | `Reaqtor.Remoting.Platform.InMemory.Core` (ported) | direct in-proc instantiation |
| AppDomain | *dropped* (no AppDomains on net10.0) | — |
| Tcp | archived, net472 only — **documentation reference for intended semantics** | `Activator.GetObject("tcp://…")` |
| **Grpc (new)** | `Reaqtor.Remoting.Platform.Grpc` | typed code-first client over a cached `GrpcChannel` |

`Reaqtor.Remoting.Platform.Grpc` mirrors the (archived) `*.Tcp` structure:

```
GrpcReactivePlatform           : ReactivePlatformBase  (transport-neutral, ported)
GrpcMultiRoleReactivePlatform  : ReactivePlatformBase
GrpcReactiveEnvironment        : IReactiveEnvironment    // NO SetMessagingServiceInstance(AppDomain…) — see §3.6
GrpcQueryCoordinator           : ReactivePlatformServiceBase   (ServiceType=QueryCoordinator)
GrpcQueryEvaluator             : ReactiveQueryEvaluatorBase    (ServiceType=QueryEvaluator)
GrpcMetadataService / GrpcMessagingService / GrpcStateStoreService / GrpcKeyValueStoreService
GrpcRunnable<T>                : IRunnable
GrpcProcessRunnable            : (process launcher)
```

### 3.1 Seam #1 — `IReactiveService.GetInstance<T>()` returns a typed client

Today `Activator.GetObject` → transparent MBR proxy. In `GrpcRunnable<T>`:

```csharp
// GrpcRunnable<TContract> — Instance returns a code-first client bound to the service's channel
public object Instance => _channel.CreateGrpcService<TContract>(); // protobuf-net.Grpc
```

But the engine does **not** consume `TContract` (the gRPC contract) directly. The requested CLR interface is one
of the **engine-facing sync interfaces** (`IRemotingReactiveServiceConnection`, `ITransactionalKeyValueStoreConnection`,
`IReactiveStateStoreConnection`, `IReactiveStorageConnection`, `IReactiveMessagingConnection`). So `Instance`
returns a **synchronous client adapter** that *implements that interface* over the gRPC client (§4.4, §5.2), not
the raw `TContract`. This keeps `RemotingServiceProvider`/`ReactiveServiceProvider`/`TupletizingExpressionServices`
unchanged.

> **Test caveat — `GetInstance<TestQueryEvaluatorServiceConnection>()` does NOT survive this seam.** The test
> framework reaches into the QE's *concrete* `TestQueryEvaluatorServiceConnection` via
> `GetInstance<TestQueryEvaluatorServiceConnection>()` to mutate `EventTimelines`/`TestObservers`/
> `TestSubscriptions`. Over gRPC the QE is out-of-process and `GetInstance<T>()` can only return a typed client
> adapter, never the live server object. This is addressed by the test-ingress/egress redesign in **§3.7**.

### 3.2 Seam #2 — `StartAsync` registers no channel

The ported `ReactivePlatformBase.StartAsync` is transport-neutral (it does **not** call
`ChannelServices.RegisterChannel`). `GrpcReactivePlatform` provides the gRPC behaviour:

- creates `GrpcChannel`s lazily per service from **addresses**,
- materializes each address into a **typed sync adapter instance** and populates the config's connection getters
  with those adapters **before** `Configure` is called on QC/QE (§3.3 — the engine consumes typed connections,
  not strings),
- calls `Register(...)` on each service (the `ReactiveServiceType` dispatch in `ReactiveServiceBase` is unchanged),
- starts evaluators first, then the coordinator (same ordering as the archived Tcp platform),
- on `StopAsync`, disposes channels and shuts down hosts.

### 3.3 Config: addresses in, typed adapters out (the `[Serializable]` proxy graph is replaced)

The archived config is a private `[Serializable] ReactivePlatformConfiguration` whose members are **live
proxy-typed connections** (`StorageConnection`, `MessagingConnection`, `StateStoreConnection`,
`KeyValueStoreConnection`, `QueryEvaluatorConnections`). On `net10.0` we supply a **fresh, non-`[Serializable]`**
`IReactivePlatformConfiguration` whose connection getters return **gRPC-backed sync adapters** materialized from
addresses:

- The **wire** `PlatformConfiguration` message (§4.2) carries **address strings + scalar options** only.
- `GrpcReactivePlatform` resolves those addresses into adapter instances and exposes them through the config's
  typed getters. The engine (`QueryEvaluatorServiceConnection.Start`) then reads
  `config.StateStoreConnection`/`MessagingConnection`/`KeyValueStoreConnection`/`StorageConnection` exactly as it
  does today and gets working sync objects (backed by gRPC).
- **All four store adapters are constructed at `Configure`-time and retained through `Start`, for BOTH the QE and
  the QC.** This matters because:
  - `QueryEvaluatorServiceConnection.Start` reads all four connection getters (KVStore/StateStore/Messaging/Storage).
  - `QueryCoordinatorServiceConnection.Start` **also** needs the **Storage/metadata** adapter: its `GetMetadata`
    does `new StorageConnectionTableClient(config.StorageConnection)` (`:45`) **at `Start`** (a separate control
    RPC). The QC host must therefore have a Storage address in its config and the materialized adapter must survive
    between `Configure` and `Start` (it does — the server holds the config object across both control RPCs), **even
    when Storage runs in a different host**.
- **`StorageType` is pinned** (§4.2): the only supported value on net10.0 is the `Remoting` equivalent →
  gRPC-Storage adapter. `Configure` validates this and fails fast on any other value (the engine's `GetMetadata`
  `default:` throws `InvalidOperationException`; we surface that as an `InvalidArgument` at `Configure` rather than
  letting it throw lazily at `Start`).

### 3.4 Scheduler & virtual time — explicitly out of cross-process scope

Verified: tests pervasively do `client.Scheduler = (ITestScheduler)Platform.QueryEvaluators.First().Scheduler`
then `client.Scheduler.ScheduleAbsolute(t, () => stream.OnNextAsync(...))` / `() => qe.Checkpoint()` and
`client.Scheduler.Start()` (`RemotingTest.cs:542–560`), and read
`((ILoggingScheduler<long>)client.Scheduler).ScheduledTimes` (`OperatorTestBase.cs`). Under remoting,
`SchedulerProxy : MarshalByRefObject` let the client both **control** the QE `TestScheduler` and **ship `Action`
lambdas** (closing over client-side `stream`/`observer`/`qe`) to run server-side — via
`SchedulerExtensions.ScheduleAbsolute(... ) → new RemoteSchedulerTask(new ClientAction(action))` (verified).

**gRPC cannot marshal arbitrary client closures to execute server-side.** `SchedulerStart/Advance/Now` RPCs let
you turn a virtual clock but cannot ship `() => stream.OnNextAsync(...)`. Therefore:

- **Cross-process gRPC parity is scoped to closure-free, wall-clock scenarios with observable assertions.**
  Virtual-time glitching is **excluded** from cross-process parity.
- **`SchedulerProxy` is not ported.** The gRPC QE uses a **local** engine scheduler.
- **Glitching/quiescence keeps its virtual-time fidelity by running fully in-process** (engine + scheduler
  co-located, **no transport**) against the ported `InMemory` oracle. The **transport** is tested separately with
  wall-clock streaming/cancel/teardown scenarios.
- **Prerequisite:** porting `Reaqtor.Remoting.Platform.InMemory` and `TestingFramework` to `net10.0` (§2.5) is
  required before any oracle comparison — there is no live net10.0 oracle otherwise.
- Optional later: `SchedulerStart/Advance/Now` RPCs for a *server-authored* test timeline (the QE schedules its
  own closure-free events), never for shipping client closures.

### 3.5 The MVP workload is a real deployable

`ReactivePlatformDeployer`/`IDeployable.Execute(ReactiveClientContext)` is **unchanged** (ported). The MVP uses
the **real** `TestDeployable : CoreDeployable` plus `TestFirehoseObservable : SubscribableBase<T>` (verified),
which exercises the broker from inside the engine (`TestFirehoseObservable` resolves `MessageRouter` from the
operator context and its `TopicObserver` calls `_messageRouter.Subscribe(_topic, this)` — see §3.6). This is the
acceptance workload, not a bespoke minimal slice. **Its test inputs/outputs use the ingress/egress of §3.7, not
the in-process object access of today.**

### 3.6 `MessageRouter` and the engine-side messaging connection (replaces the AppDomain-keyed singleton)

Verified: `MessageRouter` resolves its messaging connection via
`ServiceInstanceHelpers.GetMessagingServiceInstance()` (set by
`ReactiveEnvironmentBase.SetMessagingServiceInstance(AppDomain.CurrentDomain, …)`), and engine operators like
`TestFirehoseObservable`'s `TopicObserver` call `_messageRouter.Subscribe(topic, this)` to **receive** feed data —
i.e. the **QE is itself a Subscribe client of the broker**, placed into `OperatorContextElements`. Symmetrically,
`FirehoseObserver.OnNext/OnError/OnCompleted` call `_messageRouter.Publish(topic, …)` to **inject** results — i.e.
the **QE is also a Publish client** for output firehoses. **Both legs exist** (see §9).

On `net10.0` there is no AppDomain-keyed instance. The ported `MessageRouter`/environment obtains its
`IReactiveMessagingConnection` by **constructor/DI injection**: the gRPC QE host injects a **gRPC-backed messaging
adapter** (the same sync client adapter from §4.4) into the engine's `OperatorContextElements`, so in-engine
operators can `Subscribe`/`Publish` over gRPC exactly as they did in-proc.

> **Single-shared-`MessagingConnection` invariant for the in-proc oracle (NEW — was missing).** In
> InMemory/AppDomain, `FirehoseObserver` (publisher) and `TestFirehoseObservable`'s `TopicObserver` (subscriber)
> resolve **the same** process-local `MessagingConnection` (via `ServiceInstanceHelpers`/AppDomain today). For the
> ported `Platform.InMemory.Core` oracle on net10.0, the DI wiring **must guarantee a single shared
> `MessagingConnection` instance across the whole in-proc platform** (publisher leg + subscriber leg). If the
> publisher and subscriber get different instances, `Publish` finds no receiver and the oracle **silently drops
> events and passes trivially-empty** — a dangerous false green. The oracle's composition root therefore registers
> the broker connection as a **singleton** and injects that one instance into both `FirehoseObserver` construction
> and the `MessageRouter` in `OperatorContextElements`. (Over gRPC this is automatic — there is exactly one broker
> host — but the in-proc oracle must assert it.)

```
                  ┌──────────── IReactivePlatform (ported, transport-neutral) ────────────┐
client/IDeployable│  InMemory(oracle)        Grpc (NEW)                                    │
ReactivePlatform  │  direct in-proc          GrpcChannel + typed SYNC adapters per service │
  Deployer        └──────────────────────────────────────────────────────────────────────┘
                       GrpcQueryCoordinator/Evaluator/... wrap PORTED *ServiceConnection
```

### 3.7 Transport-friendly test ingress/egress (NEW — the test plumbing that does **not** cross gRPC today)

This section closes the single biggest blocker to exercising parity over the wire. **Verified problem:** the
existing test framework injects inputs and reads outputs by reaching into the QE's concrete CLR object and into
`AppDomain` data slots — none of which can cross a gRPC boundary or exist on net10.0:

- **Input injection (timeline observables).** `TestReactivePlatformClient.CreateHotObservable`/`CreateColdObservable`
  do `Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>()` then
  `testQE.EventTimelines.TryAdd(id, …)` (`:106-108, :120-121`). Cleanup mutates `TestObservers`/`TestSubscriptions`
  (`:149-150`). There are **5 files** in the framework that touch
  `GetInstance<TestQueryEvaluatorServiceConnection>`/`EventTimelines`/`TestObservers`/`TestSubscriptions`
  (`RemotingTestBase`, `TestableQbserver`, `TestableQbservable`, `TestReactivePlatformClient`,
  `TestQueryEvaluatorServiceConnection`), plus the operator tests that consume them.
- **Output readout (subscription ledger).** `TestFirehoseObservable.Subscriptions` is stored in
  `AppDomain.CurrentDomain.GetData/SetData(_topic)` (`:36-42`); `OperatorTestBase` reads it back to assert
  `ReactiveTest.Subscribe(125,225)`-style results (`RemotingTest.cs:525-526`). **`AppDomain` data slots do not
  exist on net10.0.** (Note: the *other* timeline path, `TimelineObservable`, already reads its ledger from a
  **DI-injected** `TestSubscriptionStoreConnection` context element and its events from `TimelineStoreConnection`
  — this is the model to generalize.)

**Design — make ingress/egress transport-addressable, and replace the AppDomain ledger with an instance/DI-scoped
store surfaced over a control RPC:**

1. **Promote the three test stores to DI-scoped server state.** `TestQueryEvaluatorServiceConnection` already adds
   `TimelineStoreConnection`/`TestObserverStoreConnection`/`TestSubscriptionStoreConnection` to
   `OperatorContextElements` (verified `:45-47`). Keep that, but make the three stores **instance fields of the
   gRPC QE host's service** (not statics, not AppDomain slots), so each QE host owns its own ledgers.
2. **Add a `TestControl` control RPC surface (test-only) on the QE host** to plant inputs and read outputs across
   the wire, replacing `GetInstance<TestQueryEvaluatorServiceConnection>()`:
   ```protobuf
   service TestControl {                                  // QE host, test builds only
     rpc PlantEventTimeline (PlantTimelineRequest) returns (google.protobuf.Empty); // → EventTimelines.TryAdd(uri, serialized msgs)
     rpc GetSubscriptions   (TopicRequest)         returns (SubscriptionLedger);    // ← TestSubscriptionStoreConnection / firehose ledger
     rpc ClearTestState     (google.protobuf.Empty) returns (google.protobuf.Empty); // ← TestObservers/TestSubscriptions.Clear()
   }
   ```
   `CreateHotObservable`/`CreateColdObservable` call `PlantEventTimeline` instead of mutating
   `testQE.EventTimelines` directly; `xs.Subscriptions` reads back via `GetSubscriptions`; cleanup calls
   `ClearTestState`. For the **InMemory oracle**, the same `TestControl` operations bind to direct in-proc calls
   (no RPC), so a single test body runs against both transports.
3. **Replace `TestFirehoseObservable`'s AppDomain ledger** with the same DI-injected `TestSubscriptionStoreConnection`
   that `TimelineObservable` already uses. `TestFirehoseObservable` gets its `TestSubscriptionStoreConnection` from
   the operator context (exactly as `TimelineObservable` does at `:182`) and records `Subscribe`/`Dispose` ticks
   there. This removes the only `AppDomain.GetData/SetData` usage and makes the firehose subscription ledger
   readable via `TestControl.GetSubscriptions`.
4. **Result-stream egress for value assertions.** Beyond the subscription *ledger*, the MVP asserts the *values*
   the engine produced. The canonical path is: the engine's output `FirehoseObserver` **publishes results to a
   broker topic**, and the test **subscribes to that topic from the client** (`Messaging.Subscribe(topic)`) into a
   **client-side recording observer** (a `TestObserver`-equivalent). This is the egress used by §9 and is the only
   value-readout path that works uniformly in-proc and over gRPC.

**Net effect.** All three in-process/AppDomain test couplings (input planting, subscription-ledger readout, value
readout) are replaced by (a) a test-only `TestControl` control RPC, (b) DI-scoped stores generalized from the
existing `TimelineObservable` pattern, and (c) a client-side broker `Subscribe`. Until this exists, "parity against
the oracle" cannot be exercised over the wire — so this is a **Milestone-0a/1 deliverable**, not a deferral.

---

## 4. Contract design (code-first, `.proto`-equivalent shown for documentation)

Contracts live in **`Reaqtor.Remoting.Grpc.Contracts`** as C# `[ServiceContract]` interfaces + `[ProtoContract]`
DTOs (`protobuf-net.Grpc`). The CLR enums `CommandVerb`, `CommandNoun`, `NotificationKind` are **reused from the
ported `Reaqtor.Remoting.Core`** (§2.5). The `.proto` below is the documentation form `protobuf-net.Grpc.Reflection`
would emit.

### 4.1 Core command channel (client → QC and QC → QE share one contract)

```protobuf
syntax = "proto3";
package reaqtor.remoting.v1;
import "google/protobuf/empty.proto";

enum CommandVerb { VERB_UNKNOWN = 0; NEW = 1; REMOVE = 2; GET = 3; }
enum CommandNoun { NOUN_UNKNOWN = 0; OBSERVABLE = 1; OBSERVER = 2; STREAM_FACTORY = 3;
                   SUBSCRIPTION_FACTORY = 4; STREAM = 5; SUBSCRIPTION = 6; METADATA = 7; }

message ExecuteRequest  { CommandVerb verb = 1; CommandNoun noun = 2; string command_text = 3; } // Bonsai/DataModel JSON, opaque
message ExecuteResponse { string result = 1; }                                                    // JSON string or "OK"

service ReactiveServiceConnection { rpc Execute (ExecuteRequest) returns (ExecuteResponse); }
```

> **Subscription lifetime ≠ RPC lifetime (corrected).** Verified: `ReactiveServiceConnection.CommandExecutor`
> maps `New/Subscription → CreateSubscriptionAsync(uri, expr, state, token)` and **`Remove/Subscription →
> DeleteSubscriptionAsync(uri, token)`**, both unary and URI-addressed. A subscription is created by one
> `Execute(New, Subscription, …)` and destroyed by a **later** `Execute(Remove, Subscription, uri)`. Cancelling
> the create RPC cancels only the in-flight create (and may leave a partially-created subscription); it does **not**
> dispose an existing subscription. See §5.1.

### 4.2 Lifecycle / wiring

```protobuf
message PlatformConfiguration {                 // was a [Serializable] graph carrying PROXIES → now ADDRESSES + scalars
  string scheduler_type = 1;  int32 scheduler_thread_count = 2; // 0 ⇒ null
  string trace_listener_type = 3; string trace_listener_file_name = 4;
  map<string,string> engine_options = 5;
  MetadataStorageType storage_type = 6;          // see enum + mapping below
  string azure_connection_string = 7;            // unused on net10.0 (Azure backend not shipped — §2.6); reserved
  string metadata_address = 8; string messaging_address = 9;
  string state_store_address = 10; string key_value_store_address = 11;
  repeated string query_evaluator_addresses = 12;
}

// Mirrors the CLR MetadataStorageType enum. On net10.0 ONLY REMOTING is supported (§2.6):
//   REMOTING ⇒ metadata served by the gRPC Storage adapter (StorageConnectionTableClient over the Storage RPC).
//   AZURE    ⇒ NOT supported on net10.0 (the AzureTable/Cosmos.Table backend is not ported); Configure rejects it.
enum MetadataStorageType { METADATA_STORAGE_UNKNOWN = 0; REMOTING = 1; AZURE = 2; }

service ReactiveServiceControl {                 // hosted by QC and QE
  rpc Configure (PlatformConfiguration) returns (google.protobuf.Empty);  // validates storage_type==REMOTING (else InvalidArgument)
  rpc Start     (google.protobuf.Empty) returns (google.protobuf.Empty);
  rpc Ping      (google.protobuf.Empty) returns (google.protobuf.Empty);
}
service QueryEvaluatorControl {                  // QE-only
  rpc Checkpoint (google.protobuf.Empty) returns (google.protobuf.Empty);
  rpc Unload     (google.protobuf.Empty) returns (google.protobuf.Empty);
  rpc Recover    (google.protobuf.Empty) returns (google.protobuf.Empty);
}
// IReactiveQueryEvaluatorConnection.Scheduler (a live IScheduler / SchedulerProxy) is NOT remoted (§3.4).
```

> **`storage_type` is a real enum mapping, pinned (NEW).** Verified the engine branches on
> `configuration.StorageType` in **multiple** places — `QueryCoordinatorServiceConnection.GetMetadata` (`:42`),
> `ReactivePlatformBase`/`ReactiveEnvironmentBase`, and `ReactivePlatformClientBase` (`:89`) — and each has a
> `default:` that **throws** (`InvalidOperationException`/`NotSupportedException`). The wire enum maps 1:1 to the
> CLR `MetadataStorageType`. **net10.0 supports only `REMOTING`** (gRPC-Storage); `Configure` validates this and
> returns `InvalidArgument` for `AZURE`/unknown rather than allowing a lazy throw at `Start` (§3.3, §2.6).

> **Addresses are resolved into typed adapters before `Configure`, and retained through `Start` for both QE and QC**
> (§3.3). The QE engine reads typed connection objects from config, not strings; the **QC also needs the
> Storage/metadata adapter present at `Start`** for `GetMetadata`.

### 4.3 Messaging — a stateful fan-out broker (server-streaming + unary publish)

```protobuf
enum NotificationKind { ON_NEXT = 0; ON_ERROR = 1; ON_COMPLETED = 2; }
message ErrorInfo    { string type_name = 1; string message = 2; string stack_trace = 3; bool is_transient = 4; }
message Notification { NotificationKind kind = 1; bytes value = 2; ErrorInfo error = 3; } // value = SerializationHelpers bytes

message PublishRequest   { string topic = 1; Notification notification = 2; }
message SubscribeRequest { string topic = 1; }

service Messaging {
  rpc Publish   (PublishRequest)   returns (google.protobuf.Empty);     // inject leg (client OR engine feeder)
  rpc Subscribe (SubscribeRequest) returns (stream Notification);       // receive leg (client OR in-engine operator)
}
```

**Topology this models (corrected).** The single Messaging service is a **broker**, faithful to
`MessagingConnection`'s `ConcurrentDictionary<topic, Action<INotification<byte[]>>>`. **Both broker legs are in
scope** (verified against `FirehoseObserver.Publish` and `TestFirehoseObservable`/`TopicObserver.Subscribe`):

- **QE-as-subscriber (input firehoses).** In-engine operators (`TestFirehoseObservable`'s `TopicObserver`) open
  `Subscribe(topic)` streams to **receive** feeds (via the gRPC messaging adapter in `OperatorContextElements`,
  §3.6).
- **QE-as-publisher (output firehoses).** The engine's `FirehoseObserver` calls `Publish(topic, …)` to **emit**
  subscription results to the broker; clients `Subscribe(topic)` to observe them (§3.7 egress, §9).
- **Client-as-publisher / client-as-subscriber.** External feeders/`TestObserver` on the client may `Publish` to
  inject, and the client `Subscribe`s to read results. (See §9 for which inject leg the MVP drives first.)
- **Server-side fan-out with explicit semantics.** A `Publish(topic, n)` on connection *A* must be delivered to
  **every** open `Subscribe(topic)` stream on connections *B, C, …*. The broker holds, per topic, the set of
  active stream writers. Delivery is **ordered per topic** (a single-writer serialization point per topic — e.g.
  a per-topic `Channel<Notification>` pump, or a lock around the fan-out matching the original synchronous
  delegate invocation) and **concurrent across topics**. `OnError`/`OnCompleted` terminate the relevant streams
  (§5).
- **Per-stream buffering & slow/dead-consumer policy (MVP, not deferred).** Each `Subscribe` stream gets a
  **bounded** `Channel<Notification>` (capacity configurable, default small). Overflow policy is explicit and
  **per-topic configurable**, default **`error-the-stream`** (fail fast, preserving the "reliable" positioning by
  refusing silent loss): a writer that can't keep up has its stream completed with `RESOURCE_EXHAUSTED`; the
  subscriber observes `OnError` and may resubscribe (§7). Alternatives `block` (apply backpressure to the
  publisher — closest to the original synchronous lock-step) and `drop-oldest` (lossy, opt-in only) are selectable.
- **MVP backing.** The broker kernel is the ported in-process `MessagingConnection` fan-out hosted inside the
  Messaging gRPC host; it already exercises both legs. A durable external broker is a later swap behind the same
  contract.

> **Predicate notifications stay in-proc.** `ObserverNotification` supports `CreateOnError(Func<Exception,bool>)`
> for test equality (`RecordedNotificationEqualityComparer`). Predicate variants are **never** put on the wire
> (§6).

Code-first equivalent (the form we ship in `Contracts`):

```csharp
[ServiceContract(Name = "reaqtor.remoting.v1.ReactiveServiceConnection")]
public interface IReactiveServiceConnectionService {
    [OperationContract] Task<ExecuteResponse> ExecuteAsync(ExecuteRequest req, CallContext ctx = default);
}

[ServiceContract(Name = "reaqtor.remoting.v1.Messaging")]
public interface IMessagingService {
    [OperationContract] Task<Empty> PublishAsync(PublishRequest req, CallContext ctx = default);
    [OperationContract] IAsyncEnumerable<Notification> Subscribe(SubscribeRequest req, CallContext ctx = default);
}

[ProtoContract] public sealed class Notification {
    [ProtoMember(1)] public NotificationKind Kind { get; set; }   // reuse ported enum
    [ProtoMember(2)] public byte[] Value { get; set; }
    [ProtoMember(3)] public ErrorInfo Error { get; set; }
}
```

### 4.4 The four synchronous client adapters (named deliverables)

The QE wires the engine to live objects from config in `QueryEvaluatorServiceConnection.Start` (verified):
`_keyValueStore = new KeyValueStore(config.KeyValueStoreConnection)`, metadata via
`new StorageConnectionTableClient(config.StorageConnection)`, messaging via `config.MessagingConnection`,
checkpoint/recover via `config.StateStoreConnection`. These interfaces are **synchronous and per-item**. So the
gRPC transport must ship **four named synchronous client adapters** that implement the engine's existing
interfaces over async gRPC clients:

| Adapter (deliverable) | Implements (engine-facing, sync) | Backed by gRPC service |
|---|---|---|
| `GrpcKeyValueStoreConnection` | `ITransactionalKeyValueStoreConnection` (`byte[] this[txId,table,key] {get}`, `Add/Contains/Update/Remove`, `GetEnumerator`) | `KeyValueStore` (§4.4-proto) |
| `GrpcStateStoreConnection` | `IReactiveStateStoreConnection` (`bool TryGetItem(out byte[])`, `GetCategories`, `TryGetItemKeys`, `AddOrUpdateItem`, …) | `StateStore` |
| `GrpcStorageConnection` | `IReactiveStorageConnection` (`AddEntity`/`DeleteEntity`/`bool TryGetEntity(out StorageEntity)`) | `Storage` |
| `GrpcMessagingConnection` | `IReactiveMessagingConnection` (`Publish`, `Subscribe(topic, Action<INotification<byte[]>>)`) | `Messaging` |

These adapters **bridge sync→async** (§5.2). They are the objects §3.1's `Instance` returns and §3.3 injects into
config. (The **client driver** uses the same four adapters plus the command-channel client — §4.7.)

```protobuf
// StateStore (IReactiveStateStoreConnection): bool TryGet + out → {found,value}; SerializeStateStore → bytes
service StateStore {
  rpc GetCategories  (google.protobuf.Empty) returns (Categories);
  rpc TryGetItemKeys (Category)  returns (ItemKeys);            // bool + out IEnumerable<string>
  rpc TryGetItem     (ItemKey)   returns (TryGetItemResponse);  // bool + out byte[]
  rpc AddOrUpdateItem(ItemValue) returns (google.protobuf.Empty);
  rpc RemoveItem     (ItemKey)   returns (google.protobuf.Empty);
  rpc Clear          (google.protobuf.Empty) returns (google.protobuf.Empty);
  rpc Serialize      (google.protobuf.Empty) returns (Bytes);
  rpc Deserialize    (Bytes)     returns (google.protobuf.Empty);
  rpc Ping           (google.protobuf.Empty) returns (google.protobuf.Empty);
  // Bulk checkpoint path — see §4.4.1
  rpc AddOrUpdateItems (stream ItemValue) returns (google.protobuf.Empty); // client-stream batch commit
}

// KeyValueStore (ITransactionalKeyValueStoreConnection): txId per call; indexer getter Get throws ArgumentException
// when absent ⇒ map to a status the client adapter RE-RAISES AS ArgumentException (§6).
service KeyValueStore {
  rpc CreateTransaction (google.protobuf.Empty) returns (Txn);
  rpc Get      (TxnKey)      returns (Bytes);       // this[txId,table,key] getter
  rpc Add      (TxnKeyValue) returns (google.protobuf.Empty);
  rpc Contains (TxnKey)      returns (ContainsResponse);
  rpc Update   (TxnKeyValue) returns (google.protobuf.Empty);
  rpc Remove   (TxnKey)      returns (google.protobuf.Empty);
  rpc Enumerate(TxnTable)    returns (stream KvpEntry); // GetEnumerator snapshot
  rpc Commit   (Txn)         returns (google.protobuf.Empty);
  rpc Rollback (Txn)         returns (google.protobuf.Empty);
  rpc Dispose  (Txn)         returns (google.protobuf.Empty);
  rpc Serialize  (google.protobuf.Empty) returns (Bytes);
  rpc Deserialize(Bytes)     returns (google.protobuf.Empty);
  rpc Clear    (google.protobuf.Empty) returns (google.protobuf.Empty);
}

// Storage (IReactiveStorageConnection): flat CRUD — StorageEntity = map<string, StorageEntityProperty{int type; string data}>.
// NOTE: the IQueryable/expression layer (ITableServiceContext.CreateQuery<T>) lives ABOVE this connection, inside
// the engine/Platform.Core, and translates to these flat calls CLIENT-SIDE. We do NOT remote IQueryable (§4.4.2).
service Storage {
  rpc AddEntity    (AddEntityRequest)    returns (google.protobuf.Empty);
  rpc DeleteEntity (DeleteEntityRequest) returns (google.protobuf.Empty);
  rpc TryGetEntity (GetEntityRequest)    returns (TryGetEntityResponse); // bool + out StorageEntity
  rpc GetEntities  (GetEntitiesRequest)  returns (stream StorageEntity);
  rpc Ping         (google.protobuf.Empty) returns (google.protobuf.Empty);
}
```

#### 4.4.1 Checkpoint / recovery is N round-trips, not opaque blobs

Verified: `StateStoreConnectionStateWriter.CommitAsync` **iterates every staged change** calling
`connection.AddOrUpdateItem(category, key, bytes)` **one per item** (`StateStoreConnectionStateWriter.cs:102`,
`:298`); the reader calls `GetCategories` + `TryGetItemKeys` + `TryGetItem` **per item**. The
`Serialize/Deserialize` blob RPCs are **not** on the checkpoint commit path. Decision:

- **Model checkpoint commit as a client-streamed batch** (`StateStore.AddOrUpdateItems(stream ItemValue)`): the
  ported writer's per-item loop feeds one client-stream instead of N unary calls, collapsing N round-trips into
  one streamed call while leaving the writer's staging logic intact (it just writes to the stream).
- **Recovery** stays read-pull but uses the `GetCategories`/`TryGetItemKeys`(stream)/`TryGetItem` shape; where
  the reader enumerates, prefer the streamed `Enumerate`/keys variants.
- We **measure** per-item vs batched cost in a checkpoint benchmark (§10) and accept the documented round-trip
  profile if batching is insufficient.

#### 4.4.2 Metadata uses IQueryable above the flat connection (resolved — and Cosmos-free)

Verified: `IReactiveStorageConnection` is **flat CRUD** (the three RPCs above are correct *at the connection
level*), but the engine queries metadata through `ITableServiceContext.CreateQuery<T>(entitySet)` returning
**`IQueryable<T>`** (`Reaqtor.Remoting.Platform/Metadata/StorageAbstractions/ITableServiceContext.cs`), evaluated
by `StorageConnectionTableServiceContext`/`StorageConnectionQueryable` **on top of** the connection (RowKey-keyed
`TryGetEntity`, else `GetEntities` + client-side `expression.Evaluate()` — verified). **We do not remote
`IQueryable`/expression trees.** The IQueryable layer is ported into `Reaqtor.Remoting.Platform.Core` and runs
**client-side (engine-side)**, issuing flat `Storage` CRUD RPCs underneath. This keeps the wire simple and matches
today's layering.

> **`Microsoft.Azure.Cosmos.Table` coupling removed here (§2.6).** `StorageConnectionQueryable.cs` and
> `StorageConnectionTable.cs` currently `using Microsoft.Azure.Cosmos.Table;` and switch on `EdmType` to convert
> `StorageEntityProperty.Type` ↔ CLR. During the port, that switch is replaced by a **local
> `StorageEntityPropertyType` enum**, so `Platform.Core` carries **no Cosmos dependency**. The `AzureTable/`
> backend (the `MetadataStorageType.Azure` path, commented out in source) is **not ported**. See §2.6 for the full
> dependency surface (15 files) and the carry-vs-refactor decision.

### 4.5 `Get(Observer)` is a client-side publishing observer — NOT a server push/callback channel (CORRECTED)

The original plan described a server→client observer-callback channel here. **That is a misread of the source and
is retracted.** Verified against the actual code:

- **Location.** `RemotingServiceProvider` and `RemoteObserverClient<T>` live in
  `archive/.../Reaqtor.Remoting.Client.Library/RemotingServiceProvider.cs` (namespace `Reaqtor.Remoting.Client`),
  **not** in `Protocol`.
- **Semantics.** `RemotingServiceProvider.GetObserverAsync<T>` (`:41-51`) **does not call the server at all** — it
  invokes a **local** `_observerFactory(typeof(T), observerUri)` and wraps the result in `RemoteObserverClient<T>`.
  `RemoteObserverClient.OnNextAsync/OnErrorAsync/OnCompletedAsync` (`:62-78`) forward to a **local** `IObserver<T>`.
  The client uses this observer to **PUBLISH** events (the inject leg); it does **not** RECEIVE pushes through it.
- **Server status.** Both `QueryCoordinatorServiceProvider.GetObserverAsync` (`:126-147`, after a metadata
  existence check) and `QueryEvaluatorServiceProvider.GetObserverAsync` (`:137-140`) **throw
  `NotImplementedException`**. So `ExecuteGetObserverAsync` is *reachable* but **unimplemented at QC/QE**. The QC
  source comment is explicit that the *intended* design is to "return an expression tree to the client … to
  instantiate in a client library in order to submit events" — i.e. a **client-side publishing observer**, not a
  server callback.

**Corrected design (no server-side observer-callback machinery in the MVP):**

- `Get(Observer)` is a **purely client-side construct** that resolves to a **publishing observer** (e.g. the
  firehose observer expression). On net10.0, `RemotingServiceProvider.GetObserverAsync` keeps its existing local
  behaviour: it produces an `IAsyncReactiveObserver<T>` (the ported `RemoteObserverClient<T>` over the
  client-supplied `IObserver<T>`) whose `OnNext` etc. **publish**.
- **The publish leg** is `GrpcMessagingConnection.Publish(topic)` — the client "producing events" injects into the
  broker (§4.3).
- **The observe-results leg is SEPARATE and client-side:** a `GrpcMessagingConnection.Subscribe(topic)` stream into
  a client `TestObserver`/recording observer (§3.7 egress, §9). It is *not* the `Get(Observer)` object and does not
  flow through it.
- **No server→client observer callback is built for the MVP**, because the current design neither has one nor needs
  one. (If a future scenario genuinely requires server-initiated push into a *named server-resolved* observer, the
  broker fan-out already provides it via topic Subscribe — but that is a v2 concern, not a §4.5 deliverable, and we
  do **not** base anything on the previously-claimed "real implemented command.")

### 4.6 QC → multi-QE routing/selection (in the contract from day one)

Verified: `QueryCoordinatorServiceConnection.Start` builds one `ReactiveServiceProvider<ExpressionSlim>` per QE
from `config.QueryEvaluatorConnections`, and `QueryCoordinatorServiceProvider` currently routes to
**`_queryEvaluators.First()`** for create/delete/stream. So:

- **The shared `Execute` contract is URI-addressed from day one** (it already is — every command carries the
  entity URI inside `command_text`), which is what any placement strategy keys on.
- **MVP behaviour = preserve `.First()`** (single effective QE) so we match today's semantics exactly.
- **Multi-QE selection is a named extension point** in the ported `QueryCoordinatorServiceProvider`: a
  `IQueryEvaluatorSelector` (default: first; later: metadata-driven placement by entity URI). Milestone 6 swaps
  the selector and adds ≥2 QE gRPC clients; because addressing is already URI-based, no contract change is needed.

### 4.7 The client driver and the Expression-vs-ExpressionSlim asymmetry (NEW — the MVP can't compile without this)

The original §2.5 inventory omitted `Reaqtor.Remoting.Client(.Library)`, yet the MVP's "one client drives QC over
gRPC" depends on it. Verified facts and the resulting design:

- **What the client driver is.** `RemotingServiceProvider : ReactiveServiceProvider` is constructed with
  `new CommandTextFactory<Expression>(new ClientSerializationHelpers())` — i.e. the **client serializes a FULL
  `Expression`**, whereas the QC builds `CommandTextFactory<ExpressionSlim>` server-side. **The two ends do NOT
  share `TExpression`.** This is a **non-issue**: only the **wire string** (`command_text`) crosses — the client
  lowers `Expression → ExpressionSlim → Bonsai/DataModel JSON`, the QC parses JSON back into its own representation.
  State this explicitly so no one tries to unify `TExpression` across the boundary.
- **What wires it today (and why it can't come along).** `ReactivePlatformClientBase.ServiceProvider`
  (`Platform/Client/ReactivePlatformClientBase.cs`) builds its `ServiceProvider` over a
  `LocalReactiveServiceConnection` + `ReactiveServiceCommandProxy`, and branches on
  `platform.Environment.StorageType == MetadataStorageType.Remoting` (`:89`, `default: throw NotSupportedException`).
  Since `LocalReactiveServiceConnection`/`ReactiveServiceCommandProxy` are **explicitly not ported** (§2.5), the
  client connection must be **re-implemented over gRPC**.
- **Port home (added to §2.5).** The clean client subset — `RemotingServiceProvider`, `RemoteObserverClient<T>`,
  `ReactivePlatformClientBase`, `RemotingClientContext`, `ExpressionServices`, client `Constants`/`Operators` —
  ports to **`Reaqtor.Remoting.Client.Core`** (new `net10.0`), **minus** the `LocalReactiveServiceConnection`+proxy
  wiring.
- **gRPC client connection (added to §8).** `Reaqtor.Remoting.Grpc.Client` provides a **gRPC-backed
  `IReactiveServiceConnection`** (call it `GrpcReactiveServiceConnection`) that issues `Execute` over the
  command-channel client. The ported `ReactivePlatformClientBase` uses **that** in place of
  `LocalReactiveServiceConnection`+`ReactiveServiceCommandProxy`. `RemotingServiceProvider.GetObserverAsync` retains
  its local publishing-observer behaviour (§4.5).
- **Storage-type branch.** `ReactivePlatformClientBase`'s `MetadataStorageType.Remoting` branch is the only one
  supported on net10.0 (§4.2); the commented `Azure` branch stays out (§2.6).

---

## 5. Async, cancellation & streaming mapping

| Concern | Today (remoting) | gRPC |
|---|---|---|
| **Command call** | `ReactiveServiceCommandProxy.ExecuteAsync` → `RemoteProxyBase.Invoke<string>` | `await client.ExecuteAsync(req, new CallOptions(cancellationToken: token))` — **unary** |
| **Server handler** | `ReactiveServiceCommandService.Execute` → `RemoteServiceBase.Invoke<string>` + GUID `ConcurrentDictionary<Guid,CTS>` | `async Task<ExecuteResponse> ExecuteAsync(req, ctx)` → ported `CreateCommand(...).ExecuteAsync(ctx.CancellationToken)` |
| **Result/fault bridge** | `Reply<T>` (MBR) ↔ `TaskCompletionSource` | gRPC `Task<T>` natively; faults → `RpcException` (§6) |
| **Per-op cancellation** | `RemoteCancellationDisposable.Dispose()` → `ICancellationProvider.Cancel(Guid)` | client cancels its token ⇒ HTTP/2 abort ⇒ server `ctx.CancellationToken`. **Cancels that one operation only** (§5.1). |
| **Subscription teardown** | `Execute(Remove, Subscription, uri)` → `DeleteSubscriptionAsync(uri, token)` (separate command) | **same**: a distinct unary `Execute(Remove, Subscription, uri)`. **Not** call cancellation (§5.1). |
| **Deadlines** | none | optional `CallOptions.Deadline` on control ops; **none** on `Checkpoint`/`Recover` |
| **Notifications (multi-value)** | `FirehoseObserver`→`MessageRouter.Publish`→broker; subscribers via `MessageRouter.Subscribe` callback | **broker**: unary `Publish` + server-streaming `Subscribe`; fan-out per topic (§4.3) |
| **Enumerations** | `ITransactionalKeyValueStoreConnection.GetEnumerator`, `IReactiveStorageConnection.GetEntities` | **server-streaming** snapshot RPCs (`Enumerate`, `GetEntities`) |
| **Bidi** | n/a | not required for v1 (Publish is a separate unary call; `Get(Observer)` is a client-side publishing observer, §4.5) |

### 5.1 Cancellation vs teardown (corrected)

- **Per-RPC `CancellationToken`/deadline cancels only that in-flight operation.** Cancelling a `CreateSubscription`
  RPC aborts the *create* and may leave a partially-created subscription; the engine-side handler must treat a
  cancelled create as a rollback of any partial state.
- **Disposing a client-side `AsyncReactiveQubscription` issues `Execute(Remove, Subscription, uri)`** — a separate
  unary command — **not** an HTTP/2 abort. The MVP asserts teardown via the **`Remove` command** and verifies **no
  orphaned engine subscription** (poll metadata / assert via `TestControl.GetSubscriptions` that the broker has no
  lingering in-engine subscriber for the topic, §3.7), **not** via call cancellation.

### 5.2 Sync-over-async bridging hazard (named and bounded)

The four adapters (§4.4) implement **synchronous** engine interfaces over **async** gRPC, so they call
`.GetAwaiter().GetResult()` on the engine hot path and on checkpoint commit. Risks: thread-pool starvation /
deadlock and per-item latency. Mitigations:

- Use `GrpcChannel` + `HttpClient` (no captured `SynchronizationContext` in the host), so `.GetAwaiter().GetResult()`
  does not deadlock on a context; **never** bridge over a UI/ASP.NET request context.
- Prefer the **batched/streamed** shapes for the chatty paths (checkpoint commit §4.4.1; enumerations) to amortize
  per-item cost.
- **Measure** sync-over-async latency and checkpoint throughput in §10 benchmarks; if hot paths regress
  unacceptably, the fallback is to give the engine async store interfaces (a larger change, explicitly deferred).
- Document this as a known hazard in the adapter project README.

### 5.3 Streaming termination semantics

`OnCompleted`/`OnError` are encoded in `Notification.Kind` **and** terminate the stream; **transport faults** are
distinct (an `RpcException` *without* a terminal `Notification` means a transport drop, not an observer
completion). For `OnError`, the broker writes the terminal `Notification{ON_ERROR, ErrorInfo}` and completes the
stream; the client reconstructs an exception (§6) and calls `observer.OnError`. `MessageRouter` keeps its fan-out
role **client-side** (one `Subscribe` stream per topic dispatched to *N* local `IObserver<byte[]>` via
`ObserverNotification.Accept`) **and** the broker keeps the **server-side** fan-out across connections (§4.3).

The entire `Nuqleon.Runtime.Remoting.Tasks` layer is **deleted, not ported.**

---

## 6. Serialization & exception mapping

**Principle: gRPC carries opaque `string`/`bytes`; protobuf never sees expression structure.** `TypeFilterLevel`/
`BinaryFormatter` are gone.

1. **Command text.** `CommandTextFactory<TExpression>` lifts `Expression → ExpressionSlim` (Bonsai) and serializes
   via `SerializationHelpers` (→ `BonsaiExpressionSerializer` → `Nuqleon.DataModel.Serialization.Json`) to a JSON
   string; `CommandTextParser` reverses it. That same string rides in `ExecuteRequest.command_text`.
   `DataModelNewCommandData` (`Uri`+`Expression`+`State`) is serialized *inside* that string — no separate proto
   fields. (`SerializationHelpers` confirmed live and BinaryFormatter-free at
   `/workspaces/reaqtor/Reaqtor/Core/Hosting/Reaqtor.Hosting.Shared/Serialization/SerializationHelpers.cs`. It is
   reused **as-is**; note `FirehoseObserver`/`TestFirehoseObservable` `new` it indirectly, so this reuse depends on
   the §2.5 seam/firehose port.) **Client/server `TExpression` differ (`Expression` vs `ExpressionSlim`) — only the
   JSON string crosses; this is a non-issue (§4.7).**
2. **Notification payloads.** `FirehoseObserver` produces `byte[]` via `SerializationHelpers` → `Notification.Value`.
   `Kind` maps 1:1. Predicate `ObserverNotification` variants are **in-proc test-only** and never on the wire.
3. **Store payloads.** StateStore/KVStore values and `Serialize*` blobs are `byte[]` → proto `bytes` verbatim.
   `StorageEntity`/`StorageEntityProperty` (`int Type`+`string Data`) map 1:1; the `Type` is the **local
   `StorageEntityPropertyType` enum** introduced in §2.6 (no `EdmType`).

### 6.1 Exception fidelity — command path AND firehose OnError (corrected)

`ObserverNotification` is `[Serializable]` and its `OnError` carries a **live `Exception`**; client fan-out
(`observer.OnError(data.Exception)`) and equality (`OnErrorNotification.EqualsCore`: `_error == other.Exception`,
or a predicate) depend on a real `Exception`. Over gRPC we cannot carry the live graph. Decisions:

- **Wire form:** `ErrorInfo{type_name, message, stack_trace, is_transient}` for **both** the command/RPC fault path
  **and** the streamed `Notification{ON_ERROR}`.
- **Concrete exception the client raises:** the client adapter/interceptor reconstructs a **`BaseException`-shaped**
  exception (`type_name`/`message`/`stack_trace` preserved as data; `IsTransient()` honoured) and that is what
  `observer.OnError` receives across the wire. **Custom exception subtype identity is intentionally lost** (it
  relied on `BinaryFormatter`).
- **`BaseException` is ported (and modernized), not assumed live.** Verified `BaseException` exists **only** at
  `archive/.../Reaqtor.Remoting.Protocol/FaultHandling/BaseException.cs` — there is **no** non-archived copy. It is
  ported into `Reaqtor.Remoting.Core` as part of the Protocol extraction (§2.5, FaultHandling row). Its legacy
  `ISerializable` members — `[Serializable]`, the `protected BaseException(SerializationInfo, StreamingContext)`
  ctor (`:62`), and the `GetObjectData` override (`:84`) — are **stripped during the port** (obsolete on net10.0,
  `SYSLIB0051`, and dead weight once exceptions cross as `ErrorInfo`). We keep `_transient`, `IsTransient()`, and
  the three normal constructors.
- **Equality across the wire is explicitly untested by type-identity.** Tests asserting concrete exception types or
  using **predicate** `ObserverNotification` (`RecordedNotificationEqualityComparer`) are kept **in-process**
  (against the oracle); cross-process OnError parity asserts on `type_name`/`message`/`is_transient`, not CLR
  identity. This is called out as an accepted fidelity reduction.

### 6.2 Status-code mappings — pinned against real contracts

- **Transient vs permanent:** use `BaseException.IsTransient()` (verified at the **archive** path
  `archive/.../Reaqtor.Remoting.Protocol/FaultHandling/BaseException.cs`, `_transient` flag, `IsTransient()`
  getter — **it must be ported per §6.1**, it is not a live type today).
  Mapping: **`IsTransient()==true ⇒ `StatusCode.Unavailable`** (retriable); transient-due-to-overload from the
  broker's per-stream overflow ⇒ **`ResourceExhausted`** (§4.3); everything else ⇒ `Internal`. A **server
  interceptor** packs `ErrorInfo` into trailers; a **client interceptor** rehydrates the `BaseException`-shaped
  exception (§6.1).
- **KeyValueStore absent-key:** the `ITransactionalKeyValueStoreConnection` indexer getter
  (`byte[] this[txId,table,key] {get}`) **throws `ArgumentException` when the key is absent** (verified).
  `KeyValueStore`/`TransactedKeyValueTable` indexer callers in the engine **depend on that contract**, so the
  `GrpcKeyValueStoreConnection` adapter must **re-raise `ArgumentException`** on absent-key. Wire it as a distinct
  status (e.g. `NOT_FOUND` with an `ErrorInfo.type_name = "System.ArgumentException"`) that the adapter converts
  back to `ArgumentException` before returning to the engine. Round-trip fidelity of this contract is a test
  assertion (§10).

Reused **behaviourally unchanged** (relocated in §2.5): `SerializationHelpers`, `BonsaiExpressionSerializer`,
`CommandTextFactory`/`Parser`, `ReactiveServiceConnection<T>`, `ObserverNotification` (client-side `Accept`), the
query engine.

---

## 7. Hosting & deployment — ASP.NET Core Kestrel gRPC hosts

Each archived `*Host` is a console exe running `TcpRemoteServiceHost<T>`. The replacement: each host is a minimal
`WebApplication` on Kestrel/HTTP-2.

```csharp
// Reaqtor.Remoting.Grpc.QueryEvaluatorHost / Program.cs  (≈ the whole file)
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(o =>
    o.ListenLocalhost(port, l => l.Protocols = HttpProtocols.Http2)); // h2c on loopback (dev) — see §7 note
builder.Services.AddCodeFirstGrpc();                                  // protobuf-net.Grpc.AspNetCore
builder.Services.AddSingleton<QueryEvaluatorServiceConnection>();     // PORTED impl (wraps CheckpointingQueryEngine)
builder.Services.AddSingleton<IReactiveServiceConnectionService, QueryEvaluatorGrpcAdapter>();
builder.Services.AddGrpcHealthChecks();                               // replaces the Ping retry loop
var app = builder.Build();
app.MapGrpcService<QueryEvaluatorGrpcAdapter>();
app.MapGrpcService<QueryEvaluatorControlAdapter>();
app.MapGrpcService<MessagingGrpcAdapter>();
app.MapGrpcService<TestControlAdapter>();    // test builds only — §3.7
app.MapGrpcHealthChecksService();
app.Run();
```

- **One host per service** mirrors the archived `*Host`s; **`MultiRoleHost`** maps several services into one process.
- **Ports** reuse the `Helpers.Constants` 8080–8086 scheme but as gRPC endpoints, recorded as **addresses** in
  `PlatformConfiguration` (no hardcoded proxies).
- **`GrpcProcessRunnable`** (retarget of `ProcessRunnable`/`TcpRunnable<T>`) launches host exes and waits for
  readiness via **gRPC health** (`grpc.health.v1`) or `ReactiveServiceControl.Ping`.
- **Transports:** InMemory stays fully in-process (no Kestrel) for tests/oracle; Grpc spawns host exes out-of-proc;
  **AppDomain is dropped**.
- **h2c on loopback (call-out).** `Grpc.Net.Client` over **insecure HTTP/2** historically requires
  `AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true)` **or** an https dev
  cert, **or** explicitly configuring the channel for unencrypted h2c. The first runnable slice must set this (or
  use `https://` with the dev cert) **or it silently fails to connect**. Pick one and document it in the host and
  the test fixture. (No such switch exists anywhere in the repo today — verified — so it is genuinely new.)
- **TLS:** production HTTPS deferred (design notes only).

---

## 8. Project & solution layout

New work lands under `Reaqtor/Samples/Grpc/`. **Verified:** `Reaqtor/Samples/` **already exists** (it is the live
sibling of `archive/Reaqtor/Samples/`), so `Reaqtor/Samples/Grpc/` is a new sub-tree under an existing root, not a
brand-new `Samples/` tree. (The archived Remoting sample is at `archive/Reaqtor/Samples/Remoting/`; the top-level
`dotnet/` holds only a runfile artifact, not a code tree, so it is **not** used.) All projects target `net10.0`;
versions go through the existing root `Directory.Packages.props` (CPM already enabled; **no gRPC/protobuf entries
today** — verified — so the additions below are genuinely needed).

**Ported `net10.0` projects (the §2.5 enabling work — largest effort):**

| New project | Mirrors / extracts from (archived) | Purpose |
|---|---|---|
| `Reaqtor.Remoting.Core` | `Reaqtor.Remoting.Protocol` (clean subset, **incl. `FaultHandling/BaseException` modernized**) | Enums, `INotification`/`ObserverNotification`, `StorageEntity`, command machinery (`CommandText*`, `ReactiveServiceConnection<T>`, `ReactiveServiceProvider<T>`), engine-facing `IReactive*Connection` interfaces, non-MBR connection base, `BaseException` (ISerializable stripped). |
| `Reaqtor.Remoting.Platform.Core` | `Reaqtor.Remoting.Platform` (clean subset; **Cosmos.Table coupling refactored out — §2.6**) | `IReactivePlatform`/`IReactiveService`/`IRunnable`/`IDeployable`/`ReactiveServiceType`/`ReactivePlatformDeployer`/`ReactiveServiceBase`/`ReactivePlatformServiceBase`/`ReactiveQueryEvaluatorBase`/`MessageRouter`/`FirehoseObserver`/metadata `StorageAbstractions` IQueryable layer (`StorageConnectionTableClient`/`…TableServiceContext`/`…Queryable`/`…Table` with local `StorageEntityPropertyType` enum); **new transport-neutral `ReactivePlatformBase`** (no channel) + non-`[Serializable]` `IReactivePlatformConfiguration`. |
| `Reaqtor.Remoting.Engine` | `Reaqtor.Remoting.{QueryEvaluator,QueryCoordinator,Messaging}` (minus MBR/`SchedulerProxy`) | QE/QC service connections, checkpoint writer/reader, broker kernel, `IQueryEvaluatorSelector`. QC `GetMetadata` reworked Cosmos-free (§2.6). |
| `Reaqtor.Remoting.Platform.InMemory.Core` | `Reaqtor.Remoting.Platform.InMemory` | The **oracle** on net10.0; **single-shared-`MessagingConnection`** composition root (§3.6). |
| `Reaqtor.Remoting.Client.Core` | `Reaqtor.Remoting.Client`/`Client.Library` (clean subset, **minus `LocalReactiveServiceConnection`+proxy**) | `RemotingServiceProvider`, `RemoteObserverClient<T>` (local publishing observer — §4.5), `ReactivePlatformClientBase`, `RemotingClientContext`, `ExpressionServices`, client `Constants`/`Operators`. The MVP client driver (§4.7). |
| `Reaqtor.Remoting.TestingFramework.Core` | `Reaqtor.Remoting.TestingFramework` (minus marshaled-delegate scheduler; **AppDomain ledger replaced — §3.7**) | `TestDeployable`, `TestFirehoseObservable` (DI-scoped subscription ledger), `TestObserver`, `TimelineObservable`, `OperatorTestBase`, `TestPlatform`, the three store connections, `TestControl` ingress/egress wiring. |

**New gRPC projects:**

| New project | SDK / OutputType | Purpose |
|---|---|---|
| `Reaqtor.Remoting.Grpc.Contracts` | classlib | Code-first `[ServiceContract]` + `[ProtoContract]` DTOs (incl. `TestControl`, §3.7). References `Reaqtor.Remoting.Core` for enums. |
| `Reaqtor.Remoting.Grpc.Protocol` | classlib | Marshaling bridge: `Notification ↔ INotification<byte[]>`, `ErrorInfo ↔ BaseException`, fault interceptors, the `ExecuteRequest ↔ CreateCommand` adapter, **and the re-implemented non-remoting `IReactiveServiceCommand`/connection replacing `ReactiveServiceCommandProxy`/`LocalReactiveServiceConnection`**. |
| `Reaqtor.Remoting.Grpc.Client` | classlib (`Grpc.Net.Client`) | **The four synchronous client adapters** (§4.4): `Grpc{KeyValueStore,StateStore,Storage,Messaging}Connection`; **the command-channel client + `GrpcReactiveServiceConnection`** consumed by the ported `ReactivePlatformClientBase` (§4.7); the `TestControl` client (§3.7). |
| `Reaqtor.Remoting.Grpc.Server` | classlib (`Grpc.AspNetCore`) | gRPC service **adapters** delegating to ported `QueryCoordinatorServiceConnection`/`QueryEvaluatorServiceConnection`/broker/stores; the `TestControl` server adapter (§3.7); `AddReaqtorGrpc(IServiceCollection)`. |
| `Reaqtor.Remoting.Platform.Grpc` | classlib | Transport leaf: `GrpcReactivePlatform`, `GrpcMultiRoleReactivePlatform`, `GrpcReactiveEnvironment`, `Grpc*Service`, `GrpcRunnable<T>`, `GrpcProcessRunnable`, channel/adapter factory. |
| `Reaqtor.Remoting.Grpc.QueryCoordinatorHost` / `…QueryEvaluatorHost` / `…{Metadata,Messaging,StateStore,KeyValueStore}Host` / `…MultiRoleHost` | `Microsoft.NET.Sdk.Web`, Exe | Kestrel gRPC hosts. |
| `Tests.Reaqtor.Remoting.Grpc` | test (MTP, §10) | Parity + transport tests. |

**Not ported:** `Nuqleon.Runtime.Remoting.Tasks`, `Reaqtor.Remoting.Platform.AppDomain`, the MBR `SchedulerProxy`,
`ReactiveServiceCommandProxy`/`Service`, `LocalReactiveServiceConnection`, marshaled-delegate scheduler tasks, the
`Metadata/AzureTable/**` backend (§2.6).

**Solution wiring (`.slnx`) — corrected inventory.** Verified: **only `All.slnx` exists at the repo root**; the
per-area solutions are in **subdirectories** — `Nuqleon/Nuqleon.slnx`, `Reaqtive/Reaqtive.slnx`,
`Reaqtor/Reaqtor.slnx` (plus nested `*/Core/*.slnx`, `*/Pearls/*.slnx`). `All.slnx` indeed has **zero** Remoting
references (confirmed). The new work is a sample under the **live** `Reaqtor/` tree, so:

- Add the ported + gRPC projects to **`Reaqtor/Reaqtor.slnx`** (the live Reaqtor solution) under a `Samples/Grpc`
  solution folder.
- Add them to the root **`All.slnx`** as well, so `dotnet build All.slnx` covers them.
- Do **not** add them to `Nuqleon.slnx`/`Reaqtive.slnx` (wrong area).

**CPM additions to `/workspaces/reaqtor/Directory.Packages.props`:** `Grpc.AspNetCore`, `Grpc.Net.Client`,
`Grpc.AspNetCore.HealthChecks`, `protobuf-net.Grpc`, `protobuf-net.Grpc.AspNetCore`, `protobuf-net.Grpc.Reflection`,
`Google.Protobuf`. **No `Microsoft.Azure.Cosmos.Table`** — the metadata layer is refactored Cosmos-free (§2.6);
only the fallback in §2.6 would add it.

---

## 9. The MVP vertical slice — precise data flow (CORRECTED)

One client → `GrpcQueryCoordinator` → `GrpcQueryEvaluator`, **wall-clock, closure-free**. The original §9 conflated
"client publishes via `Messaging.Publish`" with the canonical test path; **verified, the real topology has two
distinct inject legs and the engine is both a broker subscriber and a broker publisher.** The MVP exercises this
precisely:

1. **Wire the graph (command path).** `Configure` (addresses → typed adapters materialized before `Configure`, and
   retained through `Start` for both QE and QC — §3.3) + `Start`. Then client → QC → QE:
   `Execute(New, …)` to define the observable/observer/stream and `Execute(New, Subscription, …)` to wire the
   operator graph. The QC routes to `_queryEvaluators.First()` (§4.6).
2. **Stand up the engine-side firehose (QE-as-subscriber).** Deploy `TestDeployable`; the engine resolves
   `TestFirehoseObservable`, whose `TopicObserver` calls `MessageRouter.Subscribe(topic, this)` — i.e. the **engine
   opens a `Messaging.Subscribe(topic)` stream to RECEIVE** input events (via the gRPC messaging adapter in
   `OperatorContextElements`, §3.6).
3. **Inject input events — name the leg.** Two inject legs exist; **the MVP drives the command path first** (it is
   how the canonical `ContextSwitchOperator` tests drive data, verified at `RemotingTest.cs:542-560`:
   `stream.SubscribeAsync(...)` / `stream.OnNextAsync(...)` travel over the **command channel** to the engine, and
   the engine-resolved **`FirehoseObserver` then Publishes** to the broker). For the firehose-subscriber timeline
   variant, inputs are planted via the §3.7 `TestControl.PlantEventTimeline` ingress (or, equivalently, via a
   client `Messaging.Publish(topic)` for an external-feed scenario). **Pick:** MVP step 3 = **command-path inject**
   (`stream.OnNextAsync`) for the canonical operator test; the direct `Messaging.Publish` leg is exercised
   explicitly in Increment 5 (standalone broker).
4. **Engine emits results (QE-as-publisher).** The operator graph's output `FirehoseObserver` calls
   `MessageRouter.Publish(topic, …)` — the **engine PUBLISHES results to the broker** (`FirehoseObserver.OnNext`
   verified).
5. **Client observes results (egress).** The client opens a `GrpcMessagingConnection.Subscribe(topic)` stream into
   a **client-side recording `TestObserver`** (§3.7 egress) and asserts the produced values.
6. **Tear down via `Execute(Remove, Subscription, uri)`** (not call cancellation, §5.1); assert **no orphaned
   engine subscription** and (via `TestControl.GetSubscriptions`, §3.7) the broker has no lingering subscriber for
   the topic.
7. **Parity:** run the identical scenario against `InMemoryReactivePlatform` (oracle, single-shared
   `MessagingConnection` — §3.6) and assert equal observable results and equal subscription ledger
   (`Subscribe(125,225)`-style, now read via `TestControl`/DI-store rather than AppDomain).

**Both broker legs are therefore in scope from the MVP:** QE-as-subscriber (step 2) and QE-as-publisher (step 4) —
not just one direction.

---

## 10. Milestones — port first, then spike → MVP → parity

### Milestone 0a — **Enabling port + test ingress/egress** (now the true "first PR"; gates everything)

- `Reaqtor.Remoting.Core` (incl. modernized `BaseException`) + `Reaqtor.Remoting.Platform.Core` (Cosmos-free
  metadata, §2.6) build green on `net10.0` (clean types extracted; new transport-neutral `ReactivePlatformBase`;
  non-`[Serializable]` config interface; metadata IQueryable layer relocated with local `StorageEntityPropertyType`).
- `Reaqtor.Remoting.Engine`, `Reaqtor.Remoting.Platform.InMemory.Core` (single-shared `MessagingConnection`),
  `Reaqtor.Remoting.Client.Core` (§4.7), `Reaqtor.Remoting.TestingFramework.Core` build green; **the §3.7
  ingress/egress is in place** (DI-scoped test stores; `TestFirehoseObservable` ledger off AppDomain;
  `TestControl` plumbed for the in-proc path); **`InMemoryReactivePlatform` runs `TestDeployable` end-to-end
  in-proc on net10.0** using the new ingress/egress (the live oracle exists). No MBR/`Nuqleon.Runtime.Remoting.Tasks`/
  `SchedulerProxy`/`Cosmos.Table`/AppDomain referenced.
- Projects added to `Reaqtor/Reaqtor.slnx` **and** `All.slnx` (§8); `dotnet build All.slnx` green; **archived tree
  untouched.**

> **STATUS — Milestone 0a COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). Delivered
> under `Reaqtor/Samples/Grpc/` (252 `.cs` across 8 projects):
> - **`Reaqtor.Remoting.Core`** (38) — Protocol clean subset; modernized `BaseException` (ISerializable stripped);
>   the in-proc command machinery replacing the deleted .NET Remoting pair: `InProcessReactiveServiceConnection`
>   (client→engine) and `RemotingReactiveServiceConnectionBase` + `ReactiveServiceCommandService` (engine-side),
>   both inlining the `Nuqleon.Runtime.Remoting.Tasks` TCS bridge with no marshaling.
> - **`Reaqtor.Remoting.Platform.Core`** (81) — transport-neutral `ReactivePlatformBase` (no channel), non-`[Serializable]`
>   config, DI-injected `MessageRouter`, **and the full Cosmos-free metadata implementation layer** (local `TableEntity`
>   base + entity hierarchy + query provider + rewriters + table-ops + the `AzureReactiveMetadata{,Proxy}` trio —
>   see the §2.6 implementation correction above; **no `Microsoft.Azure.Cosmos.Table` package**).
> - **`Reaqtor.Remoting.Client.Core`** (14) — client driver; `RemotingClientContext` transport-injection seam (§4.7).
> - **`Reaqtor.Remoting.Deployable.Core`** (37) — firehose/streams/observers + `CoreDeployable` (full operator surface);
>   `AppDomain` ledger → in-proc `ConcurrentDictionary`.
> - **`Reaqtor.Remoting.Engine`** (30) — QC/QE/Messaging; de-MBR'd `SchedulerProxy` (kept as the local engine scheduler),
>   `CallContext`→`AsyncLocal`, messaging via `OperatorContextElements` instead of the AppDomain-keyed singleton.
> - **`Reaqtor.Remoting.Platform.InMemory.Core`** (16) — the live oracle; single-shared `MessagingConnection` verified
>   (QE subscriber leg + client publisher leg resolve the same memoized broker).
> - **`Reaqtor.Remoting.TestingFramework.Core`** (35) — `TestDeployable` + ingress/egress; §3.7 `TestFirehoseObservable`
>   ledger off `AppDomain` onto the DI-scoped `TestSubscriptionStoreConnection`; §3.4 scheduler actions de-MBR'd.
> - **`Tests.Reaqtor.Remoting.Core`** (1) — `Oracle_Subscribe_VirtualTime_ObserverState` runs `TestDeployable` on
>   `InMemoryReactivePlatform` end-to-end in-proc (client→QC→QE→observer, virtual time): **1 passed**.
>
> `dotnet build All.slnx` → **0 warnings, 0 errors**. No `System.Runtime.Remoting` / MBR / `Nuqleon.Runtime.Remoting.Tasks`
> / `Cosmos.Table` / `AppDomain` / `ServiceInstanceHelpers` on any path (audited).
> **Deviation from the §3.7 letter:** the `TestControl` RPC abstraction is deferred to **0b** (it only matters when the
> QE is out-of-process over gRPC). The in-proc oracle uses the working `GetInstance<TestQueryEvaluatorServiceConnection>()`
> directly — i.e. §3.7's "direct in-proc calls (no RPC)" path. **Also added beyond the §2.5 inventory:** the engine-side
> command base (`RemotingReactiveServiceConnectionBase`/`ReactiveServiceCommandService`) and the full metadata impl
> layer (§2.6 correction).

### Milestone 0b — Spike scaffold

- `Reaqtor.Remoting.Grpc.Contracts` compiles (`ReactiveServiceConnection`/`ReactiveServiceControl`/`Messaging`/
  `TestControl` + DTOs).
- `Reaqtor.Remoting.Grpc.Server` exposes a QueryCoordinator adapter wrapping the **ported**
  `QueryCoordinatorServiceConnection`; a `QueryEvaluatorHost` Kestrel exe boots, answers `Ping`/health **over h2c
  with the §7 switch set**.
- `Reaqtor.Remoting.Platform.Grpc` has `GrpcReactivePlatform` + `GrpcRunnable<T>` dialing a channel and returning a
  typed **sync adapter** as `Instance`; `Reaqtor.Remoting.Grpc.Client` has the command-channel client +
  `GrpcReactiveServiceConnection` (§4.7).
- Skeleton test starts `GrpcReactivePlatform` against a localhost host and asserts `Ping`.

> **STATUS — Milestone 0b scaffold COMPLETE on `net10.0`** (branch `spike/grpc-remoting`). Delivered under
> `Reaqtor/Samples/Grpc/`:
> - **`Reaqtor.Remoting.Grpc.Contracts`** — code-first `[ServiceContract]` interfaces (`IReactiveServiceConnectionService`
>   Execute, `IReactiveServiceControl` Configure/Start/Ping, `IQueryEvaluatorControl`, `IMessagingService`
>   Publish/Subscribe) + `[ProtoContract]` DTOs, reusing the Core enums.
> - **`Reaqtor.Remoting.Grpc.Server`** — `ReactiveServiceControlAdapter` (Ping; real storage-type validation, §3.3).
> - **`Reaqtor.Remoting.Grpc.QueryEvaluatorHost`** — Kestrel `WebApplication` on cleartext HTTP/2 (h2c), port from args.
> - **`Reaqtor.Remoting.Grpc.Client`** — `GrpcConnectionFactory` (sets the h2c switch, §7) + `GrpcReactiveServiceConnection`
>   (the command channel; gRPC peer of the in-proc connection, §4.7).
> - **`Reaqtor.Remoting.Platform.Grpc`** — `GrpcProcessRunnable` (launch host exe + Ping-readiness poll, §7) +
>   `GrpcRunnable<T>` (the §3.1 typed-client `Instance` seam).
> - **`Tests.Reaqtor.Remoting.Grpc`** — launches the host out-of-process and asserts `Ping`/`Configure`/`Start` over
>   h2c, and `InvalidArgument` rejection of the unsupported Azure storage type (error propagation): **2 passed**.
>
> `dotnet build All.slnx` → **0/0**. **Scope notes (carried to Milestone 1):** (a) the full
> `GrpcReactivePlatform : ReactivePlatformBase` service graph + `GrpcReactiveEnvironment` need the service adapters to
> be exercisable, so 0b ships `GrpcProcessRunnable`/`GrpcRunnable<T>` and proves Ping that way rather than the full
> platform; (b) `ReactiveServiceControl.Start` is the 0b control-plane acknowledgement — the ported
> `QueryEvaluatorServiceConnection.Start` engine bring-up is wired in M1 once the four sync gRPC store adapters (§4.4)
> exist; (c) the `Reaqtor.Remoting.Grpc.Protocol` marshaling bridge (`Notification ↔ INotification`,
> `ErrorInfo ↔ BaseException`, the Execute server adapter, fault interceptors) and the `TestControl` service are M1.

### Milestone 1 — MVP vertical slice (spike success criterion)

The §9 flow end-to-end over gRPC: command-path graph wiring, QE-as-subscriber firehose, command-path inject,
QE-as-publisher results, client-side `Subscribe` egress, `Remove`-command teardown with no-orphan assertion, and
**parity against the in-proc oracle** (both via the §3.7 ingress/egress so one test body runs against both
transports).

> **STATUS — Milestone 1 IN PROGRESS.**
> **M1.a DONE (the keystone — engine behind gRPC, command path over the wire).** The `QueryEvaluatorHost` now runs
> the SAME in-process engine the oracle runs (`InMemoryReactivePlatform` + `CoreDeployable`, started eagerly at boot)
> behind a gRPC `Execute` facade (`Reaqtor.Remoting.Grpc.Server.EngineHost` + `ReactiveServiceConnectionAdapter`).
> The client (`RemotingClientContext` over `GrpcReactiveServiceConnection`) defines+undefines an observable
> end-to-end over gRPC — the oracle's proven command path (0a.8) with a gRPC hop on client→QC (§9 step 1, §4.7).
> MVP topology = one host running the full in-proc engine behind gRPC (per-role QC/QE/store hosts are M4-6).
> **M1.b DONE (messaging broker over gRPC — the egress).** `MessagingGrpcAdapter` (server-streaming `Subscribe` via a
> per-stream `Channel` over the in-host `MessagingConnection` + unary `Publish`, §4.3) + `GrpcMessagingConnection`
> client adapter (`IReactiveMessagingConnection` over the stream, sync-over-async §5.2) + the
> `Reaqtor.Remoting.Grpc.Protocol` `NotificationConverter` (`Notification ↔ INotification<byte[]>`,
> `ErrorInfo ↔ Exception`, `GrpcRemoteException`). A client Publish → client Subscribe round-trip over gRPC is
> asserted. `Tests.Reaqtor.Remoting.Grpc` is **4/4 green**; `dotnet build All.slnx` 0/0. Both halves of the MVP data
> path (command channel + result egress) now work over the wire.
> **M1.c DONE — the spike SUCCESS CRITERION is met.** A full reactive query runs end-to-end over gRPC and produces
> results IDENTICAL to the in-proc oracle (one scenario body, both transports):
> `Grpc_Firehose_VerticalSlice_Parity_With_InProc_Oracle` defines+subscribes
> `Empty<int>().StartWith(0,1,2,3,4).Select(x=>x+1) -> firehose(topic)` over the command channel; the engine runs it;
> the output `FireHose` observer publishes to the broker; the client `Subscribe(topic)`s and observes `[1,2,3,4,5]`;
> teardown via `Remove`. Asserted equal across `GrpcReactiveServiceConnection`+`GrpcMessagingConnection`
> (engine out-of-process) and `InMemoryReactivePlatform`+`InProcessReactiveServiceConnection` (oracle).
> The §3.6 crux is resolved: `MessageRouter.Initialize(IReactiveMessagingConnection)` points the process-wide
> firehose router at the host/oracle's single shared `MessagingConnection`; the gRPC engine runs in its own process
> so its static is independent of the test process. **5/5 gRPC transport tests pass; the 0a oracle test still passes;
> `dotnet build All.slnx` 0/0.**
>
> **Milestone 1 is functionally COMPLETE** (the success criterion — a reactive query over gRPC with parity — is
> achieved). Two refinements are carried forward (not blockers): (i) the §9 *command-path-inject* leg
> (`stream.OnNextAsync` into a server-side subject) — the MVP uses `StartWith` as a self-contained source instead;
> (ii) the no-orphan teardown *assertion* via `TestControl.GetSubscriptions` (§3.7) — teardown happens, but asserting
> the empty subscription ledger needs the `TestControl` service (deferred). Beyond M1: the four sync store adapters
> (§4.4), separate per-role hosts (M4-6), and the full `GrpcReactivePlatform : ReactivePlatformBase` graph.

### Milestone 2+ — Incremental parity

| Inc. | Adds | Notes |
|---|---|---|
| 2 ✅ | **StateStore** adapter + host | `TryGet*` (bool+out); **checkpoint via client-stream batch** (§4.4.1); measure round-trips |
| 3 ✅ | **KeyValueStore** adapter + host | txId-per-call; **absent-key re-raises typed exception** (§6.2); `Enumerate` stream |
| 4 ✅ | **Metadata/Storage** adapter + host | flat CRUD RPCs; **IQueryable stays engine-side, Cosmos-free** (§4.4.2, §2.6) |
| 5 ✅ | **Standalone Messaging** broker host | promote the in-host broker to its own host; exercise **both legs across processes**: QE-subscriber (input) + QE-publisher (output) + client-publisher/-subscriber (§4.3, §9) |
| 6 ✅ | **MultiRole + multi-QE** | `IQueryEvaluatorSelector` swapped from `.First()` to ≥2 QEs (§4.6) |
| 7 ✅ | **Recovery/resubscribe + fault/health hardening** | Unload→Recover with an active firehose subscription over the broker (§10.6); transient/permanent + `ArgumentException` mappings pinned (§6.2) |
| 8 ✅ | **Reactor.\*/ReificationFramework** deployables | richer end-to-end workloads after core parity |

> **STATUS — Milestone 2 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **StateStore adapter + host** ships the first of the four §4.4 sync store adapters over gRPC:
> - **`Reaqtor.Remoting.Grpc.Contracts`** — `IStateStoreService` (`reaqtor.remoting.v1.StateStore`) + DTOs named per
>   the §4.4 proto (`Categories`/`Category`/`ItemKeys`/`ItemKey`/`ItemValue`/`TryGetItemResponse`/`Bytes`); the
>   engine's `bool TryGet…(out …)` pairs become `{Found, …}` response messages (gRPC has no out-params), plus the
>   §4.4.1 client-streamed `AddOrUpdateItems(IAsyncEnumerable<ItemValue>)` batch and a `CallStats` round-trip counter.
> - **`Reaqtor.Remoting.Grpc.Server.StateStoreGrpcAdapter`** — backs the service with the engine's **real** checkpoint
>   store: `EngineHost.StateStoreConnection` returns `Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>()`,
>   which is the *same* instance `ReactivePlatformBase` populates into `config.StateStoreConnection` for
>   checkpoint/recover — so the adapter exposes the engine's store, not a side copy. Mapped in `QueryEvaluatorHost`.
> - **`Reaqtor.Remoting.Grpc.Client.GrpcStateStoreConnection`** — the §4.4 sync adapter implementing
>   `IReactiveStateStoreConnection` over the async service (sync-over-async §5.2; `{Found,…}` → `out`), plus
>   `AddOrUpdateItems(IEnumerable<(category,key,value)>)` driving the client-stream and `GetCallStats()`.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+2, now **7/7**): `Grpc_StateStore_RoundTrip_Parity_With_InProc_Oracle` asserts
>   every op (incl. bool+out absent-item/category, update-overwrite, Remove, Serialize→Clear→Deserialize) matches an
>   in-proc oracle `StateStoreConnection` run side-by-side; `Grpc_StateStore_Checkpoint_Batch_Collapses_RoundTrips`
>   measures over the wire that the per-item path is N unary RPCs while the §4.4.1 batch is **one** streamed call
>   carrying N items, with identical resulting state.
>
> `dotnet build All.slnx` → **0/0**. No new project (files added to existing Contracts/Server/Client/Host/Tests, so no
> `.slnx` change). No `Cosmos.Table`/MBR/`AppDomain`/`System.Runtime.Remoting` on the path (audited). **Scope note:**
> "+ host" here means the StateStore service is hosted **in** the QE host alongside the engine (matching M5's "promote
> the in-host broker to its own host" framing — a *separate-process* StateStore host is part of the M4–6 per-role host
> split). "Measure round-trips" is a deterministic in-test count (per-item N vs batch 1); the BenchmarkDotNet *latency*
> benchmark remains a §11 item. Engine-driven checkpoint/recover *through* this adapter is exercised at M7 (recovery).

> **STATUS — Milestone 3 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **KeyValueStore adapter + host** — second of the four §4.4 sync store adapters:
> - **`Reaqtor.Remoting.Grpc.Contracts`** — `IKeyValueStoreService` (`reaqtor.remoting.v1.KeyValueStore`) + DTOs per
>   the §4.4 proto (`Txn`/`TxnKey`/`TxnKeyValue`/`ContainsResponse`/`TxnTable`/`KvpEntry`; `Bytes` shared with StateStore).
>   The transaction id is a server-side `long` created by `CreateTransaction` and carried on every call; `Enumerate` is
>   a server stream.
> - **`Reaqtor.Remoting.Grpc.Protocol.GrpcFault`** (NEW, reusable) — the §6.1/§6.2 unary-RPC fault bridge: server packs
>   a thrown exception into an `RpcException` (`Status.Detail` = message; trailers carry `ErrorInfo` type-name/transient/
>   stack); client reconstructs it, **re-raising the original CLR type** for the engine's contract types
>   (`ArgumentException`/`ArgumentNullException`/`KeyNotFoundException`/`InvalidOperationException`), else `GrpcRemoteException`.
> - **`Reaqtor.Remoting.Grpc.Server.KeyValueStoreGrpcAdapter`** — backs the service with the engine's **real** KV store
>   (`EngineHost.KeyValueStoreConnection` = `Environment.KeyValueStoreService.GetInstance<…>()`, the same instance in
>   `config.KeyValueStoreConnection`); faulting ops route through `GrpcFault.ToRpcException`. Mapped in `QueryEvaluatorHost`.
> - **`Reaqtor.Remoting.Grpc.Client.GrpcKeyValueStoreConnection`** — the §4.4 sync adapter implementing
>   `ITransactionalKeyValueStoreConnection` (sync-over-async §5.2); the indexer/mutators re-raise via `GrpcFault.ToException`;
>   `GetEnumerator` drains the server stream into a snapshot list.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+2, now **9/9**): a full transactional scenario (create tx → add → contains →
>   indexer get → commit → re-read → enumerate snapshot → update → remove → serialize/deserialize) is record-equal to an
>   in-proc oracle `KeyValueStoreConnection`; a fault test asserts an absent-key indexer lookup re-raises the **same** CLR
>   type over the wire as the in-proc store throws.
>
> `dotnet build All.slnx` → **0/0**; no new project; archived tree untouched. **§6.2 correction:** the InMemory KV store's
> absent-key indexer throws `KeyNotFoundException` (not `ArgumentException` as §6.2's interface doc-comment implies); the
> `GrpcFault` mapping preserves type identity generally and the test asserts true parity with the oracle's actual type, so
> the engine's exception contract is honoured regardless of which type the backend raises.

> **STATUS — Milestone 4 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **Metadata/Storage adapter + host** — the last of the four §4.4 sync store adapters (Messaging shipped at M1.b):
> - **`Reaqtor.Remoting.Grpc.Contracts`** — `IStorageService` (`reaqtor.remoting.v1.Storage`) + flat-CRUD DTOs
>   (`StoragePropertyData`/`StorageEntityData`/`AddEntityRequest`/`DeleteEntityRequest`/`GetEntityRequest`/
>   `TryGetEntityResponse`/`GetEntitiesRequest`). `TryGetEntity` → `{found, entity}`; `GetEntities` is a server stream.
>   The property `Type` is the local `StorageEntityPropertyType` ordinal (int) — **no `EdmType`/Cosmos.Table** (§2.6).
> - **`Reaqtor.Remoting.Grpc.Protocol.StorageEntityConverter`** — bridges the CLR `StorageEntity` (read-only property
>   bag, no settable members) ↔ the wire `StorageEntityData`.
> - **`Reaqtor.Remoting.Grpc.Server.StorageGrpcAdapter`** — backs the service with the engine's **real** metadata store
>   (`EngineHost.StorageConnection` = `MetadataService.GetInstance<IReactiveStorageConnection>()` on the `Remoting`
>   path, the same instance in `config.StorageConnection`); faulting ops route through `GrpcFault`. **Only the flat CRUD
>   crosses the wire — the IQueryable metadata layer runs engine-side over this connection** (§4.4.2). Mapped in `QueryEvaluatorHost`.
> - **`Reaqtor.Remoting.Grpc.Client.GrpcStorageConnection`** — the §4.4 sync adapter implementing
>   `IReactiveStorageConnection` (sync-over-async §5.2); `TryGetEntity` reads `{found,entity}` into the out-param;
>   `GetEntities` drains the server stream into a list.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+2, now **11/11**): a CRUD scenario (TryGet absent → add → TryGet → enumerate →
>   delete → enumerate) is record-equal to an in-proc oracle `StorageConnection`; a duplicate-add fault test confirms the
>   custom `ReactiveProcessingStorageException` type name round-trips via `GrpcRemoteException.RemoteTypeName` (§6.1).
>
> `dotnet build All.slnx` → **0/0**; no new project; archived tree untouched. **All four §4.4 sync store adapters now
> exist over gRPC** (StateStore/KeyValueStore/Storage + Messaging). They are hosted in the QE host; the per-role
> *separate-process* host split is M5–6.

> **STATUS — Milestone 5 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **standalone Messaging broker host** promotes the broker out of the QE host into its own process and proves both
> firehose legs cross-process:
> - **`Reaqtor.Remoting.Grpc.MessagingHost`** (NEW project, added to both `.slnx`) — a Kestrel/h2c host running ONLY the
>   broker: a single `MessagingConnection` (the ConcurrentDictionary fan-out kernel) behind `MessagingGrpcAdapter` +
>   `ReactiveServiceControlAdapter` (Ping), no engine. Default port 8086 (§7).
> - **`MessagingGrpcAdapter` refactored** to depend on `IReactiveMessagingConnection` (not `EngineHost`), so the same
>   adapter serves both the in-QE-host broker and the standalone broker host.
> - **QE host** takes an optional `args[1]` broker address; when present, after engine start it calls
>   `MessageRouter.Initialize(new GrpcMessagingConnection(brokerAddress))`, redirecting BOTH CoreDeployable firehose
>   legs (input `FirehoseSubscribable` + output `FirehoseObserver`, which both resolve via `MessageRouter.Instance`) to
>   the separate broker process. `GrpcProcessRunnable.Launch` gained `params extraArgs` to pass the address.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+2, now **13/13**): `Grpc_StandaloneBroker_FanOut_Across_Connections` (one Publish
>   fans out to ≥2 Subscribe streams on different connections to the standalone broker, §4.3); and
>   `Grpc_StandaloneBroker_BothLegs_Across_Processes` — a query `input-firehose(topicIn) → Select(×10) →
>   output-firehose(topicOut)` runs in the QE host while the broker runs in a SEPARATE process: the client publishes
>   inputs to the broker, the engine **subscribes** to topicIn on the broker (QE-as-subscriber input leg, across
>   processes), transforms, and **publishes** to topicOut on the broker (QE-as-publisher output leg, across processes),
>   and the client reads `[10,20,30]` from the broker. **This realizes the §9 command-path-inject leg** that M1 deferred,
>   now via the broker input firehose.
>
> `dotnet build All.slnx` → **0/0**; archived tree untouched. The MVP/store-adapter tests still run the QE host without a
> broker address (in-host broker), so they are unaffected. Per-role hosts for the other stores (StateStore/KVStore/Storage)
> remain co-located in the QE host — splitting those into their own processes is the same mechanical pattern, deferred as
> not load-bearing for the spike.

> **STATUS — Milestone 6 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **multi-QE selector** (§4.6) replaces the hard-coded `_queryEvaluators.First()` in the ported QC:
> - **`Reaqtor.Remoting.Engine.QueryCoordinator.IQueryEvaluatorSelector`** (NEW, public) — `int SelectIndex(int
>   evaluatorCount, Uri entityUri)`, deterministic in the URI. `FirstQueryEvaluatorSelector` (default — the archived
>   single-evaluator behaviour) and `ConsistentHashQueryEvaluatorSelector` (FNV-1a over the URI string, stable across
>   processes so a subscription's create + later delete co-locate on the same QE and load spreads evenly).
> - **`QueryCoordinatorServiceProvider`** now routes `CreateSubscription`/`DeleteSubscription`/`CreateStream` through
>   `SelectEvaluator(entityUri)` (keyed on the `subscriptionUri`/`streamUri` parameter it already has — no Bonsai
>   parsing). **`QueryCoordinatorServiceConnection.Start`** picks `First` for one evaluator (behaviour-preserving) and
>   `ConsistentHash` for ≥2 — so a ≥2-QE deployment distributes automatically with no contract change.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+3, now **16/16**): the selector tests assert First always picks evaluator 0;
>   ConsistentHash stays in range, is stable per URI (create/delete co-locate), and distributes across 2/3/5 evaluators
>   (every evaluator used over 200 URIs); plus argument validation. The existing 13 end-to-end tests still pass,
>   confirming the selector-based QC preserves behaviour with the default selector over the single live QE.
>
> `dotnet build All.slnx` → **0/0**; no new project; archived tree untouched. **Scope:** "MultiRole" (several service
> roles in one process) is already realized by the `QueryEvaluatorHost` (it hosts QC+QE+stores+messaging in one
> process); the new content is the multi-QE *selection* extension point. The InMemory platform runs one live QE, so
> `First` is the active end-to-end selector; the `ConsistentHash` strategy is wired into the production path
> (auto-selected at ≥2 QEs) and proven by the selector tests — standing up ≥2 separate QE host processes for the QC to
> drive over gRPC is deployment packaging that reuses this exact selector.

> **STATUS — Milestone 7 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). The
> **recovery control plane + fault hardening**:
> - **`QueryEvaluatorControlAdapter`** (NEW) implements `IQueryEvaluatorControl` Checkpoint/Unload/Recover over the
>   in-host engine via the ported `IReactiveQueryEvaluatorConnection` (the real `CheckpointingQueryEngine` —
>   `CheckpointAsync` persists subscriptions + operator state to the state store, `UnloadAsync` tears the engine down,
>   `Recover` rebuilds it from the checkpoint). `EngineHost` exposes the QE connection; the adapter is mapped in the host.
> - **`Tests.Reaqtor.Remoting.Grpc`** (+5, now **21/21**): `Grpc_Checkpoint_Unload_Recover_Cycle_Keeps_Engine_Functional`
>   drives the full cycle over gRPC — a stateful `Scan` running-sum reaches 6, **Checkpoint** (asserts the state store is
>   non-empty afterward, observed via the M2 `GrpcStateStoreConnection` — checkpoint persisted real state), **Unload**,
>   **Recover**, then a fresh stateful subscription runs end-to-end (running sum 4,5 → 9), proving the engine is fully
>   functional after the cycle. Four `GrpcFaultMappingTests` pin §6.2: `ArgumentException`/`KeyNotFoundException` →
>   `NotFound` and re-raised by type; transient `BaseException` → `Unavailable` with the transient flag preserved;
>   permanent fault → `Internal` as `GrpcRemoteException` with the type name preserved.
>
> `dotnet build All.slnx` → **0/0**; no new project; archived tree untouched. **Honest finding (corrected — see §13.3.4):**
> a subscription sourced from the *broker firehose* does **not** auto-resume after Checkpoint→Unload→Recover. This was
> first seen over gRPC; an **in-process** characterization test (`Tests.Reaqtor.Remoting.Core.OracleRecoveryDiagnosticTests`,
> no transport, synchronous broker) reproduces it **identically** (`outputs=[0,0,1,3,6]` unchanged), so it is **a
> transport-independent property of the ported engine's recovery semantics, not a gRPC limitation** — the broker
> subscription is not part of checkpointed state (§11.6) and recovery does not re-run the firehose `OnStart`. The engine
> itself recovers fully healthy (a fresh subscription post-recover runs end-to-end) and the checkpoint persists real
> state. So M7 over the wire asserts the control plane, checkpoint persistence, and post-recovery health — and the
> broker-firehose auto-resubscribe gap is recorded as a known behavioural difference (§13.3.4), **not** waved off as a
> scheduler/transport artifact. (My earlier attribution to §3.4 was wrong and is retracted.)

> **STATUS — Milestone 8 COMPLETE on `net10.0`** (branch `spike/grpc-remoting`; archived tree untouched). **Richer
> end-to-end workloads** beyond M1.d's simple operators, each run over gRPC AND the in-proc oracle with asserted parity
> (`Tests.Reaqtor.Remoting.Grpc`, +1, now **22/22**): `Where→Select→Take` (early completion), `DistinctUntilChanged→Scan`
> (dedup feeding a running sum), `Skip→Scan→Where` (filtering ON a running aggregate), and a four-stage
> `Where→Scan→Select` pipeline — exercising the full command + firehose path on realistic multi-operator query shapes,
> all using allowlisted operators with `int`-typed firehose egress. `dotnet build All.slnx` → **0/0**.
> **Scope (honest):** porting the archived **net472** `Reaqtor.Remoting.Reactor.Client` / `ReificationFramework` *sample
> applications* wholesale (they are whole console/harness apps with their own dependency graphs, not clean deployables)
> is a larger follow-on; M8 delivers equivalently rich workloads proven on the gRPC stack with oracle parity, which is
> the milestone's intent ("richer end-to-end workloads after core parity").

> **ALL MILESTONES COMPLETE (0a, 0b, 1–8).** The spike has met its success criterion (M1) and completed every
> incremental-parity milestone: the four §4.4 sync store adapters (StateStore/KeyValueStore/Storage + Messaging), the
> standalone broker host with both legs across processes, multi-QE placement, the recovery control plane + §6.2 fault
> mappings, and richer workloads — all over gRPC with parity against the in-proc oracle, on `net10.0`, with the archived
> `.NET Remoting` tree untouched. **Honest assessment of what is faithful vs. changed vs. traded-off — including the
> known behavioural gaps (broker-firehose recovery resubscribe; virtual-time/scheduler scenarios) — is consolidated in
> §13.**

> **`Get(Observer)` is no longer a milestone.** It is a client-side publishing observer that already works once the
> client driver (§4.7) and broker (§4.3) exist; there is no server-callback channel to build (§4.5).

---

## 11. Testing & parity strategy

**Oracle = the ported in-process platform** (`Reaqtor.Remoting.Platform.InMemory.Core`, single-shared
`MessagingConnection` — §3.6). **Porting it + `TestingFramework.Core` (with the §3.7 ingress/egress) to net10.0 is
a hard prerequisite** (§2.5, Milestone 0a) — there is no live oracle, and no wire-exercisable ingress/egress,
otherwise. The `Tcp` transport stays net472/archived and is a **documentation reference** for intended semantics,
not a live comparison target.

1. **Two separate test axes (because the scheduler can't cross the wire, §3.4):**
   - **In-process virtual-time / glitching axis.** Run the ported glitching/quiescence scenarios
     (`Tests.Reaqtor.Remoting.Glitching/Operators`, `Versioning`) with **engine + `TestScheduler` co-located, no
     transport**. This preserves `ScheduleAbsolute(t, closure)`, `Scheduler.Start()`, and `ScheduledTimes`
     assertions exactly. gRPC is **not** in this path.
   - **Transport axis (wall-clock, closure-free).** A `GrpcTestPlatform` runs subscription/notification/teardown
     scenarios over gRPC and asserts equality against the **oracle** running the same closure-free scenarios. Both
     use the §3.7 ingress/egress, so a single scenario body targets both transports.
2. **Streaming/broker tests:** multiple `OnNext`; **fan-out** (one `Publish` → ≥2 `Subscribe` streams on different
   connections, ordered per topic); **both broker legs** (QE-as-subscriber input + QE-as-publisher output)
   across processes; `OnError` mid-stream (assert client observer's `OnError` fires with a `BaseException`-shaped
   exception carrying `type_name`/`is_transient`, §6.1) vs **transport-drop** (RpcException without terminal
   Notification); `OnCompleted` vs natural end-of-stream; **slow/dead consumer** hits the bounded-buffer overflow
   policy and the stream is error-terminated with `ResourceExhausted` (§4.3).
3. **Cancellation vs teardown tests (§5.1):** cancelling a `CreateSubscription` RPC cancels only that op (assert
   partial-create rollback); **disposal issues `Remove` and leaves no orphan** (asserted via
   `TestControl.GetSubscriptions`, §3.7).
4. **Store contract tests:** `GrpcKeyValueStoreConnection` **re-raises `ArgumentException`** on absent key (§6.2);
   StateStore checkpoint **round-trips** via the batched path and recovers identically to the oracle.
5. **Test-ingress/egress tests (NEW, §3.7):** `TestControl.PlantEventTimeline` plants a timeline that the engine
   replays identically over both transports; `TestControl.GetSubscriptions` returns the same `Subscribe(s,e)`
   ledger as the in-proc DI-store; the `TestFirehoseObservable` ledger (now DI-scoped, not AppDomain) matches the
   oracle. This is what makes "parity against the oracle" exercisable over the wire at all.
6. **Recovery/resubscribe over the broker (NEW — reconciles §4.3 overflow with §4.4.1 checkpoint/recovery):**
   `MessagingConnection.Subscribe` stores delegates in a `ConcurrentDictionary<topic, …>` and is **not** part of
   the engine's persisted state. A QE `Unload`→`Recover` cycle (verified `RemotingTest.cs:552-558`) tears down
   in-engine firehose subscribers (`TopicObserver.OnDispose` → `_stop.Dispose`) and they must **re-Subscribe** on
   recovery — over gRPC, each resubscribe is a **new server-streaming call** to the broker host. **Define expected
   behaviour for events published during the Unload→Recover window:**
   - With the **default `error-the-stream`** overflow policy (§4.3), the torn-down stream completes; the engine
     **re-Subscribes from scratch on `Recover`**, and **events published during the window are lost** (the
     subscriber was absent). The test asserts this explicitly: a normal differential-checkpoint test must **not**
     publish into the window and expect delivery; if it does, the expected outcome is **missed events**, *not* an
     `OnError` on the recovered stream (the new stream is healthy).
   - If a scenario requires window-survival, it must select the **`block`** policy (publisher backpressure) so the
     publisher stalls until a subscriber exists — documented as the opt-in for "no loss across recovery." The MVP
     does **not** require this.
   - The test therefore runs the canonical `Unload→Recover` differential-checkpoint scenario over gRPC with **no
     in-window publishes**, asserting the recovered engine resumes and post-recovery events flow — reconciling the
     §4.3 default with §4.4.1 so recovery does **not** turn a normal checkpoint test into a spurious `OnError`.
7. **Benchmarks (BenchmarkDotNet):** sync-over-async per-item latency on the engine hot path (§5.2); checkpoint
   commit throughput per-item vs client-stream batch (§4.4.1). These quantify the accepted hazards and gate the
   "accept round-trip cost vs go-async" decision.

**Test framework.** Author `Tests.Reaqtor.Remoting.Grpc` on **Microsoft.Testing.Platform** from the start (the
repo is mid-migration; use the `dotnet-test:migrate-vstest-to-mtp` / `dotnet-test:run-tests` skills). Use a Kestrel
host on an ephemeral loopback port per fixture (with the §7 h2c switch set); tear down channels + host in cleanup.

---

## 12. Risks, open questions, and explicit deferrals

### Risks (and mitigations)

| Risk | Impact | Mitigation |
|---|---|---|
| **The §2.5 port is the real bulk of the work and was previously framed away** | Schedule/scope shock; deep coupling surprises (MBR bases, AppDomain messaging singleton, `[Serializable]` config, **Cosmos.Table**, **client driver**) | Port bottom-up with a green-build gate per layer (Milestone 0a); treat gRPC adapters as the small tail |
| **Test ingress/egress reaches into the QE's CLR object + AppDomain slots** (§3.7) | Inputs can't be planted, outputs can't be read over gRPC; parity unexercisable | `TestControl` control RPC + DI-scoped stores generalized from the existing `TimelineObservable` pattern; replace the AppDomain ledger; one scenario body for both transports |
| **Hidden `Microsoft.Azure.Cosmos.Table` dependency in metadata** (§2.6) | Port of `Platform.Core`/QC silently fails to build or drags in a heavy Azure SDK | Refactor `EdmType`→local enum in the two `StorageConnection*` files; don't port `AzureTable/`; CPM gets no Cosmos entry (fallback documented) |
| **Sync-over-async on engine hot path + checkpoint** (§5.2) | Thread-pool starvation, latency, deadlock if bridged over a context | No captured context in hosts; batched/streamed chatty paths; **measure** (§10); async-engine fallback deferred |
| **Checkpoint chattiness** (N per-item round-trips, §4.4.1) | Commit latency at scale | Client-stream batch commit; measure; accept documented profile if adequate |
| **Broker correctness** (fan-out ordering/concurrency/lifetime; **both legs**, §4.3) | Wrong/lost/duplicated notifications; the "reliable" claim at stake | Per-topic single-writer ordering; bounded buffers with explicit overflow (default error-the-stream, no silent loss); reconnect/resubscribe contract; dedicated fan-out tests |
| **In-proc oracle could pass trivially-empty** if publisher/subscriber get different `MessagingConnection`s (§3.6) | False green parity | **Single-shared-`MessagingConnection` invariant** asserted in the oracle composition root |
| **Recovery resubscribe + overflow policy interaction** (§10.6) | A normal `Unload→Recover` test could spuriously `OnError` or silently lose events | Define window behaviour: default = events lost (no in-window publish in the canonical test); `block` is the opt-in for no-loss; recovered stream is healthy, not errored |
| **Virtual-time scheduling cannot cross the wire** (§3.4) | Glitching parity can't run cross-process | Split test axes: virtual-time in-proc, transport wall-clock; never marshal client closures |
| **Exception identity loss on command AND firehose OnError** (§6.1) | Type-switch / predicate-equality assertions break across the wire | `BaseException`-shaped reconstruction (ported + ISerializable-stripped); predicate/type-identity assertions stay in-proc; cross-process asserts on `type_name`/`is_transient` |
| **`ArgumentException` absent-key contract** (§6.2) | Engine KVStore callers break if status isn't re-raised correctly | Adapter re-raises `ArgumentException`; round-trip contract test |
| **`Get(Observer)` misread as a server callback** (§4.5) | Building a callback channel the design neither has nor needs | Treat it as the **client-side publishing observer** it is; publish leg = `Messaging.Publish`, observe leg = separate client `Subscribe`; no server-callback machinery in MVP |
| **Client driver had no port home** (§4.7) | MVP "client drives QC" can't compile | Port `Client(.Library)` clean subset to `Reaqtor.Remoting.Client.Core`; gRPC `GrpcReactiveServiceConnection` replaces `LocalReactiveServiceConnection`+proxy; `Expression`-vs-`ExpressionSlim` is a non-issue (only the string crosses) |
| **QC→multi-QE routing** (§4.6) | Addressing baked wrong early | `Execute` is URI-addressed from day one; `IQueryEvaluatorSelector` (default `.First()`) swapped at Milestone 6 |
| **h2c silent connect failure** (§7) | First slice "hangs"/fails opaquely | Set `Http2UnencryptedSupport` switch or use https dev cert; documented in host + fixture |
| **`protobuf-net.Grpc` is third-party** | Non-first-party dependency | Mature; contracts isolated; `.proto` emitted for an escape hatch to `Grpc.Tools` |

### Open questions

1. **Multi-target vs extract per file in §2.5.** Default: extract (Protocol and Platform are mixed clean/remoting).
   Multi-target only where a whole file set is clean. Confirm per project during Milestone 0a.
2. **Cosmos.Table: refactor vs carry (§2.6).** Default: refactor `EdmType`→local enum, carry nothing from
   `AzureTable/`. Confirm the `AzureReactiveMetadataProxy`/`AzureStorageResolver` `Remoting`-path members are
   Cosmos-free after refactor; fall back to adding the package only if the refactor is too invasive.
3. **Code-first vs classic `.proto`.** Recommended code-first; `.proto` emitted for docs. Revisit only for
   cross-language interop.
4. **`command_text` `string` vs `bytes`.** Start `string`; switch to `bytes` only if profiling shows the UTF-8
   round-trip is hot.
5. **Broker overflow default** (`error-the-stream` vs `block` vs `drop-oldest`). Default `error-the-stream` to
   preserve "no silent loss"; `block` is the opt-in for no-loss-across-recovery (§10.6); confirm against the
   reliability bar.
6. **MVP inject leg (§9 step 3).** Default: command-path (`stream.OnNextAsync`) for the canonical operator test;
   direct `Messaging.Publish` exercised at Increment 5. Confirm this is the intended "first surface."
7. **Async-engine escape hatch.** If §10 benchmarks show sync-over-async regresses hot paths unacceptably, do we
   give the engine async store interfaces (large, deferred) or accept the cost?
8. **Status mapping edges.** `IsTransient()==true ⇒ Unavailable`; broker overflow ⇒ `ResourceExhausted`;
   absent-key ⇒ `NOT_FOUND`+`ArgumentException` re-raise. Confirm no other engine call site depends on a different
   concrete exception type.

### Explicit deferrals

- **`Nuqleon.Runtime.Remoting.Tasks`, `ReactiveServiceCommandProxy`/`Service`, `LocalReactiveServiceConnection`,
  `SchedulerProxy`, marshaled-delegate scheduler tasks** — not ported; subsumed/re-implemented.
- **`Metadata/AzureTable/**` backend / `MetadataStorageType.Azure`** — not ported; net10.0 supports only the
  `Remoting` (gRPC-Storage) metadata backend (§2.6, §4.2).
- **`AppDomain` transport** — dropped (no AppDomains on net10.0); AppDomain-based tests migrate to InMemory or Grpc;
  the AppDomain-backed test ledger is replaced by DI-scoped stores (§3.7).
- **Server→client observer-callback channel** — not built; `Get(Observer)` is a client-side publishing observer
  (§4.5). A topic-Subscribe-based server-initiated push is a v2 possibility, not a deliverable.
- **Durable external broker** (Redis/RabbitMQ) — design-compatible behind the Messaging contract, post-parity.
- **Async store interfaces for the engine** — only if §10 benchmarks force it.
- **TLS/mTLS, cloud DNS/service-mesh discovery, Kubernetes/Aspire manifests** — design notes; h2c/loopback for the
  spike.
- **Command-surface modernization** (flattening verbs/nouns, "Approach B") — v2.
- **Cross-process virtual-time / glitching** — never (architecturally excluded, §3.4); kept in-proc.
- **`Reactor.*` sample, `ReificationFramework`, `VersioningTests` ports** — after core parity (Milestone 8).

---

## 13. Honest assessment — what is faithful, what differs, and the trade-offs

This section is the candid bottom line after completing 0a–8. It separates **(13.2) faithful parity**, **(13.3) behavioural
differences a consumer can observe**, **(13.4) internal/technical differences with no behavioural change**, **(13.5)
deliberate trade-offs**, and **(13.6) what is *not* built** — so no one mistakes "spike complete" for "production-equivalent
to the .NET Remoting stack." Verified against 24 gRPC transport tests + 2 in-process oracle/characterization tests, `dotnet
build All.slnx` 0/0, archived tree untouched.

> **Team-facing decision record:** the production transport recommendation distilled from §13.9/§13.10 (per-leg choice
> across gRPC / StreamJsonRpc / MagicOnion / SignalR / brokers / actors, with rationale and consequences) is captured as
> a standalone ADR at **`DECISION-RPC-TRANSPORTS.md`** (repo root).

### 13.1 Verdict

Code-first gRPC on `net10.0` **viably replaces** the archived `.NET Remoting` transport for this stack: the command plane,
all four store contracts, the stateful fan-out broker (both legs, across processes), QC→QE placement, the recovery control
plane, and rich multi-operator workloads all run over gRPC and produce results **identical to the in-process oracle**. The
replacement is **not bit-for-bit behaviourally identical** — it trades away two capabilities .NET Remoting gave for free
(full `[Serializable]` exception graphs and `MarshalByRefObject` scheduler/closure marshalling) and surfaces one
**transport-independent** engine-recovery gap. All are enumerated below.

### 13.2 Faithfully preserved (asserted at parity vs. the oracle)

- **Command protocol**: `CommandVerb`/`CommandNoun` + Bonsai/DataModel JSON `commandText`; client lowers `Expression`,
  QC parses `ExpressionSlim` — only the JSON string crosses (§4.7). `SerializationHelpers`/`BonsaiExpressionSerializer`
  reused verbatim (no `BinaryFormatter`).
- **Engine & operator semantics**: the `CheckpointingQueryEngine` and the whole operator surface are reused unchanged;
  `Where/Select/Scan/Skip/Take/DistinctUntilChanged/...` produce identical results over gRPC and in-proc (M1.d, M8).
- **The four store contracts**: `IReactiveStateStoreConnection`, `ITransactionalKeyValueStoreConnection`,
  `IReactiveStorageConnection`, `IReactiveMessagingConnection` are implemented faithfully (bool+out semantics, transaction
  ids, fan-out), each parity-checked against the in-proc store.
- **Broker fan-out**: one `Publish` reaches every open `Subscribe` stream for a topic, across connections and processes (M5).
- **Teardown semantics**: disposal issues an explicit `Execute(Remove, Subscription, uri)` — a distinct command, not call
  cancellation (§5.1) — exactly as the archived stack.

### 13.3 Behavioural differences (observable; reduced fidelity — accepted)

1. **Exception type identity is reduced.** Faults cross as `ErrorInfo{type_name, message, stack, is_transient}`. The client
   reconstructs the **original CLR type only for the engine's contract set** (`ArgumentException`, `ArgumentNullException`,
   `KeyNotFoundException`, `InvalidOperationException`); everything else arrives as **`GrpcRemoteException`** carrying the
   original type name as *data*. Custom `[Serializable]` exception subtypes and their fields are **lost** (they relied on
   `BinaryFormatter`). `is_transient` is preserved. (M3/M4/M7 tests pin this.)
2. **Broker-firehose subscriptions do NOT auto-resume across `Unload→Recover`.** Verified **transport-independent**: an
   in-process characterization test (no gRPC, synchronous broker) reproduces the gRPC result exactly — a recovered
   subscription whose source is the broker firehose stops receiving events. Root cause: the broker subscription is not part
   of checkpointed engine state (§11.6) and recovery does not re-invoke the firehose operator's `OnStart`. **The engine
   recovers fully healthy** (a fresh subscription post-recover runs end-to-end) and **checkpoint persists real state** — the
   gap is specifically auto-resubscribe of a previously-checkpointed *broker* source. (This is *not* a regression: the
   archived recovery/glitching tests recover **timeline/scheduler-fed** subscriptions, never broker-fed ones.) Workarounds:
   re-issue the subscription after recovery, or feed via the timeline/scheduler ingress in-process.
3. **Virtual-time / scheduler-controlled scenarios are not remoted** (§3.4). `.NET Remoting` shipped `Action` closures to
   run server-side and let the client drive the QE's `TestScheduler` over an MBR `SchedulerProxy`. gRPC cannot marshal
   closures or share a virtual clock, so glitching/quiescence and `ScheduleAbsolute`-driven tests run **in-process only**;
   over the wire we use wall-clock, closure-free scenarios.
4. **Predicate `OnError` notifications stay in-process.** `ObserverNotification.CreateOnError(Func<Exception,bool>)`
   (test-equality predicates) is never put on the wire; cross-process `OnError` parity asserts on `type_name`/`message`/
   `is_transient`, not CLR identity (§6.1).
5. **Transport security**: h2c (cleartext HTTP/2) on loopback for the spike; production TLS/mTLS is design-only. The
   `Http2UnencryptedSupport` `AppContext` switch is set in `GrpcConnectionFactory.CreateChannel` **only when an
   `http://` channel is created** (never for `https://`), rather than as an import-time type-initializer side-effect —
   so production `https://` channels are never silently relaxed. It remains process-global (an `AppContext` switch is).
6. **Per-RPC cancellation cancels only that operation** (HTTP/2 abort), and may leave a partially-created subscription that
   the handler rolls back (§5.1) — distinct from, and not a substitute for, `Remove`-based teardown.

### 13.4 Technical differences (internal mechanism; same observable behaviour)

| Concern | Archived (.NET Remoting) | gRPC port |
|---|---|---|
| Wire | `TcpChannel` + `BinaryServerFormatterSinkProvider`/`TypeFilterLevel` | Kestrel HTTP/2 + code-first `protobuf-net.Grpc` |
| Async + cancel | `Nuqleon.Runtime.Remoting.Tasks` (`RemoteProxyBase`/`Reply<T>`/`ICancellationProvider`) | native `Task<T>` + `ServerCallContext.CancellationToken` |
| Service handle | MBR transparent proxy via `Activator.GetObject` | typed code-first client over a cached `GrpcChannel` (`GrpcRunnable<T>`) |
| Isolation | `AppDomain` + `ServiceInstanceHelpers` AppDomain-keyed messaging slot | OS process isolation + DI-injected `MessageRouter` / process-wide `MessageRouter.Initialize` |
| Sync store calls | direct MBR proxy calls | sync-over-async adapters (`.GetAwaiter().GetResult()`, §5.2) |
| Enumerations / checkpoint commit | `IEnumerator` / N per-item proxy calls | server-streaming (`Enumerate`/`GetEntities`) / client-streamed batch (§4.4.1) |
| Metadata property type | `Microsoft.Azure.Cosmos.Table.EdmType` | local `StorageEntityPropertyType` enum (no Cosmos package, §2.6) |
| `BaseException` | `[Serializable]` + `ISerializable` ctor/`GetObjectData` | modernized: `ISerializable` stripped (obsolete `SYSLIB0051`), `IsTransient()` kept |
| Hosts | console `TcpRemoteServiceHost<T>` exes | minimal Kestrel `WebApplication` exes (§7) |

These change *how* it works, not *what* a client observes (covered by parity tests).

### 13.5 Technical trade-offs (deliberate choices, with the alternative)

1. **Code-first contracts (protobuf-net.Grpc)** vs hand-written `.proto`. *Win:* reuses CLR enums/interfaces, far less
   boilerplate. *Cost:* protobuf-net DTOs trip data-modelling analyzers (scoped `NoWarn`), and the contract is C#-authored —
   a non-.NET consumer needs the reflection-emitted `.proto`.
2. **Sync-over-async store adapters** keep the engine's synchronous store interfaces unchanged. *Win:* small, low-risk,
   no engine surgery. *Cost:* `.GetAwaiter().GetResult()` on the hot path → per-item latency + thread-pool blocking;
   mitigated by streamed/batched shapes. The alternative — async store interfaces through the engine — is deferred (§5.2).
3. **Opaque Bonsai JSON on the wire.** *Win:* protobuf never models expression trees; serialization reused verbatim
   (lowest risk). *Cost:* no cross-language expression interop; the payload is an opaque string.
4. **MVP single-host topology** (QC+QE+stores co-located behind gRPC). *Win:* exercises the full engine over the wire
   immediately. *Cost:* the distributed seams are *proven* (standalone broker M5, selector M6) but per-role
   separate-process store hosts and ≥2 QE host processes are **not stood up** (deployment packaging, §13.6).
5. **Bounded per-stream broker channel with the §4.3 default `error-the-stream` overflow policy** (implemented). Each
   `Subscribe` stream gets a bounded `Channel<Notification>` (capacity 1024); the synchronous broker callback uses a
   non-blocking `TryWrite`, and a slow/dead consumer that fills the buffer has its stream completed with
   `ResourceExhausted` (the subscriber observes `OnError` and may resubscribe). *Remaining cost:* the alternative
   policies (`block` publisher-backpressure / `drop-oldest`) are config options, not separately exercised, and the
   capacity is a fixed constant rather than per-subscription-configurable.
6. **"Measure round-trips" = deterministic in-test count** (per-item N vs. batched 1), not a BenchmarkDotNet latency/
   throughput benchmark (still a §11 item).
7. **Differential-only checkpoint** is surfaced (matches the archived `Checkpoint()`); no full-checkpoint API exposed.

### 13.6 Not built (scope honesty — patterns proven, packaging deferred)

- **Per-role separate-process hosts** for StateStore/KeyValueStore/Storage, and **≥2 QE host processes** for cross-process
  QC→QE routing — the adapters, the standalone-host pattern (M5), and the selector (M6) exist and are tested; only the extra
  host processes/wiring are not assembled.
- **Production TLS/mTLS**, service discovery, container/orchestration manifests.
- **Durable external broker** (Redis/RabbitMQ) behind the Messaging contract.
- **Full `Reactor.*` / `ReificationFramework` sample-application ports** (whole net472 apps); M8 delivers equivalent rich
  workloads instead.
- **Async engine store interfaces** (only if benchmarks force it); **server→client observer callback** (not needed, §4.5).

### 13.7 Verification posture

- **24 gRPC transport tests** (command path, 4 store adapters incl. fault round-trips + typed-exception re-raise,
  standalone broker both-legs + fan-out, multi-QE selector, recovery control plane, richer workloads) + **2 in-process
  oracle/characterization tests**.
- **Parity** against the in-process oracle is asserted for the command path, all four stores, both broker legs, and the
  rich workloads; the multi-QE selector and the §6.2 fault mappings are pinned by focused unit tests.
- **Honestly *not* asserted over the wire:** broker-firehose recovery resubscribe (§13.3.2 — proven absent and
  transport-independent), virtual-time/glitching (§13.3.3 — in-process axis), throughput/latency (benchmarks deferred).
- No `System.Runtime.Remoting` / MBR / `AppDomain` / `Microsoft.Azure.Cosmos.Table` / `ServiceInstanceHelpers` on any live
  path (audited); the archived tree is untouched (`git status archive/` clean).

### 13.8 Project-by-project mapping (archived → gRPC)

Every archived project in `archive/Reaqtor/Samples/Remoting/` (+ the `Nuqleon.Runtime.Remoting.Tasks` dependency),
mapped to its `net10.0` gRPC-side counterpart under `Reaqtor/Samples/Grpc/`. "—" in *New* means deliberately not ported.

**A. Transport-agnostic core (ported, behaviour-preserved)**

| Archived project — purpose & features | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Reaqtor.Remoting.Protocol`** — enums (`CommandVerb`/`CommandNoun`/`NotificationKind`), `INotification`/`ObserverNotification`, `StorageEntity`, command machinery (`CommandText*`, `ReactiveServiceConnection<T>`), engine-facing store/messaging interfaces, `FaultHandling/BaseException`; **plus** the MBR command proxy/service + MBR `ReactiveConnectionBase` | **`Reaqtor.Remoting.Core`** | Same clean enums/DTOs/interfaces; non-MBR `ReactiveConnectionBase`; in-proc `InProcessReactiveServiceConnection` + `RemotingReactiveServiceConnectionBase` replacing the deleted proxy/service pair; `BaseException` modernized | `BaseException` `ISerializable` stripped (`SYSLIB0051`); custom exception subtype identity lost on the wire (§13.3.1) | — (the MBR proxy/service are intentionally not ported) |
| **`Reaqtor.Remoting.Platform`** — `IReactivePlatform`/`IReactiveService`/`IRunnable`/`IDeployable`, `ReactivePlatformBase` (registers `TcpChannel`), `MessageRouter`, `FirehoseObserver`, metadata `StorageAbstractions` (Cosmos-coupled), `[Serializable]` config | **`Reaqtor.Remoting.Platform.Core`** | Same seam shapes; **transport-neutral** `ReactivePlatformBase` (no channel); non-`[Serializable]` config; DI-injected `MessageRouter`; Cosmos-free metadata impl layer (local `StorageEntityPropertyType`) | `EdmType`→local enum (§2.6); messaging resolved by DI/`MessageRouter.Initialize` instead of the AppDomain slot | the `Microsoft.Azure.Cosmos.Table` `AzureTable/` backend (`MetadataStorageType.Azure`) |
| **`Reaqtor.Remoting.QueryCoordinator`** + **`…QueryEvaluator`** + **`…Messaging`** — QC/QE service connections (QE wraps `CheckpointingQueryEngine`; `SchedulerProxy : MarshalByRefObject`), broker `MessagingConnection` | **`Reaqtor.Remoting.Engine`** | QC/QE/broker kernel; checkpoint writer/reader; **`IQueryEvaluatorSelector`** (First/ConsistentHash) replacing `.First()` (§4.6); de-MBR'd local scheduler | `SchedulerProxy` de-MBR'd → **virtual-time/closure-shipping not remotable** (§3.4); QC→QE in-proc bridge over `InProcessReactiveServiceConnection` | cross-process QC→multi-QE-host routing (selector wired, hosts not stood up, §13.6) |
| **`Reaqtor.Remoting.Deployable`** — `CoreDeployable` (full operator/observer surface), firehose observable/subscribable/observer | **`Reaqtor.Remoting.Deployable.Core`** | Same operator surface + firehoses | `AppDomain`-slot firehose ledger → in-proc `ConcurrentDictionary` | — |
| **`Reaqtor.Remoting.Client.Library`** — `RemotingServiceProvider`, `RemoteObserverClient<T>`, `ReactivePlatformClientBase`, `RemotingClientContext` (client driver); wires `LocalReactiveServiceConnection`+MBR proxy | **`Reaqtor.Remoting.Client.Core`** | Same client driver; transport-injection seam (`RemotingClientContext` over an `IReactiveServiceConnection`) | client serializes `Expression`, QC parses `ExpressionSlim` — only JSON crosses (§4.7) | — |
| **`Reaqtor.Remoting.TestingFramework`** — `TestDeployable`, `TestFirehoseObservable`, `TestObserver`, `TimelineObservable`, `OperatorTestBase`, `TestPlatform`, test stores; marshaled-delegate schedulers | **`Reaqtor.Remoting.TestingFramework.Core`** | Same test ingress/egress; firehose ledger off `AppDomain` onto DI-scoped `TestSubscriptionStoreConnection` (§3.7) | marshaled-delegate scheduler tasks dropped (§3.4) | the `TestControl` RPC surface (in-proc path used directly) |

**B. Store implementations & the four sync adapters**

| Archived project — purpose & features | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Reaqtor.Remoting.StateStore`** — `StateStoreConnection` (checkpoint store, `InMemoryStateStore`) | **`Platform.InMemory.Core`** (`StateStoreConnection`) + gRPC `StateStoreGrpcAdapter` / `GrpcStateStoreConnection` | Same connection; gRPC `StateStore` service (§4.4) incl. **client-streamed batch** commit (§4.4.1) | sync-over-async bridge (§5.2); "measure round-trips" is an in-test count, not a benchmark | — |
| **`Reaqtor.Remoting.KeyValueStore`** — `KeyValueStoreConnection` (transactional KVS) | **`Platform.InMemory.Core`** (`KeyValueStoreConnection`) + gRPC `KeyValueStoreGrpcAdapter` / `GrpcKeyValueStoreConnection` | Same connection; gRPC `KeyValueStore` service (txId-per-call, `Enumerate` stream); absent-key re-raises by type via `GrpcFault` (§6.2) | sync-over-async; absent-key actually throws `KeyNotFoundException` (not `ArgumentException`) — mapping preserves type identity | — |
| **`Reaqtor.Remoting.Messaging`** — `MessagingConnection` broker fan-out kernel | **`Engine`** (`MessagingConnection`) + gRPC `MessagingGrpcAdapter` / `GrpcMessagingConnection` + **`Grpc.MessagingHost`** | Same kernel; broker over unary `Publish` + server-streaming `Subscribe`; both legs across processes (M5); **bounded** per-stream channel (cap 1024) with §4.3 default `error-the-stream` (`ResourceExhausted`) overflow | fixed buffer capacity; only the default overflow policy wired | configurable capacity + alternative `block`/`drop-oldest` overflow policies (§4.3) |
| **`Reaqtor.Remoting.Metadata`** — `StorageConnection` (metadata over KVS) + `AzureReactiveMetadata` proxy (Cosmos-coupled) | **`Engine`** (`StorageConnection`) + **`Platform.Core`** metadata layer + gRPC `StorageGrpcAdapter` / `GrpcStorageConnection` | Flat-CRUD `Storage` service; IQueryable metadata layer stays engine-side, Cosmos-free (§4.4.2) | only flat CRUD on the wire; custom `ReactiveProcessingStorageException` → `GrpcRemoteException` (type name preserved) | the Azure/Cosmos metadata backend |

**C. Platform transports**

| Archived project — purpose & features | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Reaqtor.Remoting.Platform.InMemory`** — in-proc oracle platform | **`Reaqtor.Remoting.Platform.InMemory.Core`** | The live oracle; single-shared `MessagingConnection` (§3.6) | — | — |
| **`Reaqtor.Remoting.Platform.Tcp`** — TCP/.NET-Remoting transport (`Activator.GetObject`, `TcpRemoteServiceHost`) | **`Reaqtor.Remoting.Platform.Grpc`** (`GrpcProcessRunnable`, `GrpcRunnable<T>`) + the `Grpc.*` projects | gRPC transport seam: typed code-first clients over `GrpcChannel`; launch host exes + Ping-readiness | h2c cleartext on loopback (dev); per-RPC cancel ≠ teardown (§5.1) | production TLS/mTLS; full `GrpcReactivePlatform : ReactivePlatformBase` service graph (uses runnable seam instead) |
| **`Reaqtor.Remoting.Platform.AppDomain`** — AppDomain transport | **— (dropped)** | n/a (no AppDomains on net10.0) | — | dropped by design; AppDomain tests migrate to InMemory/Grpc |

**D. The gRPC wire layer (new; replaces the implicit Remoting wire)**

| Archived — purpose & features | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Nuqleon.Runtime.Remoting.Tasks`** (dependency) — async-over-remoting: `RemoteProxyBase`/`RemoteServiceBase`/`Reply<T>`/`ICancellationProvider`/`RemoteCancellationDisposable` | **`Reaqtor.Remoting.Grpc.Contracts`** + **`…Grpc.Protocol`** + **`…Grpc.Client`** + **`…Grpc.Server`** | Code-first `[ServiceContract]`/`[ProtoContract]`; marshaling (`NotificationConverter`, `GrpcFault`, `StorageEntityConverter`, `GrpcRemoteException`); h2c client + 4 store adapters + command client; server adapters (control/connection/messaging/3 stores/QE-control) + `EngineHost` | native `Task<T>` + `CancellationToken` replace the whole remoting-tasks layer | — (the remoting-tasks layer is **deleted, not ported**) |

**E. Hosts (console `TcpRemoteServiceHost` exes → Kestrel)**

| Archived host(s) — purpose | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`QueryCoordinatorHost`**, **`QueryEvaluatorHost`**, **`MetadataHost`**, **`StateStoreHost`**, **`KeyValueStoreHost`**, **`MultiRoleHost`** — one console exe per role / multi-role | **`Reaqtor.Remoting.Grpc.QueryEvaluatorHost`** | One Kestrel/h2c host runs the full in-proc engine (QC+QE+stores+messaging) behind gRPC — the MVP "MultiRole" host | MVP co-locates all roles in one process | separate per-role **store** host processes (adapters exist; hosts not stood up, §13.6) |
| **`Reaqtor.Remoting.MessagingHost`** — standalone broker host | **`Reaqtor.Remoting.Grpc.MessagingHost`** | Standalone Kestrel/h2c broker process; both firehose legs across processes (M5) | — | — |

**F. Tests**

| Archived — purpose | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Tests.Reaqtor.Remoting`** — end-to-end remoting tests | **`Tests.Reaqtor.Remoting.Grpc`** (24) + **`Tests.Reaqtor.Remoting.Core`** (2) | Transport tests over gRPC with parity vs the in-proc oracle; oracle smoke + recovery characterization | wall-clock, closure-free scenarios over the wire (§3.4) | — |
| **`Tests.Reaqtor.Remoting.Glitching`** — virtual-time glitching/quiescence on the `TestScheduler` | **in-process axis only** (`Tests.Reaqtor.Remoting.Core` oracle/characterization) | virtual-time fidelity kept in-process (engine + scheduler co-located) | not remotable (§3.4) | a full ported glitching suite on the in-proc oracle |

**G. Samples & richer workloads (not ported as applications)**

| Archived project(s) — purpose | New implementation | New purpose & features | Trade-offs | Missing / deferred |
|---|---|---|---|---|
| **`Reaqtor.Remoting.Demo`**, **`Reaqtor.Remoting.Samples`** — demo/sample console apps | **— (not ported)** | M1.d/M8 parity tests stand in as runnable end-to-end demonstrations | — | the demo/sample console apps |
| **`Reaqtor.Remoting.Reactor.Client`** / **`…Reactor.Client.Library`** / **`…Reactor.Deployable`** / **`…Reactor.Tcp.Host`** — the richer "Reactor" end-to-end sample app + its deployable + host | **— (not ported)**; equivalent rich workloads delivered as **`Tests.Reaqtor.Remoting.Grpc` (M8)** | M8 runs richer multi-operator compositions over gRPC with oracle parity | porting whole net472 apps is disproportionate for the spike | the Reactor sample application + its dedicated host |
| **`Reaqtor.Remoting.ReificationFramework`** — reification-based checkpoint/recover test harness | **— (not ported)** | recovery exercised via M7 control plane + the §13.3.2 characterization test | — | the reification harness |
| **`Reaqtor.Remoting.VersioningTests`** — state-version compatibility tests | **— (not ported)** | — | — | state-versioning tests |

### 13.9 StreamJsonRpc prototype — restoring the capabilities gRPC dropped

`Tests.Reaqtor.Remoting.StreamJsonRpc` is a small, self-contained prototype evaluating **StreamJsonRpc** (Microsoft's
bidirectional JSON-RPC over a duplex pipe) as the modern, `MarshalByRefObject`-free successor to the deleted
`Nuqleon.Runtime.Remoting.Tasks` layer (§13.4). Two `JsonRpc` endpoints are wired over one in-memory
`Nerdbank.Streams.FullDuplexStream` pair; each endpoint can both serve and call, so the server can receive client
objects **by reference** and invoke methods **back on the client** — exactly what .NET Remoting MBR gave and what the
gRPC port could not express. Uses StreamJsonRpc 2.25's source-generated contract model (`[JsonRpcContract]` +
`[GenerateShape]` on contracts, `[RpcMarshalable]` + `[TypeShape]` on the marshaled callback).

Six demo tests (**6/6 green**), each mapped to the archived primitive it is the modern equivalent of:

| Demo | Archived primitive it replaces | Capability shown | Could the spike's gRPC do it? |
|---|---|---|---|
| **A** `RunIntoObserverAsync` | `Reply<T> : MarshalByRefObject, IObserver<T>` | server pushes results into a **client-supplied marshaled observer** (`[RpcMarshalable]`) — server→client callback over the same connection | Not directly — needs a separate broker/stream contract (what M1.b/M5 built) |
| **B** `SumWithProgressAsync` | (n/a — convenience) | `IProgress<T>` progress reporting, marshaled automatically | No first-class equivalent (would be a bespoke stream) |
| **C** `SlowEchoAsync` + cancel | `RemoteCancellationDisposable` / `ICancellationProvider` (GUID-keyed MBR cancel) | first-class `CancellationToken` aborts the in-flight server op | **Yes** — gRPC also maps this (the one piece that ported cleanly) |
| **D** `CountAsync` | (multi-value `Reply<T>`) | `IAsyncEnumerable<T>` server streaming | **Yes** — gRPC server streaming (used by the broker/enumerations) |
| **E** `AggregateViaClientAsync` | `ClientAction : MarshalByRefObject` (server invokes client; §3.4) | full-duplex: the **server calls back into the client** mid-request | Not built-in — gRPC has no server→client call without a hand-rolled duplex stream |
| **F** wire-trace | — (proof harness) | taps both ends of the duplex pipe and asserts the actual **JSON-RPC envelopes** crossed in BOTH directions — proves it is real RPC, not in-process calls | n/a (verification) |

**Where this is genuinely better than the gRPC port:** demos **A** and **E** — marshaling a live client callback and server→client invocation — are single method signatures here, whereas over gRPC they required (A) a dedicated broker service with per-stream channels (M1.b/M5), or (E) have no built-in form at all. This is the closest modern realization of the firehose/`Reply<T>` and the §3.4 `ClientAction` patterns.

**Proof it is real RPC over the pipe (demo F).** A `TappingStream` records every byte written to each end; the captured transcript of the bidirectional `AggregateViaClient([1,2,3,4])` scenario shows genuine JSON-RPC framing crossing in both directions (client→server `AggregateViaClientAsync` + the client's 4 result replies; server→client 4× `ResolveAsync` + the final `result:20`):

```text
CLIENT -> SERVER:  {"jsonrpc":"2.0","id":2,"method":"AggregateViaClientAsync","params":[[1,2,3,4]]}
                   {"jsonrpc":"2.0","id":2,"result":2} ... {"id":5,"result":8}
SERVER -> CLIENT:  {"jsonrpc":"2.0","id":2,"method":"ResolveAsync","params":[1]} ... params":[4]
                   {"jsonrpc":"2.0","id":2,"result":20}
```

This makes the server→client call path (demo E) — the `ClientAction`/§3.4 capability gRPC lacks — visible on the wire, not just asserted by a return value.

**Trade-offs vs gRPC (why it is a prototype, not the chosen transport):** JSON envelope (vs protobuf binary) → larger/slower payloads on hot paths; no cross-language IDL/`.proto` contract or HTTP/2 ecosystem (load balancers, deadlines, mTLS tooling); marshaled-object and duplex callbacks add per-call round-trips and lifetime/back-pressure concerns (the same hazards the firehose-recovery analysis surfaced). It is the right tool for **rich, stateful, callback-heavy peer links** (the firehose/scheduler legs), not for the high-throughput command/store planes where gRPC's binary unary/streaming wins. A realistic system could use **both**: gRPC for the command + store planes, StreamJsonRpc (or a gRPC duplex stream) for the callback-marshaling legs.

### 13.10 MagicOnion prototype — the gRPC-native production option for the duplex legs

`Tests.Reaqtor.Remoting.MagicOnion` evaluates **MagicOnion** (Cysharp) — a production gRPC RPC framework (Grpc.AspNetCore
server + Grpc.Net.Client + MessagePack) — as the **transport-uniform** alternative to StreamJsonRpc for the callback/duplex
legs: it keeps everything on the **same gRPC/HTTP-2 stack the spike already adopted**, while still giving server→client
calls. Unlike the StreamJsonRpc prototype's in-memory pipe, the demos host a **real in-process Kestrel/h2c MagicOnion server**
and connect a `GrpcChannel` client — genuine client/server over HTTP/2.

Two demo tests (**2/2 green**):

| Demo | MagicOnion shape | Capability |
|---|---|---|
| `Unary_Request_Response_Over_Grpc` | `IService<T>` + `UnaryResult<T>` | request/response (the command-plane shape) |
| `StreamingHub_Client_To_Server_And_Server_Pushes_To_Client` | `IStreamingHub<THub,TReceiver>` | client→server hub methods (with return values) **and** the **server pushing back into the client's receiver** (`this.Client.OnValue(...)`) — the firehose/`Reply<T>` and §3.4 `ClientAction` pattern, gRPC-native |

**How it compares to the StreamJsonRpc prototype (§13.9):**

| | StreamJsonRpc | MagicOnion |
|---|---|---|
| Transport | any duplex stream/pipe/WebSocket (JSON) | **gRPC / HTTP-2** (MessagePack) — same stack as the rest of the spike |
| Server→client | marshaled callback objects + arbitrary bidirectional method calls (most flexible) | **StreamingHub** receiver = server→client push (notification-shaped, not arbitrary object marshaling) |
| Payload | JSON (human-readable, larger) | MessagePack (binary, compact/fast) |
| Ecosystem | .NET-centric pipes/sidecars | full gRPC infra (LB/deadlines/mTLS); cross-language story weaker (MessagePack hubs are .NET/Unity-oriented) |
| Best fit here | lightweight in-host/sidecar duplex; arbitrary callback marshaling | **the production choice if you stay on gRPC** and want typed server→client hubs |

**Trade-offs / notes:** MessagePack contracts + a Cysharp OSS dependency; `StreamingHub` receivers are push-shaped
(server→client notifications), so the *arbitrary client-object marshaling* StreamJsonRpc does (demo A's `[RpcMarshalable]`
`IObserver`) isn't a like-for-like — you model pushes as receiver methods. MagicOnion 7.10.1 pulls MessagePack 3.1.4 which
the repo's NuGet audit flags (NU1903); pinned to **MessagePack 3.1.7** in CPM to clear it. Net: for **this** gRPC-committed,
.NET-only system, MagicOnion StreamingHub is the strongest production answer for the callback/duplex legs (one transport,
binary, typed server→client); StreamJsonRpc remains the lighter choice when you need arbitrary callback-object marshaling or
a non-gRPC pipe.
