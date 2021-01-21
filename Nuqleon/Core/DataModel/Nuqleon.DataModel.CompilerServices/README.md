# `Nuqleon.DataModel.CompilerServices`

A set of utilities used to work with Nuqleon data model entity types.

## Type system

The `DataType` base class represents a Data Model compliant data type. The structure of this class hierarchy reflects the supported types, with the different kinds specified in the `DataTypeKinds` enum. These include:

* `Primitive`, e.g. to support `int`.
* `Array` for single-dimensional arrays.
* `Structural` for structural types, where properties are modeled as `DataProperty`.
* `Function` for function types, e.g. delegate types in the CLR type system.
* `Expression` for expression trees that represent code as data.
* `Quotation` for quotations of functions as expression trees.
* `OpenGenericParameter` for support of wildcards (e.g. to support binding).

The base class is shown below:

```csharp
public abstract partial class DataType
{
    public abstract DataTypeKinds Kind { get; }

    public Type UnderlyingType { get; }

    public virtual DataType Reduce() => this;

    public abstract object CreateInstance(params object[] arguments);
}
```

Derived classes exist for all of the different kinds, as listed above. These have specified properties, e.g. an `ElementType` on the `ArrayDataType`.

A couple of things are worth noting on the `DataType` base class:

* Data model types have underlying CLR types exposed through `UnderlyingType`.
* Instances of data model types can be created using a `CreateInstance` method. This relies on the underlying CLR type; the arguments depends on the kind.
* Custom data model types are supported (e.g. supporting other types of objects from foreign libraries), which can be `Reduce`d to a standard data model type.

In many cases, data model types are purely used to support expression tree rewrites, and thus provide an intermediate representation to reason over type structures. To enable this functionality, conversions from CLR `Type`s to data model `DataType`s are provided through a set of static methods, shown below:

```csharp
public abstract partial class DataType
{
    public static DataType FromType(Type type);
    public static DataType FromType(Type type, bool allowCycles);

    public static bool TryFromType(Type type, out DataType result);
    public static bool TryFromType(Type type, bool allowCycles, out DataType result);

    public static void Check(Type type);
    public static void Check(Type type, bool allowCycles);

    public static bool TryCheck(Type type, out ReadOnlyCollection<DataTypeError> errors);
    public static bool TryCheck(Type type, bool allowCycles, out ReadOnlyCollection<DataTypeError> errors);

    public static bool IsStructuralEntityDataType(Type type);
    public static bool IsEntityEnumDataType(Type type);
}
```

All of the `From*` and `Check` variants take in a CLR `Type` and perform a conversion to the corresponding `DataType`. As usual, `Try` variants return a `bool` value to indicate success, while other variants throw exceptions. Cycles can occur in nominal types, and can optionally be permitted in data model types as well. However, when doing so, serialization as JSON is not possible because there's no notion of object identity that enables references between instances.

Two additional `Is` checks can be used to check whether a CLR type is a structural type (i.e. has `MappingAttribute`s applied to its members) or a data model compliant enum, i.e. one where all of the enumeration values have a `MappingAttribute`.

The rules for conversions of CLR types to `DataType` are summarized below:

* `PrimitiveDataType`:
  * `void` becomes a `PrimitiveDataType` with `Unit` as the underlying type;
  * primitives (integers, bool, char, string, floats, decimal, `DateTime[Offset]`, `TimeSpan`, `Uri`, and `Guid`) map to `PrimitiveDataType`;
  * enums with a supported primitive underlying types and valid `MappingAttribute` annotations also map to `PrimitiveDataType`;
  * nullable primitives and enums, as described above, also map to `PrimitiveDataType`;
* `ArrayDataType` for single-dimensional array-like types with index-based access and elements of a data model type:
  * `T[]` arrays, `IList<T>`, `IReadOnlyList<T>`, and `List<T>` become `ArrayDataType`s;
* `FunctionDataType` supports functions with parameter and return types that are data model types:
  * delegate types become `FunctionDataType`s;
* `ExpressionDataType` supports code-as-data:
  * types derived from `Expression` (except for `Expression<T>`) become `ExpressionDataType`s;
* `QuotationDataType` supports quotations of functions:
  * `Expression<T>` becomes a `QuotationDataType`;
* `StructuralDataType`:
  * structural types (anonymous types, record types, and data model entities with `MappingAttribute`s) become `StrucuturalDataType`s;
  * tuples using `System.Tuple<...>` types (but not `System.ValueTuple<...>` for historical reasons) become `StructuralDataType`s as well, with properties that have `ItemN` names.

> **Note:** Record types in this library refer to the notion of record types introduced by `Nuqleon.Linq.CompilerServices` which long predates the addition of record types in C# 9.0. They are, however, semantically close to C#'s record types, in terms of value equality semantics and their support for construction through property assignments (albeit not constrained to "initialization contexts" as introduced in C# 9.0).

## Type visitors

Once a `DataType` has been obtained, it can be useful to recurse over its structure. This is supported through the use of `DataTypeVisitor` and `DataTypeVisitor<TType, TProperty>`. Both visitor types provide `protected virtual Visit*` methods for the various subclasses that derive from `DataType`, and a `VisitProperty` method to handle `DataProperty`s referenced from `StructuralDataType`s.

The non-generic visitor variant simply traverses over the structure of the given `DataType` and produces a new `DataType` if any of the recursive visits returns a different instance. This can be used to rewrite data types.

```csharp
public class DataTypeVisitor
{
    public virtual DataType Visit(DataType type);

    protected virtual DataType VisitArray(ArrayDataType type);
    protected virtual DataType VisitExpression(ExpressionDataType type);
    protected virtual DataType VisitFunction(FunctionDataType type);
    protected virtual DataType VisitOpenGenericParameter(OpenGenericParameterDataType type);
    protected virtual DataType VisitPrimitive(PrimitiveDataType type);
    protected virtual DataType VisitQuotation(QuotationDataType type);
    protected virtual DataType VisitStructural(StructuralDataType type);
    protected virtual DataType VisitCustom(DataType type);

    protected virtual DataProperty VisitProperty(DataProperty property);

    protected T VisitAndConvert<T>(DataType type);

    protected virtual Type ChangeUnderlyingType(Type type);
    protected virtual MemberInfo ChangeProperty(PropertyInfo property);
    protected virtual MemberInfo ChangeField(FieldInfo field);

    public static ReadOnlyCollection<T> Visit<T>(ReadOnlyCollection<T> nodes, Func<T, T> elementVisitor);
}
```

When a `Visit` method returns a different `DataType` than the one that was passed in, or when `VisitProperty` returns a different `DataProperty`, calls to `Change*` methods are made to rewrite the `UnderlyingType` or `Property` value. If the visitor is used to rewrite data types or its properties, these `Change*` methods should be overridden. The idea of this type of rewrite is to treat a `DataType` as a constrained CLR type and to ensure that every rewrite of such a type still has a valid underlying CLR type representation. An example of such a rewrite is to substitute entity types for structurally identical anonymous or record types, e.g. whereby a `Person` object gets replaced by some `<>__AnonymousType1` runtime-generated type, and the `Person.Name` property gets replaced by the `<>__AnonymousType1.name` property (where the new property name was derived from a `MappingAttribute` on the original property).

The generic visitor requires two generic arguments to specify the rewrite target for `DataType` (represented as `TType`) and for `DataProperty` (represented as `TProperty`). This visitor is an abstract class and requires implementation of various `Visit` and `Make` methods:

```csharp
public abstract class DataTypeVisitor<TType, TProperty>
{
    public virtual TType Visit(DataType type);

    protected virtual TType VisitArray(ArrayDataType type);
    protected abstract TType MakeArray(ArrayDataType type, TType elementType);
    
    protected abstract TType VisitExpression(ExpressionDataType type);

    protected virtual TType VisitFunction(FunctionDataType type);
    protected abstract TType MakeFunction(FunctionDataType type, ReadOnlyCollection<TType> parameterTypes, TType returnType);

    protected abstract TType VisitOpenGenericParameter(OpenGenericParameterDataType type);

    protected abstract TType VisitPrimitive(PrimitiveDataType type);

    protected virtual TType VisitQuotation(QuotationDataType type);
    protected abstract TType MakeQuotation(QuotationDataType type, TType functionType);

    protected virtual TType VisitStructural(StructuralDataType type);
    protected abstract TType MakeStructural(StructuralDataType type, ReadOnlyCollection<TProperty> properties);

    protected abstract TType VisitCustom(DataType type);

    protected virtual TProperty VisitProperty(DataProperty property);
    protected abstract TProperty MakeProperty(DataProperty property, TType propertyType);
}
```

The `Visit` methods recurse over the structure of non-leaf types (e.g. visiting the element type of an array), and invoke corresponding `Make` methods to construct the resulting `TType` given the rewritten child nodes. The same mechanism is used to rewrite properties. An example usage of this visitor is to convert `DataType` to a corresponding `TypeSlim` (for use by Bonsai expression trees), or to create a string representation of a type for debugging purposes.

## Comparers

The `DataTypeEqualityComparer<T>` type implements `IEqualityComparer<T>` and can be used for any type `T` that is a valid data model type (i.e. passes `DataType.Check(Type)` tests). The equality comparer checks for value-based equality by recursing over the structure of the data type while performing pairwise checks of the values passed in. For example, given a class `Person`:

```csharp
class Person
{
    [Mapping("name")]
    public string Name { get; set; }

    [Mapping("age")]
    public int Age { get; set; }
}
```

the following two values will compare equal:

```csharp
var people1 = new Person[] { new Person { Name = "Bart", Age = 21 } };
var people2 = new Person[] { new Person { Name = "Bart", Age = 21 } };
```

That is, the elements in arrays are compared in a pairwise fashion, and properties on objects are compared pairwise as well. Reference equality does not play a role in determining equality. Another way to think about this is whether the corresponding JSON serialized representation of both objects would be identical.

> **Note:** This type of comparer is especially useful when implementing query operators that rely on value-based equality of data model compliant types. For example, an operator such as `DistinctUntilChanged` relies on correct value-based equality semantics. Note though that runtime-generated record types do have equality semantics built in through an `IEquatabl<T>` implementation, so the use of a specialized `IEqualityComparer<T>` is not always needed for such types. (Subtlety can arise when dealing with arrays or lists where equality is based on reference equality of the collections rather than a comparison of the elements.)

## `KnownTypeBase<T>`

The `KnownTypeBase<T>` base class implements `IEquatable<T>` in terms of `DataTypeEqualityComparer<T>` and can be used as the base class for `[KnownType]`-annotated data types in order to ensure that values have value-based equality. For example, when implementing a `WeatherObservable` as an `ISubscribable<Weather>`, it'd make sense to define `Weather` as follows:

```csharp
[KnownType]
class Weather : KnownTypeBase<Weather>
{
    [Mapping("temp")]
    public double Temperature { get; set; }

    // Etc.
}
```

This ensures that the default equality comparer obtained through `EqualityComparer<T>.Default` will pick up on the `IEquatable<T>` implementation provided through `KnownTypeBase<T>`. This in turn allows operators that rely on value-based equality (such as `DistinctUntilChanged`) to behave correctly without having to specify custom comparers.

> **Note:** Specifying custom equality comparers throughout the Reaqtor stack is also non-trivial because the `IEqualityComparer<T>` interface does currently not have a quoted variant in the Reaqtor stack. As such, a user can't refer to a well-known predefined comparer (using a URI to refer to a `KnownResource`), or use some `Create` factory method to construct an anonymous comparer based on lambda-based implementations for `Equals` and `GetHashCode`. Right now, only "reactive artifact types" are supported throughout the stack, but this set of types could be augmented with things such as `IEqualityComparer<T>`, `IComparer<T>`, `IScheduler`, etc., which may occur as parameters on query operators. This moves the Reaqtor stack closer to a general-purpose "computation space" rather than just a "reactive processing space". In fact, this type of work has been carried out in an internal spin-off project called "ICP" (for `IComputationProcessing`) as a step up from "IRP" (for `IReactiveProcessing`), where a computation space is seeded with a set of interfaces whose transitive closure represents the first-class concepts that can cross client and service boundaries through expression shipping (an example of an "ICP" space being a workflow processing system centered around an `IControlFlowNode` construct). In that worldview, Reaqtor is merely an instantiation of "ICP" for the `IReactive*` interfaces.

## Type substitution

When serializing an expression tree that uses .NET types to represent data types that have a structural nature (i.e. using `Mapping` attributes on properties), we want to shake off the nominal nature of such types, i.e. the type name and the assembly in which the type was declared. Instead, we want to capture the structure of the type, i.e. the properties and their types. One way to trade nominal types for structural types is by performing an expression tree type substitution step. The base class for such rewrites is the `ExpressionEntityTypeSubstitutor` abstract class, which relies on an `EntityTypeSubstitutor` utility.

The `EntityTypeSubstitutor` is an expression tree visitor that's parameterized on an `IDictionary<Type, Type>` which specifies the types to be substituted. It relies on two `abstract` members to be provided by derived types.

```csharp
public abstract class EntityTypeSubstitutor : TypeSubstitutionExpressionVisitor
{
    protected EntityTypeSubstitutor(IDictionary<Type, Type> typeMap);

    protected abstract Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments);
    protected abstract object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType);
}
```

The `CreateNewExpression` method is called whenever a `NewExpression` is encountered whose type is subject to substitution. This enables derived classes to adapt to the construction strategy for the target structural type. For example, anonymous types use constructors but record types use property assignment. The `ConvertConstantStructuralCore` method is called whenever a `ConstantExpression` is encountered whose type is subject to substitution. In this case the `Value` of type `System.Object` needs to be converted, which is subject to the specific semantics provided by the derived class.

In order to use this functionality, one typically uses a wrapper type called `ExpressionEntityTypeSubstitutor`, which is an expression tree visitor as well. This type is parameterized on an `EntityTypeSubstitutor`, as discussed above, and provides the base class facilities to perform rewrites of structural types to anonymous types or record types. In order to carry out a rewrite, the top-level `Apply` method is used:

```csharp
public abstract partial class ExpressionEntityTypeSubstitutor
{
    protected ExpressionEntityTypeSubstitutor(EntityTypeSubstitutor substitutor);

    public virtual Expression Apply(Expression expression);

    protected abstract TypeBuilder GetTypeBuilder(RuntimeCompiler rtc);
    protected abstract void DefineType(RuntimeCompiler rtc, TypeBuilder builder, IEnumerable<KeyValuePair<string, Type>> properties);
}
```

Implementations of these abstract methods pick either anonymous types or record types to generate at runtime. This is done by the `ExpressionEntityTypeAnonymizer` and `ExpressionEntityTypeRecordizer` derived classes. An example of performing such a rewrite is shown below:

```csharp
Expression<Func<Person, bool>> f = p => p.Age > 21;

var rw = new ExpressionEntityTypeAnonymizer();
var res = rw.Apply(f);
```

In the resulting expression, the `Person` type will have been substituted using an anonymous type that uses the `Mapping` attribute values for the property names. When passing this expression to a subsequent conversion step to `ExpressionSlim`, the nominal `Person` type will have entirely disappeared and the resulting anonymous type will be treated as a structural type. This structural nature will be preserved throughout expression tree serialization and deserialization steps using Bonsai. The final conversion from `ExpressionSlim` back to `Expression` can either reconstruct a structurally equivalent type at runtime, or be subject to rewrite steps that perform binding, which may involve substituting the structural type for a known nominal type that has the same structure (e.g. a type marked with the `KnownType` attribute).

> **Note:** Expression rewriting involving the runtime compilation of new types can be quite expensive. When the only goal is to take the resulting expression tree and serialize it (but not directly evaluate the expression tree), cheaper options are available in the form of expression rewrites in the `ExpressionSlim` space, as discussed below.

## Bonsai utilities

It's very common to substitute nominal data types for structurally equivalent data types prior to expression tree serialization. One way to achieve this is by performing a rewrite in the `Expression` space, which requires runtime compilation of structural types (e.g. record types). An alternative is to apply this rewrite in the `ExpressionSlim` space, which can be much cheaper because it doesn't involve runtime compilation and the use of reflection. To support this, we support types such as `ExpressionSlimEntityTypeRecordizer` that are functionally equivalent to the non-slim variants discussed above.
