// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Threading;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Core
{
    [TestClass]
    public class AsyncReactiveObserverExtensionsTests
    {
        [TestMethod]
        public void AsyncReactiveObserverExtensions_ArgumentChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AsyncReactiveObserverExtensions.OnNextAsync<int>(null, 42));
            Assert.ThrowsException<ArgumentNullException>(() => AsyncReactiveObserverExtensions.OnErrorAsync<int>(null, new Exception()));
            Assert.ThrowsException<ArgumentNullException>(() => AsyncReactiveObserverExtensions.OnCompletedAsync<int>(null));
            Assert.ThrowsException<ArgumentNullException>(() => AsyncReactiveObserverExtensions.ToObserver<int>(null));
        }

        [TestMethod]
        public void AsyncReactiveObserverExtensions_ToObserver()
        {
            var o = new TestObserver().ToObserver();

            // The TestObserver implementation asserts correct behavior internally.
            o.OnNext(2);
            o.OnError(new Exception());
            o.OnCompleted();
        }

        private sealed class TestObserver : AsyncReactiveObserverBase<int>
        {
            protected override Task OnNextAsyncCore(int value, CancellationToken token)
            {
                Assert.AreEqual(CancellationToken.None, token);
                return Task.FromResult(true);
            }

            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
            {
                Assert.AreEqual(CancellationToken.None, token);
                return Task.FromResult(true);
            }

            protected override Task OnCompletedAsyncCore(CancellationToken token)
            {
                Assert.AreEqual(CancellationToken.None, token);
                return Task.FromResult(true);
            }
        }
    }
}
