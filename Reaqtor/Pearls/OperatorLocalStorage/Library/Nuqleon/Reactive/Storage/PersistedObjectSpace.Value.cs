// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Diagnostics;
using System.Collections.Generic; // NB: Used for XML doc comments.
using System.IO;

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted value.
        /// </summary>
        /// <typeparam name="T">The type of data stored in the value.</typeparam>
        /// <param name="id">The identifier to use for the value.</param>
        /// <returns>A new persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedValue<T> CreateValue<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var value = new Value(this);
            _items.Add(id, value);
            return CreateValueCore<T>(id, value);
        }

        /// <summary>
        /// Gets a persisted value with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of data stored in the value.</typeparam>
        /// <param name="id">The identifier of the value to retrieve.</param>
        /// <returns>An existing persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted value type.</exception>
        public IPersistedValue<T> GetValue<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateValueCore<T>(id, (Value)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the value.</typeparam>
        /// <param name="id">The identifier of the value.</param>
        /// <param name="value">The storage entity representing the value.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="value"/>.</returns>
        /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
        private static IPersistedValue<T> CreateValueCore<T>(string id, Value value) => value.Create<T>(id);

        /// <summary>
        /// Storage entity representing the value.
        /// </summary>
        /// <remarks>
        /// Persistence of a value looks as follows:
        /// <c>
        /// data/value = 42
        /// </c>
        /// </remarks>
        private sealed class Value : PersistableBase
        {
            /// <summary>
            /// The category to store the data in.
            /// </summary>
            private const string DataCategory = "data";

            /// <summary>
            /// The key to store the value in.
            /// </summary>
            private const string ValueKey = "value";

            /// <summary>
            /// The eventual object containing the value, set by <see cref="Load(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>.
            /// </summary>
            private EventualObject _data;

            /// <summary>
            /// Creates a new entity representing a value.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Value(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.Value"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.Value;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the data stored in the value.</typeparam>
            /// <param name="id">The identifier of the value.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedValue<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    var value = new Wrapper<T>(id, this, Restore<T>());
                    _wrapper = value;
                    return value;
                }
                else
                {
                    return (IPersistedValue<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the value from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Only have to remove the value in data/value.
                //

                writer.DeleteItem(DataCategory, ValueKey);
            }

            /// <summary>
            /// Loads the value from storage.
            /// </summary>
            /// <param name="reader">The reader to load the value from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Create an eventual object for data/value which gets deserialized upon first use by Restore<T>.
                //

                using var stream = reader.GetItemReader(DataCategory, ValueKey);

                _data = EventualObject.FromState(stream);
            }

            /// <summary>
            /// Saves the value to storage.
            /// </summary>
            /// <param name="writer">The writer to write the value to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Edit).");

                //
                // Save the value to data/value unconditionally.
                //
                // NB: There's no difference for full or differential checkpoints here. We assume we only get called when StateChanged is true.
                //

                using var stream = writer.GetItemWriter(DataCategory, ValueKey);

                ((IValuePersistence)_wrapper).Save(SerializationFactory, stream);
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}.Value"/> when a new value gets assigned.
            /// </summary>
            private void Edit()
            {
                //
                // Mark the state as dirty.
                //
                // REVIEW: Should we consider supporting equality comparers in the wrappers to avoid spurious updates?
                //

                StateChanged = true;
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory representation of type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a default value of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of data stored in the value.</typeparam>
            /// <returns>An instance of type <typeparamref name="T"/> containing the data represented by the storage entity.</returns>
            private T Restore<T>()
            {
                //
                // If the state was loaded by LoadCore, we have an eventual object to deserialize from. Otherwise, return the default value.
                //

                if (_data != null)
                {
                    var res = _data.Deserialize<T>(SerializationFactory);

                    _data = null;

                    return res;
                }
                else
                {
                    return default;
                }
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="Value"/> storage entity to the statically typed <see cref="Wrapper{T}"/> instance.
            /// </summary>
            private interface IValuePersistence
            {
                /// <summary>
                /// Saves the value to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the value to.</param>
                void Save(ISerializerFactory serializerFactory, Stream stream);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted value of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the persisted value.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedValue<T>, IValuePersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly Value _storage;

                /// <summary>
                /// The stored value, always reflecting the latest in-memory state.
                /// </summary>
                private T _value;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the value.</param>
                /// <param name="storage">The storage entity representing the value.</param>
                /// <param name="value">The initial value. This could either be the result of deserializing persisted state, or a default value for a new entity.</param>
                public Wrapper(string id, Value storage, T value)
                    : base(id)
                {
                    _storage = storage;
                    _value = value;
                }

                /// <summary>
                /// Gets or sets the value.
                /// </summary>
                public T Value
                {
                    get => _value;

                    set
                    {
                        _value = value;

                        //
                        // Track the edit in the storage entity.
                        //

                        _storage.Edit();
                    }
                }

                /// <summary>
                /// Saves the value to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the value to.</param>
                void IValuePersistence.Save(ISerializerFactory serializerFactory, Stream stream)
                {
                    //
                    // REVIEW: This could be dangerous because the Get*<T> method for a persisted entity does not have any possibility to
                    //         check whether the specified type is compatible with the already persisted state. E.g. if the call to get a
                    //         persisted entity uses a (use-site) structural subtype of the original (definition-site) structural type,
                    //         we'll end up (de)serializing only part of the state. If we want to avoid this, type checking should be added
                    //         at a higher level and the definition-site type should be persisted to enable performing such checks.
                    //

                    serializerFactory.GetSerializer<T>().Serialize(_value, stream);
                }
            }
        }
    }
}
