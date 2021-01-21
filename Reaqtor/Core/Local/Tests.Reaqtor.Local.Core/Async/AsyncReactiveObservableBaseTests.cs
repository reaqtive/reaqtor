// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class AsyncReactiveObservableBaseTests
    {
        [TestMethod]
        public void AsyncReactiveObservableBase_Subscribe_ArgumentChecking()
        {
            var s = new MyAsyncReactiveObservable<int>();

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";

            Assert.ThrowsException<ArgumentNullException>(() => s.SubscribeAsync(null, uri, state).Wait());
            Assert.ThrowsException<ArgumentNullException>(() => s.SubscribeAsync(iv, null, state).Wait());
        }

        [TestMethod]
        public void AsyncReactiveObservableBase_Subscribe()
        {
            var s = new MyAsyncReactiveObservable<int>();

            var iv2 = default(IAsyncReactiveObserver<int>);
            var uri2 = default(Uri);
            var state2 = default(object);
            s.SubscribeAsyncImpl = (o, u, x, token) =>
            {
                iv2 = o;
                uri2 = u;
                state2 = x;

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";
            _ = s.SubscribeAsync(iv, uri, state).Result;

            Assert.AreSame(iv, iv2);
            Assert.AreSame(uri, uri2);
            Assert.AreSame(state, state2);
        }

        private sealed class MyAsyncReactiveObservable<T> : AsyncReactiveObservableBase<T>
        {
            public Func<IAsyncReactiveObserver<T>, Uri, object, CancellationToken, Task<IAsyncReactiveSubscription>> SubscribeAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> SubscribeAsyncCore(IAsyncReactiveObserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
            {
                return SubscribeAsyncImpl(observer, subscriptionUri, state, token);
            }
        }

        private sealed class MyObserver<T> : AsyncReactiveObserverBase<T>
        {
            protected override Task OnCompletedAsyncCore(CancellationToken token) => throw new NotImplementedException();
            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token) => throw new NotImplementedException();
            protected override Task OnNextAsyncCore(T value, CancellationToken token) => throw new NotImplementedException();
        }
    }
}
