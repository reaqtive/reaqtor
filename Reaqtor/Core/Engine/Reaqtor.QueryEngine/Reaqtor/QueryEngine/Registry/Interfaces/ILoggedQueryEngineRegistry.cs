// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for engine registries that supports snapshotting (used for log/truncate operations in checkpoints).
    /// </summary>
    internal interface ILoggedQueryEngineRegistry : IQueryEngineRegistry
    {
        /// <summary>
        /// Creates a snapshot of the registry, including reactive entity collections and removed keys in these collections.
        /// </summary>
        /// <returns>Object representing the snapshot.</returns>
        /// <remarks>
        /// This is called when a checkpoint is started, by the state saver. The checkpoint will persist all changes up to
        /// and including the current snapshot. If the checkpoint is successful, the snapshot will be used for truncation.
        /// </remarks>
        IQueryEngineRegistrySnapshot TakeSnapshot();

        /// <summary>
        /// Truncate the registry by permanently applying the logged shapshot operations.
        /// </summary>
        /// <param name="snapshot">The snapshot to apply.</param>
        /// <remarks>
        /// This is called when a checkpoint is successfully committed to the store (during the `OnStateSaved` phase) to
        /// prune the registry (e.g. by deleting entries that were on the list of removed keys).
        /// </remarks>
        void TruncateLoggedEntities(IQueryEngineRegistrySnapshot snapshot);
    }
}
