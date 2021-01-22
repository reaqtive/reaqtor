// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ReactiveSubjectBaseTests
    {
        [TestMethod]
        public void ReactiveSubjectBase_OnNext()
        {
            var s = new MyReactiveSubject<int>();

            var y = 0;
            s.OnNextImpl = (x) => { y = x; };

            s.OnNextImpl(42);

            Assert.AreEqual(42, y);
        }

        [TestMethod]
        public void ReactiveSubjectBase_OnError_ArgumentChecking()
        {
            var s = new MyReactiveSubject<int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.OnError(null));
        }

        [TestMethod]
        public void ReactiveSubjectBase_OnError()
        {
            var s = new MyReactiveSubject<int>();

            var err = default(Exception);
            s.OnErrorImpl = (e) => { err = e; };

            var ex = new Exception();

            s.OnErrorImpl(ex);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ReactiveSubjectBase_OnCompleted()
        {
            var s = new MyReactiveSubject<int>();

            var done = false;
            s.OnCompletedImpl = () => { done = true; };

            s.OnCompletedImpl();

            Assert.IsTrue(done);
        }

        [TestMethod]
        public void ReactiveSubjectBase_Subscribe_ArgumentChecking()
        {
            var s = new MyReactiveSubject<int>();

            var iv = new MyObserver<int>();
            var uri = new Uri("foo://bar");
            var state = "qux";

            Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(null, uri, state));
            Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(iv, null, state));
        }

        [TestMethod]
        public void ReactiveSubjectBase_Subscribe()
        {
            var s = new MyReactiveSubject<int>();

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
            s.SubscribeImpl(iv, uri, state);

            Assert.AreSame(iv, iv2);
            Assert.AreSame(uri, uri2);
            Assert.AreSame(state, state2);
        }

        [TestMethod]
        public void ReactiveSubjectBase_Dispose()
        {
            var s = new MyReactiveSubject<int>();

            var disposed = false;
            s.DisposeImpl = () => { disposed = true; };

            s.DisposeImpl();

            Assert.IsTrue(disposed);
        }

        private sealed class MyReactiveSubject<T> : ReactiveSubjectBase<T, T>
        {
            public Action<T> OnNextImpl;
            public Action<Exception> OnErrorImpl;
            public Action OnCompletedImpl;
            public Func<IReactiveObserver<T>, Uri, object, IReactiveSubscription> SubscribeImpl;
            public Action DisposeImpl;

            protected override void OnNextCore(T value)
            {
                OnNextImpl(value);
            }

            protected override void OnErrorCore(Exception error)
            {
                OnErrorImpl(error);
            }

            protected override void OnCompletedCore()
            {
                OnCompletedImpl();
            }

            protected override IReactiveSubscription SubscribeCore(IReactiveObserver<T> observer, Uri subscriptionUri, object state)
            {
                return SubscribeImpl(observer, subscriptionUri, state);
            }

            protected override void DisposeCore()
            {
                DisposeImpl();
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
