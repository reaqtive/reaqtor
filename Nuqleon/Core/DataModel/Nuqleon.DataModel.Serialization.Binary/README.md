# `Nuqleon.DataModel.Serialization.Binary`

Binary serialization for Nuqleon data model entity values.

## `DataTypeBinarySerializer`

To serialize objects that are data model compliant (see `DataType.Check`), one can use the `DataTypeBinarySerializer` in this library. It provides an alternative to JSON serialization which can be more efficient both in terms of space and performance. Under the hood, this serializer generates efficient specialized serialization and deserialization code for data model types at runtime, using expression tree compilation.

```csharp
public abstract class DataTypeBinarySerializer
{
    public DataTypeBinarySerializer(IExpressionSerializer expressionSerializer);

    public void Serialize<T>(T value, Stream serialized);
    public T Deserialize<T>(Stream serialized);

    public void Serialize(Type type, Stream stream, object value);
    public object Deserialize(Type type, Stream stream);
}
```

To create an instance of the serializer, an `IExpressionSerializer` has to be provided. Data and expression serializers are typically defined in a mutually recursive way, because data values may contain properties whose type is an expression tree, and expression trees may contain constant nodes that contain a data value. Unlike the JSON-based serializer, this `IExpressionSerializer` interface is defined in the current library and is specialized for the binary format:

```csharp
public interface IExpressionSerializer
{
    void Serialize(Stream stream, Expression expression);
    Expression Deserialize(Stream stream);
}
```

Where the JSON serializer requires an expression tree serializer to return a JSON fragment as a `string`, the binary serializer lets the expression (de)serialization step take place using the source or target `Stream`.

Once a serializer is created, serialization and deserialization operations are straightforward through the `Serialize<T>` and `Deserialize<T>` methods:

```csharp
var ser = new DataTypeBinarySerializer(/* expr serializer */);

var ms = new MemoryStream();

ser.Serialize(new Person { Name = "Bart", Age = 21 }, ms);

ms.Position = 0;

var persoon = ser.Deserialize<Persoon>(ms);
```

where `Person` and `Persoon` (in Dutch) are structurally compatible types (with the same values for `Mapping` attributes on properties). Alternative weakly-typed `Serialize` and `Deserialize` methods are provided as well. Under the hood, the generic variants delegate to these methods using `typeof(T)`. The weakly-typed overloads are especially useful when a data type is only known at runtime (in the form of a `System.Type`) and it'd be unweildy to force users to resort to reflection or `dynamic` to call these methods.
