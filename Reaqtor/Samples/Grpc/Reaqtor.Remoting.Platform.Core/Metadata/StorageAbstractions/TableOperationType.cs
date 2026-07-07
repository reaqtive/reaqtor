// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// The set of table operations supported by the storage abstraction.
    /// </summary>
    /// <remarks>
    /// Cosmos-free replacement for <c>Microsoft.Azure.Cosmos.Table.TableOperationType</c> (see plan §2.6). Only the
    /// members exercised by the transport-neutral storage layer (insert, delete, retrieve) are modelled.
    /// </remarks>
    public enum TableOperationType
    {
        /// <summary>An insert operation.</summary>
        Insert,

        /// <summary>A delete operation.</summary>
        Delete,

        /// <summary>A retrieve operation.</summary>
        Retrieve,
    }
}
