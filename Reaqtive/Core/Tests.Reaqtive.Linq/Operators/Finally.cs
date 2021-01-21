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
    public class Finally : TestBase
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
        public void Finally_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Finally<int>(default(ISubscribable<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Finally<int>(DummySubscribable<int>.Instance, null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Finally_OnlyCalledOnce_Empty()
        {
            var invokeCount = 0;
            var someObservable = Subscribable.Empty<int>().Finally(() => { invokeCount++; });
            var d = someObservable.Subscribe();
            d.Dispose();
            d.Dispose();

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void Finally_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var invoked = false;
                var res = client.Start(() =>
                    xs.Finally(() => { invoked = true; })
                );

                Assert.IsTrue(invoked);

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Finally_Return()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var invoked = false;
                var res = client.Start(() =>
                    xs.Finally(() => { invoked = true; })
                );

                Assert.IsTrue(invoked);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Finally_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(250, ex)
                );

                var invoked = false;
                var res = client.Start(() =>
                    xs.Finally(() => { invoked = true; })
                );

                Assert.IsTrue(invoked);

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
