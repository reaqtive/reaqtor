// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Base class for storage entities represented by a heap.
        /// </summary>
        /// <remarks>
        /// The main operations on a heap-based entity are:
        /// <list type="bullet">
        ///   <item><c>long Add()</c> used to allocate an unused key on the heap to associate with a heap item</item>
        ///   <item><c>void Remove(long)</c> used to deallocate a heap item with the specified key</item>
        ///   <item><c>void Edit(long)</c> used to mark the heap item with the specified key as dirty</item>
        /// </list>
        /// Derived types override the load, save, and delete behavior for heap items using the following methods:
        /// <list type="bullet">
        ///   <item><see cref="TryGetItemKeysCore(IStateReader, out IEnumerable{string})"/> to enumerate the keys of the heap items</item>
        ///   <item><see cref="LoadItemCore(IStateReader, string, long)"/> to load an item with a given key</item>
        ///   <item><see cref="SaveItemCore(IStateWriter, string, long)"/> to save an item with a given key</item>
        ///   <item><see cref="DeleteItemCore(IStateWriter, string, long)"/> to delete an item with a given key</item>
        /// </list>
        /// This enables derived types to decide on the storage details of heap items.
        /// </remarks>
        private abstract partial class HeapBase : PersistableBase
        {
            //
            // NB: We use numeric key representations for all data structures below and on edit pages. This prevents having to keep strings alive when save and load operations happen infrequently.
            //     This choice also provides some benefits for sorting operations which can operate on numeric values rather than strings that require character-by-character lexical sorting. One
            //     drawback is the cost imposed by string allocations in save and delete operations, which are supposed to be less frequent than steady state collection operations on the heap. As
            //     a mitigation to reduce this cost, the conversion from a numeric key representation to a string key representation leverages memoization strategies.
            //

            //
            // REVIEW: Decide whether the use of a free list is desirable and whether the fragmentation concern is warranted. Yet another alternative or complementary feature would be a "defrag" or "rebase" functionality that
            //         performs compaction of the store by moving entries around. It could do so on an ongoing basis during Save operations based on a maintenance budget, or in response to an explicit request.
            //

            /// <summary>
            /// The maximum number of keys kept on the <see cref="_freeList"/>. This limit can be used to disable the use of a free list altogether, and to reduce the overhead associated with keeping a free list.
            /// </summary>
            /// <remarks>
            /// When using a small or empty free list, the reuse of storage slots in the key/value store is limited to recycling keys that have been deleted on the most recent edit page (see <see cref="Add"/>).
            /// The main drawback of such a configuration is that the store will have a tendency to grow towards higher key values and empty storage slots will not be reused, possibly causing fragmentation.
            /// </remarks>
            private const int MaxFreeListSize = 16384;

            /// <summary>
            /// The set of storage keys as currently in used by the key/value store (if <see cref="PersistableBase.HasSaved"/> is <c>true</c>). This value gets edited upon successful <see cref="SaveCore(IStateWriter)"/> operations (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            /// <remarks>
            /// This collection is an unordered set. We only sort the entries in the set when required to get a deterministic order, e.g. in <see cref="DeleteCore(IStateWriter)"/>.
            /// Writes to this collection should only happen due to state persistence operations including <see cref="LoadCore(IStateReader)"/> and <see cref="OnSavedCore"/>. In particular, set operations should not mutate this collection.
            /// </remarks>
            private HashSet<long> _keys = new();

            /// <summary>
            /// The numeric representation of the next available store key, for use by <see cref="Add"/>. This value will be 0 for a new heap that has not yet been persisted, and will be derived from state in <see cref="LoadCore(IStateReader)"/>.
            /// </summary>
            private long _nextKey;

            /// <summary>
            /// The set containing the keys that are currently unused (both in the persisted store and due to pending in-memory edits). This set is consulted by the <see cref="Add"/> method to attempt to reuse storage slots.
            /// </summary>
            /// <remarks>
            /// This collection is an ordered set to make finding the smallest value cheaper in the <see cref="Add"/> method (used to ensure determinism and to attempt to keep the store compact).
            /// The maximum number of entries on the free list is governed by the <see cref="MaxFreeListSize"/> setting. For more information, see remarks on this constant's declaration.
            /// Writes to this collection should only happen due to heap operations, except for <see cref="LoadCore(IStateReader)"/>. In particular, <see cref="OnSavedCore"/> shall not mutate this collection, because it runs concurrently with heap operations.
            /// </remarks>
            private readonly SortedSet<long> _freeList = new();

            /// <summary>
            /// The state change manager used to keep track of changes to the key set used to store elements in the heap since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private readonly StateChangedManager<DirtyState> _dirty = new();

            /// <summary>
            /// The pending edits obtained from <see cref="_dirty"/> on the last call to <see cref="SaveCore(IStateWriter)"/>, for use by <see cref="OnSavedCore"/> to edit <see cref="_keys"/> upon a successful save operation.
            /// </summary>
            private DirtyState[] _dirtyHistory;

            /// <summary>
            /// Creates a new entity representing a heap.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public HeapBase(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Deletes the heap from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Remove the elements stored in items/key.
                //
                // NB: The _keys set is unsorted, so we perform an explicit sort here to ensure deterministic ordering of deletes and in-order store access which can reduce seek times.
                //

                foreach (var key in new SortedSet<long>(_keys))
                {
                    //
                    // PERF: Conversion of the numeric key value to the string representation is assumed to be memoized for efficiency.
                    //

                    DeleteItemCore(writer, GetKeyForIndex(key), key);
                }
            }

            /// <summary>
            /// Deletes the heap item with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operation to.</param>
            /// <param name="key">The key of the item to remove.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected abstract void DeleteItemCore(IStateWriter writer, string key, long keyValue);

            /// <summary>
            /// Loads the heap from storage.
            /// </summary>
            /// <param name="reader">The reader to load the heap from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Simply iterate over the keys in the items category, i.e. "dir items/*". Each key present contains a single heap element.
                //

                if (TryGetItemKeysCore(reader, out var keys))
                {
                    //
                    // Keep track of the numeric value of the highest key in use. We'll add 1 to this value to obtain the next free key, so we start off at -1 to account for keys being empty.
                    //

                    var maxKey = -1L;

                    //
                    // Keep a set of all the numeric key values that are in use. This set will be used to derive the free list from.
                    //

                    var usedKeys = new HashSet<long>();

                    //
                    // Iterate over the keys to populate _keys and _data, and to set maxKey and usedKeys.
                    //

                    foreach (var key in keys)
                    {
                        //
                        // Convert the key to its numeric representation
                        //
                        // NB: We don't expect invalid keys that don't represent a number. In case we do find an invalid value, we fail fast.
                        //

                        var keyValue = GetIndexForKey(key);

                        //
                        // Add the key to the set of keys in use. We don't expect duplicates and assert this.
                        //

                        Invariant.Assert(_keys.Add(keyValue), "Unexpected double occurrence of a key value.");

                        //
                        // Perform heap-specific load operations for the item.
                        //

                        LoadItemCore(reader, key, keyValue);

                        //
                        // Keep track of the key value in usedKeys and update maxKey if necessary.
                        //

                        usedKeys.Add(keyValue);
                        maxKey = Math.Max(maxKey, keyValue);
                    }

                    //
                    // Set the next unused key value, which gets used by key allocation in Add. Note we start off maxKey as -1 to account for keys being empty.
                    //

                    _nextKey = maxKey + 1L;

                    //
                    // Populate the free list with the numeric values of the unused keys in the range [0..maxKey]. This list in considered prior to advancing _nextKey in Add.
                    //
                    // NB: We cap the free list size using MaxFreeListSize to avoid keeping a lot of state. This constant also provides a toggle to turn off the free list altogether.
                    //
                    // REVIEW: Given that we cap the free list, should we add logic in Add to replenish the free list when we run out of entries?
                    //

                    var freeCount = (int)Math.Min(_nextKey - usedKeys.Count, MaxFreeListSize);

                    if (freeCount > 0)
                    {
                        for (var i = 0L; i < _nextKey && _freeList.Count < freeCount; i++)
                        {
                            if (!usedKeys.Contains(i))
                            {
                                _freeList.Add(i);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Gets the list of keys for the items on the heap.
            /// </summary>
            /// <param name="reader">The reader to obtain the list of keys from.</param>
            /// <param name="keys">The keys of the items on the heap.</param>
            /// <returns><c>true</c> if items were found and returned in <paramref name="keys"/>; otherwise, <c>false</c>.</returns>
            protected abstract bool TryGetItemKeysCore(IStateReader reader, out IEnumerable<string> keys);

            /// <summary>
            /// Loads the heap item with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="reader">The reader to load the item from.</param>
            /// <param name="key">The key of the item to load.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected abstract void LoadItemCore(IStateReader reader, string key, long keyValue);

            /// <summary>
            /// Saves the heap to storage.
            /// </summary>
            /// <param name="writer">The writer to write the heap to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Add, Remove).");

                //
                // Snapshot the dirty state (containing additions, edits, and removals of keys). See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                _dirtyHistory = _dirty.SaveState();

                //
                // Check if we need to save all the state in case of a full checkpoint or a new heap that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    //
                    // Get the set of all keys that will be in existence after applying all in-memory edits.
                    //
                    // NB: The key set will be empty for a new set instance.
                    //

                    var keys = GetKeys(_dirtyHistory);

                    if (keys.Count > 0)
                    {
                        //
                        // Sort the keys to get a deterministic in-order sequence of write operations.
                        //

                        foreach (var keyValue in new SortedSet<long>(keys))
                        {
                            //
                            // PERF: Conversion of the numeric key value to the string representation is assumed to be memoized for efficiency.
                            //

                            var key = GetKeyForIndex(keyValue);

                            SaveItemCore(writer, key, keyValue);
                        }
                    }
                }
                else
                {
                    //
                    // NB: Collecting all the keys to replace the _keys field will be done in OnSavedCore.
                    //

                    //
                    // Apply the edit pages in chronological order to determine all keys that are dirty. Dictionary entry true values indicate an addition or edit, and false values indicates a removal.
                    //
                    // NB: We use a sorted dictionary to have a deterministic and in-order application of the edits.
                    //

                    var edits = new SortedDictionary<long, bool>();

                    foreach (var dirty in _dirtyHistory)
                    {
                        foreach (var entry in dirty.Entries)
                        {
                            //
                            // NB: The most recent edit wins. However, edit pages may contain Add and Remove entries for keys that don't exist in the persistent store, so we need more checks when applying the edits (see calls to DeleteItem).
                            //     We prefer doing these checks over adding (key, true) entries based on _keys prior to applying the edit pages and performing heap operations to partition the set into edits versus removes based on _keys.
                            //

                            edits[entry.Key] = entry.Value;
                        }
                    }

                    if (edits.Count > 0)
                    {
                        //
                        // Iterate over all edits sorted by the key.
                        //

                        foreach (var edit in edits)
                        {
                            var keyValue = edit.Key;

                            var key = GetKeyForIndex(keyValue);

                            if (edit.Value)
                            {
                                //
                                // If the value has been added or edited, dispatch to SaveItemCore to facilitate the persistence.
                                //

                                SaveItemCore(writer, key, keyValue);
                            }
                            else
                            {
                                //
                                // If the value is marked for deletion, first check whether it exists in the store according to _keys. False positives for deletion can occur if edit pages contain Add and Remove operations that cancel out.
                                //

                                if (_keys.Contains(keyValue))
                                {
                                    DeleteItemCore(writer, key, keyValue);
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Saves the heap item with the specified <paramref name="key"/> to storage.
            /// </summary>
            /// <param name="writer">The writer to apply the save operation to.</param>
            /// <param name="key">The key of the item to save.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected abstract void SaveItemCore(IStateWriter writer, string key, long keyValue);

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
                // Reflect the actual keys that exist in the store.
                //

                _keys = GetKeys(_dirtyHistory);

                //
                // We no longer need the edit pages.
                //

                _dirtyHistory = null;
            }

            /// <summary>
            /// Gets all the storage keys that are in use by incorporating the edit pages specified in <paramref name="dirtyHistory"/>.
            /// </summary>
            /// <param name="dirtyHistory">The edit pages to use for computing the storage keys that are in use.</param>
            /// <returns>An unordered set of keys that are in use.</returns>
            private HashSet<long> GetKeys(DirtyState[] dirtyHistory)
            {
                //
                // Start from the keys currently in use by the persistent store.
                //

                var keys = new HashSet<long>(_keys);

                //
                // Apply all the edit pages in chronological order.
                //

                foreach (var dirty in dirtyHistory)
                {
                    foreach (var entry in dirty.Entries)
                    {
                        //
                        // REVIEW: We can't thoroughly assert invariants here due to the Boolean state for entries on a dirty page, e.g.
                        //
                        //         * Remove/Add results in edit = true, but the precondition isn't that the key does not exist;
                        //         * Add/Remove results in edit = false, but the precondition isn't that the key exists.
                        //

                        if (entry.Value)
                        {
                            //
                            // If the entry is marked as an add or edit operation, add it to the keys present.
                            //

                            keys.Add(entry.Key);
                        }
                        else
                        {
                            //
                            // If the entry is marked as a deletion operation, remove it from the keys present.
                            //

                            keys.Remove(entry.Key);
                        }
                    }
                }

                return keys;
            }

            /// <summary>
            /// Called when an element is added to the heap, requesting an unused storage key to associate with the element (which allows for removal of the entry later).
            /// </summary>
            /// <returns>An unused storage key to associate with the newly added element.</returns>
            protected long Add()
            {
                //
                // Mark the state as dirty and allocate a free key.
                //

                StateChanged = true;

                //
                // First check whether we have any keys in the latest edit page that are marked for removal. If so, we can reuse the slot.
                //

                if (_dirty.State.TryGetRemovedKeyAndSwapToEdited(out var keyValue))
                {
                    //
                    // The edit page has been altered to keep track of an edit (in lieu of a deletion) for the key that was found. We need to remove the entry from the free list.
                    //
                    // NB: We can't assert successful removal from the free list because its size is capped so free entries on and edit page may not have been recorded in the free list.
                    //

                    _freeList.Remove(keyValue);
                }
                else
                {
                    //
                    // Next, consult the free list. If it has any entry, pick the entry with the lowest value. This results in determinism and attempts to keep the store compact.
                    //

                    if (_freeList.Count > 0)
                    {
                        var min = _freeList.Min;
                        _freeList.Remove(min);

                        keyValue = min;
                    }
                    else
                    {
                        //
                        // If we can't find any free key, get one from the _nextKey maximum key pointer and advance the pointer.
                        //
                        // NB: Key values that are obtained from the _nextKey pointer can never have been used before (within the current process working set), so can't occur on the free list.
                        //

                        keyValue = _nextKey++;
                    }

                    //
                    // Record the addition on the latest edit page.
                    //

                    _dirty.State.Add(keyValue);
                }

                Debug.Assert(!_freeList.Contains(keyValue), "A key that has been allocated should not occur on the free list.");

                return keyValue;
            }

            /// <summary>
            /// Called when an element on the heap associated with the specified <paramref name="key"/> is edited.
            /// </summary>
            /// <param name="key">The storage key associated with the element that was edited.</param>
            protected void Edit(long key)
            {
                //
                // Mark the state as dirty and record the edit on the latest edit page.
                //

                StateChanged = true;

                _dirty.State.Edit(key);
            }

            /// <summary>
            /// Called when an element is removed from the heap that was associated with the specified <paramref name="key"/>.
            /// </summary>
            /// <param name="key">The storage key associated with the element that was removed.</param>
            protected void Remove(long key)
            {
                //
                // Mark the state as dirty, record the removal on the latest edit page, and return the key to the free list, if possible.
                //

                StateChanged = true;

                _dirty.State.Remove(key);

                if (_freeList.Count < MaxFreeListSize)
                {
                    AddToFreeList(key);
                }
            }

            /// <summary>
            /// Called when the heap is cleared.
            /// </summary>
            /// <param name="keys">The sorted set of storage keys associated with the elements that were removed.</param>
            /// <remarks>
            /// This method is used in lieu of <see cref="Remove(long)"/>to make bulk deletion more efficient. We require a sorted set passed to <paramref name="keys"/> to support a few optimizations,
            /// including the unioning of the deleted keys with the removed keys on the latest edit page (which is kept sorted, see <see cref="DirtyState._removed"/>) and ordered insertion of deleted
            /// keys on the free list to gain determinism and prefer lower key values in an attempt to keep the store compact.
            /// </remarks>
            protected void Clear(SortedSet<long> keys)
            {
                //
                // Mark the state as dirty.
                //

                StateChanged = true;

                //
                // Record the removed keys on the latest edit page.
                //
                // NB: Unfortunately, this strategy does not result in subsequent additions of elements to the heap restarting at key 0. For example, consider a heap that starts off with keys { 0, 1, 2, 3, 4 }.
                //     A first edit page marks elements { 0, 2 } for deletion. The second and most recent edit page marks elements { 1, 3 } for deletion. A subsequent Clear operation finds the key { 4 } as
                //     removed and adds it to the most recent edit page, resulting in keys { 1, 3, 4 } marked for deletion. Subsequent heap additions will attempt to reuse the slots from the most recent edit
                //     page, i.e. the ordered set { 1, 3, 4 }. However, the deleted elements { 0, 2 } on the first edit page are not considered because the free list is considered after consulting the most
                //     recent edit page.
                //
                //     An alternative strategy would involve clearing the free list, resetting the _nextKey pointer to 0, and force subsequent key allocations to bypass the most recent edit page. This could
                //     be achieved by pushing a new edit page but this breaks the assumption made by OnSavedCore that all but the last page should be pruned upon successful state saving. To work around this,
                //     another state could be recorded in the most recent edit page to denote a Clear operation and allocate a child page for the new Add/Remove edits beyond the Clear (a new "epoch").
                //
                // REVIEW: Review the merits of the alternative strategy suggested above.
                //

                _dirty.State.Remove(keys);

                //
                // Finally record all deletions on the free list, up to the MaxFreeListSize limit.
                //

                if (_freeList.Count < MaxFreeListSize)
                {
                    //
                    // NB: The keys are sorted, so this loop gives preference to lower key values in an attempt to keep the store compact.
                    //

                    foreach (var key in keys)
                    {
                        if (!AddToFreeList(key))
                        {
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// Helper method to add the specified <paramref name="keyValue"/> to the free list. Assumes there's enough space on the free list not to exceed <see cref="MaxFreeListSize"/>.
            /// </summary>
            /// <param name="keyValue">The key value to add to the free list.</param>
            /// <returns><c>true</c> if the free list has more space left after the addition of the key value; otherwise, <c>false</c>.</returns>
            private bool AddToFreeList(long keyValue)
            {
                Debug.Assert(_freeList.Count < MaxFreeListSize, "Expected space in the free list.");

                _freeList.Add(keyValue);

                return _freeList.Count < MaxFreeListSize;
            }

            /// <summary>
            /// State kept on dirty pages to keep track of all edits that have to be persisted in the next checkpoint.
            /// </summary>
            private sealed class DirtyState
            {
                /// <summary>
                /// The dictionary mapping each edited storage key to the latest edit operation applied to the key (<c>true</c> for add or edit, <c>false</c> for remove).
                /// </summary>
                private readonly Dictionary<long, bool> _edits = new();

                /// <summary>
                /// The sorted set of removed key values. We keep this set sorted to aid in ordered reused of slots, see <see cref="TryGetRemovedKeyAndSwapToEdited(out long)"/>.
                /// </summary>
                private readonly SortedSet<long> _removed = new();

                /// <summary>
                /// Start tracking the addition of a heap element associated with the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key associated with the heap element that was added.</param>
                public void Add(long key)
                {
                    //
                    // If the element was marked as removed, get rid of the entry in the set of removed keys, so TryGetRemovedKeyAndSwapToEdited does not hand it out
                    // again for subsequent allocation requests. This situation can arise when performing a Remove/Add sequence.
                    //

                    _edits[key] = true;
                    _removed.Remove(key);
                }

                /// <summary>
                /// Starts tracking an edit of a heap element associated with the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key associate with the heap element that was edited.</param>
                public void Edit(long key)
                {
                    _edits[key] = true;
                }

                /// <summary>
                /// Start tracking the removal of a heap element associated with the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key associated with the heap element that was removed.</param>
                public void Remove(long key)
                {
                    //
                    // Mark the entry as deleted and add the key to the set of removed keys, so TryGetRemovedKeyAndSwapToEdited can attempt to reuse it.
                    //
                    // REVIEW: Can we get rid of spurious edits more aggressively? E.g. Add/Remove on the same page is a no-op.
                    //

                    _edits[key] = false;
                    _removed.Add(key);
                }

                /// <summary>
                /// Start tracking the removal of a heap elements associated with the specified <paramref name="keys"/>.
                /// </summary>
                /// <param name="keys">The key associated with the heap elements that were removed.</param>
                public void Remove(SortedSet<long> keys)
                {
                    //
                    // REVIEW: See remarks above.
                    //

                    foreach (var key in keys)
                    {
                        _edits[key] = false;
                    }

                    _removed.UnionWith(keys);
                }

                /// <summary>
                /// Tries to allocate a storage key by reusing a storage key that's marked for deletion.
                /// </summary>
                /// <param name="key">The allocated storage key, if found.</param>
                /// <returns><c>true</c> if a storage key was found that can be reused (and the method has changed the entry to an edit state in lieue of a delete state); otherwise, <c>false</c>.</returns>
                public bool TryGetRemovedKeyAndSwapToEdited(out long key)
                {
                    if (_removed.Count > 0)
                    {
                        key = _removed.Min;
                        _removed.Remove(key);

                        _edits[key] = true;
                        return true;
                    }
                    else
                    {
                        key = -1L;
                        return false;
                    }
                }

                /// <summary>
                /// Gets the unordered sequence of edits that have to be applied to the store upon saving state. Each entry represents a mapping of a storage key
                /// onto an edit operation where <c>true</c> stands for an addition or edit, and <c>false</c> stands for a deletion.
                /// </summary>
                public IEnumerable<KeyValuePair<long, bool>> Entries => _edits;
            }
        }
    }
}
