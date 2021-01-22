// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing an object with persistable state.
    /// </summary>
    internal interface IPersistable
    {
        /// <summary>
        /// Gets the kind of the object.
        /// </summary>
        PersistableKind Kind { get; }

        /// <summary>
        /// Gets a value indicating whether state has changed since the last successful state saving (see <see cref="Save(IStateWriter)"/> and <see cref="OnSaved"/>).
        /// </summary>
        bool StateChanged { get; }

        /// <summary>
        /// Loads the state of the object from the specified state <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader to read the state from.</param>
        void Load(IStateReader reader);

        /// <summary>
        /// Saves the state of the object to the specified state <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the state to.</param>
        /// <remarks>
        /// In order for the <see cref="StateChanged"/> property's value to be updated upon a successful persistence of the state into some permanant store,
        /// a call to the <see cref="OnSaved"/> method should be made. The role of the <see cref="Save(IStateWriter)"/> method is to write the state of the
        /// object to the state <paramref name="writer"/>, which acts as a staging ground for changes to be committed. Once this method has completed, normal
        /// execution can resume and subsequent changes will be tracked for subsequent calls to <see cref="Save(IStateWriter)"/>. This allows for asynchronous
        /// commit to the permanent store, after which a call to <see cref="OnSaved"/> is made to re-evaluate the value of <see cref="StateChanged"/>.
        /// </remarks>
        void Save(IStateWriter writer);

        /// <summary>
        /// Marks the last <see cref="Save(IStateWriter)"/> operation as successful.
        /// </summary>
        /// <remarks>
        /// See <see cref="Save(IStateWriter)"/> for more information.
        /// </remarks>
        void OnSaved();

        /// <summary>
        /// Deletes all the state of the object using the specified state <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to perform state deletions on.</param>
        /// <remarks>
        /// Unlike the <see cref="Save(IStateWriter)"/> method, no subsequent call to <see cref="OnSaved"/> is required when attempting a deletion. It is
        /// assumed that a deletion operation is always the last operation performed on an object. The caller can re-attempt deletion if a prior commit of
        /// the deletion operations has failed, simply by calling <see cref="Delete(IStateWriter)"/> on the object again.
        /// </remarks>
        void Delete(IStateWriter writer);
    }
}
