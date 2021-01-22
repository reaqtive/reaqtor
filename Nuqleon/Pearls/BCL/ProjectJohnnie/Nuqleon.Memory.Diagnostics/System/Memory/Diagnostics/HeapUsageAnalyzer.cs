// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Provides facilities to analyze the memory allocated on an object heap which can be split into partitions.
    /// </summary>
    public sealed class HeapUsageAnalyzer
    {
        //
        // CONSIDER: We could be more sophisticated here and introduce a hierarchy of partitions, allowing heaps to
        //           be divided into more fine-grained partitions, with aggregations across child partitions.
        //

        /// <summary>
        /// The partitions managed by the analyzer.
        /// </summary>
        private readonly Dictionary<string, HeapPartition> _partitions = new();

        /// <summary>
        /// Adds a heap partition to the analyzer.
        /// </summary>
        /// <param name="name">The name of the partition to add. This name should be unique within an analyzer instance.</param>
        /// <param name="roots">The set of roots that define the heap partition.</param>
        /// <returns>A new heap partition instance.</returns>
        public HeapPartition AddPartition(string name, params object[] roots)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (roots == null)
                throw new ArgumentNullException(nameof(roots));

            var p = new HeapPartition(name, roots);
            _partitions.Add(name, p);
            return p;
        }

        /// <summary>
        /// Removes a heap partition from the analyzer.
        /// </summary>
        /// <param name="name">The name of the partition to remove.</param>
        public void RemovePartition(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _partitions.Remove(name);
        }

        /// <summary>
        /// Performs an analysis of the heap.
        /// </summary>
        /// <param name="options">Options specifying the behavior of the analysis.</param>
        /// <returns>A heap usage analysis instance containing the results of the analysis.</returns>
        /// <remarks>
        /// Heap analysis is a CPU-intensive operation and can be parallellized using <see cref="HeapAnalysisOptions.DegreeOfParallelism"/>.
        /// In cases where the caller should not be blocked during the execution of the analysis, consider using <see cref="Task.Run(Action)"/>.
        /// See the <see cref="Analyze(HeapAnalysisOptions, CancellationToken)"/> overload to support early cancellation of the analysis.
        /// </remarks>
        public HeapUsageAnalysis Analyze(HeapAnalysisOptions options) => Analyze(options, CancellationToken.None);

        /// <summary>
        /// Performs an analysis of the heap with cancellation support.
        /// </summary>
        /// <param name="options">Options specifying the behavior of the analysis.</param>
        /// <param name="token">A token to observe to allow for cancellation.</param>
        /// <returns>A heap usage analysis instance containing the results of the analysis.</returns>
        /// <remarks>
        /// Heap analysis is a CPU-intensive operation and can be parallellized using <see cref="HeapAnalysisOptions.DegreeOfParallelism"/>.
        /// In cases where the caller should not be blocked during the execution of the analysis, consider using <see cref="Task.Run(Action)"/>.
        /// Note that this method overload supports a <see cref="CancellationToken"/> to allow for early cancellation of the analysis.
        /// </remarks>
        public HeapUsageAnalysis Analyze(HeapAnalysisOptions options, CancellationToken token)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            //
            // Create a set of all roots which will be used to avoid traversing from one partition into another
            // during the analysis. See the logic in GetReport and how it passes a check against sibling roots
            // to the fence predicate passed to the Scan operation.
            //

            var allRoots = new ObjectSet(_partitions.Values.SelectMany(p => p.Roots));

            //
            // The dictionary that will contain the result of scanning each partition. In case the algorithm is
            // run in a parallel fashion, access to this dictionary should be synchronized.
            //

            var reports = new Dictionary<HeapPartition, HeapReport>(_partitions.Count);

#pragma warning disable 1587
            /// <summary>
            /// Helper method to get a heap report for a given heap partition.
            /// </summary>
            /// <param name="partition">The heap partition to analyze.</param>
            /// <param name="ct">The cancellation used to support early termination of the analysis.</param>
            /// <returns>A heap report the given heap partition.</returns>
#pragma warning restore 1587
            HeapReport GetReport(HeapPartition partition, CancellationToken ct)
            {
                //
                // Calculate the roots of all sibling partition in order to avoid traversing into these paritions.
                //

                var allOtherRoots = new ObjectSet(allRoots);
                allOtherRoots.ExceptWith(partition.Roots);

                var fence = default(Func<object, bool>);

                if (ct.CanBeCanceled)
                {
                    //
                    // In case cancellation is requested during a scan, we can simply make the fence predicate
                    // return false, which will cause a prompt bail out. This avoids having to add more plumbing
                    // to wire down the cancellation token to Scan and Walk.
                    //

                    fence = o => !ct.IsCancellationRequested && !allOtherRoots.Contains(o);
                }
                else
                {
                    fence = o => !allOtherRoots.Contains(o);
                }

                var r = partition.Scan(fence);

                return new HeapReport(r);
            }

            //
            // Check if we need parallel execution and use Parallel.ForEach if so.
            //

            var dop = options.DegreeOfParallelism;

            if (dop > 1)
            {
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = dop, CancellationToken = token };

                Parallel.ForEach(_partitions.Values, parallelOptions, partition =>
                {
                    var report = GetReport(partition, token);

                    lock (reports)
                    {
                        reports.Add(partition, report);
                    }
                });
            }
            else
            {
                foreach (var partition in _partitions.Values)
                {
                    var report = GetReport(partition, token);

                    reports.Add(partition, report);
                }
            }

            //
            // NB: Right now, we simply perform the computation of the shared heap as a post-analysis task
            //     over here. However, given that the option is passed to the Analyze call, we can consider
            //     to perform the computation of the shared heap while executing the core scan, though it
            //     has some challenges (e.g. shared access to state for writing, undoing parallel execution
            //     benefits). One advantage of being able to separate the partition scan from calculating
            //     the shared heap is that users can calculate the ratio of private memory and all memory
            //     reachable from a given partition by getting stats for a partition prior to removing all
            //     shared objects from a partition.
            //

            var res = new HeapUsageAnalysis(reports);

            if (options.ComputeSharedHeap)
            {
                res.ComputeShared();
            }

            return res;
        }
    }
}
