// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a system that can be checkpointed.
    /// </summary>
    public interface ICheckpointable
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable CA1068 // CancellationToken parameters must come last (historical remnant kept for compat)

        /// <summary>
        /// Saves the state of the system to a store.
        /// </summary>
        /// <param name="writer">Writer to save state to.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        Task CheckpointAsync(IStateWriter writer, CancellationToken token = default, IProgress<int> progress = null);

        /// <summary>
        /// Recovers the state of the system from a store.
        /// </summary>
        /// <param name="reader">Reader to load state from.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        Task RecoverAsync(IStateReader reader, CancellationToken token = default, IProgress<int> progress = null);

        /// <summary>
        /// Unloads the system.
        /// </summary>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        Task UnloadAsync(IProgress<int> progress = null);

#pragma warning restore CA1068
#pragma warning restore IDE0079 // Remove unnecessary suppression
    }
}
