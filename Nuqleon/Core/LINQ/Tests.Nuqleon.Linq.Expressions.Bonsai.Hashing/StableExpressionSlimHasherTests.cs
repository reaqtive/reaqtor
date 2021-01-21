// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Tests.System.Linq.Expressions.Bonsai.Hashing
{
    [TestClass]
    public class StableExpressionSlimHasherTests
    {
#if DEBUG
        private static readonly int minimumIterationCount = 8;
        private static readonly TimeSpan maximumTestDuration = TimeSpan.FromSeconds(1);
#else
        private static readonly int minimumIterationCount = 8;
        private static readonly TimeSpan maximumTestDuration = TimeSpan.FromSeconds(30);
#endif

        [TestMethod]
        public void GetStableHashCode_Unique()
        {
            var set = new HashSet<int>();

            foreach (var expr in TestCases.GetExpressionsUnique())
            {
                var h = TestCases.GetHashCode(expr);

                Assert.IsTrue(set.Add(h), expr?.ToString() ?? "null");
            }
        }

        [TestMethod]
        public void GetStableHashCode_Equivalent()
        {
            var set = new HashSet<int>();

            foreach (var exprs in TestCases.GetExpressionsEquivalent())
            {
                var n = exprs.Select(TestCases.GetHashCode).Distinct().Count();
                Assert.AreEqual(1, n, exprs.First().ToString());
            }
        }

        [TestMethod]
        public void GetStableHashCode_All_StableLocally()
        {
            RepeatTest(() =>
            {
                var hs1 = TestCases.GetHashes();
                var hs2 = TestCases.GetHashes();

                AssertEqual(hs1, hs2);
            });
        }

#if FALSE
        [TestMethod]
        public void GetStableHashCode_All_CrossProc()
        {
            var local = TestCases.GetHashes().ToArray();

            RepeatTest(() =>
            {
                var hs1 = TestCases.GetHashes();
                var hs2 = GetHashesOutOfProc();

                AssertEqual(hs1, hs2);
            });
        }

        private static IEnumerable<int> GetHashesOutOfProc()
        {
            var location = new Uri(typeof(Program).Assembly.CodeBase).LocalPath;

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo(Path.GetFileName(location))
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(location),
                }
            };

            proc.Start();

            var res = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();

            return res.Split('\r', '\n').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();
        }
#endif

        private static void RepeatTest(Action test)
        {
            var sw = Stopwatch.StartNew();

            for (var i = 0; i < minimumIterationCount || sw.Elapsed < maximumTestDuration; i++)
            {
                test();
            }
        }

        private static void AssertEqual(IEnumerable<int> h1, IEnumerable<int> h2)
        {
            Assert.IsTrue(h1.SequenceEqual(h2));
        }
    }
}
