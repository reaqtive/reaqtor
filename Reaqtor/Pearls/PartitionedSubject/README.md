# PartitionedSubject

Prototype of partitioned subjects which were implemented in the Nuqleon query engine.

## History

The need for (automatic) partitioning on streams became apparent as high-volume streams were being consumed using simple `Where(x => x.Key == value)` query clauses for initial filtering. Query rewrite mechanisms have been used across Nuqleon and services based on it, including query execution planning, as well as within query engines (see `Nuqleon.Reactive.QueryEngine` for more details).

This project was an initial example to illustrate the rewrite from a `Where` clause to a special type of subject for use within a query engine. It omits the expression tree rewriting steps needed to perform a translation from a `Where` clause to a key-based subscription to a partition in a subject, but focuses on the `PartitionedSubject<T, K>` concept underneath.

```csharp
var sub = new PartitionedSubject<Weather, string>(w => w.City, EqualityComparer<string>.Default);

// Subscribe (takes a key)
var d = sub.Subscribe("Redmond", Observer.Create<Weather>(w => { /* handle events within partition */ }));

// Publish (same as before)
sub.OnNext(new Weather { City = "Redmond" });
```

In here, the subscription is the result of rewriting a query like:

```csharp
sub.Where(w => w.City == "Redmond").Subscribe(o)
```

## Query execution planning

Global support for partioned streams, in addition to local support within query engines (to reduce computational code), is built around the following ideas.

First, stream factories can support metadata or operators applied on `IAsyncReactiveQubjectFactory`-related types in order to express the partitioning intent. This way, a single stream can be created (e.g. `contoso://weather`), with multiple partition schemes applied to it. Users can still publish into a single stream or subscribe to a single stream in a query expression, but partitions are used under the hood. For example, a weather stream could have partitions expressed as `PartitionBy(w => w.City)` or `PartitionBy(w => w.Region)`. Having more than one partition results in event publications to the stream being redirected to multiple partitions (i.e. a multicast operation), which can either happen in a service front-end or all the way down in the client, because partition selectors such as `w => w.City` can be shipped to the publisher (i.e. shipping on Bonsai expressions retrieved from metadata to the event publication front-end service or into the publishing reactive client). That is, given an event of type `Weather`, all registered partition selectors are applied to the event to get partition keys. These keys are then used to publish to the right partition.

> **Note:** Automatic partitioning can also be achieved using this mechanism by using a consistent hashing of the event payload and a `%` operation to pick a partition. It's really modeled underneath as a partition selector, and the user does not see it on either the publication or subscription path. This is contrary to having to write `contoso://weather_by_city("Seattle")` or `contoso://weather_partitioned(magic_hash_value)`.

Multiple types of partitions are possible, including value-based ones (e.g. `City` equals `Seattle`), but also range-based ones (e.g. `Delay` greater than `30s`). For range-based partitions, intervals can be specified. For value-based partitions, values in the domain can (optionally) be specified (e.g. when there's a finite set, much like an enum value).

Second, queries written over the stream are normalized in order to reason over binding to partitions. A few examples:

```csharp
weather.Where(w => w.City == "Seattle")
```

is the normal form for various ways to express the same intent:

- `"Seattle" == w.City`
- `string.Equals(w.City, "Seattle")`
- etc.

and

```csharp
weather.Where(w => w.City == "Seattle" && w.Temperature < 20)
```

gets normalized as

```csharp
weather.Where(w => w.City == "Seattle")
       .Where(w => w.Temperature < 20)
```

and

```csharp
weather.Where(w => w.City == "Seattle" || w.City == "Bellevue")
```

gets normalized as

```csharp
Merge(
    weather.Where(w => w.City == "Seattle"),
    weather.Where(w => w.City == "Bellevue")
)
```

All of these rewrites reveal simpler predicates, which may be equality based or comparison based, which can subsequently be matched against partitions available for the bound `weather` stream. This partitioning information is retrieved from the stream's metadata.

> **Note:** If a stream happens to be automatically partitioned into `N` partitions, the same mechanism would figure out that a raw subscription to the entire stream is really performing a subscription to the result of `Merge` applied to all `N` underlying partitions. This can further be decomposed into a distributed query plan by pushing down operations over the merge (e.g. `Where` and `Select` can be pushed over a `Merge` to its sources).

Once partitioning information is obtained, rewrites can take place to use specific partitions. For example:

```csharp
weather.Where(w => w.City == "Seattle")
```

may be bound as

```csharp
weather_by_city("Seattle")
```

which is a stream holding values published for the city of `Seattle`, or even

```csharp
weather_by_city__Seattle
```

if the domain of cities was finite and specified upon the creation of the stream (this is more typical for enum-based values).

As an other example, consider a query like:

```csharp
weather.Where(w => w.Temperature < 20)
```

which may be bound using range partitions, potentially resulting in residiual filters. Assume that the specified partition ranges are `[int.MinValue, 32]` and `[33, int.MaxValue]` (as specified during stream creation). The query rewriter can then turn the query expression into:

```csharp
weather_by_temp__le_32.Where(w => w.Temperature < 20)
```

Deciding which partition to bind to is a task of the query planner, which can perform static analysis (e.g. knowing the domain of `int` and the size of intervals, and similar for other types that are comparable), or consult runtime statistics to know event volumes per partition. This often involves a ranking algorithm to evaluate different possible plans.

Finally, if no predicate is found that enables binding to one or more partitions, it may be possible to generate a `Merge` between all known partitions, and then perform further query rewriting to avoid having a single query engine that receives all events from all partitions (e.g. by performing predicate pushdown).

> **Note:** Merging all partitions may be something that can be inferred statically if there exists a partition with a finite number of values (or disjoint ranges that union to the whole domain). However, if partition keys are dynamically discovered, this involves the use of a meta-stream to indicate the creation of new partitions upon publication of events with a new unique partition key. For example, if a weather stream just has a `City` selector of type `string`, a partition for a city only pops up after the first publication of an event for that city. Meta-streams are then used to indicate the creation of such a partition stream (i.e. an `IObservable<Uri>` where the `Uri` represents the unique identifier for the newly created stream; this meta-stream is typically backed by a `ReplaySubject` so new subscriptions can discover all existing partitions - and receive events for newly added partitions - by subscribing to the meta-stream). To consume all streams, a construction like `meta.SelectMany(uri => ctx.GetObservable<T>(uri))` is used, and subsequent query clauses can be pushed down onto the inner lambda. Query planning for `SelectMany` then can distribute the inner subscriptions (i.e. per-partition queries) across query engines.
