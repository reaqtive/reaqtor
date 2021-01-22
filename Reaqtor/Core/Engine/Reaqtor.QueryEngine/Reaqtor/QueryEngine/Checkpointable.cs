// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Extension methods for the <see cref="ICheckpointable"/> interface.
    /// </summary>
    public static class CheckpointableExtensions
    {
        /// <summary>
        /// Saves the state of the system to a store.
        /// </summary>
        /// <param name="this">System whose state will be saved.</param>
        /// <param name="writer">Writer to save state to.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task CheckpointAsync(this ICheckpointable @this, IStateWriter writer)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.CheckpointAsync(writer, CancellationToken.None, progress: null);
        }

        /// <summary>
        /// Saves the state of the system to a store.
        /// </summary>
        /// <param name="this">System whose state will be saved.</param>
        /// <param name="writer">Writer to save state to.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task CheckpointAsync(this ICheckpointable @this, IStateWriter writer, CancellationToken token)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.CheckpointAsync(writer, token, progress: null);
        }

        /// <summary>
        /// Saves the state of the system to a store.
        /// </summary>
        /// <param name="this">System whose state will be saved.</param>
        /// <param name="writer">Writer to save state to.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task CheckpointAsync(this ICheckpointable @this, IStateWriter writer, IProgress<int> progress)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.CheckpointAsync(writer, CancellationToken.None, progress);
        }

        /// <summary>
        /// Recovers the state of the system from a store.
        /// </summary>
        /// <param name="this">System whose state will be loaded.</param>
        /// <param name="reader">Reader to load state from.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task RecoverAsync(this ICheckpointable @this, IStateReader reader)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.RecoverAsync(reader, CancellationToken.None, progress: null);
        }

        /// <summary>
        /// Recovers the state of the system from a store.
        /// </summary>
        /// <param name="this">System whose state will be loaded.</param>
        /// <param name="reader">Reader to load state from.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task RecoverAsync(this ICheckpointable @this, IStateReader reader, CancellationToken token)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.RecoverAsync(reader, token, progress: null);
        }

        /// <summary>
        /// Recovers the state of the system from a store.
        /// </summary>
        /// <param name="this">System whose state will be loaded.</param>
        /// <param name="reader">Reader to load state from.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task RecoverAsync(this ICheckpointable @this, IStateReader reader, IProgress<int> progress)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.RecoverAsync(reader, CancellationToken.None, progress);
        }

        /// <summary>
        /// Unloads the system.
        /// </summary>
        /// <param name="this">System that will be unloaded.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public static Task UnloadAsync(this ICheckpointable @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.UnloadAsync(progress: null);
        }
    }
}
