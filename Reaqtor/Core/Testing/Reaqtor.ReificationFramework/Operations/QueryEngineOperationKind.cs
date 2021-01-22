// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Enumeration of reified query engine operation kinds.
    /// </summary>
    public enum QueryEngineOperationKind
    {
        /// <summary>
        /// Operation to perform a differential checkpoint.
        /// </summary>
        DifferentialCheckpoint,

        /// <summary>
        /// Operation to perform a full checkpoint.
        /// </summary>
        FullCheckpoint,

        /// <summary>
        /// Operation to perform a recovery.
        /// </summary>
        Recovery,
    }
}
