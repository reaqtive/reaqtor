# `Nuqleon.Linq.Expressions.Bonsai`

Provides a lightweight object model for expression trees with a lightweight representation of reflection.

## Design goals

The goal of Bonsai trees is twofold:

1. Provide a more flexible approach to expression trees in terms of typing restrictions.
2. Provide an intermediate representation for expression tree serialization.

As an example of the first goal, consider an expression tree which is strongly typed using `System.Type` for every `Expression` node. The expression factories perform various checks using reflection to assert validity of the expression being constructed. For example, they reject constructing a `MethodCallExpression` for the `string.ToUpper()` method when applied to a node of type `System.Int32`. While this is very helpful when the goal is eventual code generation and compilation, it ties the expression tree APIs to the typing requirements of the CLR. This prevents us from introducing different notions of typing, for example structural types or record types (predating C# 9.0's record types).

> **Note:** Even though types in `System.Reflection` are not sealed, deriving from these types has limited value. For example, constructing a type derived from `System.Type` to denote some different kind of type (e.g. a structural type) will make various methods in reflection fail on not recognizing the type. Also, many checks are not symmetric; e.g. `t1.IsAssignableFrom(t2)` consults `t1` for the assignability, but not `t2`. If our custom type happens to be `t2`, it has no say in the outcome of this type check. Due to these type of limitations, we decided to have our own lightweight (but extensible) reflection object model, which also acts as an object model towards expression tree serialization.

For the second design goal, the resulting `ExpressionSlim`, `TypeSlim`, etc. objects support serialization with formats such as JSON or binary. This enables transport and persistence of expression trees, including interoperability with other languages. For example, Bonsai trees have been deserialized in native code and interpreted in C++, or been converted to strings that are `eval`-able in JavaScript. Other languages with first-class quotation support can also produce Bonsai expression trees, for example a JavaScript library parsing a piece of JavaScript into an AST and converting it to Bonsai.

> **Note:** The name Bonsai reflects on the compact nature of these trees. In fact, the specification allows for omission of reflection information, resulting in weakly typed or dynamically typed trees. These are useful in environments such as JavaScript. Typed execution environments can perform type inference after binding leaf nodes in such trees.

## `ExpressionSlim`, `ObjectSlim`, `TypeSlim`, etc.

The key types in this library are suffixes with `Slim` and are completely analogous to their corresponding non-`Slim` variants. In particular:

* `ObjectSlim` is the slim sibling of `System.Object`. It has an underlying value and a `TypeSlim` rather than a `System.Type`.
* `TypeSlim` is the slim sibling of `System.Type`. It supports different kinds such as primitive types, array types, generic types, function types, and custom types (for structural typing).
* `*InfoSlim` are the slim siblings of `System.Reflection.*Slim`, including fields, methods, properties, and constructors.
* `ExpressionSlim` and derived types are the slim siblings of types in `System.Linq.Expressions`.

One of the key differences between these types' counterparts in the .NET base class libraries is the minimal support for behaviors. Slim representations are much closer to data types than classes with various methods on them. For example, a `TypeSlim` does not support enumeration of members defined on the type. Instead, it merely represents the "shape" of a type through various type kinds exposed via a `Kind` property. In fact, `TypeSlim` is designed as a discriminated union type to represent different kinds of types.

Similarly, `ExpressionSlim` has a smaller API surface than `Expression`. In particular, it lacks a `Type` property on the base class. Instead, it relies on leaf nodes (such as `DefaultExpressionSlim`, `ConstantExpressionSlim`, etc.) or some other nodes (such as `Convert` nodes) to carry a `TypeSlim` when necessary. When typing for an entire slim expression tree is required, type inference can be carried out (see `TypeSlimDerivationVisitor`).

## Factories

Bonsai trees can be directly constructed using static methods on `ExpressionSlim`. These are completely analogous to their counterparts in `System.Linq.Expressions`. For example:

```csharp
ExpressionSlim.Add(a, b)
```

where `a` and `b` are expressions themselves. For various nodes, types other than `ExpressionSlim` have to be used, requiring instantation of various `System.Reflection.*Slim` types, `TypeSlim`, or `ObjectSlim`. For example, when a `TypeSlim` is needed, factory methods can be used to construct such objects:

```csharp
ExpressionSlim.Convert(e, TypeSlim.Array(TypeSlim.Simple(asm, "Foo")))
```

In here, we're creating a single-dimensional array of type `Foo`, defined in some assembly represented as an `AssemblySlim` object. It's turtles all the way down.

> **Note:** An `AssemblySlim` is really a string representation of some "container" for types. While it borrows its nomenclature from .NET, this string has often been used to refer to virtual libraries. Upon serialization and deserialization, the name of a "slim assembly" can be bound to a concrete thing. In the case of .NET, this may be an assembly on disk, while for other languages it may correspond to importing a module. Note that Bonsai enables reduction of the amount of reflection information carried, so it's also possible to omit an assembly altogether (assuming the receiver of a Bonsai tree can perform some type of binding at runtime).

## Conversions with `System.Linq.Expressions`

While factories can be used to construct all constituents of a tree, this is often quite clunky in .NET. Moreover, one is often dealing with a traditional `System.Linq.Expressions.Expression` object and wants to leverage the capabilities of Bonsai trees, either to perform tree rewrites with a different approach to typing (or looser rules around type checking), or to prepare for serialization of an expression tree.

> **Note:** An example of extensible typing is Bonsai's support for structural types and record types. C# language constructs such as anonymous types can be converted to such notions, detaching them from some compiler-generated type that may not exist across a serialization boundary. By representing the structure of a type (e.g. fields or properties and their types) rather than a (made-up by the compiler) type name, the structure of the type can be serialized. Note that support for these advanced typing notions predates C# 9.0's introduction of record types (by as much as 8 years).

To support conversion to and from Bonsai trees, this library provides `ToExpression` and `ToExpressionSlim` conversions to go back and forth between the two representations:

```csharp
// Expression to ExpressionSlim
public static ExpressionSlim ToExpressionSlim(this Expression expression);
public static ExpressionSlim ToExpressionSlim(this Expression expression, IExpressionSlimFactory factory);

// ExpressionSlim to Expression
public static Expression ToExpression(this ExpressionSlim expression);
public static Expression ToExpression(this ExpressionSlim expression, IExpressionFactory factory);
public static Expression ToExpression(this ExpressionSlim expression, IExpressionFactory factory, IReflectionProvider provider);
```

Conversion from an `Expression` to an `ExpressionSlim` involves also mapping `object` to `ObjectSlim`, `Type` to `TypeSlim`, etc. Management of all reflection conversions is carried out via a so-called `TypeSpace` which performs a technique called hash consing to map equal instances in the source domain (`System.Reflection`) to equal instances in the target domain (the `Slim` counterparts). Furthermore, the conversion can be parameterized on an expression factory which provides a layer of indirection between the conversion and the `ExpressionSlim` factories.

Conversion from an `ExpressionSlim` to an `Expression` is completely symmetric and uses an `InvertedTypeSpace` to convert slim reflection objects back to their `System.Reflection` counterparts. This part can be controlled via an `IReflectionProvider` which can be used to cache expensive reflection operations or to perform binding redirect decisions (e.g. an `AssemblySlim`'s name can be rewritten prior to applying an `Assembly.Load` to get an `Assembly` object back) but also to rewrite type names. Finally, an `IExpressionFactory` is used to construct expressions. For this, an `UnsafeExpressionFactory` can be used to bypass some of the checks in `System.Linq.Expressions` (after evaluating the safety implications - invalid trees may result in invalid IL code generation that destabilizes the CLR runtime - versus the performance gains).

A typical diagram of conversions looks as follows:

```
Expression -> ExpressionSlim -> Bonsai JSON
Bonsai JSON -> ExpressionSlim -> Expression
```

where additional rewrites can take place in any of the "domains" by leveraging expression visitors.

## Visitors

Types in this library are inherently recursive and support visitors:

* `ExpressionSlim` supports an `ExpressionSlimVisitor`, in a way completely analogous to `ExpressionVisitor`.
* `TypeSlim` supports a `TypeSlimVisitor` which visits the structure of the type (e.g. `List<int[]>` will visit `List<T>`, `int[]`, `int`).

Generic variants are provided as well, supporting conversions to other types. A concrete example of such uses are the visitors implementing `ToCSharp()` on both `ExpressionSlim` and `TypeSlim`. These methods are used for debugging and diagnostic purposes only and provide a best-effort C#-like string representation of objects such as types and Bonsai expression trees. The implementation of these is done via visitors that convert to `string`.

> **Note:** The `ToCSharpString` methods aren't particularly fast or memory-efficient because they rely on string concatenation (rather than using a `StringBuilder`). These are only provided for diagnostic purposes and provide a sample use of the generic visitors.

Similarly, the implementation of conversions between `Slim` and non-`Slim` variants of types is often carried out using generic visitors as well. For example, the conversion from `TypeSlim` to `Type` is implemented by means of a `TypeSlimVisitor<Type>`.
