# `ExpressionJit`

A just-in-time compiler for expression trees.

## History

Expression trees often have nested lambda expressions. For example, when evaluating a bound reactive query expression, one often ends up invoking `Expression<T>.Compile` on an expression like this:

```csharp
() => xs.Where(x => x > 0).Select(x => x * x)
```

This top-level compilation results in recursive compilation of the inner lambda expressions `x => x > 0` and `x => x * x` to delegates, which are subsequently passed to `Where` and `Select` method calls. (Note this is only the case if these lambdas are assigned to delegate types and not to expression tree types, in which case a `Quote` node is present, and an object of type `Expression` is passed instead.)

However some of these nested lambda expressions may not be invoked at all, or only much later. In the example shown above, the `x => x * x` selector may never run if all values of `x` are `<= 0`. Similarly, the filter `x => x > 0` will not be invoked until an event arrives. As such, time could be saved if these nested lambdas were not to be compiled immediately.

> **Note:** This scenario is very common when recovering a query engine, where the expression trees for all standing queries are deserialized and re-evaluated. In addition to the deserialization and expression tree instantiation cost (including Bonsai conversion and some binding steps), there's the cost of expression tree compilation which is partly mitigated by using compiled delegate caches. An additional cost is due to the top-level compilation which is often more costly (due to code generation and CLR JIT) than simply evaluating the top-level tree using an interpreter. The recent addition of `Compile(bool preferInterpretation)` in .NET can help with the latter. However, inner lambdas will still benefit from (eventual) compilation because they may execute frequently (e.g. for every event received).

This library provides a just-in-time compiler for expression trees by rewriting an expression tree such that all inner lambdas can get lazily compiled. It does so by rewriting the tree to make all closures over variables explicit and by hoisting all nested lambdas up into a so-called method table. All places where these nested lambdas occurred are replaced by thunks which expose a delegate of the required type. The delegate's implementation will trigger compilation of the original lambda expression and will then replace the delegate by the result, such that subsequent invocations go straight to the compiled delegate.

> **Note:** The implementation of the expression JIT makes all closures explicit, so the underlying expression tree interpreter or compiler (in `System.Linq.Expressions`) won't generate its own closure objects. This has the added benefit of being able to reduce the cost of such closures (which in .NET are represented as `object[]` arrays containing `StrongBox<T>` objects), but it also allows for compilation schemes where nested lambda thunks can dynamically switch between interpretation and compilation (or even have the compiled delegate expire if it's called infrequently, thus resetting the thunk). For example, if a lambda gets invoked more than a certain threshold, the thunk replaces the delegate inside of it from an interpreted delegate to a compiled one.

## Usage

The entry point of the library is simply a `Compile(CompilationOptions)` extension method for `Expression<T>` where the `CompilationOptions` enum can be used to control the behavior. For example, when specifying `CompilationOptions.EnableJustInTimeCompilation`, the JIT behavior of this library is enabled.
