// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System;
using System.Memory;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CacheEntryTests
    {
        [TestMethod]
        public void MetricsValueCacheEntry_Simple()
        {
            var entry = new MetricsValueCacheEntry<int, int>(42, 43);

            Assert.AreEqual(42, entry.Key);
            Assert.AreEqual(43, entry.Value);

            Assert.AreEqual(0, entry.HitCount);
            Assert.AreEqual(0L, entry.CreationTime.Ticks);
            Assert.AreEqual(0L, entry.InvokeDuration.Ticks);
            Assert.AreEqual(0L, entry.LastAccessTime.Ticks);
            Assert.AreEqual(0L, entry.TotalDuration.Ticks);

            //
            // NB: AverageAccessTime and SpeedupFactor throw DivideByZeroException until values are
            //     initialized. Accesses to those properties is guaranteed to happen after the hit
            //     count has been incremented to 1, so this is not an issue.
            //

            Assert.ThrowsException<DivideByZeroException>(() => entry.AverageAccessTime);
            Assert.ThrowsException<DivideByZeroException>(() => entry.SpeedupFactor);

            entry.HitCount++;
            Assert.AreEqual(0L, entry.AverageAccessTime.Ticks);
            Assert.IsTrue(double.IsNaN(entry.SpeedupFactor)); // can only happen if access time is zero

            entry.TotalDuration = TimeSpan.FromTicks(27182818284590451L);
            Assert.AreEqual(27182818284590451L, entry.AverageAccessTime.Ticks);
            Assert.AreEqual(0.0, entry.SpeedupFactor);

            entry.InvokeDuration = TimeSpan.FromTicks(31415926535897931L);
            Assert.AreEqual(27182818284590451L, entry.AverageAccessTime.Ticks);
            Assert.AreEqual((double)31415926535897931L / 27182818284590451L, entry.SpeedupFactor);

            entry.HitCount++;
            Assert.AreEqual(27182818284590451L / 2, entry.AverageAccessTime.Ticks);
            Assert.AreEqual((double)31415926535897931L / (27182818284590451L / 2), entry.SpeedupFactor);
        }

        [TestMethod]
        public void MetricsErrorCacheEntry_Simple()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                var entry = new MetricsErrorCacheEntry<int, int>(42, ex);

                Assert.AreEqual(42, entry.Key);
                Assert.AreSame(ex, entry.Exception);

                var stack = ex.StackTrace;
                try
                {
                    _ = entry.Value;
                }
                catch (Exception ex2)
                {
                    Assert.AreSame(ex, ex2);
                    Assert.AreEqual(stack, ex2.StackTrace.Substring(0, stack.Length));
                }

                Assert.AreEqual(0, entry.HitCount);
                Assert.AreEqual(0L, entry.CreationTime.Ticks);
                Assert.AreEqual(0L, entry.InvokeDuration.Ticks);
                Assert.AreEqual(0L, entry.LastAccessTime.Ticks);
                Assert.AreEqual(0L, entry.TotalDuration.Ticks);

                //
                // NB: AverageAccessTime and SpeedupFactor throw DivideByZeroException until values are
                //     initialized. Accesses to those properties is guaranteed to happen after the hit
                //     count has been incremented to 1, so this is not an issue.
                //

                Assert.ThrowsException<DivideByZeroException>(() => entry.AverageAccessTime);
                Assert.ThrowsException<DivideByZeroException>(() => entry.SpeedupFactor);

                entry.HitCount++;
                Assert.AreEqual(0L, entry.AverageAccessTime.Ticks);
                Assert.IsTrue(double.IsNaN(entry.SpeedupFactor)); // can only happen if access time is zero

                entry.TotalDuration = TimeSpan.FromTicks(27182818284590451L);
                Assert.AreEqual(27182818284590451L, entry.AverageAccessTime.Ticks);
                Assert.AreEqual(0.0, entry.SpeedupFactor);

                entry.InvokeDuration = TimeSpan.FromTicks(31415926535897931L);
                Assert.AreEqual(27182818284590451L, entry.AverageAccessTime.Ticks);
                Assert.AreEqual((double)31415926535897931L / 27182818284590451L, entry.SpeedupFactor);

                entry.HitCount++;
                Assert.AreEqual(27182818284590451L / 2, entry.AverageAccessTime.Ticks);
                Assert.AreEqual((double)31415926535897931L / (27182818284590451L / 2), entry.SpeedupFactor);

                var sb = new StringBuilder();
                entry.ToDebugView(sb, "");
                Assert.IsTrue(sb.ToString().Contains("="));
            }
        }

        [TestMethod]
        public void EntityMetrics_Simple()
        {
            var entry = "".GetMetrics();

            Assert.AreEqual(0, entry.HitCount);
            Assert.AreEqual(0L, entry.CreationTime.Ticks);
            Assert.AreEqual(0L, entry.InvokeDuration.Ticks);
            Assert.AreEqual(0L, entry.LastAccessTime.Ticks);
            Assert.AreEqual(0L, entry.TotalDuration.Ticks);

            //
            // NB: AverageAccessTime and SpeedupFactor throw DivideByZeroException until values are
            //     initialized. Accesses to those properties is guaranteed to happen after the hit
            //     count has been incremented to 1, so this is not an issue.
            //

            Assert.ThrowsException<DivideByZeroException>(() => entry.AverageAccessTime);
            Assert.ThrowsException<DivideByZeroException>(() => entry.SpeedupFactor);

            entry.HitCount++;
            Assert.AreEqual(0L, entry.AverageAccessTime.Ticks);
            Assert.IsTrue(double.IsNaN(entry.SpeedupFactor)); // can only happen if access time is zero

            entry.TotalDuration = TimeSpan.FromTicks(31415926535897931L);
            Assert.AreEqual(31415926535897931L, entry.AverageAccessTime.Ticks);
            Assert.AreEqual(0.0, entry.SpeedupFactor);

            entry.InvokeDuration = TimeSpan.FromTicks(27182818284590451L);
            Assert.AreEqual(31415926535897931L, entry.AverageAccessTime.Ticks);
            Assert.AreEqual((double)27182818284590451L / 31415926535897931L, entry.SpeedupFactor);

            var sb = new StringBuilder();
            entry.ToDebugView(sb, "");
            Assert.IsTrue(sb.ToString().Contains("=") && sb.ToString().Contains("DEGRADATION"));

            entry.HitCount++;
            Assert.AreEqual(31415926535897931L / 2, entry.AverageAccessTime.Ticks);
            Assert.AreEqual((double)27182818284590451L / (31415926535897931L / 2), entry.SpeedupFactor);
        }
    }
}
