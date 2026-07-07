// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Scheduler;

using Test.Reaqtive.Scheduler.Tasks;

namespace Tests.Reaqtive.Scheduler;

[TestClass]
public class AccessCheckTests
{
    [TestMethod]
    public void Scheduler_Access_Physical()
    {
        using var ph = PhysicalScheduler.Create();

        Assert.IsFalse(ph.CheckAccess());
        Assert.ThrowsExactly<InvalidOperationException>(() => ph.VerifyAccess());

        using var l = new LogicalScheduler(ph);

        Assert.IsFalse(l.CheckAccess());
        Assert.ThrowsExactly<InvalidOperationException>(() => l.VerifyAccess());

        var e = new ManualResetEvent(false);

        var b = false;

        l.Schedule(ActionTask.Create(_ =>
        {
            b = l.CheckAccess() && ph.CheckAccess();
            l.VerifyAccess();
            ph.VerifyAccess();

            e.Set();
            return true;
        }, 1));

        e.WaitOne();

        Assert.IsTrue(b);
    }
}
