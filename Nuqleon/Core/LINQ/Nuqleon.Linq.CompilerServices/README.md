# `Nuqleon.Linq.CompilerServices`

Provides expression tree utilities such as visitors, analyzers, rewriters, evaluators, and more.

## Visitors

`System.Linq.Expressions` comes with a default `ExpressionVisitor` that visits all nodes of an expression tree and invokes the `Update` method on a node if any of its children changes (as tested by an reference equality check). This library provides additional types of visitors to enable rewrites of expressions to other types, to track scope information of variables, to visit other elements of the tree (such as reflection information), etc.

### Generic visitors

`ExpressionVisitor<TExpression>` is the base type for expression visitors that rewrite an expression of type `Expression` to an object of type `TExpression`. All of the `Visit*` methods are `abstract` and have to be implemented by derived types to perform the recursion and the conversion, e.g.:

```csharp
protected abstract TExpression VisitBinary(BinaryExpressionAlias node);
```

Because this is quite tedious to do (both the recursion and the construction of results), a more derived type is provided:

```csharp
public abstract class ExpressionVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionVisitor<TExpression>
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding;
```

It's quite rich in generic type parameters, but they should be straightforward to understand. While this visitor takes care of the all the recursion over the structure of the tree, it still requires the user to supply a means to construct objects of the result types. These are represented as `abstract` methods that use the `Make` prefix, e.g.:

```csharp
protected abstract TExpression MakeBinary(BinaryExpressionAlias node, TExpression left, TLambdaExpression conversion, TExpression right);
```

An example of a generic visitor that performs a conversion from an expression tree to another type is a printer (returning a `string` for each node) or a converter to another tree representation (e.g. `ExpressionSlim` for Bonsai trees).

### Narrow visitors

The `ExpressionVisitorNarrow<...>` class acts as a base class for expression visitors that do not support statement nodes (e.g. `Block`, `Loop`, etc.). It is provided as a means to deal with just expressions, as they existed in .NET 3.5 prior to the introduction of the DLR and statement nodes in .NET 4.0. All `Visit*` methods for statement nodes are overridden and throw `NotSupportedException`.

### Partial visitors

The `PartialExpressionVisitor<TExpression>` class implements all `Visit*` methods by throwing `NotSupportedException`. Derived types can override the nodes they want to support, therefore rejecting trees that have unsupported nodes. This is unlike the built-in `ExpressionVisitor` where failure to override a `virtual` method on the base class can go unnoticed.

### Visitors with reflection

The `ExpressionVisitorWithReflection` class is a traditional visitor but it also visits all of the "reflection" objects that exist on tree nodes. For example, `BinaryExpression` has an optional `Method` property of type `MethodInfo`. The visitor with reflection support does invoke a `VisitMethod` virtual to visit the `Method` (in addition to recursing over `Left`, `Right`, and `Conversion`). A corresponding `MakeBinary` method is supplied to construct a new node in case any of the children (including the visited method) has changed:

```csharp
protected virtual Expression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method, LambdaExpression conversion);
```

Other examples of visiting reflection information include the `Type` or `ConstructorInfo` on a `NewExpression`, the `MemberInfo` on a `MemberExpression`, the `Type` in a `UnaryExpression` used for purposes of `Convert`, etc. By visiting all of these additional reflection objects, visitors can perform tasks such as checking reflection objects against allow lists, retargeting methods or members, etc.

### Cooperative visitors

Cooperative expression visitors enable dispatching into a user-specified visitor for nodes that refer to methods, properties, fields, or constructors. For example, consider an expression representing a call to a method `Bar()` on an instance of type `Foo`:

```csharp
(Foo f) => f.Bar()
```

If the expression gets visited using a `CooperativeExpressionVisitor` and the `Bar` method has a `VisitorAttribute` applied to it, the visit of the `MethodCallExpression` will dispatch into the user-specified visitor type using an interface called `IRecursiveExpressionVisitor` with a single `TryVisit` method:

```csharp
bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result);
```

For example:

```csharp
class Foo
{
    [Visitor(typeof(BarVisitor))]
    public void Bar() { /*...*/ }

    private sealed class BarVisitor : IRecursiveExpressionVisitor
    {
        public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
        {
            // implement custom logic here
        }
    }
}
```

The custom visitor can recurse into child nodes (using the `visit` delegate passed in), and return a rewritten expression through the `result` output parameter. If the `TryVisit` method returns `false`, the node remains unchanged. This mechanism is useful to inject behaviors into an expression visitor across component boundaries to support loose coupling. Rather than implementing a visitor that has references to a whole bunch of types (resulting in tight coupling and a central place where a lot of dependencies come together), a cooperative visitor can support a decoupled approach where the logic lives alongside the types and members that are being operated on.

### Scope-tracking visitors

Scope-tracking visitors are useful to keep track of variables (represented as a `ParameterExpression`) that are being declared in expression tree nodes including `Lambda` (cf. `LambdaExpression.Parameters`), `Block` (cf. `BlockExpression.Variables`), and `Catch` (cf. `CatchBlock.Variable`). These are the *definition sites* of variables. When visiting a tree, we'll also encounter *use sites*, i.e. any other place where `ParameterExpression` is used. Scope-tracking visitors build an environment with symbol tables enabling a visitor to locate a definition site when encountering a use site. For example:

```csharp
x => y => x + y
```

When visiting the outermost lambda expression, a new scope is entered where `x` is declared. Next, when visiting its body, we encounter another lambda expression that declares `y`. Finally, when visiting the body of this inner lambda expression, we'll pass through a binary addition node to finally find the use sites for `x` and `y`. At this point, we may want to figure out where these variables were declared.

The `ScopedExpressionVisitor<TState>` base class can be used to build scope-tracking visitors. The `TState` parameter represents a piece of state to keep and associate with a `ParameterExpression` variable whenever a declaration site is encountered. This piece of state can then be looked up when a use site of a `ParameterExpression` variable is visited. Examples of types used for `TState` include:

* another representation of a variable to translate to (e.g. when going from `ParameterExpression` to `ParameterExpressionSlim` as part of Bonsai conversion);
* a dummy value when the visitor is simply used to find unbound "global" variables that do no have a definition site within the tree being visited;
* a new instance of `ParameterExpression` to rewrite both definition and use sites to, for example when renaming variables or to avoid accidental aliasing or shadowing;
* etc.

When a declaration site is encountered during the visit of a node such as `Lambda`, `Block`, or `Catch`, a call is made to the abstract `GetState` method:

```csharp
protected abstract TState GetState(ParameterExpression parameter);
```

Once the state is obtained for all variables declared in a scope established by any of the nodes mentioned, it gets pushed into an environment through a virtual `Push` method (which can optionally be overridden). This is done prior to visiting the body of the node (e.g. the `Body` of a `Lambda`, the `Expressions` in a `Block`, or the `Body` in `CatchBlock`).

In order to handle use sites, the derived visitor class can override `VisitParameter` which will only be called by the base class for use sites (and no longer for definition sites). In there, one typically uses the `TryLookup` method to try to look up the state associated with the given `ParameterExpression`:

```csharp
protected override Expression VisitParameter(ParameterExpression node)
{
    if (TryLookup(node, out var state))
    {
        // do something with the state
    }

    return node;
}
```

Note that scope-tracking visitors honor the scoping and binding rules for variables. For example, in `x => x => x` where all variables `x` are represented using the same `ParameterExpression` instance, the use site of `x` binds to the innermost lambda's declaration site. Consider a more complex example:

```csharp
x => f(x, x => x)
```

The corresponding bindings are as follows:

```
x => f(x, x => x)
^      |  ^    |
|      |  |    |
+------+  +----+
```

### Type-based visitors

The `TypeBasedExpressionRewriter` is a specialized visitor that can be parameterized on delegate-based `Visit` functions to be invoked for nodes of a specified type. This can be useful in expression trees that mix different domains (e.g. `IEnumerable<T>` and `IObservable<T>`) where different types of rewrites have to be carried out.

To use this class, simply create an instance and add rewrite rules using the `Add` method. For example:

```csharp
var rewriter = new TypeBasedExpressionRewriter();

rewriter.Add(typeof(DateTime), expr => { /* process and return an expression */ });
rewriter.AddDefinition(typeof(IObservable<>), expr => { /* process and return an expression */ })

Expression res = rewriter.Visit(expr);
```

The `AddDefinition` method is used for open generic types.

## Expression tree factories

`IExpressionFactory` is an interface abstracting over the static factory methods on `System.Linq.Expressions.Expression`. We use this interface in various places where we have to create new nodes. The use of an interface enables alternative implementations of factory methods that perform additional checks, disallow the use of certain node types or members referenced by nodes, return cached node instances, etc.

A default implementation called `ExpressionFactory` simply implements the interface in terms of the static factory methods on `System.Linq.Expressions.Expression`.

In addition, an alternative `ExpressionUnsafeFactory` is provided which instantiates the expression objects with tricks to bypass the static factory methods on `System.Linq.Expressions.Expression`. This can significantly speed up the creation of new nodes (due to bypassing of various expensive reflection-based checks), at the expense of potentially constructing invalid trees. When trying to `Compile` a `LambdaExpression` containing nodes that don't properly type check, the CLR may encounter fatal execution engine conditions. Use of this factory is reserved for cases where there's no intent to compile the tree, or when prior checks have guaranteed correctness (for example because we're deserializing an expression tree that was serialized after having been constructed through safe factories).

**Note:** Unsafe expression factories have been used in some deployments of Nuqleon to speed up recovery times of query engines where a lot of expression tree deserialization operations take place.

## `FuncletExpression`

Funclets are custom extension expression tree nodes that evaluate a given expression to a constant upon reduction. They're useful for partial evaluation and can be generated in an expression visitor when encountering a subtree that cannot be processed by some remote system (e.g. in query translation or prior to expression tree shipping). By leaving a `FuncletExpression` wrapper around such a subtree in the tree, further visits to the tree will trigger a reduction that replaces the functlet by a `ConstantExpression` containing the result of evaluating the subtree.

For example, consider a tree of the form `x => x + a`, where `a` is a variable captured from an outer scope. In an expression tree, `a` will be represented as some `MemberExpression` accessing a field `a` on some `ConstantExpression` which contains a `Value` that references the compiler-generated closure class (in C# parlance, a "display class"). Some visitor may figure out that this closure class is something a service doesn't support (e.g. by checking all types and members against a set of known types and members, i.e. an allow list), thus deciding that the `Member(Constant(closure), a)` subexpression should be locally evaluated. By wrapping it with a `FuncletExpression` the partial evaluation to a `Constant` can be deferred until future visits over the tree (in visitors that are not handling `FuncletExpression` directly, thus causing reduction of the extension node through a call to `Reduce`).

An additional extension method called `Funcletize` can be used to create a funclet around a given expression.

## Expression tree analysis

Various utilities are provided in this library to make the task of analyzing expression trees easier.

### Equality comparers

`ExpressionEqualityComparer` implements `IEqualityComparer<Expression>` to compare two `Expression` instances for equality, taking binding of variables and labels (used in `Goto` and `Label` expressions) into account. For example, given trees `x => x` and `y => y`, they will compare equal even though `x` and `y` are not reference equal. That is, as long as the binding of variables between use sites and definition sites is equivalent in both trees being compared, the equality requirement is met.

> **Note:** Because keeping track of scopes requires state, the `ExpressionEqualityComparer` is really a factory for a stateful underlying `ExpressionEqualityComparator` implementation. Each call to `Equals` on the comparer causes the instantiation of a new comparator, using fresh state. This makes the use of a single equality comparer instance safe across threads.

### Free variable scanner

In order to find unbound variables that occur in an expression, the `FreeVariableScanner.Scan` method can be used:

```csharp
public static IEnumerable<ParameterExpression> Scan(Expression expression);
```

Alternatively, a Boolean-valued check can be used (which is more efficient than compiling a list of unbound variables):

```csharp
public static bool HasFreeVariables(Expression expression);
```

As an example, consider an expression of the form `x => x + y`, where `y` is unbound. Note than a single `ParameterExpression` may be used in a bound context as well as an unbound context, for example `Bar.Foo(x, x => x + 1)` contains an unbound occurence of `x` in the first argument of the call to `Foo`.

Free variable scanners are often used as part of a binding step. For example, in Nuqleon we represent query operators using `Invoke(Parameter(...), ...)`, where the `Parameter` is unbound and has a `Name` that refers to the query operator, e.g. `rx://filter`. Upon receiving such an expression, a free variable scan is carried out to find these unbound parameters, look them up in some environment (e.g. a query engine registry), and finally bind them by some form of inlining (e.g. beta reduction in lambda calculus terms, see `BetaReducer` in this library).

### Allow list scanner

When processing expressions, it's often useful to scan them for undesirable operations, such as calls to methods that are unsafe to the hosting environment (when evaluating an expression) or constructs that cannot be translated to some target language (e.g. when writing a query provider). This library provides allow list scanners that check expressions against a given list of allowed items. Two different types are provided:

* `ExpressionTypeAllowListScanner` which checks the `Type` for every `Expression` node in a given expression tree, e.g. to make sure a tree evaluates within a certain "type domain";
* `ExpressionMemberAllowListScanner` which checks occurrences of any `MemberInfo` on any `Expression` node in a given exprssion tree, e.g. to check for methods, properties, fields, and constructors used.

Base classes are provided that offer a single Boolean-valued `Check` function that can be implemented. In particular:

```csharp
public abstract class ExpressionTypeAllowListScannerBase : ExpressionVisitor
{
    protected abstract bool Check(Type type);
}

public abstract class ExpressionMemberAllowListScannerBase : ExpressionVisitor
{
    protected abstract bool Check(MemberInfo member);
}
```

These visitors take care of recursing over the structure of the tree and inquiring about the validity of encountered `Type` or `MemberInfo` objects by calling `Check`. In case it's more desirable to simply check types or members against a given list of allowed ones, the more derived classes can be used:

```csharp
public class ExpressionTypeAllowListScanner : ExpressionTypeAllowListScannerBase
{
    public TypeList Types { get; }
}

public class ExpressionMemberAllowListScanner : ExpressionMemberAllowListScannerBase
{
    public TypeList DeclaringTypes { get; }
    public MemberList Members { get; }
}
```

Instances of these scanners can be created using collection initializers for the `Type`, `DeclaringTypes`, and `Members` properties. For example:

```csharp
var scanner  = new ExpressionMemberAllowListScanner
{
    DeclaringTypes = { typeof(string) },
    Members = { typeof(DateTime).GetProperty(nameof(DateTime.Now)) }
};
```

When using this scanner, all uses of members on `System.String` will be allowed, as will be the use of `DateTime.Now`. However, uses of methods like `DateTime.Add` or `Guid.Parse` will be rejected.

Upon encountering a type or member that is not supported, the default implementation will throw a `NotSupportedException`. In some cases, a different action is warranted, which can be provided by overring various `Resolve` virtual methods such as:

```csharp
protected virtual Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit) where T : Expression;
```

Methods like this are called when the specified `member` was rejected because it was detected on the given `expression`. This provides the derived class with the option to take an action other than throwing an exception. For example, one may decide to rewrite the expression to a supported construct (while optionally using `visit` to first continue scanning any subexpression of the current given expression), or decide to wrap the expression in a `FuncletExpression` to trigger partial local evaluation (e.g. when the allow list scanning is done prior to sending an expression to some service, where local evaluation can reduce certain constructs - such as accesses to closures - to constants first).

> **Note:** Nuqleon has been using allow lists, either table-driven or with manual overrides for `Check`, in various layers of the system including the client (prior to submitting expressions to a Nuqleon-compliant service), various front-end services (as a defense-in-depth mechanism, e.g. making sure expressions don't do things like `Environment.FailFast()`), as well as back-end services to further narrow down the set of allowed operations (or as a further defense-in-depth piece, right before evaluating an expression that may have been rehydrated from an untrusted store or has been subject to many rewrites by pluggable components).

## Expression tree rewriters

This library also provides a whole plethora of expression tree rewriting utilities.

### Alpha renaming

Lambda calculus has the notion of alpha renaming whereby the names of variables get changed in a way that doesn't inadvertently changes the bindings of variable. For example, consider an expression `x => y => x + y`. In here, we can change the name of `x` to `a` but not to `y` because it'd cause the binding to change to the `y` declared on the inner lambda expression.

This library provides an implementation of alpha renaming using the `AlphaRenamer.EliminateNameConflicts` static method, shown below:

```csharp
public static Expression EliminateNameConflicts(Expression expression);
```

Its goal is to eliminate naming conflicts, for example in expressions like `x => x => x` where it's unclear if this means `x0 => x1 => x0` or `x0 => x1 => x1` or `x0 => x1 => x2`, which depends on the reference equality of the `ParameterExpression` nodes in use sites and definition sites (in this case, the `Parameters` collection of a `LambdaExpression`, but all of this holds for `Block` and `Catch` nodes as well). Given an expression like this, the alpha renamer will introduce new variables to ensure that names are unique when confusion can arise.

> **Note:** Alpha renaming is purely cosmetic; it doesn't ensure that `ParameterExpression` nodes are only used for a single definition site. For example, in `f(x => x / 2, x => x + 1)`, all occurrences of `x` may be reference equal, but there's no name conflict in nested scopes, so the alpha renamer won't touch it. To ensure all instances of `ParameterExpression` nodes are unique across definition sites, on can simply use a `ScopedExpressionVisitor<ParameterExpression>` where the `GetState` override simply instantiates a new `ParameterExpression` with the same name (or optionally a different name) and the same type.

### Beta reduction

Computation in lambda calculus is driven by beta reduction. Given an invocation of a lambda expression, a reduction can be made by substituting the arguments used in the invocation for the parameters of the lambda expression. For example:

```csharp
(x => x + 1)(2)
```

can be reduced to `2 + 1` by substituting `2` for `x` in the body of the lambda expression. That is, arguments get inlined in the lambda body. There are a few caveats when variables can shadow one another, for example:

```csharp
(x => f(y => x + y))(y)
```

In here, the `y` used for the top-level invocation may be bound to a declaration in a higher scope, or represent a global variable. If we naively carry out beta reduction, we may end up with `f(y => y + y)` where both occurences of `y` are now bound to the parameter introduced by the lambda expression passed to `f`. To overcome this, lambda calculus provides a means to rename variables by using alpha renaming.

This library provides a beta reduction mechanism in `BetaReducer.Reduce`. A few variants are available, as shown below:

```csharp
public static Expression Reduce(Expression expression);
public static Expression Reduce(Expression expression, BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions);
public static Expression ReduceEager(Expression expression, BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions, bool throwOnCycle);
```

The simplest form is the most conservative one and only performs reduction for a select set of nodes including `Constant`, `Default`, `Quote`, and `Parameter` (if no incorrect "capture" results). For example:

```csharp
(x => x + x)(42)
```

will be reduced to `42 + 42`, because the `Constant` node representing `42` is free of side-effects and can safely be inlined for every occurrence of `x` in the body of the `Lambda`. However, an argument like `int.Parse(Console.ReadLine())` does have side-effects and reducing:

```csharp
(x => x + x)(int.Parse(Console.ReadLine()))
```

to

```csharp
int.Parse(Console.ReadLine()) + int.Parse(Console.ReadLine())
```

will cause the side-effect of reading from the console to be duplicated. If this is desirable, or a separate analysis has figured out that the arguments passed to an `Invoke` node are side-effect free (e.g. `Math.Abs(42)` is pure because `Abs` is a pure function, but the `BetaReducer` does not have that knowledge), other overloads of `Reduce` can be used that specify the `BetaReductionNodeeTypes` and the `BetaReductionRestrictions`.

The node types indicate which nodes are safe for inlining. By default, a value of `Atoms` is used, which is a combination of `Constant`, `Default`, `Parameter`, and `Quote`. A value named `Molecules` is used to refer to any other expression tree node type, and `Unrestricted` combines both.

The restrictions control whether or not side-effects can be duplicated or dropped. Valid options are `DisallowDiscard` (to prevent a side-effect from being dropped if the parameter does not occur in the lambda body), `DisallowMultiple` (to prevent a side-effect from being duplicated if the parameter occurs more than once in the lambda body), or `ExactlyOnce` (to ensure the parameter is used exactly once in the lambda body).

> **Note:** `ExactlyOnce` merely enforces that exactly one use site of a parameter is found. Whether or not the use site will be evaluated (and how often) is an orthogonal concern. For example, the use site of the parameter may be inside a conditional (e.g. `b ? x : y`), or inside a loop construct. As such, the number of evaluations of the inlined expression depends on the control flow around the binding site. This library does not perform control flow analysis at the moment.

Note that the order of side-effects may change due to inlining; the beta reducer does not provide an option to control that. For example:

```csharp
(x => int.Parse(Console.ReadLine()) - x)(int.Parse(Console.ReadLine()))
```

Prior to beta reduction, the first read of the console happens as part of building the argument list for the invocation. This value will get bound to `x`, and then the second read happens as part of evaluating the lambda expression. Say the user entered `5` and then `3`, the result will be `-2`. However, after beta reduction, the order of side-effects is changed, and the left operand of (the non-commutative operator) `-` is read first, causing the operands to be flipped, with the result being `2` instead.

> **Note:** The beta reducer is typically used when inlining definitions for global unbound parameters, i.e. a binding step. For example, consider an expression `rx://filter(xs, x => x > 0)`.
> * First, a free variable scanner is used to find `rx://filter` and `xs`. By "lambda lifting" the unbound parameters out of the original expression, we obtain:
>
>   `(Func<IObservable<int>, Func<int, bool>, IObservable<int>> rx://filter, IObservable<int> xs) => rx://filter(xs, x => x > 0)`
>
> * Next, the free variables (now lifted into a top-level parameter list on the constructed lambda expression) are looked up in some registry (e.g. in a query engine), resulting in bindings like:
>
>   `rx://filter := (IObservable<int> source, Func<int, bool> predicate) => Observable.Where(source, predicate)`
>   `xs := Observable.Return(42)`
>
> * Next, we construct an invocation expression around the (lambda lifted) original expression, by using the binding targets for the arguments, which yields:
>
>   `((Func<IObservable<int>, Func<int, bool>, IObservable<int>> rx://filter, IObservable<int> xs) => rx://filter(xs, x => x > 0))((IObservable<int> source, Func<int, bool> predicate) => Observable.Where(source, predicate), Observable.Return(42))`
>
> * Finally, two successive steps of beta reduction cause inlining to happen:
>
>   `((IObservable<int> source, Func<int, bool> predicate) => Observable.Where(source, predicate))(Observable.Return(42), x => x > 0)`
>
>   followed by
>
>   `Observable.Where(Observable.Return(42), x => x > 0)`

Quite often, as illustrated in the example above, one wants to repeatedly perform beta reduction until there is no further opportunity to do so (i.e. all `Invoke(Lambda, ...)` sites have been reduced). This is achieved by using `ReduceEager` which repeats beta reduction until the tree no longer changes (i.e. has reached a "fixed point").

> **Note:** `ReduceEager` accepts a `throwOnCycle` parameter which is used for cycle detection which can occur if a tree keeps reducing to itself (exercise left to the reader; hint: look up a lambda calculus operator called "omega").

### Eta converter

A final lambda calculus construct is eta conversion, which simplifies an expression of the form `(x => f(x))` to `f`, by taking away the redundant lambda "abstraction" whose body contains an invocation "application". The result is a simpler, more compact, expression.

This library provides eta conversion through `EtaConverter.Convert`.

### Compiler-generated name elimination

The C# compiler emits compiler-generated names for certain lambda expression parameters, e.g. in the context of `let` clauses that introduce so-called "transparent identifiers". An example is:

```csharp
from x in xs
let y = x + 1
select x + y
```

Compilation of this query expression results in lowering to the following form:

```csharp
xs
.Select(x => new { x, y = x + 1 })
.Select(<>__blah => <>__blah.x + <>__blah.y)
```

where `<>__blah` is some compiler-generated identifier. This can make debug output for expressions quite cumbersome to read, so this library provides a utility (`CompilerGeneratedNameEliminator.Prettify`) to make such an expression look prettier by renaming these parameters.

### Constant hoisting

Constant expressions often occur in expression trees due to partial evaluation of locals at the point of rewriting an expression for submission to a service. For example:

```csharp
int a = 41;

IQueryable<int> query = from x in xs where x > a select x + 1;

a = 42;

foreach (var x in query)
{
    // use results
}
```

In this example, the `foreach` loop triggers the query provider underneath the `IQueryable<int>` to prepare the expression tree representing the query in order to perform evaluation. In some cases, this evaluation involves translation to an other language (e.g. SQL), but in systems like Nuqleon, we end up serializing the expression tree (in Bonsai format) to another machine.

The lowered query expression looks like this:

```csharp
xs.Where(x => x > a).Select(x => x + 1)
```

where `a` is a local variable in the outer scope. The compiler generates a closure to hold `a` in a so-called display class, like this:

```csharp
var closure = new <>__DisplayClass();
closure.a = 41;

IQueryable<int> query = xs.Where(x => x > closure.a).Select(x => x + 1);
```

but rather than now closing over `closure` (which would go on ad infinitum), the expression tree generated for `x => x > closure.a` contains a `ConstantExpression` node holding the display class instance. That is, the body of the `LambdaExpression` looks like this:

```csharp
Expression.GreaterThan(
    x,
    Expression.Field(
        Expression.Constant(closure),
        typeof(<>__DisplayClass).GetField("a")
    )
)
```

where `x` is a `ParameterExpression`.

During preparation of this expression for submission to a service, we typically perform partial evaluation (through a process called *funcletization*) of the `MemberExpression` that reads the field from the closure, in order to avoid serializing the closure object (which refers to an compiler-generated type that does not exist in the service, nor do we want to deploy random assemblies).

> **Note:** An alternative approach could be to rewrite display classes to `System.Tuple<...>` values which can be serialized more easily. However, we should be careful about serializing such values more than once. For example, in an expression `x => x > a && x < b`, both `a` and `b` end up being represented as `Member(Constant(closure), field)` trees. If we keep these trees for both `a` and `b`, and serialize the tuple by value, we end up with two copies of the entire tuple. By partial evaluation of these `Member` expressions, we effectively "slice" the closure and only retain the hoisted variable's value in each use site. This said, if a variable occurs multiple times, we still end up with copies of such values for every use site. To mitigate that, we'd need to store the closure value in a "constants table" (a later version of Bonsai supports this notion).

In the example above, the expression gets rewritten to the equivalent of:

```csharp
xs.Where(x => x > 42).Select(x => x + 1)
```

where `42` was captured at the point of query evaluation, in this case by the `foreach` loop, which happens after re-assignment of `a` with value `42`.

Once we end up installing this query on the service (and in the case of Nuqleon, it'd be a standing reactive event processing query that will keep running until explicitly disposed), we may have a lot of similar queries floating around due to earlier query submission from the same client library (e.g. with different values of `a`), for example:

```csharp
xs.Where(x => x > 42).Select(x => x + 1)
xs.Where(x => x > 17).Select(x => x + 1)
xs.Where(x => x > 99).Select(x => x + 1)
```

This results in expression tree compilation of queries that are completely identical, except for constants, and thus growth of the JIT-compiled code heap, extra memory used for expression trees (which are often kept resident in memory, e.g. to be checkpointed later, or to be analyzed at a later point, for example by a query optimizer). One way to solve this is by "hoisting" constants out of the expression and turn them into parameters:

```csharp
((int p0, int p1) => xs.Where(x => x > p0).Select(x => x + p1))(42, 1)
```

Note that this starts to look like the original user code again, where `a` has been replaced by a generated variable `p0`. In addition, the constant `1` used in the `Select` selector body has also been lifted into a variable. One can think of this as a generalization or templatization of an expression by punching holes (represented as variables) where constants used to be.

Now, when a new expression comes in, we can apply constant hoisting and compare the obtained lambda expression against already existing query expressions that may have been compiled to a delegate before. This lookup can be implemented by the `ExpressionEqualityComparer` (for example used with a dictionary type) which supports equality checks and hash code computation.

> **Note:** Constant hoisting results in a beta-reducible construct consisting of a `Lambda` expression and bindings of `Parameter` to `Constant` nodes. One can think of constant hoisting as a form of "outlining" where beta reduction is a form of "inlining".

This library supports constant hoisting via the `ConstantHoister` class. To create a constant hoister, the `Create` factory method can be used:

```csharp
public static ConstantHoister Create(bool useDefaultForNull, params LambdaExpression[] exclusions);
```

The first parameter can be used to convert `Expression.Constant(null, type)` to `Expression.Default(type)` nodes, which reduces the number of `null` constants to hoist (because it's common for expressions to contain `null`-checks).

The second parameter can be used to specify exclusions of constants through patterns expressed as lambda expressions. For example `(string s) => string.Format(s, default(object[]))` states that a constant occurring as the first argument of a `string.Format` call should not be hoisted as a constant (because such uses are typically invariant across many copies of the tree, i.e. a format string is not something that varies across different copies of the tree). Other examples including `ToString` format strings or `Regex.Match` containing a regular expression string.

Once a `ConstantHoister` has been created, the `Hoist` method is used to perform hoisting:

```csharp
public ExpressionWithEnvironment Hoist(Expression expression);
```

This returns an `ExpressionWithEnvironment` which represents the hoisted expression and bindings of introduced variables to constant values:

```csharp
public sealed class ExpressionWithEnvironment : IExpressionWithEnvironment
{
    public Expression Expression { get; }
    public IReadOnlyDictionary<ParameterExpression, object> Environment { get; }

    IReadOnlyList<Binding> IExpressionWithEnvironment.Bindings { get; }
}
```

One typically constructs a `LambdaExpression` from the `Expression` and the keys in the `Environment` (or the `Bindings` list, which guarantees a stable order), and uses the resulting expression as a key for lookups used to check for existing expressions that have the same shape, except for constants that were outlined.

Alternatively, one can use `ToInvocation` to obtain an `Invoke(Lambda(...), ...)` expression that can be fed back into a `BetaReducer.Reduce` call to inline the constants again.

### Type substitution

The `TypeSubstitutionExpressionVisitor` can be used to retype an expression tree from one set of types to another set of types. This involves specifying rules to rebind members involving the types being substituted. For example:

```csharp
var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
{
    { typeof(DateTime), typeof(DateTimeOffset) }
});

Expression rewritten = subst.Apply(expr);
```

By default, members are located by finding a member with the same name (and parameter types, after type substitution in child nodes, in case of methods and constructors) on the target type. For example, when rewriting `() => DateTime.Now.AddDays(1)` from `DateTime` to `DateTimeOffset`, both `Now` and `AddDays` will be located successfully.

However, if more advanced rules are needed to retarget members on types, a subclass of `TypeSubstitutionExpressionVisitor` can be used to override various `Resolve*` methods. An an example, consider the method used to resolve a `MethodInfo`:

```csharp
protected virtual MethodInfo ResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType);
```

Here, we're given the original method in the input expression tree, as well as all of the type constraints that are required to be met by the newly returned method to be used in the output expression tree. These constraints originate from a depth-first rewrite of the input tree, e.g. the target instance and the arguments on a `MethodCallExpression` have been rewritten already.

If a `Resolve` method returns `null`, another virtual call is made to a `FailResolve` method:

```csharp
protected virtual MethodInfo FailResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType);
```

This provides a last chance to resolve the member when the default logic (using reflection) in `ResolveMethod` works in most cases, but only failure cases have to handled manually. The default implementation of `FailResolve` methods is to throw `InvalidOperationException`.

One final extensibility point is the conversion of constants. For example, if an expression contains a `ConstantExpression` of type `DateTime` containing a value of said type, any rewrite to another type (e.g. `DateTimeOffset`) needs to perform a conversion of these constants. This is done through a `ConvertConstant` method:

```csharp
protected virtual Object ConvertConstant(Object originalValue, Type newType);
```

The default implementation only supports assignability checks (using `IsAssignableFrom`) to check for a possible automic conversion, as well as conversions of `null` references. It does not try to invoke implicit conversions. A derived class can specify the exact behavior to use (for example, it could try to construct an expression tree that performs an `Expression.Convert`, or perform partial evaluation of such a conversion to end up with a retyped `ConstantExpression`).

> **Note:** Type substitution is used in Nuqleon when erasing anonymous types or data model types for custom generated types (e.g. replacing `MappingAttribute`-annoted properties on a nominal type for a generated structural type using the names specified in such mappings). This is both applicable in the `Expression` as well as the `ExpressionSlim` space. Another example is finding anonymous types in a query expression and replacing them by tuple types, which involves resolving properties on anonymous types to tuple `Item*` member access expressions.

### Tupletization

One of the drawbacks of delegate types in .NET is the lack of support for variadic generics, causing us to end up with a whole ladder of types to support functions with different numbers of parameters (cf. `Action<...>` and `Func<...>` families).

While expression trees support the creation of `LambdaExpression` nodes with any number of parameters, it relies on runtime code generation (using `System.Reflection.Emit`) to manufacture delegate types of arities that exceed the available `Func<>` and `Action<>` types. This results in types that are tricky to carry along across machine boundaries (though Bonsai supports ways to represent function types as first-class citizens).

> **Note:** One can argue that functions with more than 16 parameters should be avoided, but keep in mind that expressions are often machine generated. For example, when performing constant hoisting, the number of parameters generated is linear in the number of constants that occurred in an expression.

Furthermore, having to support various arities of functions on APIs can be cumbersome. For example, consider a `DefineObservable<TArg1, ..., TArgN, TResult>` function that has to provide 16 overloads. While this may still be desirable for user convenience, it becomes less appealing for cases where a call to such methods is generated by another component in the system, having to perform a binding step to pick the right overload. Therefore, it's desirable to have a "normal form" that enables squeezing N-ary functions through a single unary function.

Various ways to normalize are possible, including the use of curried functions (`(x, y) => x + y` becomes `x => y => x + y`), the use of record types (`(x, y) => x + y` becomes `p => p.x + p.y`), or the use of tuple types whereby a parameter list is "materialized" as a tuple (`(x, y) => x + y` becomes `t => t.Item1 + t.Item2`). The `ExpressionTupletizer` type provides facilities to work with this normal form.

> **Note:** Nuqleon has standardized on tuples as the normal form, but spin-offs of the technology (ICP , i.e. `IComputationProcessing`, a generalization of IRP, i.e. `IReactiveProcessing`) have also implemented support for record normal form (where function parameters are composite record-based entities themelves) and curry normal form (which fits naturally with functional programming front-end languages).

In order to transform a `LambdaExpression` to its tuple form, use the `ExpressionTupletizer.Pack` method, as shown below:

```csharp
Expression<Func<int, int, int>> f = (x, y) => x + y;

var g = (Expression<Func<Tuple<int, int>, int>>)ExpressionTupletizer.Pack(f);
```

The resulting expression will look like `t => t.Item1 + t.Item2`, which can now be squeezed through APIs that accept a `Func<TArgs, TResult>` parameter (or an `Expression<>` variant thereof).

> **Note:** Tupletization was introduced prior to the availability of `ValueTuple<>` types and hence uses `Tuple<>` types instead. Support for value tuples can be introduced, albeit separately, because the current behavior ensures read-only usage of parameters because `Item*` properties on tuples are `get`-only (and they are properties, so they can't be used - safely - for `ref` parameters either). This restriction was intentional because the use of tupletization in Nuqleon applied to expressions with a functional programming nature. When introducing support for value tuples (as an alternative, not a replacemnet), assignments will also be allowed.

The reverse operation is also possible using `Unpack`. For example:

```csharp
var h = (Expression<Func<int, int, int>>)ExpressionTupletizer.Unpack(g);
```

Putting these pieces together enables flowing a normal form through various layers of the system, only ever having to worry about unary functions, while still going back to the original form in some back-end system (e.g. right before compiling an expression where the construction and deconstruction of tuple-based values is deemed an unneccesary performance hit)

To deal with lambda expressions that have no parameters, the `Pack` and `Unpack` methods support an additional `Type` parameter named `voidType`. This can be used to insert a dummy unused parameter (of the specified type) in case the given lambda expression has no parameters. Also note that unary functions get rewritten to use a `Tuple<T1>` type.

Other overloads of `Pack` enable conversion of a `params Expression[]` to a single expression that uses a tuple to combine all of the expressions (which should have non-`void` types) into a single tuple creation expression. The inverse `Unpack` operation can be used to get the original array of expressions back (i.e. it applies a deconstruction).

> **Note:** Tuples support any number of components through the use of the `Rest` properties which can nest arbitrarily deep. `Pack` and `Unpack` support the use of `Rest` properties to nest tuples.

Another utility that works with tuples is `AnonynmousTypeTupletizer`. This expression rewriter is a type substitution visitor that replaces anonymous types (e.g. produced by `let` clauses in query expression, due to the use of transparent identifiers) by tuple types. This can be useful to shake off compiler-generated types prior to carrying out further rewrite steps (or prior to serialization of the expression for shipment to another machine). To use this functionality, use the `Tupletize` static method:

```csharp
public static Expression Tupletize(Expression expression, Expression unitValue);
public static Expression Tupletize(Expression expression, Expression unitValue, bool excludeVisibleTypes);
```

The `unitValue` is used to deal with empty anonymous types (`new {}` in C#), and `excludeVisibleTypes` is used to prevent rewriting of anonymous types that "leak" out of the expression (e.g. `xs.Select(x => new { x })` causes the anonymous type to appear on the type of the entire expression, so it's not just used within the expression). As an example, consider a query like this:

```csharp
from x in xs
let y = x + 1
select x + y
```

which is lowered into:

```csharp
xs
.Select(x => new { x, y = x + 1 })
.Select(t => t.x + t.y)
```

After applying tupletization, the resulting expression will be similar to this:

```csharp
xs
.Select(x => new Tuple<int, int>(x, x + 1))
.Select(t => t.Item1 + t.Item2)
```

except that the `NewExpression` for the tuple creation will have its `Members` collection set to properties `Item1` and `Item2`. This in turn enables query providers to make mappings from the arguments passed to the constructor to the properties they get exposed on (note: this is where the term *transparent* in transparent identifiers comes from).

### CPS transformations

This library provides very rudimentary support for continuation passing style (CPS) transforms of code which can be used in the context of binding query execution plans to asynchronous infrastructure.

> **Note:** The CPS transform support in this library only works for expressions because it originates from a LINQ provider toolkit we wrote in the .NET 3.5 days, prior to support for statement nodes. A full-blown CPS transformation framework existed in the context of Volta's tier splitting work, but it was written in CCI (the Common Compiler Infrastructure) and never got ported to .NET 4.0's expressions. While Nuqleon doesn't use this implementation of CPS transform directly (anymore), we're keeping it around for other uses (e.g. execution plans in some internal stores). A more complete CPS transformation framework could be built, especially in conjunction with the work on modernized expression trees with support for async lambdas and `await` expressions.

CPS transforms involve rewriting method invocations such as `Add(a, b)` to `Add(a, b, ret => ...)` where the result is provided through a callback rather than a returned result. This then enables the use of asynchronous functions.

> **Note:** This functionality has been used for execution plans in stores that have asynchronous callback-based `Get`, `Read`, `Enumerate`, etc. operations, but the user's code uses synchronous operations instead. By enabling rewrites to synchronous stub methods (e.g. `T Get<T>(string key)`) and annotating these stubs with a `UseAsyncMethod` attribute, the CPS transformation framework performs a set of tree rewrites that replace the stub method calls with their async counterparts (e.g. `void Get<T>(string key, Action<T>)`).

See `ClassicCpsRewriter` and `ClassicCpsRewriterWithErrorPropagation` for more details.

> **Note:** Other implementations of CPS transforms using `Begin`/`End` patterns or `Task<T>` can be built using the same mechanisms, but it's relatively easy to write converters from the trivial callback-based model to any of these other patterns (at the expense of another layer of indirection).

## Expression tree evaluation

Expression trees support compilation and evaluation through the `Expression<T>.Compile` method. This is a very powerful mechanism that enables highly efficient JIT-compiled code to be emitted, using `System.Reflection.Emit` under the hood. However, compiling a lot of expression trees can be quite expensive, both due to calling into the CLR's JIT (possibly deferred until the compiled delegate gets invoked), and the memory usage for the compiled expressions. This is particularly wasteful if a lot of expressions are the same or very similar.

The compiled delegate cache support in this library tries to reuse already compiled expressions in order to reduce the cost of expression tree compilation (by avoiding it, if possible) and to increase the density when many expressions are compiled and ran.

> **Note:** The query engine in Nuqleon is all built around the idea of compiling expression trees representing standing query expressions. Compiling these expressions (rather than interpreting them one way or another) is very beneficial because many expressions are invoked for every single event flowing through an observable sequence (e.g. predicates passed to `Where`, selectors passed to `Select`, etc.).

To compile expressions with support for compiled delegate caching, use the `CachedLambdaCompiler.Compile` method, which looks as follows:

```csharp
public static T Compile<T>(this Expression<T> expression, ICompiledDelegateCache cache, bool outliningEnabled, IConstantHoister hoister);
```

Various overloads exist that reduce the number of options, or that deal with `LambdaExpression` rather than `Expression<T>`. The top-level structure is the same as the `Compile` instance method on `Expression<T>`, namely that the method returns a delegate of type `T`. The method differs in its caching behavior which is controlled through 3 parameters:

* `cache` to specify a cache (and a policy) to hold on to compiled delegates;
* `outliningEnabled` can make the cache more efficient by outlining nested lambdas and compiling them separately;
* `hoister` is used to perform constant hoisting to make trees equivalent modulo constants.

For example, when outlining is enabled and a constant hoister is specified, an expression like:

```csharp
y => f(x => x + 1, y + 2)
```

will first get decomposed into smaller expressions for `x => x + 1` and `y => f(d, y + 2)` (where d is a constant that will hold the compiled delegate for `x => x + 1`). This technique enables reuse of `x => x + 1` in case it occurs in other unrelated expressions.

> **Note:** Outlining is quite beneficial in stream processing systems, especially after steps to normalize queries. For example, one query may write `xs.Where(x => x > 0 && x < 10)` while another one may write `xs.Take(5).Where(x => x > 0)`. By normalizing the first query to `xs.Where(x => x > 0).Where(x => x < 10)` we end up with smaller expressions that have a higher likelihood of being reused (in addition, things like `0 < x` can be rewritten to `x < 0`, i.e. some normal form). Now both queries contain `x => x > 0` and the compiled delegate for this predicate can be reused.

Next, the resulting lambda expressions are constant-hoisted, producing `c0 => (x => x + c0)` and `(c1, c2) => (y => f(c1, y + c2))`, where `c0`, `c1`, and `c2` are variables that replace the constants that used to be in that position. Note that the outlined delegate (for `x => x + 1`) itself became a constant, so any other expression of the form `y => f(*, y + *)` (where `*` is a placeholder for any constant) can match the reused compiled delegate.

Finally, a lookup for the constant-hoisted expressions is carried out against the supplied cache. If a match is found, the compiled delegate is retrieved and an invocation is made to provide the constants that were hoisted out. Otherwise, compilation is carried out, and the resulting delegate is added to the cache.

Three built-in caches are provided:

* `VoidCompiledDelegateCache` does not cache anything; it's useful for debugging, performance analysis, or to disable caching without changing the core `Compile` code.
* `SimpleCompiledDelegateCache` is unbounded; it can be cleared externally by calling `Clear`, based on some user policy. (If not cleared, it can be a source of leaks.)
* `LeastRecentlyUsedCompiledDelegateCache` uses an LRU eviction policy. A capacity is specified on the constructor. When the entries in the cache exceed the threshold, the least recently used one is evicted.

> **Note:** It's relatively straightforward to build other caching policies by implementing `ICompiledDelegateCache`, for example based on the hit frequency for entries, to evict the least commonly used entries first.

## Expression tree optimization

This library provides a few expression tree optimizations. More optimizations are available in `Nuqleon.Linq.Expressions.Optimizers`.

### Delegate invocation inlining

The `DelegateInvocationInliner` provides a very narrow optimization that looks for `InvocationExpression` nodes whose target is a `ConstantExpression` containing a delegate-typed expression. If the delegate has an invocation list with a single invocation target (i.e. it's not a multicast delegate with multiple invocation targets attached to it), the tree is rewritten to a `MethodCallExpression` for the method targeted by the delegate.

This optimization can be useful after carrying out binding steps where the binding targets for functions are `ConstantExpression`s containing delegates pointing at a function's implementation. For example:

```csharp
f(1)
```

where `f` is an unbound variable of type `Func<int, int>`. During a binding step, `f` is resolved to a `ConstantExpression` containing a delegate of type `Func<int, int>` (e.g. `Math.Abs`). When rewriting the expression using lambda lifting and lambda application, we end up with the equivalent of:

```csharp
((Func<int, int> f) => f(1))(new Func<int, int>(Math.Abs))
```

except that the `new` expression is not represented by a `NewExpression` but rather by a `ConstantExpression` containing the delegate instance.

When applying `DelegateInvocationInliner.Apply` to this expression, the resulting expression tree will be equivalent to:

```csharp
Math.Abs(1)
```

and thus avoid the indirection of delegate invocation.

> **Note:** Some implementations of Nuqleon query engines have historically used delegate values for binding targets, rather than `LambdaExpression`s that can get inlined by using `BetaReducer`. Both approaches can be optimized well.

### Expression tree interning

Because expression trees are immutable, they can be shared safely. For example, if two expressions have a common subexpression, the same object can be used to represent that common subexpression:

```csharp
f() + g() + h()
```

and

```csharp
f() + g() + i()
```

can reuse the common subexpression `f() + g()`. However, it's not always possible to figure out that two or more expressions have commonalities that present opportunities for sharing. For example, when expression visitors are used to manipulate expression trees, commonalities may not occur until rewrites have completed.

Expression tree interning allows for the detection of common subexpressions across a "forest" of trees, and rewrite expressions to allow for reuse of common subexpressions, thus reducing the memory utilitization. This is very similar to `string.Intern`, although the latter only operates on complete strings, while expression tree interning can consider every subexpression.

> **Note:** Interning is quite expensive due to the computations involved to figure out commonalities between expressions. It's only recommended to use interning if expressions are long-lived. In the context of Nuqleon, this is most useful when expression trees are kept alive in registries or on quotations that are needed to support higher-order query operators (e.g. in `xs.SelectMany(x => f(x))`, the expression `x => f(x)` is kept - even after compilation to a delegate - because inner subscriptions need to be able to construct an expression, which will get checkpointed, that represents the observable `f(x)` for a given value of `x`).

The entrypoint of the interning functionality is the `ExpressionInterning.Intern` method:

```csharp
public static TExpression Intern<TExpression>(this TExpression expression, IExpressionInterningCache cache)
    where TExpression : Expression;
```

When a cache is omitted, a global cache is used. Alternatively, an instance of `ExpressionInterningCache` or a custom implementation of `IExpressionInterningCache` can be provided.

Similar to `string.Intern`, the `Intern` method returns an object of the original type which should be used instead of the object that was passed in the first parameter. If interning was successful, the returned will be a different instance than the one that was passed in. The original instance is no longer needed and can get garbage collected.

For example:

```csharp
var expr1 = Expression.Add(Expression.Constant(1), Expression.Constant(2));
expr1 = ExpressionInterning.Intern(expr1);

var expr2 = Expression.Multiply(Expression.Constant(2), Expression.Constant(3));
expr2 = ExpressionInterning.Intern(expr2);
```

After interning both expressions, `expr2.Left` will be reference equal to `expr1.Right`, because these nodes can be shared. Interning doesn't just work for leaf nodes, it works for subexpressions (in a bottom-up fashion) of any size. For example, if another expression represents `f(1 + 2, 2 * 3)`, with unique nodes for `1`, `2`, `1 + 2`, `2`, `3`, and `2 + 3`, interning can substitute the subexpressions `1 + 2` and `2 * 3` using `expr1` and `expr2` from the sample above.

## Diagnostics

To assist with logging or debugging of expression trees, this library provides a `ToCSharpString` extension method for `Expression` which produces C#-like syntax to represent the tree. Not all constructs in expression trees can be (correctly) represented in C# (e.g. `LoopExpression` having a result), so the resulting syntax just looks and feels like C#.

An additional Boolean parameter called `allowCompilerGeneratedNames` can be passed to `ToCSharpString` to control whether the resulting string can contain compiler-generated names (otherwise, an exception will be thrown when encountering such a name).

Alternatively, the `ToCSharp` method can be used which returns a `CSharpExpression`, providing additional information about the tree, alongside a string representation. This additional information includes a table of `Constants` as well as a list of `GlobalParameters`.

> **Note:** Better expression tree printing support is available in https://github.com/bartdesmet/ExpressionFutures/tree/master/CSharpExpressions.

## BURS

BURS stands for Bottom-Up Rewrite System and is based on the https://www.researchgate.net/publication/220752446_Simple_and_Efficient_BURS_Table_Generation paper. It provides for a table-driven weight-based approach to generate code. The tables contain rules which consist of patterns to recognize, a cost, and a rewrite rule.

This library generalizes the BURS mechanism to conversions between a generalized notion of trees, represented as `ITree<T>`. In this context, BURS is a table-driven way to build a compiler that translates between an `ITree<T>` to an `ITree<R>`. The special case of rewriting from an `ITree<T>` to an `ITree<T>` is a rule-driven optimizer.

As an example, consider a first `ITree<T>` where `T` represents node information for a tree of some arithmetic language providing unary operators `+` and `-`, as well as binary operators `+`, `-`, `*`, and `%`. Next, consider a second `ITree<R>` where `R` represents node information for a tree of some broader scientific computing language, which may provide operations such as `Negate`, `Add`, `Inverse`, and `Multiply`, etc. In this language, subtraction is represented as an `Add` with the second operand being `Negate`d first. Similarly, `Divide` is represented as a `Multiply` with the second operand being `Inverse`d first. Using BURS, we can formulate rewrite rules, like this:

```
a + b    =>    Add(a, b)
a - b    =>    Add(a, Negate(b))
a * b    =>    Multiply(a, b)
a / b    =>    Multiply(a, Inverse(b))
```

Both sides really represent trees; the use of infix `+` versus prefix `Add` is just a means to denote different domains. Left is an arithmetic language, right is a different kind of math language.

In this example, the patterns on the left are pretty simple. They simply state to look for nodes such as `+`, `-`, etc. and recursively rewrite the left and right operands (represented as "variables" `a` and `b`). A rewrite across different domains requires an input tree to be fully covered by rules in the table in order to produce a tree in the output domain.

Most likely, our input and output languages also support symbolic variables and constant numbers. In the sample, `a` and `b` are really just used as wildcards to represent "any subtree" in the input domain, and the result of rewriting these subtrees for construction of a tree in the output domain. To add support for nodes such as `Constant` or `Variable`, extra rules can be provided:

```
c (where c is a constant)  =>  Const(/* value of c */)
v (where v is a variable)  =>  Variable(/* name of v */)
```

For now, let's ignore the details on how to encode the constraints, but let's assume a tree in the input domain that looks like:

```
(x - y) * 2 + (z - 1)
```

where `x`, `y`, and `z` are variables, and `2` and `1` are constants. Given the rule tables above, the BURS rewriter will try to cover the tree in a bottom-up fashion:

```
          + 
       /     \
      *       -
    /   \   /   \
   -     2 z     1
 /   \
x     y
```

First `x` will turn into `Variable("x")`, then `y` will turn into `Variable("y")`. Next, the `-` node combines the result of recursively rewriting `x` and `y` by applying the rule with production `Add(a, Negate(b))`, thus yielding `Add(Variable("x"), Negate(Variable("y")))`. In a next step, `2` gets converted to `Const(2)`, then `*`, etc. The ultimate result is:

```
Add(Multiply(Add(Variable("x"), Negate(Variable("y"))), Const(2)), Add(Variable("z"), Negate(Const(1))))
```

BURS can also be used to write optimizers. For example, assume we want to simplify expressions in the math language. We could then construct rules tables that look like this:

```
Negate(Const(c))    =>    Const(-c)
Negate(Negate(a))   =>    a
```

This illustrates that patterns expressed on the left hand side can be more complex and deeply nested. In this case, we match two levels deep, but much more complex rules could be written. For example, if we have a Boolean logic language, we could express De Morgan's rules:

```
Not(And(Not(a), Not(b)))   =>   Or(a, b)
Not(Or(Not(a), Not(b)))    =>   And(a, b)
```

Building on the earlier example, we could use BURS to create a reverse translation as well, which involves more complex patterns to match on:

```
Add(a, b)                 =>  a + b
Add(a, Negate(b))         =>  a - b
Multiply(a, b)            =>  a * b
Multiply(a, Inverse(b))   =>  a / b
```

However, in this case, we also need to supply a way to support `Inverse` and `Negate` when these occur by themselves, which contributes rules like these:

```
Negate(a)                 =>  0 - a
Inverse(a)                =>  1 / a
```

To prefer one pattern over another, rules can get weights assigned. In the original context of BURS weights typically corresponded to the cost of the emitted code for a pattern (e.g. if an instruction set supports some `addmul` ternary operation, it may be cheaper than successive applications of `add` and `mul` operations, thus a rule producing `addmul` would have a cost that's lower than the sum of `add` and `mul` individually).

BURS support in this library is provided through types such as `BottomUpRewriter<...>`. An example taken from test code is shown below:

```csharp
var burw = new BottomUpRewriter<ArithExpr, ArithNodeType, NumExpr, ArithWildcardFactory>
{
    // Leaf nodes
    Leaves =
    {
        { (Const c) => new Val(c.Value), 1 },
    },

    // Tree patterns
    Rules =
    {
        { (l, r) => new Add(l, r), (l, r) => new Plus(l, r), 2 },
        { (l, r) => new Mul(l, r), (l, r) => new Times(l, r), 3 },
        { (a, b, c) => new Add(new Mul(a, b), c), (a, b, c) => new TimesPlus(a, b, c), 4 },
        { x => new Add(x, new Const(1)), x => new Inc(x), 1 },
    },

    Log = logger
};
```

In here, we add rules for leaf nodes (through `Leaves`) like constants (or variables, as we alluded to in the earlier examples), as well as for tree patterns (through `Rules`). Such rules are triplets of a pattern to match on in the source domain, a production in the target domain, and a weight.

Once a BURS rewriter has been constructed with all tables populated, the rewriter can be applied to an input tree, producing an output tree (or an error). For example:

```csharp
// (((2 * 3) + 1) * (4 * 5)) + 6
var e = new Add(
    new Mul(
        new Add(
            new Mul(
                new Const(2),
                new Const(3)
            ),
            new Const(1)
        ),
        new Mul(
            new Const(4),
            new Const(5)
        )
    ),
    new Const(6)
);

var res = burw.Rewrite(e);
```

In this example, the input is an `ArithExpr`, and the output in `res` is of type `NumExpr`. The result for the rewrite shown above is:

```
TimesPlus(Inc(Times(2, 3)), Times(4, 5), 6)
```

where one can observethe selection of the third rewrite rule for trees of the form `a * b + c` because the weight of that rule (`4`) is less than the cumulative weight (`5`) for successive applications of rewrite rules for `l * r` (weight `3`) and `l + r` (weight `2`).

To work with BURS, trees have to be implemented using the `ITree<T>` abstraction. To use BURS over `System.Linq.Expressions.Expression` trees, the library provides a `ToExpressionTree` conversion from `Expression` to `ITree<ExpressionTreeNode>`.

> **Note:** BURS has been used to build table-driven query providers (e.g. going from expression trees to some `ITree<SqlNode>` for translation to SQL) and optimizers. Mileage varies depending on the complexity of the rule matching involved. Further extensions of BURS have been written in spin-off projects, modeling type system checks (e.g. how does a rule involving a method invocation `object.Equals(object)` relate to rules that involve an override of this virtual method on a more derived type?), supporting additional predicates to drive the rule selection process, and dynamic computation of weights.

## Miscellaneous utilities

This section contains documentation for utilities that don't really fit in another other section.

### `ReflectionHelpers`

Provides a set of `InfoOf` methods that obtain a `MemberInfo` from an expression tree. This is a mechanism akin to `typeof` for types but targeting methods, properties, fields, and constructors instead (much like a hypothethical C# `infoof` operator could do). For example:

```csharp
var m = (MethodInfo)ReflectionHelpers.InfoOf((string s, int x, int y) => s.Substring(x, y));
```

will return the `MethodInfo` representing `string.Substring(int, int)`.

### `RuntimeCompiler`

The runtime compiler uses `System.Reflection.Emit` to build anonymous types, closure types, and so-called record types.

#### Anonymous types

Runtime-generated anonymous types are analogous to C# 3.0 and VB 9.0 anonymous types. Both flavors can be built using different overloads of `CreateAnonymousType`:

```csharp
public static Type CreateAnonymousType(IEnumerable<KeyValuePair<string, Type>> properties);
public static Type CreateAnonymousType(IEnumerable<StructuralFieldDeclaration> properties);

public static Type CreateAnonymousType(IEnumerable<KeyValuePair<string, Type>> properties, params string[] keys);
public static Type CreateAnonymousType(IEnumerable<StructuralFieldDeclaration> properties, params string[] keys);
```

Overloads with `StructuralFieldDeclaration` support adding custom attributes to the generated properties. The difference between overloads that lack a `keys` parameter versus the ones that have one has to do with the properties that participate in the implementation for `Equals` and `GetHashCode`. Being able to specify particular properties as "keys" matches the design of anonymous types in Visual Basic.

#### Closure types

Closure types are simply classes that declare a bunch of fields. To create a closure type at runtime, use the `CreateClosureType` method:

```csharp
public static Type CreateClosureType(IEnumerable<KeyValuePair<string, Type>> fields);
```

#### Record types

The notion of record types in this library predates C# 9.0's record types by almost a decade. Record types in this library are classes that are similar to anonymous types but provide control over the implementation of equality (value versus reference equality). Use the `CreateRecordType` method to create them:

```csharp
public static Type CreateRecordType(IEnumerable<KeyValuePair<string, Type>> properties, bool valueEquality);
public static Type CreateRecordType(IEnumerable<StructuralFieldDeclaration> properties, bool valueEquality);
```

#### `Define*` method variants

In addition to the `Create*` methods illustrated above, variants with the `Define*` prefix are provided as well. Rather than returning a `Type`, these accept a `TypeBuilder` to define the type on. These variants are useful when trying to build recursive types (i.e. there's a cycle between declarations and uses of types), because one can use `TypeBuilder` instances for the types of the properties on the anonymous or record type being constructed. Once all types have been defined, the user can then call `CreateType` on the `TypeBuilder` instances. Examples of types with cycles are:

```csharp
// A -> A
class A
{
    public A Next { get; set; }
}

// B -> C
class B
{
    public C C { get; set; }
}

// C -> B
class C
{
    public B B { get; set; }
}
```
