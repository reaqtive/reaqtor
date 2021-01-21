# Nuqleon.DataModel

Provides a minimal set of types to declare Nuqleon data model compliant entity types.

## Basic concepts

The Nuqleon data model provides an approach to deal with structural data in the form of entity types. Structural types are in contrast to *nominal types*, where the name of the declaring type is used to establish typing relationships (e.g. `Bar` inherits from `Foo`). Instead, *structural types* have a typing relationship that's defined in terms of set theory relationships involving the sets of members on such types. For example, a 2D point containing `int x` and `int y` can be considered as related to a 3D point containg `int x`, `int y`, and `int z`. A typical relationship is assignability, whereby a 3D point value can be assigned to a variable typed as a 2D point.

> **Note:** The Nuqleon data model predates any language support for modeling data "records" as is done in C# 9.0. However, the record types introduced in C# 9.0 still follow a nominal typing philosophy and are indeed just classes under the hood. A better comparison can be found in TypeScript which has deep support for structural typing, inherited from the underlying JavaScript execution environment. In fact, the Bonsai expression tree serialization format can be used to interoperate with JavaScript and TypeScript due to its first-class representation of structural types (in addition to supporting nominal types). The roots of the Nuqleon data model go back to the development of graph and document databases where the entries in the database are modeled as JSON values. Versioning of schema required the ability to add "fields" to existing objects. Furthermore, the use of .NET clients for these databases requires a way to model the entity types as .NET types.

In order to support a structural typing approach, the Nuqleon data model starts from the JSON data model where objects are purely structural, i.e. they don't have a name and solely consist of a map of property names to values. As such, the JSON "type system" (reflecting the possible values that can be constructed in JSON) consists of:

* primitives, e.g. null, Boolean values, numbers, and strings.
* single-dimensional arrays where the elements are JSON values themselves.
* objects that map string-based properties names onto JSON values.

Written down in a more formal pseudo-grammar, we get:

```
json := primitive
      | array
      | object
      ;

primitive := null
           | bool
           | number
           | string
           ;

array := json []

object := { property* }

property := "name" : json ,
```

As an example, consider the following value:

```
{
    "name": "Bart",
    "age": 10,
    "address":
    {
        "street": "Evergreen Terrace",
        "number": 742,
        "city": "Springfield",
        "zip": "80085",
        "state": "NT"
    },
    "hobbies":
    [
        "skateboarding",
        "television",
        "comic books",
        "mischief"
    ]
}
```

Note that the object itself doesn't have a type name. The same holds for the `address` field, which just comprises of a list of properties that map names onto values. Some uses of this value may just want to look at the `name` and `age` properties, while newer values of this "person" entity may have additional fields added. One of the goals of the data model is to allow for projection of these structures onto .NET types, while ignoring the nominal nature of .NET types, and allowing for differences between producers and consumers of values, in order to allow for versioning. A possible way to declare an object with the same structure in C# is shown below:

```csharp
new Person
{
    Name = "Bart",
    Age = 10,
    City = new City
    {
        Street = "Evergreen Terrace",
        Number = 742,
        City = "Springfield",
        Zip = "80085",
        State = States.NorthTakoma
    },
    Hobbies = new string[]
    {
        "skateboarding",
        "television",
        "comic books",
        "mischief"
    }
}
```

A few discrepancies show up. First, the use of type names such as `Person` and `City`. Second, the use of properties that have different casing. And finally, the use of enums such as `States`, which makes it more natural to model some values. The Nuqleon data model needs to deal with these while performing a mapping between data model values and their corresponding .NET type projections.

## Relationship to Bonsai expression trees

Expression tree serialization using Bonsai indirectly leverages the Nuqleon data model facilities. While there is no static dependency of Bonsai on the Nuqleon data model, various expression tree rewrites take place prior to serialization and after deserialization in order to deal with the mapping from nominal .NET types onto structural data types.

At the level of Bonsai, support exists for structural types in the form of records (with the `{}` and `{;}` discriminators in type table entries). During the conversion of an `Expression` to an `ExpressionSlim` (prior to Bonsai serialization), nominal .NET types (of type `System.Type`) can be turned into their equivalent structural types (using `System.TypeSlim` which supports structural types). This rewrite is facilitated by utilities in `Nuqleon.DataModel.CompilerServices`. This process is referred to as *anonymization* or *recordization*.

Similarly, when deserializing a Bonsai tree followed by conversion from `ExpressionSlim` to `Expression`, structural types can be rebound to nominal .NET types (thus allowing the resulting expression tree to be compiled and evaluated) in a variety of ways. One approach is to perform binding of global unbound variables to known artifacts (e.g. binding a stream `reaqtor://streams/weather` to a subject of type `MultiSubject<Weather>` in the context of Reaqtor), which allows for type inference (e.g. `Weather` in the preceding example). In such a setup, the resulting type gets substituted for the original structural type in the `ExpressionSlim`. Another approach is to materialize structural types by use of runtime compilation of a fresh type (using `System.Reflection.Emit`). Yet another approach is to bind all structural types to some `ExpandoObject` and rewrite expressions that use property accesses (e.g. `w.Temperature`) to use indexers and conversions (e.g. `(double)w["temperature"]`) instead.

To summarize, the serialiation and deserialization of expressions involving data model types is a two-step process. The first step is to convert between `Expression` and `ExpressionSlim` where support for structural typing is introduced. The next step is to convert to and from the Bonsai format. The conversions between `Expression` and `ExpressionSlim` involve detection of data model entity types to perform conversions between `Type` and `TypeSlim`.

## MappingAttribute

The `Nuqleon.DataModel` library has very few types, the most important of which is `MappingAttribute`. This custom attribute can be applied to fields, properties, and (constructor) parameters to perform a mapping from .NET field, property, or parameter names to an underlying name. These names are then used when referring to the properties in the `ExpressionSlim` and `TypeSlim` space. For example:

```csharp
class Person
{
    [Mapping("name")]
    public string Name { get; set; }

    [Mapping("age")]
    public int Age { get; set; }
}
```

corresponds to a data model entity type with the following structure:

```json
{ "name" : int, "age" : number }
```

In other words, the nominal type's name `Person` is entirely ignored, and the names specified on `MappingAttribute`s are used for the property names. As such, an alternative .NET type with the same properties is structurally equivalent to this type:

```csharp
class Persoon
{
    [Mapping("name")]
    public string Naam { get; set; }

    [Mapping("age")]
    public int Leeftijd { get; set; }
}
```

In the context of Reaqtor, when users write queries using entity types (e.g. an `IAsyncReactiveQbservable<Person>`), the expression tree normalization and rewrite steps will erase the `Person` type in favor of its structural type definition at the level of Bonsai. When the query reaches a query evaluator node, binding steps are carried out to bind the expression to runtime artifacts (e.g. a `MultiSubject<T>` where the `T` is a structurally compatible type).

When an entity type has a constructor, its parameters should be annotated with `Mapping` attributes as well, as shown below:

```csharp
class Person
{
    public Person([Mapping("name")] string name, [Mapping("age")] int age) => (Name, Age) = (name, age);

    [Mapping("name")]
    public string Name { get; set; }

    [Mapping("age")]
    public int Age { get; set; }
}
```

This is required to make expression tree rewrites that involve `new` expressions work.

> **Note:** The `MappingAttribute`'s parameter is of type `string` but its corresponding property name is `Uri`. Nothing in the Nuqleon Data Model stack requires the string to represent a valid URI. However, many uses of mapping attributes have historically used ontologies such as schema.org to pick names for properties. This, too, is a remnant of the use of the Data Model for a graph and document database.

## KnownTypeAttribute

The `KnownTypeAttribute` is used to annotate a type as "known", which means it isn't subject of expression tree rewrites during *anonymization* or *recordizaton* steps. As a result, uses of the type are treated as references to the nominal type rather than requiring a rewrite to the structurally equivalent representation. This allows for types to flow as-is through layers of expression tree shipping and is especially useful in deployment scenarios.

As an example, consider the deployment of some `WeatherObservable` type in a Reaqtor cluster, defined as follows:

```csharp
class WeatherObservable : ISubscribable<Weather>
{
    public WeatherObservable(string city) => /*...*/

    // ...
}
```

When trying to define an observable reactive entity in a Reaqtor cluster, one uses `DefineObservable` with an expression tree, as shown below:

```csharp
ctx.DefineObservable<string, Weather>(new Uri("reaqtor://weather/city"), city => new WeatherObservable(city).AsObservable())
```

> **Note:** Alternatively, the async variants of `DefineObservable` may be used. Also note the use of `AsObservable` which provides a conversion between `ISubscribable<T>` and `I[Async]ReactiveQbservable<T>`, which is required to push the expression through the `Define` operation. Details of this mechanism are outside the scope of this document.

Underneath the covers of these `IReactive[Proxy]` operations, expression tree rewrite steps take place to try to shake off references to nominal types in favor of their structural equivalents. However, when defining resources such as `WeatherObservable`, we want the type to flow as-is, so we can deploy the assembly containing `WeatherObservable` to nodes in the cluster and get its code behavior carried over. In other words, the `WeatherObservable` type does not represent some data, but has a behavior. While we want data types to be rewritten to structural types (to avoid shipping assemblies containing data types between clients and services), we do not want to touch types that have behaviors. To achieve this, such types are annotated with `KnownType` attributes:

```csharp
[KnownType]
class WeatherObservable : ISubscribable<Weather>
{
    public WeatherObservable(string city) => /*...*/

    // ...
}
```

Types marked with `KnownType` are left untouched by expression rewrites.

## Unit

The `Unit` type is a .NET projection to conveniently represent an empty object akin to `{}` in JSON. All instances of `Unit` compare equal.