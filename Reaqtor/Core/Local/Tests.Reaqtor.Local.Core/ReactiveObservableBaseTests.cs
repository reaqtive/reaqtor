// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ReactiveObservableBaseTests
    {
        [TestMethod]
        public void ReactiveObservableBase_Subscribe_ArgumentChecking()
        {
            var s = new MyReactiveObservable<int>();

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";

            Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(null, uri, state));
            Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(iv, null, state));
        }

        [TestMethod]
        public void ReactiveObservableBase_Subscribe()
        {
            var s = new MyReactiveObservable<int>();

            var iv2 = default(IReactiveObserver<int>);
            var uri2 = default(Uri);
            var state2 = default(object);
            s.SubscribeImpl = (o, u, x) =>
            {
                iv2 = o;
                uri2 = u;
                state2 = x;

                return null;
            };

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";
            s.Subscribe(iv, uri, state);

            Assert.AreSame(iv, iv2);
            Assert.AreSame(uri, uri2);
            Assert.AreSame(state, state2);
        }

        private sealed class MyReactiveObservable<T> : ReactiveObservableBase<T>
        {
            public Func<IReactiveObserver<T>, Uri, object, IReactiveSubscription> SubscribeImpl;

            protected override IReactiveSubscription SubscribeCore(IReactiveObserver<T> observer, Uri subscriptionUri, object state)
            {
                return SubscribeImpl(observer, subscriptionUri, state);
            }
        }

        private sealed class MyObserver<T> : ReactiveObserverBase<T>
        {
            protected override void OnNextCore(T value)
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
