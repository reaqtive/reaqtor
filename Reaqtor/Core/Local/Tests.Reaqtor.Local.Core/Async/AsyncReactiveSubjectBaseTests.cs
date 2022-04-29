// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests
{
    [TestClass]
    public class AsyncReactiveSubjectBaseTests
    {
        [TestMethod]
        public void AsyncReactiveSubjectBase_OnNextAsync()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var y = 0;
            s.OnNextImpl = async (x, ct) => { y = x; await Task.Yield(); };

            s.OnNextAsync(42).Wait();

            Assert.AreEqual(42, y);
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_OnErrorAsync_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubject<int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.OnErrorAsync(null, CancellationToken.None));
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_OnErrorAsync()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var err = default(Exception);
            s.OnErrorImpl = async (e, ct) => { err = e; await Task.Yield(); };

            var ex = new Exception();

            s.OnErrorAsync(ex).Wait();

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_OnCompletedAsync()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var done = false;
            s.OnCompletedImpl = async (ct) => { done = true; await Task.Yield(); };

            s.OnCompletedAsync().Wait();

            Assert.IsTrue(done);
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_SubscribeAsync_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";

            Assert.ThrowsException<ArgumentNullException>(() => s.SubscribeAsync(null, uri, state, CancellationToken.None));
            Assert.ThrowsException<ArgumentNullException>(() => s.SubscribeAsync(iv, null, state, CancellationToken.None));
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_SubscribeAsync()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var iv2 = default(IAsyncReactiveObserver<int>);
            var uri2 = default(Uri);
            var state2 = default(object);
            s.SubscribeImpl = async (o, u, x, ct) =>
            {
                iv2 = o;
                uri2 = u;
                state2 = x;

                await Task.Yield();

                return null;
            };

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";
            s.SubscribeAsync(iv, uri, state, CancellationToken.None).Wait();

            Assert.AreSame(iv, iv2);
            Assert.AreSame(uri, uri2);
            Assert.AreSame(state, state2);
        }

        [TestMethod]
        public void AsyncReactiveSubjectBase_DisposeAsync()
        {
            var s = new MyAsyncReactiveSubject<int>();

            var disposed = false;
            s.DisposeImpl = async (ct) => { disposed = true; await Task.Yield(); };

#if NET6_0 || NETCOREAPP3_1
            s.DisposeAsync().AsTask().Wait();
#else
            s.DisposeAsync().Wait();
#endif

            Assert.IsTrue(disposed);
        }

        private sealed class MyAsyncReactiveSubject<T> : AsyncReactiveSubjectBase<T, T>
        {
            public Func<T, CancellationToken, Task> OnNextImpl;
            public Func<Exception, CancellationToken, Task> OnErrorImpl;
            public Func<CancellationToken, Task> OnCompletedImpl;
            public Func<IAsyncReactiveObserver<T>, Uri, object, CancellationToken, Task<IAsyncReactiveSubscription>> SubscribeImpl;
            public Func<CancellationToken, Task> DisposeImpl;

            protected override Task OnNextAsyncCore(T value, CancellationToken token)
            {
                return OnNextImpl(value, token);
            }

            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
            {
                return OnErrorImpl(error, token);
            }

            protected override Task OnCompletedAsyncCore(CancellationToken token)
            {
                return OnCompletedImpl(token);
            }

            protected override Task<IAsyncReactiveSubscription> SubscribeAsyncCore(IAsyncReactiveObserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
            {
                return SubscribeImpl(observer, subscriptionUri, state, token);
            }

            protected override Task DisposeAsyncCore(CancellationToken token)
            {
                return DisposeImpl(token);
            }
        }

        private sealed class MyObserver<T> : AsyncReactiveObserverBase<T>
        {
            protected override Task OnNextAsyncCore(T value, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task OnCompletedAsyncCore(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
