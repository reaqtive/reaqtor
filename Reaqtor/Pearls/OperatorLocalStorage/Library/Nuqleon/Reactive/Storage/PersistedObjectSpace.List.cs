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
        /// Creates a persisted list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier to use for the list.</param>
        /// <returns>A new persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedList<T> CreateList<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var list = new List(this);
            _items.Add(id, list);
            return CreateListCore<T>(id, list);
        }

        /// <summary>
        /// Gets a persisted list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier of the list to retrieve.</param>
        /// <returns>An existing persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted list type.</exception>
        public IPersistedList<T> GetList<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateListCore<T>(id, (List)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the list.</typeparam>
        /// <param name="id">The identifier of the list.</param>
        /// <param name="list">The storage entity representing the list.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="list"/>.</returns>
        private static IPersistedList<T> CreateListCore<T>(string id, List list) => list.Create<T>(id);

        /// <summary>
        /// Storage entity representing the list.
        /// </summary>
        /// <remarks>
        /// Persistence of a list looks as follows:
        /// <c>
        /// metadata/length = 5
        /// items/0 = 42
        /// items/1 = 43
        /// items/2 = 44
        /// items/3 = 45
        /// items/4 = 46
        /// </c>
        /// The keys of the items are in the range [0..length), where length is mutable (cf. Add and Remove methods).
        /// <list type="bullet">
        ///   <item><c>list[i] = v</c> results in <c>Edit($"items/{i}", v)</c></item>
        ///   <item><c>list.Add(v)</c> results in <c>Edit("metadata/length", length + 1); Add($"items/{length}", v)</c></item>
        ///   <item><c>list.RemoveAt(i)</c> results in <c>Edit("metadata/length", length - 1); for (j in [i..length-1)) { Edit($"items/{j}", items[j + 1]) }; Delete($"items/{length}")</c></item>
        /// </list>
        /// </remarks>
        private sealed class List : PersistableBase
        {
            /// <summary>
            /// The category to store the metadata (i.e. the list length, using the <see cref="LengthKey"/> key) in.
            /// </summary>
            private const string MetadataCategory = "metadata";

            /// <summary>
            /// The category to store the data (i.e. the list elements) in.
            /// </summary>
            private const string ItemsCategory = "items";

            /// <summary>
            /// The key to store the length in (using the <see cref="MetadataCategory"/> category).
            /// </summary>
            private const string LengthKey = "length";

            /// <summary>
            /// The eventual objects containing the list elements, set by <see cref="LoadCore(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>. Indexes in this array range over the elements in [0..length).
            /// </summary>
            private EventualObject[] _data;

            /// <summary>
            /// The length of the list as currently saved to the key/value store (if <see cref="PersistableBase.HasSaved"/> is <c>true</c>). This value gets edited upon successful <see cref="SaveCore(IStateWriter)"/> operations (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private int _length;

            /// <summary>
            /// The state change manager used to keep track of the edits to elements in the list that have been changed since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private readonly StateChangedManager<DirtyState> _dirty = new();

            /// <summary>
            /// The pending edits obtained from <see cref="_dirty"/> on the last call to <see cref="SaveCore(IStateWriter)"/>, for use by <see cref="OnSavedCore"/> to edit <see cref="_length"/> upon a successful save operation.
            /// </summary>
            private DirtyState[] _dirtyHistory;

            /// <summary>
            /// Creates a new entity representing a list.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public List(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.List"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.List;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the list.</typeparam>
            /// <param name="id">The identifier of the list.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedList<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    var list = new Wrapper<T>(id, this, Restore<T>());
                    _wrapper = list;
                    return list;
                }
                else
                {
                    return (IPersistedList<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the list from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Remove the metadata associated with the list.
                //

                writer.DeleteItem(MetadataCategory, LengthKey);

                //
                // Remove the elements stored in items/i in the range [0..length).
                //
                // NB: Pending in-memory edits are not reflected in _length.
                //

                for (var i = 0; i < _length; i++)
                {
                    writer.DeleteItem(ItemsCategory, GetKeyForIndex(i));
                }
            }

            /// <summary>
            /// Loads the list from storage.
            /// </summary>
            /// <param name="reader">The reader to load the list from.</param>
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
            /// Saves the list to storage.
            /// </summary>
            /// <param name="writer">The writer to write the list to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Edit, RemoveAt, SetLength).");

                //
                // List element count value to be persisted in case of a full checkpoint, or when the list has not yet been persisted, or if the element count changed since the last checkpoint. Otherwise, this value will be null.
                //

                var lengthValue = default(int?);

                //
                // An ordered collection of indexes containing elements that have been edited and need to be persisted. For a full checkpoint or a new array, this collection will contain the range [0..length). This value is never null.
                //

                IEnumerable<int> editIndexes;

                //
                // The number of elements that should be removed from the tail of the persisted elements in range [newLength..oldLength). This value is null for full checkpoints and when the element count of the list hasn't changed since the last checkpoint.
                //

                var deleteCount = default(int?);

                //
                // Snapshot the dirty state (containing edits and length changes). See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                _dirtyHistory = _dirty.SaveState();

                //
                // Get the length by incorporating all in-memory edits (if any).
                //

                var length = GetLength(_dirtyHistory);

                //
                // Check if we need to save all the state in case of a full checkpoint or a new list that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    lengthValue = length;

                    var allIndexes = new int[length];

                    for (var i = 0; i < length; i++)
                    {
                        allIndexes[i] = i;
                    }

                    editIndexes = allIndexes;
                }
                else
                {
                    //
                    // If length has changed, it should be persisted. If the length has decreased, we have elements to remove at the tail end.
                    //

                    if (length != _length)
                    {
                        lengthValue = length;

                        if (length < _length)
                        {
                            deleteCount = _length - length;
                        }
                    }

                    //
                    // Use a sorted set to get deterministic behavior for testing and in-order access to the key/value store which can reduce seek times.
                    //

                    var dirtyIndexes = new SortedSet<int>();

                    foreach (var dirty in _dirtyHistory)
                    {
                        //
                        // Iterate over all the edited indexes. Note that the enumeration is in order, allowing us to break from the loop upon encountering the first edit that's beyond the current list element count (see below).
                        //

                        foreach (var entry in dirty.Edits)
                        {
                            //
                            // Filter out any edits to elements that are no longer in the list. This can happen when editing a value prior to removing it, possibly across dirty page boundaries.
                            //
                            // NB: We choose to filter out these edits at the point of creating a checkpoint rather than during edits, for two reasons:
                            //
                            //     * Only the last edit page should be considered mutable, so if an edit to an element happens on one page, and a delete happens on a more recent page, we can't prune the edit.
                            //     * Many lists have append-only behavior and those that shrink often get elements added again, resulting in unnecessary overhead induced by the bookkeeping.
                            //
                            //     Note that the maximum number of dirty index entries per edit page (kept in a set) is proportional to the maximum element count in the list, which provides an upper bound for the memory overhead.
                            //

                            if (entry < length)
                            {
                                dirtyIndexes.Add(entry);
                            }
                        }
                    }

                    editIndexes = dirtyIndexes;
                }

                //
                // If length has changed, persist it now.
                //

                if (lengthValue != null)
                {
                    using var lengthStream = writer.GetItemWriter(MetadataCategory, LengthKey);

                    GetSerializer<int>().Serialize(lengthValue.Value, lengthStream);
                }

                //
                // Save each edited element by dispatching to IListPersistence.Save which has access to the list element static type used for serialization.
                //

                var target = (IListPersistence)_wrapper;

                foreach (var index in editIndexes)
                {
                    var key = GetKeyForIndex(index);

                    using var itemStream = writer.GetItemWriter(ItemsCategory, key);

                    target.Save(SerializationFactory, itemStream, index);
                }

                //
                // If we have deletes to perform, do them last and in order. This helps with the sequential access of the underlying store.
                //

                if (deleteCount != null)
                {
                    Debug.Assert(deleteCount.Value > 0);

                    for (var i = _length - deleteCount.Value; i < _length; i++)
                    {
                        var key = GetKeyForIndex(i);

                        writer.DeleteItem(ItemsCategory, key);
                    }
                }
            }

            /// <summary>
            /// Marks the last call to <see cref="SaveCore(IStateWriter)"/> as successful.
            /// </summary>
            protected override void OnSavedCore()
            {
                Debug.Assert(_dirtyHistory != null, "Expected preceding Save call and at most one call to OnSaved.");

                //
                // Prune the edit pages that were persisted by the prior call to SaveCore.
                //

                _dirty.OnStateSaved();

                //
                // Reflect the element count that was persisted in the _length field.
                //

                _length = GetLength(_dirtyHistory);

                //
                // We no longer need the edit pages.
                //

                _dirtyHistory = null;
            }

            /// <summary>
            /// Gets the current element count of the list, incorporating any pending in-memory edits on the specified <paramref name="dirty"/> edit pages.
            /// </summary>
            /// <param name="dirty">The edit pages to consider for the element count determination.</param>
            /// <returns>The current element count of the list, including the edits on the specified <paramref name="dirty"/> edit pages.</returns>
            private int GetLength(DirtyState[] dirty)
            {
                //
                // Traverse the edit pages in reverse chronological order. The latest known element count value is returned.
                //

                for (var i = dirty.Length - 1; i >= 0; i--)
                {
                    var length = dirty[i].Length;

                    if (length != null)
                    {
                        return length.Value;
                    }
                }

                //
                // If no changes to the element count are found on any of the edit pages, return the currently persisted element count from _length.
                //

                return _length;
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}"/> when the element at the specified <paramref name="index"/> gets mutated (due to element assignment or addition).
            /// </summary>
            /// <param name="index">The index of the element in the list that got mutated.</param>
            private void Edit(int index)
            {
                //
                // Mark the state as dirty and record the edited index in the latest edit page.
                //

                StateChanged = true;

                _dirty.State.Edits.Add(index);
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}"/> when the elements between the specified <paramref name="inclusiveStartIndex"/> and <paramref name="exclusiveEndIndex"/> get mutated (due to shifting of elements due to insertion or removal).
            /// </summary>
            /// <param name="inclusiveStartIndex">The index of the first element in the list that got mutated.</param>
            /// <param name="exclusiveEndIndex">The exclusive index of the last element in the list that got mutated.</param>
            private void EditRange(int inclusiveStartIndex, int exclusiveEndIndex)
            {
                //
                // Mark the state as dirty and record the edited indexes in the latest edit page.
                //

                StateChanged = true;

                for (var index = inclusiveStartIndex; index < exclusiveEndIndex; index++)
                {
                    _dirty.State.Edits.Add(index);
                }
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}"/> when the element count of the list changes (due to element addition, insertion, removal, or cleaning of the list).
            /// </summary>
            /// <param name="length">The new length of the list.</param>
            private void SetLength(int length)
            {
                //
                // Mark the state as dirty and record the new length in the latest edit page.
                //

                StateChanged = true;

                _dirty.State.Length = length;
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory list representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a new empty list instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the list.</typeparam>
            /// <returns>An instance of type List{<typeparamref name="T"/>} containing the data represented by the storage entity.</returns>
            private List<T> Restore<T>()
            {
                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize list elements from. Otherwise, return a fresh list instance.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<T>();

                    //
                    // Deserialize all the list elements.
                    //

                    var res = new List<T>(_data.Length);

                    for (var i = 0; i < _data.Length; i++)
                    {
                        res.Add(_data[i].Deserialize(deserializer));
                    }

                    _data = null;

                    return res;
                }
                else
                {
                    return new List<T>();
                }
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="List"/> storage entity to the statically typed <see cref="Wrapper{T}"/> instance.
            /// </summary>
            private interface IListPersistence
            {
                /// <summary>
                /// Saves the list element at the specified <paramref name="index"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the list element to.</param>
                /// <param name="index">The index of the list element to save.</param>
                void Save(ISerializerFactory serializerFactory, Stream stream, int index);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted list with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the list.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedList<T>, IListPersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly List _storage;

                /// <summary>
                /// The stored list, always reflecting the latest in-memory state.
                /// </summary>
                private readonly List<T> _list;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the list.</param>
                /// <param name="storage">The storage entity representing the list.</param>
                /// <param name="list">The initial list. This could either be the result of deserializing persisted state, or an empty list for a new entity.</param>
                public Wrapper(string id, List storage, List<T> list)
                    : base(id)
                {
                    _storage = storage;
                    _list = list;
                }

                /// <summary>
                /// Gets or sets the element at the specified index.
                /// </summary>
                /// <param name="index">The zero-based index of the element to get or set.</param>
                /// <returns>The element at the specified index.</returns>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the list.</exception>
                public T this[int index]
                {
                    get => _list[index];

                    set
                    {
                        //
                        // First try to apply the edit, allowing an exception to occur prior to having touched the storage entity.
                        //

                        _list[index] = value;

                        //
                        // Track the edit in the storage entity.
                        //

                        _storage.Edit(index);
                    }
                }

                /// <summary>
                /// Gets the number of elements in the list.
                /// </summary>
                public int Count => _list.Count;

                /// <summary>
                /// Gets a value indicating whether the list is read-only. Always returns <c>false</c>.
                /// </summary>
                public bool IsReadOnly => false;

                /// <summary>
                /// Adds the specified <paramref name="item"/> to the list.
                /// </summary>
                /// <param name="item">The item to add to the end of the list.</param>
                public void Add(T item)
                {
                    //
                    // First add the element to the list. Note the logic below depends on this order for the calculation of the index and new element count.
                    //

                    _list.Add(item);

                    //
                    // Get the new element count and track the new length and the index of the added element in the storage entity.
                    //

                    var count = Count;
                    _storage.Edit(count - 1);
                    _storage.SetLength(count);
                }

                /// <summary>
                /// Clears the list.
                /// </summary>
                public void Clear()
                {
                    //
                    // Apply the change to the list instance. For consistency, we do this prior to updating the storage entity.
                    //

                    _list.Clear();

                    //
                    // Track the new (zero) length in the storage entity.
                    //
                    // NB: This suffices for all elements to get removed in the persistent store; the storage entity will take care of removing all elements at the end of the list that are no longer in use.
                    //

                    _storage.SetLength(0);
                }

                /// <summary>
                /// Checks whether the list contains the specified <paramref name="item"/>.
                /// </summary>
                /// <param name="item">The item to find in the list.</param>
                /// <returns><c>true</c> if the list contains the specified <paramref name="item"/>; otherwise, <c>false</c>.</returns>
                public bool Contains(T item) => _list.Contains(item);

                /// <summary>
                /// Copies the elements in the list to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
                /// </summary>
                /// <param name="array">The array to copy the list elements to.</param>
                /// <param name="arrayIndex">The index in the array to copy the first element to.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
                /// <exception cref="ArgumentException">The number of elements in the list is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
                public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the list.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the list.</returns>
                public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

                /// <summary>
                /// Gets the index of the specified <paramref name="item"/> in the list.
                /// </summary>
                /// <param name="item">The item to retrieve the index for.</param>
                /// <returns>The index of the item in the list, if found; otherwise, a negative value.</returns>
                public int IndexOf(T item) => _list.IndexOf(item);

                /// <summary>
                /// Inserts the specified <paramref name="item"/> at the specified <paramref name="index"/> in the list.
                /// </summary>
                /// <param name="index">The index in the list to insert the specified <paramref name="item"/>.</param>
                /// <param name="item">The item to insert at the specified <paramref name="index"/>.</param>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0, or <paramref name="index"/> is greater than <see cref="List{T}.Count"/>.</exception>
                public void Insert(int index, T item)
                {
                    //
                    // First insert the element in the list, allowing an exception to occur prior to having touched the storage entity.
                    //

                    _list.Insert(index, item);

                    //
                    // Track the change of list length in the storage entity.
                    //

                    var count = Count;
                    _storage.SetLength(count);

                    //
                    // Insertion causes all elements beyond the specified index to be shifted, so all these storage slots have to be dirtied. Obviously, the storage slot containing the inserted element is dirty too.
                    //

                    _storage.EditRange(index, count); // [index, count)
                }

                /// <summary>
                /// Removes the specified <paramref name="item"/> from the list, if found.
                /// </summary>
                /// <param name="item">The item to remove from the list.</param>
                /// <returns><c>true</c> if the specified <paramref name="item"/> was found in the list and got removed; otherwise, <c>false</c>.</returns>
                public bool Remove(T item)
                {
                    var index = IndexOf(item);

                    if (index >= 0)
                    {
                        RemoveAt(index);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Removes the item in the list at the specified <paramref name="index"/>.
                /// </summary>
                /// <param name="index">The index of the item in the list to remove.</param>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0, or <paramref name="index"/> is greater than <see cref="List{T}.Count"/>.</exception>
                public void RemoveAt(int index)
                {
                    //
                    // First remove the element from the list, allowing an exception to occur prior to having touched the storage entity.
                    //

                    _list.RemoveAt(index);

                    //
                    // Track the change of list length in the storage entity.
                    //

                    var count = Count;
                    _storage.SetLength(count);

                    //
                    // Removal causes all elements beyond the specified index (if the index didn't refer to the last element) to be shifted, so all these storage slots have to be dirtied.
                    //

                    if (index < count)
                    {
                        _storage.EditRange(index, count); // [index, count)
                    }
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the list.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the list.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the list element at the specified <paramref name="index"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the list element to.</param>
                /// <param name="index">The index of the list element to save.</param>
                void IListPersistence.Save(ISerializerFactory serializerFactory, Stream stream, int index)
                {
                    serializerFactory.GetSerializer<T>().Serialize(_list[index], stream);
                }
            }

            /// <summary>
            /// State kept on dirty pages to keep track of all edits that have to be persisted in the next checkpoint.
            /// </summary>
            private sealed class DirtyState
            {
                /// <summary>
                /// Set containing the indexes of the elements in the list that have been edited.
                /// </summary>
                public readonly HashSet<int> Edits = new();

                /// <summary>
                /// The element count of the list. This value is <c>null</c> when the element count hasn't changed (due to calls to Add or Remove).
                /// </summary>
                public int? Length;
            }
        }
    }
}
