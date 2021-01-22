// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Test.Reaqtive.Scheduler.Tasks;

namespace Tests.Reaqtive.Scheduler
{
    [TestClass]
    public class NopSchedulerTests
    {
        [TestMethod]
        public void NopScheduler_SwallowAllWork()
        {
            using var p = PhysicalScheduler.Create();

            var l = new LogicalScheduler(p);
            l.Dispose();

            var n = l.CreateChildScheduler();
            var o = n.CreateChildScheduler();

            Assert.IsTrue(n.CheckAccess());
            Assert.IsTrue(o.CheckAccess());

            n.VerifyAccess();
            o.VerifyAccess();

            var hasError = false;
            var h = new EventHandler<SchedulerUnhandledExceptionEventArgs>((_, e) =>
            {
                hasError = true;
            });

            n.UnhandledException += h;

            var failed = false;
            var fail = ActionTask.Create(s => { failed = true; throw new Exception(); }, 1);

            o.Schedule(fail);
            o.Schedule(TimeSpan.Zero, fail);
            o.Schedule(DateTimeOffset.UtcNow, fail);

            o.RecalculatePriority();

            Task.Run(async () =>
            {
                await o.PauseAsync();
                o.Continue();
            }).Wait();

            n.UnhandledException -= h;

            Assert.IsFalse(failed);
            Assert.IsFalse(hasError);

            var d = o.Now - DateTimeOffset.UtcNow;
            Assert.IsTrue(d < TimeSpan.FromMinutes(1));

            n.Dispose();
            o.Dispose();
        }
    }
}
