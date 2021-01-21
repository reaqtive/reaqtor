# `Reaqtive.Core`

Provides a set of base classes and utilities used for in-memory reactive event processing.

## Layering of the assemblies

This library implements various interfaces that are provided in `Reaqtive.Interfaces`. It is used by a variety of components in Reaqtive, including `Reaqtive.Linq` but also query engine implementations. The separation of the `Core` assembly from the `Linq` assembly on top is an intentional design goal to reduce coupling between a query engine and the operator library, because alternative implementations (or subsets) can be provided.

## Subscribables

The `SubscribableBase<TResult>` class provides an abstract base class to implement `ISubscribable<T>` with a few asserting behaviors enabled in DEBUG builds. It merely provides a `SubscribeCore` abstract method, as shown below:

```csharp
protected abstract ISubscription SubscribeCore(IObserver<TResult> observer);
```

Using this base class is recommended over manual implementation of the interface.

## Subscriptions

Various `ISubscription` implementations are provided by this library, in a manner similar to Rx's `IDisposable` algebra. These include:

* `CompositeSubscription` to wrap a dynamic number of `ISubscription` objects, where the number of them can vary widely (e.g. dependent on event counts).
* `StableCompositeSubscription` to wrap a dynamic number of `ISubscription` objects, whose total count is relatively stable (e.g. dependent on static properties of an operator).
* `StaticCompositeSubscription` to wrap a static number of `ISubscription` objects (e.g. when an operator always has the same number of child subscriptions).
* `SingleAssignmentSubscription` to wrap a single `ISubscription` object that can get assigned at a later point in time.
* `SerialSubscription` to wrap a single `ISubscription` object that replaces an existing `ISubscription` object upon reassignment.
* `NopSubscription` as a no-op implementation of `ISubscription`.

Besides the `ISubscription` implementations, the library also provides facilities to visit subscription object trees. The base class is a `SubscriptionVisitor` that implements `ISubscriptionVisitor` and discovers children of `IOperator` nodes by using the `IOperator.Inputs` property. Derived classes can override `Visit` (to control behavior of visiting children) and `VisitCore` methods (to simply handle visiting each node). For example:

```csharp
class MyVisitor : SubscriptionVisitor
{
    protected override void VisitCore(IOperator node)
    {
        // Handle operator node
    }
}
```

Auxiliary visitor types are provided to perform simple type checks on nodes prior to calling a virtual method. This is useful to visit nodes that implement a certain interface, e.g. `IUnloadableOperator`. To do so, one can instantiate an `SubscriptionVisitor<T>`, giving it an `Action<T>` to handle nodes that pass the `if (node is T)` check. An easier way is to use a factory method that uses a builder pattern:

```csharp
public class SubscriptionVisitor : ISubscriptionVisitor
{
    public static SubscriptionVisitorBuilder<T> Do<T>(Action<T> visit)
        where T : class, IOperator;

    // Omitted other members
}
```

The builder allows specifying more `Do<T>` actions for different types, so you can build a visitor that handles nodes of different types. When the builder is configured, one can use `Apply(ISubscription)` to apply the logic to a given subscription object. For example:

```csharp
SubscriptionVisitor.Do<IUnloadableOperator>(u => u.Unload()).Apply(subscription);
```

A few standard visitors are provided as well, which operate on the basic lifecycle operations provided by `IOperator` and `IStatefulOperator`. These visitors are `SubscriptionInitializeVisitor` and `SubscriptionStateVisitor`. Let's have a look at the former one first, focusing on a few commonly used methods:

```csharp
public class SubscriptionInitializeVisitor
{
    public static void Initialize(ISubscription subscription, IOperatorContext context);

    public static void SetContext(ISubscription subscription, IOperatorContext context);

    public static void Start(ISubscription subscription);
}
```

> **Note:** Besides static methods, instance variants are provided as well, which operate on an instance of the visitor that's given an `ISubscription`. We omit this detail for clarity.

In here, `Initialize` calls both `SetContext` and `Start` on all of the nodes in the given subscription. This combination is useful when node state needs to be loaded (e.g. when recovering a subscription). If state loading should take place prior to calling `Start`, one can call `SetContext` first, then use the `SubscriptionStateVisitor` to handle state loading, and finally call `Start`.

> **Note:** A query engine does perform these operations in separate stages of recovery. For example, it may first provide context to all reactive entities in the system after reinstantiating these from persisted expression trees. Next, it may restore state to stateful artifacts, possibly in a concurrent fashion. Finally, it can kick off the event flow using `Start`, often phasing this on an artifact-by-artifact basis (e.g. subscriptions first, subjects lsat).

The `SubscriptionStateVisitor` provides ways to visit `IStatefulOperator` nodes to perform state loading and saving, and related operations. The essence is shown below:

```csharp
public class SubscriptionStateVisitor
{
    public static bool HasStateChanged(ISubscription subscription);

    public static void SaveState(ISubscription subscription, IOperatorStateWriterFactory factory);

    public static void LoadState(ISubscription subscription, IOperatorStateReaderFactory factory);

    public static void OnStateSaved(ISubscription subscription);
}
```

> **Note:** Besides static methods, instance variants are provided as well, which operate on an instance of the visitor that's given an `ISubscription`. We omit this detail for clarity.

These methods should be pretty self-explanatory. For state recovery, the `LoadState` method is used, between calls to `SetContext` and `Start`. For state checkpointing, three phases are used:

1. Use `HasStateChanged` to figure out if any state in a subscription has changed. This is optional if one wants to perform a full checkpoint, but is used to support differential checkpoints.
2. Use `SaveState` to write the state of a subscription to a state writer.
3. Use `OnStateSaved` to unmark dirty bits after a successful commit of the transaction used to save the state.

> **Note:** A query engine performs all of these steps after pausing the scheduler, and often does this in a highly concurrent manner. State is often saved to in-memory streams that only later get committed to a persistent store. This is done to ensure that the first two steps take as little time as possible, in order to avoid pausing the scheduler for too long.

## Operators

In order to make implementing query operators easier, this library provides a number of base classes that can be used to reduce the amount of plumbing needed. These are:

* `Operator<TParam, TResult>` as the most general purpose base class.
  * `VersionedOperator<TParam, TResult>` adds support for versioning.
    * `StatefulVersionedOperator<TParam, TResult>` adds supports to save and load state.
* `UnaryOperator<TParam, TResult>` as a specialization for operators with a single source.
  * `VersionedUnaryOperator<TParam, TResult>` adds support for versioning.
    * `StatefulVersionedUnaryOperator<TParam, TResult>` adds supports to save and load state.

First, note the commonality in generic parameters:

* `TParam` is used to specify a piece of state that represents the operator's parameters, which are not persisted as part of operator state. These typically flow down from operator factories in the `ISubscribable<T>` space, e.g. the `count` parameter of a `Take` operator. Upon recovery of the expression that represents the composition of operators (the "query" expression), the operators get reinstantiated, and thus the (immutable) parameters will be retained.
* `TResult` parameter represents the result produced by the operator, which is fed to the downstream observer through an `IObserver<TResult>` that's made available to derived classes using the `Output` property.

Operator base classes implement some logic directly, e.g. `Start`, and provide additional virtual methods that can be overridden to implement custom behaviors. For example, `OnSubscribe` is called when the operator is connected to one or more sources, and the derived class is asked to return a sequence of `ISubscription` objects that will be exposed through the `Inputs` property that's used by visitors to traverse the tree:

```csharp
// Operator<TParam, TResult>
protected virtual IEnumerable<ISubscription> OnSubscribe();
```

For unary operators, this method is required to return a single `ISubscription` instead:

```csharp
// UnaryOperator<TParam, TResult>
protected virtual ISubscription OnSubscribe();
```

Other virtual methods include `OnStart` and `OnDispose` to deal with the basic lifecycle events.

```csharp
protected virtual void OnStart();
protected virtual void OnDispose();
```

Finally, `SetContext` provides a means to consult the context, or store it in a field. By default, this method doesn't do anything. Common uses are for operators to look up settings from the context, or for sources and sinks to access host-level facilities that are provided by the query engine that hosts the operator instance (e.g. an ingress and egress manager that is used to receive and send events to other machines or services).

```csharp
public virtual void SetContext(IOperatorContext context);
```

The `Versioned*` variants implement `IVersioned` and require the derived class to implement `Name` and `Version`:

```csharp
public abstract string Name { get; }
public abstract string Version { get; }
```

Finally, the `Stateful*` variants derive from the `Versioned*` variants and add `IStatefulOperator` support by providing a few members:

```csharp
public virtual bool StateChanged { get; protected set; }

public void LoadState(IOperatorStateReader reader, Version version);

protected virtual void LoadStateCore(IOperatorStateReader reader);
protected virtual void LoadStateCore(IOperatorStateReader reader, Version version);

public void SaveState(IOperatorStateWriter writer, Version version);

protected virtual void SaveStateCore(IOperatorStateWriter writer);
protected virtual void SaveStateCore(IOperatorStateWriter writer, Version version);

public virtual void OnStateSaved();
```

The first `StateChanged` property is used to detect whether state is dirty. Operators use the setter to mark the state as dirty by assigning `true`. For example, this is done after receiving an event in an `Average` operator which has caused the `sum` and `count` fields to get updated. On the other hand, receiving in event in `DistinctUntilChanged` that's the same as the previous event will not toggle the dirty flag. Every operator has different behavior when it comes to marking state as dirty.

Next, `LoadState` and `SaveState` are implemented in terms of `*Core` methods. Variants without a `Version` parameter are called when the version is the same as the one returned by the `Version` property, thus reflecting state persistence using the latest version. If the version ever changes, the overloads with `Version` parameters become important to deal with state that has a different version, e.g. when recovering state that was written using an older version of the operator, and thus may need to deal with missing values or conversions of state representation.

Finally, `OnStateSaved` is typically not overridden and deals with the `StateChanged` dirty flag logic internally, but derived classes can override it as a way to get notified when a checkpoint was successfully committed. An example of such an override is an ingress operator that receives events from an external system, which may use the `OnStateSaved` call to send an acknowledgement to the external system allowing it to prune events up to a given sequence number.

## Context-switch operators

The `ContextSwitchOperator<TParam, TResult>` operator deserves its own section. This type acts as a base class for stateful operators that receive events from external sources and need to produce `On*` invocations on downstream observers running within a query engine. One of the requirements for events flowing through query operators hosted in a query engine is that they originate on the right `IScheduler` in order to allow the processing of events to be paused when a checkpoint is made. To achieve this, we need to "context switch" events from the outside world into the query engine world, i.e. on the `IScheduler`.

From a top-level point of view, a context switch operator exposes the `IObserver<TResult>` interface which is used to send notifications from any thread external to the engine. Think of this interface at sticking out of the boundaries of the hosting query engine, into the external world. Rather than flowing these events directly to downstream observers (for example, query operators to process the events), the context switch operator maintains an internal queue that gets drained on the `IScheduler` that's obtained from the `IOperatorContext` passed to `SetContext`. This is part of the `IOperator` implementation, which is used within the query engine's world. The context switch operator connects both worlds.

Internally, the context switch operator is implemented as a stateful operator (where the state is comprised of the events in the queue that have not yet been sent downstream) and an `IYieldableItemProcessor`, which is a scheduler construct that processes the items in a queue and gets scheduled periodically to continue to drain the queue. The yield support is required to support prompt bail out from processing events when a checkpoint is requested.

## Observers

This library also provides a number of built-in observers and base classes to build observers. Some of the ready-to-use observers include:

* `NopObserver<T>`, an observer that ignores all `On*` notifications.
* `FaultObserver<T>`, an observer that throws exceptions when handling `On*` notifications.
* `Observer.Create<T>`, a factory method to create observers given delegates for the three `On*` methods.

The `Observer<T>`, `VersionedObserver<T>`, and `StatefulObserver<T>` are similar to the base classes used to implement operators; they do provide a set of virtual and/or abstract methods to control the behavior of an observer. Stateful observers implement `IStatefulOperator` can be used within `ISubscription`-based operator trees (for example in the `Do` operator and as the input to the top-level `Subscribe` method) and are hence reachable by visitors. Examples of stateful observers include egress sinks that generate sequence numbers for events sent to an external service; their state can contain the latest known sequence number that was used when emitting an event (i.e. a high watermark).

## Subjects

Besides subscribables ("observables") and observers ("subscribers"), this library supports subjects based on the `IMultiSubject` interface. Base classes are provided in the form of `MultiSubjectBase` and related classes, which all implement `IMultiSubject`. The two abstract methods to override are:

```csharp
protected abstract IObserver<T> GetObserverCore<T>();
protected abstract ISubscribable<T> GetObservableCore<T>();
```

to handle both the consuming (observable) and producing (observer) sides of a subject. Implementations of subjects may be in-memory, akin to the different Rx subjects, or be backed by an external streaming service where the observers publish events and the observables receive events. State can be maintained by inheritin from `StatefulMultiSubjectBase` which derives from `VerionedMultiSubjectBase`, similar to operators and observers.

> **Note:** There are many valid approaches to model external streams. One is to have a subject that acts as a proxy to the external stream both for publication and subscription. Another is to have observable/observer pairs that are defined as distinct artifacts. Yet another approach is to have generic observable and observer implementations for a specific streaming service, accepting the external stream name (or a more elaborate connection string) as a parameter (e.g. `EventHubObserver<T>(string)`).

## Tasks

Scheduling work through the `IScheduler` interface requires the use of the `ISchedulerTask` interface. This enables reduction of allocations compared to a delegate-based approach, as is done in Rx. For example, an operator can implement the task interface and use it to register for callbacks from the scheduler at a specified time. However, it's often handy to allocate simple tasks through common implementations of `ISchedulerTask`. Therefore, this library provides a few such implementations, including:

* `ActionTask` which takes an `Action` to invoke on the scheduler.
* `ActionTask<T>` which takes an `Action<T>` and a `T`, representing some state to avoid a closure, to invoke on the scheduler.

All action-based tasks are single-shot and implement the `Execute` method by returning `true` to indicate that the task has completed after invoking the action once.

Besides regular tasks, there's also support for `IYieldableSchedulerTask` which is recommended if a task goes in a busy loop and should be able to yield back to the scheduler promptly, e.g. in case a checkpoint is initiated. Yieldable tasks are given a `YieldToken` that should be observed for yield requests. The implementation of the `Execute` method is represented through a `Func<YieldToken, bool>`, thus supporting periodically invoked tasks that return a `false` value to indicate they are not yet complete and have to be invoked again. Because yielding is interrupting running work, it's natural for a yieldable task to return in a non-completed status. Two yieldable tasks are provided:

* `YieldableActionTask` which takes a `Func<YieldToken, bool>` to invoke on the scheduler.
* `YieldableActionTask<T>` which takes a `Func<T, YieldToken, bool>` and a `T`, representing some state to avoid a closure, to invoke on the scheduler.

Finally, the library introduces the notion of an `IItemProcessor` which looks like this:

```csharp
public interface IItemProcessor
{
    int ItemCount { get; }
    void Process(int batchSize);
}
```

Item processors are runnable whenever `ItemCount` is larger than `0`. When invoked, the `Process` method is requested to process a batch of items, causing `ItemCount` to decrease (though, obviously, other items may get enqueued at the same time). This is useful for constructs such as `ContextSwitchOperator` which maintain an internal queue of events that need to get processed on a scheduler, while periodically yielding after processing a batch, in order to allow other work to make process as well. The `IYieldableItemProcessor` variant supports yielding while processing a batch:

```csharp
public interface IYieldableItemProcessor : IItemProcessor
{
    void Process(int batchSize, YieldToken yieldToken);
}
```

Two implementations of these interfaces are provided in the library:

* `ItemProcessingTask` which wraps an `IItemProcessor` implementation to make it an `ISchedulerTask`.
* `YieldableItemProcessingTask` which wraps an `IYieldableItemProcessor` implementation to make it an `IYieldableSchedulerTask`.

It is quite common though to implement the interfaces directly, e.g. on an operator whose state contains a queue of events to dispatch.

## Utilities

Finally, this library provides a number of utilities, including:

* The `ToSubscribable` conversion for `IObservable<T>` implementations.
* An implementation of `IOperatorContext` provided by the `OperatorContext` class.
* Various static methods on `StateManager` to deal with loading and saving operator state.
* A `StateChangedManager` struct which can be embedded in the implementaton of stateful operators and manages the `StateChanged` transitions.
