# `CSE`

Illustrates the concept of common subexpression elimination applied to reactive expressions.

## History

Common subexpression elimination is a well-known technique in the compiler literature and is used as an optimization by eliminating redundant computation. A typical example is the rewrite of:

```csharp
a = b + c * d - e;
z = f + c * d - g;
```

into

```csharp
var tmp = c * d;
a = b + tmp - e;
z = f + tmp - g;
```

where `c * d` is only evaluated once. Obviously such rewrites are only valid if they don't take away or reorder side-effects.

In the context of reactive event processing, query expressions often contain common patterns that lend themselves to applying a similar rewrite. In particular, consider the following two queries:

```csharp
stock.Where(s => s.Symbol == "TSLA" && s.Quote > 420).Subscribe(high)
stock.Where(s => s.Symbol == "TSLA" && s.Quote < 420).Subscribe(low)
```

Both queries share the `s.Symbol == "TSLA"` predicate, which can be revealed after performing some query normalization:

```csharp
stock.Where(s => s.Symbol == "TSLA").Where(s => s.Quote > 420).Subscribe(high)
stock.Where(s => s.Symbol == "TSLA").Where(s => s.Quote < 420).Subscribe(low)
```

This results in a common prefix which is:

```csharp
stock.Where(s => s.Symbol == "TSLA")
```

In classic Rx, one could perform a manual transformation to rewrite both queries using `Publish` and `RefCount`, like this:

```csharp
var tsla = stock.Where(s => s.Symbol == "TSLA").Publish();

tsla.Where(s => s.Quote > 420).Subscribe(high);
tsla.Where(s => s.Quote < 420).Subscribe(low);

tsla.Connect();
```

This is ignoring resource management and assumes a static set of query expressions (hence the use of `Connect` after setting all of these up). Under the hood, the `Publish` operator allocates a `Subject<T>` which is used for sharing. This is effectively similar to the `tmp` variable in the classic CSE sample shown earlier.

In this pearl, we demonstrate the essentials of CSE when applied to reactive query expressions. The basic idea is to rewrite query expressions by inserting subjects (i.e. streams in Nuqleon) that can act as multicast points. In the most naive implementation, every `.` in a query expression becomes a subject (sometimes referred to as the *dot-pipe equivalence principle* by BD, i.e. in a distributed query every `.` in a query expression can become a pipe backed by a stream, which them potentially supports `T` style junctions to be added). For example:

```csharp
IDisposable d = a.B().C().D().Subscribe(o);
```

becomes

```csharp
Subject<TA> tmpa = new();
Subject<TB> tmpb = new();
Subject<TC> tmpc = new();
Subject<TD> tmpd = new();

var d =
    new CompositeDisposable(
        tmpd.Subscribe(o),
        tmpc.D().Subscribe(tmpd),
        tmpb.C().Subscribe(tmpc),
        tmpa.B().Subscribe(tmpb),
        a.Subscribe(tmp0)
    );
```

This ignores reference counting to manage the lifetime of the temporary subjects. It also ignores policies that can be used to determine when it's worth introducing a subject (i.e. based on the likelihood for reuse, which gets smaller the bigger a query gets). Focusing on the main principle, a subsequent query expression of the form:

```csharp
a.B().C().E()
```

can now reuse `tmpc` in lieu of recomputin `a.B()).C()`, thus rewriting the query as:

```csharp
tmpc.E()
```

while also adding logic to increment the reference count on `tmpc` to keep it alive as long as any query depends on it.

The code in this pearl demonstrates this mechanism using classic Rx. In Nuqleon-based services, CSE has been implemented at the query planning level (e.g. in query coordinators) by rewriting incoming query expressions into query plans that create and/or reuse existing streams that are used to share results of subexpressions. (I.e. the query gets rewritten as if an external user was using stream factories to create the intermediary "temporary" streams.)
