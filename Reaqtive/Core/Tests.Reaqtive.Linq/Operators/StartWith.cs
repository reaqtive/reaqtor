// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class StartWith : OperatorTestBase
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
        public void StartWith_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).StartWith(1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.StartWith(default(int[])));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void StartWith_SaveAndReload_Simple()
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
                () =>
                    xs.StartWith(1, 2, 3, 4, 5).Apply(Scheduler, checkpoints)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(207, 400)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(204, 2),
                OnNext(205, 3),
                OnNext(206, 4),
                OnNext(207, 5),
                OnNext(210, 1),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void StartWith_SaveAndReload_BeforeDoSubscribe()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(201, state),
                OnLoad(206, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(212, 1),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.StartWith(1, 2, 3, 4, 5).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(204, 4),
                OnNext(205, 5),
                OnNext(212, 1),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(205, 400));
        }

        [TestMethod]
        public void StartWith_SaveAndReload_AfterDoSubscribe()
        {
            var state = default(IOperatorStateContainer);

            var getQuery = FromContext(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(250, 6),
                    OnNext(350, 7),
                    OnCompleted<int>(400)
                );

                return xs.StartWith(1, 2, 3, 4, 5);
            });

            // Before checkpoint
            Run(client =>
            {
                state = client.CreateStateContainer();

                var query = getQuery(client);

                var ctx = client.CreateContext();

                var res = client.Start(() =>
                    query.Apply(Scheduler, OnSave(210, state)),
                    100, 200, 220
                );

                res.Messages.AssertEqual(
                    OnNext(201, 1),
                    OnNext(202, 2),
                    OnNext(203, 3),
                    OnNext(204, 4),
                    OnNext(205, 5)
                );
            });

            // After checkpoint
            Run(client =>
            {
                var query = getQuery(client);

                var ctx = client.CreateContext();

                var res = client.Start(ctx, () =>
                    query,
                    100, 225, 1000, state
                );

                res.Messages.AssertEqual(
                    OnNext(250, 6),
                    OnNext(350, 7),
                    OnCompleted<int>(400)
                );
            });
        }
    }
}
