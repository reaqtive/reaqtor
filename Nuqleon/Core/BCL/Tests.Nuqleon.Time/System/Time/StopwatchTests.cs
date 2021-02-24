// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Wrote these tests.
//

using System;
using System.Threading;
using System.Time;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class StopwatchTests
    {
        [TestMethod]
        public void Stopwatch_Diagnostics()
        {
            var sw = StopwatchFactory.Diagnostics.Create();

            Assert.AreEqual(0, sw.ElapsedTicks);
            Assert.AreEqual(0, sw.ElapsedMilliseconds);
            Assert.AreEqual(0, sw.Elapsed.Ticks);
            Assert.IsFalse(sw.IsRunning);

            sw.Start();
            Assert.IsTrue(sw.IsRunning);

            sw.Stop();
            Assert.IsFalse(sw.IsRunning);

            sw.Reset();
            Assert.AreEqual(0, sw.ElapsedTicks);
            Assert.AreEqual(0, sw.ElapsedMilliseconds);
            Assert.AreEqual(0, sw.Elapsed.Ticks);
            Assert.IsFalse(sw.IsRunning);

            sw.Restart();
            Assert.IsTrue(sw.IsRunning);

            Assert.IsTrue(SpinWait.SpinUntil(() => sw.ElapsedTicks > 100, 60000));

            Assert.IsTrue(sw.GetTimestamp() > 0L);
        }

        [TestMethod]
        public void StopwatchFactory_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StopwatchFactory.FromClock(clock: null));
        }

        [TestMethod]
        public void StopwatchFactoryExtensions_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StopwatchFactoryExtensions.StartNew(factory: null));
        }

        [TestMethod]
        public void StopwatchFactoryExtensions_StartNew()
        {
            var c = new VirtualTimeClock();

            var sw = StopwatchFactory.FromClock(c).StartNew();

            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(0, sw.ElapsedTicks);

            c.Now += 1000;
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(1000, sw.ElapsedTicks);
        }

        [TestMethod]
        public void Stopwatch_Clock()
        {
            var c = new VirtualTimeClock();

            var sw = StopwatchFactory.FromClock(c).Create();

            Assert.IsFalse(sw.IsRunning);

            sw.Start();
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(0, sw.ElapsedTicks);

            c.Now += 1000;
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(1000, sw.ElapsedTicks);

            sw.Stop();

            c.Now += 2000;
            Assert.IsFalse(sw.IsRunning);
            Assert.AreEqual(1000, sw.ElapsedTicks);

            sw.Start();
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(1000, sw.ElapsedTicks);

            c.Now += 3000;
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(4000, sw.ElapsedTicks);

            sw.Reset();
            Assert.IsFalse(sw.IsRunning);
            Assert.AreEqual(0, sw.ElapsedTicks);

            sw.Start();
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(0, sw.ElapsedTicks);

            c.Now += 5000;
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(5000, sw.ElapsedTicks);

            sw.Restart();
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(0, sw.ElapsedTicks);

            c.Now += 1500;
            Assert.IsTrue(sw.IsRunning);
            Assert.AreEqual(1500, sw.ElapsedTicks);

            Assert.AreEqual(1500, sw.Elapsed.Ticks);
            Assert.AreEqual(0, sw.ElapsedMilliseconds); // not a double!

            c.Now += TimeSpan.FromMilliseconds(1).Ticks;
            Assert.AreEqual(1, sw.ElapsedMilliseconds); // not a double!
        }

        [TestMethod]
        public void Stopwatch_HighResolution()
        {
            var c = new VirtualTimeClock();

            var sw = new MyStopwatch(c);

            sw.Start();

            c.Now += 1000;

            Assert.AreEqual(1000, sw.ElapsedTicks);
            Assert.AreEqual(2000, sw.Elapsed.Ticks);
        }

        private sealed class MyStopwatch : StopwatchBase
        {
            private readonly IClock _clock;

            public MyStopwatch(IClock clock) => _clock = clock;

            public override long GetTimestamp() => _clock.Now;

            protected override bool IsHighResolution => true;

            protected override double TickFrequency => 2.0;
        }
    }
}
