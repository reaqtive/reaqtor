// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.TestingFramework.Mocks;

using Reaqtor;
using Reaqtor.Metadata;
using Reaqtor.QueryEngine;
using Reaqtor.Reliable;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class InnerSubjectTests
    {
        [TestMethod]
        public void Engine_Subject_Empty()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            subject.Seal();

            Assert.IsTrue(svc.DeletedStreams.SequenceEqual(new[] { id }));
        }

        [TestMethod]
        public void Engine_Subject_NoIReliableMultiSubject()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            Assert.ThrowsException<NotSupportedException>(() => ((IReliableMultiSubject<int, int>)subject).CreateObserver());
        }

        [TestMethod]
        public void Engine_Subject_NoIReliableObservable()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            Assert.ThrowsException<NotSupportedException>(() => ((IReliableObservable<int>)subject).Subscribe(null));
        }

        [TestMethod]
        public void Engine_Subject_AsIObservable()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            var xs = new List<int>();

            var s = (ISubscription)((IObservable<int>)subject).Subscribe(xs.Add);

            var o = subject.CreateObserver();
            o.OnNext(41);

            var ctx = CreateOperatorContext(new Uri("tests://qux/1"), svc, new MiniScheduler());
            new SubscriptionInitializeVisitor(s).Initialize(ctx);

            o.OnNext(42);

            s.Dispose();

            o.OnNext(43);

            Assert.IsTrue(new[] { 42 }.SequenceEqual(xs));
        }

        [TestMethod]
        public void Engine_Subject_NoInputs()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            Assert.AreEqual(0, subject.Inputs.Count());
        }

        [TestMethod]
        public void Engine_Subject_Sealed()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            for (var i = 0; i < 2; i++)
            {
                subject.Seal();

                var xs = (ISubscribable<int>)subject;
                var iv = Observer.Nop<int>();

                Assert.ThrowsException<InvalidOperationException>(() => xs.Subscribe(iv));
            }
        }

        [TestMethod]
        public void Engine_Subject_Sealed_StartThrows()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            var s1 = subject.Subscribe(Observer.Nop<int>());

            subject.Seal();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var ctx = CreateOperatorContext(new Uri("tests://qux/1"), svc, new MiniScheduler());
                new SubscriptionInitializeVisitor(s1).Initialize(ctx);
            });

            Assert.ThrowsException<InvalidOperationException>(() => subject.Subscribe(Observer.Nop<int>()));
        }

        [TestMethod]
        public void Engine_Subject_Dispose()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();
            var sch = new MiniScheduler();

            var subject = GetSubject(id, svc);

            var o = subject.CreateObserver();

            var xs = new List<int>();

            var s1 = subject.Subscribe(Observer.Create<int>(xs.Add, _ => { }, () => { }));
            new SubscriptionInitializeVisitor(s1).Initialize(CreateOperatorContext(new Uri("tests://qux/1"), svc, sch));

            var s2 = subject.Subscribe(Observer.Create<int>(xs.Add, _ => { }, () => { }));
            new SubscriptionInitializeVisitor(s2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

            o.OnNext(42);

            subject.Dispose();

            o.OnNext(43); // does not throw; by design

            var s3 = subject.Subscribe(Observer.Nop<int>()); // does not throw; by design

            o.OnNext(44); // does not throw; by design

            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                new SubscriptionInitializeVisitor(s3).Initialize(CreateOperatorContext(new Uri("tests://qux/3"), svc, sch));
            });
        }

        [TestMethod]
        public void Engine_Subject_Single()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            var res = new List<int>();
            var done = false;

            var xs = (ISubscribable<int>)subject;
            var iv = Observer.Create<int>(
                x => { res.Add(x); },
                ex => { Assert.Fail(); },
                () => { done = true; }
            );

            var sub = xs.Subscribe(iv);

            var ctx = CreateOperatorContext(new Uri("tests://qux/1"), svc, new MiniScheduler());
            new SubscriptionInitializeVisitor(sub).Initialize(ctx);

            var observer = subject.CreateObserver();

            subject.Seal();

            Assert.IsTrue(svc.DeletedStreams.Count == 0);

            observer.OnNext(42);
            observer.OnNext(43);
            observer.OnCompleted();

            Assert.IsTrue(new[] { 42, 43 }.SequenceEqual(res));
            Assert.IsTrue(done);

            Assert.IsTrue(svc.DeletedStreams.Count == 0);

            sub.Dispose();

            Assert.IsTrue(svc.DeletedStreams.SequenceEqual(new[] { id }));
        }

        [TestMethod]
        public void Engine_Subject_Multiple_OnCompleted()
        {
            Engine_Subject_Multiple_Core(false);
        }

        [TestMethod]
        public void Engine_Subject_Multiple_OnError()
        {
            Engine_Subject_Multiple_Core(true);
        }

        private static void Engine_Subject_Multiple_Core(bool fail)
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var subject = GetSubject(id, svc);

            var res = new List<int>[3];
            var done = new bool[3];
            var errors = new Exception[3];

            var xs = (ISubscribable<int>)subject;

            var subs = new List<ISubscription>();

            for (var i = 0; i < 3; i++)
            {
                var j = i;

                res[j] = new List<int>();
                done[j] = false;

                var iv = Observer.Create<int>(
                    x =>
                    {
                        res[j].Add(x);
                    },
                    ex =>
                    {
                        Assert.IsNull(errors[j]);
                        errors[j] = ex;
                    },
                    () =>
                    {
                        Assert.IsFalse(done[j]);
                        done[j] = true;
                    }
                );

                var sub = xs.Subscribe(iv);

                var ctx = CreateOperatorContext(new Uri("tests://qux/" + i), svc, new MiniScheduler());
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);

                subs.Add(sub);
            }

            var observer = subject.CreateObserver();

            subject.Seal();

            Assert.IsTrue(svc.DeletedStreams.Count == 0);

            observer.OnNext(42);

            subs[1].Dispose();

            Assert.IsTrue(svc.DeletedStreams.Count == 0);

            observer.OnNext(43);

            subs[0].Dispose();

            Assert.IsTrue(svc.DeletedStreams.Count == 0);

            var err = new Exception();

            if (fail)
            {
                observer.OnError(err);
                observer.OnError(err);
            }
            else
            {
                observer.OnCompleted();
                observer.OnCompleted();
            }

            subs[2].Dispose();

            Assert.IsTrue(svc.DeletedStreams.SequenceEqual(new[] { id }));

            Assert.IsTrue(new[] { 42, 43 }.SequenceEqual(res[0]));
            Assert.IsTrue(new[] { 42 }.SequenceEqual(res[1]));
            Assert.IsTrue(new[] { 42, 43 }.SequenceEqual(res[2]));

            if (fail)
            {
                Assert.IsFalse(done[0]);
                Assert.IsFalse(done[1]);
                Assert.IsFalse(done[2]);

                Assert.IsNull(errors[0]);
                Assert.IsNull(errors[1]);
                Assert.AreSame(err, errors[2]);
            }
            else
            {
                Assert.IsFalse(done[0]);
                Assert.IsFalse(done[1]);
                Assert.IsTrue(done[2]);

                Assert.IsNull(errors[0]);
                Assert.IsNull(errors[1]);
                Assert.IsNull(errors[2]);
            }
        }

        [TestMethod]
        public void Engine_Subject_Checkpoint_Empty()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();

            var mosc = new MockOperatorStateContainer();

            {
                var subject = GetSubject(id, svc);
                subject.Seal();

                Assert.IsTrue(subject.StateChanged);

                using (var oswf = mosc.CreateWriter())
                {
                    oswf.SaveState(subject);
                }

                Assert.IsTrue(subject.StateChanged);

                subject.OnStateSaved();

                Assert.IsFalse(subject.StateChanged);
            }

            {
                var subject = GetSubject(id, svc);

                Assert.IsTrue(subject.StateChanged);

                using (var osrf = mosc.CreateReader())
                {
                    osrf.LoadState(subject);
                }

                Assert.IsFalse(subject.StateChanged);

                // gets sealed immediately upon recovery because it was empty
                Assert.ThrowsException<InvalidOperationException>(() => subject.Subscribe(Observer.Nop<int>()));
            }
        }

        [TestMethod]
        public void Engine_Subject_Checkpoint_SubscriptionsRecovering()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();
            var sch = new MiniScheduler();

            var mosc = new MockOperatorStateContainer();

            {
                var subject = GetSubject(id, svc);

                var xs = (ISubscribable<int>)subject;

                var d1 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d1).Initialize(CreateOperatorContext(new Uri("tests://qux/1"), svc, sch));

                var d2 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

                d1.Dispose();

                var d3 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d3).Initialize(CreateOperatorContext(new Uri("tests://qux/3"), svc, sch));

                subject.Seal();

                Assert.IsTrue(subject.StateChanged);

                using (var oswf = mosc.CreateWriter())
                {
                    oswf.SaveState(subject);
                }

                Assert.IsTrue(subject.StateChanged);

                subject.OnStateSaved();

                Assert.IsFalse(subject.StateChanged);
            }

            {
                var subject = GetSubject(id, svc);

                Assert.IsTrue(subject.StateChanged);

                using (var osrf = mosc.CreateReader())
                {
                    osrf.LoadState(subject);
                }

                Assert.IsFalse(subject.StateChanged);

                var xs = (ISubscribable<int>)subject;

                var d2 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

                var d3 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d3).Initialize(CreateOperatorContext(new Uri("tests://qux/3"), svc, sch));

                Assert.IsFalse(subject.StateChanged);

                // gets sealed after all subscriptions have been recreated
                Assert.ThrowsException<InvalidOperationException>(() => subject.Subscribe(Observer.Nop<int>()));

                Assert.IsFalse(subject.StateChanged);
            }
        }

        [TestMethod]
        public void Engine_Subject_Checkpoint_SealLater()
        {
            var id = new Uri("tests://bar/foo");
            var svc = new MiniService();
            var sch = new MiniScheduler();

            var mosc1 = new MockOperatorStateContainer();
            var mosc2 = new MockOperatorStateContainer();

            {
                var subject = GetSubject(id, svc);

                var xs = (ISubscribable<int>)subject;

                var d1 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d1).Initialize(CreateOperatorContext(new Uri("tests://qux/1"), svc, sch));

                var d2 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

                d1.Dispose();

                var d3 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d3).Initialize(CreateOperatorContext(new Uri("tests://qux/3"), svc, sch));

                Assert.IsTrue(subject.StateChanged);

                using (var oswf = mosc1.CreateWriter())
                {
                    oswf.SaveState(subject);
                }

                Assert.IsTrue(subject.StateChanged);

                subject.OnStateSaved();

                Assert.IsFalse(subject.StateChanged);
            }

            {
                var subject = GetSubject(id, svc);

                Assert.IsTrue(subject.StateChanged);

                using (var osrf = mosc1.CreateReader())
                {
                    osrf.LoadState(subject);
                }

                Assert.IsFalse(subject.StateChanged);

                var xs = (ISubscribable<int>)subject;

                var d2 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

                var d3 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d3).Initialize(CreateOperatorContext(new Uri("tests://qux/3"), svc, sch));

                Assert.IsFalse(subject.StateChanged);

                var d4 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d4).Initialize(CreateOperatorContext(new Uri("tests://qux/4"), svc, sch));

                Assert.IsTrue(subject.StateChanged);

                var d5 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d5).Initialize(CreateOperatorContext(new Uri("tests://qux/5"), svc, sch));

                d3.Dispose();

                subject.Seal();

                Assert.IsTrue(subject.StateChanged);

                using (var oswf = mosc2.CreateWriter())
                {
                    oswf.SaveState(subject);
                }

                Assert.IsTrue(subject.StateChanged);

                subject.OnStateSaved();

                Assert.IsFalse(subject.StateChanged);
            }

            {
                var subject = GetSubject(id, svc);

                Assert.IsTrue(subject.StateChanged);

                using (var osrf = mosc2.CreateReader())
                {
                    osrf.LoadState(subject);
                }

                Assert.IsFalse(subject.StateChanged);

                var xs = (ISubscribable<int>)subject;

                var d2 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d2).Initialize(CreateOperatorContext(new Uri("tests://qux/2"), svc, sch));

                var d4 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d4).Initialize(CreateOperatorContext(new Uri("tests://qux/4"), svc, sch));

                var d5 = xs.Subscribe(Observer.Nop<int>());
                new SubscriptionInitializeVisitor(d5).Initialize(CreateOperatorContext(new Uri("tests://qux/5"), svc, sch));

                Assert.IsFalse(subject.StateChanged);

                // gets sealed after all subscriptions have been recreated
                Assert.ThrowsException<InvalidOperationException>(() => subject.Subscribe(Observer.Nop<int>()));
            }
        }

        private static InnerSubject<int> GetSubject(Uri id, MiniService svc)
        {
            var sch = new MiniScheduler();
            var ctx = CreateOperatorContext(id, svc, sch);

            var subject = new InnerSubject<int>();
            subject.SetContext(ctx);
            subject.Start();

            return subject;
        }

        private static HostedOperatorContext CreateOperatorContext(Uri uri, IReactive service, IScheduler scheduler, IExecutionEnvironment executionEnvironment = null)
        {
            return new HostedOperatorContext(uri, scheduler, traceSource: null, executionEnvironment, service);
        }

        private class MiniService : IReactive
        {
            public readonly HashSet<Uri> DeletedStreams = new();

            public IReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri)
            {
                return new Qubject<TInput, TOutput>(this, uri);
            }

            private sealed class Qubject<TInput, TOutput> : IReactiveQubject<TInput, TOutput>
            {
                private readonly MiniService _parent;
                private readonly Uri _uri;

                public Qubject(MiniService parent, Uri uri)
                {
                    _parent = parent;
                    _uri = uri;
                }

                public void Dispose()
                {
                    _parent.DeletedStreams.Add(_uri);
                }

                public void OnNext(TInput value)
                {
                    throw new NotImplementedException();
                }

                public void OnError(Exception error)
                {
                    throw new NotImplementedException();
                }

                public void OnCompleted()
                {
                    throw new NotImplementedException();
                }

                public IReactiveSubscription Subscribe(IReactiveObserver<TOutput> observer, Uri subscriptionUri, object state)
                {
                    throw new NotImplementedException();
                }

                public Type ElementType => throw new NotImplementedException();

                public IReactiveQueryProvider Provider => throw new NotImplementedException();

                public System.Linq.Expressions.Expression Expression => throw new NotImplementedException();

                public IReactiveQubscription Subscribe(IReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state)
                {
                    throw new NotImplementedException();
                }
            }

            public IReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQbservable<T> GetObservable<T>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQbserver<T> GetObserver<T>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQubscription GetSubscription(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public void DefineStreamFactory<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void DefineStreamFactory<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineStreamFactory(Uri uri)
            {
                throw new NotImplementedException();
            }

            public void DefineObservable<T>(Uri uri, IReactiveQbservable<T> observable, object state)
            {
                throw new NotImplementedException();
            }

            public void DefineObservable<TArgs, TResult>(Uri uri, System.Linq.Expressions.Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineObservable(Uri uri)
            {
                throw new NotImplementedException();
            }

            public void DefineObserver<T>(Uri uri, IReactiveQbserver<T> observer, object state)
            {
                throw new NotImplementedException();
            }

            public void DefineObserver<TArgs, TResult>(Uri uri, System.Linq.Expressions.Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineObserver(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public void DefineSubscriptionFactory(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void DefineSubscriptionFactory<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineSubscriptionFactory(Uri uri)
            {
                throw new NotImplementedException();
            }

            public IQueryableDictionary<Uri, global::Reaqtor.Metadata.IReactiveStreamFactoryDefinition> StreamFactories => throw new NotImplementedException();

            public IQueryableDictionary<Uri, global::Reaqtor.Metadata.IReactiveStreamProcess> Streams => throw new NotImplementedException();

            public IQueryableDictionary<Uri, global::Reaqtor.Metadata.IReactiveObservableDefinition> Observables => throw new NotImplementedException();

            public IQueryableDictionary<Uri, global::Reaqtor.Metadata.IReactiveObserverDefinition> Observers => throw new NotImplementedException();

            public IQueryableDictionary<Uri, global::Reaqtor.Metadata.IReactiveSubscriptionProcess> Subscriptions => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories => throw new NotImplementedException();
        }

        private class MiniScheduler : IScheduler
        {
            public DateTimeOffset Now => throw new NotImplementedException();

            public IScheduler CreateChildScheduler()
            {
                throw new NotImplementedException();
            }

            public void Schedule(ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public void Schedule(TimeSpan dueTime, ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public void Schedule(DateTimeOffset dueTime, ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public Task PauseAsync()
            {
                throw new NotImplementedException();
            }

            public void Continue()
            {
                throw new NotImplementedException();
            }

            public void RecalculatePriority()
            {
                throw new NotImplementedException();
            }

            public bool CheckAccess()
            {
                throw new NotImplementedException();
            }

            public void VerifyAccess()
            {
                throw new NotImplementedException();
            }

            public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException
            {
                add { }
                remove { }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
