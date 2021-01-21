// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ProgressTests
    {
        #region Catch

        [TestMethod]
        public void Progress_Catch_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Catch<int>(default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Catch<int, Exception>(default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Catch<int, Exception>(default, _ => false));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Catch<int, Exception>(p, default));
        }

        [TestMethod]
        public void Progress_Catch_All()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = 100 / x; }).Catch();

            p.Report(1);
            Assert.AreEqual(100, res);

            p.Report(4);
            Assert.AreEqual(25, res);

            p.Report(0);
            Assert.AreEqual(25, res);
        }

        [TestMethod]
        public void Progress_Catch_ByType()
        {
            var res = default(int);

            var b = false;

            var p = Progress.Create<int>(x => { res = 100 / x; if (b) throw new InvalidOperationException(); }).Catch<int, DivideByZeroException>();

            p.Report(1);
            Assert.AreEqual(100, res);

            p.Report(4);
            Assert.AreEqual(25, res);

            p.Report(0);
            Assert.AreEqual(25, res);

            b = true;
            Assert.ThrowsException<InvalidOperationException>(() => p.Report(4));

            b = false;
            p.Report(2);
            Assert.AreEqual(50, res);
        }

        [TestMethod]
        public void Progress_Catch_Filtered_Handled()
        {
            var res = default(int);

            var b = false;

            var p = Progress.Create<int>(x => { res = 100 / x; }).Catch<int, DivideByZeroException>(ex => b);

            p.Report(1);
            Assert.AreEqual(100, res);

            p.Report(4);
            Assert.AreEqual(25, res);

            Assert.ThrowsException<DivideByZeroException>(() => p.Report(0));

            b = true;
            p.Report(0);
            Assert.AreEqual(25, res);
        }

        #endregion

        #region Create

        [TestMethod]
        public void Progress_Create_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Create<int>(default));
        }

        [TestMethod]
        public void Progress_Create_Simple()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; });

            p.Report(1);
            Assert.AreEqual(1, res);

            p.Report(7);
            Assert.AreEqual(7, res);
        }

        [TestMethod]
        public void Progress_Create_Throws()
        {
            var ex = new DivideByZeroException();

            var p = Progress.Create<int>(x => { throw ex; });

            Assert.ThrowsException<DivideByZeroException>(() => p.Report(1));
        }

        #endregion

        #region DistinctUntilChanged

        [TestMethod]
        public void Progress_DistinctUntilChanged_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.DistinctUntilChanged(default(IProgress<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.DistinctUntilChanged(default, EqualityComparer<int>.Default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.DistinctUntilChanged(p, default));
        }

        [TestMethod]
        public void Progress_DistinctUntilChanged_NoComparer()
        {
            var i = 0;
            var res = default(int);

            var p = Progress.Create<int>(x => { i++; res = x; }).DistinctUntilChanged();

            p.Report(8);
            Assert.AreEqual(1, i);
            Assert.AreEqual(8, res);

            p.Report(3);
            Assert.AreEqual(2, i);
            Assert.AreEqual(3, res);

            p.Report(3);
            Assert.AreEqual(2, i);
            Assert.AreEqual(3, res);

            p.Report(5);
            Assert.AreEqual(3, i);
            Assert.AreEqual(5, res);

            p.Report(8);
            Assert.AreEqual(4, i);
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        public void Progress_DistinctUntilChanged_Comparer()
        {
            var i = 0;
            var res = default(string);

            var p = Progress.Create<string>(x => { i++; res = x; }).DistinctUntilChanged(StringComparer.InvariantCultureIgnoreCase);

            p.Report("foo");
            Assert.AreEqual(1, i);
            Assert.AreEqual("foo", res);

            p.Report("bar");
            Assert.AreEqual(2, i);
            Assert.AreEqual("bar", res);

            p.Report("BAR");
            Assert.AreEqual(2, i);
            Assert.AreEqual("bar", res);

            p.Report("qux");
            Assert.AreEqual(3, i);
            Assert.AreEqual("qux", res);

            p.Report("FOO");
            Assert.AreEqual(4, i);
            Assert.AreEqual("FOO", res);
        }

        #endregion

        #region Monotonic

        [TestMethod]
        public void Progress_Monotonic_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Monotonic(default(IProgress<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Monotonic(default, Comparer<int>.Default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Monotonic(p, default));
        }

        [TestMethod]
        public void Progress_Monotonic_NoComparer()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Monotonic();

            p.Report(2);
            Assert.AreEqual(2, res);

            p.Report(7);
            Assert.AreEqual(7, res);

            p.Report(5);
            Assert.AreEqual(7, res);

            p.Report(8);
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        public void Progress_Monotonic_Comparer()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Monotonic(new ReverseComparer<int>(Comparer<int>.Default));

            p.Report(8);
            Assert.AreEqual(8, res);

            p.Report(5);
            Assert.AreEqual(5, res);

            p.Report(7);
            Assert.AreEqual(5, res);

            p.Report(2);
            Assert.AreEqual(2, res);
        }

        private sealed class ReverseComparer<T> : IComparer<T>
        {
            private readonly IComparer<T> _comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return -_comparer.Compare(x, y);
            }
        }

        #endregion

        #region Range

        [TestMethod]
        public void Progress_Range_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Range<int>(default, 0, 100));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Range<int>(default, 0, 100, Comparer<int>.Default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Range<int>(p, 0, 100, default));
        }

        [TestMethod]
        public void Progress_Range_Simple()
        {
            var res = default(int?);

            var p = Progress.Create<int>(x => { res = x; }).Range(0, 100);

            p.Report(-10);
            Assert.AreEqual(default, res);

            p.Report(-1);
            Assert.AreEqual(default, res);

            p.Report(0);
            Assert.AreEqual(0, res);

            p.Report(1);
            Assert.AreEqual(1, res);

            p.Report(99);
            Assert.AreEqual(99, res);

            p.Report(100);
            Assert.AreEqual(100, res);

            p.Report(101);
            Assert.AreEqual(100, res);
        }

        [TestMethod]
        public void Progress_Range_Comparer()
        {
            var res = default(int?);

            var h = false;

            var p = Progress.Create<int>(x => { res = x; }).Range(0, 100, Comparer<int>.Create((x, y) => { h = true; return x < y ? -1 : (x == y ? 0 : 1); }));

            p.Report(-10);
            Assert.AreEqual(default, res);

            p.Report(-1);
            Assert.AreEqual(default, res);

            p.Report(0);
            Assert.AreEqual(0, res);

            p.Report(1);
            Assert.AreEqual(1, res);

            p.Report(99);
            Assert.AreEqual(99, res);

            p.Report(100);
            Assert.AreEqual(100, res);

            p.Report(101);
            Assert.AreEqual(100, res);

            Assert.IsTrue(h);
        }

        #endregion

        #region Select

        [TestMethod]
        public void Progress_Select_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Select<int, int>(default, x => x));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Select<int, int>(p, default));
        }

        [TestMethod]
        public void Progress_Select_Simple()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Select((int x) => x * 2);

            p.Report(1);
            Assert.AreEqual(2, res);

            p.Report(4);
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        public void Progress_Select_Throws()
        {
            var p = Progress.Create<int>(x => { }).Select((int x) => 100 / x);

            Assert.ThrowsException<DivideByZeroException>(() => p.Report(0));
        }

        #endregion

        #region Split

        [TestMethod]
        public void Progress_Split_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Split<int, int, int>(default, (x, y) => x + 1));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Split<int, int, int>(p, default));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Split<int>(default, 1, xs => xs.Sum()));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Progress.Split<int>(p, 0, xs => xs.Sum()));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Progress.Split<int>(p, -1, xs => xs.Sum()));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Split<int>(p, 1, default));
        }

        [TestMethod]
        public void Progress_Split_Binary()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Split<int, int, int>((x, y) => x + y);

            var p1 = p.Item1;
            var p2 = p.Item2;

            p1.Report(1);
            Assert.AreEqual(1 + 0, res);

            p1.Report(2);
            Assert.AreEqual(2 + 0, res);

            p2.Report(3);
            Assert.AreEqual(2 + 3, res);

            p2.Report(4);
            Assert.AreEqual(2 + 4, res);

            p1.Report(5);
            Assert.AreEqual(5 + 4, res);
        }

        [TestMethod]
        public void Progress_Split_Nary()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Split(3, xs => xs.Sum());

            var p1 = p[0];
            var p2 = p[1];
            var p3 = p[2];

            p1.Report(1);
            Assert.AreEqual(1 + 0 + 0, res);

            p1.Report(2);
            Assert.AreEqual(2 + 0 + 0, res);

            p2.Report(3);
            Assert.AreEqual(2 + 3 + 0, res);

            p2.Report(4);
            Assert.AreEqual(2 + 4 + 0, res);

            p1.Report(5);
            Assert.AreEqual(5 + 4 + 0, res);

            p3.Report(6);
            Assert.AreEqual(5 + 4 + 6, res);

            p2.Report(7);
            Assert.AreEqual(5 + 7 + 6, res);
        }

        #endregion

        #region SplitWeight

        [TestMethod]
        public void Progress_SplitWeight_ArgumentChecking()
        {
            var p = new Progress<int>();
            var f = new Func<int, int, int>((x, y) => x + y);
            var w = new int[2];

            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(default, f, f, f, f, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(p, default, f, f, f, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(p, f, default, f, f, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(p, f, f, default, f, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(p, f, f, f, default, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight<int, int>(p, f, f, f, f, default));

            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight(default, w));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.SplitWeight(p, default));
        }

        [TestMethod]
        public void Progress_SplitWeight_None()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).SplitWeight();

            Assert.AreEqual(0, p.Length);
        }

        [TestMethod]
        public void Progress_SplitWeight_Percentage()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).SplitWeight(20, 30, 40, 10);

            Assert.AreEqual(4, p.Length);
            var p1 = p[0];
            var p2 = p[1];
            var p3 = p[2];
            var p4 = p[3];

            p1.Report(50);
            Assert.AreEqual(10 + 0 + 0 + 0, res);

            p2.Report(50);
            Assert.AreEqual(10 + 15 + 0 + 0, res);

            p3.Report(25);
            Assert.AreEqual(10 + 15 + 10 + 0, res);

            p4.Report(100);
            Assert.AreEqual(10 + 15 + 10 + 10, res);

            p1.Report(100);
            Assert.AreEqual(20 + 15 + 10 + 10, res);

            p2.Report(100);
            Assert.AreEqual(20 + 30 + 10 + 10, res);

            p3.Report(75);
            Assert.AreEqual(20 + 30 + 30 + 10, res);

            p3.Report(100);
            Assert.AreEqual(20 + 30 + 40 + 10, res);
        }

        #endregion

        #region Throttle

        [TestMethod]
        public void Progress_Throttle_ArgumentChecking()
        {
            var p = new Progress<int>();
            var t = TimeSpan.FromSeconds(1);
            var s = new Stopwatch().ToStopwatch();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Throttle<int>(default, t));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Progress.Throttle<int>(p, -t));

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Throttle<int>(default, t, s));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Progress.Throttle<int>(p, -t, s));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Throttle<int>(p, t, default));
        }

        [TestMethod]
        public void Progress_Throttle_Simple()
        {
            var sw = new MyStopwatch();

            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Throttle(TimeSpan.FromSeconds(1), sw);

            p.Report(2);
            Assert.AreEqual(2, res);

            sw.Elapsed += TimeSpan.FromMilliseconds(350);

            p.Report(3);
            Assert.AreEqual(2, res);

            sw.Elapsed += TimeSpan.FromMilliseconds(350);

            p.Report(4);
            Assert.AreEqual(2, res);

            sw.Elapsed += TimeSpan.FromMilliseconds(350);

            p.Report(5);
            Assert.AreEqual(5, res);

            sw.Elapsed += TimeSpan.FromMilliseconds(750);

            p.Report(6);
            Assert.AreEqual(5, res);

            sw.Elapsed += TimeSpan.FromMilliseconds(260);

            p.Report(7);
            Assert.AreEqual(7, res);
        }

        [TestMethod]
        public void Progress_Throttle_RealClock_Long()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Throttle(TimeSpan.FromMinutes(1));

            p.Report(2);
            Assert.AreEqual(2, res);

            p.Report(3);
            Assert.AreEqual(2, res);

            p.Report(4);
            Assert.AreEqual(2, res);
        }

        [TestMethod]
        public void Progress_Throttle_RealClock_Short()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Throttle(TimeSpan.FromMilliseconds(50));

            p.Report(2);
            Assert.AreEqual(2, res);

            Thread.Sleep(200);

            p.Report(3);
            Assert.AreEqual(3, res);
        }

        [TestMethod]
        public void Stopwatch_Wrapper()
        {
            var sw = new Stopwatch();
            var sz = sw.ToStopwatch();

            sw.Stop();
            Assert.AreEqual(sw.Elapsed, sz.Elapsed);

            sz.Start();

            Thread.Sleep(50);

            sw.Stop();
            Assert.AreEqual(sw.Elapsed, sz.Elapsed);

            sz.Restart();

            Thread.Sleep(50);

            sw.Stop();
            Assert.AreEqual(sw.Elapsed, sz.Elapsed);
        }

        private sealed class MyStopwatch : IStopwatch
        {
            public TimeSpan Elapsed
            {
                get;
                set;
            }

            public void Restart()
            {
                Elapsed = TimeSpan.Zero;
            }

            public void Start()
            {
            }
        }

        #endregion

        #region ToPercentageIncrement

        [TestMethod]
        public void Progress_ToPercentageIncrement_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.ToPercentageIncrement(default, 123));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Progress.ToPercentageIncrement(p, -1));
        }

        [TestMethod]
        public void Progress_ToPercentageIncrement_Zero()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).ToPercentageIncrement(0);

            Assert.AreEqual(100, res);

            p();

            Assert.AreEqual(100, res);
        }

        [TestMethod]
        public void Progress_ToPercentageIncrement_Simple()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).ToPercentageIncrement(120);

            for (int i = 0; i < 30; i++)
                p();

            Assert.AreEqual(25, res);

            for (int i = 0; i < 30; i++)
                p();

            Assert.AreEqual(50, res);

            for (int i = 0; i < 60; i++)
                p();

            Assert.AreEqual(100, res);
        }

        #endregion

        #region Where

        [TestMethod]
        public void Progress_Where_ArgumentChecking()
        {
            var p = new Progress<int>();

            Assert.ThrowsException<ArgumentNullException>(() => Progress.Where<int>(default, x => true));
            Assert.ThrowsException<ArgumentNullException>(() => Progress.Where<int>(p, default));
        }

        [TestMethod]
        public void Progress_Where_Simple()
        {
            var res = default(int);

            var p = Progress.Create<int>(x => { res = x; }).Where(x => x % 2 == 0);

            p.Report(2);
            Assert.AreEqual(2, res);

            p.Report(3);
            Assert.AreEqual(2, res);

            p.Report(4);
            Assert.AreEqual(4, res);

            p.Report(5);
            Assert.AreEqual(4, res);
        }

        #endregion

        #region ToStopwatch

        [TestMethod]
        public void ToStopwatch_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StopwatchExtensions.ToStopwatch(null));
        }

        [TestMethod]
        public void ToStopwatch_Simple()
        {
            var sw = new Stopwatch();

            var it = sw.ToStopwatch();

            Assert.AreEqual(sw.Elapsed.Ticks, it.Elapsed.Ticks);

            it.Start();

            while (sw.ElapsedMilliseconds < 1)
                ;

            sw.Stop();

            Assert.AreEqual(sw.Elapsed.Ticks, it.Elapsed.Ticks);
        }

        #endregion
    }
}
