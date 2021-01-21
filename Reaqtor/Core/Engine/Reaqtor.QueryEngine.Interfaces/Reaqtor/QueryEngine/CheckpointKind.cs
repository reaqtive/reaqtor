// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Enum with different checkpoint kinds.
    /// </summary>
    public enum CheckpointKind
    {
        /// <summary>
        /// Invalid transaction kind.
        /// </summary>
        Invalid,

        /// <summary>
        /// The complete state is part of the transaction.
        /// </summary>
        Full,

        /// <summary>
        /// Only the differences in the state are part of the transaction.
        /// </summary>
        Differential,
    }
}
