// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Scheduler;

using Test.Reaqtive.Scheduler.Tasks;

namespace Tests.Reaqtive.Scheduler
{
    [TestClass]
    public class WorkItemBaseTests
    {
        [TestMethod]
        public void Scheduler_WorkItemBase_ArgumentChecking()
        {
            var s = new Scheduler();
            var a = ActionTask.Create(_ => true, 1);
            var d = new Disposable();

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => new WorkItemBase<int>(default(IScheduler), a, 0, d));
            Assert.ThrowsException<ArgumentNullException>(() => new WorkItemBase<int>(s, default(ISchedulerTask), 0, d));
            Assert.ThrowsException<ArgumentNullException>(() => new WorkItemBase<int>(s, a, 0, default(IDisposable)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Scheduler_WorkItemBase_Simple()
        {
            var s = new Scheduler();
            var a = ActionTask.Create(_ => true, 17);
            var t = 42;
            var d = new Disposable();

            var w = new WorkItemBase<int>(s, a, t, d);

            Assert.AreSame(s, w.Scheduler);
            Assert.AreSame(a, w.Task);
            Assert.AreEqual(t, w.DueTime);
            Assert.AreEqual(a.Priority, w.Priority);

            w.DueTime = 43;

            Assert.AreEqual(43, w.DueTime);
        }

        [TestMethod]
        public void Scheduler_WorkItemBase_Order()
        {
            var s = new Scheduler();
            var d = new Disposable();

            var a1 = ActionTask.Create(_ => true, 17);
            var t1 = 42;

            var a2 = ActionTask.Create(_ => true, 18);
            var t2 = 41;

            var a3 = ActionTask.Create(_ => true, 19);
            var t3 = 43;

            var a4 = ActionTask.Create(_ => true, 16);
            var t4 = 41;

            var w1 = new WorkItemBase<int>(s, a1, t1, d);
            var w2 = new WorkItemBase<int>(s, a2, t2, d);
            var w3 = new WorkItemBase<int>(s, a3, t3, d);
            var w4 = new WorkItemBase<int>(s, a4, t4, d);

            var res = new[] { w1, w2, w3, w4 }.OrderBy(x => x).ToList();

            Assert.IsTrue(new[] { w4, w2, w1, w3 }.SequenceEqual(res));
        }

        private sealed class Scheduler : IScheduler
        {
            public DateTimeOffset Now => throw new NotImplementedException();

            public IScheduler CreateChildScheduler()
            {
                throw new NotImplementedException();
            }

            public void Schedule(ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public void Schedule(TimeSpan dueTime, ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public void Schedule(DateTimeOffset dueTime, ISchedulerTask task)
            {
                throw new NotImplementedException();
            }

            public Task PauseAsync()
            {
                throw new NotImplementedException();
            }

            public void Continue()
            {
                throw new NotImplementedException();
            }

            public void RecalculatePriority()
            {
                throw new NotImplementedException();
            }

            public bool CheckAccess()
            {
                throw new NotImplementedException();
            }

            public void VerifyAccess()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException { add { } remove { } }
        }

        private sealed class Disposable : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
