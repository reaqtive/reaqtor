# Nuqleon.Linq.Expressions.Bonsai.Hashing

Provides support to hash Bonsai expressions.

## ExpressionSlimHasher

To obtain a hash code for a Bonsai expression of type `ExpressionSlim`, the `ExpressionSlimHasher` class can be used. Hashes can be used to quickly check if two given expressions may be equal, or to get some signature for a given expression. Advanced scenarios can inherit from `ExpressionSlimHasher` to control some behaviors, as discussed later.

```csharp
var esh = new ExpressionSlimHasher();
int h = esh.GetHashCode(expr);
```

Hashing of Bonsai expressions incorporates semantic information about variable bindings. As such, expressions that are equivalent modulo variable names will have the same hash value. For example:

```csharp
x => x + 1
```

and

```csharp
y => y + 1
```

will hash to the same value, provided the type of `x` and `y` is the same.

## Advanced behavior

`ExpressionSlimHasher` provides a couple of extensibility points:

* Control how global unbound parameters are hashed; by default, the hash code for such parameters incorporates the type as well as the parameter's name.
* Control how constants are hashed; by default, it uses `object.GetHashCode()` on the `Value` of `ConstantExpressionSlim` nodes, as well as the type of the node.

These are especially relevant for scenarios where one wants to check for expression tree equality in the presence of unbound parameters (e.g. referring to some resource that will be bound in a service) and constants which may or may not be considered for equality. By ignoring constants as part of the hashing, one can get equality modulo constants, for example to match a given expression against known execution plans. For example:

```csharp
xs.Where(x => x > 1)
```

and

```csharp
xs.Where(y => y > 2)
```

can be made to hash to the same value, by ensuring that global parameters (in this case `xs`) are hashed based on the `Name` property, and by ignoring constants. This way, both incoming expressions can be matched against an expression "template" that maps to an execution plan.