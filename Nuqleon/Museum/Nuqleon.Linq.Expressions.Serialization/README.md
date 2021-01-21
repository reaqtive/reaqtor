# `Nuqleon.Linq.Expressions.Serialization`

A legacy rule-driven serialization library for expression trees.

> **Note:** This library goes back to the early days of expression tree shipping before we introduced Bonsai trees.

## Usage

A simple example of expression tree serialization using this library is shown below:

```csharp
Expression expr = Expression.Add(Expression.Constant(1), Expression.Parameter(typeof(int), "x"));

var ser = new ExpressionJsonSerializer();

Nuqleon.Json.Expressions.Expression json = ser.Serialize(expr);

Expression res = ser.Deserialize(json);
```
