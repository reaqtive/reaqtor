﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Reaqtive
{
    /// <summary>
    /// Base implementation of the non-generic multi-subject, with support for checkpointing.
    /// </summary>
    public abstract class StatefulMultiSubjectBase : VersionedMultiSubjectBase, IStatefulOperator
    {
        private StateChangedManager _stateful;
        private bool _disposedFromState;

        /// <summary>
        /// Gets a flag indicating whether state has changed since the last time the
        /// subject state got saved.
        /// </summary>
        public virtual bool StateChanged
        {
            get => _stateful.StateChanged;
            protected set => _stateful.StateChanged = value;
        }

        /// <summary>
        /// Loads the state of the subject.
        /// </summary>
        /// <param name="reader">Reader to read subject state from.</param>
        /// <param name="version">Version of the state.</param>
        public void LoadState(IOperatorStateReader reader, Version version)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            _disposedFromState = reader.Read<bool>();

            if (version != Version)
            {
                LoadStateCore(reader, version);
            }
            else
            {
                LoadStateCore(reader);
            }

            _stateful.LoadState();
        }

        /// <summary>
        /// Loads the state of the subject.
        /// </summary>
        /// <param name="reader">Reader to read subject state from.</param>
        protected virtual void LoadStateCore(IOperatorStateReader reader)
        {
        }

        /// <summary>
        /// Loads the state of the subject using the specified state version.
        /// </summary>
        /// <param name="reader">Reader to read subject state from.</param>
        /// <param name="version">Version of the state being read.</param>
        protected virtual void LoadStateCore(IOperatorStateReader reader, Version version)
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Version {0} of subject '{1}' does not support loading from checkpoint state version {2}.", Version, Name, version));
        }

        /// <summary>
        /// Saves the state of the operator.
        /// </summary>
        /// <param name="writer">Writer to write operator state to.</param>
        /// <param name="version">Version of the state being saved.</param>
        public void SaveState(IOperatorStateWriter writer, Version version)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(IsDisposed);

            if (version != Version)
            {
                SaveStateCore(writer, version);
            }
            else
            {
                SaveStateCore(writer);
            }

            _stateful.SaveState();
        }

        /// <summary>
        /// Saves the state of the subject.
        /// </summary>
        /// <param name="writer">Writer to write subject state to.</param>
        protected virtual void SaveStateCore(IOperatorStateWriter writer)
        {
        }

        /// <summary>
        /// Saves the state of the subject using the specified state version.
        /// </summary>
        /// <param name="writer">Writer to write subject state to.</param>
        /// <param name="version">Version of the state being written.</param>
        protected virtual void SaveStateCore(IOperatorStateWriter writer, Version version)
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Version {0} of subject '{1}' does not support saving checkpoint state version {2}.", Version, Name, version));
        }

        /// <summary>
        /// Called when subject state was successfully saved.
        /// </summary>
        public virtual void OnStateSaved()
        {
            _stateful.OnStateSaved();
        }

        /// <summary>
        /// Called when the subject is disposed.
        /// </summary>
        protected override void OnDispose()
        {
            StateChanged = !_disposedFromState;

            base.OnDispose();
        }

        /// <summary>
        /// Used internally to modify the IsDisposed flag, in case it should return
        /// true before a call to the Dispose method is made.
        /// </summary>
        internal sealed override bool IsDisposedCore => _disposedFromState;
    }
}
