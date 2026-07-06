# Nuqleon.Serialization

A legacy rule-driven serialization library.

## Usage

Serializers built using this library use a rule table to provide serialization and deserialization logic for specified types. It's useful to first understand rule tables before discussing the top-level serializer base class etc. Entries in rule tables can have different forms, for example:

```csharp
table.Add<Person>(p => new Person { Name = p.Name, Age = p.Age });
```

This first type of rule is the simplest and is called a roundtrip rule. It deconstructs and reconstructs an object of the given type. The serializer automatically infers that it needs to recursively serialize the `Name` and `Age` properties, and perform the inverse operation to reconstruct a `Person` object. (Note this is very similar to pattern matching in later versions of C#.) This could be written by hand as follows:

```csharp
table.Add<Person>("Person",
  (p, serialize,   ctx) => new Dictionary<string, object>
                           {
                             { "Name", serialize(p.Name, ctx) },
                             { "Age" , serialize(p.Age,  ctx) },
                           },
  (d, deserialize, ctx) => new Person
                           {
                             Name = (string)deserialize(d["Name"], ctx),
                             Age  =    (int)deserialize(d["Age" ], ctx),
                           }
);
```

Here we specify a name for the rule (used for diagnostics) and a serialization/deserialization function pair. A piece of context is threaded through the serialization and deserialization logic. Further variants of rules exist, including support for Boolean-valued predicates to run a filter on an object instance to decide on which rule to pick.

In the example shown above, we serialize objects to a `Dictionary<string, object>` representation. This is called the output type of the serializer. Commonly used type for the output include `XNode`, `JObject`, etc. In fact, expression tree serialization uses `Nuqleon.Json.Expressions.Expression` as the output type.

Rule tables are really built using collection initializers via the `Rules` property exposed on the `Serializer` class. This class takes a number of generic parameters:

```csharp
public class Serializer<TInput, TOutput, TContext> { ... }
```

The `TInput` is the input type, e.g. `System.Linq.Expressions.Expressions` for an expression serializer. The `TOutput` is the serialization target type, e.g. `Nuqleon.Json.Expressions.Expression`. Finally, the `TContext` is an arbitrary type that can be used to thread context through the (recursive) serialization and deserialization steps.

Next, the constructor of the serializer may look quite complex:

```csharp
public Serializer(Func<TaggedByRule<TOutput>, TOutput> tag,
                  Func<TOutput, TaggedByRule<TOutput>> untag,
                  Func<TContext> newContext,
                  Func<Contextual<TContext, TOutput>, TOutput> addContext,
                  Func<TOutput, Contextual<TContext, TOutput>> getContext,
                  Func<IDictionary<string, Expression>, Expression> wrap,
                  Func<Expression, string, Expression> unwrap)
```

but note that 6 of the 7 parameters are symmetric.

The `tag` and `untag` pararameters are functions that allow serialization and deserialization to make use of the tags specified on rules (e.g. `"Person"` in the example above). For example, if serialization picks some rule named `Foo` and produces some output value, the `tag` function can combine the output with the tag value to create a final output value. Note that this is the operating principle behind Bonsai serialization as well, where the tag is a discriminator. The inverse `untag` function can then be used to look for a tag in the serialized representation and strip it off prior to performing the core deserialization logic.

The `newContext` parameter provides a factory for `TContext` instances created during serialization and deserialization. It's often implemented as a simple object instantiation. For example, expression tree serialization creates a context object that keeps track of variables declared in the tree to build up scope tables. During serialization these tables are used to map variables onto unique identifiers, such that deserialization will restore the original variable bindings (e.g. an expression tree `x => x => x` may either mean `p0 => p1 => p0` or `p0 => p1 => p1` or `p0 => p1 => p2` depending on reference equality for all occurences of the parameter expression `x`).

The `addContext` and `getContext` functions are used during serialization and deserialization to append and strip the context from the payload, if needed. This enables the gathered context (or parts thereof) to become part of the serialized representation. For example, in expression tree serialization, the context keeps track of types, members, etc. used in the tree. In addition, it stores unbound "global" parameters in order to build a globals table that's part of the serialization output.

The `wrap` and `unwrap` functions are the most tricky ones and are used for "roundtrip rules", as shown earlier. When a rule table encounters a roundtrip rule (e.g. `(Point p) => new Point(p.X, p.Y)`), it extracts serialization and deserialization expressions based on the members that are accessed (in this case, `X` and `Y`). These look like:

* `(p, rec, ctx) => _wrap(new Dictionary<string, Expression> { { "X", rec(p.X, ctx) }, { "Y", rec(p.Y, ctx) } })`
* `(o, rec, ctx) => new Point(rec(_unwrap(o, "X"), ctx), rec(_unwrap(o, "Y"), ctx))`

Wrap and unwrap functions are used to combine the serialized members (here by accessing `p.X` and `p.Y`) into a `TOutput`, or to extract members during deserialization prior to combining them into a `TInput` (here by running `new Point(...)`).
