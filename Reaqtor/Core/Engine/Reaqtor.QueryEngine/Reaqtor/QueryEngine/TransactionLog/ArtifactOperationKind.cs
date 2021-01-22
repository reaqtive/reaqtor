// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Enumeration of the kinds of operations that can be applied to persisted artifacts.
    /// </summary>
    internal enum ArtifactOperationKind
    {
        /// <summary>
        /// Invalid kind.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The operation is a creation of an artifact that didn't exist yet.
        /// </summary>
        Create = 1,

        /// <summary>
        /// The operation is a deletion of an existing artifact.
        /// </summary>
        Delete = 2,

        /// <summary>
        /// The operation is a creation of an artifact that has been deleted.
        /// </summary>
        /// <remarks>
        /// This case occurs when a transaction log contains a create operation after a delete operation
        /// for the same artifact. This allows us to coalesce different operations on the same artifact in
        /// a single transaction, ensuring we have at most one operation per artifact.
        /// </remarks>
        DeleteCreate = 3,
    }
}
