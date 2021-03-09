# Nuqleon.Json.Interop.Newtonsoft

Provides interoperability with `Newtonsoft.Json` using `JsonReader` and `JsonWriter` implementations.

> **Note:** This assembly was introduced to provide interoperability between the lightweight `Nuqleon.Json.Expressions` object model and the `Newtonsoft.Json` object model, without having to go through intermediate string representations.

## JsonExpressionReader

The `JsonExpressionReader` class inherits from Newtonsoft's `JsonReader` base class and supports reading from a `Nuqleon.Json.Expressions.Expression` object, passed to its constructor. The reader instance can then be passed to Newtonsoft's `JsonSerializer.Deserialize` method. For example:

```csharp
var expr = Nuqleon.Json.Expressions.Expression.Parse("{ \"bar\": 42 }");
var reader = new JsonExpressionReader(expr);
var serializer = new Newtonsoft.Json.JsonSerializer();
var res = serializer.Deserialize(reader);
```

## JsonExpressionWriter

The `JsonExpressionWriter` class inherits from Newtonsoft's `JsonWriter` base class and supports constructing a `Nuqleon.Json.Expressions.Expression` object, exposed via  the `Expression` property. The writer instance can be passed to Newtonsoft's `JsonSerializer.Serialize` method. For example:

```csharp
var obj = new { bar = 42 };
var writer = new JsonExpressionWriter();
var serializer = new Newtonsoft.Json.JsonSerializer();
serializer.Serialize(writer, obj);
var expr = writer.Expression;
```
