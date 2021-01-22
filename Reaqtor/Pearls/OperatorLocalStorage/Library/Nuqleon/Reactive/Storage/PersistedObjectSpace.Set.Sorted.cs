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
        /// Creates a persisted sorted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier to use for the sorted set.</param>
        /// <returns>A new persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSortedSet<T> CreateSortedSet<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var set = new SortedSet(this);
            _items.Add(id, set);
            return CreateSortedSetCore<T>(id, set);
        }

        /// <summary>
        /// Gets a persisted sorted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier of the sorted set to retrieve.</param>
        /// <returns>An existing persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted set type.</exception>
        public IPersistedSortedSet<T> GetSortedSet<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateSortedSetCore<T>(id, (SortedSet)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier of the sorted set.</param>
        /// <param name="set">The storage entity representing the sorted set.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="set"/>.</returns>
        private static IPersistedSortedSet<T> CreateSortedSetCore<T>(string id, SortedSet set) => set.Create<T>(id);

        /// <summary>
        /// Storage entity representing a sorted set.
        /// </summary>
        private sealed class SortedSet : Set
        {
            /// <summary>
            /// Creates a new entity representing a sorted set.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public SortedSet(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.SortedSet"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.SortedSet;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the sorted set.</typeparam>
            /// <param name="id">The identifier of the sorted set.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedSortedSet<T> Create<T>(string id)
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
                    return (IPersistedSortedSet<T>)_wrapper;
                }
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory sorted set representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="Load"/>.
            /// If the entity has not been persisted before, this methods returns a new empty sorted set instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the sorted set.</typeparam>
            /// <returns>A pair consisting of an instance of type SortedSet{<typeparamref name="T"/>} containing the data represented by the storage entity, and a map that associates all set entries with their storage key.</returns>
            private (SortedSet<T> set, Map<T, long> storageKeys) Restore<T>()
            {
                //
                // The comparers to use for values, both in the set (to determine uniqueness) and the map (for the value-to-key entries).
                //
                // REVIEW: Consider making this configurable, which requires the persistence of an expression to construct the comparer in the metadata (likely using a wildcard type so it can be late-bound).
                //
                // NB: We assume that the default equality and ordinal comparers are consistent when it comes to performing equality checks (i.e. (Compare(a, b) == 0) == Equals(a, b)).
                //

                var eq = EqualityComparer<T>.Default;
                var cmp = Comparer<T>.Default;

                //
                // Always allocate an empty set and map. In case we were called for a new set, we'll just return the empty ones.
                //

                var set = new SortedSet<T>(cmp);
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
            private class Wrapper<T> : WrapperBase<T, SortedSet<T>>, IPersistedSortedSet<T>
            {
                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the set.</param>
                /// <param name="storage">The storage entity representing the set.</param>
                /// <param name="set">The initial set. This could either be the result of deserializing persisted state, or an empty set for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each element in <paramref name="set"/>.</param>
                public Wrapper(string id, Set storage, SortedSet<T> set, Map<T, long> storageKeys)
                    : base(id, storage, set, storageKeys)
                {
                }

                /// <summary>
                /// Gets the maximum value in the set.
                /// </summary>
                public T Max => _set.Max;

                /// <summary>
                /// Gets the minimum value in the set.
                /// </summary>
                public T Min => _set.Min;

                /// <summary>
                /// Returns a view of a subset in the set.
                /// </summary>
                /// <param name="lowerValue">The lowest desired value in the view.</param>
                /// <param name="upperValue">The highest desired value in the view.</param>
                /// <returns>A subset view that contains only the values in the specified range.</returns>
                /// <exception cref="ArgumentException"><paramref name="lowerValue" /> is more than <paramref name="upperValue" /> according to the set's comparer.</exception>
                /// <exception cref="ArgumentOutOfRangeException">A tried operation on the view was outside the range specified by <paramref name="lowerValue" /> and <paramref name="upperValue" />.</exception>
                public ISortedSet<T> GetViewBetween(T lowerValue, T upperValue)
                {
                    return new Subset(this, lowerValue, upperValue);
                }

                /// <summary>
                /// Returns an enumerable sequence that enumerates over the set in reverse order.
                /// </summary>
                /// <returns>An enumerator that iterates over the set in reverse order.</returns>
                public IEnumerable<T> Reverse() => _set.Reverse();

                /// <summary>
                /// Removes all elements in the specified <paramref name="other"/> collection from the set.
                /// </summary>
                /// <param name="other">The collection of items to remove from the set.</param>
                protected override void ExceptWithCore(IEnumerable<T> other)
                {
                    //
                    // Check if the other sequence is a sorted set with the same comparer, which enables us to take the Min and Max values into account to reduce work.
                    //

                    if (TryGetOtherAsSetWithSameComparer(other, out var set))
                    {
                        //
                        // We'll compare elements in the other set (which has the same comparer as the current set) to the current set's Min and Max values. Cache these.
                        //

                        var min = Min;
                        var max = Max;
                        var comparer = _set.Comparer;

                        //
                        // First check if the range of the specified set is outside the range of the current set. If so, don't do anything.
                        //

                        if (!(comparer.Compare(set.Max, min) < 0 || comparer.Compare(set.Min, max) > 0))
                        {
                            //
                            // Iterate over the other set, skipping over everything that's below our own Min value, and stopping after seeing a value beyond our own Max value.
                            //
                            // REVIEW: SortedSet<T> doesn't do this either, but would using GetViewBetween be more efficient rather than skipping over a bunch of values? It introduces an allocation though.
                            //

                            foreach (T item in other)
                            {
                                if (comparer.Compare(item, min) < 0)
                                {
                                    continue;
                                }

                                if (comparer.Compare(item, max) > 0)
                                {
                                    break;
                                }

                                Remove(item);
                            }
                        }
                    }
                    else
                    {
                        base.ExceptWithCore(other);
                    }
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present in the set and in the specified <paramref name="other"/> collection.
                /// </summary>
                /// <param name="other">The collection to compare to the set.</param>
                protected override void IntersectWithCore(IEnumerable<T> other)
                {
                    //
                    // We can't get the unique set of elements from other because there's no general way to relate an IComparer<T> to an IEqualityComparer<T> we could use for building a set representation of other.
                    //

                    var toSave = new List<T>(Count);

                    foreach (var item in other)
                    {
                        //
                        // Check if the current set contains the element to determine whether it's in the intersection. Then remove it so we don't consider it again.
                        //

                        if (Contains(item))
                        {
                            toSave.Add(item);

                            //
                            // NB: The other collection may be an alias for this. Don't run the risk of mutating the current collection inside this loop, e.g. by calling Remove.
                            //
                        }
                    }

                    //
                    // Clear the current set and substitute it for the elements we decided to keep because they were in the intersection.
                    //
                    // REVIEW: Rather than calling Clear here (and reinserting what to keep), consider removing what should be gone to reduce churn on the underlying store.
                    //

                    Clear();

                    foreach (var item in toSave)
                    {
                        Add(item);
                    }
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present either in the set or in the specified <paramref name="other"/> collection, but not both.
                /// </summary>
                /// <param name="other">The collection of items to compare to the current set.</param>
                protected override void SymmetricExceptWithCore(IEnumerable<T> other)
                {
                    //
                    // Check if the other sequence is a sorted set with the same comparer, in which case we don't have to worry about duplicates which would lead to multiple Remove/Add calls in the loop below, yielding a wrong result.
                    //

                    if (TryGetOtherAsSetWithSameComparer(other, out var set))
                    {
                        SymmetricExceptWithSet(set);
                    }
                    else
                    {
                        //
                        // Otherwise, iterate over the other sequence once to create an array and sort it using the set's comparer.
                        //
                        // PERF: We use LINQ here because its ToArray implementation is rather efficient with regards to buffering and checking for ICollection<T> to obtain a Count.
                        //

                        var array = System.Linq.Enumerable.ToArray(other);
                        System.Array.Sort(array, _set.Comparer);

                        SymmetricExceptWithArray(array);
                    }
                }

                /// <summary>
                /// Implementation of <see cref="SymmetricExceptWithCore(IEnumerable{T})"/> where the <paramref name="other"/> sequence is a sorted set.
                /// </summary>
                /// <param name="other">A sorted set.</param>
                private void SymmetricExceptWithSet(SortedSet<T> other)
                {
                    //
                    // NB: The set obtained here can't be an alias for this because the base class SymmetricExceptWith method checks for this condition.
                    //

                    foreach (var item in other)
                    {
                        if (Contains(item))
                        {
                            Remove(item);
                        }
                        else
                        {
                            Add(item);
                        }
                    }
                }

                /// <summary>
                /// Implementation of <see cref="SymmetricExceptWithCore(IEnumerable{T})"/> where the <paramref name="other"/> sequence is a sorted array.
                /// </summary>
                /// <param name="other">A sorted array.</param>
                private void SymmetricExceptWithArray(T[] other)
                {
                    //
                    // If the original sequence didn't pass any collection type tests, we may still see an empty array here. Simply return for this special case.
                    //

                    if (other.Length == 0)
                    {
                        return;
                    }

                    //
                    // Get the comparer once; we'll need it many times.
                    //

                    var comparer = _set.Comparer;

                    //
                    // After sorting the sequence by using an array, we may still have adjacent elements that are equal. Go through the array left-to-right and prune out these duplicates.
                    //

                    var last = other[0];

                    for (int i = 0; i < other.Length; i++)
                    {
                        //
                        // NB: The number or order of calls to Compare can't be predicted easily upfront due to the use of sorting and filtering of distinct elements in separate steps. We deem the comparer to be pure.
                        //

                        while (i < other.Length && i != 0 && comparer.Compare(other[i], last) == 0)
                        {
                            i++;
                        }

                        if (i >= other.Length)
                        {
                            break;
                        }

                        last = other[i];

                        if (Contains(last))
                        {
                            Remove(last);
                        }
                        else
                        {
                            Add(last);
                        }
                    }
                }

                /// <summary>
                /// Tries to convert the enumerable sequence specified in <paramref name="other"/> to a <see cref="SortedSet{T}"/> instance, provided it has the
                /// same comparer as the current <see cref="Wrapper{T}"/> instance's underlying set.
                /// </summary>
                /// <param name="other">The sequence to convert.</param>
                /// <param name="set">A <see cref="SortedSet{T}"/> instance if <paramref name="other"/> is convertible to this type and has the same comparer; otherwise, <c>null</c>.</param>
                /// <returns><c>true</c> if the conversion succeeded and the result is stored in <paramref name="set"/>; otherwise, <c>false</c>.</returns>
                private bool TryGetOtherAsSetWithSameComparer(IEnumerable<T> other, out SortedSet<T> set)
                {
                    set = AsSortedSet(other);

                    if (set != null && set.Comparer.Equals(_set.Comparer))
                    {
                        return true;
                    }

                    set = null;
                    return false;
                }

                /// <summary>
                /// Tries to convert the enumerable sequence specified in <paramref name="other"/> to a <see cref="SortedSet{T}"/> instance.
                /// </summary>
                /// <param name="other">The sequence to convert.</param>
                /// <returns>A <see cref="SortedSet{T}"/> instance if <paramref name="other"/> is convertible to this type; otherwise, <c>null</c>.</returns>
                private static SortedSet<T> AsSortedSet(IEnumerable<T> other)
                {
                    //
                    // Check if other is a HashSet<T> or a Wrapper<T> from which we can extract the _set field.
                    //

                    return other switch
                    {
                        SortedSet<T> set => set,
                        Wrapper<T> wrapper => wrapper._set,
                        _ => null,
                    };
                }

                /// <summary>
                /// Subset implementation for use by <see cref="GetViewBetween(T, T)"/>.
                /// </summary>
                private sealed class Subset : Wrapper<T>
                {
                    /// <summary>
                    /// Creates a new subset of a persisted set using the specified <paramref name="parent"/> to gain access to the underlying subset and the storage keys.
                    /// The subset in the underlying in-memory collection is obtained by using <paramref name="lowerValue"/> and <paramref name="upperValue"/> in a call to <see cref="SortedSet{T}.GetViewBetween(T, T)"/>.
                    /// </summary>
                    /// <param name="parent">The parent persisted set.</param>
                    /// <param name="lowerValue">The lowest desired value in the view.</param>
                    /// <param name="upperValue">The highest desired value in the view.</param>
                    public Subset(Wrapper<T> parent, T lowerValue, T upperValue)
                        : base(parent.Id, parent._storage, parent._set.GetViewBetween(lowerValue, upperValue), parent._storageKeys /* NB: See remarks below. */)
                    {
                    }

                    //
                    // NB: This class has a tight coupling with the behavior of its base class. The following methods cause mutation of the underlying subset.
                    //
                    //       Add
                    //         Performs an addition to the (sub)set, which may throw an exception if the value is not in range for the subset. If the check passes, it allocates a storage key. This works for subsets.
                    //
                    //       Remove
                    //         Performs a removal from the (sub)set and uses the Boolean return value to determine whether the storage key has to be deallocated. This works for subsets.
                    //
                    //       Clear
                    //         The base class implementation butchers all of the storage keys, which is not compatible with the logic for subsets. We don't keep the subset of storage keys in memory (see ctor).
                    //         We override the method here to do the right thing by issuing fine-grained RemoveCore calls to update the storage keys.
                    //
                    //       ExceptWith
                    //         Uses Count, Clear, and Remove virtual method calls on the current instance. Count is read-only and correctly obtains the count on the subset.
                    //         Remove is correct as-is (see above). Clear is overridden to apply to the subset (see above).
                    //
                    //       IntersectWith
                    //         Uses Count, Clear, Contains, and Add virtual method calls on the current instance, and Remove calls on the underlying (sub)set. Count and Contains are read-only and correctly pertain to the subset.
                    //         Clear is overridden to apply to the subset (see above). Add performs the required range checks on the subset (see above). Calls to Remove on the subset are guarded by a Contains check and are innocent due to the subsequent use of Clear.
                    //
                    //       SymmetricExceptWith
                    //         Uses Count, Clear, and UnionWith for special cases. These are all proven to be correct here. For the regular case, it uses Contains, Add, and Remove virtual method calls on the current instance.
                    //         All of these correctly apply to the subset instance and update storage keys accordingly after checking the outcome of the operation applied to the subset.
                    //
                    //       UnionWith
                    //         Uses Add virtual method calls on the current instance. These correctly perform additions on the subset with appropriate range checks (see above).
                    //         Note that UnionWith throws upon encountering the first element that does not fit the subset range, possibly leaving the set in a partially unioned state. This is consistent with BCL behavior.
                    //

                    /// <summary>
                    /// Removes all elements from the subset and in the underlying set.
                    /// </summary>
                    public override void Clear()
                    {
                        //
                        // Do a cheap check to bail out quickly and avoid unnecessary allocations.
                        //

                        if (Count == 0)
                        {
                            return;
                        }

                        //
                        // Enumerate over the subset to obtain all elements that need to be removed.
                        //

                        var removed = new List<T>(Count);

                        foreach (var item in this)
                        {
                            removed.Add(item);
                        }

                        //
                        // Clear the subset, which will remove the elements gathered above.
                        //
                        // NB: We keep going through the SortedSet<T>.GetViewBetween(T,T) subset that was obtained in the constructor, rather than going directly to the parent by calling Remove for each element.
                        //     This is required to mutate the state in the underlying subset, e.g. the _root, _count, and _version fields.
                        //

                        _set.Clear();

                        //
                        // Finally use the RemoveCore method to release the storage keys for the items that were removed.
                        //

                        foreach (var item in removed)
                        {
                            RemoveCore(item);
                        }
                    }
                }
            }
        }
    }
}
