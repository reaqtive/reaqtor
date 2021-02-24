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

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents the result of analyzing heap usage.
    /// </summary>
    public sealed class HeapUsageAnalysis
    {
        /// <summary>
        /// Creates a new heap usage analysis without a shared heap.
        /// </summary>
        /// <param name="reports">The reports for each heap partition.</param>
        public HeapUsageAnalysis(Dictionary<HeapPartition, HeapReport> reports)
        {
            Reports = reports ?? throw new ArgumentNullException(nameof(reports));
            Shared = new HeapReport();
        }

        /// <summary>
        /// Creates a new heap usage analysis with a shared heap.
        /// </summary>
        /// <param name="reports">The reports for each heap partition.</param>
        /// <param name="shared">The report for the shared heap.</param>
        public HeapUsageAnalysis(Dictionary<HeapPartition, HeapReport> reports, HeapReport shared)
        {
            Reports = reports ?? throw new ArgumentNullException(nameof(reports));
            Shared = shared ?? throw new ArgumentNullException(nameof(shared));
        }

        /// <summary>
        /// Gets a dictionary mapping heap partitions onto heap reports.
        /// </summary>
        public Dictionary<HeapPartition, HeapReport> Reports { get; }

        /// <summary>
        /// Gets a heap report for the objects shared across all partitions.
        /// </summary>
        public HeapReport Shared { get; }

        /// <summary>
        /// Performs a deep clone of the current analysis instance.
        /// </summary>
        /// <returns>A deep clone of the current analysis instance.</returns>
        public HeapUsageAnalysis Clone()
        {
            var reports = Reports.ToDictionary(kv => kv.Key, kv => kv.Value.Clone());
            var shared = Shared?.Clone();

            return new HeapUsageAnalysis(reports, shared);
        }

        /// <summary>
        /// Computes the shared objects reachable from the partitions in <see cref="Reports"/> and adds those to the
        /// <see cref="Shared"/> partition.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, this method mutates the state of the current instance and all <see cref="HeapReport"/>
        /// instances in <see cref="Reports"/> and <see cref="Shared"/>. It's recommended to obtain any heap usage stats
        /// from the current instance prior to running this method. If the analysis before and after splitting off the
        /// shared heap should remain available, consider calling <see cref="Clone"/>.
        /// </remarks>
        public void ComputeShared()
        {
            //
            // We keep a dictionary that maps objects onto the first heap partition that contains a reference to them.
            // If an object is not found in this map when analyzing a partition, it will be added to the map. If an
            // object already occurs in the map when analyzing a partition, this indicates that the object is shared
            // by at least two heap partitions. In this case, we substitute the value of the map entry for null, remove
            // the object from the original and current partition's private state, and move it to the shared partition.
            // When an entry with a null value is found, it indicates that the object has already been moved to the
            // shared partition, so we don't need to move it from a private heap to a shared heap anymore; we just need
            // to remove it fromt he current partition.
            //

            var map = new Dictionary<object, HeapPartition>(ReferenceEqualityComparer<object>.Instance); // PERF: Consider pooling?

            //
            // In order to avoid mutating a set while enumerating over it, we keep a list of objects to be removed from
            // a set. This list can be reused across iterations simply by clearing it, so we keep it outside the loop.
            //

            var remove = new List<object>();

            foreach (var entry in Reports)
            {
                var partition = entry.Key;
                var report = entry.Value;

                foreach (var o in report.Objects)
                {
                    if (map.TryGetValue(o, out var otherPartition))
                    {
                        //
                        // If the other partition is non-null, it indicates that the current partition is the first one
                        // to detect that the object is shared. Remove it from the other partition, and add it to the
                        // shared partition.
                        //

                        if (otherPartition != null)
                        {
                            Reports[otherPartition].Objects.Remove(o);
                            Shared.Objects.Add(o);

                            //
                            // From now on, we don't care about the original heap containing the shared object anymore;
                            // instead, we just change the value of the entry to null, so any subsequent TryGetValue
                            // checks will pass and will result in removing the already-determined-to-be-shared object
                            // from all subsequent private partitions that reference the object.
                            //

                            map[o] = null;
                        }

                        //
                        // Unconditionally remove the object form the current private partition. TryGetValue returning
                        // true indicates that the object is shared and should no longer be kept on the private heap.
                        //

                        remove.Add(o);
                    }
                    else
                    {
                        //
                        // This is the first partition to encounter the object. Keep track of it in case we see the same
                        // object in another partition, so we can move it from the private heap to the shared heap.
                        //

                        map[o] = partition;
                    }
                }

                report.Objects.ExceptWith(remove);
                remove.Clear();
            }
        }
    }
}
