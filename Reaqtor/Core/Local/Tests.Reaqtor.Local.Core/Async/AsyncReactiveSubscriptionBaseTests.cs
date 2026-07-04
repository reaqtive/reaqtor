// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor;

namespace Tests;

[TestClass]
public class AsyncReactiveSubscriptionBaseTests
{
    [TestMethod]
    public void AsyncReactiveSubscriptionBase_Dispose()
    {
        var s = new MyAsyncReactiveSubscription();

        var disposed = false;
        s.DisposeAsyncImpl = (token) => { disposed = true; return Task.CompletedTask; };

        s.DisposeAsync().AsTask().Wait();

        Assert.IsTrue(disposed);
    }

    private sealed class MyAsyncReactiveSubscription : AsyncReactiveSubscriptionBase
    {
        public Func<CancellationToken, Task> DisposeAsyncImpl;

        protected override Task DisposeAsyncCore(CancellationToken token) => DisposeAsyncImpl(token);
    }
}
