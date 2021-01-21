// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests
{
    [TestClass]
    public class ReactiveSubscriptionFactoryBaseTests
    {
        [TestMethod]
        public void ReactiveSubscriptionFactoryBase2_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase2_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, state) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2> : ReactiveSubscriptionFactoryBase<TArg1, TArg2>
        {
            public Create<TArg1, TArg2> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state) => CreateImpl(subscriptionUri, arg1, arg2, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase3_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase3_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, state) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3>
        {
            public Create<TArg1, TArg2, TArg3> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase4_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase4_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, state) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4>
        {
            public Create<TArg1, TArg2, TArg3, TArg4> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase5_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase5_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, state) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase6_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase6_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state) =>
            {
                Assert.AreEqual(uri, subscriptionUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase7_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase7_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase8_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase8_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase9_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase9_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase10_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase10_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase11_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase11_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase12_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase12_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase13_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase13_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase14_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase14_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state);
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase15_ArgumentChecking()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, new object()));
        }

        [TestMethod]
        public void ReactiveSubscriptionFactoryBase15_Create()
        {
            var s = new MyReactiveSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state) =>
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

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, stateObj);
        }

        private delegate IReactiveSubscription Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state);

        private sealed class MyReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : ReactiveSubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
        {
            public Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateImpl;

            protected override IReactiveSubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state) => CreateImpl(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state);
        }

    }
}
