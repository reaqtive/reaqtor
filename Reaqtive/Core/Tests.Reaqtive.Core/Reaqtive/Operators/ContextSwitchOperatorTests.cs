// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Reaqtive;
using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class ContextSwitchOperatorTests
    {
        [TestMethod]
        public void ContextSwitchOperator_Monitoring()
        {
            var vals = new List<int>();
            var obv = Observer.Create<int>(vals.Add, _ => { }, () => { });
            var myOp = new MyOp<int>(obv);

            using var ps = PhysicalScheduler.Create();
            using var ls = new LogicalScheduler(ps);

            ls.PauseAsync().Wait();

            var opCtx = new Context
            {
                InstanceId = new Uri("test://sub"),
                Scheduler = ls,
            };

            new SubscriptionInitializeVisitor(myOp).Initialize(opCtx);

            var msgs = new List<int> { 4, 3, 2, 1 };

            var enqueueCount = 0;
            myOp.MonitorableQueue.Enqueueing += msg =>
            {
                Assert.AreEqual(msg, msgs[enqueueCount++]);
            };

            var countdown = new CountdownEvent(msgs.Count);
            var dequeueCount = 0;
            myOp.MonitorableQueue.Dequeued += msg =>
            {
                Assert.AreEqual(msg, msgs[dequeueCount++]);
                countdown.Signal();
            };

            foreach (var msg in msgs)
            {
                myOp.OnNext(msg);
            }

            Assert.AreEqual(msgs.Count, myOp.MonitorableQueue.QueueSize);

            ls.Continue();

            countdown.Wait();
        }

        private sealed class MyOp<TResult> : ContextSwitchOperator<bool, TResult>
        {
            public MyOp(IObserver<TResult> observer)
                : base(false, observer)
            {
            }

            public override string Name => "test:MyOp";

            public override Version Version => new(1, 0, 0, 0);
        }

        private sealed class Context : IOperatorContext
        {
            public Uri InstanceId { get; set; }

            public IScheduler Scheduler { get; set; }

            public TraceSource TraceSource => null;

            public IExecutionEnvironment ExecutionEnvironment => null;

            public bool TryGetElement<T>(string id, out T value)
            {
                value = default;
                return false;
            }
        }
    }
}
