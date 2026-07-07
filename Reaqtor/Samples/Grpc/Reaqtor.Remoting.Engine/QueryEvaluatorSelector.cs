// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.QueryCoordinator
{
    //
    // QC -> QE placement (plan §4.6 / Milestone 6). The archived QueryCoordinatorServiceProvider hard-coded
    // _queryEvaluators.First() for every create/delete. This is the named extension point that replaces it: given the
    // number of registered query evaluators and the entity URI being placed, pick which evaluator owns it. The choice
    // is keyed on the entity URI (which the QC already has as a parameter — subscriptionUri/streamUri), so a create
    // and its later delete resolve to the SAME evaluator. The default preserves the single-QE behaviour exactly; a
    // multi-QE deployment selects a distributing strategy.
    //
    /// <summary>Selects which query evaluator owns a given entity URI (plan §4.6).</summary>
    public interface IQueryEvaluatorSelector
    {
        /// <summary>
        /// Returns the index in [0, <paramref name="evaluatorCount"/>) of the evaluator that owns
        /// <paramref name="entityUri"/>. Must be deterministic in the URI so a create and its later delete agree.
        /// </summary>
        int SelectIndex(int evaluatorCount, Uri entityUri);
    }

    /// <summary>
    /// The default selector — always the first evaluator (the archived <c>_queryEvaluators.First()</c> behaviour).
    /// With a single registered evaluator this is the only valid choice.
    /// </summary>
    public sealed class FirstQueryEvaluatorSelector : IQueryEvaluatorSelector
    {
        /// <summary>A shared instance.</summary>
        public static readonly FirstQueryEvaluatorSelector Instance = new();

        public int SelectIndex(int evaluatorCount, Uri entityUri)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(evaluatorCount);

            return 0;
        }
    }

    /// <summary>
    /// Distributes entities across ≥2 evaluators by a deterministic hash of the entity URI (metadata-driven
    /// placement, plan §4.6). Deterministic across processes (a fixed FNV-1a over the URI string, not the
    /// per-process-randomized <see cref="string.GetHashCode()"/>), so the same URI always lands on the same
    /// evaluator — co-locating a subscription's create and delete — and the load spreads evenly across evaluators.
    /// </summary>
    public sealed class ConsistentHashQueryEvaluatorSelector : IQueryEvaluatorSelector
    {
        /// <summary>A shared instance.</summary>
        public static readonly ConsistentHashQueryEvaluatorSelector Instance = new();

        public int SelectIndex(int evaluatorCount, Uri entityUri)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(evaluatorCount);

            ArgumentNullException.ThrowIfNull(entityUri);

            // Hash the CANONICAL form (AbsoluteUri == the topic layer's ToCanonicalString), not OriginalString, so two
            // textually-different-but-equal URIs (escaping, default port, etc.) route to the same evaluator — keeping a
            // subscription's create and its later delete co-located regardless of how each URI was constructed.
            return (int)(Fnv1a(entityUri.AbsoluteUri) % (uint)evaluatorCount);
        }

        // FNV-1a (32-bit) — a small, deterministic, dependency-free string hash. Unlike string.GetHashCode() it is
        // stable across processes, which matters once create and delete for the same URI can be served by different
        // coordinator instances.
        private static uint Fnv1a(string value)
        {
            const uint OffsetBasis = 2166136261;
            const uint Prime = 16777619;

            var hash = OffsetBasis;
            foreach (var c in value)
            {
                hash ^= c;
                hash *= Prime;
            }

            return hash;
        }
    }
}
