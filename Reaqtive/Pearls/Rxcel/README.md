# `Rxcel`

An Rx-based implementation of a miniature Excel to demonstrate the concept of "reactive graphs".

## History

This was an early example to demonstrate "reactive graphs" where nodes are subjects and edges are computations. For example, an entity whose value can change over time can be represented as a `BehaviorSubject<T>`. Computations can then observe changes to the entity's values in order to recompute other dependent entity values.

Using Excel where cells are modeled subjects and computations are derived from user-supplied formulae, the reactive graph consists of `CombineLatest` operators that lift the formula over cells. For example, when editing cell `C1` with formula `=A1+B1`, this really turns into a `A1.CombineLatest(B1, (a1, b1) => a1 + b1).Subscribe(C1)` computation, where the user's formula was lifted from `int` to `IObservable<int>`. The destination cell is used as an observer, the source cells are used as an observable, and get combined using `CombineLatest`.

An alternative way of writing `CombineLatest` could be lift, to make this more clear:

```csharp
Func<IObservable<T1>, IObservable<T2>, IObservable<R>> Lift<T1, T2, R>(this Func<T1, T2, R> f)
{
    return (IObservable<T1> xs, IObservable<T2> ys) => Observable.CombineLatest(xs, ys, f);
}
```

This sample was later rewritten using `IReactiveProxy` to build a persisted Excel in the cloud:

* Creation of a new worksheet corresponds to stream creation operations for the cells. E.g. `excel://bart/demo/sheet/cell/A1`.
* Editing of a formula corresponds to subscription creation operations to connect the source and destination cells (using `CombineLatest`). E.g. `excel://bart/demo/sheet/formula/C1`.
* Putting a value in a cell corresponds to deleting a subscription for the cell, if any exists (using the identifier as shown above), and then performing an `OnNextAsync` into the cell's stream.
* A local UI subscribes to all the cells using a mechanism that can funnel data back to the UI, e.g. using a `WebSocketObserver`. Because streams are "behavior subjects", the latest value is received immediately.
