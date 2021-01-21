# Nuqleon.Json.Serialization

Provides fast JSON serialization for objects using runtime code compilation for specialized parsers and printers.

> **Note:** The functionality in this assembly has been used to provide high throughput serialization and deserialization using JSON, for objects of a given static type. It allows for efficient skipping of tokens that are irrevelant to the object being deserialized, beating performance of `Newtonsoft.Json` hands down. Alternatives with `System.Text.Json` have not been explored; this assembly predates the inclusion of JSON support in .NET by several years.

## FastJsonSerializerFactory

The `FastJsonSerializerFactory` class acts as a factory for strongly-typed serializers and deserializers using the `CreateSerializer<T>` and `CreateDeserializer<T>` methods. Internally, these methods craft a specialized emitter (to produce JSON text) and a specialized parser (to read JSON text) guided by the shape of the specified type `T`. This runtime code generation is backed by `System.Linq.Expressions` expression trees and `System.Reflection.Emit`.

### CreateSerializer<T>

The `CreateSerializer<T>` method is used to create a serializer for objects of the specified type `T`; its signature looks like:

```csharp
public static IFastJsonSerializer<T> CreateSerializer<T>(INameProvider provider, FastJsonSerializerSettings settings)
```

For the first parameter, a name provider is specified that is used to map properties and fields onto JSON member names in JSON objects. A good default choice is `DefaultNameProvider.Instance` (which uses the `MemberInfo.Name` property) but custom implementations of `INameProvider` could perform mappings based on custom attributes or external mapping tables.

For the second parameter, an object of type `FastJsonSerializerSettings` is used to control the behavior of the serializer. It currently only supports a `FastJsonConcurrencyMode` setting, which is used to influence the thread-safety of the returned serializer. Higher throughput can be obtained when using the single-threaded variant.

Once a serializer is created, the `Serialize` method overloads are used to perform serialization:

```csharp
var serializer = FastJsonSerializerFactory.CreateSerializer<Person>(DefaultNameProvider.Instance, new FastJsonSerializerSettings(FastJsonConcurrencyMode.SingleThreaded));

string json = serializer.Serialize(new Person { Name = "Bart", Age = 21 });
```

Another overload of `Serialize` accepts a `System.IO.TextWriter` to write into.

### CreateDeserializer<T>

The `CreateDeserializer<T>` method is analoous to the `CreateSerializer<T>` method; its signature looks like:

```csharp
public static IFastJsonDeserializer<T> CreateDeserializer<T>(INameResolver resolver, FastJsonSerializerSettings settings)
```

Rather than specifying a name provider, this one accepts a name resolver. Given a property or field, it inquires about the JSON member names (more than one is possible, allowing for schema versioning) to look for while deserializing a JSON object. These strings are used to build an efficient lexer and parser. A good default choice is `DefaultNameResolver.Instance` (which uses the `MemberInfo.Name` property) but custom implementations of `INameResolver` could perform different mappings.

The second parameter of type `FastJsonSerializerSettings` is completely analogous to the `CreateSerializer<T>` case.

Once a deserializer is created, the `Deserialize` method overloads are used to perform deserialization:

```csharp
var deserializer = FastJsonSerializerFactory.CreateDeserializer<Person>(DefaultNamePResolver.Instance, new FastJsonSerializerSettings(FastJsonConcurrencyMode.SingleThreaded));

Person obj = deserializer.Deserialize("{ \"Name\": \"Bart\", \"Age\": 21 }");
```

Another overload of `Deserialize` accepts a `System.IO.TextReader` to read from.

## Benchmarks

Some benchmark results for trivial data (a single `Person` object) are shown below. Differences are more pronounced when dealing with bigger payloads, and especially when "selectively" deserializing a JSON object into a .NET object, i.e. when only a subset of the members in the JSON object are assigned to properties or fields in the .NET object.

### Serialize

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.20236
Intel Xeon E-2176M CPU 2.70GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=5.0.100-rc.2.20479.15
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```

|                    Method |     Mean |    Error |   StdDev |
|-------------------------- |---------:|---------:|---------:|
|                   Nuqleon | 136.0 ns |  2.63 ns |  3.23 ns |
|    Newtonsoft_JsonConvert | 633.4 ns | 11.89 ns | 10.54 ns |
| Newtonsoft_JsonSerializer | 598.0 ns | 10.00 ns |  8.87 ns |
|                SystemText | 345.3 ns |  6.92 ns |  8.75 ns |

### Deserialize

```ini
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.20236
Intel Xeon E-2176M CPU 2.70GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=5.0.100-rc.2.20479.15
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```


|                    Method |       Mean |    Error |   StdDev |
|-------------------------- |-----------:|---------:|---------:|
|                   Nuqleon |   131.5 ns |  1.95 ns |  1.73 ns |
|    Newtonsoft_JsonConvert | 1,021.7 ns | 19.61 ns | 23.35 ns |
| Newtonsoft_JsonSerializer |   967.1 ns | 19.07 ns | 21.20 ns |
|                SystemText |   482.2 ns |  9.43 ns | 12.91 ns |
