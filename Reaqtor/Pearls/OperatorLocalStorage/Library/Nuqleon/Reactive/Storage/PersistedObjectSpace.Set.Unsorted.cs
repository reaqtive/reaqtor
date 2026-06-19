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

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier to use for the set.</param>
        /// <returns>A new persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSet<T> CreateSet<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var set = new UnsortedSet(this);
            _items.Add(id, set);
            return CreateSetCore<T>(id, set);
        }

        /// <summary>
        /// Gets a persisted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier of the set to retrieve.</param>
        /// <returns>An existing persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted set type.</exception>
        public IPersistedSet<T> GetSet<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateSetCore<T>(id, (UnsortedSet)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
        /// <param name="id">The identifier of the set.</param>
        /// <param name="set">The storage entity representing the set.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="set"/>.</returns>
        private static IPersistedSet<T> CreateSetCore<T>(string id, UnsortedSet set) => set.Create<T>(id);

        /// <summary>
        /// Storage entity representing an unsorted set.
        /// </summary>
        private sealed class UnsortedSet : Set
        {
            /// <summary>
            /// Creates a new entity representing a set.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public UnsortedSet(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.Set"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.Set;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
            /// <param name="id">The identifier of the set.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedSet<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    //
                    // NB: Restoring a set to an in-memory data structure returns both the set (of type HashSet<T>) and the map of set entries to storage keys (of type Map<T, long>).
                    //

                    var (set, storageKeys) = Restore<T>();

                    var wrapper = new Wrapper<T>(id, this, set, storageKeys);
                    _wrapper = wrapper;
                    return wrapper;
                }
                else
                {
                    return (IPersistedSet<T>)_wrapper;
                }
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory set representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="Load"/>.
            /// If the entity has not been persisted before, this methods returns a new empty set instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
            /// <returns>A pair consisting of an instance of type HashSet{<typeparamref name="T"/>} containing the data represented by the storage entity, and a map that associates all set entries with their storage key.</returns>
            private (HashSet<T> set, Map<T, long> storageKeys) Restore<T>()
            {
                //
                // The equality comparer to use for values, both in the set (to determine uniqueness) and the map (for the value-to-key entries).
                //
                // REVIEW: Consider making this configurable, which requires the persistence of an expression to construct the comparer in the metadata (likely using a wildcard type so it can be late-bound).
                //

                var eq = EqualityComparer<T>.Default;

                //
                // Always allocate an empty set and map. In case we were called for a new set, we'll just return the empty ones.
                //

                var set = new HashSet<T>(eq);
                var storageKeys = new Map<T, long>(eq, EqualityComparer<long>.Default);

                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize set elements from. Keep track of both the values and the storage keys.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<T>();

                    //
                    // Deserialize each eventual object and populate the set and map entries.
                    //

                    foreach (var (key, data) in _data)
                    {
                        var value = data.Deserialize(deserializer);

                        //
                        // NB: The use of the Add method on the storageKeys map has the side-effect of asserting uniqueness of entries, causing restoration to fail if duplicate entries are detected.
                        //     It is assumed that the equality comparer for T is stable across recoveries.
                        //

                        set.Add(value);
                        storageKeys.Add(value, key);
                    }

                    _data = null;
                }

                return (set, storageKeys);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted set with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
            private sealed class Wrapper<T> : WrapperBase<T, HashSet<T>>, IPersistedSet<T>
            {
                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the set.</param>
                /// <param name="storage">The storage entity representing the set.</param>
                /// <param name="set">The initial set. This could either be the result of deserializing persisted state, or an empty set for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each element in <paramref name="set"/>.</param>
                public Wrapper(string id, Set storage, HashSet<T> set, Map<T, long> storageKeys)
                    : base(id, storage, set, storageKeys)
                {
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present in the set and in the specified <paramref name="other"/> collection.
                /// </summary>
                /// <param name="other">The collection to compare to the set.</param>
                protected override void IntersectWithCore(IEnumerable<T> other)
                {
                    //
                    // Convert the other sequence to a HashSet<T> for efficient Contains checks.
                    //

                    if (!TryGetOtherAsSetWithSameComparer(other, out var set))
                    {
                        //
                        // NB: HashSet<T> has a more efficient implementation for this case; we have to content ourselves with allocating a set with the same comparer.
                        //

                        set = new HashSet<T>(other, _set.Comparer);
                    }

                    //
                    // Because we can't remove elements from the set while iterating over it, we have to keep track of the keys to remove in a separate set.
                    //

                    var remove = new HashSet<T>(_set.Comparer);

                    foreach (var item in this)
                    {
                        if (!set.Contains(item))
                        {
                            remove.Add(item);
                        }
                    }

                    //
                    // Perform the removal of all the items that are not in the intersection.
                    //

                    foreach (var item in remove)
                    {
                        Remove(item);
                    }
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present either in the set or in the specified <paramref name="other"/> collection, but not both.
                /// </summary>
                /// <param name="other">The collection of items to compare to the current set.</param>
                protected override void SymmetricExceptWithCore(IEnumerable<T> other)
                {
                    //
                    // Convert the other sequence to a HashSet<T> to eliminate duplicates which would lead to multiple Remove/Add calls in the loop below, yielding a wrong result.
                    //

                    if (!TryGetOtherAsSetWithSameComparer(other, out var set))
                    {
                        //
                        // NB: HashSet<T> has a more efficient implementation for this case; we have to content ourselves with allocating a set with the same comparer.
                        //

                        set = new HashSet<T>(other, _set.Comparer);
                    }

                    //
                    // Iterate over the other collection's unique elements (cf. the set conversion above) and remove every element from the current set (so it doesn't occur in
                    // both sets, effectively obtaining this \ other), or add the element to the current set if it's not in the current set yet (implying it only existed in the
                    // other set, thus computing other \ this).
                    //

                    foreach (var item in set)
                    {
                        if (!Remove(item))
                        {
                            Add(item);
                        }
                    }
                }

                /// <summary>
                /// Tries to convert the enumerable sequence specified in <paramref name="other"/> to a <see cref="HashSet{T}"/> instance, provided it has the
                /// same comparer as the current <see cref="Wrapper{T}"/> instance's underlying set.
                /// </summary>
                /// <param name="other">The sequence to convert.</param>
                /// <param name="set">A <see cref="HashSet{T}"/> instance if <paramref name="other"/> is convertible to this type and has the same comparer; otherwise, <c>null</c>.</param>
                /// <returns><c>true</c> if the conversion succeeded and the result is stored in <paramref name="set"/>; otherwise, <c>false</c>.</returns>
                private bool TryGetOtherAsSetWithSameComparer(IEnumerable<T> other, out HashSet<T> set)
                {
                    set = AsHashSet(other);

                    if (set != null && set.Comparer.Equals(_set.Comparer))
                    {
                        return true;
                    }

                    set = null;
                    return false;
                }

                /// <summary>
                /// Tries to convert the enumerable sequence specified in <paramref name="other"/> to a <see cref="HashSet{T}"/> instance.
                /// </summary>
                /// <param name="other">The sequence to convert.</param>
                /// <returns>A <see cref="HashSet{T}"/> instance if <paramref name="other"/> is convertible to this type; otherwise, <c>null</c>.</returns>
                private static HashSet<T> AsHashSet(IEnumerable<T> other)
                {
                    //
                    // Check if other is a HashSet<T> or a Wrapper<T> from which we can extract the _set field.
                    //

                    return other switch
                    {
                        HashSet<T> set => set,
                        Wrapper<T> wrapper => wrapper._set,
                        _ => null,
                    };
                }
            }
        }
    }
}
