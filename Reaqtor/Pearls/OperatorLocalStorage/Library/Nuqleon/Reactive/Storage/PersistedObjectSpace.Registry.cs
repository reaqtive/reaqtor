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

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Registry of persisted entities in a persisted object space.
        /// </summary>
        private sealed class Registry : TransactionalDictionary<string, IPersistable>
        {
            /// <summary>
            /// Map of persisted entity kind discriminators to factories to instantiate persisted entities, used during recovery (see <see cref="Load"/>).
            /// </summary>
            private static readonly Dictionary<string, Func<PersistedObjectSpace, IPersistable>> s_factory = new()
            {
                { PersistableKind.Array.ToString(), p => new Array(p) },
                { PersistableKind.LinkedList.ToString(), p => new LinkedList(p) },
                { PersistableKind.List.ToString(), p => new List(p) },
                { PersistableKind.Value.ToString(), p => new Value(p) },
                { PersistableKind.Queue.ToString(), p => new Queue(p) },
                { PersistableKind.Stack.ToString(), p => new Stack(p) },
                { PersistableKind.Set.ToString(), p => new UnsortedSet(p) },
                { PersistableKind.SortedSet.ToString(), p => new SortedSet(p) },
                { PersistableKind.Dictionary.ToString(), p => new UnsortedDictionary(p) },
                { PersistableKind.SortedDictionary.ToString(), p => new SortedDictionary(p) },
            };

            /// <summary>
            /// The parent persisted object space this registry belongs to.
            /// </summary>
            private readonly PersistedObjectSpace _parent;

            /// <summary>
            /// State manager to keep track of the deleted persisted entities. Each entry represents an entity that has been deleted from the persisted object space, with deletion pending in the persisted key/value store.
            /// </summary>
            private readonly StateChangedManager<List<KeyValuePair<string, IPersistable>>> _deletes = new();

            /// <summary>
            /// Latest snapshot created by a call to <see cref="Save(IStateWriter)"/> and used by <see cref="OnSaved"/> upon successful commit to prune the dirty pages in the transactional dictionary.
            /// </summary>
            private ISnapshot<string, IPersistable> _snapshot;

            /// <summary>
            /// List of persisted entities that are part of the latest <see cref="Save(IStateWriter)"/> operation, for use by <see cref="OnSaved"/> to call <see cref="IPersistable.OnSaved"/> upon successful commit.
            /// </summary>
            private List<IPersistable> _edits;

            /// <summary>
            /// Creates a new registry instance for the specified <paramref name="parent"/> persisted object space.
            /// </summary>
            /// <param name="parent">The parent persisted object space this registry belongs to.</param>
            public Registry(PersistedObjectSpace parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Loads the registry from the specified state <paramref name="reader"/>.
            /// </summary>
            /// <param name="reader">The state reader to load the registry from.</param>
            public void Load(IStateReader reader)
            {
                var descriptorDeserializer = _parent._serializationFactory.GetDeserializer<Descriptor>();

                //
                // Try to get item keys in the state/index category.
                //

                if (reader.TryGetItemKeys(OperatorStateIndexCategory, out var keys))
                {
                    //
                    // These keys correspond to the persisted objects in the persisted object space, so we expect one object per key.
                    //

                    foreach (var key in keys)
                    {
                        //
                        // Get the reader for the key.
                        //
                        // NB: It is assumed that the IStateReader implementation is correct and doesn't return keys for which no state exists.
                        //

                        using var data = reader.GetItemReader(OperatorStateIndexCategory, key);

                        //
                        // Deserialize the discriminator and try to find the corresponding persisted entity type factory.
                        //

                        var descriptor = descriptorDeserializer.Deserialize(data);

                        var obj = default(IPersistable);

                        if (s_factory.TryGetValue(descriptor.Kind, out var factory))
                        {
                            obj = factory(_parent);
                        }

                        //
                        // Add the persisted entity to the registry.
                        //
                        // NB: If we can't find the persisted entity kind above, we may be recovering a persisted object space from the future (i.e. having new types supported). To avoid overwriting entries, we put a null reference.
                        // NB: This makes an assumption we won't load other than during recovery (and never more than once). I.e. this bypasses any edit page checks and assumes uniqueness of keys.
                        //

                        Restore(key, obj);

                        //
                        // If we were able to instantiate the entity, go ahead and load its state.
                        //
                        // NB: This makes an assumption that persisted entities won't try to access sibling entities during recovery. In case this assumption has to change, we need to introduce an order or better phasing of load operations.
                        //

                        obj?.Load(new ItemReader(reader, key));
                    }
                }
            }

            /// <summary>
            /// Saves the registry to the specified state <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The state writer to save the registry to.</param>
            public void Save(IStateWriter writer)
            {
                var descriptorSerializer = _parent._serializationFactory.GetSerializer<Descriptor>();

                //
                // Create a snapshot of the transactional dictionary and accept all of the edits to the state writer.
                //
                // NB: Writing these changes will include deletion operations for deleted entities in the state/index category. The state held by these persisted entities is removed below.
                //

                _snapshot = CreateSnapshot(differential: writer.CheckpointKind == CheckpointKind.Differential);
                _snapshot.Accept(new SnapshotVisitor(writer, descriptorSerializer));

                //
                // Get the deletes to process. For each persisted entity to be deleted, we have to make a call to the entity's Delete method.
                //

                List<KeyValuePair<string, IPersistable>>[] deletes;

                lock (_deletes)
                {
                    deletes = _deletes.SaveState();
                }

                foreach (var page in deletes)
                {
                    foreach (var delete in page)
                    {
                        //
                        // REVIEW: Deletion of a persisted object and subsequent creation of a persisted object with the same identifier will result in removes and edits for the same keys leaking to the writer.
                        //         Note that the delete/add order is maintained for such key/value store state entries.
                        //

                        delete.Value.Delete(new ItemWriter(writer, delete.Key));
                    }
                }

                //
                // Create a list of all the persisted entities that have their StateChanged flag set to true, and perform Save operations for these. The list is kept to support subsequent OnSaved calls.
                //
                // NB: If a checkpoint fails, the StateChanged flag will no revert to false, so it's fine to overwrite the existing list for each call to Save.
                // NB: Enumeration over "this" includes all of the newly added (but not yet committed) entries to the transactional dictionary. For new entities, StateChanged will be set to true as well.
                //

                _edits = new List<IPersistable>();

                foreach (var entry in this)
                {
                    var persistable = entry.Value;

                    if (persistable.StateChanged)
                    {
                        _edits.Add(persistable);

                        persistable.Save(new ItemWriter(writer, entry.Key));
                    }
                }
            }

            /// <summary>
            /// Marks the last <see cref="Save(IStateWriter)"/> operation as successful.
            /// </summary>
            public void OnSaved()
            {
                //
                // Prune the dirty pages in the transactional dictionary.
                //

                _snapshot.OnCommitted();

                //
                // Prune the dirty pages in the deletion list.
                //

                lock (_deletes)
                {
                    _deletes.OnStateSaved();
                }

                //
                // Mark the last Save operation for each persisted entity as successful.
                //

                foreach (var entry in _edits)
                {
                    entry.OnSaved();
                }
            }

            /// <summary>
            /// Removes the persisted entity with the specified <paramref name="key"/> identifier from the registry, if found.
            /// </summary>
            /// <param name="key">The identifier of the persisted entity to remove from the registry.</param>
            /// <returns><c>true</c> if a persisted entity with the specified <paramref name="key"/> was found and removed; otherwise, <c>false</c>.</returns>
            public override bool Remove(string key)
            {
                //
                // In addition to the transactional dictionary deletion, keep track of the deleted persisted entity in the deletion list, so we can clean up its state (see Save).
                //

                if (TryGetValue(key, out var item))
                {
                    lock (_deletes)
                    {
                        _deletes.State.Add(new KeyValuePair<string, IPersistable>(key, item));
                    }
                }

                return base.Remove(key);
            }

            /// <summary>
            /// Snapshot visitor used to persist changes to the registry to a given <see cref="IStateWriter"/>.
            /// </summary>
            private sealed class SnapshotVisitor : ISnapshotVisitor<string, IPersistable>
            {
                /// <summary>
                /// The state writer to persist changes to.
                /// </summary>
                private readonly IStateWriter _writer;

                /// <summary>
                /// The serializer to use for writing <see cref="Descriptor"/> values for registry entries.
                /// </summary>
                private readonly ISerializer<Descriptor> _serializer;

                /// <summary>
                /// Snapshot visitor used to persist changes to the registry to the given <paramref name="writer"/>.
                /// </summary>
                /// <param name="writer">The state writer to persist changes to.</param>
                /// <param name="serializer">The serializer to use for writing <see cref="Descriptor"/> values for registry entries.</param>
                public SnapshotVisitor(IStateWriter writer, ISerializer<Descriptor> serializer)
                {
                    _writer = writer;
                    _serializer = serializer;
                }

                /// <summary>
                /// Adds or updates the key/value store index entry for the persisted entity <paramref name="value"/> identified using the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The identifier of the persisted entity whose index entry to add or update.</param>
                /// <param name="value">The persisted entity whose index entry to add or update.</param>
                public void AddOrUpdate(string key, IPersistable value)
                {
                    //
                    // REVIEW: We don't impose any limits on key (length, characters, etc.). Can we assume the IStateWriter has to deal with this, or should we have our own mapping somewhere (hashing, use of a dictionary, etc.)?
                    //

                    var stream = _writer.GetItemWriter(OperatorStateIndexCategory, key);
                    _serializer.Serialize(new Descriptor { Kind = value.Kind.ToString() }, stream);
                }

                /// <summary>
                /// Deletes the key/value store index entry for the persisted entity identifier using the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The identifier of the persisted entity whose index entry to remove.</param>
                public void Delete(string key)
                {
                    _writer.DeleteItem(OperatorStateIndexCategory, key);
                }
            }
        }
    }
}
