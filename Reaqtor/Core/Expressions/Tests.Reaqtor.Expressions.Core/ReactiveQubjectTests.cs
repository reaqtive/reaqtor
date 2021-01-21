// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class ReactiveQubjectTests
    {
        [TestMethod]
        public void ReactiveQubject_ElementType()
        {
            var q = new MyReactiveQubject<bool, int>(null);
            Assert.AreEqual(typeof(int), ((IReactiveQbservable)q).ElementType);
            Assert.AreEqual(typeof(bool), ((IReactiveQbserver)q).ElementType);
        }

        [TestMethod]
        public void ReactiveQubject_Subscribe_ArgumentChecking()
        {
            var q = new MyReactiveQubject<bool, int>(null);
            var o = new MyReactiveQbserver<int>(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(default(IReactiveQbserver<int>), u, null));
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(o, default(Uri), null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ReactiveQubject_Subscribe_NoLocalObserverSupportYet()
        {
            var q = new MyReactiveQubject<bool, int>(null);
            var o = new MyLocalObserver<int>();
            Assert.ThrowsException<NotSupportedException>(() => q.Subscribe(o, new Uri("bar://foo"), null));
        }

        [TestMethod]
        public void ReactiveQubject_Subscribe()
        {
            var p = new QueryProvider();
            var q = new MyReactiveQubject<bool, int>(p);
            var o = new MyReactiveQbserver<int>(p);
            var s = (IReactiveQubscription)q.Subscribe((IReactiveObserver<int>)o, new Uri("bar://foo"), null);
            Assert.IsNotNull(s.Expression);
        }
    }
}
