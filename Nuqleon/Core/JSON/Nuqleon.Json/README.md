# Nuqleon.Json

Provides a simple object model for JSON, a parser, a printer, and visitors.

> **Note:** The history of this assembly goes all the way back 2009, before `Newtonsoft.Json` became the de facto standard for JSON serialization on .NET. It also predates the `System.Text.Json` library in the BCL.

## Nuqleon.Json.Expressions.Expression

The heart of the JSON library is an object model around JSON expressions, with `Expression` being the base class. The type also provides factories for the following:

* `ConstantExpression Null()` - A JSON constant expression representing `null`.
* `ConstantExpression Boolean(bool)` - A JSON constant expression representing `true` or `false`.
* `ConstantExpression String(string)` - A JSON constant expression representing a string literal, e.g. `"foo"`.
* `ConstantExpression Number(string)` - A JSON constant expression representing a numeric value, e.g. `3.14`.
* `ArrayExpression Array(IEnumerable<Expression>)` - A JSON array expression with the given child nodes (supporting various overloads).
* `ObjectExpression Object(IDictionary<string, Expression>)` - A JSON object expression with the given members.

The `Expression` class also provides support to parse JSON using a `Parse(string)` method, and a way to print JSON using a `ToString()` and `ToString(StringBuilder)` method.

## Nuqleon.Json.Expressions.ExpressionVisitor

The `ExpressionVisitor` base class can be used to visit the nodes of a JSON expression. An `ExpressionVisitor<T>` variant exists that can be used to convert a JSON expression to another object model.

> **Note:** Visitors are used by other parts of Nuqleon to traverse a JSON expression.

## Nuqleon.Json.Serialization.JsonSerializer

This namespace provides rudimentary support to JSON serialization and deserialization for .NET objects, including support for:

* Primitive types such as `int`, `string`, `bool`, etc.
* Array types with a single dimension, as well as `IList` implementations.
* Objects either by implementing `IDictionary<string, object>` or by reflection over properties or fields.

> **Note:** This implementation is legacy (going back to 2009) but has been retained for compatibility in certain parts of the Nuqleon stack and services built on top. More flexible and performant options for serialization exist, either in the `Nuqleon.Json.Serialization` library, `Newtonsoft.Json`, or `System.Text.Json`. New code should consider to use any of these.