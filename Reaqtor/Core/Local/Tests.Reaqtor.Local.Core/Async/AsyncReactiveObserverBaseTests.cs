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
    public class AsyncReactiveObserverBaseTests
    {
        [TestMethod]
        public void AsyncReactiveObserverBase_OnNext()
        {
            var s = new MyAsyncReactiveObserver<int>();

            var y = 0;
            s.OnNextAsyncImpl = (x, token) => { y = x; return Task.CompletedTask; };

            s.OnNextAsync(42).Wait();

            Assert.AreEqual(42, y);
        }

        [TestMethod]
        public void AsyncReactiveObserverBase_OnError_ArgumentChecking()
        {
            var s = new MyAsyncReactiveObserver<int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.OnErrorAsync(null).Wait());
        }

        [TestMethod]
        public void AsyncReactiveObserverBase_OnError()
        {
            var s = new MyAsyncReactiveObserver<int>();

            var err = default(Exception);
            s.OnErrorAsyncImpl = (e, token) => { err = e; return Task.CompletedTask; };

            var ex = new Exception();

            s.OnErrorAsync(ex).Wait();

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void AsyncReactiveObserverBase_OnCompleted()
        {
            var s = new MyAsyncReactiveObserver<int>();

            var done = false;
            s.OnCompletedAsyncImpl = token => { done = true; return Task.CompletedTask; };

            s.OnCompletedAsync().Wait();

            Assert.IsTrue(done);
        }

        private sealed class MyAsyncReactiveObserver<T> : AsyncReactiveObserverBase<T>
        {
            public Func<T, CancellationToken, Task> OnNextAsyncImpl;
            public Func<Exception, CancellationToken, Task> OnErrorAsyncImpl;
            public Func<CancellationToken, Task> OnCompletedAsyncImpl;

            protected override Task OnNextAsyncCore(T value, CancellationToken token) => OnNextAsyncImpl(value, token);

            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token) => OnErrorAsyncImpl(error, token);

            protected override Task OnCompletedAsyncCore(CancellationToken token) => OnCompletedAsyncImpl(token);
        }
    }
}
