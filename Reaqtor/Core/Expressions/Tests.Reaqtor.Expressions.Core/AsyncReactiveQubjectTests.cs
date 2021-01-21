// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class AsyncReactiveQubjectTests
    {
        [TestMethod]
        public void AsyncReactiveQubject_ElementType()
        {
            var q = new MyAsyncReactiveQubject<bool, int>(null);
            Assert.AreEqual(typeof(int), ((IAsyncReactiveQbservable)q).ElementType);
            Assert.AreEqual(typeof(bool), ((IAsyncReactiveQbserver)q).ElementType);
        }

        [TestMethod]
        public void AsyncReactiveQubject_Subscribe_ArgumentChecking()
        {
            var q = new MyAsyncReactiveQubject<bool, int>(null);
            var o = new MyAsyncReactiveQbserver<int>(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(default(IAsyncReactiveQbserver<int>), u, null).GetAwaiter().GetResult());
            Assert.ThrowsException<ArgumentNullException>(() => q.SubscribeAsync(o, default(Uri), null).GetAwaiter().GetResult());
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void AsyncReactiveQubject_Subscribe_NoLocalObserverSupportYet()
        {
            var q = new MyAsyncReactiveQubject<bool, int>(null);
            var o = new MyLocalAsyncObserver<int>();
            Assert.ThrowsException<NotSupportedException>(() => q.SubscribeAsync(o, new Uri("bar://foo"), null).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void AsyncReactiveQubject_Subscribe()
        {
            var p = new AsyncQueryProvider();
            var q = new MyAsyncReactiveQubject<bool, int>(p);
            var o = new MyAsyncReactiveQbserver<int>(p);
            var s = (IAsyncReactiveQubscription)q.SubscribeAsync((IAsyncReactiveObserver<int>)o, new Uri("bar://foo"), null).GetAwaiter().GetResult();
            Assert.IsNotNull(s.Expression);
        }
    }
}
