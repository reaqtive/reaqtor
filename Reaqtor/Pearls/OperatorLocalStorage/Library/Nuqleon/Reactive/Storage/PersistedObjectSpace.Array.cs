// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted array.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier to use for the array.</param>
        /// <param name="length">The length of the array.</param>
        /// <returns>A new persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedArray<T> CreateArray<T>(string id, int length)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var array = new Array(this, length);
            _items.Add(id, array);
            return CreateArrayCore<T>(id, array);
        }

        /// <summary>
        /// Gets a persisted array with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier of the array to retrieve.</param>
        /// <returns>An existing persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted array type.</exception>
        public IPersistedArray<T> GetArray<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateArrayCore<T>(id, (Array)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the array.</typeparam>
        /// <param name="id">The identifier of the array.</param>
        /// <param name="array">The storage entity representing the array.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="array"/>.</returns>
        private static IPersistedArray<T> CreateArrayCore<T>(string id, Array array) => array.Create<T>(id);

        /// <summary>
        /// Storage entity representing the array.
        /// </summary>
        /// <remarks>
        /// Persistence of an array looks as follows:
        /// <c>
        /// metadata/length = 5
        /// items/0 = 42
        /// items/1 = 43
        /// items/2 = 44
        /// items/3 = 45
        /// items/4 = 46
        /// </c>
        /// The keys of the items are in the range [0..length), where length is immutable.
        /// <list type="bullet">
        ///   <item><c>array[i] = v</c> results in <c>Edit(items/i, v)</c></item>
        /// </list>
        /// </remarks>
        private sealed class Array : PersistableBase
        {
            /// <summary>
            /// The category to store the metadata (i.e. the array length, using the <see cref="LengthKey"/> key) in.
            /// </summary>
            private const string MetadataCategory = "metadata";

            /// <summary>
            /// The category to store the data (i.e. the array elements) in.
            /// </summary>
            private const string ItemsCategory = "items";

            /// <summary>
            /// The key to store the length in (using the <see cref="MetadataCategory"/> category).
            /// </summary>
            private const string LengthKey = "length";

            /// <summary>
            /// The eventual objects containing the array elements, set by <see cref="LoadCore(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>. Indexes in this array range over the elements in [0..length).
            /// </summary>
            private EventualObject[] _data;

            /// <summary>
            /// The length of the array (immutable). This value gets set by <see cref="LoadCore(IStateReader)"/> and by the constructor when creating a new array instance.
            /// </summary>
            private int _length;

            /// <summary>
            /// The state change manager used to keep track of the indexes of elements in the array that have been changed since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private readonly StateChangedManager<HashSet<int>> _dirty = new();

            /// <summary>
            /// Creates a new entity representing an array. This constructor should only be used for recovery of existing entities.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Array(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Creates a new entity representing a new array instance with the specified <paramref name="parent"/> and the specified array <paramref name="length"/>.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            /// <param name="length">The length of the array to create.</param>
            public Array(PersistedObjectSpace parent, int length)
                : base(parent)
            {
                _length = length;
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.Array"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.Array;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the array.</typeparam>
            /// <param name="id">The identifier of the array.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedArray<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    var array = new Wrapper<T>(id, this, Restore<T>());
                    _wrapper = array;
                    return array;
                }
                else
                {
                    return (IPersistedArray<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the array from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Remove the metadata associated with the array.
                //

                writer.DeleteItem(MetadataCategory, LengthKey);

                //
                // Remove the elements stored in items/i in the range [0..length).
                //

                for (var i = 0; i < _length; i++)
                {
                    writer.DeleteItem(ItemsCategory, GetKeyForIndex(i));
                }
            }

            /// <summary>
            /// Loads the array from storage.
            /// </summary>
            /// <param name="reader">The reader to load the array from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Deserialize metadata/length as an integer.
                //

                using (var lengthStream = reader.GetItemReader(MetadataCategory, LengthKey))
                {
                    _length = GetDeserializer<int>().Deserialize(lengthStream);
                }

                //
                // Prepare an array of eventual objects and fill with the elements obtained from the reader at items/i with indexes in range [0..length).
                //

                _data = new EventualObject[_length];

                for (var i = 0; i < _length; i++)
                {
                    using var itemStream = reader.GetItemReader(ItemsCategory, GetKeyForIndex(i));

                    _data[i] = EventualObject.FromState(itemStream);
                }
            }

            /// <summary>
            /// Saves the array to storage.
            /// </summary>
            /// <param name="writer">The writer to write the array to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Edit).");

                //
                // Value indicating whether metadata/length has to be stored. Will be set to true for a full checkpoint or a for a new array that has not yet been persisted.
                //

                var shouldSaveLength = false;

                //
                // An ordered collection of indexes containing elements that have been edited and need to be persisted. For a full checkpoint or a new array, this collection will contain the range [0..length). This value is never null.
                //

                IEnumerable<int> indexes;

                //
                // Snapshot the dirty state (containing edits). See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                var dirtyHistory = _dirty.SaveState();

                //
                // Check if we need to save all the state in case of a full checkpoint or a new array that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    shouldSaveLength = true;

                    var allIndexes = new int[_length];

                    for (var i = 0; i < _length; i++)
                    {
                        allIndexes[i] = i;
                    }

                    indexes = allIndexes;
                }
                else
                {
                    //
                    // Use a sorted set to get deterministic behavior for testing and in-order access to the key/value store which can reduce seek times.
                    //

                    var dirtyIndexes = new SortedSet<int>();

                    foreach (var dirty in dirtyHistory)
                    {
                        dirtyIndexes.UnionWith(dirty);
                    }

                    indexes = dirtyIndexes;
                }

                //
                // Save the metadata/length value if needed.
                //

                if (shouldSaveLength)
                {
                    using var lengthStream = writer.GetItemWriter(MetadataCategory, LengthKey);

                    GetSerializer<int>().Serialize(_length, lengthStream);
                }

                //
                // Save each edited element by dispatching to IArrayPersistence.Save which has access to the array element static type used for serialization.
                //

                var target = (IArrayPersistence)_wrapper;

                foreach (var index in indexes)
                {
                    var key = GetKeyForIndex(index);

                    using var itemStream = writer.GetItemWriter(ItemsCategory, key);

                    target.Save(SerializationFactory, itemStream, index);
                }
            }

            /// <summary>
            /// Marks the last call to <see cref="SaveCore(IStateWriter)"/> as successful.
            /// </summary>
            protected override void OnSavedCore()
            {
                //
                // Prune the edit pages that were persisted by the prior call to SaveCore.
                //

                _dirty.OnStateSaved();
            }

            /// <summary>
            /// Called by the <see cref="Wrapper{T}"/> indexer when the element at the specified <paramref name="index"/> gets assigned.
            /// </summary>
            /// <param name="index">The index of the element in the array that got assigned to.</param>
            private void Edit(int index)
            {
                //
                // Mark the state as dirty and record the edited index in the latest edit page.
                //

                StateChanged = true;

                _dirty.State.Add(index);
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory array representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a new array instance with default values of type <typeparamref name="T"/> for each of its elements.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the array.</typeparam>
            /// <returns>An instance of type <typeparamref name="T"/>[] containing the data represented by the storage entity.</returns>
            private T[] Restore<T>()
            {
                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize array elements from. Otherwise, return a fresh array instance.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<T>();

                    //
                    // Deserialize all the array elements.
                    //

                    var res = new T[_data.Length];

                    for (var i = 0; i < _data.Length; i++)
                    {
                        res[i] = _data[i].Deserialize(deserializer);
                    }

                    _data = null;

                    return res;
                }
                else
                {
                    return new T[_length];
                }
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="Array"/> storage entity to the statically typed <see cref="Wrapper{T}"/> instance.
            /// </summary>
            private interface IArrayPersistence
            {
                /// <summary>
                /// Saves the array element at the specified <paramref name="index"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the array element to.</param>
                /// <param name="index">The index of the array element to save.</param>
                void Save(ISerializerFactory serializerFactory, Stream stream, int index);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted array with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the array.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedArray<T>, IArrayPersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly Array _storage;

                /// <summary>
                /// The stored array, always reflecting the latest in-memory state.
                /// </summary>
                private readonly T[] _array;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the array.</param>
                /// <param name="storage">The storage entity representing the array.</param>
                /// <param name="array">The initial array. This could either be the result of deserializing persisted state, or an array containing default values for a new entity.</param>
                public Wrapper(string id, Array storage, T[] array)
                    : base(id)
                {
                    _storage = storage;
                    _array = array;
                }

                /// <summary>
                /// Gets or sets the element at the specified <paramref name="index"/> in the array.
                /// </summary>
                /// <param name="index">The index in the array.</param>
                /// <returns>The element at the specified <paramref name="index"/>.</returns>
                /// <exception cref="IndexOutOfRangeException">The specified <paramref name="index"/> is outside the bounds of the array.</exception>
                public T this[int index]
                {
                    get => _array[index];

                    set
                    {
                        //
                        // First try to apply the edit, allowing an exception to occur prior to having touched the storage entity.
                        //

                        _array[index] = value;

                        //
                        // Track the edit in the storage entity.
                        //

                        _storage.Edit(index);
                    }
                }

                /// <summary>
                /// Gets the length of the array.
                /// </summary>
                public int Count => _array.Length;

                /// <summary>
                /// Gets the length of the array.
                /// </summary>
                public int Length => _array.Length;

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the array.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the array.</returns>
                public IEnumerator<T> GetEnumerator()
                {
                    foreach (var item in _array)
                    {
                        yield return item;
                    }
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the array.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the array.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the array element at the specified <paramref name="index"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the array element to.</param>
                /// <param name="index">The index of the array element to save.</param>
                void IArrayPersistence.Save(ISerializerFactory serializerFactory, Stream stream, int index)
                {
                    serializerFactory.GetSerializer<T>().Serialize(_array[index], stream);
                }
            }
        }
    }
}
