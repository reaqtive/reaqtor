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
    public class ReactiveQbservableTests
    {
        [TestMethod]
        public void ReactiveQbservable_ElementType()
        {
            var q = new MyReactiveQbservable<int>(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }

        [TestMethod]
        public void ReactiveQbservable_Subscribe_ArgumentChecking()
        {
            var q = new MyReactiveQbservable<int>(null);
            var o = new MyReactiveQbserver<int>(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(default(IReactiveQbserver<int>), u, null));
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(o, default(Uri), null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ReactiveQbservable_Subscribe_NoLocalObserverSupportYet()
        {
            var q = new MyReactiveQbservable<int>(null);
            var o = new MyLocalObserver<int>();
            Assert.ThrowsException<NotSupportedException>(() => q.Subscribe(o, new Uri("bar://foo"), null));
        }

        [TestMethod]
        public void ReactiveQbservable_Subscribe()
        {
            var p = new QueryProvider();
            var q = new MyReactiveQbservable<int>(p);
            var o = new MyReactiveQbserver<int>(p);
            var s = (IReactiveQubscription)q.Subscribe((IReactiveObserver<int>)o, new Uri("bar://foo"), null);
            Assert.IsNotNull(s.Expression);
        }
    }
}
