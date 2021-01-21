// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public class Do : TestBase
    {
        [TestMethod]
        public void Do_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(default(ISubscribable<int>), DummyObserver<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(default(ISubscribable<int>), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(default(ISubscribable<int>), _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, default(Action<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, _ => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(default(ISubscribable<int>), _ => { }, _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, default(Action<int>), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, _ => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(default(ISubscribable<int>), _ => { }, _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, default(Action<int>), _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, _ => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int>(DummySubscribable<int>.Instance, _ => { }, _ => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int, int>(default(ISubscribable<int>), x => x, DummyObserver<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int, int>(DummySubscribable<int>.Instance, default(Func<int, int>), DummyObserver<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Do<int, int>(DummySubscribable<int>.Instance, x => x, default(IObserver<int>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Do_ShouldSeeAllValues()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_PlainAction()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                var res = client.Start(() =>
                    xs.Do(_ => { i++; })
                );

                Assert.AreEqual(4, i);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextCompleted()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool completed = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; }, () => { completed = true; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsTrue(completed);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextCompleted_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>();

                int i = 0;
                bool completed = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; }, () => { completed = true; })
                );

                Assert.AreEqual(0, i);
                Assert.IsFalse(completed);

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Do_NextError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsTrue(sawError);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorNot()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; }, _ => { sawError = true; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsFalse(sawError);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompleted()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsFalse(sawError);
                Assert.IsTrue(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompletedError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; })
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsTrue(sawError);
                Assert.IsFalse(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompletedNever()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>();

                int i = 0;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(x => { i++; }, e => { sawError = true; }, () => { hasCompleted = true; })
                );

                Assert.AreEqual(0, i);
                Assert.IsFalse(sawError);
                Assert.IsFalse(hasCompleted);

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Do_Observer_SomeDataWithError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; }))
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsTrue(sawError);
                Assert.IsFalse(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_ObserverAndSelector_SomeDataWithError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(x => x * 2, Observer.Create<int>(x => { i++; sum -= x / 2; }, e => { sawError = e == ex; }, () => { hasCompleted = true; }))
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsTrue(sawError);
                Assert.IsFalse(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_Observer_SomeDataWithoutError()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; }))
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsFalse(sawError);
                Assert.IsTrue(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_ObserverAndSelector_SomeDataWithoutError()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                int i = 0;
                int sum = 2 + 3 + 4 + 5;
                bool sawError = false;
                bool hasCompleted = false;
                var res = client.Start(() =>
                    xs.Do(x => x * 2, Observer.Create<int>(x => { i++; sum -= x / 2; }, e => { sawError = true; }, () => { hasCompleted = true; }))
                );

                Assert.AreEqual(4, i);
                Assert.AreEqual(0, sum);
                Assert.IsFalse(sawError);
                Assert.IsTrue(hasCompleted);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_ObserverAndSelector_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 4),
                    OnNext(210, 3),
                    OnNext(220, 2),
                    OnNext(230, 1),
                    OnNext(240, 0),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => 100 / x, Observer.Create<int>(x => { }, e => { Assert.Fail(); }, () => { Assert.Fail(); }))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(220, 2),
                    OnNext(230, 1),
                    OnError<int>(240, e => e is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void Do_Next_NextThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { throw ex; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextCompleted_NextThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { throw ex; }, () => { })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextCompleted_CompletedThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { }, () => { throw ex; })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_NextError_NextThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { throw ex; }, _ => { })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextError_ErrorThrows()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex1)
                );

                var res = client.Start(() =>
                    xs.Do(x => { }, _ => { throw ex2; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex2)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompleted_NextThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { throw ex; }, _ => { }, () => { })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompleted_ErrorThrows()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex1)
                );

                var res = client.Start(() =>
                    xs.Do(x => { }, _ => { throw ex2; }, () => { })
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex2)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_NextErrorCompleted_CompletedThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(x => { }, _ => { }, () => { throw ex; })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Do_Observer_NextThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(Observer.Create<int>(x => { throw ex; }, _ => { }, () => { }))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_Observer_ErrorThrows()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex1)
                );

                var res = client.Start(() =>
                    xs.Do(Observer.Create<int>(x => { }, _ => { throw ex2; }, () => { }))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex2)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Do_Observer_CompletedThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Do(Observer.Create<int>(x => { }, _ => { }, () => { throw ex; }))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
