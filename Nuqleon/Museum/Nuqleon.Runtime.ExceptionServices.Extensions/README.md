# Nuqleon.Runtime.ExceptionServices.Extensions

Provides support for exception filters and fault handlers.

> **Note:** This library predates support for exception filters in C# (using `when`).

## Usage

The library is written in IL but can be directly consumed in any .NET language through `TryFault` and `TryFilter<E>` static methods that take delegate parameters representing the `try`, `when`, `catch`, and `fault` blocks.

```csharp
using static Nuqleon.Runtime.ExceptionServices.Helpers;

TryFault(() => { /* do this */ }, () => { /* fault handler */ });
TryFilter<MyException>(() => { /* do this */}, ex => ex.Bar == 42, ex => { /* exception handler */ })
```

This library has been used historically to support `try...catch...when...` and `try...fault...` in the context of expression trees, even when those are being written to disk using an `AssemblyBuilder` (using `System.Reflection.Emit` which lacks proper support for fault handlers). The helper methods in this library can be used as rewrite targets, at the expense of closure allocations.
