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
using System.Linq;
using System.Threading;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Base class for transactional dictionary implementations supporting the creation of snapshots to apply edits to a persistent key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the dictionary.</typeparam>
    /// <remarks>
    /// This type is thread-safe.
    ///
    /// A transactional dictionary contains an in-memory representation of the persisted key/value story entries, and a list of edit pages that represent the not yet
    /// persisted edits that have been made to the dictionary. All dictionary operations provide a view over the dictionary that combines the persisted key/value store
    /// entries and the pending edits. By using the snapshot functionality in <see cref="CreateSnapshot(bool)"/>, the set of pending edits can be retrieved in order to
    /// apply these edits to the persistent key/value store by using the <see cref="ISnapshot{TKey, TValue}.Accept(ISnapshotVisitor{TKey, TValue})"/> method. Upon
    /// successful persistence of a snapshot, as indicated through a call to <see cref="ISnapshot{TKey, TValue}.OnCommitted"/>, the in-memory representation of the
    /// persisted key/value store entries is updated, and the edit pages are pruned.
    /// </remarks>
    internal class TransactionalDictionary<TKey, TValue> : ITransactionalDictionary<TKey, TValue>
    {
        /// <summary>
        /// Object used to synchronize access to the <see cref="_store"/> and <see cref="_deltas"/> fields.
        /// </summary>
        private readonly object _gate;

        /// <summary>
        /// The in-memory representation of the key/value store. The entries in this dictionary reflect those that are currently known to be persisted in the underlying
        /// key/value store. Edits to this field are made upon committing of a snapshot via <see cref="ISnapshot{TKey, TValue}.OnCommitted"/> to merge the edits to the
        /// in-memory representation, allowing to reclaim dirty pages held by <see cref="_deltas"/>. In addition, calls to <see cref="Restore(TKey, TValue)"/> allow for
        /// direct addition to this in-memory representation of the key/value store, for use during recovery operations.
        /// </summary>
        /// <remarks>
        /// This field should be accessed under the <see cref="_gate"/> lock.
        /// </remarks>
        private readonly Dictionary<TKey, TValue> _store;

        /// <summary>
        /// The state manager to keep track of edit pages containing the edits that have been applied on top of the in-memory representation of the key/value store. All
        /// read operations use the edit pages in reverse chronological order to determine the latest state of a dictionary entry, prior to consulting the in-memory
        /// representation of the key/value store in <see cref="_store"/>. All write operations are performed against the latest edit page which is made accessible by
        /// the state manager through <see cref="StateChangedManager{T}.State"/>. Upon creation of a snapshot in <see cref="CreateSnapshot(bool)"/>, the state manager
        /// is also snapshotted through <see cref="StateChangedManager{T}.SaveState"/> causing all existing edit pages to become read-only and a new edit page to be
        /// appended. This snapshot is then encapsulated in the snapshot returned from <see cref="CreateSnapshot(bool)"/> and gets committed via the state manager's
        /// <see cref="StateChangedManager{T}.OnStateSaved"/> method upon a call to <see cref="ISnapshot{TKey, TValue}.OnCommitted"/>. As a result, the edit pages that
        /// were captured by the state manager snapshot get pruned and the edits get applied to the in-memory representation of the key/value store.
        /// </summary>
        /// <remarks>
        /// This field should be accessed under the <see cref="_gate"/> lock.
        /// </remarks>
        private readonly StateChangedManager<Dictionary<TKey, Maybe<TValue>>> _deltas;

        /// <summary>
        /// Creates a new empty transactional dictionary.
        /// </summary>
        public TransactionalDictionary()
        {
            _gate = new object();
            _store = new Dictionary<TKey, TValue>();
            _deltas = new StateChangedManager<Dictionary<TKey, Maybe<TValue>>>();
        }

        /// <summary>
        /// Gets or sets the value of the entry in the dictionary with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>The value of the entry in the dictionary with the specified <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The specified <paramref name="key"/> does not exist when attempting to get the corresponding value.</exception>
        public virtual TValue this[TKey key]
        {
            get
            {
                lock (_gate)
                {
                    // NB: TryGetValueCore does not acquire this lock itself.

                    if (!TryGetValueCore(key, out var value))
                    {
                        throw new KeyNotFoundException();
                    }

                    return value;
                }
            }

            set
            {
                lock (_gate)
                {
                    // NB: Edit does not acquire this lock itself.

                    Edit(key, new Maybe<TValue>(value));
                }
            }
        }

        /// <summary>
        /// Adds an entry to the dictionary with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add.</param>
        /// <exception cref="InvalidOperationException">A entry with the specified <paramref name="key"/> already exists.</exception>
        public virtual void Add(TKey key, TValue value)
        {
            lock (_gate)
            {
                // NB: ContainsKeyCore does not acquire this lock itself.

                if (ContainsKeyCore(key))
                    throw new InvalidOperationException(FormattableString.Invariant($"An element with index '{key}' already exists."));

                // NB: Edit does not acquire this lock itself.

                Edit(key, new Maybe<TValue>(value));
            }
        }

        /// <summary>
        /// Checks whether the dictionary contains an entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsKey(TKey key)
        {
            lock (_gate)
            {
                // NB: ContainsKeyCore does not acquire this lock itself.

                return ContainsKeyCore(key);
            }
        }

        /// <summary>
        /// Gets an enumerator to enumerate over a snapshot of the entries in the dictionary.
        /// </summary>
        /// <returns>An enumerator used to enumerate over a snapshot of the key/value pair entries in the dictionary.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            //
            // We'll return a snapshot of all the entries that are currently in that dictionary. This ensures consistency and avoids having to
            // keep a lock for the duration of the enumeration. Allocated outside the lock to reduce the lock duration.
            //

            List<KeyValuePair<TKey, TValue>> res;

            lock (_gate)
            {
                res = GetEnumerableCore().ToList();
            }

            //
            // Return the enumerator over the snapshot. Note there's implicit boxing happening here because the list has a struct-based enumerator.
            //

            return res.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator to enumerate over a snapshot of the entries in the dictionary.
        /// </summary>
        /// <returns>An enumerator used to enumerate over a snapshot of the key/value pair entries in the dictionary.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Removes the entry with the specified <paramref name="key"/> from the dictionary, if found.
        /// </summary>
        /// <param name="key">The key of the entry to remove from the dictionary.</param>
        /// <returns><c>true</c> if an entry with the specified <paramref name="key"/> was found and removed; otherwise, <c>false</c>.</returns>
        public virtual bool Remove(TKey key)
        {
            lock (_gate)
            {
                // NB: ContainsKeyCore does not acquire this lock itself.

                if (!ContainsKeyCore(key))
                {
                    return false;
                }

                // NB: Edit does not acquire this lock itself.

                Edit(key, new Maybe<TValue>(/* no value denotes a removal */));

                return true;
            }
        }

        /// <summary>
        /// Restores the entry with the specified <paramref name="key"/> and <paramref name="value"/> into the in-memory representation of the key/value store.
        /// </summary>
        /// <param name="key">The key of the entry to add to the in-memory representation of the key/value store.</param>
        /// <param name="value">The value of the entry to add to the in-memory representation of the key/value store.</param>
        /// <exception cref="InvalidOperationException">A entry with the specified <paramref name="key"/> already exists.</exception>
        /// <remarks>
        /// This method should only be used during initialization of the transactional directory when restoring the in-memory representation of the key/value store
        /// to reflect the entries in the persisted key/value store. Use of this method after performing edits to the transactional dictionary results in undefined
        /// behavior because entries to the edit pages may hide the entries in the in-memory representation of the key/value store.
        /// </remarks>
        protected virtual void Restore(TKey key, TValue value)
        {
            // REVIEW: Should we introduce a bulk load operation so we can avoid having to acquire the lock for each addition? The drawback is we'd have to keep another in-memory copy of pending adds.

            lock (_gate)
            {
                _store.Add(key, value);
            }
        }

        /// <summary>
        /// Tries to retrieve the value of the entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <param name="value">The value of the entry, if found.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            lock (_gate)
            {
                // NB: TryGetValueCore does not acquire this lock itself.

                return TryGetValueCore(key, out value);
            }
        }

        /// <summary>
        /// Creates a snapshot of the dictionary which can be used to apply addition, edit, and deletion operations to a persistent key/value store.
        /// </summary>
        /// <param name="differential">A value indicating whether to return a differential snapshot (<c>true</c>), i.e. one that only contains the edits since the last successful snapshot persistence. Otherwise, a full snapshot is created.</param>
        /// <returns>The snapshot containing the addition, edit, and deletion operations. For more information, see <see cref="ISnapshot{TKey, TValue}"/>.</returns>
        public virtual ISnapshot<TKey, TValue> CreateSnapshot(bool differential) => differential ? CreateSnapshotDifferential() : CreateSnapshotFull();

        /// <summary>
        /// Creates a differential snapshot.
        /// </summary>
        /// <returns>The snapshot containing the addition, edit, and deletion operations since the last successful snapshot commit.</returns>
        private ISnapshot<TKey, TValue> CreateSnapshotDifferential()
        {
            //
            // First, get a snapshot of the edit pages. This causes a new edit page to be pushed for subsequent edits, and the existing
            // pages to become read-only. Upon signaling a successful commit of the snapshot returned from this method, these pages will
            // be pruned. Note that taking the snapshot of edit pages is the only operation that requires to acquire the lock.
            //

            Dictionary<TKey, Maybe<TValue>>[] snapshot;

            lock (_gate)
            {
                snapshot = _deltas.SaveState();
            }

            //
            // Next, prepare a list of edits to be applied to the persisted key/value store. To do so, we iterate over the edit pages
            // snapshot in reverse chronological order in order to only retain the last edit for each key. To keep track of all the
            // keys which we have already included in the edits list, we use a set.
            //

            var edits = new List<Edit<TKey, TValue>>();

            var processed = new HashSet<TKey>();

            var n = snapshot.Length;

            for (var i = n - 1; i >= 0; i--)
            {
                foreach (var entry in snapshot[i])
                {
                    //
                    // If we haven't encountered any edit for the given key yet, process it now. Note that edits for the same key
                    // can exist in multiple edit pages in case a commit has failed and the returned snapshot was discarded. In
                    // such a case, subsequent edits to keys that occurred on an earlier edit page will be put in the latest edit
                    // page, resulting in multiple entries. Hence, we have to scan through the edit pages in reverse chronological
                    // order and only retain the latest edit for each key.
                    //

                    if (processed.Add(entry.Key))
                    {
                        if (entry.Value.HasValue)
                        {
                            //
                            // If the entry has a value, the edit represents an addition or edit. Note that we can't distinguish
                            // between adds and edits using the current use of Maybe<TValue>. However, such distinction is not
                            // required for the persistent key/value store interface we're targeting right now.
                            //

                            edits.Add(new AddOrUpdateEdit<TKey, TValue>(entry.Key, entry.Value.Value));
                        }
                        else
                        {
                            //
                            // If the entry has no value, this indicates that the entry got removed. However, it's possible that
                            // the removal was recorded on an edit page because of subsequent "add or edit" and "remove" operations
                            // applied to the key. In such a case, we can't annihilate these to remove the entry from the edit page
                            // because an "edit" followed by a "remove" operation would result in a no-op while the intent was to
                            // remove the entry altogether. Because we can't distinguish between "add; remove" and "edit; remove"
                            // sequences, such annihilation is not possible. To remedy seeing spurious deletes for non-existing
                            // entries in the persisted key/value store, we perform an additional check to our in-memory key/value
                            // store representation, which reflects the current state in the persisted key/value store.
                            //

                            //
                            // NB: The in-memory key/value store representation in _store is only mutated from the Recover method
                            //     during initialization and from the OnCommitted method during commit of a snapshot. None of these
                            //     operations are supposed to overlap with CreateSnapshot, so we don't need a lock here.
                            //

                            if (_store.ContainsKey(entry.Key))
                            {
                                edits.Add(new DeleteEdit<TKey, TValue>(entry.Key));
                            }
                        }
                    }
                }
            }

            return new Snapshot(this, edits.ToArray());
        }

        /// <summary>
        /// Creates a full snapshot.
        /// </summary>
        /// <returns>The snapshot containing the addition operations that can reconstruct the whole key/value store as it exists in memory right now.</returns>
        private ISnapshot<TKey, TValue> CreateSnapshotFull()
        {
            var edits = new List<Edit<TKey, TValue>>();

            lock (_gate)
            {
                //
                // Enumerate the whole transactional dictionary to obtain all the records that exist right now. This takes place under
                // the lock in order to prevent concurrent edits that would not be included in the snapshot.
                //

                foreach (var entry in GetEnumerableCore())
                {
                    edits.Add(new AddOrUpdateEdit<TKey, TValue>(entry.Key, entry.Value));
                }

                //
                // To create a full snapshot, we still need to create a snapshot of the edit pages in order to allow for subsequent
                // differential snapshots to be consistent with the current snapshot. Note we perform the snapshot last as a minor
                // optimization to the enumeration above, so it does not need to enumerate the new empty edit page that gets pushed
                // to the _deltas state manager by the call to SaveState.
                //

                //
                // NB: We don't need to keep the snapshot instance around at this point. The call to OnCommitted will call OnStateSaved
                //     on the state manager maintaining the edit pages. Note this assumes a proper invocation order of CreateSnapshot
                //     and OnCommitted. Given this is an internal utility, it's fine to make these assumptions.
                //

                _ = _deltas.SaveState();
            }

            return new Snapshot(this, edits.ToArray());
        }

        /// <summary>
        /// Signals that the specified <paramref name="snapshot"/> has been applied to the persisted key/value store. This method gets
        /// called from the <see cref="ISnapshot{TKey, TValue}.OnCommitted"/> invocation on <see cref="Snapshot"/> and causes the edit
        /// pages captured by the snapshot to be pruned by merging their edits with the in-memory key/value store representation kept
        /// in the <see cref="_store"/> field.
        /// </summary>
        /// <param name="snapshot">The snapshot that has been successfully committed.</param>
        private void OnCommitted(Snapshot snapshot)
        {
            //
            // Acquire the lock to edit the in-memory key/value store representation kept in _store.
            //

            lock (_gate)
            {
                snapshot.Accept(new DictionarySnapshotVisitor<TKey, TValue>(_store));

                _deltas.OnStateSaved();
            }
        }

        /// <summary>
        /// Checks whether the dictionary contains an entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method should be called under the <see cref="_gate"/> lock.
        /// </remarks>
        private bool ContainsKeyCore(TKey key)
        {
            Debug.Assert(Monitor.IsEntered(_gate));

            //
            // First check all the edit pages in reverse chronological order. If any entry is found, its HasValue flag indicates whether
            // the entry was added or edited, if the flag is set to true (implying the entry exists), or whether the entry was removed,
            // if the flag was set to true (implying the entry does not exist anymore).
            //

            foreach (var delta in _deltas)
            {
                if (delta.TryGetValue(key, out var edit))
                {
                    return edit.HasValue;
                }
            }

            //
            // If we didn't find any edit for the entry corresponding to the given key, the in-memory representation of the key/value
            // store is the source of truth.
            //

            return _store.ContainsKey(key);
        }

        /// <summary>
        /// Gets an enumerable sequence containing a snaphot of the entries in the dictionary.
        /// </summary>
        /// <returns>An enumerable sequence representing a snapshot of the key/value pair entries in the dictionary.</returns>
        private IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerableCore()
        {
            Debug.Assert(Monitor.IsEntered(_gate));

            //
            // Set used to keep track of the entries that have already been processed and for which the absence or presence of a value has been
            // determined while scanning the edit pages (see below).
            //

            var yielded = new HashSet<TKey>();

            //
            // Enumerate over all the edit pages in reverse chronological order, thus ensuring that the last recorded edit for an entry wins.
            //

            foreach (var delta in _deltas)
            {
                //
                // Iterate through all the edits in the edit page.
                //

                foreach (var entry in delta)
                {
                    //
                    // Make sure to keep track of all entries encountered on edit pages, even if these represent the deletion of an entry. By
                    // doing so, we avoid yielding a value for an entry that has since been deleted on more recent edit pages.
                    //

                    if (yielded.Add(entry.Key))
                    {
                        //
                        // If this is the first time we encounter this entry's key and the edit has a value, it means that this is the most
                        // recent value of the entry and it should be yielded from the enumeration, so add it to the result snapshot.
                        //

                        if (entry.Value.HasValue)
                        {
                            yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value.Value);
                        }
                    }
                }
            }

            //
            // After going through all the edits, scan the in-memory representation of the key/value store for all entries that have not been
            // shadowed by edits that were processed above.
            //

            foreach (var entry in _store)
            {
                //
                // If an edit was found for a key, don't add the value to the result set. The entry will since have been removed or edited.
                //

                if (yielded.Add(entry.Key))
                {
                    yield return entry;
                }
            }
        }

        /// <summary>
        /// Tries to retrieve the value of the entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <param name="value">The value of the entry, if found.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method should be called under the <see cref="_gate"/> lock.
        /// </remarks>
        private bool TryGetValueCore(TKey key, out TValue value)
        {
            Debug.Assert(Monitor.IsEntered(_gate));

            //
            // See remarks in ContainsKeyCore for the strategy employed here. The only difference is the logic to read the value if an
            // entry with the specified key is found.
            //

            foreach (var delta in _deltas)
            {
                if (delta.TryGetValue(key, out var edit))
                {
                    value = edit.Value;
                    return edit.HasValue;
                }
            }

            return _store.TryGetValue(key, out value);
        }

        /// <summary>
        /// Applies an edit to the entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to apply an edit to.</param>
        /// <param name="edit">The description of the edit. If the <see cref="Maybe{T}.HasValue"/> is set to true, the edit represents an addition or an edit; otherwise, the edit represents a deletion.</param>
        /// <remarks>
        /// This method should be called under the <see cref="_gate"/> lock.
        /// </remarks>
        private void Edit(TKey key, Maybe<TValue> edit)
        {
            Debug.Assert(Monitor.IsEntered(_gate));

            //
            // Use the State property on the state manager keeping the edit pages, causing the latest (most recent) writeable edit page
            // to record the edit. Note that we unconditionally overwrite any existing edit on the latest page and we don't attempt any
            // annihilation of consecutive edits (e.g. an add followed by a remove could result in the removal of the entry). This may
            // result in the presence of a remove edit for a given key even though the entry does not exist in the persisted key/value
            // store. To avoid leaking a delete request to the committer of a snapshot, the CreateSnapshotDifferential method filters
            // out such spurious deletes. For more information, see CreateSnapshotDifferential.
            //

            _deltas.State[key] = edit;
        }

        /// <summary>
        /// Representation of a snapshot returned by <see cref="CreateSnapshot(bool)"/>.
        /// </summary>
        private sealed class Snapshot : ISnapshot<TKey, TValue>
        {
            // REVIEW: Should we add a version number to the snapshot so we can detect invalid use patterns (i.e. operating on an old snapshot if a newer one exists)?

            /// <summary>
            /// The parent transactional dictionary used to make a call to <see cref="TransactionalDictionary{TKey, TValue}.OnCommitted(TransactionalDictionary{TKey, TValue}.Snapshot)"/> when the snapshot was successfully committed.
            /// </summary>
            private readonly TransactionalDictionary<TKey, TValue> _parent;

            /// <summary>
            /// The edits to be applied to the persisted key/value store via the <see cref="Accept(ISnapshotVisitor{TKey, TValue})"/> method.
            /// </summary>
            private readonly Edit<TKey, TValue>[] _edits;

            /// <summary>
            /// Creates a new instance of <see cref="Snapshot"/> containing the specified <paramref name="edits"/> to be applied to the persisted key/value store.
            /// </summary>
            /// <param name="parent">The parent transactional dictionary used to make a call to <see cref="TransactionalDictionary{TKey, TValue}.OnCommitted(TransactionalDictionary{TKey, TValue}.Snapshot)"/> when the snapshot was successfully committed.</param>
            /// <param name="edits">The edits to be applied to the persisted key/value store via the <see cref="Accept(ISnapshotVisitor{TKey, TValue})"/> method.</param>
            public Snapshot(TransactionalDictionary<TKey, TValue> parent, Edit<TKey, TValue>[] edits)
            {
                _parent = parent;
                _edits = edits;
            }

            /// <summary>
            /// Dispatches all the edits in the snapshot to the appropriate <paramref name="visitor"/> methods.
            /// </summary>
            /// <param name="visitor">The visitor to dispatch to.</param>
            /// <remarks>
            /// There will be at most one edit per key/value store entry sharing the same key. The order of the edits in a snapshot is
            /// undefined and does not reflect the order in which edits took place. Snapshots reconcile edits to the same entries by
            /// only retaining the last edit for each key.
            /// </remarks>
            public void Accept(ISnapshotVisitor<TKey, TValue> visitor)
            {
                foreach (var edit in _edits)
                {
                    edit.Accept(visitor);
                }
            }

            /// <summary>
            /// Called after a successful commit to merge the snapshot state to the in-memory representation of the key/value store.
            /// </summary>
            public void OnCommitted() => _parent.OnCommitted(this);
        }
    }
}
