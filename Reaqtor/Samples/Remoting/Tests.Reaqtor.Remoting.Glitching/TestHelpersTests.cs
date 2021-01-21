// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Reaqtor.Remoting.Glitching
{
    [TestClass]
    public class TestHelpersTests
    {
        [TestMethod]
        public void Powerset_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((IEnumerable<int>)null).Powerset());
        }

        [TestMethod]
        public void Choose_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TestHelpers.Choose<int>(null, 1).ToList());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => (new int[] { 1, 2, 3 }).Choose(4).ToList());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => (new int[] { 1, 2, 3 }).Choose(-1).ToList());
        }

        [TestMethod]
        public void Powerset_RegressionTest()
        {
            var set = new[] { "a", "b", "c" };
            var keys = new[] { "", "a", "b", "c", "ab", "ac", "bc", "abc" };
            var results = keys.ToDictionary(s => s, s => false);
            var powerset = set.Powerset();

            foreach (var ss in powerset)
            {
                var key = string.Join("", ss);
                Assert.IsTrue(results.ContainsKey(key));
                results[key] = true;
            }

            Assert.IsTrue(results.Values.All(b => b));
        }

        [TestMethod]
        public void Choose_RegressionTest()
        {
            var set = new[] { "a", "b", "c", "d" };
            var keys = new[]
            {
                new[] { "" },
                new[] { "a", "b", "c", "d" },
                new[] { "ab", "ac", "ad", "bc", "bd", "cd" },
                new[] { "abc", "abd", "acd", "bcd" },
                new[] { "abcd" },
            };

            for (var i = 0; i <= set.Length; ++i)
            {
                var cs = set.Choose(i);
                var results = keys[i].ToDictionary(k => k, _ => false);
                foreach (var c in cs)
                {
                    var key = string.Join("", c);
                    Assert.IsTrue(results.ContainsKey(key));
                    results[key] = true;
                }
                Assert.IsTrue(results.Values.All(b => b));
            }
        }
    }
}
