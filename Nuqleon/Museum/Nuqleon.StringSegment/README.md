# Nuqleon.StringSegment

Provides a type providing a view into a `System.String` to avoid allocations when performing string operations, e.g. `Substring`.

> **Note:** This type predates the introduction of `Span<T>` APIs in .NET, which may provide a valid alternative.

## StringSegment

A `StringSegment` is similar to an `ArraySegment<char>`. It provides a view over a `System.String` given an offset and a length. All accesses are bounds checked and get forwarded to the underlying string using the specified offset.

All APIs on `System.String` are supported, so a `StringSegment` can be used as a drop-in replacement for a `System.String`. A call to `ToString` will allocate a fresh `System.String` containing the substring of the original underlying string.

```csharp
StringSegment segment = new StringSegment("foobar");

StringSegment res = segment.Substring(1, 3);
```

> This type is useful when building efficient parsers.

## Why is this in the Museum?

Changes were made to how [System.String.Remove](https://github.com/reaqtive/reaqtor/issues/124) works in .NET 6.0. This caused breaking changes in Nuqleon.StringSegment. As `StringSegment` is not actively used in the Reaqtor codebase, and was included as possible a building block. `StringSegment` has been superseded by `Span<T>`. the decision was made to not 
