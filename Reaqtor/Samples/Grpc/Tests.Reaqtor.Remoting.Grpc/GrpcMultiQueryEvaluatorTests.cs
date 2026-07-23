// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Remoting.QueryCoordinator;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 6: QC -> QE placement via IQueryEvaluatorSelector (plan §4.6). The ported QueryCoordinatorServiceProvider
// no longer hard-codes _queryEvaluators.First(); it routes each create/delete through the selector keyed on the
// entity URI, and QueryCoordinatorServiceConnection.Start picks FirstQueryEvaluatorSelector for a single evaluator
// (preserving the archived behaviour — and the reason the existing 13 end-to-end tests are unaffected) or
// ConsistentHashQueryEvaluatorSelector for ≥2 evaluators. These tests exercise the real selector logic that drives
// multi-QE distribution.
//
[TestClass]
public class GrpcMultiQueryEvaluatorTests
{
    [TestMethod]
    public void First_Selector_Always_Picks_The_Single_Evaluator()
    {
        var selector = FirstQueryEvaluatorSelector.Instance;

        for (var count = 1; count <= 3; count++)
        {
            for (var i = 0; i < 10; i++)
            {
                var uri = new Uri("reactor://test/sub/" + i.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(0, selector.SelectIndex(count, uri));
            }
        }
    }

    [TestMethod]
    public void ConsistentHash_Selector_Is_In_Range_Stable_And_Distributes()
    {
        var selector = ConsistentHashQueryEvaluatorSelector.Instance;

        var uris = Enumerable.Range(0, 200)
            .Select(i => new Uri("reactor://test/sub/" + i.ToString(CultureInfo.InvariantCulture)))
            .ToList();

        foreach (var evaluatorCount in new[] { 2, 3, 5 })
        {
            var used = new HashSet<int>();
            foreach (var uri in uris)
            {
                var index = selector.SelectIndex(evaluatorCount, uri);

                // In range.
                Assert.IsTrue(index >= 0 && index < evaluatorCount, FormattableString.Invariant($"index {index} out of range for count {evaluatorCount}"));

                // Stable: the same URI always resolves to the same evaluator (so create + later delete co-locate).
                Assert.AreEqual(index, selector.SelectIndex(evaluatorCount, uri), "selection must be deterministic in the URI");
                Assert.AreEqual(index, selector.SelectIndex(evaluatorCount, new Uri(uri.OriginalString)), "selection must depend only on the URI value");

                used.Add(index);
            }

            // Distributes: every evaluator gets at least one entity across the 200 URIs.
            Assert.AreEqual(evaluatorCount, used.Count, FormattableString.Invariant($"all {evaluatorCount} evaluators should receive entities; used {used.Count}"));
        }
    }

    [TestMethod]
    public void Selectors_Validate_Arguments()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => FirstQueryEvaluatorSelector.Instance.SelectIndex(0, new Uri("reactor://x")));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => ConsistentHashQueryEvaluatorSelector.Instance.SelectIndex(0, new Uri("reactor://x")));
        Assert.ThrowsExactly<ArgumentNullException>(() => ConsistentHashQueryEvaluatorSelector.Instance.SelectIndex(2, null));
    }
}
