// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if DEBUG

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class SubscribableBaseTests
    {
        [TestMethod]
        public void SubscribableBase_Assert_StoppedAfterOnCompleted()
        {
            var sub = new MySubscribable();

            var res = -1;

            var o = Observer.Create<int>(
                x =>
                {
                    res *= x;
                },
                ex =>
                {
                    Assert.Fail();
                },
                () =>
                {
                    res *= -1;
                }
            );

            sub.Subscribe(o);

            sub.Observer.OnNext(42);
            sub.Observer.OnCompleted();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(42), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("terminated"));

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void SubscribableBase_Assert_StoppedAfterOnError()
        {
            var sub = new MySubscribable();

            var res = -1;

            var o = Observer.Create<int>(
                x =>
                {
                    res *= x;
                },
                ex =>
                {
                    Assert.IsTrue(ex.Message == "foo");
                    res *= -1;
                },
                () =>
                {
                    Assert.Fail();
                }
            );

            sub.Subscribe(o);

            sub.Observer.OnNext(42);
            sub.Observer.OnError(new Exception("foo"));

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(42), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("terminated"));

            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void SubscribableBase_Assert_ThrowWhileBusy_OnNext()
        {
            var sub = new MySubscribable();

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var o = Observer.Create<int>(
                x =>
                {
                    s.Set();
                    e.WaitOne();
                },
                ex =>
                {
                    e.WaitOne();
                },
                () =>
                {
                    e.WaitOne();
                }
            );

            sub.Subscribe(o);

            var t = Task.Run(() => sub.Observer.OnNext(42));
            s.WaitOne();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(43), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("processing"));

            e.Set();
            t.Wait();

            sub.Observer.OnNext(43);
            sub.Observer.OnCompleted();
        }

        [TestMethod]
        public void SubscribableBase_Assert_ThrowWhileBusy_OnError()
        {
            var sub = new MySubscribable();

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var o = Observer.Create<int>(
                x =>
                {
                    e.WaitOne();
                },
                ex =>
                {
                    s.Set();
                    e.WaitOne();
                },
                () =>
                {
                    e.WaitOne();
                }
            );

            sub.Subscribe(o);

            var t = Task.Run(() => sub.Observer.OnError(new Exception()));
            s.WaitOne();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(43), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("processing"));

            e.Set();
            t.Wait();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(42), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("terminated"));
        }

        [TestMethod]
        public void SubscribableBase_Assert_ThrowWhileBusy_OnCompleted()
        {
            var sub = new MySubscribable();

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var o = Observer.Create<int>(
                x =>
                {
                    e.WaitOne();
                },
                ex =>
                {
                    e.WaitOne();
                },
                () =>
                {
                    s.Set();
                    e.WaitOne();
                }
            );

            sub.Subscribe(o);

            var t = Task.Run(() => sub.Observer.OnCompleted());
            s.WaitOne();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(43), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("processing"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("processing"));

            e.Set();
            t.Wait();

            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnNext(42), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnError(new Exception()), ex => ex.Message.Contains("terminated"));
            AssertEx.ThrowsException<InvalidOperationException>(() => sub.Observer.OnCompleted(), ex => ex.Message.Contains("terminated"));
        }

        private sealed class MySubscribable : SubscribableBase<int>
        {
            public IObserver<int> Observer;

            protected override ISubscription SubscribeCore(IObserver<int> observer)
            {
                Observer = observer;
                return new CompositeSubscription();
            }
        }
    }
}
#endif
