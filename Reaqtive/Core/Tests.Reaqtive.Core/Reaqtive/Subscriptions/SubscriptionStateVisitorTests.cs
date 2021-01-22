// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.TestingFramework.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class SubscriptionStateVisitorTests
    {
        [TestMethod]
        public void SubscriptionStateVisitor_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SubscriptionStateVisitor(null));

            var s = new SingleAssignmentSubscription();

            Assert.ThrowsException<ArgumentNullException>(() => new SubscriptionStateVisitor(s).LoadState(null));
            Assert.ThrowsException<ArgumentNullException>(() => new SubscriptionStateVisitor(s).SaveState(null));
        }

        [TestMethod]
        public void SubscriptionStateVisitor_Basics()
        {
            var state = new MockOperatorStateContainer();
            var writerFactory = state.CreateWriter();
            var readerFactory = state.CreateReader();

            var xs = new SimpleSubject<int>();
            var o = xs.CreateObserver();

            var ys = new Take<int>(xs, 5);

            {
                var s1 = ys.Subscribe(Observer.Create<int>(_ => { }, _ => { }, () => { }));
                var v = new SubscriptionInitializeVisitor(s1);
                v.Subscribe();
                v.Start();

                o.OnNext(42);
                o.OnNext(43);

                var sv = new SubscriptionStateVisitor(s1);

                Assert.IsTrue(sv.HasStateChanged());

                sv.SaveState(writerFactory);

                Assert.IsTrue(sv.HasStateChanged());

                sv.OnStateSaved();

                Assert.IsFalse(sv.HasStateChanged());

                o.OnNext(44);
                o.OnNext(45);

                Assert.IsTrue(sv.HasStateChanged());
            }

            {
                var done = false;

                var s2 = ys.Subscribe(Observer.Create<int>(_ => { }, _ => { }, () => { done = true; }));

                var sv = new SubscriptionStateVisitor(s2);

                sv.LoadState(readerFactory);

                var v = new SubscriptionInitializeVisitor(s2);
                v.Subscribe();
                v.Start();

                o.OnNext(46);
                Assert.IsFalse(done);

                o.OnNext(47);
                Assert.IsFalse(done);

                o.OnNext(48);
                Assert.IsTrue(done);
            }
        }

        // NB: The code below is a copy of Take<T> from Reaqtive.Linq, but is kept here for test layering purposes.

        private sealed class Take<TResult> : SubscribableBase<TResult>
        {
            private readonly ISubscribable<TResult> _source;
            private readonly int _count;

            public Take(ISubscribable<TResult> source, int count)
            {
                _source = source;
                _count = count;
            }

            protected override ISubscription SubscribeCore(IObserver<TResult> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : StatefulUnaryOperator<Take<TResult>, TResult>, IObserver<TResult>
            {
                private int _remaining;

                public _(Take<TResult> parent, IObserver<TResult> observer)
                    : base(parent, observer)
                {
                    _remaining = Params._count;
                }

                public override string Name => "test:Take";

                public override Version Version => new(1, 0, 0, 0);

                public void OnCompleted()
                {
                    Output.OnCompleted();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    Output.OnError(error);
                    Dispose();
                }

                public void OnNext(TResult value)
                {
                    if (_remaining > 0)
                    {
                        --_remaining;
                        StateChanged = true;

                        Output.OnNext(value);
                        if (_remaining == 0)
                        {
                            Output.OnCompleted();
                            Dispose();
                        }
                    }
                }

                protected override void LoadStateCore(IOperatorStateReader reader)
                {
                    base.LoadStateCore(reader);

                    _remaining = reader.Read<int>();
                }

                protected override void SaveStateCore(IOperatorStateWriter writer)
                {
                    base.SaveStateCore(writer);

                    writer.Write(_remaining);
                }

                protected override ISubscription OnSubscribe()
                {
                    return Params._source.Subscribe(this);
                }
            }
        }
    }
}
