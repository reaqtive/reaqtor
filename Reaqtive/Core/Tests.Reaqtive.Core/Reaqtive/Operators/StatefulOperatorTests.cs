// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;
using Reaqtive.TestingFramework.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class StatefulOperatorTests
    {
        [TestMethod]
        public void StatefulOperator_Basics()
        {
            var o = Observer.Create<int>(_ => { }, _ => Assert.Fail(), () => Assert.Fail());

            {
                var mo = new MyOperator(o);

                mo.Start();
                Assert.IsTrue(mo.Started);
                //Assert.IsFalse(mo.StateChanged); // TODO: Hmm, can we do this?

                mo.OnNext(42);
                mo.OnNext(43);
                mo.OnCompleted();

                Assert.IsTrue(new[] { 42, 43 }.SequenceEqual(mo.Values));
                Assert.IsTrue(mo.Done);

                mo.Dispose();
                Assert.IsTrue(mo.Disposed);

                mo.Dispose();
                Assert.IsTrue(mo.Disposed);
            }

            {
                var mo = new MyOperator(o);

                mo.Start();
                Assert.IsTrue(mo.Started);
                //Assert.IsFalse(mo.StateChanged); // TODO: Hmm, can we do this?

                var ex = new Exception();

                mo.OnNext(42);
                mo.OnNext(43);
                mo.OnError(ex);

                Assert.IsTrue(new[] { 42, 43 }.SequenceEqual(mo.Values));
                Assert.AreSame(ex, mo.Error);

                mo.Dispose();
                Assert.IsTrue(mo.Disposed);

                mo.Dispose();
                Assert.IsTrue(mo.Disposed);
            }
        }

        [TestMethod]
        public void StatefulOperator_State()
        {
            var o = Observer.Create<int>(_ => { }, _ => Assert.Fail(), () => Assert.Fail());

            var state = new MockOperatorStateContainer();
            var writerFactory = state.CreateWriter();
            var readerFactory = state.CreateReader();

            var mo = new MyOperator(o) { State = 42 };

            mo.Start();
            Assert.IsTrue(mo.Started);
            Assert.IsTrue(mo.StateChanged);

            Assert.ThrowsException<ArgumentNullException>(() => mo.SaveState(null, mo.Version));

            var writer = writerFactory.Create(mo);
            mo.SaveState(writer, mo.Version);

            Assert.IsTrue(mo.StateChanged);
            mo.OnStateSaved();
            Assert.IsFalse(mo.StateChanged);

            var mor = new MyOperator(o);

            var reader = readerFactory.Create(mor);

            Assert.ThrowsException<ArgumentNullException>(() => mor.LoadState(null, mor.Version));

            mor.LoadState(reader, mor.Version);

            mor.Start();
            Assert.IsTrue(mor.Started);

            Assert.AreEqual(42, mor.State);

            mo.State = 43;
            Assert.IsTrue(mo.StateChanged);

            writer = writerFactory.Create(mo);
            mo.SaveState(writer, mo.Version);
            mo.OnStateSaved();
            Assert.IsFalse(mo.StateChanged);

            var moq = new MyOperator(o);

            reader = readerFactory.Create(moq);
            moq.LoadState(reader, moq.Version);

            moq.Start();
            Assert.IsTrue(moq.Started);

            Assert.AreEqual(43, moq.State);

            mo.Dispose();

            Assert.IsTrue(mo.Disposed);

            writer = writerFactory.Create(mo);
            mo.SaveState(writer, mo.Version);
            mo.OnStateSaved();

            var mop = new MyOperator(o);

            reader = readerFactory.Create(mop);
            mop.LoadState(reader, mop.Version);
            mop.Start();

            Assert.IsTrue(mop.Disposed);
            Assert.IsFalse(mop.Started);
        }

        [TestMethod]
        public void StatefulOperator_Versioning_NotSupportedByDefault()
        {
            var o = Observer.Create<int>(_ => { }, _ => Assert.Fail(), () => Assert.Fail());

            var state = new MockOperatorStateContainer();
            var writerFactory = state.CreateWriter();
            var readerFactory = state.CreateReader();

            var mo = new MyOperator(o) { State = 42 };

            mo.Start();

            var writer = writerFactory.Create(mo);
            Assert.ThrowsException<NotSupportedException>(() => mo.SaveState(writer, new Version(2, 0, 0, 0)));

            var mor = new MyOperator(o);

            var reader = readerFactory.Create(mor);
            Assert.ThrowsException<NotSupportedException>(() => mor.LoadState(reader, new Version(2, 0, 0, 0)));
        }

        private sealed class MyOperator : StatefulOperator<string, int>, IObserver<int>
        {
            public MyOperator(IObserver<int> observer)
                : base("qux", observer)
            {
            }

            public override string Name => "foo";

            public override Version Version => new Version(1, 0, 0, 0);

            public bool Done;

            public void OnCompleted()
            {
                Done = true;
            }

            public Exception Error;

            public void OnError(Exception error)
            {
                Error = error;
            }

            public List<int> Values = new();

            public void OnNext(int value)
            {
                Values.Add(value);
            }

            public bool Subscribed;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                var res = base.OnSubscribe();

                Subscribed = true;

                return res;
            }

            public bool Started;

            protected override void OnStart()
            {
                base.OnStart();

                Started = true;
            }

            public bool Disposed;

            protected override void OnDispose()
            {
                if (Disposed)
                    Assert.Fail();

                base.OnDispose();

                Disposed = true;
            }

            private int _state;

            public int State
            {
                get => _state;

                set
                {
                    _state = value;
                    StateChanged = true;
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _state = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_state);
            }
        }
    }
}
