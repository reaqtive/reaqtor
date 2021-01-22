// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Remoting.Tasks;

namespace Tests
{
    [TestClass]
    public class RemoteCancellationDisposableTests
    {
        [TestMethod]
        public void RemoteCancellationDisposable_NullChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new RemoteCancellationDisposable(provider: null, Guid.Empty), ex => Assert.AreEqual("provider", ex.ParamName));
        }

        [TestMethod]
        public void RemoteCancellationDisposable_RemotingBehavior()
        {
            var guid = Guid.NewGuid();
            var testProvider = new TestCancellationProvider(guid);
            var remoteDisposable = new RemoteCancellationDisposable(testProvider, guid);

            Assert.IsNull(remoteDisposable.InitializeLifetimeService());
        }

        [TestMethod]
        public void RemoteCancellationDisposable_Simple()
        {
            var guid = Guid.NewGuid();
            var testProvider = new TestCancellationProvider(guid);
            var remoteDisposable = new RemoteCancellationDisposable(testProvider, guid);

            remoteDisposable.Dispose();
            Assert.IsTrue(testProvider.Cancelled);

            // Idempotency
            testProvider.Cancelled = false;
            remoteDisposable.Dispose();
            Assert.IsFalse(testProvider.Cancelled);
        }

        private sealed class TestCancellationProvider : ICancellationProvider
        {
            private readonly Guid _expected;

            public TestCancellationProvider(Guid expected)
            {
                _expected = expected;
            }

            public bool Cancelled
            {
                get;
                set;
            }

            public void Cancel(Guid guid)
            {
                Assert.AreEqual(_expected, guid);
                Cancelled = true;
            }
        }
    }
}
