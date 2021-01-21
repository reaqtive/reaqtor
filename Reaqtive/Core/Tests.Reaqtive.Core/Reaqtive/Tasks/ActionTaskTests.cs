// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Reaqtive.Scheduler;
using Reaqtive.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class ActionTaskTests
    {
        [TestMethod]
        public void ActionTask_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => new ActionTask(default(Action)));
            Assert.ThrowsException<ArgumentNullException>(() => new ActionTask<int>(default(Action<int>), 42));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ActionTask_Simple()
        {
            var e = new ManualResetEvent(false);
            var task = new ActionTask(() => e.Set());
            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using var s = PhysicalScheduler.Create();
            using var l = new LogicalScheduler(s);

            l.Schedule(task);
            e.WaitOne();
        }

        [TestMethod]
        public void ActionTask_Generic_Simple()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var task = new ActionTask<int>(state => { x = state; e.Set(); }, 42);
            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using (var s = PhysicalScheduler.Create())
            using (var l = new LogicalScheduler(s))
            {
                l.Schedule(task);
                e.WaitOne();
            }

            Assert.AreEqual(42, x);
        }
    }
}
