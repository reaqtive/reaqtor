# Nuqleon.Linq.CompilerServices.Optimizers

Provides optimizers for LINQ query expressions.

## QueryTree

A central type in this library is `QueryTree` which represents a node in a query expression. It differentiates between known query operators (for which optimizations can be applied) versus opaque nodes representing other operators. For example, a node whose `QueryNodeType` is set to `QueryNodeType.Operator` represents a known query operator. Such a node will have a `QueryOperator` derived type which further has a `OperatorType` to differentiate between different known query operators such as `Where`, `Select`, `First`, `Take`, etc.

> **Note:** This list of known query operators is limited but can easily be extended. Query operators that are currently recognized are useful to apply optimizations for.

In order to analyze or rewrite query trees, various visitor types are provided, such as `QueryVisitor` and the generic variant `QueryVisitor<TQueryTree, TMonadMember, TQueryOperator>`. For details, please refer to the code.

## Conversions

In order to use `QueryTree` expressions, one performs a conversion from an expression tree that represents a LINQ query expression using well-known methods such as `Enumerable.*` or `Queryable.*`. These conversions are provided through helper types such as `EnumerableToQueryTreeConverter` and `QueryableToQueryTreeConverter`.

> **Note:** Because the semantics of query operators in this library is identical between `IEnumerable<T>` and `IObservable<T>`, it is trivial to write converters for the `IObservable<T>` set of query operators as well. This library refrains from this in order to limit the number of dependencies on external libraries, in this case Rx. However, a separate assembly can be built (and has in fact been built for both classic Rx as well as Nuqleon query algebra methods), which brings in this dependency at a higher layer. Furthermore, converters can be built that operate on a normal form (e.g. `Invoke(Parameter, args)` where the `Parameter` node is an unbound variable referencing a query operator, as is done in Nuqleon).

For example:

```csharp
IQueryable<int> query = new int[] { 1, 2, 3 }.AsQueryable().Where(x => x % 2 == 0).Where(x => x < 10).Select(x => x * x).Select(y => y + 1).Take(2).Take(1);
Expression expr = query.Expression;
QueryTree queryTree = new QueryableToQueryTreeConverter().Convert(expr);
```

## Optimizers

Once a query tree has been obtained, optimizations can be applied. This library provides a number of optimizers, including:

* `CoalescingOptimizer` to coalesce adjacent nodes, e.g. `xs.Where(f).Where(g)` becomes `xs.Where(x => f(x) && g(x))`.
* `LetCoalescingOptimizr` to coalesce transparent identifiers introduced by `let` clauses (which reduces allocations).

More optimizers can be built given the extensible nature of the library; all of the above implement `IOptimizer` which has a single `Optimize` method:

```csharp
QueryTree Optimize(QueryTree queryTree);
```

Furthermore, the `Optimizer` static class provides a set of operators or combinators to work with optimizers. For example:

* `Then(IOptimizer, IOptimizer)` enables chaining of optimizers;
* `FixedPoint(IOptimizer, ...)` enables applying an optimizer many times until no further optimization is obtained.

Once an optimizer has been applied, the resulting `QueryTree` can be converted back to the original domain by using the `Reduce` method which returns an expression tree. For example:

```csharp
IOptimizer optimizer = new CoalescingOptimizer();
QueryTree optimized = optimizer.Optimize(queryTree);
Expression optimizedExpr = optimized.Reduce();
IQueryable<int> optimizedQuery = query.Provider.CreateQuery<int>(optimizedExpr);
```

For the running example, the resulting query will be equivalent to:

```csharp
new int[] { 1, 2, 3 }.AsQueryable().Where(x => x % 2 == 0 && x < 10).Select(x => x * x + 1).Take(1);
```
