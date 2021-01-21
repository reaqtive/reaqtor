# `Reaqtive.Scheduler`

Provides a scheduler used to run reactive event processing queries with notions of time and support for pause and resume.

## Basic concepts

This library provides scheduling infrastructure similar to Rx's `IScheduler`. As such, it provides management for concurrent execution of tasks, including support for time-based scheduling. All query operators leverage schedulers to process events and to schedule actions to run at specified times (e.g. to implement operators such as `Delay`, `Sample`, etc.). This layering also enables execution of event processing queries in virtual time, which is useful for testing and to process historical events (e.g. from a log).

The key abstraction is `IScheduler`, which models a *logical scheduler*. Query operators use this interface to schedule work, which is modeled as `ISchedulerTask` objects passed to `Schedule` methods. Host infrastructure, such as query engines, use this interface to pause and resume event processing through `PauseAsync` and `Continue` methods. This is useful to take consistent snapshots of query operator state during checkpoints. For more information on these interfaces, see the `Reaqtive.Interfaces` project. For more information on implementations of tasks using `Action` delegates, see the `Reaqtive.Core` project.

> **Note:** The `IScheduler` interface can be implemented over different sources of concurrency, akin to Rx's schedulers for threads, thread pools, task pools, event loops, UI frameworks, etc. This would work well and enable the use of the `Reaqtive.Linq` library in a way similar to classic Rx. However, the design of this library is centered around the notion of a *physical scheduler* that provides the underlying infrastructure where tasks are run. At this point, there's no interface definition for the *physical scheduler* construct, and only one implementation is provided out of the box. The reason for this design is historical due to the use of this library by query engines which have specific threading requirements.

Logical schedulers can be thought of as managing groups of tasks that can be paused and resumed together. Furthermore, logical scheduler form a tree-based hierarchy; that is, a logical scheduler can have any number of child schedulers. Pause and resume operations applied on parent schedulers recursively apply to child schedulers and their tasks. Logical schedulers can also be disposed, which enables cancellation of groups of tasks all at once. This is particularly useful for host infrastructure, e.g. when a query engine is unloaded from memory.

An implementation of `IScheduler` provided by this library is `LogicalScheduler`. It's layered on top of a *physical scheduler* implementation that provides a statically sized thread pool. The type implementing the physical scheduler is apropriately named as `PhysicalScheduler`.

## `PhysicalScheduler`

Physical schedulers own a set of threads on which scheduled tasks will get run. It is common for a host process to have a single instance of a `PhysicalScheduler`, with multiple `LogicalScheduler` instances on top of it. For example, when a single host process runs multiple query engine replicas that can fail over independently (e.g. due to failure of another node in the cluster, more replicas may be moved over, or due to rebalancing, replicas may be moved out), each of these would have its own `LogicalScheduler` that deposits work into the single shared `PhysicalScheduler`.

To create a `PhysicalScheduler`, one can use the `Create` method with an optional `int` parameter to control the number of threads. The default overload uses `Environment.ProcessorCount` to determine the number of threads. Note that the number of worker threads is static; query operators should be written to avoid blocking operations that would cause the scheduler threads to stop processing work.

```csharp
var phy = PhysicalScheduler.Create();
```

The physical scheduler does not have public APIs other than the `IDisposable` implementation used for graceful shutdown. All scheduling of work is mediated through `LogicalScheduler` instances, as discussed below.

> **Note:** At this point, the library does not provide support to control the thread creation, e.g. to control thread priorities.

## `LogicalScheduler`

Logical schedulers implement `IScheduler` and are instantiated by providing an underlying `PhysicalScheduler` used to execute the tasks. Once a logical scheduler has been constructed, work can be scheduled using the `Schedule` methods, the scheduler can be paused and resumed, and child schedulers can be created.

```csharp
var sch = new LogicalScheduler(phy);

sch.Schedule(new ActionTask(() => { /* task 1 */ }));
sch.Schedule(TimeSpan.FromSeconds(5), new ActionTask(() => { /* task 2 */ }));
sch.Schedule(DateTimeOffset.UtcNow.AddMinutes(5), new ActionTask(() => { /* task 3 */ }));

await sch.PauseAsync();

// E.g. take a checkpoint of query operator state

sch.Continue();
```

Creation of child schedulers is often used to create task groups which can get disposed together. It also allows for creating computational hierarchies with support for pausing and resuming of computations.

Other members on `LogicalScheduler` include:

* `Status` to query the current status of a scheduler, including `Running`, `Pausing`, `Paused`, and `Disposed`.
* `UnhandledException` to observe and potentially handle unhandled exceptions that were thrown by tasks executing on the scheduler. This acts as a global exception handler for tasks.
* `QueryPerformanceCounters` to obtain a set of performance counters to assess the efficiency of the scheduler, including the number of tasks, timer ticks, the paused duration, time spent in scheduling infrastructure, etc.
