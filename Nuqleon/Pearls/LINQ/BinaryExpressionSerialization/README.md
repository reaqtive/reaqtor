# BinaryExpressionSerialization

Binary expression tree serialization for expression trees and Bonsai trees.

## History

Binary expression tree serialization (directly from `System.Linq.Expressions.Expression` or via prior Bonsai conversion to `System.Linq.Expressions.ExpressionSlim`) has been considered as an alternative to traditional JSON-based Bonsai serialization. The main motivation is size rather than performance. An example usage is for checkpoint storage in query evaluator engines. However, the JSON-based format is better for interoperability between languages (allowing for reuse of JSON libraries, e.g. in C++, Python, or directly as objects in JavaScript), and for debugging and diagnostics given its human-readable form.

Alternative approaches have been considered to reduce checkpoint storage size and hence reduce I/O size, including expression tree templates. These suffer from still having pretty big reflection contexts around, though the size of the expression's serialized form tends to be smaller in raw bytes (saving balancing tokens such as `[`, `]`, `{`, `}`, but also textual encoding of numeric values, and extra tokens such as `,` and `"` in JSON).

Over time, a lot of investment was poured into optimizing the JSON stack and the serialization/deserialization code paths (also to reduce heap allocations to a large extent), so the initial performance benefits (in terms of CPU and memory) from the binary format have diminished. However, with some more engineering effort, gains are likely to be had. The continued investment in the JSON format was mostly due to existing state persistence in that format, available tooling for debugging and diagnostics, as well as the universal nature of the format (e.g. used in the mini-Reactor written in C++ that ships in Windows, as well as various JavaScript/TypeScript-based Bonsai utilities).

## Usage

To use this serializer, include `System.Linq.Expressions.Bonsai.Serialization.Binary` and instantiate `ExpressionSerializer` by giving it a serializer for objects (e.g. originating from `ConstantExpression.Value`) and an expression tree factory used for deserialization purposes:

```csharp
var serializer = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);
```

> **Note:** The code can be compiled with or without the `USE_SLIM` symbol defined. This enables the library to be used with `Expression` as well as `ExpressionSlim`. The OSS port of the code only includes a build with `USE_SLIM` enabled at the moment; different project build flavors could be added back, or two class library projects could be made, referencing the same source files but only differing in `USE_SLIM`. Note that more `#if USE_SLIM` should be added to support different namespaces for different build flavors.
