// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Remoting.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.System.Runtime.Remoting.Tasks
{
    [TestClass]
    public class RemoteBaseTests
    {
        [TestMethod]
        public async Task RemoteBase_ArgumentChecks()
        {
            await ProxyNullCheckAsserter.DoAsync();
            new ServiceNullCheckAsserter().Do();
        }

        [TestMethod]
        public void RemoteProxyBase_Assembly()
        {
            Type type = typeof(RemoteProxyBase);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Runtime.Remoting.Tasks", assembly);
        }

        [TestMethod]
        public void RemoteServiceBase_RemotingBehavior()
        {
            var mre = new ManualResetEvent(false);
            var svc = new TestService(mre);
            Assert.IsNull(svc.InitializeLifetimeService());
        }

        [TestMethod]
        public void RemoteProxyBase_NoCancellation()
        {
            var mre = new ManualResetEvent(false);
            var svc = new TestService(mre);
            var proxy = new TestProxy(svc);

            var val = 42;
            var t = proxy.IncrementAsync(val);
            Assert.IsFalse(t.IsCompleted);
            mre.Set();
            Assert.AreEqual(val + 1, t.Result);

            mre.Reset();

            var t2 = proxy.IncrementAsync(val, CancellationToken.None);
            Assert.IsFalse(t2.IsCompleted);
            mre.Set();
            Assert.AreEqual(val + 1, t2.Result);
        }

        [TestMethod]
        public async Task RemoteProxyBase_WithCancellation_Cancelled()
        {
            var mre = new ManualResetEvent(false);
            var svc = new TestService(mre);
            var proxy = new TestProxy(svc);

            var val = 42;
            var cts = new CancellationTokenSource();
            var t = proxy.IncrementAsync(val, cts.Token);
            Assert.IsFalse(t.IsCompleted);
            Assert.IsFalse(t.IsFaulted);
            cts.Cancel();

            try
            {
                await t;
            }
            catch (TaskCanceledException)
            {
                mre.Set();
                return;
            }

            mre.Set();
            Assert.Fail();
        }

        [TestMethod]
        public async Task RemoteProxyBase_OnError()
        {
            var mre = new ManualResetEvent(false);
            var svc = new TestService(mre);
            var proxy = new TestProxy(svc);

            var error = new Exception();
            var t = proxy.OnErrorAsync(error);

            try
            {
                await t;
            }
            catch (AggregateException ex)
            {
                Assert.AreSame(error, ex.InnerException);
            }
        }

        private sealed class TestProxy : RemoteProxyBase
        {
            private readonly TestService _svc;

            public TestProxy(TestService svc)
            {
                _svc = svc;
            }

            public Task<int> OnErrorAsync(Exception ex)
            {
                return Invoke<int>(observer => _svc.OnError(ex, observer));
            }

            public Task<int> IncrementAsync(int x)
            {
                return Invoke<int>(observer => _svc.Increment(x, observer));
            }

            public Task<int> IncrementAsync(int x, CancellationToken token)
            {
                return Invoke<int>(observer => _svc.Increment(x, observer), token);
            }
        }

        private sealed class TestService : RemoteServiceBase
        {
            private readonly EventWaitHandle _handle;

            public TestService(EventWaitHandle handle)
            {
                _handle = handle;
            }

            public IDisposable OnError(Exception ex, IObserver<int> reply)
            {
                return Invoke(
                    token => Task.Run(new Func<int>(() => { throw ex; })),
                    reply);
            }

            public IDisposable Increment(int x, IObserver<int> reply)
            {
                return Invoke(
                    token => Task.Run(
                        () =>
                        {
                            while (true)
                            {
                                if (_handle.WaitOne(100))
                                    break;
                            }
                        }, token).ContinueWith(t => x + 1, token),
                    reply);
            }
        }

        private sealed class ProxyNullCheckAsserter : RemoteProxyBase
        {
            public static async Task DoAsync()
            {
                try
                {
                    await Invoke<int>(invokeRemote: null);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.AreEqual("invokeRemote", ex.ParamName);
                }

                try
                {
                    await Invoke<int>(invokeRemote: null, CancellationToken.None);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.AreEqual("invokeRemote", ex.ParamName);
                }
            }
        }

        private sealed class ServiceNullCheckAsserter : RemoteServiceBase
        {
            public void Do()
            {
                var reply = new Reply<int>(new TaskCompletionSource<int>());
                AssertEx.ThrowsException<ArgumentNullException>((Action)(() => Invoke(invokeTask: null, reply)), ex => Assert.AreEqual("invokeTask", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>((Action)(() => Invoke(_ => null, reply: (IObserver<int>)null)), ex => Assert.AreEqual("reply", ex.ParamName));
            }
        }
    }
}
