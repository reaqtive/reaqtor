// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Test.Reaqtive.Scheduler.Tasks;

namespace Tests.Reaqtive.Scheduler
{
    [TestClass]
    public class ErrorHandlingTests
    {
        [TestMethod]
        public void Scheduler_ErrorHandling_HandlePhysical()
        {
            using var ph = PhysicalScheduler.Create();

            var ex = new Exception();
            var err = default(Exception);
            var evt = new ManualResetEvent(false);
            var sh = default(IScheduler);
            var fs = default(IScheduler);

            ph.UnhandledException += (o, e) =>
            {
                err = e.Exception;
                e.Handled = true;
                sh = e.Scheduler;
                evt.Set();
            };

            using (var s = new LogicalScheduler(ph))
            {
                s.Schedule(ActionTask.Create(_ =>
                {
                    fs = s;
                    throw ex;
                }, 1));

                evt.WaitOne();
            }

            Assert.AreSame(ex, err);
            Assert.AreSame(fs, sh);
        }

        [TestMethod]
        public void Scheduler_ErrorHandling_Physical_Chain()
        {
            using var ph = PhysicalScheduler.Create();

            var ex = new Exception();
            var err = default(Exception);
            var evt = new ManualResetEvent(false);
            var log = new List<string>();
            var sh = default(IScheduler);
            var fs = default(IScheduler);

            ph.UnhandledException += (o, e) =>
            {
                log.Add("ph");
                err = e.Exception;
                e.Handled = true;
                sh = e.Scheduler;
                evt.Set();
            };

            using (var s = new LogicalScheduler(ph))
            {
                s.UnhandledException += (o, e) => { log.Add("s"); };

#pragma warning disable IDE0063 // Use simple 'using' statement (indentation helps to document the parent-child relationship)
                using (var c1 = s.CreateChildScheduler())
                {
                    c1.UnhandledException += (o, e) => { log.Add("c1"); };

                    using (var c2 = c1.CreateChildScheduler())
                    {
                        c2.UnhandledException += (o, e) => { log.Add("c2"); };

                        c2.Schedule(ActionTask.Create(_ =>
                        {
                            fs = c2;
                            throw ex;
                        }, 1));

                        evt.WaitOne();
                    }
                }
#pragma warning restore IDE0063 // Use simple 'using' statement
            }

            Assert.IsTrue(new[] { "c2", "c1", "s", "ph" }.SequenceEqual(log));
            Assert.AreSame(ex, err);
            Assert.AreSame(fs, sh);
        }

        [TestMethod]
        public void Scheduler_ErrorHandling_Logical_Chain()
        {
            using var ph = PhysicalScheduler.Create();

            var ex = new Exception();
            var err = default(Exception);
            var evt = new ManualResetEvent(false);
            var log = new List<string>();
            var sh = default(IScheduler);
            var fs = default(IScheduler);

            ph.UnhandledException += (o, e) =>
            {
                log.Add("ph");
            };

            using (var s = new LogicalScheduler(ph))
            {
                s.UnhandledException += (o, e) =>
                {
                    log.Add("s");
                    err = e.Exception;
                    e.Handled = true;
                    sh = e.Scheduler;
                    evt.Set();
                };

#pragma warning disable IDE0063 // Use simple 'using' statement (indentation helps to document the parent-child relationship)
                using (var c1 = s.CreateChildScheduler())
                {
                    c1.UnhandledException += (o, e) => { log.Add("c1"); };

                    using (var c2 = c1.CreateChildScheduler())
                    {
                        c2.UnhandledException += (o, e) => { log.Add("c2"); };

                        c2.Schedule(ActionTask.Create(_ =>
                        {
                            fs = c2;
                            throw ex;
                        }, 1));

                        evt.WaitOne();
                    }
                }
#pragma warning restore IDE0063 // Use simple 'using' statement
            }

            Assert.IsTrue(new[] { "c2", "c1", "s" }.SequenceEqual(log));
            Assert.AreSame(ex, err);
            Assert.AreSame(fs, sh);
        }
    }
}
