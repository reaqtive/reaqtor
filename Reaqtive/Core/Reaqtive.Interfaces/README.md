# Reaqtive.Interfaces

Provides the essential interfaces used for in-memory reactive event processing.

## Basic concepts

This library provides alternative implementations for reactive event processing in a way similar to Rx. The main difference is the ability to visit an event processing object tree in order to apply various operations to it, including saving or loading the state of query operators. This enables the library to be used in the context of a hosting environment, for example the Reaqtor query engine which performs periodic checkpointing of query operator state.

The base interface is `ISubscribable<T>` which is analogous to `IObservable<T>` in Rx. The definition is shown below:

```csharp
public interface ISubscribable<out T> : IObservable<T>
{
    new ISubscription Subscribe(IObserver<T> observer);
}
```

> **Note:** The inheritance from `IObservable<T>` is a historical artifact from an earlier effort to try to unify classic Rx and the Reaqtive operator library. A better alternative is likely to build Rx-compatible wrappers around the Reaqtive library.

The main difference with `IObservable<T>` is the return type of `Subscribe` which is no longer an `IDisposable` but an `ISubscription`. Note that the parameter to `Subscribe` is still a classic `IObserver<T>`, though a variant called `ISubscriber<T>` could be considered moving forward (to enable a few more lifecycle operations).

An `ISubscription` is the handle to a reactive computation which can be composed out of query operators. Similar to Rx, it can be used to dispose the subscription because it inherits from `IDisposable`. In addition, it supports visiting the operator tree underneath the subscription handle by accepting an `ISubscriptionVisitor` through an `Accept` method:

```csharp
public interface ISubscription : IDisposable
{
    void Accept(ISubscriptionVisitor visitor);
}

public interface ISubscriptionVisitor
{
    void Visit(IOperator node);
}
```

Subscription visitors use an auxiliary interface called `IOperator` that gets implemented by objects used to implement (observable) sources, query operators, and (observer) sinks. This interface is central to the basic lifecycle management of these nodes, as well as to reflect the structure of the operator tree. In particular, the `Inputs` property is used to find the inputs to an operator, i.e. traverse from a downstream operator to its upstream operators. For example, in a query expression like `xs.CombineLatest(ys, f)`, the `IOperator` implementation of `CombineLatest` would return the `ISubscription` handles retrieved from subscribing to `xs` and `ys`.

```csharp
public interface IOperator : IDisposable
{
    IEnumerable<ISubscription> Inputs { get; }

    void Subscribe();

    void SetContext(IOperatorContext context);

    void Start();
}
```

The lifecycle of operators is also different from Rx. In classic Rx, `Subscribe` both attaches an observer and kicks off the flow of events. In Reaqtive, `Subscribe` merely attaches an observer, and subsequent calls to `IOperator.Subscribe`, `IOperator.SetContext`, and `IOperator.Start` are used to initialize inputs, to provide additional context, and to kick off the event flow. Additional steps can be taken in between `IOperator.SetContext` and `IOperator.Start`, as we'll discuss later. Tearing down an active operator is still done through the `Dispose` method.

While the top-level `Subscribe` call is shallow, the `IOperator.Subscribe` call can trigger the initialization of upstream subscriptions, which get persisted in the `Inputs` collection, used to continue the traversal into the upstream subscriptions. For example, an operator like `CombineLatest` will use `Subscribe` to create subscriptions to its sources and store these in `Inputs`.

`IOperator.SetContext` allows providing additional context to operators, through an `IOperatorContext` object. This facility enables distributing various utilities to operators, some of which are general-purpose, but some may be domain-specific. For example, a Reaqtive `IScheduler` is provided through the operator context, which is different from the approach in Rx where operators are parameterized on schedulers.

> **Note:** The use of a single scheduler for an entire reactive computation is essential for a query engine to control the flow of events and support pausing computations in order to checkpoint query operator state. Furthermore, users in Reaqtive do not have a notion of schedulers (i.e. there's no "quoted" variant of `IScheduler` - `IQeduler`? - to flow a scheduler representation through expression trees). The mechanism to distribute a scheduler to operators has also been a popular request for classic Rx. Both approaches could be made complementary, but supporting `IScheduler` parameters on operators and having one scheduler take precedence over another. This could be an approach for Reaqtive-Rx convergence.

The complete `IOperatorContext` interface is shown below:

```csharp
public interface IOperatorContext
{
    Uri InstanceId { get; }

    IReactive ReactiveService { get; }

    TraceSource TraceSource { get; }

    IExecutionEnvironment ExecutionEnvironment { get; }

    bool TryGetElement<T>(string id, out T value);
}
```

Extensibility is provided through the `TryGetElement<T>` method. In hosting environments such as the Reaqtor query engine, this method is typically implemented to return facilities that are retrieved by some query operators, sources, or sinks. For example, source that receive events from an external source may need access to some "ingress manager" object (which may deal with asynchronously receiving events from some message bus, deal with connection management and retry logic, multicasting across multiple engines, handle replay requests based on sequence IDs, etc.). One can think of the context as a collection of utilities that are not specified by users but need to flow to operators, without relying on static global variables.

Other properties are briefly described below:

* `InstanceId` is an artifact of hosting in a query engine and represents the identifier of the top-level artifact the operator is instantiated for (e.g. a subscription identifier). The use of `Uri` is historical.
* `ExecutionEnvironment` is also used for hosting scenarios and provides direct access to sibling artifacts. For example, `SelectMany` uses this to get `ISubscription` handles for inner subscriptions.
* `TraceSource` is a historical artifact used for tracing in query operators, which can be scoped to an operator, the containing artifact (e.g. a subscription), an engine, or a host.

Zooming in to the `IExecutionEnvironment`, we find the following members:

```csharp
public interface IExecutionEnvironment
{
    IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri);

    ISubscription GetSubscription(Uri uri);
}
```

As mentioned earlier, these are used in the context of hosting the query operator library in an engine. Various operators like `SelectMany` and `GroupBy` will check whether the environment is available (by checking for `null`). If so, they use the environment to decouple the lifetimes of outer and inner artifacts, e.g. inner subscriptions generated by `SelectMany` and groups (which are subjects in disguise) generated by `GroupBy`.

## Subscriptions

Classic Rx models the subscriptions are `IDisposable` interfaces and provides a small algebra on top of this type, e.g. to provide things like `CompositeDisposable`. The Reaqtive library provides similar facilities in the form of `ICompositeSubscription` and `IFutureSubscription` (similar to `SingleAssignmentDisposable`). The key difference is in the ability to traverse such subscriptions using visitors.

Note that the Reaqtive library has the advantage of a better lifecycle management compared to Rx. In particular, when creating a subscription, the computation doesn't kick off immediately, so there are no inherent race conditions when trying to set up inner subscriptions. It's not until `Start` is called that events will start flowing, which may in turn cause subcriptions to be torn down. As such, the object tree can be made stable before events flow. For more advanced query operators, the library often relies on the execution environment and hosting facilities to decouple lifetimes of inner and outer artifacts, in order to reduce the amount of state that has to be checkpointed.

> **Note:** One of the trickiest parts of state persistence in Rx is in the dynamically evolving operator object tree. For example, an operator like `StartWith` does not subscribe to its source until it has emitted all the prepended events that are specified to "start with". Therefore, if a checkpoint happens before the prepended events are emitted, there won't be an inner subscription (which is a subtree that can have any arbitrary shape). Operators often persist additional state to indicate their internal phasing, e.g. `StartWith` has to keep track of the index of the latest prepended event it has emitted. That index will also indicate when an inner subscription has been made after emitting all of the prepended events.

## Operators

Query operators are implemented using an auxiliary `IOperator` interface, which is used by the visitor to traverse from an operator to its inputs (thus constructing a tree-like structure). It provides the basic lifecycle methods `SetContext`, `Start`, and `Dispose`. However, more derived interfaces can be used to add additional phases, for example to implement stateful operators that can be checkpointed and recovered. That's where `IStatefulOperator` comes in:

```csharp
public interface IStatefulOperator : IOperator, IVersioned
{
    void LoadState(IOperatorStateReader reader, Version version);

    void SaveState(IOperatorStateWriter writer, Version version);

    void OnStateSaved();

    bool StateChanged { get; }
}
```

First of all, stateful operators support versioning through some `IVersioned` interface. All persisted state is prefixed with an operator name and version in order to have a basic correctness check (e.g. if an operator object tree does not match persisted state, the name will mismatch) and to support versioning of state.

```csharp
public interface IVersioned
{
    string Name { get; }

    Version Version { get; }
}
```

Besides versioning support, stateful operators support two core operations, namely `LoadState` and `SaveState`. Let's first focus on saving state, which uses an `IOperatorStateWriter`:

```csharp
public interface IOperatorStateWriter : IDisposable
{
    void Write<T>(T value);

    IOperatorStateWriter CreateChild();
}
```

The key method here is `Write<T>` which is used to write a value of type `T`. Operators can call `Write` many times to append pieces of state. For example, an `Average<double>` operator will likely call `Write<double>` and `Write<int>` to write a sum and a count. The notion of child writers is an advanced concept.

Prior to calling `SaveState`, a query engine may first traverse an operator tree to check if any state has changed since a previous checkpoint took place. That's where `StateChanged` and `OnStateSaved` come in. The basic lifecycle of a checkpoint is as follows:

1. Pause event processing, which will ensure that operator state can't mutate. This is done through the scheduler.
2. Traverse each subscription to check `StateChanged`. If any operator has changed state, the containing subscription has to be checkpointed.
3. For each dirty subscription detected in the previous step, traverse the subscription and call `SaveState` to persist the state.
4. Start committing the saved state of all dirty subscriptions to a reliable store. This is an asynchronous operation.
5. Resume event processing. This is done through the scheduler.
6. Await the completion of the state persistence commit operation kicked off in step 4.
7. If the commit was successful, visit the dirty subscriptions and call `OnStateSaved` which enables them to unmark dirty bits.

Note that pausing of event processing is kept to a minimum between steps 1 and 5, by resuming the flow of events prior to a successful commit of the state to a persistent store. Therefore, the tricky part is in the dirty flag kept behind the `StateChanged` property. In case the checkpoint commit fails, we need to keep dirty flags as-is, so a future checkpoint attempt can still find all of the dirty state to persist. In case the checkpoint succeeds, we need to unmark the dirty flags, but only if no additional changes were made after resuming event processing. See the `StateChangedManager` struct in `Reaqtive.Core` for a utility that helps to keep track of state changes.

Loading state is the opposite operation and uses a symmetric `IOperatorStateReader` interface:

```csharp
/// <summary>
/// Represents an operator state reader.
/// </summary>
public interface IOperatorStateReader : IDisposable
{
    T Read<T>();

    bool TryRead<T>(out T value);

    void Reset();

    IOperatorStateReader CreateChild();
}
```

It's a bit more advanced by virtue of having `TryRead` and `Reset` which were introduced to deal with state changes, allowing for an operator to peek at operator state and potentially reset the read position back to the start if a certain set of reads fail.

> **Note:** A query engine typically plays various tricks on readers and writers to isolate them from neighboring operators. For example, a save operation will persist the operator name and version, leave space for a size value, and then append the operator state by calling `SaveState`. The size value can then get backpatched, so the span containing the operator state can be reconstructed when recovering state. During a load, a stream segment over the operator's state is constructed, which ensures that `[Try]Read<T>` operations can't read past the end of the operator state.

Other operator interfaces include `IUnaryOperator` (which optimizes for common operators which have a single source), `IUnloadableOperator` (which support an `Unload` operation to unload underlying resources in case a hosting query engine gets unloaded from memory without taking down the process), etc.

## Subjects

Besides observables, observers, and subscriptions, Rx also has a notion of subjects. The Reaqtive library provides a similar concept in the form of an `IMultiSubject<TInput, TOutput>`:

```csharp
public interface IMultiSubject<TInput, TOutput> : ISubscribable<TOutput>, IDisposable
{
    IObserver<TInput> CreateObserver();
}
```

The key difference with Rx is that a multi subject *is* a subscribable but *has* zero or more observers used to push events into the subject. This difference enables defining different policies to deal with multiple publishers and multiple consumers. The way to think of this is that `ISubscribable<T>` is a *subscription factory*, while a subject is also an *observer factory*. That is, hot artifacts are instantiated through factories.

An additional non-generic interface is provided which supports a more flexible approach to typing. This `IMultiSubject` interface is sketched below:

```csharp
public interface IMultiSubject : IDisposable
{
    IObserver<T> GetObserver<T>();
    ISubscribable<T> GetObservable<T>();
}
```

> **Note:** The change in shape between both interfaces is a historical remnant. Future work may consolidate these abstractions.

The idea of a non-generic multi-subject is that the types of events are supplied in usage "binding" sites rather than subject "definition" sites. That is, rather than a subject dictating the shape of an event, observers and observables can supply a compatible type. This is particularly useful when events are versioned and publishers may start producing events of later versions (e.g. with additional fields or properties), while long-running queries have been bound to the observable side with an earlier version of the event type. When a query engine binds an `IObserver<T>` or an `ISubscribable<T>` to a subject, it first looks for an implementation of a strongly typed subject. If that's not found, it tries to bind to a `Get[Observer|Observable]<T>()` method on an untyped subject. The subject itself is then responsible to analyze whether the requested type is compatible with the subject instance, e.g. by matching it against some schema.

> **Note:** Alternative options have been considered, including building a graph of interconnected subjects using `Select` operators to convert events from one shape to another. However, even if that's the execution strategy, a policy component is needed to decide on such conversions. That logic tends to be well encapsulated in subjects, for example when such subjects are proxies to external streams that carry metadata about the event schema. So, rather than having a runtime binding dialogue with a subject to discover conversions and manage a graph of subjects and `Select` operators to perform transformations, we delegate all responsibilities for conversions to the subject itself. This allows it to build efficient routing tables that connect publishing observers to consuming observables, providing the necessary conversions (and possibly reusing conversion logic across different types, e.g. some version 2 of an event may simply be a wrapper around a version 1 event instance, allowing for object reuse).

## Schedulers

Schedulers form the lowest layer of event processing and control ordering and time. In the context of Reaqtive, schedulers are even more essential than in Rx, because we need the ability to pause and resume event processing in order to be able to take consistent state snapshots. To support this, Reaqtive defines the concept of *logical schedulers* which can be thought of as groups of tasks that can be paused, resumed, and disposed together. (In a way, logical schedulers are the temporal equivalent to the spatial concept of arenas used to manage memory.) The essential `IScheduler` interface is shown below:

```csharp
public interface IScheduler : IDisposable
{
    DateTimeOffset Now { get; }

    IScheduler CreateChildScheduler();

    void Schedule(ISchedulerTask task);
    void Schedule(TimeSpan dueTime, ISchedulerTask task);
    void Schedule(DateTimeOffset dueTime, ISchedulerTask task);

    Task PauseAsync();
    void Continue();

    void RecalculatePriority();

    bool CheckAccess();
    void VerifyAccess();

    event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;
}
```

> **Note:** Further factoring of the interface has been considered, including `IClock`, `IPausable`, `IThreadAffine`, and `ISchedulerExceptionHandling`. These changes have not yet materialized.

There are roughly 8 groups of concepts:

* `Now` is used to obtain the current time. This can correspond to wall clock time or be obtained from some virtual time source, e.g. when replaying persisted events that have application time.
* `CreateChildScheduler` enables to create a hierarchy of schedulers. Each child scheduler represents a task group and can be paused, resumes, and disposed. Applying these operations to a parent cascades down to all children.
* `Schedule` supports scheduling of tasks (discussed below), either to run as soon as possible, or at a specified relative or absolute due time.
* `PauseAsync` and `Continue` are used to pause and resume all the tasks in the scheduler (and any child schedulers). This can be used for checkpointing or other management operations.
* `Dispose` disposes the scheduler, all of its tasks, and recursively all of its children.
* `RecalculatePriority` allows to reevaluate the scheduler queue after scheduling more work that has different priorities. It's an advanced concept.
* `[Check|Verify]Access` are used to detect whether code is running on the scheduler already, which allows for inlining of work. This is also used to consume events from an external facility and *context switch* them onto the scheduler.
* `UnhandledException` provides an event handler that can be used to handle exceptions thrown by work running on the scheduler. Such exceptions can be marked as handled through the event arguments.

> **Note:** The `Schedule` methods do not return an `IDisposable` as is the case in Rx. It was found that fine-grained disposal of tasks is rather uncommon and is often paired with disposal of related tasks. By supporting `IDisposable` at the scheduler level and having the ability to create child schedulers, one can create groups of tasks that get disposed together. In addition, one can use flags to suppress late invocations of tasks that are no longer relevant, which is a feasible approach for tasks that are due soon. Similarly, we've cut a direct notion of recursive schedulers; the same effect can be achieved by scheduling more work on the same scheduler, for example through the `Execute` method on `ISchedulerTask`.

The related `ISchedulerTask` is typically not used directly, but gets used through utilities in `Reaqtive.Core`, such as `ActionTask`. We show the interface below to complete the story:

```csharp
public interface ISchedulerTask
{
    long Priority { get; }

    bool IsRunnable { get; }

    bool Execute(IScheduler scheduler);

    void RecalculatePriority();
}
```

Tasks due at the same time (or that are due immediately) have an order established by `Priority`. A priority can be changed by a scheduler task, for example in response to receiving an external event, and scheduler queues can be recalculated to reflect the new priority. Furthermore, tasks has an `IsRunnable` state that reflects whether a task has work to do. All tasks support periodic execution and `IsRunnable` is used to indicate that a task is ready to be run again. The `Execute` method is used to invoke the task, and return whether the task is completed (`true`) or needs to be run again to do more work (`false`), which enables interleaving of work. Single-shot tasks are simply periodic tasks that are only runnable once and have their `Execute` method return `true` after the first and only invocation.

A more advanced version of `ISchedulerTask` is `IYielableSchedulerTask` which is used to provide a more cooperative approach to scheduling, allowing tasks to spontaneously yield at their earliest convenience when requested to do so. This is used by schedulers when a pause is requested, and can be used to interrupt workers in a graceful cooperative manner. For example, when a worker is receiving events from some external queue and draining them on the scheduler (for example into and observer that feeds into a subject), it may have a busy inner loop in `Execute`. While this loop should yield periodically in order to let other work on the scheduler run, it can also check for yield requests to bail out quicker if such a request is made.

```csharp
public interface IYieldableSchedulerTask : ISchedulerTask
{
    bool Execute(IScheduler scheduler, YieldToken yieldToken);
}
```

`YieldToken` is very similar to `CancellationToken` but it's not a one-way street. That is, a yield token's `IsYieldRequested` can return `true` at one time and return `false` at a later time (i.e. it can be reset, unlike cancellation).

> **Note:** Yield support can be extended to allow other tasks to preempt running tasks, either due to priority calculations or based on elsapsed time. It is and remains a cooperative mechanism though, so tasks should be written to honor yield tokens. It is recommend that all tasks that have loops in them support yielding.
