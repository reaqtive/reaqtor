# Decision record: RPC transports for the distributed Reaqtor sample

- **Status:** Proposed — recommendation from the gRPC remoting spike (`spike/grpc-remoting`), pending team ratification for production.
- **Date:** 2026-06-14
- **Supersedes:** the archived `.NET Remoting` transport (`archive/Reaqtor/Samples/Remoting/**`, net472).
- **Related:** `GRPC-REMOTING-SPIKE-PLAN.md` (full spike; §13 = honest assessment; §13.8 project map; §13.9 StreamJsonRpc; §13.10 MagicOnion).

## TL;DR — recommendation by axis

There is **no single replacement** for .NET Remoting here; pick the transport per *what the link does*:

| Communication leg | Recommended transport | Status |
|---|---|---|
| **Command plane** (client→QC→QE) | **gRPC unary** (code-first protobuf-net.Grpc) | Done & proven (spike M1) |
| **Store planes** (StateStore / KeyValueStore / Storage) | **gRPC unary + server-streaming** | Done & proven (spike M2–M4) |
| **Messaging fan-out broker** (transport) | **gRPC** unary `Publish` + server-streaming `Subscribe` | Done & proven (spike M1.b/M5); bounded per-stream buffer with `error-the-stream` (`ResourceExhausted`) back-pressure |
| **Callback / duplex legs** (firehose `Reply<T>`, server→client, §3.4 `ClientAction`) | **MagicOnion StreamingHub** (default) · **native gRPC bidi streaming** (no-new-dep fallback) · **StreamJsonRpc** (when arbitrary callback-object marshaling or a non-gRPC pipe is needed) | Prototyped (StreamJsonRpc, MagicOnion); bidi analyzed |
| **Server-push / fan-out to many external subscribers** | **ASP.NET Core SignalR** | If/when broad external clients appear |
| **Durable messaging** (persisted pub/sub) | **Redis / RabbitMQ / Kafka / Azure Service Bus** (± MassTransit) | Planned post-parity (§4.3) |
| **Distributing the engine itself** (QEs/subscriptions as remote objects) | **Orleans / Akka.NET** (actors) | Strategic, not yet needed |
| **Long-running `Task<T>` surviving restarts** | **Temporal / Durable Task Framework** | Orthogonal; only if required |

**Headline decision:** keep **gRPC** for the command/store/broker planes (already delivered), and for the **callback/duplex legs** prefer **MagicOnion StreamingHub** to stay on one transport — falling back to **native gRPC bidi streaming** if a new dependency is unwanted, and reserving **StreamJsonRpc** for cases that need arbitrary callback-object marshaling or a non-gRPC pipe.

## Context

The archived sample used `.NET Remoting`, which provided two things gRPC does not give for free:

1. **`MarshalByRefObject` reference marshaling** — pass a live object across the boundary so the *other side* can call back into it (the `Reply<T> : MarshalByRefObject, IObserver<T>` completion, `RemoteCancellationDisposable`, and the scheduler's `ClientAction` closures — see `GRPC-REMOTING-SPIKE-PLAN.md` §3.4, §13.4).
2. A hand-rolled **async-over-sync `Task<T>`** layer (`Nuqleon.Runtime.Remoting.Tasks`).

gRPC subsumes (2) natively (unary `Task<T>` + `CancellationToken` + streaming) and is the right fit for the high-throughput **command and store planes** — this is built and proven across spike milestones M1–M8, at parity with the in-process oracle. What gRPC does **not** give natively is (1): **server→client calls and live callback marshaling**, which the firehose/`Reply<T>` and §3.4 legs rely on. This record evaluates the production options for those legs.

**Constraints that shape the choice (verified):**
- Target: `net10.0`; the system is **.NET-to-.NET only** — no cross-language/polyglot client requirement exists today (no committed `.proto`/OpenAPI, no JS/Java/Python consumers).
- **gRPC is already adopted** as the primary transport, with a code-first contract model.
- The messaging broker is in-process today with a **durable broker explicitly planned** as a later swap behind the same contract (§4.3).
- Recovery/virtual-time scheduler scenarios are **in-process-only** by design (§3.4) and are out of scope for any wire transport.

## Decision drivers

Transport uniformity (one stack vs many) · native server→client capability · payload efficiency · cross-language reach · ops/ecosystem maturity (LB, deadlines, mTLS, observability) · dependency footprint & supply-chain · AOT/trimming friendliness.

## Options considered (with spike evidence)

| Option | Server→client? | Payload | Cross-lang | Dependency | Spike evidence |
|---|---|---|---|---|---|
| **gRPC unary/server-streaming** | No (unary); push only via server-streaming | protobuf (binary) | Strong | already in stack | **Built**, M1–M8, parity vs oracle |
| **StreamJsonRpc** | **Yes** — arbitrary callback-object marshaling + full bidi method calls | JSON (text) | Weak (.NET-centric) | `StreamJsonRpc` + `Nerdbank.Streams` | **Prototyped, 6/6** (`Tests.Reaqtor.Remoting.StreamJsonRpc`); wire-trace proves real bidi traffic |
| **MagicOnion** | **Yes** — typed `StreamingHub` receiver push | MessagePack (binary) | Weak (MessagePack/.NET/Unity) | `MagicOnion.*` (+ MessagePack) | **Prototyped, 2/2** (`Tests.Reaqtor.Remoting.MagicOnion`); real in-proc Kestrel/h2c host |
| **Native gRPC bidi streaming** | **Yes** — but as *messages*; you hand-roll the call/correlation protocol | protobuf (binary) | Strong | already in stack | Analyzed (§13.10 / the "what is bidi" discussion); not prototyped |
| **CoreWCF (duplex)** | **Yes** — `CallbackContract` over NetTcp/HTTP | binary/SOAP | Weak | `CoreWCF.*` (heavy) | Analyzed; most literal Remoting-duplex successor |
| **SignalR** | **Yes** — hub→client (return values since .NET 7) | JSON/MessagePack | **Strong** (JS/TS/Java/Python clients) | ASP.NET Core | Analyzed; best for push/fan-out to many |
| **Orleans / Akka.NET** | Yes — location-transparent calls/streams | framework | — | large | Analyzed; for distributing the engine, not a transport drop-in |
| **Message brokers** (Redis/RabbitMQ/Kafka/Service Bus) | Pub/sub (not RPC) | broker-specific | Strong | broker + client | Planned (§4.3) for the durable messaging leg |

## Rationale for the recommendation

- **Stay on gRPC for command/store/broker.** Already delivered, binary/fast, full HTTP-2 ecosystem, cross-language-ready. No reason to change.
- **Callback/duplex legs → MagicOnion StreamingHub (default).** It keeps the duplex legs on the **same gRPC/HTTP-2 transport** (uniform ops, binary MessagePack, typed server→client hubs) — the lowest-friction production answer for a gRPC-committed, .NET-only system. The prototype shows a real Kestrel/h2c host with the server pushing into the client's receiver (the `Reply<T>`/§3.4 analog).
  - **Fallback: native gRPC bidi streaming** when adding the MagicOnion/MessagePack dependency is unwanted — zero new dependencies, but you own the message-envelope + correlation layer (the spike's M1.b/M5 broker already did a focused version of this).
  - **Reserve StreamJsonRpc** for links that genuinely need **arbitrary callback-object marshaling** (`[RpcMarshalable]` `IObserver`/`IDisposable`) or a **non-gRPC pipe/sidecar** — it's the most flexible and is itself production-grade (it powers Visual Studio / Roslyn LSP), just not transport-uniform with the gRPC planes.
- **SignalR only for broad external push/fan-out** (many subscribers, possibly non-.NET) — not for internal peer RPC.
- **Durable broker (Redis/RabbitMQ/Service Bus) for the messaging leg** — already the planned swap; orthogonal to the RPC question.
- **Orleans/Akka only if/when the engine itself is distributed** (QEs/subscriptions as grains) — a programming-model decision, not a transport swap; overlaps the M6 multi-QE / M7 recovery directions.

## Consequences

**Positive**
- Command/store/broker on gRPC + duplex legs on MagicOnion = **one wire transport (HTTP-2) and one ops story** end-to-end.
- Server→client and callback semantics are restored without `MarshalByRefObject`.
- Each leg uses a fit-for-purpose, production-grade technology rather than forcing one model everywhere.

**Negative / costs**
- MagicOnion adds a **MessagePack contract model + a Cysharp OSS dependency**; its StreamingHub receivers are *push-shaped*, so the arbitrary client-object marshaling StreamJsonRpc offers is not like-for-like.
- Supply chain: MagicOnion 7.10.1 transitively pulls **MessagePack 3.1.4**, flagged by the NuGet audit (NU1903, GHSA-hv8m-jj95-wg3x); mitigated by pinning **MessagePack 3.1.7** in `Directory.Packages.props`. Track MessagePack advisories.
- Multiple technologies = more concepts for contributors (mitigated by confining each to a clearly-scoped leg).
- MessagePack/MagicOnion weaken any future **cross-language** story for the duplex legs; if polyglot duplex is ever required, prefer **native gRPC bidi** or **SignalR**.

**Neutral**
- StreamJsonRpc remains a valid, retained option for in-host/sidecar duplex; keeping the prototype documents the trade space.

## Verification / evidence

All prototypes build and run green on `net10.0`; `dotnet build All.slnx` is 0/0; the archived tree is untouched.

- **gRPC planes:** `Tests.Reaqtor.Remoting.Grpc` (command, 4 store adapters, standalone broker both-legs, multi-QE selector, recovery control plane, richer workloads) — parity vs the in-process oracle.
- **StreamJsonRpc:** `Tests.Reaqtor.Remoting.StreamJsonRpc` — 6 demos incl. a wire-trace that captures the actual JSON-RPC envelopes crossing the pipe in both directions (proves real bidi RPC, not in-process calls).
- **MagicOnion:** `Tests.Reaqtor.Remoting.MagicOnion` — 2 demos over a real in-process Kestrel/h2c MagicOnion server (unary + StreamingHub server→client push).

## Open questions / explicitly deferred

- A **cross-language** consumer requirement would re-weight toward native gRPC (incl. bidi) or SignalR over MagicOnion/StreamJsonRpc — none exists today.
- **Production TLS/mTLS** (the spike uses h2c on loopback), service discovery, and deployment manifests.
- **Durable broker selection** (Redis vs RabbitMQ vs Service Bus vs Kafka) for the messaging leg.
- Whether to **distribute the engine** (Orleans/Akka) — revisit alongside multi-QE (M6) and recovery (M7).
- Recovery's broker-firehose auto-resubscribe is a **transport-independent engine-semantics gap** (§13.3.2), not a transport-choice question.
