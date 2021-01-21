// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Options available for <see cref="HeapUsageAnalyzer.Analyze(HeapAnalysisOptions)"/>.
    /// </summary>
    public sealed class HeapAnalysisOptions
    {
        /// <summary>
        /// Gets or sets the degree of parallellism used to scan the various heap partitions, which is
        /// an embarrasingly parallel task. Use a value of <c>0</c> to disable parallel execution.
        /// </summary>
        public int DegreeOfParallelism { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the analysis should compute a shared heap across
        /// all partitions. An object is considered shared if it is reachable from at least two partitions.
        /// </summary>
        public bool ComputeSharedHeap { get; set; }
    }
}
