# `DelegatingBinder`

Prototype of delegation support to push down query operations into subjects within query engines.

## History

Delegation is the general notion of supporting osmosis of query operators through artifact or service boundaries. For example, a queryable source may or may not support direct execution of various query operators. If it does, such query operators can be delegated into the source, while others stay behind. Consider a generalized example:

```csharp
o.Foo(args1).Bar(args2).Execute()
```

where `Execute` triggers execution of the intent expressed to the left. In the context of LINQ, `Execute` may be `GetEnumerator` or `Subscribe`, where `Foo` and `Bar` would be query operators, and `o` would be an enumerable or observable source. Furthermore, LINQ codifies the ability to execute a query by means of a provider through interfaces such as `IQueryable<T>` or `IQbservable<T>`. However, that provides an all-or-nothing situation where query providers get to see the whole intent and then have to "cut off" remote execution at some point. For example:

```csharp
xs.Where(x => x > 0).Select(x => x * 2).GetEnumerator()
```

Assume that `Where` can be executed by the provider, but `Select` can't. In that case, the query provider could rewrite the expression to be similar to:

```csharp
xs.Where(x => x > 0).AsEnumerable().Select(x => x * 2).GetEnumerator()
```

This involves rewriting `Queryable.Select` to `Enumerable.Select`. One way to avoid having to do such rewrites is by using `AsQueryable`, which uses the LINQ to Objects query provider that does these rewrites internally:

```csharp
xs.Where(x => x > 0).AsEnumerable().AsQueryable().Select(x => x * 2).GetEnumerator()
```

In a way, this has put in a barrier in the form of `.AsEnumerable().AsQueryable()` to separate remote from local.

## Mechanism

The concept of delegation is an automated mechanism whereby an execution engine tries to bind a query expression against one or more sources by having a dialogue with them on what they're willing (or able) to execute. Let's look back at the initial generalized sample:

```csharp
o.Foo(args1).Bar(args2).Execute()
```

Without delegation, this is akin to writing:

```csharp
o.ToLocal().Foo(args1).Bar(args2).Execute()
```

where `ToLocal` is a means to force the remainder (right hand side) of the expression to be evaluated "locally" as opposed to being delegated into the source (left hand side, i.e. `o`). Delegation performs a dialogue with `o` by checking if supports delegation (e.g. by implementing some interface such as `IDelegationTarget`). If it does, subexpressions are given to the delegation target to check whether they can be accepted by the target. For example:

```csharp
@this.Foo(args1)
```

where `@this` is a hole in the expression representing the delegation target itself. If the delegation target is willing to take this on for direct execution (e.g. because it can translate the operation to some target language, or because it has internal optimizations), the expression gets rewritten like this:

```csharp
o.Foo(args1).ToLocal().Bar(args2).Execute()
```

That is, `ToLocal` is moving to the right, and the whole process starts again for the next operation. We're effectively having a form of osmosis of operations through the `ToLocal` membrane.

## Usage

Delegation is a general concept that can work on local objects but also across service boundaries. In Nuqleon, local delegation is implemented in the query engine, to support things like partitioned subjects. In services built on top of Nuqleon, the `IReactiveMetadata` has been used to query remote services for capabilities in order to figure out which portions of a query expression to delegate. For example, in a cloud-edge scenario, you may have a reactive query that performs some type of join over sensor data and cloud data:

```csharp
user.Geolocation.SkipUntil(startTime).TakeUntil(endTime).Sample(interval).Where(geofence)
```

and

```csharp
cloud.Traffic(userLocation, destinationLocation)
```

Assume both are combined in a much bigger query expression that involves a `SelectMany` or `Switch` to combine both "legs" of the query. The entire query expression ends up in a Nuqleon-based cloud service where the naive execution could be to stream all of the geolocation data from the user to the cloud all the time, only to trim it to a time interval, sample it, and filter it based on some geofence. That's a lot of wasteful bandwidth usage, a battery drain, and potentially a privacy concern. Instead, we'd like to delegate as much of the operations applied to the user's geolocation event stream to the source (e.g. a phone) as possible. Using an `IReactiveMetadata` discovered from binding `user.Geolocation`, we can query for supported query operators (through the `Observables` queryable dictionary). The implementation of this interface for devices is typically based on a digital twin in the cloud that mirrors the device's capabilities (including the observable "signals", query operations, and observer "actions").
