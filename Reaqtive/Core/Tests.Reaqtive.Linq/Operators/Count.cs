// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Count : OperatorTestBase
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
        public void Count_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Count<int>(default(ISubscribable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Count<int>(default(ISubscribable<int>), x => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Count<int>(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Count_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.CreateObserver<int>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Count();
                    var sub = ys.Subscribe(res);

                    InitializeSubscription(sub, client);

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType().GetGenericTypeDefinition())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, int.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<int>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void Count_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @305
                OnNext(310, 13),
                OnNext(340, 17),
                OnNext(370, 19),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Count().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(400, 7),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void Count_Predicate_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @305
                OnNext(310, 13),
                OnNext(340, 17),
                OnNext(370, 19),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Count(x => x % 4 == 3).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(400, 3),
                OnCompleted<int>(400)
            );
        }
    }
}
