// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted linked list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier to use for the linked list.</param>
        /// <returns>A new persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedLinkedList<T> CreateLinkedList<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var list = new LinkedList(this);
            _items.Add(id, list);
            return CreateLinkedListCore<T>(id, list);
        }

        /// <summary>
        /// Gets a persisted linked list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier of the linked list to retrieve.</param>
        /// <returns>An existing persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted linked list type.</exception>
        public IPersistedLinkedList<T> GetLinkedList<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateLinkedListCore<T>(id, (LinkedList)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified linked <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier of the linked list.</param>
        /// <param name="list">The storage entity representing the linked list.</param>
        /// <returns>A statically typed wrapper around the specified linked <paramref name="list"/>.</returns>
        private static IPersistedLinkedList<T> CreateLinkedListCore<T>(string id, LinkedList list) => list.Create<T>(id);

        // REVIEW: Complete the docs below with more Add and Remove methods.

        /// <summary>
        /// Storage entity representing the linked list.
        /// </summary>
        /// <remarks>
        /// Persistence of a linked list looks as follows:
        /// <c>
        /// items/0/data = 42
        /// items/0/metadata = { prev: -1, next:  1 }
        /// items/1/data = 43
        /// items/1/metadata = { prev:  0, next:  2 }
        /// items/2/data = 44
        /// items/2/metadata = { prev:  1, next:  3 }
        /// items/3/data = 45
        /// items/3/metadata = { prev:  2, next:  4 }
        /// items/4/data = 46
        /// items/4/metadata = { prev:  3, next: -1 }
        /// </c>
        /// The keys of the items are insignificant due to the encoding of the linked list node order in the metadata entries.
        /// Metadata entries contain the key of the previous and the next node. This strategy allows for insertion and removal of nodes by only touching metadata entries of adjacent nodes.
        /// <list type="bullet">
        ///   <item><c>node.Value = v</c> results in <c>Edit($"items/{getKeyOf(node)}/data", v)</c></item>
        ///   <item><c>list.AddAfter(o, v)</c> results in <c>let newKey = getUnusedKey() in Edit("items/{getKeyOf(o)}/metadata", $"prev = {getKeyOf(o.Previous)}, next = {newKey}"); Edit("items/{getKeyOf(o.Next)}/metadata", $"prev = {newKey}, next = {getKeyOf(o.Next.Next)}"); Add($"items/{newKey}/data", v); Add($"items/{newKey}/metadata", $"prev = {getKeyOf(o)}, next = {getKeyOf(o.Next)}")</c></item>
        /// </list>
        /// </remarks>
        private sealed class LinkedList : HeapBase
        {
            /// <summary>
            /// The category to store the metadata (i.e. the references to the previous and next node for each linked list node) in.
            /// </summary>
            private const string MetadataCategory = "metadata";

            /// <summary>
            /// The category to store the data (i.e. the value for each linked list node) in.
            /// </summary>
            private const string DataCategory = "data";

            /// <summary>
            /// The list of linked list node representations, set by <see cref="LoadCore(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>. Each entry contains the storage key, the eventual object containing the value, and the eventual object containing the metadata.
            /// </summary>
            private List<(long key, EventualObject data, EventualObject metadata)> _data;

            /// <summary>
            /// The state change manager used to keep track of edits applied to linked list nodes since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            /// <remarks>
            /// The base class has a <see cref="HeapBase._dirty"/> state manager as well, used to keep track of additions and removals. This derived type's state manager is added on top to keep track of <see cref="NodeEditKind"/> edit kinds.
            /// </remarks>
            private readonly StateChangedManager<DirtyState> _dirty = new();

            /// <summary>
            /// The pending edits obtained from <see cref="_dirty"/> on the last call to <see cref="SaveCore(IStateWriter)"/>, for use by <see cref="SaveItemCore(IStateWriter, string, long)"/> to look up up the edits associated with a storage key.
            /// </summary>
            private DirtyState[] _dirtyHistory;

            /// <summary>
            /// Creates a new entity representing a linked list.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public LinkedList(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.LinkedList"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.LinkedList;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
            /// <param name="id">The identifier of the linked list.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedLinkedList<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    //
                    // NB: Restoring a linked list to an in-memory data structure returns both the linked list (of type LinkedList<T>) and the map of linked list nodes to keys (of type Map<LinkedListNode<T>, long>).
                    //

                    var (list, storageKeys) = Restore<T>();

                    var wrapper = new Wrapper<T>(id, this, list, storageKeys);
                    _wrapper = wrapper;
                    return wrapper;
                }
                else
                {
                    return (IPersistedLinkedList<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the linked list node with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operation to.</param>
            /// <param name="key">The key of the linked list node to remove.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void DeleteItemCore(IStateWriter writer, string key, long keyValue)
            {
                //
                // Remove the item in both the data and metadata collection.
                //

                writer.DeleteItem(MetadataCategory, key);
                writer.DeleteItem(DataCategory, key);
            }

            /// <summary>
            /// Loads the linked list from storage.
            /// </summary>
            /// <param name="reader">The reader to load the linked list from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Prepare the storage for eventual objects.
                //

                _data = new List<(long key, EventualObject data, EventualObject metadata)>();

                //
                // Perform the core load operations, which will result in calls to TryGetItemKeysCore and LoadItemCore.
                //

                base.LoadCore(reader);

                //
                // We may never be asked to deserialize the linked list elements, so let's try to be space efficient.
                //
                // NB: The design assumption right now is that the call to Load is our only chance to read from the key/value store, so we have to keep eventual objects around.
                //     However, the caller of Load could have more awareness of whether the entity will (eventually) be used, thus allowing it to prevent unnecessary loads.
                //

                _data.TrimExcess();
            }

            /// <summary>
            /// Gets the list of keys for the nodes in the linked list.
            /// </summary>
            /// <param name="reader">The reader to obtain the list of keys from.</param>
            /// <param name="keys">The keys of the nodes in the linked list.</param>
            /// <returns><c>true</c> if items were found and returned in <paramref name="keys"/>; otherwise, <c>false</c>.</returns>
            protected override bool TryGetItemKeysCore(IStateReader reader, out IEnumerable<string> keys)
            {
                //
                // Get both the keys and the metadata keys to assert both sets match.
                //
                // REVIEW: Do we need a rigorous assert at this stage?
                //

                var dataFound = reader.TryGetItemKeys(DataCategory, out var dataKeys);
                var metadataFound = reader.TryGetItemKeys(MetadataCategory, out var metadataKeys);

                Invariant.Assert(dataFound == metadataFound, "Expected metadata and data keys to both exist or not exist.");

                //
                // Assert both sets are equal.
                //
                // NB: We can't assert the sets are non-empty; an empty list is represented as an empty heap.
                //

                if (dataFound)
                {
                    var dataKeysSet = new HashSet<string>(dataKeys);
                    var metadataKeysSet = new HashSet<string>(metadataKeys);

                    Invariant.Assert(dataKeysSet.SetEquals(metadataKeysSet), "Expected metadata and data keys to match.");

                    keys = dataKeys;
                    return true;
                }

                keys = null;
                return false;
            }

            /// <summary>
            /// Loads the linked list node with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="reader">The reader to load the item from.</param>
            /// <param name="key">The key of the linked list node to load.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void LoadItemCore(IStateReader reader, string key, long keyValue)
            {
                //
                // Create an entry in _data containing the key, and the eventual object for the data, and the eventual object for the metadata, for deferred deserialization in Restore<T>.
                //

                EventualObject data, metadata;

                using (var itemStream = reader.GetItemReader(DataCategory, key))
                {
                    data = EventualObject.FromState(itemStream);
                }

                using (var itemStream = reader.GetItemReader(MetadataCategory, key))
                {
                    metadata = EventualObject.FromState(itemStream);
                }

                _data.Add((key: keyValue, data, metadata));
            }

            /// <summary>
            /// Saves the linked list to storage.
            /// </summary>
            /// <param name="writer">The writer to write the linked list to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                //
                // Snapshot the dirty state (containing edits of linked list nodes). See SaveItemCore for the use of this field to look up the edit kind. See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                _dirtyHistory = _dirty.SaveState();

                //
                // Next, let the base class perform its save operations. This will dispatch into SaveItemCore.
                //

                base.SaveCore(writer);
            }

            /// <summary>
            /// Saves the linked list node with the specified <paramref name="key"/> to storage.
            /// </summary>
            /// <param name="writer">The writer to apply the save operation to.</param>
            /// <param name="key">The key of the linked list node to save.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void SaveItemCore(IStateWriter writer, string key, long keyValue)
            {
                Debug.Assert(_dirtyHistory != null, "Expected prior call to SaveCore.");

                //
                // The final edit reflects what we have to do for the item (i.e. saving metadata and/or data).
                //

                var finalEdit = default(NodeEditKind);

                //
                // Check if we need to save all the state in case of a full checkpoint or a new heap that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    finalEdit = NodeEditKind.All;
                }
                else
                {
                    //
                    // Compute the final edit by unioning the edits on all the dirty pages that were snapshotted.
                    //
                    // NB: We use reverse chronological traversal for consistency with other persisted entities, even though the order doesn't really matter here.
                    //

                    for (var i = _dirtyHistory.Length - 1; i >= 0; i--)
                    {
                        if (_dirtyHistory[i].Edits.TryGetValue(keyValue, out var edit))
                        {
                            finalEdit |= edit;

                            //
                            // Can bail out early if we have to do "the works" anyway.
                            //

                            if (finalEdit == NodeEditKind.All)
                            {
                                break;
                            }
                        }
                    }

                    Debug.Assert(finalEdit != default, "Expected a non-trivial edit value.");
                }

                //
                // Cast the wrapper to ILinkedListPersistence to dispatch into save logic for linked list nodes indexed by storage key.
                //

                var target = (ILinkedListPersistence)_wrapper;

                //
                // Cache the serializer for references.
                //

                var keyValuePairSerializer = SerializationFactory.GetSerializer<KeyValuePair<long, long>>();

                //
                // Edit the metadata, or the data, or both.
                //

                if ((finalEdit & NodeEditKind.Metadata) != default)
                {
                    var (previous, next) = target.GetPreviousAndNextReferences(keyValue);

                    using var stream = writer.GetItemWriter(MetadataCategory, key);

                    keyValuePairSerializer.Serialize(new KeyValuePair<long, long>(previous, next), stream);
                }

                if ((finalEdit & NodeEditKind.Value) != default)
                {
                    using var stream = writer.GetItemWriter(DataCategory, key);

                    target.SaveValue(SerializationFactory, stream, keyValue);
                }
            }

            /// <summary>
            /// Marks the last call to <see cref="SaveCore(IStateWriter)"/> as successful.
            /// </summary>
            protected override void OnSavedCore()
            {
                Debug.Assert(_dirtyHistory != null, "Expected preceding Save call and at most one call to OnSaved.");

                //
                // Ensure the base class can prune its edit pages as well.
                //

                base.OnSavedCore();

                //
                // Prune the edit pages that were persisted by the prior call to SaveCore.
                //

                _dirty.OnStateSaved();

                //
                // We no longer need the edit pages.
                //

                _dirtyHistory = null;
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory linked list representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a new empty linked list instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
            /// <returns>A pair consisting of an instance of type LinkedList{<typeparamref name="T"/>} containing the data represented by the storage entity, and a map that associates all linked list nodes with their storage key.</returns>
            private (LinkedList<T>, Map<LinkedListNode<T>, long> storageKeys) Restore<T>()
            {
                var list = new LinkedList<T>();
                var storageKeys = new Map<LinkedListNode<T>, long>(EqualityComparer<LinkedListNode<T>>.Default, EqualityComparer<long>.Default);

                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize linked list nodes from. Otherwise, return a fresh linked list instance.
                //

                if (_data != null)
                {
                    //
                    // For an empty linked list, we can just return a fresh empty instance. This avoids extra allocations and having to do extra bookkeeping with regards to finding the head node.
                    //

                    if (_data.Count > 0)
                    {
                        //
                        // We need to find the head node and tail nodes which use -1 for their previous and next references. Use nullable fields to assert having found these exactly once.
                        //

                        long? headKey = null;
                        long? tailKey = null;

                        //
                        // For the first phase of recovering the list, we'll keep a dictionary to map storage keys on the deserialized data and metadata. Once we have found the head, we can use this to reconstruct the order.
                        //

                        var nodesByKey = new Dictionary<long, (long previous, long next, T value)>();

                        //
                        // Obtain the deserializers once to reduce overhead.
                        //

                        var dataDeserializer = SerializationFactory.GetDeserializer<T>();
                        var metadataDeserializer = SerializationFactory.GetDeserializer<KeyValuePair<long, long>>();

                        //
                        // Deserialize all the list elements (including the data and the metadata) into `nodesByKey`.
                        //

                        foreach (var (key, data, metadata) in _data)
                        {
                            //
                            // Deserialize the node value.
                            //

                            var value = data.Deserialize(dataDeserializer);

                            //
                            // Deserialize the node previous and next references.
                            //

                            var nodeRefs = metadata.Deserialize(metadataDeserializer);
                            var previous = nodeRefs.Key;
                            var next = nodeRefs.Value;

                            //
                            // Check if this node is the head or the tail and assert we haven't found these yet.
                            //

                            if (previous == -1L)
                            {
                                Invariant.Assert(!headKey.HasValue, "Duplicate head node found.");
                                headKey = key;
                            }

                            if (next == -1L)
                            {
                                Invariant.Assert(!tailKey.HasValue, "Duplicate tail node found.");
                                tailKey = key;
                            }

                            //
                            // Add an entry to the intermediate dictionary, using Add to guard against adding duplicates.
                            //

                            nodesByKey.Add(key, (previous, next, value));
                        }

                        Debug.Assert(nodesByKey.Count == _data.Count, "All entries from `_data` should end up in `nodesByKey`."); // NB: Debug assert suffices here; this is a static invariant of the algorithm above.

                        //
                        // Assert we have found the head and the tail.
                        //

                        Invariant.Assert(headKey.HasValue, "Expected to find head node.");
                        Invariant.Assert(tailKey.HasValue, "Expected to find tail node.");

                        //
                        // For the second phase of recovering the list, we'll traverse through the dictionary from head to tail to build the linked list and a map from linked list node to storage key.
                        //

                        var previousKey = -1L;
                        var currentKey = headKey.Value;

                        while (currentKey != -1L)
                        {
                            //
                            // Ensure we're not encountering a cycle by checking `map` for the key.
                            //
                            // NB: We don't remove entries from the `nodesByKey` dictionary to reduce overheads. An alternative strategy to using `map` for cycle detection is to remove entries from `nodesByKey` and rely on
                            //     failed lookups to prevent cycles. However, this makes it harder to distinguish between nodes missing from storage and cycles. We need to build `map` anyway, so let's use it.
                            //

                            if (storageKeys.ContainsValue(currentKey))
                            {
                                throw new InvalidOperationException(FormattableString.Invariant($"Cycle detected for node with storage key '{currentKey}', referenced from the previous node with storage key '{previousKey}'."));
                            }

                            //
                            // Retrieve the entry from the dictionary using Using TryGetValue to provide more meaningful exception messages if we can't find the key due to a storage inconsistency.
                            //

                            if (!nodesByKey.TryGetValue(currentKey, out var t))
                            {
                                throw new InvalidOperationException(FormattableString.Invariant($"Expected to find node for storage key '{currentKey}'."));
                            }

                            //
                            // Deconstruct the tuple and assert that the previous storage key reference matches.
                            //

                            var (previous, next, value) = t;

                            if (previous != previousKey)
                            {
                                throw new InvalidOperationException(FormattableString.Invariant($"Previous key '{previous}' found in metadata storage for the node with key '{currentKey}' does not match the next key '{previousKey}' of the predecessor node."));
                            }

                            //
                            // Add the value to the linked list and map of the linked list node to the storage key.
                            //

                            var node = list.AddLast(value);
                            storageKeys.Add(node, currentKey);

                            //
                            // Advance the loop while keeping track of the previous key in order to be able to assert consistency of node references.
                            //

                            previousKey = currentKey;
                            currentKey = next;
                        }

                        //
                        // Assert we've reached the end of the list.
                        //

                        Invariant.Assert(previousKey == tailKey.Value, "Expected to finish recovery at tail node.");

                        //
                        // All entries from the dictionary should be processed.
                        //

                        if (list.Count != nodesByKey.Count)
                        {
                            throw new InvalidOperationException(FormattableString.Invariant($"Linked list restoration processed '{list.Count}' nodes while '{nodesByKey.Count}' nodes were expected."));
                        }
                    }

                    _data = null;
                }

                return (list, storageKeys);
            }

            /// <summary>
            /// Called when the linked list is cleared.
            /// </summary>
            /// <param name="keys">The list of storage keys associated with the elements that were removed.</param>
            private void ClearKeys(IEnumerable<long> keys) => base.Clear(new SortedSet<long>(keys));

            /// <summary>
            /// Called when an element in the linked list associated with the specified <paramref name="key"/> is edited.
            /// </summary>
            /// <param name="key">The storage key associated with the element that was edited.</param>
            /// <param name="edit">The kind of edit that was applied to the element.</param>
            private void Edit(long key, NodeEditKind edit)
            {
                //
                // Use a call to the base class Edit method to keep track of the presence of an edit. This takes care of setting StateChanged as well.
                //

                Edit(key);

                //
                // Keep track of the specific edit kind in our own state change manager.
                //

                _dirty.State.Edit(key, edit);
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="LinkedList"/> storage entity to the statically typed linked list node wrapper instance of type <see cref="Wrapper{T}.NodeWrapper"/>.
            /// </summary>
            private interface ILinkedListPersistence
            {
                /// <summary>
                /// Saves the value of the linked list node with the specified storage <paramref name="key"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the value of the linked list node element to.</param>
                /// <param name="key">The storage key of the linked list node to save.</param>
                void SaveValue(ISerializerFactory serializerFactory, Stream stream, long key);

                /// <summary>
                /// Gets the previous and next references of the linked list node with the specified storage <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The storage key of the linked list node whose previous and next node's storage keys to retrieve.</param>
                /// <returns>A pair of storage keys corresponding to the previous and the next linked list node.</returns>
                (long previous, long next) GetPreviousAndNextReferences(long key);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted linked list with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedLinkedList<T>, ILinkedListPersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly LinkedList _storage;

                /// <summary>
                /// The stored linked list, always reflecting the latest in-memory state.
                /// </summary>
                private readonly LinkedList<T> _list;

                /// <summary>
                /// The bi-directional map used to associate the storage key used for each linked list node in <see cref="_list"/>.
                /// </summary>
                private readonly Map<LinkedListNode<T>, long> _storageKeys;

                /// <summary>
                /// Weakly rooted association of nodes in the underlying linked list (see <see cref="_list"/>) and our wrappers, used to reduce allocation overhead and to ensure reference equality for the same wrapped node.
                /// </summary>
                private readonly ConditionalWeakTable<LinkedListNode<T>, NodeWrapper> _nodeCache = new();

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the linked list.</param>
                /// <param name="storage">The storage entity representing the linked list.</param>
                /// <param name="list">The initial linked list. This could either be the result of deserializing persisted state, or an empty linked list for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each linked list node in <paramref name="list"/>.</param>
                public Wrapper(string id, LinkedList storage, LinkedList<T> list, Map<LinkedListNode<T>, long> storageKeys)
                    : base(id)
                {
                    _storage = storage;
                    _list = list;
                    _storageKeys = storageKeys;
                }

                /// <summary>
                /// Gets the number of elements in the linked list.
                /// </summary>
                public int Count => _list.Count;

                /// <summary>
                /// Gets a value indicating whether the linked list is read-only. Always returns <c>false</c>.
                /// </summary>
                public bool IsReadOnly => false;

                /// <summary>
                /// Gets the first node of the linked list, or <c>null</c> if the linked list is empty.
                /// </summary>
                public ILinkedListNode<T> First => CreateWrapper(_list.First);

                /// <summary>
                /// Gets the last node of the linked list, or <c>null</c> if the linked list is empty.
                /// </summary>
                public ILinkedListNode<T> Last => CreateWrapper(_list.Last);

                /// <summary>
                /// Gets the first node of the linked list, or <c>null</c> if the linked list is empty.
                /// </summary>
                IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

                /// <summary>
                /// Gets the last node of the linked list, or <c>null</c> if the linked list is empty.
                /// </summary>
                IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

                /// <summary>
                /// Adds the specified <paramref name="item"/> to the linked list.
                /// </summary>
                /// <param name="item">The item to add to the end of the linked list.</param>
                public void Add(T item) => AddLast(item);

                /// <summary>
                /// Adds a new node containing the specified <paramref name="value"/> after the specified existing <paramref name="node"/> in the linked list.
                /// </summary>
                /// <param name="node">The node after which to insert a new node containing <paramref name="value"/>.</param>
                /// <param name="value">The value to add to the linked list.</param>
                /// <returns>The new node containing <paramref name="value"/>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list.</exception>
                public ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));

                    //
                    // First unwrap the node. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node belongs to the current list. LinkedList<T>.AddAfter will do that.
                    //

                    var wrappedNode = GetWrapper(node);

                    //
                    // Perform the addition on the in-memory collection and create a wrapper.
                    //

                    var newNode = _list.AddAfter(wrappedNode.Node, value);

                    //
                    // If we get here, the addition was successful. Keep track of the new node and the edits to adjacent nodes, and return the wrapper.
                    //

                    return OnInserted(newNode);
                }

                /// <summary>
                /// Adds the specified new node after the specified existing <paramref name="node"/> in the linked list.
                /// </summary>
                /// <param name="node">The node after which to insert <paramref name="newNode"/>.</param>
                /// <param name="newNode">The new node to add to the linked list.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>, or <paramref name="newNode"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list,  or <paramref name="newNode"/> belongs to another linked list.</exception>
                public void AddAfter(ILinkedListNode<T> node, ILinkedListNode<T> newNode)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));
                    if (newNode == null)
                        throw new ArgumentNullException(nameof(newNode));

                    //
                    // First unwrap the nodes. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node and newNode belong to the current list, nor whether the newNode does not have an owner. LinkedList<T>.AddAfter will do that.
                    //

                    var wrappedNode = GetWrapper(node);
                    var wrappedNewNode = GetWrapper(newNode);

                    //
                    // Perform the addition on the in-memory collection.
                    //

                    _list.AddAfter(wrappedNode.Node, wrappedNewNode.Node);

                    //
                    // Keep track of the edits to adjacent nodes.
                    //

                    OnInserted(wrappedNewNode);
                }

                /// <summary>
                /// Adds a new node containing the specified <paramref name="value"/> before the specified existing <paramref name="node"/> in the linked list.
                /// </summary>
                /// <param name="node">The node before which to insert a new node containing <paramref name="value"/>.</param>
                /// <param name="value">The value to add to the linked list.</param>
                /// <returns>The new node containing <paramref name="value"/>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list.</exception>
                public ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));

                    //
                    // First unwrap the node. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node belongs to the current list. LinkedList<T>.AddBefore will do that.
                    //

                    var wrappedNode = GetWrapper(node);

                    //
                    // Perform the addition on the in-memory collection and create a wrapper.
                    //

                    var newNode = _list.AddBefore(wrappedNode.Node, value);

                    //
                    // If we get here, the addition was successful. Keep track of the new node and the edits to adjacent nodes, and return the wrapper.
                    //

                    return OnInserted(newNode);
                }

                /// <summary>
                /// Adds the specified new node before the specified existing <paramref name="node"/> in the linked list.
                /// </summary>
                /// <param name="node">The node before which to insert <paramref name="newNode"/>.</param>
                /// <param name="newNode">The new node to add to the linked list.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>, or <paramref name="newNode"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list,  or <paramref name="newNode"/> belongs to another linked list.</exception>
                public void AddBefore(ILinkedListNode<T> node, ILinkedListNode<T> newNode)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));
                    if (newNode == null)
                        throw new ArgumentNullException(nameof(newNode));

                    //
                    // First unwrap the nodes. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node and newNode belong to the current list, nor whether the newNode does not have an owner. LinkedList<T>.AddBefore will do that.
                    //

                    var wrappedNode = GetWrapper(node);
                    var wrappedNewNode = GetWrapper(newNode);

                    //
                    // Perform the addition on the in-memory collection.
                    //

                    _list.AddBefore(wrappedNode.Node, wrappedNewNode.Node);

                    //
                    // Keep track of the edits to adjacent nodes.
                    //

                    OnInserted(wrappedNewNode);
                }

                /// <summary>
                /// Adds a new node containing the specified <paramref name="value"/> at the start of the linked list.
                /// </summary>
                /// <param name="value">The value to add at the start of the linked list.</param>
                /// <returns>The new node containing the <paramref name="value"/>.</returns>
                public ILinkedListNode<T> AddFirst(T value)
                {
                    //
                    // Perform the edit on the underlying list. This will return the new node.
                    //

                    var newNode = _list.AddFirst(value);

                    //
                    // Wrap the new node and keep track of its creation and insertion into the list.
                    //

                    return OnInserted(newNode);
                }

                /// <summary>
                /// Adds the specified new <paramref name="node"/> at the start of the linked list.
                /// </summary>
                /// <param name="node">The new node to add at the start of the linked list.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another linked list.</exception>
                public void AddFirst(ILinkedListNode<T> node)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));

                    //
                    // First unwrap the node. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node belongs to the current list. LinkedList<T>.AddFirst will do that.
                    //

                    var wrappedNode = GetWrapper(node);

                    //
                    // Perform the edit on the underlying list.
                    //

                    _list.AddFirst(wrappedNode.Node);

                    //
                    // Keep track of the edits to adjacent nodes.
                    //

                    OnInserted(wrappedNode);
                }

                /// <summary>
                /// Adds a new node containing the specified <paramref name="value"/> at the end of the linked list.
                /// </summary>
                /// <param name="value">The value to add at the end of the linked list.</param>
                /// <returns>The new node containing the <paramref name="value"/>.</returns>
                public ILinkedListNode<T> AddLast(T value)
                {
                    //
                    // Perform the edit on the underlying list. This will return the new node.
                    //

                    var newNode = _list.AddLast(value);

                    //
                    // Wrap the new node and keep track of its creation and insertion into the list.
                    //

                    return OnInserted(newNode);
                }

                /// <summary>
                /// Adds the specified new <paramref name="node"/> at the end of the linked list.
                /// </summary>
                /// <param name="node">The new node to add at the end of the linked list.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another linked list.</exception>
                public void AddLast(ILinkedListNode<T> node)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));

                    //
                    // First unwrap the node. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node belongs to the current list. LinkedList<T>.AddLast will do that.
                    //

                    var wrappedNode = GetWrapper(node);

                    //
                    // Perform the edit on the underlying list.
                    //

                    _list.AddLast(wrappedNode.Node);

                    //
                    // Keep track of the edits to adjacent nodes.
                    //

                    OnInserted(wrappedNode);
                }

                /// <summary>
                /// Clears the linked list.
                /// </summary>
                public void Clear()
                {
                    //
                    // Get all the keys currently in use. This is more efficient than having the storage entity apply all edit pages to _keys to derive this set.
                    //

                    var keys = new List<long>(_storageKeys.Values);

                    //
                    // Clear the underlying list and the map containing the storage keys.
                    //

                    _list.Clear();
                    _storageKeys.Clear();

                    //
                    // Mark all the keys as deleted in the storage entity.
                    //

                    _storage.ClearKeys(keys);
                }

                /// <summary>
                /// Checks whether the linked list contains the specified <paramref name="item"/>.
                /// </summary>
                /// <param name="item">The item to find in the linked list.</param>
                /// <returns><c>true</c> if the linked list contains the specified <paramref name="item"/>; otherwise, <c>false</c>.</returns>
                public bool Contains(T item) => _list.Contains(item);

                /// <summary>
                /// Copies the elements in the linked list to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
                /// </summary>
                /// <param name="array">The array to copy the linked list elements to.</param>
                /// <param name="arrayIndex">The index in the array to copy the first element to.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
                /// <exception cref="ArgumentException">The number of elements in the linked  list is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
                public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

                /// <summary>
                /// Finds the first node that contains the specified <paramref name="value"/>.
                /// </summary>
                /// <param name="value">The value to locate in the linked list.</param>
                /// <returns>The first node that contains the specified <paramref name="value"/>, if found; otherwise, <c>null</c>.</returns>
                public ILinkedListNode<T> Find(T value) => CreateWrapper(_list.Find(value));

                /// <summary>
                /// Finds the last node that contains the specified <paramref name="value"/>.
                /// </summary>
                /// <param name="value">The value to locate in the linked list.</param>
                /// <returns>The last node that contains the specified <paramref name="value"/>, if found; otherwise, <c>null</c>.</returns>
                public ILinkedListNode<T> FindLast(T value) => CreateWrapper(_list.FindLast(value));

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the linked list.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the linked list.</returns>
                public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

                /// <summary>
                /// Removes the specified <paramref name="item"/> from the linked list, if found.
                /// </summary>
                /// <param name="item">The item to remove from the linked list.</param>
                /// <returns><c>true</c> if the specified <paramref name="item"/> was found in the linked list and got removed; otherwise, <c>false</c>.</returns>
                public bool Remove(T item)
                {
                    //
                    // Find the item and forward the node to the more primitive Remove operation, if found.
                    //

                    var node = Find(item);

                    if (node != null)
                    {
                        Remove(node);

                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Removes the specified <paramref name="node"/> from the linked list.
                /// </summary>
                /// <param name="node">The node to remove from the linked list.</param>
                /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
                /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current list.</exception>
                public void Remove(ILinkedListNode<T> node)
                {
                    if (node == null)
                        throw new ArgumentNullException(nameof(node));

                    //
                    // First unwrap the node. If this fails, there's no point in proceeding.
                    //
                    // NB: We don't check whether node belongs to the current list. LinkedList<T>.Remove will do that.
                    //

                    var wrappedNode = GetWrapper(node);

                    //
                    // Keep track of the previous and next nodes; once we update the underlying list, we won't have a clue about these anymore.
                    //

                    var previous = wrappedNode.Previous;
                    var next = wrappedNode.Next;

                    //
                    // Remove the node from the underlying list.
                    //

                    _list.Remove(wrappedNode.Node);

                    //
                    // If we get here, the removal was successful. Update the storage.
                    //

                    OnRemoved(wrappedNode, previous, next);
                }

                /// <summary>
                /// Removes the node at the start of the linked list.
                /// </summary>
                /// <exception cref="InvalidOperationException">The linked list is empty.</exception>
                public void RemoveFirst()
                {
                    //
                    // Keep track of the original first node and its successor; once we update the underlying list, we won't have a clue about these anymore.
                    //
                    // NB: We guard against null here and omit a check for an empty list ourselves; the call to LinkedList<T>.RemoveFirst will do that.
                    //

                    var first = GetWrapper(First);
                    var next = first?.Next;

                    //
                    // Remove the first element from the underlying list.
                    //

                    _list.RemoveFirst();

                    //
                    // If we get here, the removal was successful. Update the storage.
                    //

                    OnRemoved(first, previous: null, next);
                }

                /// <summary>
                /// Removes the node at the end of the linked list.
                /// </summary>
                /// <exception cref="InvalidOperationException">The linked list is empty.</exception>
                public void RemoveLast()
                {
                    //
                    // Keep track of the original last node and its predecessor; once we update the underlying list, we won't have a clue about these anymore.
                    //
                    // NB: We guard against null here and omit a check for an empty list ourselves; the call to LinkedList<T>.RemoveLast will do that.
                    //

                    var last = GetWrapper(Last);
                    var previous = last?.Previous;

                    //
                    // Remove the last element from the underlying list.
                    //

                    _list.RemoveLast();

                    //
                    // If we get here, the removal was successful. Update the storage.
                    //

                    OnRemoved(last, previous, next: null);
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the linked list.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the linked list.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Gets the wrapper for the specified in-memory linked list <paramref name="node"/>.
                /// </summary>
                /// <param name="node">The persisted linked list node.</param>
                /// <returns>The in-memory linked list node wrapper corresponding to the specified persisted linked list <paramref name="node"/>.</returns>
                private static NodeWrapper GetWrapper(ILinkedListNode<T> node)
                {
                    if (node == null)
                    {
                        return null;
                    }

                    return (NodeWrapper)node;
                }

                /// <summary>
                /// Wrap an in-memory linked list node in a persisted linked list node.
                /// </summary>
                /// <param name="node">The in-memory linked list node.</param>
                /// <returns>The persisted linked list node corresponding to the specified in-memory linked list <paramref name="node"/>.</returns>
                private NodeWrapper CreateWrapper(LinkedListNode<T> node)
                {
                    if (node == null)
                    {
                        return null;
                    }

                    if (_nodeCache.TryGetValue(node, out var wrapper))
                    {
                        return wrapper;
                    }

                    return _nodeCache.GetValue(node, CreateWrapperCore);
                }

                /// <summary>
                /// Creates a new wrapper for an in-memory linked list node, used by <see cref="_nodeCache"/> when starting to track a new node.
                /// </summary>
                /// <param name="node">The in-memory linked list node.</param>
                /// <returns>The persisted linked list node corresponding to the specified in-memory linked list <paramref name="node"/>.</returns>
                private NodeWrapper CreateWrapperCore(LinkedListNode<T> node) => new(this, node);

                /// <summary>
                /// Called after a new node has been added to the in-memory linked list in order to allocate storage for it.
                /// </summary>
                /// <param name="node">The node that was added to the in-memory linked list.</param>
                private void OnAdded(NodeWrapper node)
                {
                    Debug.Assert(!_storageKeys.ContainsKey(node.Node), "Expected node with no storage key.");

                    //
                    // Allocate a storage key for the node and keep track of it in the map.
                    //

                    var key = _storage.Add();
                    _storageKeys.Add(node.Node, key);

                    //
                    // Ensure the value of the new node will be written.
                    //

                    _storage.Edit(key, NodeEditKind.Value);
                }

                /// <summary>
                /// Tracks storage changes due to the creation and insertion of <paramref name="newNode"/>.
                /// </summary>
                /// <param name="newNode">The node that was inserted to the linked list.</param>
                /// <returns>The persisted node wrapper around <paramref name="newNode"/>.</returns>
                private NodeWrapper OnInserted(LinkedListNode<T> newNode)
                {
                    //
                    // Allocate a wrapper which will be used to hold the storage key, set by OnAdded below.
                    //

                    var wrappedNewNode = CreateWrapper(newNode);

                    //
                    // Keep track of the edits to adjacent nodes.
                    //

                    OnInserted(wrappedNewNode);

                    //
                    // Return the wrapper for use by AddAfter and AddBefore methods that return the new node.
                    //

                    return wrappedNewNode;
                }

                /// <summary>
                /// Tracks storage changes due to insertion of <paramref name="newNode"/>.
                /// </summary>
                /// <param name="newNode">The node that was inserted to the linked list.</param>
                private void OnInserted(NodeWrapper newNode)
                {
                    //
                    // Set the node's owner to the current list. This is critical to ensure that nodes relocating between lists don't cause edits in the wrong storage entity.
                    //

                    Debug.Assert(newNode.List == null || newNode.List == this, "Expected the node to not have another owner list.");
                    newNode.List = this;

                    //
                    // Assert consistency of the list. This has to happen after setting the List property above because Previous and Next properties depend on it.
                    //

                    Debug.Assert((newNode.Previous == null || newNode.Previous.Next == newNode) && (newNode.Next == null || newNode.Next.Previous == newNode), "Inconsistent linked list node references.");

                    //
                    // Check if the node is new (or is being reused after having been removed) in order to keep track of the new node in storage.
                    //

                    if (!_storageKeys.ContainsKey(newNode.Node))
                    {
                        OnAdded(newNode);
                    }

                    //
                    // The new node has an edit for both its adjacent node references.
                    //

                    _storage.Edit(newNode.Key, NodeEditKind.Previous | NodeEditKind.Next);

                    //
                    // If the new node isn't the new head, there's a node before it whose next node has changed to the new node.
                    //

                    if (newNode.Previous != null)
                    {
                        _storage.Edit(newNode.Previous.Key, NodeEditKind.Next);
                    }

                    //
                    // If the new node isn't the new tail, there's a node beyond it whose previous node has changed to the new node.
                    //

                    if (newNode.Next != null)
                    {
                        _storage.Edit(newNode.Next.Key, NodeEditKind.Previous);
                    }
                }

                /// <summary>
                /// Tracks storage changed due to the removal of <paramref name="node"/>.
                /// </summary>
                /// <param name="node">The node that was removed from the linked list.</param>
                /// <param name="previous">The previous node of the <paramref name="node"/> prior to its removal.</param>
                /// <param name="next">The next node of the <paramref name="node"/> prior to its removal.</param>
                private void OnRemoved(NodeWrapper node, NodeWrapper previous, NodeWrapper next)
                {
                    //
                    // Release the storage key. Upon re-insertion of the node into some list, a new key can be allocated again.
                    //
                    // NB: We can't remove the reference to the underlying in-memory linked list's node in order to avoid memory leaks when someone holds on to the wrapper
                    //     because it prevents users from setting the Value property on the wrapper prior to re-inserting the element into the list.
                    //

                    var key = node.Key;
                    _storage.Remove(key);

                    //
                    // Stop tracking the key in the map and assert the map entry's storage key did match what we just removed.
                    //

                    var oldKey = _storageKeys.Remove(node.Node);
                    Invariant.Assert(oldKey == key, "Inconsistency detected with storage keys.");

                    //
                    // Disassociate the node from the list.
                    //
                    // REVIEW: We keep the wrapper entry in the node cache associated with its in-memory linked list node. This could cause a leak when nodes relocate across
                    //         different lists. Should we remove the entry here instead? If so, we also need to update OnInserted to re-insert the entry in the cache when missing.
                    //

                    node.List = null;

                    //
                    // If the node originally had a predecessor, update its next node reference.
                    //

                    if (previous != null)
                    {
                        _storage.Edit(previous.Key, NodeEditKind.Next);
                    }

                    //
                    // If the node originally had a successor, update its previous node reference.
                    //

                    if (next != null)
                    {
                        _storage.Edit(next.Key, NodeEditKind.Previous);
                    }
                }

                /// <summary>
                /// Saves the value of the linked list node with the specified storage <paramref name="key"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the value of the linked list node element to.</param>
                /// <param name="key">The storage key of the linked list node to save.</param>
                void ILinkedListPersistence.SaveValue(ISerializerFactory serializerFactory, Stream stream, long key)
                {
                    var node = _storageKeys.GetByValue(key);

                    serializerFactory.GetSerializer<T>().Serialize(node.Value, stream);
                }

                /// <summary>
                /// Gets the previous and next references of the linked list node with the specified storage <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The storage key of the linked list node whose previous and next node's storage keys to retrieve.</param>
                /// <returns>A pair of storage keys corresponding to the previous and the next linked list node.</returns>
                (long previous, long next) ILinkedListPersistence.GetPreviousAndNextReferences(long key)
                {
                    var node = _storageKeys.GetByValue(key);

                    return (GetKey(node.Previous), GetKey(node.Next));

                    long GetKey(LinkedListNode<T> n)
                    {
                        return n == null ? -1 : _storageKeys.GetByKey(n);
                    }
                }

                /// <summary>
                /// Wrapper around an in-memory linked list node, used to keep track of the associated storage key.
                /// </summary>
                private sealed class NodeWrapper : ILinkedListNode<T>
                {
                    /// <summary>
                    /// The list the node belongs to, used to perform storage updates upon editing the value through <see cref="Value"/>. This reference can be <c>null</c> for a removed list node.
                    /// </summary>
                    public Wrapper<T> List;

                    /// <summary>
                    /// The corresponding in-memory linked list node.
                    /// </summary>
                    public readonly LinkedListNode<T> Node;

                    /// <summary>
                    /// Creates a new persisted linked list node wrapper around the specified in-memory linked list <paramref name="node"/>.
                    /// </summary>
                    /// <param name="list">The list the node belongs (initially) to.</param>
                    /// <param name="node"> The corresponding in-memory linked list node.</param>
                    public NodeWrapper(Wrapper<T> list, LinkedListNode<T> node)
                    {
                        List = list;
                        Node = node;
                    }

                    /// <summary>
                    /// Gets the storage key used to persist the linked list element.
                    /// </summary>
                    public long Key => List._storageKeys.GetByKey(Node);

                    /// <summary>
                    /// Gets or sets the value contained in the node.
                    /// </summary>
                    public T Value
                    {
                        get => Node.Value;

                        set
                        {
                            //
                            // NB: Even if the node was removed from the list, we keep the Node field alive so we can accept writes to the Value property prior to re-insertion.
                            //

                            Node.Value = value;

                            //
                            // NB: The node may have been removed from the list causing List to be set to null.
                            //

                            List?._storage.Edit(Key, NodeEditKind.Value);
                        }
                    }

                    /// <summary>
                    /// Gets the wrapper for the previous node in the linked list.
                    /// </summary>
                    /// <remarks>
                    /// Note that this property is lazy in its allocation of a wrapper around the previous node.
                    /// </remarks>
                    public NodeWrapper Previous => List.CreateWrapper(Node.Previous);

                    /// <summary>
                    /// Gets the wrapper for the next node in the linked list.
                    /// </summary>
                    /// <remarks>
                    /// Note that this property is lazy in its allocation of a wrapper around the next node.
                    /// </remarks>
                    public NodeWrapper Next => List.CreateWrapper(Node.Next);

                    /// <summary>
                    /// Gets the linked list the node belongs to.
                    /// </summary>
                    ILinkedList<T> ILinkedListNode<T>.List => List;

                    /// <summary>
                    /// Gets the linked list the node belongs to.
                    /// </summary>
                    IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T>.List => List;

                    /// <summary>
                    /// Gets the value contained in the node.
                    /// </summary>
                    T IReadOnlyLinkedListNode<T>.Value => Value;

                    /// <summary>
                    /// Gets the previous node in the linked list.
                    /// </summary>
                    ILinkedListNode<T> ILinkedListNode<T>.Previous => Previous;

                    /// <summary>
                    /// Gets the previous node in the linked list.
                    /// </summary>
                    IReadOnlyLinkedListNode<T> IReadOnlyLinkedListNode<T>.Previous => Previous;

                    /// <summary>
                    /// Gets the next node in the linked list.
                    /// </summary>
                    ILinkedListNode<T> ILinkedListNode<T>.Next => Next;

                    /// <summary>
                    /// Gets the next node in the linked list.
                    /// </summary>
                    IReadOnlyLinkedListNode<T> IReadOnlyLinkedListNode<T>.Next => Next;
                }
            }

            /// <summary>
            /// Flags enumeration used to describe the kind of edit applied to a linked list element.
            /// </summary>
            [Flags]
            private enum NodeEditKind
            {
                /// <summary>
                /// Default value representing no edit.
                /// </summary>
                None = 0,

                /// <summary>
                /// The value of the element was edited.
                /// </summary>
                Value = 0b_0001,

                /// <summary>
                /// The reference to the next linked list element was changed.
                /// </summary>
                Next = 0b_0010,

                /// <summary>
                /// The reference to the previous linked list element was changed.
                /// </summary>
                Previous = 0b_0100,

                /// <summary>
                /// Value representing <see cref="Next"/> and <see cref="Previous"/> edits.
                /// </summary>
                Metadata = Next | Previous,

                /// <summary>
                /// Value representing all edits.
                /// </summary>
                All = Value | Metadata,
            }

            /// <summary>
            /// State kept on dirty pages to keep track of all edits that have to be persisted in the next checkpoint.
            /// </summary>
            private sealed class DirtyState
            {
                /// <summary>
                /// The dictionary mapping each edited storage key to the latest edit operation applied to the key.
                /// </summary>
                public readonly Dictionary<long, NodeEditKind> Edits = new();

                /// <summary>
                /// Starts tracking an edit of a heap element associated with the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key associate with the linked list node that was edited.</param>
                /// <param name="edit">The kind of edit that was applied to the linked list node.</param>
                public void Edit(long key, NodeEditKind edit)
                {
                    if (Edits.TryGetValue(key, out var oldEdit))
                    {
                        Edits[key] = oldEdit | edit;
                    }
                    else
                    {
                        Edits[key] = edit;
                    }
                }
            }
        }
    }
}
