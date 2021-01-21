// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Base class for storage entities that support persistence to a key/value store.
        /// </summary>
        private abstract class PersistableBase : IPersistable
        {
            /// <summary>
            /// The parent object space, used to access serialization facilities.
            /// </summary>
            private readonly PersistedObjectSpace _parent;

            /// <summary>
            /// The strongly typed wrapper, managed by derived types.
            /// </summary>
            protected object _wrapper;

            /// <summary>
            /// The state change manager used to track the dirty state of the entity (see <see cref="Load"/>, <see cref="Save"/>, and <see cref="OnSaved"/>).
            /// </summary>
            private StateChangedManager _changed;

            /// <summary>
            /// Creates a new storage entity.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            protected PersistableBase(PersistedObjectSpace parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the storage entity has dirty state that needs to be persisted in future checkpoints.
            /// </summary>
            public bool StateChanged
            {
                get => _changed.StateChanged;
                protected set => _changed.StateChanged = value;
            }

            /// <summary>
            /// Gets a value indicating whether the storage entity has been saved at least once, used to determine whether the instance represents a newly created storage entity
            /// or all of the entity's state has to be saved in <see cref="Save(IStateWriter)"/> regardless of the checkpoint type.
            /// </summary>
            protected bool HasSaved { get; private set; }

            /// <summary>
            /// Loads the state of the object from the specified state <paramref name="reader"/>.
            /// </summary>
            /// <param name="reader">The reader to read the state from.</param>
            public void Load(IStateReader reader)
            {
                _changed.LoadState();

                HasSaved = true;

                LoadCore(reader);
            }

            /// <summary>
            /// Loads the state of the object from the specified state <paramref name="reader"/>.
            /// </summary>
            /// <param name="reader">The reader to read the state from.</param>
            protected abstract void LoadCore(IStateReader reader);

            /// <summary>
            /// Marks the last <see cref="Save(IStateWriter)"/> operation as successful.
            /// </summary>
            /// <remarks>
            /// See <see cref="Save(IStateWriter)"/> for more information.
            /// </remarks>
            public void OnSaved()
            {
                _changed.OnStateSaved();

                HasSaved = true;

                OnSavedCore();
            }

            /// <summary>
            /// Marks the last <see cref="Save(IStateWriter)"/> operation as successful.
            /// </summary>
            /// <remarks>See remarks on <see cref="OnSaved"/>.</remarks>
            protected virtual void OnSavedCore() { }

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
            public void Save(IStateWriter writer)
            {
                _changed.SaveState();

                SaveCore(writer);
            }

            /// <summary>
            /// Saves the state of the object to the specified state <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The writer to write the state to.</param>
            /// <remarks>See remarks on <see cref="Save(IStateWriter)"/>.</remarks>
            protected abstract void SaveCore(IStateWriter writer);

            /// <summary>
            /// Deletes all the state of the object using the specified state <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The writer to perform state deletions on.</param>
            /// <remarks>
            /// Unlike the <see cref="Save(IStateWriter)"/> method, no subsequent call to <see cref="OnSaved"/> is required when attempting a deletion. It is
            /// assumed that a deletion operation is always the last operation performed on an object. The caller can re-attempt deletion if a prior commit of
            /// the deletion operations has failed, simply by calling <see cref="Delete(IStateWriter)"/> on the object again.
            /// </remarks>
            public void Delete(IStateWriter writer)
            {
                //
                // Check if the entity has been saved at least once. If not, we only have in-memory state.
                //

                if (HasSaved)
                {
                    DeleteCore(writer);
                }
            }

            /// <summary>
            /// Deletes all the state of the object using the specified state <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The writer to perform state deletions on.</param>
            /// <remarks>See remarks on <see cref="Delete(IStateWriter)"/>.</remarks>
            protected abstract void DeleteCore(IStateWriter writer);

            /// <summary>
            /// Gets the serialization factory used to serialize and deserialize state.
            /// </summary>
            protected ISerializationFactory SerializationFactory => _parent._serializationFactory;

            /// <summary>
            /// Gets the kind of the persisted entity.
            /// </summary>
            public abstract PersistableKind Kind { get; }

            /// <summary>
            /// Gets a serializer for objects of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the objects to serialize.</typeparam>
            /// <returns>A serializer for objects of type <typeparamref name="T"/>.</returns>
            protected ISerializer<T> GetSerializer<T>() => SerializationFactory.GetSerializer<T>();

            /// <summary>
            /// Gets a deserializer for objects of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
            /// <returns>A deserializer for objects of type <typeparamref name="T"/>.</returns>
            protected IDeserializer<T> GetDeserializer<T>() => SerializationFactory.GetDeserializer<T>();

            /// <summary>
            /// Gets the numeric representation of the specified <paramref name="key"/>.
            /// </summary>
            /// <param name="key">The key to get a numeric representation for.</param>
            /// <returns>The numeric representation of the specified <paramref name="key"/>.</returns>
            protected static long GetIndexForKey(string key) => IntegerKey.Base62.Parse(key);

            /// <summary>
            /// Gets the string representation of the specified <paramref name="index"/>.
            /// </summary>
            /// <param name="index">The index to get a string representation for.</param>
            /// <returns>The string representation of the specified <paramref name="index"/>.</returns>
            protected static string GetKeyForIndex(long index) => IntegerKey.Base62.ToString(index); // REVIEW: Pad left? Memoize commonly used values?
        }
    }
}
