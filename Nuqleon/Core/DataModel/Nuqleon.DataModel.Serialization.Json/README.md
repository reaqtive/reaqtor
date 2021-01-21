# `Nuqleon.DataModel.Serialization.Json`

JSON serialization for Nuqleon data model entity values.

## `DataSerializer`

To serialize objects that are data model compliant (see `DataType.Check`), one can use the `DataSerializer` in this library. For historical reasons including earlier support for a BSON format in addition to JSON support, the `DataSerializer` is an abstract base class with various factory methods on it. Alternatively, the `JsonDataSerializer` class can be used directly.

```csharp
public abstract class DataSerializer
{
    public static DataSerializer Create() => new JsonDataSerializer();
    public static DataSerializer Create(IExpressionSerializer expressionSerializer);
    public static DataSerializer Create(IExpressionSerializer expressionSerializer, params DataConverter[] converters);

    public void Serialize<T>(T value, Stream serialized);
    public T Deserialize<T>(Stream serialized);
}
```

If values may include expression trees, an `IExpressionSerializer` has to be provided as well. Data and expression serializers are typically defined in a mutually recursive way, because data values may contain properties whose type is an expression tree, and expression trees may contain constant nodes that contain a data value. The `IExpressionSerializer` interface is defined in `Nuqleon.Linq.Expressions.Bonsai` and looks like this:

```csharp
public interface IExpressionSerializer
{
    ExpressionSlim Lift(Expression expression);
    Expression Reduce(ExpressionSlim expression);

    string Serialize(ExpressionSlim expression);
    ExpressionSlim Deserialize(string expression);
}
```

The `Lift` and `Reduce` methods support conversion between `Expression` and `ExpressionSlim` because Bonsai serialization is only supported on `ExpressionSlim` instances. Because the interface is defined in the Bonsai assembly and a textual format is used, the `Serialize` and `Deserialize` methods operate on `string` values.

Additionally, during creation of serializers, custom `DataConverter`s can be registered as well. Under the hood, the serializer uses Newtonsoft JSON. Once a serializer is created, serialization and deserialization operations are straightforward through the `Serialize<T>` and `Deserialize<T>` methods:

```csharp
var ser = DataSerializer.Create();

var ms = new MemoryStream();

ser.Serialize(new Person { Name = "Bart", Age = 21 }, ms);

ms.Position = 0;

var persoon = ser.Deserialize<Persoon>(ms);
```

where `Person` and `Persoon` (in Dutch) are structurally compatible types (with the same values for `Mapping` attributes on properties).
