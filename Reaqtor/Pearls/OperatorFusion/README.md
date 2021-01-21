# `OperatorFusion`

Prototype of reactive operator fusion.

## History

LINQ query operators are traditionally written as chains of objects which implement either `IEnumerator<T>` or `IObserver<T>`. For example, a filter implementation for a `Where` operator will encapsulate an upstream enumerator or a downstream observer, and implement the filtering logic inside. In the case of pull-based queries (LINQ to Objects), its `MoveNext` implementation calls the source enumerator's `MoveNext` method and `Current` property to retrieve the next element, applies a filter, and then yields the result if the filter passes. In the case of push-based queries (LINQ to Events, aka Rx), its `OnNext` implementation gets called with a new event, applies a filter, and then calls the downstream observer's `OnNext` method if the filter passes.

Chains like this entirely consist of virtual method calls, which do have some overhead. Furthermore, the implementation of operators contains a lot of pointers, for example to a filtering predicate, and to the upstream enumerator or the downstream observer, which add to the memory cost. Finally, in the context of reliable implementation of such operators, additional state is often kept to track whether state is dirty and needs to be persisted in a checkpoint. However, such state is kept per operator, even though the persistence is often done for a chain of operators (i.e. the dirty flags are `|`-ed together during some scan over all operators in a chain).

One option to reduce the cost is to apply some query optimization that combines adjacent operators. For example, two consecutive `Where` operators can be combined into one, using `&&` to combine the predicates (`x => f(x) && g(x)`). Similarly, two consecutive `Select` operators can also be combined, by means of function composition (`x => g(f(x))`). However, mileage is limited. While this reduces the number of operator instances (and thus memory cost, as well as virtual function overhead), it only works for known patterns and still leaves a lot of overhead on the table (e.g. combining two delegates for selectors or filters does also allocate a closure object).

Another option is do perform true fusion of operators, which works across any set of different operators, as long as they are *fusable*. As an example, consider the blueprints of `Where`, `Select`, and `Take` operators, shown below:

```csharp
// Where
void OnNext(T x)
{
    if (_predicate(x))
    {
        _observer.OnNext(x);
    }
}

// Select
void OnNext(T x)
{
    _observer.OnNext(_selector(x));
}

// Take
void OnNext(T x)
{
    if (_remaining > 0)
    {
        _observer.OnNext(x);

        _remaining--;
        _dirty = true;
    }

    if (_remaining == 0)
    {
        _observer.OnCompleted();
    }
}
```

In here, we're omitting `OnError` and `OnCompleted` code, as well as `try...catch...` wrappers around calls to `_predicate` and `_selector`. All of the fields are also implied for simplicity sake. Now assume the user wrote `xs.Where(x => x > 0).Select(x => x * 2).Take(5)`. Operator fusion can create a combined `WhereSelectTake` operator on the fly by stitching the fragments together, and inlining predicates and selectors:

```csharp
// WhereSelectTake
void OnNext(T x)
{
    if (x > 0)
    {
        var res = x * 2;

        if (_remaining > 0)
        {
            _observer.OnNext(res);

            _remaining--;
            _dirty = true;
        }

        if (_remaining == 0)
        {
            _observer.OnCompleted();
        }
    }
}
```

There is no longer any need to keep pointers to delegates or downstream observer. Only one single fused operator gets allocated. All dirty flags can be coalesced into a single one as well (e.g. if the code above would have multiple uses of `Take`, which would also lead to different `_remaining` fields to be added for each operator).

> **Note:** This approach forms a trade-off between reusable generic operators with various CPU and memory overheads, versus the cost of performing fusion and ending up with a bigger code heap. Also note that the JIT is generally not able to get to the same level of fusion through inlining, because virtual methods calls prevent it from seeing across these boundaries. Similarly, it wouldn't fuse object layouts together.

An additional benefit of this approach is that the resulting fused operator implementations can be persisted as assemblies, which can provide an alternative means to recover queries during a failover. Rather than deserializing big expression trees, fused operator implementations could be reloaded. Approaches like this have been prototyped as well, with a generational approach whereby dynamically generated assemblies with fused operators get merged to contain multiple such operators, based on the survival of query expressions using these across checkpoint times. The more a query gets checkpointed, the more it makes sense to bundle it together with other survivors, into something akin to a "generation" (in GC parlance). Old generations can then be restored quickly, while younger generations are still based on expression tree deserialization. When they survive across many checkpoints, the work to fuse them can take place in order to generate new dynamic assemblies. When query expressions get removed, tombstones are kept to prevent instantiating them when loading the dynamic assembly for the generation they belong to. Once many query expressions are tombstoned, a compaction can take place by generating a new dynamic assembly omitting tombstoned operators, and potentially merging it with younger generations.

> **Note:** Operator fusion is most beneficial in conjunction with persistence of dynamically generated assemblies. As such, it only works in .NET Framework where these APIs are available. This said, there are many limitations including the inability to generate types and instance members using expression trees, or to generate open generic types using expression trees. This library works around a few of them, but a bigger effort was undertaken to create a fork of the expression tree library with support for generating types (e.g. a class with instance members, and the use of `this` in an expression tree to refer to the current instance). An alternative to consider today would be the use of Roslyn, though there's a lot of extra cost involved with generating syntax trees.

## Example

This prototype contains a sample of fusion for some of the simplest query operators, but including some stateful ones (to showcase the implementation of dirty flags). It omits implementations for more complex operators such as higher-order ones, operators with multiple sources, and operators with dynamically changing object graphs at runtime. In most cases it makes sense for such operators to be used in a more traditional way because they tend to introduce inner subscriptions that are checkpointed separately, which is beneficial to keep the scope of dirtying state as small as possible. As such, this prototype would likely be combined with an expression rewriter to tile (maximum) subtrees of fusable operators, fuse them, and rewrite the resulting expression as a mix of fused operators as well s regular ones. For example:

```csharp
xs.Where(f).Take(n).SelectMany(x => ys(x).Select(g).Skip(m)).Where(h).Take(o).Average()
```

could be turned into

```csharp
xs.Fused1(n).SelectMany(x => ys(x).Fused2(m)).Fused3(o)
```

where `Fused*` operators are the result of fusion of various operators within these "segments". Operators like `SelectMany` remaine cause they naturally give rise to the creation of inner subscriptions which can be checkpointed separately. Also note that parameters like the `count` on the `Take(int)` operator is still passed to fused operators so they can be reused. The degree to which parameters are kept versus get inlined is something that deserves a dial. For example, the fusion of `Where(f).Take(n)` could have no parameters, or have `f` and/or `n`. Right now, we make an arbitrary choice to inline some of them, in particular lambdas, so we can perform more aggressive inlining and avoid yet another virtual call to the delegate.

Additionally, note that operators such as `First(Func<T, bool>)` with predicates are not implemented directly but rather as macros over `Where(Func<T, bool>)` combined with `First()`. After fusion, the generated code is pretty much identical to a hand-rolled implementation of the predicate-based overload. This shows the power of fusion.

The code in the project contains a few samples to illustrate the behavior of fusion. When run, it also persists an `Artifacts.dll` assembly to disk. This can be opened in tools like ILDASM or ILSpy to see the resulting fused code that was generated at runtime.

> **Note:** Several prototype deployments have also combined the expression rewrites carried out by fusion with expression tree optimizer libraries to further trim the code size. For example, operators like `Where` and `Select` do emit a `try...catch...` statements around evaluations of predicates or selectors. In some cases, these cannot throw (e.g. `x > 0` or `x * 2` in an `unchecked` context), and the exception handlers can be taken away.
