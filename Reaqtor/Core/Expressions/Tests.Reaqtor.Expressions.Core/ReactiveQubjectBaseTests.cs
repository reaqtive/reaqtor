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

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class ReactiveQubjectBaseTests
    {
        [TestMethod]
        public void ReactiveQubjectBase_ElementType()
        {
            var q = new MyReactiveQubject(null);
            Assert.AreEqual(typeof(int), ((IReactiveQbservable)q).ElementType);
            Assert.AreEqual(typeof(bool), ((IReactiveQbserver)q).ElementType);
        }

        [TestMethod]
        public void ReactiveQubjectBase_Subscribe_ArgumentChecking()
        {
            var q = new MyReactiveQubject(null);
            var o = new MyReactiveQbserver(null);
            var u = new Uri("bar://foo");

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(default(IReactiveQbserver<int>), u, null));
            Assert.ThrowsException<ArgumentNullException>(() => q.Subscribe(o, default(Uri), null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ReactiveQubjectBase_Subscribe_NoLocalObserverSupportYet()
        {
            var q = new MyReactiveQubject(null);
            var o = new MyLocalObserver();
            Assert.ThrowsException<NotSupportedException>(() => q.Subscribe(o, new Uri("bar://foo"), null));
        }

        private sealed class MyReactiveQubject : ReactiveQubjectBase<bool, int>
        {
            public MyReactiveQubject(IReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override IReactiveQubscription SubscribeCore(IReactiveQbserver<int> observer, Uri subscriptionUri, object state)
            {
                throw new NotImplementedException();
            }

            protected override void OnNextCore(bool value)
            {
                throw new NotImplementedException();
            }

            protected override void OnErrorCore(Exception error)
            {
                throw new NotImplementedException();
            }

            protected override void OnCompletedCore()
            {
                throw new NotImplementedException();
            }

            protected override void DisposeCore()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MyReactiveQbserver : ReactiveQbserverBase<int>
        {
            public MyReactiveQbserver(IReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override void OnNextCore(int value)
            {
                throw new NotImplementedException();
            }

            protected override void OnErrorCore(Exception error)
            {
                throw new NotImplementedException();
            }

            protected override void OnCompletedCore()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MyLocalObserver : ReactiveObserverBase<int>
        {
            protected override void OnNextCore(int value)
            {
                throw new NotImplementedException();
            }

            protected override void OnErrorCore(Exception error)
            {
                throw new NotImplementedException();
            }

            protected override void OnCompletedCore()
            {
                throw new NotImplementedException();
            }
        }
    }
}
