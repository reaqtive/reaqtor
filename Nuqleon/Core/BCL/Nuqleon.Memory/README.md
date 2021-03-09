# Nuqleon.Memory

Provides object pools, function memoization, and caching utilities.

## Tuplet<...>

A value-type implementation of `System.Tuple<...>` with support for `IStructuralEquatable` and `IStructuralComparable`. The types are immutable.

```csharp
var t = new Tuplet<int, bool, double>(1, true, 3.14);
Console.WriteLine(t.Item1);
Console.WriteLine(t.Item2);
Console.WriteLine(t.Item3);
```

> **Note:** These types predate the introduction of `System.ValueType<...>` in .NET, which can provide a useful alternative.

## WeakReferenceExtensions

Provides a set of methods to work with `WeakReference<T>` objects. For example, supports creating `WeakReference<T>` objects that contain a `null` reference, and adds a `GetOrSetTarget` method uses a function to get or set the target in a way similar to `Lazy<T>`.

```csharp
WeakReference<string> w = WeakReferenceExtensions.Create<string>(null);

string s1 = w.GetTarget();
Console.WriteLine(s1 == null);

string s2 = w.GetOrSetTarget(() => DateTime.Now.ToString());
Console.WriteLine(s2);
```

## Object Pools

### ObjectPoolBase<T>

Provides a base class for object pools. It provides two symmetric methods:

* `T Allocate()` to allocate an object from the pool.
* `void Free(T)` to return an object to the pool.

To make freeing objects easier, it also provides a `New` method which returns a `PooledObject<T>` implementing `IDisposable`, so a `using` statement can be used to allocate and free an object.

When objects get returned to a pool, and they implement an `IClearable` interface, a call to `Clear` is made to restore the object to a state for future reuse.

### ObjectPool<T>

Provides an implementation of an object pool using a `Func<T>` factory method to construct new instances of pooled objects. A pool also has a size of the number of objects to keep in the pool.

```csharp
var pool = new ObjectPool<MyObject>(() => new MyObject(), size: 64);

MyObject obj1 = pool.Allocate();
Use(obj1);
pool.Free(obj1);

using (PooledObject<MyObject> obj2 = pool.New())
{
    Use(obj2.Object);
}
```

### PooledXYZ types

Specialized object pool implementations are provided for various .NET types:

* `PooledMemoryStream`
* `PooledStringBuilder`
* `PooledDictionary<TKey, TValue>`
* `PooledHashSet<T>`
* `PooledLinkedList<T>`
* `PooledList<T>`
* `PooledQueue<T>`
* `PooledStackT>`

For each such type `PooledXYZ`, a number of related types are provided:

* `PooledXYZ` is the pooled equivalent of `XYZ`.
* `XYZPool` is a pool for instances of `PooledXYZ`.
* `PooledXYZHolder` is returned from `New` on the pool.

For example:

```csharp
// Create a pool for 64 List<int> instances, passing a capacity of 1024 to the List<int> constructor.
var pool = ListPool<int>.Create(size: 64, capacity: 1024);

// Get a list from the pool. PooledList<T> inherits from List<T>.
PooledList<int> xs = pool.Allocate();
xs.Add(42);

// Return the list to the pool; it will get cleared. Don't use xs anymore.
pool.Free(xs);

// Use disposable helper.
using (PooledListHolder<T> holder = pool.New())
{
    List<int> xs = holder.List;
    xs.Add(42);
}
```

## Function Memoization

Memoization is a technique to transform a pure function into a compatible function which caches the result of evaluating the original function in order to speed up repeated invocations. Two forms of memoization are supported:

* Regular memoization, where a `Func<T, R>` (or a function of a higher arity) has a cache that maps values of type `T` to type `R`, with the risk of keeping instances of `T` rooted.
* Weak memoization, which only works for delegates where all arguments are constrained as `class`. The underlying cache uses weak references to avoid keepng instances of `T` rooted.

In order to use memoization, one first creates a memoizer using factory methods on `Memoizer`. These factory methods accept a cache factory and return an instance of type `IMemoizer` or `IWeakMemoizer`. Memoization cache factories are used to specify the policy for caching of function results. For example:

```csharp
IMemoizationCacheFactory factory = MemoizationCacheFactory.CreateLru(16);
IMemoizer memoizer = Memoizer.Create(factory);
```

In this example, we use a least recently used (LRU) eviction strategy for the memoization caches that get created when memoizing functions. Other policies can be specified as well, including:

* `Unbounded` for an unbounded cache.
* `Nop` for no cache (useful for testing).
* `CreateEvictedByLowest` and `CreateEvictedByHighest`, specifying a metric to rank entries by:
  * `CreationTime` - can be used to evict based on the initial invocation;
  * `InvokeDuration` - how long it took to invoke the function (e.g. to cache the expensive ones);
  * `AverageAccessTime` - how long it takes to locate the item in the cache (e.g. due to `IEqualityComparer<T>` costs);
  * `HitCount` - how often an entry is retrieved;
  * `LastAccessTime` - can be used to mimic LRU;
  * `SpeedupFactor` - a ratio of invocation time versus lookup time (e.g. to only keep entries that have a good speedup).

Once a memoizer instance is obtained, one can memoize functions using the `Memoize` method and various extension methods for higher arity functions. For example:

 ```csharp
 IMemoizedDelegate<Func<string, string>> toUpper = memoizer.Memoize(s => s.ToUpper());
 ```

Additional parameters can be specified to control caching behavior (e.g. whether or not to cache exceptions as well) and to specify an `IEqualityComparer<T>` used for key comparisons.

The resulting `IMemoizedDelegate<TDelegate>` provides access to:
 
 * the cache through a `Cache` property that can be used to inspect the cache, to clear it, etc.;
 * the memoized delegate through a `Delegate` property.

In order to invoke the memoized function, simply use the `Delegate` returned through the `IMemoizedDelegate<TDelegate>`. For example:

```csharp
string foo = toUpper.Delegate("foo");
```

Subsequent invocations with the same argument(s) are subject to cache lookups.