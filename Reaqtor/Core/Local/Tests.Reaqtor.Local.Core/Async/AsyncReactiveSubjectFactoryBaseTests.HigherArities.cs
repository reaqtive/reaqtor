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
    public class AsyncReactiveSubjectFactoryBaseTests
    {
        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase2_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase2_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, state, token) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2>(Uri streamUri, TArg1 arg1, TArg2 arg2, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase3_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase3_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, state, token) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase4_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase4_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, state, token) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase5_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase5_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, state, token) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase6_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase6_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, state, token) =>
            {
                Assert.AreEqual(uri, streamUri);
                Assert.AreEqual(1, arg1);
                Assert.AreEqual(2, arg2);
                Assert.AreEqual(3, arg3);
                Assert.AreEqual(4, arg4);
                Assert.AreEqual(5, arg5);
                Assert.AreEqual(6, arg6);
                Assert.AreSame(stateObj, state);

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase7_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase7_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase8_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase8_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase9_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase9_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase10_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase10_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase11_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase11_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase12_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase12_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase13_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase13_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase14_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase14_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state, token);
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase15_ArgumentChecking()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateAsync(null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, new object()).Wait());
        }

        [TestMethod]
        public void AsyncReactiveSubjectFactoryBase15_Create()
        {
            var s = new MyAsyncReactiveSubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>();

            var uri = new Uri("bar://foo");
            var stateObj = new object();

            s.CreateAsyncImpl = (streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state, token) =>
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

                return Task.FromResult<IAsyncReactiveSubject<int, int>>(null);
            };

            _ = s.CreateAsync(uri, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, stateObj).Result;
        }

        private delegate Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state, CancellationToken token);

        private sealed class MyAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : AsyncReactiveSubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
        {
            public CreateAsync<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateAsyncImpl;

            protected override Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state, CancellationToken token) => CreateAsyncImpl(streamUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state, token);
        }

    }
}
