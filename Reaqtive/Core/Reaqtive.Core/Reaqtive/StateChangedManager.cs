// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1815 // Override equals and operator equals on value types. (Just used as a collection of fields; not meant to be used for value equality.)

namespace Reaqtive
{
    /// <summary>
    /// Struct bundling all state management flags required to implement stateful artifacts.
    /// </summary>
    public struct StateChangedManager
    {
        private bool _lastCheckpointNoSuccess;
        private bool _stateChanged;
        private bool _savedOnce;

        /// <summary>
        /// Gets or sets whether the artifact's state has changed since the last checkpoint.
        /// </summary>
        public bool StateChanged
        {
            // The operator should be considered dirty if it has never been
            // through a `SaveState` operation. We know an operator has been
            // through a `SaveState` operation at least once if `LoadState`
            // occurs, or if `SaveState` is invoked in the current lifecycle
            // of the operator. The remaining flags will signify that the
            // operator is dirty if the `StateChanged` property was set
            // explicitly to true, or the last checkpoint did not succeed.
            get => !_savedOnce || _stateChanged || _lastCheckpointNoSuccess;
            set => _stateChanged = value;
        }

        /// <summary>
        /// Indicates that state has been loaded from storage.
        /// </summary>
        public void LoadState()
        {
            _savedOnce = true;
        }

        /// <summary>
        /// Indicates that state has been captured for getting saved to storage. Notice that a subsequent
        /// call to OnStateSaved is required to toggle the StateChanged flag upon successful persistence.
        /// </summary>
        public void SaveState()
        {
            _savedOnce = true;
            _lastCheckpointNoSuccess = true;
            _stateChanged = false;
        }

        /// <summary>
        /// Indicates that the state captured during SaveState has been successfully committed to persistent
        /// storage.
        /// </summary>
        public void OnStateSaved()
        {
            _lastCheckpointNoSuccess = false;
        }
    }
}
