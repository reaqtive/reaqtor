// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Indicates the state of the entity with respect to the transaction log.
    /// </summary>
    internal enum TransactionState : int
    {
        /// <summary>
        /// Entity has been created but has not been recorded in the transaction log.
        /// </summary>
        None = 0,

        /// <summary>
        /// The entity has been created and its creationg has been recorded in the transaction log.
        /// </summary>
        Active = 1,

        /// <summary>
        /// The entity's deletion is being recorded in the transaction log.
        /// </summary>
        Deleting = 2,

        /// <summary>
        /// The entity's deletion has been recorded in the transaction log.
        /// </summary>
        Deleted = 3,
    }
}
