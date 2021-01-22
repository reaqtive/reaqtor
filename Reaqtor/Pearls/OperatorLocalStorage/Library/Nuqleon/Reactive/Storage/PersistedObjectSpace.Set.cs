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
using System.IO;

using Reaqtive.Serialization;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Base class for storage entities representing a set.
        /// </summary>
        /// <remarks>
        /// Persistence of a set looks as follows:
        /// <c>
        /// items/0 = 42
        /// items/1 = 43
        /// items/2 = 46
        /// items/4 = 44
        /// items/6 = 45
        /// </c>
        /// The keys of the items are insignificant due to the unordered nature of the set. Holes can exist in the key range (e.g. 3 in the same above).
        /// <list type="bullet">
        ///   <item><c>set.Add(v)</c> results in <c> Add($"items/{getUnusedKey()}", v)</c></item>
        ///   <item><c>set.Remove(v)</c> results in <c>Delete($"items/{getKeyOf(v)}")</c></item>
        /// </list>
        /// </remarks>
        private abstract partial class Set : Heap
        {
            /// <summary>
            /// Creates a new entity representing a set.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Set(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Base class for statically typed wrappers for persisted sets with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the set.</typeparam>
            /// <typeparam name="TSet">The type of the in-memory set, inheriting from <see cref="ISet{T}"/>.</typeparam>
            protected abstract class WrapperBase<T, TSet> : PersistedBase, IHeapPersistence, ISet<T>
                where TSet : ISet<T>
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                protected readonly Set _storage;

                /// <summary>
                /// The stored set, always reflecting the latest in-memory state.
                /// </summary>
                protected readonly TSet _set;

                /// <summary>
                /// The bi-directional map used to associate the storage key used for each element in <see cref="_set"/>.
                /// </summary>
                protected readonly Map<T, long> _storageKeys;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the set.</param>
                /// <param name="storage">The storage entity representing the set.</param>
                /// <param name="set">The initial set. This could either be the result of deserializing persisted state, or an empty set for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each element in <paramref name="set"/>.</param>
                public WrapperBase(string id, Set storage, TSet set, Map<T, long> storageKeys)
                    : base(id)
                {
                    _storage = storage;
                    _set = set;
                    _storageKeys = storageKeys;
                }

                /// <summary>
                /// Gets the number of elements in the set.
                /// </summary>
                public int Count => _set.Count;

                /// <summary>
                /// Gets a value indicating whether the set is read-only. Always returns <c>false</c>.
                /// </summary>
                public bool IsReadOnly => false;

                /// <summary>
                /// Adds the specified <paramref name="item"/> to the set.
                /// </summary>
                /// <param name="item">The item to add to the set.</param>
                /// <returns><c>true</c> if the <paramref name="item"/> was added to the set; <c>false</c> if the element is already present.</returns>
                public bool Add(T item)
                {
                    //
                    // Add the item to the underlying set and allocate a key from the storage entity if the item was not already present.
                    //

                    if (_set.Add(item))
                    {
                        AddCore(item);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Allocates a storage key for the specified <paramref name="item"/> that was added to the set.
                /// </summary>
                /// <param name="item">The item that was added to the set.</param>
                protected void AddCore(T item)
                {
                    // NB: We use Add on the _storageKey map to catch failed invariants. In particular:
                    //
                    //     * _set and _storageKeys should always be updated together, and,
                    //     * keys returned from the storage entity Add method should not be in use in _storageKeys.
                    //

                    var key = _storage.Add();
                    _storageKeys.Add(item, key);
                }

                /// <summary>
                /// Removes all elements from the set.
                /// </summary>
                public virtual void Clear()
                {
                    //
                    // Get all the keys currently in use. This is more efficient than having the storage entity apply all edit pages to _keys to derive this set.
                    //

                    var keys = new SortedSet<long>(_storageKeys.Values);

                    //
                    // Clear the set and the map containing the storage keys.
                    //

                    _set.Clear();
                    _storageKeys.Clear();

                    //
                    // Mark all the keys as deleted in the storage entity.
                    //

                    _storage.Clear(keys);
                }

                /// <summary>
                /// Determines whether the set contains the specified <paramref name="item"/>.
                /// </summary>
                /// <param name="item">The element to locate in the set.</param>
                /// <returns><c>true</c> if the set contains the specified <paramref name="item"/>; otherwise, <c>false</c>.</returns>
                public bool Contains(T item) => _set.Contains(item);

                /// <summary>
                /// Copies the elements of the set to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
                /// </summary>
                /// <param name="array">The array to copy the set elements to.</param>
                /// <param name="arrayIndex">The index in the array to copy the first element to.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
                /// <exception cref="ArgumentException">The number of elements in the set is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
                public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

                /// <summary>
                /// Removes all elements in the specified <paramref name="other"/> collection from the set.
                /// </summary>
                /// <param name="other">The collection of items to remove from the set.</param>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public void ExceptWith(IEnumerable<T> other)
                {
                    if (other == null)
                        throw new ArgumentNullException(nameof(other));

                    //
                    // Special case: {} \ X = {}
                    //

                    if (Count == 0)
                    {
                        return;
                    }

                    //
                    // Special case: X \ X = {}
                    //
                    // NB: This check also prevents iterating over other when it's an alias for this, which could lead to enumeration failure.
                    //     Note we don't have to check for reference equality between other and _set because the _set reference should never leak the wrapper.
                    //

                    if (other == this)
                    {
                        Clear();
                        return;
                    }

                    //
                    // Regular case.
                    //

                    ExceptWithCore(other);
                }

                /// <summary>
                /// Removes all elements in the specified <paramref name="other"/> collection from the set.
                /// </summary>
                /// <param name="other">The collection of items to remove from the set.</param>
                protected virtual void ExceptWithCore(IEnumerable<T> other)
                {
                    //
                    // Iterate over the specified collection and remove the elements one by one. Note that multiple calls to Remove for the same element are valid.
                    //

                    foreach (var item in other)
                    {
                        Remove(item);
                    }
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the set. Enumeration order is undefined.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the set.</returns>
                public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

                /// <summary>
                /// Modifies the set to contain only elements that are present in the set and in the specified <paramref name="other"/> collection.
                /// </summary>
                /// <param name="other">The collection to compare to the set.</param>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public void IntersectWith(IEnumerable<T> other)
                {
                    if (other == null)
                        throw new ArgumentNullException(nameof(other));

                    //
                    // Special case: {} /\ X = {}
                    //

                    if (Count == 0)
                    {
                        return;
                    }

                    //
                    // Special case: X /\ {} = {}
                    //
                    // NB: There's no danger of other aliasing this and causing enumeration failure because we never edit the set during enumeration (see below).
                    //     Note we don't have to check for reference equality between other and _set because the _set reference should never leak the wrapper.
                    //

                    if (other is ICollection<T> collection && collection.Count == 0)
                    {
                        Clear();
                        return;
                    }

                    //
                    // Regular case.
                    //

                    IntersectWithCore(other);
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present in the set and in the specified <paramref name="other"/> collection.
                /// </summary>
                /// <param name="other">The collection to compare to the set.</param>
                protected abstract void IntersectWithCore(IEnumerable<T> other);

                /// <summary>
                /// Determines whether the set is a proper subset of the collection specified in <paramref name="other"/>.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set is a proper subset of the collection specified in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

                /// <summary>
                /// Determines whether the set is a proper superset of the collection specified in <paramref name="other"/>.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set is a proper superset of the collection specified in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

                /// <summary>
                /// Determines whether the set is a subset of the collection specified in <paramref name="other"/>.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set is a subset of the collection specified in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

                /// <summary>
                /// Determines whether the set is a superset of the collection specified in <paramref name="other"/>.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set is a superset of the collection specified in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

                /// <summary>
                /// Determines whether the set and the collection specified in <paramref name="other"/> share common elements.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set and the collection specified in <paramref name="other"/> share at least one common element; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

                /// <summary>
                /// Removes the specified <paramref name="item"/> from the set.
                /// </summary>
                /// <param name="item">The element to remove.</param>
                /// <returns><c>true</c> if the element specified in <paramref name="item"/> is successfully found and removed; otherwise, <c>false</c>.</returns>
                public bool Remove(T item)
                {
                    //
                    // Remove the item from the underlying set and free the key from the storage entity if the item was found and removed.
                    //

                    if (_set.Remove(item))
                    {
                        RemoveCore(item);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Marks the storage key of the specified <paramref name="item"/> which has been removed from the set as unused.
                /// </summary>
                /// <param name="item">The item that has been removed from the set.</param>
                protected virtual void RemoveCore(T item)
                {
                    //
                    // NB: We use the Remove method on the _storageKey map to catch failed invariants when the key is not found. In particular:
                    //
                    //     * _set and _storageKeys should always be updated together, and,
                    //     * an item found in the set should always have a key in the _storageKeys map.
                    //

                    var key = _storageKeys.Remove(item);
                    _storage.Remove(key);
                }

                /// <summary>
                /// Determines whether the set and the collection specified in <paramref name="other"/> contain the same elements.
                /// </summary>
                /// <param name="other">The collection to compare to the current set.</param>
                /// <returns><c>true</c> if the set is equal to the collection specified in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

                /// <summary>
                /// Modifies the set to contain only elements that are present either in the set or in the specified <paramref name="other"/> collection, but not both.
                /// </summary>
                /// <param name="other">The collection of items to compare to the current set.</param>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public void SymmetricExceptWith(IEnumerable<T> other)
                {
                    if (other == null)
                        throw new ArgumentNullException(nameof(other));

                    //
                    // Special case: {} ^ X = {} \/ X = X
                    //
                    //

                    if (Count == 0)
                    {
                        UnionWith(other);
                        return;
                    }

                    //
                    // Special case: X ^ X = {}
                    //
                    // NB: There's no danger of other aliasing this and causing enumeration failure because we never edit the set during enumeration (see below).
                    //     Note we don't have to check for reference equality between other and _set because the _set reference should never leak the wrapper.
                    //

                    if (other == this)
                    {
                        Clear();
                        return;
                    }

                    //
                    // Regular case.
                    //

                    SymmetricExceptWithCore(other);
                }

                /// <summary>
                /// Modifies the set to contain only elements that are present either in the set or in the specified <paramref name="other"/> collection, but not both.
                /// </summary>
                /// <param name="other">The collection of items to compare to the current set.</param>
                protected abstract void SymmetricExceptWithCore(IEnumerable<T> other);

                /// <summary>
                /// Modifies the set to contain all elements that are present in itself, the specified <paramref name="other"/> collection, or both.
                /// </summary>
                /// <param name="other">The collection of items to union with the current set.</param>
                /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
                public void UnionWith(IEnumerable<T> other)
                {
                    if (other == null)
                        throw new ArgumentNullException(nameof(other));

                    //
                    // Special case: X \/ X = X
                    //
                    // NB: This check also prevents iterating over other when it's an alias for this, which could lead to enumeration failure.
                    //     Note we don't have to check for reference equality between other and _set because the _set reference should never leak the wrapper.
                    //

                    if (other == this)
                    {
                        return;
                    }

                    //
                    // Add the elements one by one. Note that duplicate items are safe to add multiple times.
                    //

                    foreach (var item in other)
                    {
                        Add(item);
                    }
                }

                /// <summary>
                /// Adds the specified <paramref name="item"/> to the set.
                /// </summary>
                /// <param name="item">The item to add to the set.</param>
                void ICollection<T>.Add(T item) => Add(item);

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the set. Enumeration order is undefined.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the set.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the set element stored at the specified <paramref name="key"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the set element to.</param>
                /// <param name="key">The key of the set element to save.</param>
                void IHeapPersistence.Save(ISerializerFactory serializerFactory, Stream stream, string key)
                {
                    var keyValue = GetIndexForKey(key);
                    var value = _storageKeys.GetByValue(keyValue);
                    serializerFactory.GetSerializer<T>().Serialize(value, stream);
                }
            }
        }
    }
}
