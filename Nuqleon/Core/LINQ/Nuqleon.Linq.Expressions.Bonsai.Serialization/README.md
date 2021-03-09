# Nuqleon.Linq.Expressions.Bonsai.Serialization

Provides serialization of Bonsai expressions in JSON and binary form.

## Design

The design of Bonsai serialization is centered around a couple of principles:

* Separate the representation of the tree structure from auxiliary information such as types, members, etc.
* Use LISP-style S-expressions to denote nodes in the tree.
* Making typing optional to enable interoperability with different languages.

An example of a Bonsai tree serialized in JSON is shown below:

```json
{
    "Context":
    {
        "Types":
        [
            ["::", "System.Int32"]
        ]
    },
    "Expression":
    [
        "+",
        [
            ":",
            42,
            0
        ],
        [
            "$",
            "x",
            0
        ]
    ]
}
```

The `Context` provides information about assemblies, types, and members. For example, `Types` contains an array of types, which are represented using arrays themselves. The first element of such arrays is a *discriminator*. In the example above:

```json
["::", "System.Int32"]
```

uses `::` to denote a simple type, which just has a name and an (optional) reference to an assembly in the `Assembly` table. While the names of types are typically inherited from CLR constructs, further rewrite steps can normalize these in a manner similar to expression tree normalization carried out in Nuqleon (e.g. a method call to `Where` becomes an invocation expression of an unbound function variable named `rx://filter`).

> **Note:** Various deployments of Nuqleon has been augmented with further Bonsai expression normalization steps based on the interoperability targets involved. For example, when shipping expressions down from the cloud service to devices (e.g. phones running signal processing for GPS sensors, etc.), there's an interoperability boundary between .NET and C++. In such a setting, Bonsai rewrites are used to erase assembly references such as `mscorlib` and type references such as `System.Int32` in favor of an agreed-upon set of "virtual" assemblies and types. Similarly, any remaining references to members on types are erased by tree rewriting steps that refer to unbound functions (e.g. a `Substring` method call can be turned into an invocation of an unbound function called `std://string/substring`).

The convention of using arrays and discriminators as the first element carried forward in the expression's serialization format as well. In the example above, the `+` discriminator is used to denote an `Add` operation, while the `:` discriminator is used to denote a `Constant` node. Zooming in to such a constant:

```json
[":", 42, 0]
```

there are two elements following the discriminator. The first one contains the value, here serialized as a JSON number. The second one is optional and, if specified, contains a reference to an entry in the type table (here referencing `System.Int32`). Various leaf nodes such as `Default`, `Constant`, `Parameter`, etc. support such optional type annotations. In the example above, `$` represents a `Parameter` node which also has an optional type reference (besides a name).

The full list of discriminators can be found in a file called `Discriminators.cs`. This includes discriminators for expression nodes, type kinds, and member kinds. A couple of examples:

* `[]` is used as a discriminator for array types, but also for index expressions.
  - `["[]", 0]` is a single-dimensional array with element type `0`, which is an index into the type table.
  - `["[]", obj, [arg0, arg1]]` is an index expression with `obj` representing the object being indexed (e.g. an array), and the `arg*` operands representing the arguments of the indexer.
* `<>` is used as the discriminator for closed generic types.
  - `["<>", 7, [0, 1]]` closes an open generic type definition in slot `7` with generic arguments in slots `0` and `1`.
* Etc.

> **Note:** While the Bonsai specification allows for the omission of various type references (to have "optional static typing"), the implementation shared in the Nuqleon OSS codebase does not support this in all places right now. A separate Bonsai deserialization implementation with support for dynamic typing in .NET has been built, but relied on an alternative implementation of the Dynamic Language Runtime (with different types of binders and tighter control over call site caches, enabling higher density in services running millions of compiled expression trees) which is currently not OSS. However, an implementation using the standard DLR is possible, and performance can likely be made to match fairly closely by playing more expression rewrite tricks where call sites are hoisted up and reused across various expressions. As an alternative, more aggressive type inference could be used in a pre-processing step (prior to deerialization) in an attempt to reconstruct typing earlier rather than relying on dynamic binding at runtime. Finally note that type inference can also take place prior to serialization, e.g. if a JavaScript client loads the TypeScript compiler as a library to try to infer types (or require type inference to be complete by running in a strict mode, disallowing `any`).

## Serialization and deserialization

Serialization and deserialization of `ExpressionSlim` objects is carried out via an `ExpressionSlimBonsaiSerializer` instance. The two core methods are:

```csharp
public Json.Expression Serialize(ExpressionSlim expression);
public ExpressionSlim Deserialize(Json.Expression expression);
```

Note the use of `Nuqleon.Json.Expression` as the object used to represent the JSON document. Having an object representation for these allows for pre- and post-processing steps using visitors for JSON nodes as well. The final conversion from and to strings is carried out in `Nuqleon.Json` or using interoperability helpers with other JSON frameworks.

In order to construct an `ExpressionSlimBonsaiSerializer`, two more pieces are needed:

* a *lift* factory for `Object` to `ObjectSlim` conversion, and,
* a *reduce* factory for `ObjectSlim` to `Object` conversion.

These are passed to the constructor and provide a place to plug in serializers for values. For example, the `Nuqleon.DataModel` serializer is plugged in here at the Reactive layer of the stack. This supports decoupling of expression tree and value serialization.

> **Note:** Some deployments of Nuqleon use JSON for DataModel serialization, while others use binary (and store it in a Bonsai tree as a base64 encoded string). Yet other deployments of Nuqleon have used totally different data models, for example based on Entity Framework's EDM.

The constructor is shown below:

```csharp
public ExpressionSlimBonsaiSerializer(
    Func<Type, Func<object, object>> liftFactory, 
    Func<Type, Func<object, object>> reduceFactory, 
    Version version)
```

Use the `Versioning.Default` value for the version of Bonsai to use (currently `0.9`). The lift and and reduce factories are higher-order functions that deal with the `ConstantExpression.Value` or `ConstantExpressionSlim.Value` to convert to/from the .NET object. If only primitve types are used, one can create an instance of `ObjectSerializer` and use its lift/reduce support:

```csharp
var obj = new ObjectSerializer();
var ser = new ExpressionSlimBonsaiSerializer(obj.GetJsonSerializer, obj.GetJsonDeserializer, Versioning.Default);
```

More advanced scenarios require parameterization with a custom lift/reduce factory to handle more complex values. Examples of this can be found in higher layers of Nuqleon where the Bonsai serializer and Data Model serializer are integrated to support any direction of nesting (e.g. an entity with a property that's an expression tree, which in turn contains a constant node holding an entity value).