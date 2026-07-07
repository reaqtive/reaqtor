// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if DEBUG

using Reaqtive;

namespace Test.Reaqtive;

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

        var ex2 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(42));
        Assert.Contains("terminated", ex2.Message);
        var ex3 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("terminated", ex3.Message);
        var ex4 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("terminated", ex4.Message);

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
                Assert.AreEqual("foo", ex.Message);
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

        var ex2 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(42));
        Assert.Contains("terminated", ex2.Message);
        var ex3 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("terminated", ex3.Message);
        var ex4 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("terminated", ex4.Message);

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

        var ex2 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(43));
        Assert.Contains("processing", ex2.Message);
        var ex3 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("processing", ex3.Message);
        var ex4 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("processing", ex4.Message);

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

        var ex2 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(43));
        Assert.Contains("processing", ex2.Message);
        var ex3 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("processing", ex3.Message);
        var ex4 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("processing", ex4.Message);

        e.Set();
        t.Wait();

        var ex5 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(42));
        Assert.Contains("terminated", ex5.Message);
        var ex6 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("terminated", ex6.Message);
        var ex7 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("terminated", ex7.Message);
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

        var ex2 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(43));
        Assert.Contains("processing", ex2.Message);
        var ex3 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("processing", ex3.Message);
        var ex4 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("processing", ex4.Message);

        e.Set();
        t.Wait();

        var ex5 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnNext(42));
        Assert.Contains("terminated", ex5.Message);
        var ex6 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnError(new Exception()));
        Assert.Contains("terminated", ex6.Message);
        var ex7 = Assert.ThrowsExactly<InvalidOperationException>(() => sub.Observer.OnCompleted());
        Assert.Contains("terminated", ex7.Message);
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
#endif
