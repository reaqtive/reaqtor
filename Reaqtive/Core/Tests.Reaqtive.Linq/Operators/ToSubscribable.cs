// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtive;
using Reaqtive.Disposables;
using Reaqtive.Linq;
using Reaqtive.Subjects;
using Reaqtive.Scheduler;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;
using Reaqtive.TestingFramework.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public class ToSubscribable : TestBase
    {
        private PhysicalScheduler _physicalScheduler;
        private LogicalScheduler _logicalScheduler;
        private MockOperatorContext _logicalContext;

        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
            _physicalScheduler = PhysicalScheduler.Create();
            _logicalScheduler = new LogicalScheduler(_physicalScheduler);
            _logicalContext = new MockOperatorContext { Scheduler = _logicalScheduler };
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
            _logicalContext = null;
            _logicalScheduler.Dispose();
            _physicalScheduler.Dispose();
        }

        [TestMethod]
        public void ToSubscribable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).ToSubscribable());
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(observer => Disposable.Empty).ToSubscribable().Subscribe(null));
        }

        #region +++ Virtual time +++
        [TestMethod]
        public void ToSubscribable_Completed()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnCompleted<int>(530)
            );

            var results = Scheduler.Start(() =>
                Hide(xs).ToSubscribable());

            results.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnCompleted<int>(530)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 530)
            );
        }

        [TestMethod]
        public void ToSubscribable_Error()
        {
            var ex = new Exception();

            var xs = Scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnError<int>(530, ex)
            );

            var results = Scheduler.Start(() =>
                Hide(xs).ToSubscribable());

            results.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnError<int>(530, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 530));
        }

        [TestMethod]
        public void ToSubscribable_Dispose()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6)
            );

            var results = Scheduler.Start(() =>
                Hide(xs).ToSubscribable());

            results.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000));
        }

        [TestMethod]
        public void ToSubscribable_SameTime()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(210, 2)
            );

            var results = Scheduler.Start(() =>
                Hide(xs).ToSubscribable());

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(210, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ToSubscribable_Batch()
        {
            var batch = new List<Recorded<Notification<int>>>();
            for (int i = 0; i < 256; i++)
            {
                batch.Add(OnNext(210, i));
            }

            var xs = Scheduler.CreateHotObservable(
                batch.ToArray()
            );

            var results = Scheduler.Start(() =>
                Hide(xs).ToSubscribable());

            results.Messages.AssertEqual(
                batch.ToArray()
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ToSubscribable_SaveAndReload_BeforeFirstEvent()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(202, state),
                OnLoad(204, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () => Hide(xs).ToSubscribable().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void ToSubscribable_SaveAndReload_EventsInBuffer()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(203, state),
                OnLoad(210, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(203, 4),
                OnNext(203, 5),
                OnNext(211, 6),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () => Hide(xs).ToSubscribable().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3), // will be missing because it has been processed before the checkpoint is taken.
                                //@203 Processing task has run
                OnNext(203, 4),
                OnNext(203, 5),
                //@203 Checkpoint saved

                OnNext(211, 4),
                OnNext(211, 5),
                OnNext(211, 6),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400));
        }
        #endregion

        #region +++ Concurrency +++
        [TestMethod]
        public void ToSubscribable_Concurrency_ContextSwitchOnEvents()
        {
            var subject = new Subject<int>();

            var completed = new ManualResetEvent(false);
            int i = 0;
            var result = Observer.Create<int>(
                x =>
                {
                    AssertSchedulerThread();
                    Assert.AreEqual(x, i++);
                },
                err => Assert.Fail(),
                () =>
                {
                    AssertSchedulerThread();
                    completed.Set();
                });

            var s = subject.ToSubscribable().Subscribe(result);
            InitializeSubscription(s);

            for (var j = 0; j < 100; j++)
            {
                subject.OnNext(j);
            }

            subject.OnCompleted();

            Assert.IsTrue(completed.WaitOne(TimeSpan.FromMinutes(1)));
        }

        [TestMethod]
        public void ToSubscribable_Concurrency_ContextSwitchOnCompleted()
        {
            var subject = new Subject<int>();

            var completed = new ManualResetEvent(false);
            var result = Observer.Create<int>(
                x => Assert.Fail(),
                err => Assert.Fail(),
                () =>
                {
                    AssertSchedulerThread();
                    completed.Set();
                });

            var s = subject.ToSubscribable().Subscribe(result);
            InitializeSubscription(s);
            subject.OnCompleted();
            Assert.IsTrue(completed.WaitOne(TimeSpan.FromMinutes(1)));
        }

        [TestMethod]
        public void ToSubscribable_Concurrency_ContextSwitchOnError()
        {
            var subject = new Subject<int>();

            var completed = new ManualResetEvent(false);
            var result = Observer.Create<int>(
                x => Assert.Fail(),
                err =>
                {
                    AssertSchedulerThread();
                    completed.Set();
                },
                Assert.Fail);

            var s = subject.ToSubscribable().Subscribe(result);
            InitializeSubscription(s);
            subject.OnError(new Exception());
            Assert.IsTrue(completed.WaitOne(TimeSpan.FromMinutes(1)));
        }

        [TestMethod]
        public void ToSubscribable_Concurrency_OnNextOnDisposed()
        {
            const int NumberOfMessages = 16000; // Should be big enough to cause a lock.
            var subject = new Subject<int>();
            var result = Observer.Create<int>(
                x => { },
                err => Assert.Fail(),
                Assert.Fail);

            bool exceptionCaught = false;
            var s = subject.ToSubscribable().Subscribe(result);
            InitializeSubscription(s);

            var threads = new List<Thread>();
            for (int i = 0; i < 64; i++)
            {
                threads.Add(new Thread(
                    () =>
                    {
                        try
                        {
                            for (int j = 0; j < NumberOfMessages; ++j)
                            {
                                subject.OnNext(j);
                            }
                        }
                        catch (Exception)
                        {
                            exceptionCaught = true;
                        }
                    }));
            }

            threads.ForEach(t => t.Start());
            s.Dispose();
            threads.ForEach(t => t.Join());
            Assert.IsFalse(exceptionCaught);
        }
        #endregion

        #region +++ Helpers +++
        private void InitializeSubscription(ISubscription subscription)
        {
            var visitor = new SubscriptionInitializeVisitor(subscription);
            visitor.Initialize(_logicalContext);
        }

        private static void AssertSchedulerThread()
        {
            Assert.IsTrue(Thread.CurrentThread.Name.StartsWith("Reaqtive.Scheduler.Worker"));
        }

        private static IObservable<T> Hide<T>(IObservable<T> observable)
        {
            return Observable.Create<T>(observer => observable.Subscribe(observer));
        }
        #endregion
    }
}
