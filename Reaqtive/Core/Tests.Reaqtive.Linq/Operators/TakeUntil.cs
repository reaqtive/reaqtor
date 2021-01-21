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
    public partial class TakeUntil : OperatorTestBase
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
        public void TakeUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.TakeUntil<int, int>(null, DummySubscribable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.TakeUntil<int, int>(DummySubscribable<int>.Instance, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.TakeUntil(default(ISubscribable<int>), DateTimeOffset.Now));
        }

        [TestMethod]
        public void TakeUntil_Preempt_BeforeFirstProduced_RemainSilentAndProperDisposed()
        {
            Run(client =>
            {
                bool sourceNotDisposed = false;

                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(215, new Exception()), // should not come
                    OnCompleted<int>(240)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //!
                    OnCompleted<int>(220)
                );

                var res = client.Start(() =>
                    l.Do(_ => sourceNotDisposed = true).TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(210)
                );

                Assert.IsFalse(sourceNotDisposed);
            });
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_AfterLastProduced_ProperDisposedSignal()
        {
            Run(client =>
            {
                bool signalNotDisposed = false;

                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(230, 2),
                    OnCompleted<int>(240)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnCompleted<int>(260)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r.Do(_ => signalNotDisposed = true))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 2),
                    OnCompleted<int>(240)
                );

                Assert.IsFalse(signalNotDisposed);
            });
        }
    }
}
