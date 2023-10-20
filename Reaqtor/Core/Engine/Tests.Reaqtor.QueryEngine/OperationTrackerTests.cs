// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.QueryEngine;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class OperationTrackerTests
    {
        [TestMethod]
        public void OperationTracker_Basics1()
        {
            var o = new OperationTracker();

            //
            // Enter a few times.
            //

            var g1 = o.Enter();
            var g2 = o.Enter();

            //
            // Double-dispose is fine.
            //

            g1.Dispose();
            g1.Dispose();

            //
            // Initiate dispose.
            //

            var d = o.DisposeAsync()
#if NET6_0
                .AsTask()
#endif
                ;

            Assert.IsFalse(d.IsCompleted);

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());

            //
            // Exit the last operation.
            //

            g2.Dispose();

            //
            // Tracker is disposed now.
            //

            d.Wait();

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());
        }

        [TestMethod]
        public void OperationTracker_Basics2()
        {
            var o = new OperationTracker();

            //
            // Enter a few times.
            //

            var g1 = o.Enter();
            var g2 = o.Enter();

            //
            // Initiate dispose a first time.
            //

            var d1 = o.DisposeAsync()
#if NET6_0
                .AsTask()
#endif
                ;

            Assert.IsFalse(d1.IsCompleted);

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());

            //
            // Initiate dispose a second time.
            //

            var d2 = o.DisposeAsync()
#if NET6_0
                .AsTask()
#endif
                ;

            Assert.IsFalse(d2.IsCompleted);

            //
            // Exit one of the operations.
            //

            g2.Dispose();

            Assert.IsFalse(d1.IsCompleted);
            Assert.IsFalse(d2.IsCompleted);

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());

            //
            // Exit the other operation.
            //

            g1.Dispose();

            //
            // Tracker is disposed now.
            //

            d1.Wait();
            d2.Wait();

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());

            //
            // Dispose is a no-op now.
            //

            o.DisposeAsync()
#if NET6_0
                .AsTask()
#endif
                .Wait();

            //
            // Cannot enter again.
            //

            Assert.ThrowsException<ObjectDisposedException>(() => o.Enter());
        }

        [TestMethod]
        public void OperationTracker_Concurrent()
        {
            //
            // Create enough competing threads so we have concurrency the moment we dispose the tracker.
            //

            int n = Environment.ProcessorCount * 4;

            using var c = new CountdownEvent(n);
            using var e = new ManualResetEvent(initialState: false);

            var tasks = new Thread[n];

            var t = new OperationTracker();

            //
            // Each thread will enter, then signal having arrived within the tracked scope, and then wait for a signal to continue.
            //

            for (int i = 0; i < n; i++)
            {
                var u = new Thread(() =>
                {
                    using var _ = t.Enter();

                    c.Signal();
                    e.WaitOne();

                    Thread.Yield();
                });

                tasks[i] = u;

                u.Start();
            }

            //
            // Wait for all threads to arrive, then let them loose.
            //

            c.Wait();
            e.Set();

            //
            // Initiate and await completion of closing the tracker.
            //

            t.DisposeAsync()
#if NET6_0
                .AsTask()
#endif
                .Wait();

            //
            // Join all threads.
            //

            foreach (var u in tasks)
            {
                u.Join();
            }
        }
    }
}
