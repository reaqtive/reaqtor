// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class AsyncReactiveQbservableTests
    {
        [TestMethod]
        public void AsyncReactiveQbservable_ElementType()
        {
            var q = new MyAsyncReactiveQbservable<int>(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }

        [TestMethod]
        public void AsyncReactiveQbservable_SubscribeAsync_ArgumentChecking()
        {
            var q = new MyAsyncReactiveQbservable<int>(null);
            var o = new MyAsyncReactiveQbserver<int>(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(default(IAsyncReactiveQbserver<int>), u, null, CancellationToken.None));
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(o, default(Uri), null, CancellationToken.None));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void AsyncReactiveQbservable_SubscribeAsync_NoLocalObserverSupportYet()
        {
            var q = new MyAsyncReactiveQbservable<int>(null);
            var o = new MyLocalAsyncObserver<int>();
            Assert.ThrowsException<NotSupportedException>(() => q.SubscribeAsync(o, new Uri("bar://foo"), null, CancellationToken.None));
        }

        [TestMethod]
        public void AsyncReactiveQbservable_Subscribe()
        {
            var p = new AsyncQueryProvider();
            var q = new MyAsyncReactiveQbservable<int>(p);
            var o = new MyAsyncReactiveQbserver<int>(p);
            var s = (IAsyncReactiveQubscription)q.SubscribeAsync((IAsyncReactiveObserver<int>)o, new Uri("bar://foo"), null).Result;
            Assert.IsNotNull(s.Expression);
        }
    }
}
