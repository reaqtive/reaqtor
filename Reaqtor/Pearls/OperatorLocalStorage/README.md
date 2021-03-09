# OperatorLocalStorage

Fine-grained state management for query operators running in a checkpointing query engine.

## History

The Nuqleon query engine with checkpointing persists the state of stateful artifacts, such as subscriptions, as small blobs by using the `SaveState` method on `IOperator`, which gets invoked during a visit to the artifact's operator tree. As a result, the state of all operators in an artifact gets appended into a single blob that's stored in a key/value pair for the artifact (where the key corresponds to the artifact's identifier). This is generally okay and fairly small, for example:

```csharp
xs.Take(5).Average()
```

will have state for `xs`, which may include a sequence id, state for `Take`, which will contain a single integer with the number of remaining values, and `Average`, which will contain a sum and a count value. Whenever an event in `xs` is received, the state of `Take` and `Average` will also get dirtied, so it is necessary to re-persist the state of each operator.

For more complex query operators, artifacts get split into smaller units, which are checkpointed individually. A good example is the use of `SelectMany` where each inner subscription is persisted separately. However, the `SelectMany` operator itself (in the "parent" subscription) still needs to keep a list of `Uri` values to refer to these inner subscriptions. For example:

```csharp
xs.SelectMany(x => ys(x).Where(f).Take(5)).Average()
```

The top-level query consists of `SelectMany` and `Average`, which will persist a list of inner subscriptions and a sum and count, respectively. Whenever `xs` receives an event, `SelectMany` creates a new inner subscription and dirties its list of inner subscriptions by adding the `Uri` for the inner subscription. The inner subscription `ys(x).Where(f).Take(5)` is then persisted separately. Therefore, if the state of one inner subscription gets dirty, it doesn't cause any of its sibling inner subscriptions or its parent subscription (the one with `SelectMany`) to become dirty as well. This effectively saves on checkpointing cost (and the amount of I/O required); the persisted state is less proportional to the number of events produced on a sequence. Note though that `SelectMany` still has a list of inner subscription `Uri` values, which is proportional in the volume of events it received.

Other operators that can decompose subscriptions into smaller constituents include `Window`, `GroupBy`, `SkipUntil`, `Concat`, etc.

However, some query operators still suffer from two potential issues that may cause state size bloat:

* State that's proportional in the number of events received.
* State that remains unchanged even if the containng operator gets dirtied.

For the first example, consider a query operator like `Buffer` which has to keep a `List<T>` to store the events in the buffer, before sending them out to the downstream observer. The size of these lists is dependent on the event count specified on the `Buffer` operator, or may be arbitrarily big if a `TimeSpan`-based overload is used. Furthermore, every checkpoint has to repersist the entire list if at least one element was added to it. For example, consider that an event is received every 29.9 seconds, and a buffer size of 100 is specified. If a checkpoint happens once a minute, 2 new events have been appended each time. For the first checkpoint, we persist 2 events. For the second checkpoint, we persist 4 events, of which the first 2 remain unchanged. One hour in, at checkpoint 60, we're persisting 120 events of which 118 were persisted before. The problem gets worse because buffers can overlap, so there may be many such buffers in progress of being populated, each of them requiring persistence. Other operators that suffer from unbounded state growth include `Zip`, `SelectMany` (for its inner subscription list), `GroupBy`, `Window` (for their inner subject lists), etc.

For the second example, consider the behavior of `CombineLatest` where the latest event value for all of the input sequences gets stored. Every time any of the input sequences produces a value, the state of `CombineLatest` gets dirtied. However, only 1 piece of state changed, namely the latest event on any of the input sequences. The remainder `n - 1` pieces of state, for the latest event on the other sequences, did not change. Because the entire `CombineLatest` operator is marked as dirty, these events get repersisted as well in the next checkpoint.

Finally, it's important to appreciate that an entire subscription, including all of its operators, get persisted as long as any of its operators is marked as dirty. That is, the dirty flags are `|`-ed together by a visitor. In many cases, a single operator becoming dirty due to processing of a new event does not necessary result in adjacent operators becoming dirty as well. For example, `xs.Take(n).Where(f).Take(m)` has state for both `Take` operators. However, the latter one may not get dirtied even if the first one did, because of the filter in between.

Operator local storage is a mechanism to make state more fine-grained, by providing first-class representations of values and collections. These form a so-called *object space* where each object has a type and has a unique identifier. Query operators can allocate these persisted objects and save their unique identifier in their own state (effectively acting as a *persisted pointer*). Objects in object spaces use a fine-grained state persistence mechanism that exploits the characteristics of the type. For example, when persisting a queue, we can persist the individual values as key/value entries in the store, and keep additional persisted metadata for the head and the tail of the queue. When an element gets enqueued or dequeued, only one key/value slot for an element gets edited, in addition to one metadata update for the head/tail values.

> **Note:** Operator local storage was built and used for various stateful micro-compute runtimes, including the Nuqleon query engine. Therefore it did not get fused into the query engine, but was developed as a standalone pluggable component.

## Example

An example of using the standalone object spaces technology is shown below. First, an object space is created:

```csharp
var serializer = new SerializationFactory();
var state = new PersistedObjectSpace(serializer);
```

This requires the implementation of an `ISerializationFactory` which is used to serialize state. In the case of Nuqleon, the `Nuqleon.DataModel` is used in conjunction with JSON-based serialization, but other uses may want to pick a different strategy.

Next, objects can be allocated. As an example, consider creating an array with fixed length:

```csharp
IPersistedArray<int> array = state.CreateArray<int>("foo", 8);
```

During the next checkpoint, the array named `foo` will get persisted in some index table, in addition to metadata to keep track of the length of the array. Default values for all the slots will be persisted as well.

Next, edits can be made to the array, simply by assigning to slots. For other types such as lists, queues, stacks, etc. familiar mutator methods are provided (such as `Add`, `Enqueue`, `Push`, etc.). It's also worth noting that an object can be retrieved from the object space using a `Get` method.

```csharp
IPersistedArray<int> array = state.GetArray<int>("foo");
array[3] = 42;
array[5] = 43;
```

In here, any assignment to a slot in the array will result in just that slot being marked as dirty. Upon persisting the object space during a subsequent checkpoint, only the dirty slots will get written. Because arrays have a fixed length, no additional metadata is being mutated when a slot is written. (This is different for lists that can dynamically grow, or for queues that keep track of head and tail indexes.)

Finally, to persist an object space, an interaction similar to the Nuqleon query engine is performed. First, a state writer is allocated. Next, this writer gets passed to a `Save` method which collects all the dirty state and toggles additional flags to keep track of state that was persisted (so that subsequent edits to already-dirty state won't cause the dirty flag to be reset after a successful commit). After this, a call to `CommitAsync` is made to persist the state. Once successful, a call to `OnSaved` is made to reset dirty flags. These steps are shown below:

```csharp
var writer = GetWriter(store, CheckpointKind.Full);
state.Save(writer);
writer.CommitAsync().GetAwaiter().GetResult();
state.OnSaved();
```

Under the hood, the store will have the following tables with key/values pairs:

```
state/index
  foo = {"kind":"Array"}
  
state/item/foo/metadata
  length = 8

state/item/foo/items
  0 = 0
  1 = 0
  2 = 0
  3 = 42
  4 = 0
  5 = 43
  6 = 0
  7 = 0
```

The `state/index` table is used to keep track of the types of the objects, and to be able to enumerate the whole object space. One can compare the addition and removal of entries in this table with `new` and `delete` operations. The array itself is persisted in two tables, one for metadata (immutable for an array, simply keeping track of the length), and one for the items themselves (indexed by numbers representing the slot index in case of arrays).

When a differential checkpoint is taken, only the dirty slots get repersisted.

## Supported types

Object spaces support the following types:

* `IPersistedValue<T>`, a single slot of type `T`, similar to `StrongBox<T>` or `ValueTuple<T>`.
* `IPersistedArray<T>`, an array with fixed length and elements of type `T`, similar to `T[]`.
* `IPersistedList<T>`, a list with dynamic length and elements of type `T`, similar to `List<T>`.
* `IPersistedQueue<T>`, a queue with elements of type `T`, similar to `Queue<T>`.
* `IPersistedStack<T>`, a stack with elements of type `T`, similar to `Stack<T>`.
* `IPersistedLinkedList<T>`, a linked list with elements of type `T`, similar to `LinkedList<T>`.
* `IPersistedSet<T>`, a set with elements of type `T`, similar to `Set<T>`.
* `IPersistedSortedSet<T>`, a sorted set with elements of type `T`, similar to `SortedSet<T>`.
* `IPersistedDictionary<K, V>`, a dictionary with keys of type `K` and values of type `V`, similar to `Dictionary<K, V>`.
* `IPersistedSortedDictionary<K, V>`, a dictionary with keys of type `K` and values of type `V`, similar to `SortedDictionary<K, V>`.

Other types can be added as extensions to the base functionality, for example by combining the primitives above into more sophisticated types. A good example is a `Dictionary<T, List<V>>` which can be implemented as a `Dictionary<T, string>` where the values are names of `List<V>` objects in the object space.

> **Note:** A further prototype on top of object spaces was built to support sparse arrays and multi-dimensional arrays ("tensors"). These can benefit from specialized storage implementations to make various operations fast (e.g. a common operation on multi-dimensions arrays is to take a slice). The use of a key/value store underneath makes it harder to exploit various characteristics of the data.
