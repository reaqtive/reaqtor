// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Reaqtor.QueryEngine
{
    //
    // REVIEW: Is this still needed?
    //

    /// <summary>
    /// Represents a checkpoint storage provider.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ICheckpointStorageProvider
    {
        /// <summary>
        /// Start a new (full) checkpoint with the provided identifier.
        /// </summary>
        /// <param name="id">Identifier for the checkpoint.</param>
        /// <returns>A writer which is used to store the state.</returns>
        IStateWriter StartNewCheckpoint(string id);

        /// <summary>
        /// Update the latest full checkpoint and update its identifier. 
        /// This corresponds to a differential checkpoint.
        /// </summary>
        /// <param name="id">Identifier for the checkpoint</param>
        /// <returns>A writer which is used to store the state.</returns>
        /// <remarks>Throws an exception if there is no full checkpoint in the storage provider.</remarks>
        IStateWriter UpdateCheckpoint(string id);

        /// <summary>
        /// Read the state contained in the latest full checkpoint.
        /// In case of success, the identifier of the checkpoint as well as a reader 
        /// are provided.
        /// </summary>
        /// <param name="id">Identifier for the checkpoint</param>
        /// <param name="reader">A reader which is used to extract the state.</param>
        /// <returns><b>true</b> if a full checkpoint is available, <b>false</b> otherwise.</returns>
        bool TryReadCheckpoint(out string id, out IStateReader reader);
    }
}
