# `Reaqtive.Linq`

Provides an implementation of the Rx query operators with support for state persistence.

## Basic concepts

This library provides an implementation of a subset of Rx query operators that are semantically equivalent to classic Rx, but using the `ISubscribable<T>` interface rather than the `IObservable<T>` interface. Operators are provided as static methods on `Subscribable`, and are often extension methods to aid in fluent composition, e.g. through C# query expression syntax (LINQ). All operators implemented in this library support state persistence and can thus be used in the context of a query engine that performs checkpointing and recovery.

### Factories

The following factory methods are supported:

* `Empty<T>`
* `Return<T>(T)`
* `Throw<T>(Exception)`
* `Timer` with all variants known from Rx

> **Note:** We don't support `Create<T>` which is fairly complex given the additional lifecycle states implied by `ISubscribable<T>`. When creating a custom operator, one typically derives from `SubscribableBase<T>` instead.

Current omissions include:

* `Range` which could be added easily
* `FromAsync[Pattern]`
* `FromEvent[Pattern]`
* Various conversions from other types, e.g. `IEnumerable<T>` and `Task<T>`

### Aggregates

The following aggregates are supported:

* `Aggregate<TSource>`, `Aggregate<TSource, TResult>`, and `Aggregate<TSource, TAccumulate, TResult>`
* `All` with predicate support and `Any` with optional predicate support
* `Count<T>`, `LongCount<T>`, and `IsEmpty<T>`
* `First[OrDefault]Async` with optional predicate support
* `Min` and `Max`, for arbitrary types `T` and for primitives (and their nullable variants)
* `Sum` and `Average`, for arbitrary types `T` (using a selector function), and for primitives (and their nullable variants)
* `ToList<T>`

Other Rx aggregates such as `ToArray`, `ToLookup`, `ToDictionary`, and `ToHashSet` are currently not supported, but could be added in the future. These do have some concerns around state accumulation (see remark below).

> **Note:** The `ToList<T>` implementation supports a setting named `rx://operators/toList/settings/maxListSize` which is obtained through the `IOperatorContext.TryGetElement` method. This setting can be used to avoid endless growth of the state of the operator due to appending to the list upon every `OnNext` call (e.g. if a sequence is infinite). This is used by query engines to avoid checkpoint state bloat. Note that an alternative implementation could use operator local storage (see Pearls for a candidate implementation) to reduce the cost of checkpointing big lists, though it'd still be useful to enforce maximum state sizes.

Current omissions include:

* `Contains`
* `Last[OrDefault]Async`
* `Single[OrDefault]Async`
* `ToArray`, `ToLookup`, `ToDictionary`, `ToHashSet`

> **Note:** Operators like `Contains` are often parameterized on `IEqualityComparer<T>` for which we don't yet have quotation support across service boundaries. However, support for such operators can be added to support default comparers, as well as support with custom comparers for direct usage of the library. In the future, we could add first-class support for concepts such as (quoted) equality comparers and comparers, both through `Create<T>` factories for these, as well as support to bind to defaults.

### Standard Query Operators

The following traditional LINQ standard query operators are supported:

* `Where<T>` with an optional index parameter passed to the predicate function
* `Select<T, R>` with an optional index parameter passed to the selector function
* `Take<T>` and `Skip<T>` with either a count or a relative time
* `TakeWhile<T>` and `SkipWhile<T>` with predicates
* `SequenceEqual<T>`

> **Note:** The `SequenceEqual<T>` implementation does accumulate state in the form of queues because the receive rates on both sources can differ. To bound the state size, the operator supports a setting named `rx://operators/sequenceEqual/settings/maxQueueSize` which is obtained through the `IOperatorContext.TryGetElement` method. See remarks on `ToList<T>` for general information about state size manamgenet.

Furhermore, the following higher-order operators are supported as well:

* `SelectMany` with various overloads
* `GroupBy` with various overloads, producing `IGroupedSubscribable<TKey, TSource>` sequences

Support for higher-order operators is discussed in more detail later in this document.

Current omissions include:

* `Distinct` due to unbounded state growth (see remarks above)
* `Zip` due to unbounded state growth (see remarks above)
* `GroupJoin` and `Join` which have temporal semantics in Rx
* `Cast` and `OfType` because subtyping relationship are often erased in hosted settings (in favor of a structural typing approach)

Finally note that Rx omits operators such as `OrderBy[Descending]`, `ThenBy[Descending]`, `Intersect`, `Except`, and `Union`. Therefore, these are missing in Reaqtive as well.

### Rx Operators

The following Rx operators are supported:

* `Buffer` both count-based and time-based
* `CombineLatest` up to 16 sources
* `DelaySubscription`
* `DistinctUntilChanged`
* `Do`
* `Finally`
* `Merge` (higher-order)
* `Retry`
* `Sample`
* `Scan`
* `SkipUntil` and `TakeUntil` both with another sequence and with an absolute time
* `StartWith`
* `Switch` (higher-order)
* `Throttle` (higher-order)
* `Window` both count-based and time-based (higher-order)

Current omissions include:

* `Append` and `Prepend` which were added much later in .NET
* `Catch` because (nominal) exception types are poorly supported across service boundaries
* `Delay` due to concerns around unbounded state growth

> **Note:** Some of these can be provided in future iterations to reach parity with Rx. While not all operators may be usable across service boundaries (e.g. `Catch` comes to mind unless we support nominal types to a limited extent), it would still enable the use of the Reaqtive library as a drop-in replacement for classic Rx, e.g. if state persistence is desirable.

Note that `Finally` is often used by query engines to install a hook to handle `OnError` and `OnCompleted` in order to remove the subscription artifact from the engine. By default, subscription artifacts are kept in the engine, even after spontaneous termination, such that metadata queries can still locate the artifact (and more advanced operations such as visiting the now-terminated subscription operator tree, e.g. to harvest logs, stats, debug errors, etc.). However, when explicit deletion of an artifact is undesirable, and automatic clenaup is warranted, queries can get instrumented using a `Finally` hook whose `Action`-based callback is used to trigger deletion of the artifact in the engine (though the top-level interfaces for create and delete operations, i.e. as if the deletion request was made from the outside).

Uses of `Do` have also been common to support side-effects such as HTTP POST of events, potentially through a sequence of chained `Do` operators to support calling multiple endpoints. Note that Reaqtive can support "operators" in the `IObserver<T>` space as well, even though no such operators are directly provided in this library. Examples of such operators that have been built in extension libraries include `Then` (to chain observers for sequential invocation), `Parallel` (to invoke various observers in parallel), `Catch` (to fall back from one observer to an alternative, e.g. when using observers to POST to alternative endpoints), etc. In fact, a lot of the Rx operators (in the `IObservable<T>` or `ISubscribable<T>` space) could be implemented as lifted over corresponding observer operators, so things like `Select`, `Where`, etc. can be used here as well.

## Example usage

To illustrate the use of the reactive operator library in Reaqtive in the absence of a hosting environment such as a query engine, let's have a look at the following example:

```csharp
ISubscribable<string> obs = Subscribable.Timer(TimeSpan.FromSeconds(1)).Where(t => t % 2 == 0).Select(t => $"Tick = {t}");
IObserver<string> obv = Observer.Create<string>(s => Console.WriteLine(s));
ISubscription sub = obs.Subscribe(obv);
```

In regular Rx, the equivalent code using `Observable` rather than `Subscribable` would suffice to kick off a timer, apply the filter and projection, and see results getting printed by the observer. In Reaqtive, the resulting `ISubscription` is not yet active and needs additional steps to kick off the computation. At a minimum, there are two steps that are needed:

* Distribute operator context to the operators through the `SetContext(IOperatorContext)` method.
* Call `Start` on the operators to kick off the flow of events.

If the subscription were to be recovered from existing persisted runtime state, an additional `LoadState` phase would be inserted between the two phases above. The simplest way to perform the `SetContext` and `Start` operations is by using the `SubscriptionInitializeVisitor.Initialize(ISubscription, IOperatorContext)` method. To do so, we need to first create an operator context, which can be done by instantiating `OperatorContext`:

```csharp
public class OperatorContext : IOperatorContext
{
    public OperatorContext(Uri instanceId, IScheduler scheduler, TraceSource traceSource = null, IExecutionEnvironment executionEnvironment = null);

    // Properties omitted
}
```

We can safely omit the trailing parameters if we don't care for logging using a `TraceSource` and if we're not embedding the subscription in a hosting environment such as a query engine (more on the role of `IExecutionEnvironment` later, in the section on higher order operators).

The `Uri` parameter is an artifact of hosting and can be an arbitrary non-`null` value when used in a stand-alone context. However, a unique value is recommended because operators use it to perform tracing, and custom sources and observer sinks can also access it through the `IOperatorContext`.

The `IScheduler` will be important for our `Timer` to function properly. We can use the `Reaqtive.Scheduler` implementation, which provides a layered approach of `LogicalScheduler` instances (that own groups of tasks) that run on top of a `PhysicalScheduler` (that owns physical threads). In most cases, you just want one `PhysicalScheduler` per process, and individual `LogicalScheduler` instances for groups of subscriptions that are managed together. In this example, we'll create one of each:

```csharp
var phy = PhysicalScheduler.Create();
var sch = new LogicalScheduler(phy);
```

Finally, we can create an operator context, as follows:

```csharp
var uri = new Uri("nucleus://sample/reactive/sub/1");
var ctx = new OperatorContext(uri, sch);
```

Now we can perform the initialization by using `SubscriptionInitializeVisitor.Initialize`:

```csharp
SubscriptionInitializeVisitor.Initialize(sub, ctx);
```

and we'll see the timer ticking along. To dispose the subscription, we can simply call `Dispose` at a later point in time.

Because we have a `LogicalScheduler` around, much like a query engine would provide as well, we can also pause and resume the computation by using `PauseAsync` and `Continue` on the scheduler. This would be a first step if we want to perform checkpointing by using `SaveState` (typically through the `SubscriptionStateVisitor` helper functions). For example:

```csharp
await sch.PauseAsync(); // timer will be paused

await Task.Delay(10); // no events flow; we could take a stable checkpoint here

sch.Continue(); // timer resumes
```

A typical checkpointing interaction, as performed by query engines, looks as follows:

```csharp
await sch.PauseAsync();

if (SubscriptionStateVisitor.HasStateChanged(sub))
{
    var transaction = new Transaction(); // some transaction into a checkpoint state store
    var writer = new StateWriterFactory(tx); // implementing IOperatorStateWriterFactory

    SubscriptionStateVisitor.SaveState(sub, writer);

    sch.Continue();

    await transaction.CommitAsync();

    SubscriptionStateVisitor.OnStateSaved(sub);
}
else
{
    sch.Continue();
}
```

where `Transaction` and `StateWriterFactory` are custom-built facilities over some underlying store. Focusing on the basic steps, this is how checkpointing works:

1. Pause the computation so we can take a stable snapshot.
2. Check if any state in the subscription is dirty. (This can be skipped if we want a full checkpoint rather than a differential one.)
   a. If not, continue the computation straight away.
   b. Otherwise, if there's dirty state:
      1. Save the state to a state writer, through an `IOperatorStateWriterFactory`. This could be done to in-memory streams and will involve the use of a state serializer.
      2. Continue the computation, so we can perform useful event processing while the state is attempted to be committed to the underlying store.
      3. Start committing the state snapshot to the store.
         a. If it succeeds, visit the subscription one more time to unmark `StateChanged` flags (unless new edits to state happened already).
         b. Otherwise, if it fails, report the error and hope that a future checkpoint succeeds in committing the (newly harvested) state to the underlying store.

Loading state from a checkpoint is done by inserting a `SubscriptionStateVisitor.LoadState` call in between performing `SetContext` and `Start`. So rather than using `Initialize`, we'd decompose this into three steps:

```csharp
SubscriptionInitializeVisitor.SetContext(sub, ctx);

var transaction = new Transaction(); // some transaction into a checkpoint state store
var reader = new StateReaderFactory(tx); // implementing IOperatorStateReaderFactory
SubscriptionStateVisitor.LoadState(sub, reader);

SubscriptionInitializeVisitor.Start(sub);
```

where `Transaction` and `StateWriterFactory` are custom-built facilities over some underlying store.

## Higher-order operators

One of the most powerful features of LINQ and Rx is in their ability to deal with higher-order sequences, i.e. sequences of sequences. For example, operators like `GroupBy` produce a sequence of sequences, where the inner sequences represent groups that have elements within them. Conversely, operators like `Merge` take nested sequences and flatten them:

```csharp
ISequence<IGroupedSequence<TKey, TElement>> GroupBy<TKey, TElement>(this ISequence<TElement> source, Func<TElement, TKey> keySelector);
ISequence<T> Merge<T>(ISequence<ISequence<T>> sources);
```

We're intentionally using a non-existent `ISequence<T>` construct here to emphasize that such operators work for any sequence type, including `IEnumerable<T>` and `IObservable<T>` sequences, or variants thereof. Focusing on Rx in particular now, we have various operators that exhibit a higher-order nature, of which a subset is shown below:

```csharp
// Produce higher-order sequences
IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TKey, TElement>(this IObservable<TElement> source, Func<TElement, TKey> keySelector);
IObservable<IObservable<T>> Window<T>(this IObservable<T> source, /* extra parameters to control windowing behavior */);

// Consume higher-order sequences
IObservable<T> Merge<T>(IObservable<IObservable<T>> sources);

// Construct inner sequences
IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector);
IObservable<TSource> Throttle<TSource, TThrottle>(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> selector);
```

A key characteristic of such operators is that they create other reactive artifacts such as subjects (to represent inner sequences and push events into these) or subscriptions (to receive events from inner sequences). This poses some challenges when we want to checkpoint and recover state of computations that involve such higher-order operators.

In the absence of state persistence, all of these operators work out of the box in this library. When state persistence is desired, additional support is required from the environment in the form of the `IExecutionEnvironment` passed through the `IOperatorContext`. In a nutshell, this mechanism is used by higher-order operators to instruct the surrounding environment to create or delete inner artifacts such as subjects or subscriptions. The parent higher-order operator then keeps track of these artifacts using `Uri`-based identifiers. The benefit of this approach of deconstructing higher-order operator graphs into smaller sibling artifacts is that their lifetimes can be decoupled and state persistence can be done at a more fine-grained level, which reduces the amount of state that has to be saved in case some piece of state is dirtied. For example:

```csharp
xs.SelectMany(x => ys(x).Take(10))
```

For each event received from `xs`, an inner subscription is made to `ys(x).Take(10)` which involves a piece of state to keep track of the number of remaining elements to be emitted by the `Take` operator. Assume that one such inner `ys(x)` has a low event volume, while another has a high event volume. If we were to checkpoint the whole top-level subscription as one entity, we'd have to rewrite the state of all operators, the number of which is dynamic and a function of the number of events received by `xs`. To tame this state growth, we create the inner subscriptions and subjects as separate related artifacts.

> **Note:** Alternative approaches to checkpointing are possible, for example by making the entire `ISubscription`/`IOperator` based object tree fine-grained at the level of individual nodes. The drawback of such an approach is that the state size becomes bigger because we're persisting "pointers" between the individual objects as well. In the current design, we save "segments" that have a relatively static shape (typically just chains of stateful operators, or small-ish trees, e.g. in the case of `CombineLatest`) as state blobs, while dynamically changing nodes (e.g. `SelectMany` having an arbitrary number of inner subscriptions, or `GroupBy` having an arbitrary number of inner subjects representing the groups) get decomposed into smaller constituents.

While an in-depth overview of the inner workings of higher-order operators is outside the scope of this document, it suffices to say that operators check for the `IOperatorContext.ExecutionEnvironment` property to be `null` when deciding whether they're being hosted in an environment that provides facilities to create specialized inner artifacts. Furthermore, a type test for an axuiliary `IHigherOrderExecutionEnvironment` specialization of `IExecutionEnvironment` is often used to discover additional facilities. Hosting environments (such as query engines) that provide these additional facilities implement this functionality in a separate assembly and initialize reactive aritfacts using a context that provides the `IHigherOrderExecutionEnvironment`.

> **Note:** When digging into higher-order operator support, one will find funny names such as *bridges*, *tunnels* and *tollbooths*. These are used to provide connections between higher-order and their inner artifacts, and to do some form of resource management. Their names are historical. Also, the separation of `IHigherOrderExecutionEnvironment` from `IExecutionEnvironment` is a matter of decoupling of the operator library from hosting environments, making the `Reaqtive.Linq` library usable outside the context of an execution environment where it gets "enlightened".

## Supporting async operations in query expressions

Use of async operations as part of query operator evaluation is not directly supported by this library. As a running example, consider a `Where` operator that's parameterized on a `Func<T, Task<bool>>` rather than a synchronous `Func<T, bool>`. One possible approach to enable this is the use of an `IAsyncObservable<T>` interface (and hence async variants of observers and disposables as well) akin to Async Rx. This would enable all operators to perform async operations, not only as specified by the user (through selectors, predicates, etc.) but also for purposes of binding inner sequences. For example, `SelectMany`'s selector function may dynamically bind an inner sequence based on the payload of a received event, e.g. `xs.SelectMany(x => ctx,GetObservable<int>(f(x)))`. Such binding steps may need lookups in registries which require async (e.g. crossing network boundaries).

However, the use of async gets quite pervasive because some operators may need async logic during the `Start` phase, which gets invoked through a subscription visitor, which then in turns needs to become asynchronous as well. Furthermore, this now poses a challenge on state persistence as well. The current checkpointing approach is based on efficient pause and resume operations, and the use of async operations can make the pause time much longer. Alternative checkpointing approaches by flowing checkpoint markers through operators can solve this issue, but have their own challenges as well.

> **Note:** The main idea of flowing checkpoint markers through operators is to send a special notification on "ingress" observers in between regular events. This happens at the edge of a query engine, and the markers are associated with sources and carry a sequence number of the preceding event in the source (i.e. they will represent the state of the computation up to and including said event in the source). Such a notification always flows through query operators directly (i.e. never gets queued, and thus behaves in a way similar to `OnError` for most operators), and operators append their state to it. When this special event comes out on "egress" observers on the other end, its contents can get persisted. The computation was never paused in this scheme. However, complexity arises when dealing with complex query operators that have multiple sources (e.g. `CombineLatest` having to match up checkpoint markers on all sources), complex lifecycle events (e.g. `Concat`), or a high-order nature (e.g. `SelectMany`, `Window`, etc.).

An alternative approach is to rely on higher-order variants of query operators where predicates, selectors, etc. are modeled as `Func<T, IObservable<R>>`. Such inner sequences then get managed by a query engine as external sequences (just like sources), where the underlying async evaluation (e.g. of a `Task<bool>`-returning predicate that gets ingested as an `IObservable<bool>` through the query engine boundary) is taken care of outside the query engine, and thus doesn't interfere with the requirement of a timely pause of the computation when taking a checkpoint. When the result of such an "externalized" asynchronous operation is available, it gets pushed into the inner sequence across the query engine boundary by using a context-switch operator that properly schedules the event. In addition, prior to sending the event into the computation, the external component can persist the event, assign a sequence number, and associate it with parameter values of a selector or predicate (in order to support consistent replay at a later point in time).

For example, consider a query `xs.Where(async x => await f(x))`, where `f(int)` returns a `Task<bool>`. When binding such a query, we would rewrite it as `xs.Where(x => f_observable_proxy(x))` where `f_observable_proxy(int)` is an observable wrapper that returns an `IObservable<bool>`. Internally, this wrapper evaluates and awaits `f(int)`, but prior to sending the resulting `bool` on the inner sequence, it persists the `int, bool` pair. Upon replay during a failover of a query engine, this (persisted) function memoization cache is consulted to look up prior evaluations of the predicate in order to ensure consistent behavior (i.e. we force the function to behave idempotently). Alternatively, we can persist a `long, bool` pair, where the `long` value represents a sequence number (i.e. the n-th evaluation of the predicate). This could get modeled internally as an `IMultiSubject<int, bool>` where each `OnNext(int)` on the input side results in an `OnNext(bool)` on the output side, and the subject keeps track of sequence numbers internally. Alternatively, operators such as `Where` with an asynchronous predicate could do their own sequencing (i.e. just keep a monotonically increasing sequence number) and use that as a unique key passed to the externalized (asynchronous) function evaluation component, which can use it to look up the result of a prior evaluation.
