// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests
{
    [TestClass]
    public class ReactiveSubjectFactoryBaseTests
    {
        [TestMethod]
        public void ReactiveSubjectFactoryBase2_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase2_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, state) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, stateObj);
        }

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2>(Uri streamUri, TArg1 arg1, TArg2 arg2, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2>
        {
            public Create<TInput, TOutput, TArg1, TArg2> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, object state) => CreateImpl(streamUri, arg1, arg2, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase3_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase3_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, state) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, stateObj);
        }

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state) => CreateImpl(streamUri, arg1, arg2, arg3, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase4_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase4_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, state) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreSame(stateObj, state);

                return null;
            };

            _ = s.Create(uri, 1, 2, 3, 4, stateObj);
        }

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase5_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase5_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase6_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase6_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase7_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase7_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase8_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase8_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase9_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase9_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase10_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase10_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase11_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase11_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase12_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase12_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase13_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase13_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase14_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase14_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state);
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase15_ArgumentChecking()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Create(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, new object()));
        }

        [TestMethod]
        public void ReactiveSubjectFactoryBase15_Create()
        {
            var s = new MyReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state) =>
            {
                Assert.AreEqual(uri, streamUri);
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

        private delegate IReactiveSubject<TInput, TOutput> Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state);

        private sealed class MyReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : ReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
        {
            public Create<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateImpl;

            protected override IReactiveSubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state) => CreateImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state);
        }

    }
}
