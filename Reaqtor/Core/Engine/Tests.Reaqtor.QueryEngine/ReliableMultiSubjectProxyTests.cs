// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtive;
using Reaqtive.Subjects;
using Reaqtive.Tasks;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;
using Reaqtive.TestingFramework.Mocks;

using Reaqtor.QueryEngine;
using Reaqtor.Reliable;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ReliableMultiSubjectProxyTests : TestBase
    {
        [TestMethod]
        public void ReliableMultiSubjectProxy_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new ReliableMultiSubjectProxy<int, int>(null), ex => Assert.AreEqual("uri", ex.ParamName));
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_Dispose_NoOp()
        {
            using (new ReliableMultiSubjectProxy<int, int>(new Uri("test://subject")))
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsReliableObservable()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var subject = new ReliableSubject<int>();
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var observer = new MockObserver<int>(client);
                var sub = proxy.ToSubscribable().Subscribe(observer);
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);

                var subjObsvr = subject.CreateObserver();
                Schedule(client, 10, () => subjObsvr.OnNext(1, 0L));
                Schedule(client, 20, () => subjObsvr.OnNext(2, 1L));
                Schedule(client, 30, () => subjObsvr.OnNext(3, 2L));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var inputSubject = new ReliableSubject<int>();
                var outputSubject = new ReliableSubject<int>();
                env.AddArtifact(inputUri, inputSubject);
                env.AddArtifact(outputUri, outputSubject);

                var inputProxy = new ReliableMultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new ReliableMultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy.ToSubscribable(), new ObserverToReliableObserver<int>(outputProxy.CreateObserver()));
                var outputSub = SubscribeRoot(outputProxy.ToSubscribable(), observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                var subjObsvr = inputSubject.CreateObserver();
                Schedule(client, 10, () => subjObsvr.OnNext(1, 0L));
                Schedule(client, 20, () => subjObsvr.OnNext(2, 1L));
                Schedule(client, 30, () => subjObsvr.OnNext(3, 2L));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver_OnError()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var inputSubject = new ReliableSubject<int>();
                var outputSubject = new ReliableSubject<int>();
                env.AddArtifact(inputUri, inputSubject);
                env.AddArtifact(outputUri, outputSubject);

                var inputProxy = new ReliableMultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new ReliableMultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy.ToSubscribable(), new ObserverToReliableObserver<int>(outputProxy.CreateObserver()));
                var outputSub = SubscribeRoot(outputProxy.ToSubscribable(), observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                var subjObsvr = inputSubject.CreateObserver();
                var ex = new Exception();
                Schedule(client, 10, () => subjObsvr.OnError(ex));
                client.Start();

                observer.Messages.AssertEqual(
                    OnError<int>(10, ex)
                );
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver_OnCompleted()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var inputSubject = new ReliableSubject<int>();
                var outputSubject = new ReliableSubject<int>();
                env.AddArtifact(inputUri, inputSubject);
                env.AddArtifact(outputUri, outputSubject);

                var inputProxy = new ReliableMultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new ReliableMultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy.ToSubscribable(), new ObserverToReliableObserver<int>(outputProxy.CreateObserver()));
                var outputSub = SubscribeRoot(outputProxy.ToSubscribable(), observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                var subjObsvr = inputSubject.CreateObserver();
                Schedule(client, 10, () => subjObsvr.OnCompleted());
                client.Start();

                observer.Messages.AssertEqual(
                    OnCompleted<int>(10)
                );
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver_AsSubscription()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://subject");
                var observer = new SubscriptionObserver<int>();
                var subject = new TestSubject<int>(observer);
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var sub = SubscribeRoot(Subscribable.Never<int>(), new ObserverToReliableObserver<int>(proxy.CreateObserver()));
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);
                Assert.IsTrue(observer.IsAcceptCalled);

                sub.Dispose();
                Assert.IsTrue(observer.IsDisposeCalled);
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver_AsOperator()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://subject");
                var observer = new OperatorObserver<int>();
                var subject = new TestSubject<int>(observer);
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var sub = SubscribeRoot(Subscribable.Never<int>(), new ObserverToReliableObserver<int>(proxy.CreateObserver()));
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);
                Assert.IsTrue(observer.IsInputsCalled);
                Assert.IsTrue(observer.IsSetContextCalled);
                Assert.IsTrue(observer.IsStartCalled);

                sub.Dispose();
                Assert.IsTrue(observer.IsDisposeCalled);
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsReliableObservable_Dispose()
        {
            Run(client =>
            {
                var env = new TestReliableExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var subject = new ReliableSubject<int>();
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var observer = new MockObserver<int>(client);
                var sub = proxy.ToSubscribable().Subscribe(observer);
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);
                sub.Dispose();

                // Reliable subscriptions must be ack'd before they can be disposed
                // Unfortunately, this acking must be done on the implicit interface.
                SubscriptionVisitor.Do<ReliableSubscriptionBase>(s => ((IReliableSubscription)s).AcknowledgeRange(0)).Apply(sub);
                sub.Dispose();

                var isDisposed = false;
                SubscriptionVisitor.Do<ReliableSubcription<int>>(s => isDisposed |= s.IsDisposed).Apply(sub);

                Assert.IsTrue(isDisposed);
            });
        }


        [TestMethod]
        public void ReliableMultiSubjectProxy_AsObserver_InvalidEnvironment()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://subject");
                var observer = new SubscriptionObserver<int>();
                var subject = new TestSubject<int>(observer);
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var sub = SubscribeRoot(Subscribable.Never<int>(), new ObserverToReliableObserver<int>(proxy.CreateObserver()));
                Assert.ThrowsException<InvalidOperationException>(() => new SubscriptionInitializeVisitor(sub).Initialize(ctx));
            });
        }

        [TestMethod]
        public void ReliableMultiSubjectProxy_AsReliableObservable_InvalidEnvironment()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var subject = new ReliableSubject<int>();
                env.AddArtifact(uri, subject);

                var proxy = new ReliableMultiSubjectProxy<int, int>(uri);
                var observer = new MockObserver<int>(client);
                var sub = proxy.ToSubscribable().Subscribe(observer);
                Assert.ThrowsException<InvalidOperationException>(() => new SubscriptionInitializeVisitor(sub).Initialize(ctx));
            });
        }

        private sealed class ReliableSubject<T> : IReliableMultiSubject<T>
        {
            private readonly Subject<Tuple<T, long>> _subject = new();

            public IReliableObserver<T> CreateObserver()
            {
                return new Observer(this);
            }

            public IReliableSubscription Subscribe(IReliableObserver<T> observer)
            {
                return new ReliableSubcription<T>(_subject, observer);
            }

            public void Dispose()
            {
            }

            private sealed class Observer : IReliableObserver<T>
            {
                private readonly ReliableSubject<T> _parent;

                public Observer(ReliableSubject<T> parent)
                {
                    _parent = parent;
                }

                public Uri ResubscribeUri => throw new NotImplementedException();

                public void OnNext(T item, long sequenceId)
                {
                    _parent._subject.OnNext(Tuple.Create(item, sequenceId));
                }

                public void OnStarted()
                {
                }

                public void OnError(Exception error)
                {
                    _parent._subject.OnError(error);
                }

                public void OnCompleted()
                {
                    _parent._subject.OnCompleted();
                }
            }
        }

        private sealed class ReliableSubcription<T> : ReliableSubscriptionBase
        {
            private readonly Subject<Tuple<T, long>> _subject;
            private readonly IReliableObserver<T> _observer;

            private IDisposable _disposable;
            private int _isDisposed = 0;

            public ReliableSubcription(Subject<Tuple<T, long>> subject, IReliableObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public override Uri ResubscribeUri => throw new NotImplementedException();

            public override void Start(long sequenceId)
            {
                _disposable = _subject.Subscribe(
                    t => _observer.OnNext(t.Item1, t.Item2),
                    e => _observer.OnError(e),
                    () => _observer.OnCompleted());
            }

            public override void AcknowledgeRange(long sequenceId)
            {
            }

            public override void DisposeCore()
            {
                if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
                {
                    using (_disposable) { }
                }
            }

            public bool IsDisposed => _isDisposed == 1;
        }

        private sealed class TestSubject<T> : IReliableMultiSubject<T>
        {
            private readonly IReliableObserver<T> _instance;

            public TestSubject(IReliableObserver<T> instance)
            {
                _instance = instance;
            }

            public IReliableObserver<T> CreateObserver()
            {
                return _instance;
            }

            #region Not Implemented

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IReliableSubscription Subscribe(IReliableObserver<T> observer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private sealed class SubscriptionObserver<T> : IReliableObserver<T>, ISubscription
        {
            public bool IsAcceptCalled;
            public bool IsDisposeCalled;

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId)
            {
            }

            public void OnStarted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
                IsAcceptCalled = true;
            }

            public void Dispose()
            {
                IsDisposeCalled = true;
            }
        }

        private sealed class OperatorObserver<T> : IReliableObserver<T>, IOperator
        {
            public bool IsInputsCalled;
            public bool IsSetContextCalled;
            public bool IsStartCalled;
            public bool IsDisposeCalled;

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId)
            {
            }

            public void OnStarted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }

            public IEnumerable<ISubscription> Inputs
            {
                get { IsInputsCalled = true; return new[] { new DummyInput() }; }
            }

            public void Subscribe() { }

            public void SetContext(IOperatorContext context)
            {
                IsSetContextCalled = true;
            }

            public void Start()
            {
                IsStartCalled = true;
            }

            public void Dispose()
            {
                IsDisposeCalled = true;
            }

            private sealed class DummyInput : ISubscription
            {
                public void Accept(ISubscriptionVisitor visitor)
                {
                }

                public void Dispose()
                {
                }
            }
        }

        private static ISubscription SubscribeRoot<T>(ISubscribable<T> source, IObserver<T> observer)
        {
            var sub = source.Subscribe(observer);

            if (observer is ISubscription o)
            {
                return new StableCompositeSubscription(sub, o);
            }

            return sub;
        }

        private static void Schedule(global::Reaqtive.TestingFramework.TestScheduler scheduler, long ticks, Action task)
        {
            scheduler.Schedule(new DateTimeOffset(ticks, TimeSpan.Zero), new ActionTask(task));
        }
    }
}
