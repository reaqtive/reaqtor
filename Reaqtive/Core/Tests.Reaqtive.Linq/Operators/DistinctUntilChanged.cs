// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class DistinctUntilChanged : OperatorTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void DistinctUntilChanged_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int>(null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int>(DummySubscribable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int, int>(null, _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int, int>(DummySubscribable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int, int>(DummySubscribable<int>.Instance, _ => _, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int, int>(null, _ => _, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DistinctUntilChanged<int, int>(DummySubscribable<int>.Instance, null, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void DistinctUntilChanged_Comparer_AllDifferent()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 2),
                    OnNext(230, 2),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => false))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 2),
                    OnNext(230, 2),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_Comparer_AllEqual()
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

                var res = client.Start(() =>
                    xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => true))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        private sealed class FuncComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;

            public FuncComparer(Func<T, T, bool> equals)
            {
                _equals = equals;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelectorThrows()
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
                    xs.DistinctUntilChanged(new Func<int, int>(x => { throw ex; }))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_ComparerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged(new ThrowComparer<int>(ex))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnError<int>(220, ex)
                );
            });
        }

        private sealed class ThrowComparer<T> : IEqualityComparer<T>
        {
            private readonly Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(T x, T y)
            {
                throw _ex;
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelector_Comparer()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), // * key = 1    % 3 = 1
                    OnNext(220, 8), //   key = 4    % 3 = 1   same
                    OnNext(230, 2), //   key = 1    % 3 = 1   same
                    OnNext(240, 5), // * key = 2    % 3 = 2
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged(x => x / 2, new FuncComparer<int>((x, y) => x % 3 == y % 3))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                OnNext(270, 1),
                // state saved
                OnNext(280, 2),
                // state loaded
                OnNext(310, 1),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.DistinctUntilChanged().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(280, 2),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );
        }
    }
}
