// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// In-memory Checkpoint storage provider.
    /// Keeps the latest full checkpoint in memory and at most one ongoing checkpoint.
    /// </summary>
    public class InMemoryStorageProvider : ICheckpointStorageProvider
    {
        private volatile CheckpointInfo _latestFullCheckpoint;
        private volatile CheckpointInfo _currentCheckpoint;
        private readonly object _lock = new();

        /// <summary>
        /// Start a new (full) checkpoint with the provided identifier.
        /// <remarks> A new checkpoint cannot be started while another is ongoing.</remarks>
        /// </summary>
        /// <param name="id">Identifier for the checkpoint.</param>
        /// <returns>A writer which is used to store the state.</returns>
        public IStateWriter StartNewCheckpoint(string id)
        {
            return CreateCheckpoint(id, CheckpointKind.Full);
        }

        /// <summary>
        /// Update the checkpoint with the provided identifier. 
        /// This coresponds to a differential checkpoint.
        /// <remarks> A new checkpoint cannot be started while another is ongoing.</remarks>
        /// </summary>
        /// <param name="id">Identifier for the checkpoint</param>
        /// <returns>A writer which is used to store the state.</returns>
        public IStateWriter UpdateCheckpoint(string id)
        {
            return CreateCheckpoint(id, CheckpointKind.Differential);
        }

        /// <summary>
        /// Read the state contained in the latest full checkpoint.
        /// In case of success, the identifier of the checkpoint as well as a reader 
        /// are provided.
        /// </summary>
        /// <param name="id">Identifier for the checkpoint</param>
        /// <param name="reader">A reader which is used to extract the state.</param>
        /// <returns><b>true</b> if a full checkpoint is available, <b>false</b> otherwise.</returns>
        public bool TryReadCheckpoint(out string id, out IStateReader reader)
        {
            CheckpointInfo latest = _latestFullCheckpoint;

            if (latest == null)
            {
                reader = default;
                id = default;
                return false;
            }

            reader = new InMemoryStateReader(latest.Store);

            id = _latestFullCheckpoint.Id;

            return true;
        }

        /// <summary>
        /// Method called when a checkpoint writer is committing.
        /// </summary>
        /// <param name="checkpointInfo">The checkpoint information</param>
        private void OnCommit(CheckpointInfo checkpointInfo)
        {
            lock (_lock)
            {
                Debug.Assert(checkpointInfo.Id == _currentCheckpoint.Id
                    && checkpointInfo.Store == _currentCheckpoint.Store,
                    "Multiple checkpointing occurring concurrently.");

                _currentCheckpoint = null;

                if (checkpointInfo.Kind == CheckpointKind.Differential)
                {
                    Debug.Assert(_latestFullCheckpoint != null,
                        "At least one full checkpoint must exist before creating differential checkpoint.");

                    _latestFullCheckpoint.Store.Update(checkpointInfo.Store);
                    _latestFullCheckpoint.LatestUpdate = DateTime.UtcNow;
                }
                else
                {
                    _latestFullCheckpoint = checkpointInfo;
                    _latestFullCheckpoint.LatestUpdate = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// Method called when a checkpoint writer is rolling back.
        /// </summary>
        /// <param name="checkpointInfo">The checkpoint information</param>
        private void OnRollback(CheckpointInfo checkpointInfo)
        {
            lock (_lock)
            {
                Debug.Assert(checkpointInfo.Id == _currentCheckpoint.Id
                    && checkpointInfo.Store == _currentCheckpoint.Store,
                    "Multiple checkpointing occurring concurrently.");

                _currentCheckpoint.Store.Clear();
                _currentCheckpoint = null;
            }
        }

        /// <summary>
        /// Create a new checkpoint (full or differential).
        /// </summary>
        /// <param name="id">The checkpoint id.</param>
        /// <param name="kind">The checkpoint kind.</param>
        /// <returns>A writer used for storing the state.</returns>
        private IStateWriter CreateCheckpoint(string id, CheckpointKind kind)
        {
            lock (_lock)
            {
                if (_currentCheckpoint != null)
                {
                    throw new InvalidOperationException("There is already a checkpoint being generated.");
                }

                _currentCheckpoint = new CheckpointInfo(new InMemoryStateStore(id), kind)
                {
                    Creation = DateTime.UtcNow
                };

                return new InMemoryStateWriter(_currentCheckpoint.Store, kind, () => OnCommit(_currentCheckpoint), () => OnRollback(_currentCheckpoint));
            }
        }

        private sealed class CheckpointInfo
        {
            public CheckpointInfo(InMemoryStateStore store, CheckpointKind kind)
            {
                Store = store;
                Kind = kind;
            }

            public DateTime Creation { get; set; }

            public DateTime LatestUpdate { get; set; }

            public InMemoryStateStore Store { get; }

            public string Id => Store.Id;

            public CheckpointKind Kind { get; }
        }
    }
}
