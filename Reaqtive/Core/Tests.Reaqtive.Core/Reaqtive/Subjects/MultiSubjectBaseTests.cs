// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Reaqtive;
using Reaqtive.Tasks;
using Reaqtive.TestingFramework.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class MultiSubjectBaseTests
    {
        [TestMethod]
        public void MultiSubjectBase_Gets()
        {
            var subject = new TestMultiSubject();
            var n = 3;

            for (var i = 0; i < n; ++i)
            {
                subject.GetObserver<int>();
            }

            for (var i = 0; i < n; ++i)
            {
                subject.GetObservable<int>();
            }

            Assert.AreEqual(n, subject.GetObserverCalls);
            Assert.AreEqual(n, subject.GetObservableCalls);
        }

        [TestMethod]
        public void MultiSubjectBase_OperatorDefaults()
        {
            var subject = new TestMultiSubject();
            Assert.AreEqual(0, subject.Inputs.Count());
        }

        [TestMethod]
        public void MultiSubjectBase_OperatorMethods()
        {
            var subject = new TestMultiSubject();
            subject.SetContext(default);
            subject.Start();

            Assert.AreEqual(1, subject.SetContextCalls);
            Assert.AreEqual(1, subject.OnStartCalls);
        }

        [TestMethod]
        public void MultiSubjectBase_Dispose()
        {
            var subject = new TestMultiSubject();
            Assert.IsFalse(subject.CheckDisposed);

            subject.Dispose();
            Assert.IsTrue(subject.CheckDisposed);
            Assert.AreEqual(1, subject.OnDisposeCalls);

            subject.Start();
            Assert.AreEqual(0, subject.OnStartCalls);

            subject.Dispose();
            Assert.AreEqual(1, subject.OnDisposeCalls);
        }

        [TestMethod]
        public void MultiSubjectBase_SubscriptionVisitor()
        {
            var subject = new TestMultiSubject();
            SubscriptionVisitor.Do<TestMultiSubject>(s => s.Foo()).Apply(subject);
            Assert.AreEqual(1, subject.FooCalls);
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_LoadSave_SameVersion()
        {
            var state = new MockOperatorStateContainer();
            var subject = new TestStatefulMultiSubject();

            var writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, subject.Version);
            Assert.AreEqual(1, subject.SaveStateCalled);
            subject.OnStateSaved();
            Assert.AreEqual(1, subject.OnStateSavedCalled);

            var reader = state.CreateReader().Create(subject);
            subject.LoadState(reader, subject.Version);
            Assert.AreEqual(1, subject.LoadStateCalled);
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_LoadSave_ThrowsArgumentException()
        {
            var subject = new TestStatefulMultiSubject();

            AssertEx.ThrowsException<ArgumentNullException>(() => subject.SaveState(null, subject.Version), ex => Assert.AreEqual("writer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => subject.LoadState(null, subject.Version), ex => Assert.AreEqual("reader", ex.ParamName));
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_LoadSave_DifferentVersion()
        {
            var state = new MockOperatorStateContainer();
            var subject = new TestStatefulMultiSubject();

            var v2 = new Version(2, 0, 0, 0);

            var writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, v2);
            Assert.AreEqual(1, subject.SaveStateVersionCalled);

            var reader = state.CreateReader().Create(subject);
            subject.LoadState(reader, v2);
            Assert.AreEqual(1, subject.LoadStateVersionCalled);
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_DisposeFromState()
        {
            var state = new MockOperatorStateContainer();
            var subject = new TestStatefulMultiSubject();
            subject.Dispose();

            var writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, subject.Version);

            subject = new TestStatefulMultiSubject();
            Assert.IsFalse(subject.CheckDisposed);
            var reader = state.CreateReader().Create(subject);
            subject.LoadState(reader, subject.Version);
            Assert.IsTrue(subject.CheckDisposed);
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_StateChanged()
        {
            var state = new MockOperatorStateContainer();
            var subject = new TestStatefulMultiSubject();

            Assert.IsTrue(subject.StateChanged);

            var writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, subject.Version);
            subject.OnStateSaved();

            Assert.IsFalse(subject.StateChanged);

            writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, subject.Version);

            Assert.IsTrue(subject.StateChanged);
        }

        [TestMethod]
        public void ToTypedMultiSubject_Simple()
        {
            var subject = new TestMultiSubject();
            var typed = subject.ToTyped<int, int>();

            typed.Subscribe(NopObserver<int>.Instance);
            Assert.AreEqual(1, subject.GetObservableCalls);

            var observer = typed.CreateObserver();
            Assert.IsNotNull(observer);
            Assert.AreEqual(1, subject.GetObserverCalls);

            ((IObservable<int>)typed).Subscribe(NopObserver<int>.Instance);
            Assert.AreEqual(2, subject.GetObservableCalls);

            typed.Dispose();
            Assert.AreEqual(1, subject.OnDisposeCalls);
        }

        [TestMethod]
        public void StatefulMultiSubjectBase_LoadFromState_StateChangedIsFalse()
        {
            var state = new MockOperatorStateContainer();
            var subject = new TestStatefulMultiSubject();
            var writer = state.CreateWriter().Create(subject);
            subject.SaveState(writer, subject.Version);
            subject.OnStateSaved();

            subject = new TestStatefulMultiSubject();
            Assert.IsTrue(subject.StateChanged);
            var reader = state.CreateReader().Create(subject);
            subject.LoadState(reader, subject.Version);
            Assert.IsFalse(subject.StateChanged);
        }

        [TestMethod]
        public void MultiSubject_Default()
        {
            var s = new MultiSubject<int>();

            Assert.ThrowsException<NotImplementedException>(() => s.CreateObserver());
            Assert.ThrowsException<NotImplementedException>(() => s.Subscribe(Observer.Nop<int>()));
            Assert.ThrowsException<NotImplementedException>(() => ((IObservable<int>)s).Subscribe(Observer.Nop<int>()));

            s.Dispose();
        }

        [TestMethod]
        public void MultiSubject_ArgumentChecking()
        {
            var s = new MultiSubject<int>();

            Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(default));

            s.Dispose();
        }

        private sealed class TestMultiSubject : MultiSubjectBase
        {
            public int GetObserverCalls;
            public int GetObservableCalls;
            public int OnStartCalls;
            public int OnDisposeCalls;
            public int SetContextCalls;
            public int FooCalls;

            protected override IObserver<T> GetObserverCore<T>()
            {
                GetObserverCalls++;
                return NopObserver<T>.Instance;
            }

            protected override ISubscribable<T> GetObservableCore<T>()
            {
                GetObservableCalls++;
                return new Return<T>(default);
            }

            protected override void OnStart()
            {
                ++OnStartCalls;
                base.OnStart();
            }

            protected override void OnDispose()
            {
                OnDisposeCalls++;
                base.OnDispose();
            }

            public override void SetContext(IOperatorContext context)
            {
                SetContextCalls++;
                base.SetContext(context);
            }

            public bool CheckDisposed => base.IsDisposed;

            public void Foo()
            {
                FooCalls++;
            }
        }

        private sealed class TestStatefulMultiSubject : StatefulMultiSubjectBase
        {
            public int LoadStateCalled;
            public int LoadStateVersionCalled;
            public int SaveStateCalled;
            public int SaveStateVersionCalled;
            public int OnStateSavedCalled;

            public TestStatefulMultiSubject()
            {
            }

            public override string Name => "foo";

            public override Version Version => new(1, 0, 0, 0);

            protected override IObserver<T> GetObserverCore<T>()
            {
                throw new NotImplementedException();
            }

            protected override ISubscribable<T> GetObservableCore<T>()
            {
                throw new NotImplementedException();
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                LoadStateCalled++;
                base.LoadStateCore(reader);
            }

            protected override void LoadStateCore(IOperatorStateReader reader, Version version)
            {
                LoadStateVersionCalled++;
                Assert.ThrowsException<NotSupportedException>(() => base.LoadStateCore(reader, version));
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                SaveStateCalled++;
                base.SaveStateCore(writer);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer, Version version)
            {
                SaveStateVersionCalled++;
                Assert.ThrowsException<NotSupportedException>(() => base.SaveStateCore(writer, version));
            }

            public override void OnStateSaved()
            {
                OnStateSavedCalled++;
                base.OnStateSaved();
            }

            public bool CheckDisposed => IsDisposed;
        }

        private sealed class Return<TResult> : SubscribableBase<TResult>
        {
            private readonly TResult _value;

            public Return(TResult value)
            {
                _value = value;
            }

            protected override ISubscription SubscribeCore(IObserver<TResult> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<Return<TResult>, TResult>
            {
                private IOperatorContext _context;

                public _(Return<TResult> parent, IObserver<TResult> observer)
                    : base(parent, observer)
                {
                }

                public override void SetContext(IOperatorContext context)
                {
                    base.SetContext(context);

                    _context = context;
                }

                protected override void OnStart()
                {
                    _context.Scheduler.Schedule(new ActionTask(() =>
                    {
                        Output.OnNext(Params._value);
                        Output.OnCompleted();
                        Dispose();
                    }));
                }
            }
        }
    }
}
