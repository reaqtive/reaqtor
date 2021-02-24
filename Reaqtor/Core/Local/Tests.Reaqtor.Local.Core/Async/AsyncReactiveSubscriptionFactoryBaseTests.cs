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
    public class AsyncReactiveSubscriptionFactoryBaseTests
    {
        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase2_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase2_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2>
        {
            public CreateAsync<TArg1, TArg2> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase3_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase3_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3>
        {
            public CreateAsync<TArg1, TArg2, TArg3> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase4_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase4_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase5_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase5_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase6_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase6_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase7_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase7_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase8_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase8_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase9_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase9_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase10_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase10_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase11_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase11_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreEqual(11, arg11);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase12_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase12_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreEqual(11, arg11);
                Assert.AreEqual(12, arg12);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase13_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase13_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreEqual(11, arg11);
                Assert.AreEqual(12, arg12);
                Assert.AreEqual(13, arg13);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase14_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase14_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreEqual(11, arg11);
                Assert.AreEqual(12, arg12);
                Assert.AreEqual(13, arg13);
                Assert.AreEqual(14, arg14);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase15_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubscriptionFactoryBase15_Create()
        {
            var s = new MyAsyncReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state, token) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreEqual(7, arg7);
                Assert.AreEqual(8, arg8);
                Assert.AreEqual(9, arg9);
                Assert.AreEqual(10, arg10);
                Assert.AreEqual(11, arg11);
                Assert.AreEqual(12, arg12);
                Assert.AreEqual(13, arg13);
                Assert.AreEqual(14, arg14);
                Assert.AreEqual(15, arg15);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubscription>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubscription> CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : AsyncReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
        {
            public CreateAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state, CancellationToken token) => CreateAsyncImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state, token);
        }

    }
}
