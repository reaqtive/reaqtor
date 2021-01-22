// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Stateful operators shold consider implementing this interface so they can participate in
    /// state persistence (checkpointing) and recovery.
    /// 
    /// Checkpoint sequence:
    /// 1. SaveState
    /// 2. OnStateSaved
    /// 
    /// Recovery sequence:
    /// 1. SetContext
    /// 2. LoadState
    /// 3. Start
    /// </summary>
    public interface IStatefulOperator : IOperator, IVersioned
    {
        /// <summary>
        /// Called to load the operator's state from a previously persisted snapshot.
        /// </summary>
        /// <param name="reader">Reader to read operator state from.</param>
        /// <param name="version">Version of the state being loaded.</param>
        void LoadState(IOperatorStateReader reader, Version version);

        /// <summary>
        /// Called to persist a snapshot of the operator's state. 
        /// </summary>
        /// <param name="writer">Writer to write operator state to.</param>
        /// <param name="version">Version of the state being saved.</param>
        void SaveState(IOperatorStateWriter writer, Version version);

        /// <summary>
        /// Called after a checkpoint (persisting the states of multiple operators) has completed and before any
        /// new incoming events are received. Operators can update/trim their state in this method.
        /// </summary>
        void OnStateSaved();

        /// <summary>
        /// True if the state has changes since the last time it was saved and false otherwise.
        /// This flag can be used to avoid saving the state of the operator unnecessary.
        /// Operators might choose not to implement this and always return true.
        /// </summary>
        bool StateChanged { get; }
    }
}
