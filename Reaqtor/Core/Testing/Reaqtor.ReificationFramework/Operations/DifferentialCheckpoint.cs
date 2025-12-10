// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to perform a differential checkpoint.
    /// </summary>
    public class DifferentialCheckpoint : QueryEngineOperation
    {
        /// <summary>
        /// Creates a new differential checkpoint operation.
        /// </summary>
        public DifferentialCheckpoint(/* TODO: query engine URI? */)
            : base(QueryEngineOperationKind.DifferentialCheckpoint, targetObjectUri: null, state: null)
        {
        }
    }
}
