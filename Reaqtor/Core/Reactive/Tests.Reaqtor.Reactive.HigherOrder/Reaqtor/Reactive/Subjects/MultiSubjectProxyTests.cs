// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Tasks;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;
using Reaqtive.TestingFramework.Mocks;

using Reaqtor.Reactive;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtor
{
    [TestClass]
    public class MultiSubjectProxyTests : TestBase
    {
        [TestMethod]
        public void MultiSubjectProxy_Untyped_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new MultiSubjectProxy(null), ex => Assert.AreEqual("uri", ex.ParamName));
        }

        [TestMethod]
        public void MultiSubjectProxy_Untyped_Dispose_NoOp()
        {
            using (new MultiSubjectProxy(new Uri("test://subject")))
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void MultiSubjectProxy_Untyped_AsSubscribable()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner/untyped"));
                var stream = sf.Create(uri, null);

                var proxy = new MultiSubjectProxy(uri);
                var observer = new MockObserver<int>(client);
                var sub = SubscribeRoot(proxy.GetObservable<int>(), observer);
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);

                Schedule(client, 10, () => stream.OnNext(1));
                Schedule(client, 20, () => stream.OnNext(2));
                Schedule(client, 30, () => stream.OnNext(3));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_Untyped_AsObserver()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner/untyped"));
                var inputStream = sf.Create(inputUri, null);
                var outputStream = sf.Create(outputUri, null);

                var inputProxy = new MultiSubjectProxy(inputUri);
                var outputProxy = new MultiSubjectProxy(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy.GetObservable<int>(), outputProxy.GetObserver<int>());
                var outputSub = SubscribeRoot(outputProxy.GetObservable<int>(), observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                Schedule(client, 10, () => inputStream.OnNext(1));
                Schedule(client, 20, () => inputStream.OnNext(2));
                Schedule(client, 30, () => inputStream.OnNext(3));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new MultiSubjectProxy<object, object>(null), ex => Assert.AreEqual("uri", ex.ParamName));
        }

        [TestMethod]
        public void MultiSubjectProxy_Dispose_NoOp()
        {
            using (new MultiSubjectProxy<int, int>(new Uri("test://subject")))
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void MultiSubjectProxy_AsSubscribable()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner"));
                var stream = sf.Create(uri, null);

                var proxy = new MultiSubjectProxy<int, int>(uri);
                var observer = new MockObserver<int>(client);
                var sub = SubscribeRoot(proxy, observer);
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);

                Schedule(client, 10, () => stream.OnNext(1));
                Schedule(client, 20, () => stream.OnNext(2));
                Schedule(client, 30, () => stream.OnNext(3));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_AsObservable()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://stream");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner"));
                var stream = sf.Create(uri, null);

                var proxy = new MultiSubjectProxy<int, int>(uri);
                var observer = new MockObserver<int>(client);
                var sub = ((IObservable<int>)proxy).Subscribe(observer);
                new SubscriptionInitializeVisitor((ISubscription)sub).Initialize(ctx);

                Schedule(client, 10, () => stream.OnNext(1));
                Schedule(client, 20, () => stream.OnNext(2));
                Schedule(client, 30, () => stream.OnNext(3));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_AsObserver()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner"));
                var inputStream = sf.Create(inputUri, null);
                var outputStream = sf.Create(outputUri, null);

                var inputProxy = new MultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new MultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy, outputProxy.CreateObserver());
                var outputSub = SubscribeRoot(outputProxy, observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                Schedule(client, 10, () => inputStream.OnNext(1));
                Schedule(client, 20, () => inputStream.OnNext(2));
                Schedule(client, 30, () => inputStream.OnNext(3));
                client.Start();

                observer.Messages.AssertEqual(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_AsObserver_OnError()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner"));
                var inputStream = sf.Create(inputUri, null);
                var outputStream = sf.Create(outputUri, null);

                var inputProxy = new MultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new MultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy, outputProxy.CreateObserver());
                var outputSub = SubscribeRoot(outputProxy, observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                var ex = new Exception();
                Schedule(client, 10, () => inputStream.OnError(ex));
                client.Start();

                observer.Messages.AssertEqual(
                    OnError<int>(10, ex)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_AsObserver_OnCompleted()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var inputUri = new Uri("test://stream/input");
                var outputUri = new Uri("test://stream/output");
                var sf = env.Reactive.GetStreamFactory<int, int>(new Uri("rx://subject/inner"));
                var inputStream = sf.Create(inputUri, null);
                var outputStream = sf.Create(outputUri, null);

                var inputProxy = new MultiSubjectProxy<int, int>(inputUri);
                var outputProxy = new MultiSubjectProxy<int, int>(outputUri);
                var observer = new MockObserver<int>(client);
                var inputSub = SubscribeRoot(inputProxy, outputProxy.CreateObserver());
                var outputSub = SubscribeRoot(outputProxy, observer);
                new SubscriptionInitializeVisitor(inputSub).Initialize(ctx);
                new SubscriptionInitializeVisitor(outputSub).Initialize(ctx);

                Schedule(client, 10, () => inputStream.OnCompleted());
                client.Start();

                observer.Messages.AssertEqual(
                    OnCompleted<int>(10)
                );
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_Untyped_AsObserver_AsSubscription()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://subject");
                var observer = new SubscriptionObserver<int>();
                var subject = new TestSubject(observer);
                env.AddArtifact(uri, subject);

                var proxy = new MultiSubjectProxy(uri);
                var sub = SubscribeRoot(new Never<int>(), proxy.GetObserver<int>());
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);
                Assert.IsTrue(observer.IsAcceptCalled);

                sub.Dispose();
                Assert.IsTrue(observer.IsDisposeCalled);
            });
        }

        [TestMethod]
        public void MultiSubjectProxy_Untyped_AsObserver_AsOperator()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();
                var ctx = client.CreateContext(env);

                var uri = new Uri("test://subject");
                var observer = new OperatorObserver<int>();
                var subject = new TestSubject(observer);
                env.AddArtifact(uri, subject);

                var proxy = new MultiSubjectProxy(uri);
                var sub = SubscribeRoot(new Never<int>(), proxy.GetObserver<int>());
                new SubscriptionInitializeVisitor(sub).Initialize(ctx);
                Assert.IsTrue(observer.IsInputsCalled);
                Assert.IsTrue(observer.IsSetContextCalled);
                Assert.IsTrue(observer.IsStartCalled);

                sub.Dispose();
                Assert.IsTrue(observer.IsDisposeCalled);
            });
        }

        private sealed class Never<T> : SubscribableBase<T>
        {
            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : UnaryOperator<Never<T>, T>
            {
                public _(Never<T> parent, IObserver<T> observer)
                    : base(parent, observer)
                {
                }
            }
        }

        private sealed class TestSubject : IMultiSubject
        {
            private readonly object _instance;

            public TestSubject(object instance)
            {
                _instance = instance;
            }

            public IObserver<T> GetObserver<T>()
            {
                return (IObserver<T>)_instance;
            }

            #region Not Implemented

            public ISubscribable<T> GetObservable<T>()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private sealed class SubscriptionObserver<T> : IObserver<T>, ISubscription
        {
            public bool IsAcceptCalled;
            public bool IsDisposeCalled;

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(T value)
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

        private sealed class OperatorObserver<T> : IObserver<T>, IOperator
        {
            public bool IsInputsCalled;
            public bool IsSetContextCalled;
            public bool IsStartCalled;
            public bool IsDisposeCalled;

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(T value)
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

        private static void Schedule(TestScheduler scheduler, long ticks, Action task)
        {
            scheduler.Schedule(new DateTimeOffset(ticks, TimeSpan.Zero), new ActionTask(task));
        }
    }
}
