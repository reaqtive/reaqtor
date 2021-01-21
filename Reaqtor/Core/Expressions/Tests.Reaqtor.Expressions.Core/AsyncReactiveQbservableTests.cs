// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class AsyncReactiveQbservableTests
    {
        [TestMethod]
        public void AsyncReactiveQbservable_ElementType()
        {
            var q = new MyAsyncReactiveQbservable(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }

        [TestMethod]
        public void AsyncReactiveQbservable_SubscribeAsync_ArgumentChecking()
        {
            var q = new MyAsyncReactiveQbservable(null);
            var o = new MyAsyncReactiveQbserver(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(default(IAsyncReactiveQbserver<int>), u, null, CancellationToken.None));
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(o, default(Uri), null, CancellationToken.None));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void AsyncReactiveQbservable_SubscribeAsync_NoLocalObserverSupportYet()
        {
            var q = new MyAsyncReactiveQbservable(null);
            var o = new MyLocalObserver();
            Assert.ThrowsException<NotSupportedException>(() => q.SubscribeAsync(o, new Uri("bar://foo"), null, CancellationToken.None));
        }

        private sealed class MyAsyncReactiveQbservable : AsyncReactiveQbservableBase<int>
        {
            public MyAsyncReactiveQbservable(IAsyncReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<int> observer, Uri subscriptionUri, object state, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MyAsyncReactiveQbserver : AsyncReactiveQbserverBase<int>
        {
            public MyAsyncReactiveQbserver(IAsyncReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override Task OnNextAsyncCore(int value, CancellationToken token)
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

        private sealed class MyLocalObserver : AsyncReactiveObserverBase<int>
        {
            protected override Task OnNextAsyncCore(int value, CancellationToken token)
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
