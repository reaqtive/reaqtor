# `Nuqleon.Runtime.Remoting.Tasks`

Provides `Task<T>`-based async support for .NET Remoting including support for cancellation.

> **Note:** .NET Remoting is considered a legacy technology. However, Nuqleon has a so-called "remoting stack" that's been used extensively for testing. This assembly provides base functionality to implement this "remoting stack".

## `RemoteServiceBase`

`RemoteServiceBase` provides a base class for .NET Remoting services (i.e. a `MarshalByRefObject`) that support async methods. It contains a single `Invoke` method for use by derived types. An example of a simple service is shown below:

```csharp
class CalcService : RemoteServiceBase
{
    public IDisposable Add(int x, int y, IObserver<int> reply) => Invoke(token => AddAsync(x, y, token), reply);

    private async Task<int> AddAsync(int x, int y, CancellationToken token)
    {
        await Task.Delay(5000, token);
        return x + y;
    }
}
```

Publicly exposed methods use a signature of the form `IDisposable Operation(/*args*/, IObserver<T> reply)`:

* The `IDisposable` returned will be a `MarshalByRefObject` that allows the client to cancel the running operation, which gets mapped to a signal on the `CancellationToken` supplied by `Invoke`.
* The `IObserver<T>` passed in will be a `MarshalByRefObject` for the reply channel that has been manufactured by the client. It's used to flow either a result of type `R` or an `Exception`.

The `Invoke` method converts a `Task<T>`-based async method with `CancellationToken` support to this calling convention.

## `RemoteProxyBase`

`RemoteProxyBase` provides a base class for .NET Remoting clients ("proxies") for corresponding `RemoteServiceBase`-derived serivces. It contains a single `Invoke` method for use by derived type. An example of a proxy for the service shown earlier:

```csharp
class CalcProxy : RemoteProxyBase
{
    private readonly CalcService _svc;

    public TestProxy(CalcService svc)
    {
        _svc = svc;
    }

    public Task<int> AddAsync(int x, int y, CancellationToken token) => Invoke<int>(observer => _svc.Add(x, y, observer), token);
}
```

The cancellation support supplied via the `CancellationToken` parameter supports deep cancellation all the way to the corresponding service-side `AddAsync` method invocation.
