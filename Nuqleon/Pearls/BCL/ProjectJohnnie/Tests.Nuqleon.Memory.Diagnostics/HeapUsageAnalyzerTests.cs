// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory.Diagnostics;

namespace Tests
{
    [TestClass]
    public class HeapUsageAnalyzerTests
    {
        [TestMethod]
        public void HeapUsageAnalyzer_Basics()
        {
            var a = new HeapUsageAnalyzer();

            var name = "Bart";

            var anon1 = new { bar = "bar" };
            var anon2 = new { now = DateTime.Now };
            var arr1 = new[] { "foo", "baz" };
            var arr2 = new[] { "qux", "odd" };

            var objs1 = new object[] { anon1, new KeyValuePair<string, int>(name, 21), arr1 };
            var objs2 = new object[] { anon2, new Tuple<string, int>(name, 21), arr2 };
            var objs3 = new object[] { new object() };

            Assert.ThrowsException<ArgumentNullException>(() => a.AddPartition(null, objs1));
            Assert.ThrowsException<ArgumentNullException>(() => a.AddPartition("foo", null));

            var p1 = a.AddPartition("Part1", objs1);
            var p2 = a.AddPartition("Part2", objs2);
            var p3 = a.AddPartition("Part3", objs3);

            Assert.ThrowsException<ArgumentNullException>(() => a.RemovePartition(null));

            a.RemovePartition("Part3");

            Assert.ThrowsException<ArgumentNullException>(() => a.Analyze(null));

            var res = a.Analyze(new HeapAnalysisOptions { ComputeSharedHeap = true, DegreeOfParallelism = 0 });

            var r1 = res.Reports[p1].Objects;
            Assert.IsTrue(r1.SetEquals(new[] { objs1[0], objs1[1], objs1[2], anon1.bar, arr1[0], arr1[1] }));

            var r2 = res.Reports[p2].Objects;
            Assert.IsTrue(r2.SetEquals(new[] { objs2[0], objs2[1], objs2[2], arr2[0], arr2[1] }));

            var sh = res.Shared.Objects;
            Assert.IsTrue(sh.SetEquals(new[] { name }));

            var byGen1 = res.Reports[p1].SplitByGeneration();
            Assert.AreEqual(GC.MaxGeneration + 1, byGen1.Length);
            Assert.IsTrue(r1.SetEquals(byGen1.SelectMany(g => g.Objects)));

            var byGen2 = res.Reports[p2].SplitByGeneration();
            Assert.AreEqual(GC.MaxGeneration + 1, byGen2.Length);
            Assert.IsTrue(r2.SetEquals(byGen2.SelectMany(g => g.Objects)));

            var stats1 = res.Reports[p1].GetStats();

            Assert.AreEqual(4, stats1.InstanceCountPerType.Count); // anon, string, KeyValuePair<string, int>, string[]
            Assert.AreEqual(1, stats1.InstanceCountPerType[anon1.GetType()]);
            Assert.AreEqual(3, stats1.InstanceCountPerType[typeof(string)]);
            Assert.AreEqual(1, stats1.InstanceCountPerType[typeof(KeyValuePair<string, int>)]);
            Assert.AreEqual(1, stats1.InstanceCountPerType[typeof(string[])]);

            Assert.AreEqual(1, stats1.BoxedValueCount); // KeyValuePair<string, int>
            Assert.AreEqual(3, stats1.StringCount); // "bar", "foo", "baz"
            Assert.AreEqual(9, stats1.TotalStringCharacterCount); // "bar", "foo", "baz"

            Assert.IsFalse(string.IsNullOrEmpty(stats1.ToString()));

            var stats2 = res.Reports[p2].GetStats();

            Assert.AreEqual(4, stats2.InstanceCountPerType.Count); // anon, string, KeyValuePair<string, int>, string[]
            Assert.AreEqual(1, stats2.InstanceCountPerType[anon2.GetType()]);
            Assert.AreEqual(2, stats2.InstanceCountPerType[typeof(string)]);
            Assert.AreEqual(1, stats2.InstanceCountPerType[typeof(Tuple<string, int>)]);
            Assert.AreEqual(1, stats2.InstanceCountPerType[typeof(string[])]);

            Assert.AreEqual(0, stats2.BoxedValueCount);
            Assert.AreEqual(2, stats2.StringCount); // "qux", "odd"
            Assert.AreEqual(6, stats2.TotalStringCharacterCount); // "qux", "odd"

            Assert.IsFalse(string.IsNullOrEmpty(stats2.ToString()));

            var statsS = res.Shared.GetStats();

            Assert.AreEqual(1, statsS.InstanceCountPerType.Count); // string
            Assert.AreEqual(1, statsS.InstanceCountPerType[typeof(string)]);

            Assert.AreEqual(0, statsS.BoxedValueCount);
            Assert.AreEqual(1, statsS.StringCount);
            Assert.AreEqual(name.Length, statsS.TotalStringCharacterCount);

            Assert.IsFalse(string.IsNullOrEmpty(statsS.ToString()));

            var cln = res.Clone();

            Assert.AreNotSame(res, cln);
            Assert.AreNotSame(res.Shared, cln.Shared);
            Assert.AreNotSame(res.Reports, cln.Reports);
        }
    }
}
