// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections;
using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// State change manager used to keep track of the dirty state of a storage entity.
    /// </summary>
    internal struct StateChangedManager
    {
        /// <summary>
        /// A value indicating whether the last checkpoint is pending and has not yet been committed by a call to <see cref="OnStateSaved"/>.
        /// </summary>
        private bool _lastCheckpointNoSuccess;

        /// <summary>
        /// A value indicating whether a state change has been reported via a call to the <see cref="StateChanged"/> setter.
        /// </summary>
        private bool _stateChanged;

        /// <summary>
        /// A value indicating whether the entity has been saved at least once, e.g. due to a call to <see cref="LoadState"/> or <see cref="SaveState"/>.
        /// </summary>
        private bool _savedOnce;

        /// <summary>
        /// Gets or sets a value indicating whether a state change has occurred, requiring persistence of the entity.
        /// </summary>
        public bool StateChanged
        {
            get => !_savedOnce || _stateChanged || _lastCheckpointNoSuccess;
            set => _stateChanged = value;
        }

        /// <summary>
        /// Called after successfully loading the state.
        /// </summary>
        public void LoadState()
        {
            _savedOnce = true;
        }

        /// <summary>
        /// Called upon initiating a state saving operation.
        /// </summary>
        public void SaveState()
        {
            //
            // NB: We reset the _stateChanged flag so subsequent calls to StateChanged can toggle it back. We track the not-yet-committed state using _lastCheckpointNoSuccess,
            //     which gets reverted to false by OnStateSaved upon a successful commit. Setting the _savedOnce flag to true here is harmless because any failure to commit the
            //     state will be tracked by _lastCheckpointNoSuccess.
            //

            _savedOnce = true;
            _lastCheckpointNoSuccess = true;
            _stateChanged = false;
        }

        /// <summary>
        /// Called upon successful persistence of the dirty state.
        /// </summary>
        public void OnStateSaved()
        {
            //
            // NB: We only set _lastCheckpointNoSuccess. By now, _stateChanged can be set to true again, causing StateChanged to remain reporting a dirty state.
            //

            _lastCheckpointNoSuccess = false;
        }
    }

    /// <summary>
    /// State change manager using edit pages of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the edit pages</typeparam>
    internal sealed class StateChangedManager<T> : IEnumerable<T>
        where T : new()
    {
        /// <summary>
        /// The list of edit pages in chronological order. This list gets appended with an empty edit page in <see cref="SaveState"/> and pruned in <see cref="OnStateSaved"/>.
        /// </summary>
        private readonly List<T> _edits = new();

        /// <summary>
        /// Creates a new state changed manager with a single empty edit page.
        /// </summary>
        public StateChangedManager()
        {
            State = new T();
            _edits.Add(State);
        }

        /// <summary>
        /// Gets the latest (most recent) edit page.
        /// </summary>
        /// <remarks>
        /// Keeping this as separate state to avoid list access in the get accessor which can occur concurrently with a call to <see cref="OnStateSaved"/>.
        /// </remarks>
        public T State { get; private set; }

        /// <summary>
        /// Called upon initiating a state saving operation to create a snapshot of the edit pages to incorporate in the checkpoint.
        /// </summary>
        /// <returns>A snapshot of the edit pages.</returns>
        public T[] SaveState()
        {
            var res = _edits.ToArray();

            State = new T();
            _edits.Add(State);

            return res;
        }

        /// <summary>
        /// Called upon successful persistence of the dirty state. This cause all edit pages (except for the last one) to be pruned.
        /// </summary>
        public void OnStateSaved()
        {
            _edits.RemoveRange(0, _edits.Count - 1);
        }

        /// <summary>
        /// Gets an enumerator to enumerate over the edit pages in reverse chronological order (i.e. most recent edit page first).
        /// </summary>
        /// <returns>An enumerator to enumerate over the edit pages in reverse chronological order.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var n = _edits.Count;

            for (var i = n - 1; i >= 0; i--)
            {
                yield return _edits[i];
            }
        }

        /// <summary>
        /// Gets an enumerator to enumerate over the edit pages in reverse chronological order (i.e. most recent edit page first).
        /// </summary>
        /// <returns>An enumerator to enumerate over the edit pages in reverse chronological order.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
