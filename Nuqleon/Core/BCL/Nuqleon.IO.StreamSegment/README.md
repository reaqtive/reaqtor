# `Nuqleon.IO.StreamSegment`

Provides a type providing a view into a `System.IO.Stream`.

> **Note:** This type predates the introduction of `Span<T>` APIs in .NET, which may provide a valid alternative.

## `StreamSegment`

A `StreamSegment` is similar to an `ArraySegment<T>`. It provides a view over a `System.IO.Stream` given an offset and a length. All accesses are bounds checked and get forwarded to the underlying stream using the specified offset.

```csharp
Stream segment = new StreamSegment(stream, offset: 16, count: 32);

var bytes = new byte[4];
int count = segment.Read(bytes, 8, 4); // reads in stream at offset 24
```

> This type is used by Reactor Core for recovery of operator state, which are stored with a length prefix encoding. The length is used to create a stream segment that gets passed to the operator to recover state from.
