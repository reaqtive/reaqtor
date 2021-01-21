# Nuqleon.Linq.Expressions.Optimizers

Provides optimizers for expression trees.

## ExpressionOptimizer

The `ExpressionOptimizer` class is an expression tree visitor that rewrites an expression tree by performing various types of optimizations that can be configured by the user. To create an optimizer instance, the constructor accepts two parameters:

* `ISemanticProvider` to specify a semantic provider that's consulted by the optimizer to make optimization decisions;
* `IEvaluatorFactory` to control the behavior of partial evaluation of subtrees.

## Semantic providers

Semantic providers are used by the optimizer to gather semantic information about expressions, types, members, values, etc. See the `ISemanticProvider` interface for more information.

While custom implementations of this interface are possible, a good default choice for the semantic provider is `DefaultSemanticProvider` or `MetadataSemanticProvider`. The latter is more powerful because it supports specifying semantic information about .NET types and members, for example if a given type is immutable, or if a given member is a pure function. The library comes with various catalogs for commonly used types and members in the .NET Base Class Libraries, which can be used to construct a `MetadataSemanticProvider` as shown below:

```csharp
var msp = new MetadataSemanticProvider
{
    PureMembers = PureMemberCatalog.All,
    ConstParameters = ConstParameterCatalog.All,
    ImmutableTypes = ImmutableTypeCatalog.All
};
```

Pure members are used to perform partial evaluation of nodes such as `MethodCallExpression`, for example `Math.Abs(n)` where `n` itself is pure. Constant parameters are required for partial evaluation of a function if any of its parameters has a mutable type but the function doesn't perform any mutation, e.g. `string.Split(string, char[])` doesn't mutate the given `char[]`. Finally, checks for immutable types are used for various checks to ensure a member can't mutate the state of an object, e.g. `System.Tuple<T1, T2>`.

## Evaluator factories

Evaluator factories abstract over the mechanism to perform partial evaluation of an expression tree. The use of custom factories enables caching of compiled delegates, checking whether an expression to be evaluated doesn't have harmful side-effects, etc.

The default implementation is `DefaultEvaluatorFactory` which can be instantiated as follows:

```csharp
var eval = new DefaultEvaluatorFactory();
```

## Using an optimizer

Constructing an optimizer is easy, given the semantic provider and the evaluator factory:

```csharp
var optimizer = new ExpressionOptimizer(msp, eval);
```

In more advanced scenarios, one can derive from `ExpressionOptimizer` and override additional methods to control the behavior of the optimizer.

To optimize an expression tree, simply call the `Visit` method, as shown below:

```csharp
var three = optimizer.Visit(Expression.Add(Expression.Constant(1), Expression.Constant(2)));
```

In the sample above, the result of running the optimizer will be a `ConstantExpression` with value `3`.

## Types of optimizations

The expression optimizer performs a whole slew of optimizations, mostly driven by algebraic rewrites (e.g. double negation, De Morgan's law, etc.) and partial evaluation of sub-expressions. Such optimizations are often enabled by inlining steps (e.g. originating from beta reduction).

> **Note:** One of the motivating factors for building this optimizer was reduction of expression tree size after applying various rewrites. For example, substituting parameters in a query expression using argument values can result in many constants that can be folded together, with subsequent optimizations applied. The design of having custom semantic providers enables supplying semantic information about functions in other libraries, such as JSON libraries. An an example, consider an expression such as `((ObjectExpression)Json.Expression.Parse(s))["foo"]` which can be reduced to a constant if `s` itself is a constant. If this expression occurs in a high-frequency code path, e.g. the predicate of a `Where` operator, such optimizations pay off a lot.
