// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ReactiveObserverBaseTests
    {
        [TestMethod]
        public void ReactiveObserverBase_OnNext()
        {
            var s = new MyReactiveObserver<int>();

            var y = 0;
            s.OnNextImpl = (x) => { y = x; };

            s.OnNext(42);

            Assert.AreEqual(42, y);
        }

        [TestMethod]
        public void ReactiveObserverBase_OnError_ArgumentChecking()
        {
            var s = new MyReactiveObserver<int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.OnError(null));
        }

        [TestMethod]
        public void ReactiveObserverBase_OnError()
        {
            var s = new MyReactiveObserver<int>();

            var err = default(Exception);
            s.OnErrorImpl = (e) => { err = e; };

            var ex = new Exception();

            s.OnError(ex);

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void ReactiveObserverBase_OnCompleted()
        {
            var s = new MyReactiveObserver<int>();

            var done = false;
            s.OnCompletedImpl = () => { done = true; };

            s.OnCompleted();

            Assert.IsTrue(done);
        }

        private sealed class MyReactiveObserver<T> : ReactiveObserverBase<T>
        {
            public Action<T> OnNextImpl;
            public Action<Exception> OnErrorImpl;
            public Action OnCompletedImpl;

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
        }
    }
}
