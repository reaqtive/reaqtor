// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Scheduler;
using Reaqtive.Tasks;

namespace Test.Reaqtive
{
    [TestClass]
    public class ItemProcessingTaskTests
    {
        [TestMethod]
        public void ItemProcessingTask_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ItemProcessingTask(null));
        }

        [TestMethod]
        public void ItemProcessingTask_Simple()
        {
            const int IterationCount = 1; // NB: Increase this to debug subtle issues.

            for (var i = 0; i < IterationCount; i++)
            {
                ItemProcessingTask_Simple_Core();
            }
        }

        private static void ItemProcessingTask_Simple_Core()
        {
            var proc = new ItemProcessor();
            var task = new ItemProcessingTask(proc);

            Assert.AreEqual(2, task.Priority);
            Assert.IsFalse(task.IsRunnable);

            task.RecalculatePriority();

            Assert.IsFalse(task.IsRunnable);

            proc.Add();

            Assert.IsTrue(task.IsRunnable);

            using var s = PhysicalScheduler.Create();
            using var l = new LogicalScheduler(s);

            l.Schedule(task);
            proc.Events[0].WaitOne();

            proc.Add();
            l.RecalculatePriority();
            proc.Events[1].WaitOne();
        }

        private sealed class ItemProcessor : IItemProcessor
        {
            public void Add() => ItemCount++;

            public int ItemCount { get; private set; }

            private int _index;
            public ManualResetEvent[] Events = new[] { new ManualResetEvent(false), new ManualResetEvent(false) };

            public void Process(int batchSize)
            {
                ItemCount--;
                Events[_index++].Set();
            }
        }
    }
}
