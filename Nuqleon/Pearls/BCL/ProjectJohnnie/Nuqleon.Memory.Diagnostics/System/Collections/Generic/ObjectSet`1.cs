// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#define NO_HASH

using System.Diagnostics;
using System.Memory;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    //
    // NB: The code in this class is adapted from the github.com/dotnet/corefx code for HashSet<T> by
    //     specializing it for sets of (reference-typed) objects using reference equality as the set
    //     membership condition. This results in an increased performance due to the following:
    //
    //       - Omission of the hashCode field in Slot values. We need to check for reference equality
    //         anyway when checking whether an entry in a slot matches an element being looked up.
    //       - Removal of the indirection through an IEqualityComparer<T> in favor of direct use of
    //         the RuntimeHelpers.GetHashCode method and the == operator for reference equality.
    //
    //     Besides the changes mentioned above, other changes include the addition of a field to track
    //     whether `null` is in the set, various improvements to the comments and documentation, removal
    //     of some unused functionality, and modernization using recent C# language features.
    //

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1033 // Make 'ObjectSet' sealed. (Keeping in sync with corefx implementation.)

    /// <summary>
    /// Specialized implementation of <see cref="ISet{T}"/> using reference equality comparison
    /// to determine set membership.
    /// </summary>
    /// <typeparam name="T">The type of the objects stored in the set. This type should be a reference type.</typeparam>
    public class ObjectSet<T> : ISet<T>, IClearable
        where T : class
    {
        /// <summary>
        /// Used to obtain lower 31 bits of hash code.
        /// </summary>
        private const int Lower31BitMask = 0x7FFFFFFF;

        /// <summary>
        /// Cutoff point, above which we won't do stackallocs. This corresponds to 100 integers.
        /// </summary>
        private const int StackAllocThreshold = 100;

        /// <summary>
        /// When constructing a hashset from an existing collection, it may contain duplicates,
        /// so this is used as the max acceptable excess ratio of capacity to count.
        ///
        /// Note that this is only used by the constructor and not to automatically shrink if the
        /// set has, e.g, a lot of adds followed by removes. Users must explicitly shrink by calling
        /// <see cref="TrimExcess"/>.
        ///
        /// This is set to 3 because capacity is acceptable as 2x rounded up to nearest prime.
        /// </summary>
        private const int ShrinkThreshold = 3;

        /// <summary>
        /// The buckets array where the indexes correspond to element hash codes modulo the length
        /// of the buckets array. The values of the array map to the index in <see cref="_slots"/>
        /// holding the first element for the bucket.
        /// </summary>
        private int[] _buckets;

        /// <summary>
        /// The entries of the set, where each entry contains a value of type <c>T</c> and an index
        /// to the next slot that belongs to the same bucket.
        /// </summary>
        private Slot[] _slots;

        /// <summary>
        /// The index of the last slot that's in use. Upon adding elements to the set, this field
        /// is consulted and incremented to obtain the next slot. When the <see cref="_freeList"/>
        /// is non-empty, indicated by a value that's non-negative, the free list is used prior to
        /// consulting this field to obtain a new slot index.
        /// </summary>
        private int _lastIndex;

        /// <summary>
        /// The index of the first slot that's free, due to a removal of an element from the set.
        /// Even though this is a single field, it marks the head of a free list which is obtained
        /// by traversing the <see cref="Slot.next"/> field until a negative value is encountered.
        /// When this field is set to a negative value, the free list is empty.
        /// </summary>
        private int _freeList;

        /// <summary>
        /// The current version of the set, used to protect against enumeration during mutation.
        /// </summary>
        private int _version;

        /// <summary>
        /// A Boolean value indicating whether the set contains <c>null</c>.
        /// </summary>
        private bool _containsNull;

        /// <summary>
        /// Creates a new empty set.
        /// </summary>
        public ObjectSet()
        {
            _lastIndex = 0;
            Count = 0;
            _freeList = -1;
            _version = 0;
        }

        /// <summary>
        /// Creates a new set containing the elements from the specified <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements to add to the set.</param>
        public ObjectSet(IEnumerable<T> collection)
            : this()
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (collection is ObjectSet<T> otherAsObjectSet)
            {
                CopyFrom(otherAsObjectSet);
            }
            else
            {
                // to avoid excess resizes, first set size based on collection's count. Collection
                // may contain duplicates, so call TrimExcess if resulting hashset is larger than
                // threshold
                int suggestedCapacity = collection is not ICollection<T> coll ? 0 : coll.Count;
                Initialize(suggestedCapacity);

                UnionWith(collection);

                if (Count > 0 && _slots.Length / Count > ShrinkThreshold)
                {
                    TrimExcess();
                }
            }
        }

        /// <summary>
        /// Initializes the current set from the specified other set.
        /// </summary>
        /// <param name="source">The other set to initialize from.</param>
        private void CopyFrom(ObjectSet<T> source)
        {
            int count = source.Count;

            if (count == 0)
            {
                // As well as short-circuiting on the rest of the work done,
                // this avoids errors from trying to access otherAsObjectSet._buckets
                // or otherAsObjectSet._slots when they aren't initialized.
                return;
            }

            int capacity = source._buckets.Length;
            int threshold = HashHelpers.ExpandPrime(count + 1);

            if (threshold >= capacity)
            {
                _buckets = (int[])source._buckets.Clone();
                _slots = (Slot[])source._slots.Clone();

                _lastIndex = source._lastIndex;
                _freeList = source._freeList;
            }
            else
            {
                int lastIndex = source._lastIndex;
                Slot[] slots = source._slots;

                Initialize(count);

                int index = 0;
                for (int i = 0; i < lastIndex; ++i)
                {
                    if (slots[i].value != null)
                    {
                        AddValue(index, slots[i].value);
                        ++index;
                    }
                }

                Debug.Assert(index == count);

                _lastIndex = index;
            }

            _containsNull = source._containsNull;
            Count = count;
        }

        /// <summary>
        /// Adds the specified <paramref name="item"/> to this set.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <seealso cref="Add(T)"/>
        void ICollection<T>.Add(T item) => AddIfNotPresent(item);

        /// <summary>
        /// Removes all elements from this set.
        /// </summary>
        /// <remarks>
        /// This method clears the underlying data structures but does not shrink them. In order to
        /// achieve shrinking, consider calling <see cref="TrimExcess"/> after calling this method.
        /// </remarks>
        public void Clear()
        {
            if (_lastIndex > 0)
            {
                Debug.Assert(_buckets != null, "_buckets was null but _lastIndex > 0");

                // clear the elements so that the gc can reclaim the references.
                // clear only up to _lastIndex for _slots
                Array.Clear(_slots, 0, _lastIndex);
                Array.Clear(_buckets, 0, _buckets.Length);

                _lastIndex = 0;
                _containsNull = false;
                Count = 0;
                _freeList = -1;
            }

            _version++;
        }

        /// <summary>
        /// Checks if this set contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns><c>true</c> if the item occurs in the set; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            if (item == null)
            {
                return _containsNull;
            }

            if (_buckets != null)
            {
                int hashCode = InternalGetHashCode(item);

                // see note at "ObjectSet" level describing why "- 1" appears in for loop
                for (int i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].next)
                {
                    if (_slots[i].value == item)
                    {
                        return true;
                    }
                }
            }

            // either _buckets is null or wasn't found
            return false;
        }

        /// <summary>
        /// Copy items in this set to the specified <paramref name="array"/>, starting at <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">The array to copy items to.</param>
        /// <param name="arrayIndex">The index to start at.</param>
        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex, Count);

        /// <summary>
        /// Removes the specified <paramref name="item"/> from this set.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was removed; otherwise, <c>false</c> (i.e. the set didn't contain the item).</returns>
        public bool Remove(T item)
        {
            if (item == null)
            {
                if (_containsNull)
                {
                    _containsNull = false;
                    Count--;

                    return true;
                }

                return false;
            }

            if (_buckets != null)
            {
                int hashCode = InternalGetHashCode(item);
                int bucket = hashCode % _buckets.Length;
                int last = -1;

                for (int i = _buckets[bucket] - 1; i >= 0; last = i, i = _slots[i].next)
                {
                    if (_slots[i].value == item)
                    {
                        if (last < 0)
                        {
                            // first iteration; update buckets
                            _buckets[bucket] = _slots[i].next + 1;
                        }
                        else
                        {
                            // subsequent iterations; update 'next' pointers
                            _slots[last].next = _slots[i].next;
                        }

                        _slots[i].value = default;
                        _slots[i].next = _freeList;

                        Count--;
                        _version++;

                        if (Count == 0)
                        {
                            _lastIndex = 0;
                            _freeList = -1;
                        }
                        else
                        {
                            _freeList = i;
                        }

                        return true;
                    }
                }
            }

            // either _buckets is null or wasn't found
            return false;
        }

        /// <summary>
        /// Gets the number of elements in this set.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Gets an enumerator used to enumerate over the elements in this set.
        /// </summary>
        /// <returns>An enumerator used to enumerate over the elements in this set.</returns>
        public Enumerator GetEnumerator() => new(this);

        /// <summary>
        /// Gets an enumerator used to enumerate over the elements in this set.
        /// </summary>
        /// <returns>An enumerator used to enumerate over the elements in this set.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Gets an enumerator used to enumerate over the elements in this set.
        /// </summary>
        /// <returns>An enumerator used to enumerate over the elements in this set.</returns>
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Adds the specified <paramref name="item"/> to this set.
        /// </summary>
        /// <param name="item">The item to add to the set.</param>
        /// <returns><c>true</c> if the item was added; otherwise, <c>false</c> (i.e. the item was already in the set).</returns>
        public bool Add(T item) => AddIfNotPresent(item);

        /// <summary>
        /// Modifies the current set to contain all elements that are present in both itself and in the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to add to the current set.</param>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            foreach (T item in other)
            {
                AddIfNotPresent(item);
            }
        }

        /// <summary>
        /// Modifies the current set to contain only elements that are present in that object and in the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to compute the intersection with.</param>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // intersection of anything with empty set is empty set, so return if count is 0
            if (Count == 0)
            {
                return;
            }

            // set intersecting with itself is the same set
            if (other == this)
            {
                return;
            }

            // if other is empty, intersection is empty set; remove all elements and we're done
            // can only figure this out if implements ICollection<T>. (IEnumerable<T> has no count)
            if (other is ICollection<T> otherAsCollection)
            {
                if (otherAsCollection.Count == 0)
                {
                    Clear();
                    return;
                }

                if (other is ObjectSet<T> otherAsSet)
                {
                    IntersectWithObjectSet(otherAsSet);
                    return;
                }
            }

            IntersectWithEnumerable(other);
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the current set.</param>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // this is already the empty set; return
            if (Count == 0)
            {
                return;
            }

            // special case if other is this; a set minus itself is the empty set
            if (other == this)
            {
                Clear();
                return;
            }

            // remove every element in other from this
            foreach (T element in other)
            {
                Remove(element);
            }
        }

        /// <summary>
        /// Modifies the current set to contain only elements that are present either in that object or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // if set is empty, then symmetric difference is other
            if (Count == 0)
            {
                UnionWith(other);
                return;
            }

            // special case this; the symmetric difference of a set with itself is the empty set
            if (other == this)
            {
                Clear();
                return;
            }

            if (other is ObjectSet<T> otherAsSet)
            {
                SymmetricExceptWithUniqueObjectSet(otherAsSet);
            }
            else
            {
                SymmetricExceptWithEnumerable(other);
            }
        }

        /// <summary>
        /// Determines whether this set is a subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if this set is a subset of the specified collection; otherwise, <c>false</c>.</returns>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // The empty set is a subset of any set
            if (Count == 0)
            {
                return true;
            }

            // Set is always a subset of itself
            if (other == this)
            {
                return true;
            }

            if (other is ObjectSet<T> otherAsSet)
            {
                // if this has more elements then it can't be a subset
                if (Count > otherAsSet.Count)
                {
                    return false;
                }

                return IsSubsetOfObjectSet(otherAsSet);
            }
            else
            {
                ElementCount result = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);
                return (result.uniqueCount == Count && result.unfoundCount >= 0);
            }
        }

        /// <summary>
        /// Determines whether this set is a proper subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if this set is a proper subset of the specified collection; otherwise, <c>false</c>.</returns>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // no set is a proper subset of itself.
            if (other == this)
            {
                return false;
            }

            if (other is ICollection<T> otherAsCollection)
            {
                // no set is a proper subset of an empty set
                if (otherAsCollection.Count == 0)
                {
                    return false;
                }

                // the empty set is a proper subset of anything but the empty set
                if (Count == 0)
                {
                    return otherAsCollection.Count > 0;
                }

                if (other is ObjectSet<T> otherAsSet)
                {
                    if (Count >= otherAsSet.Count)
                    {
                        return false;
                    }

                    // this has strictly less than number of items in other, so the following
                    // check suffices for proper subset.
                    return IsSubsetOfObjectSet(otherAsSet);
                }
            }

            ElementCount result = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);

            return result.uniqueCount == Count && result.unfoundCount > 0;
        }

        /// <summary>
        /// Determines whether this set is a superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if this set is a superset of the specified collection; otherwise, <c>false</c>.</returns>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // a set is always a superset of itself
            if (other == this)
            {
                return true;
            }

            // try to fall out early based on counts
            if (other is ICollection<T> otherAsCollection)
            {
                // if other is the empty set then this is a superset
                if (otherAsCollection.Count == 0)
                {
                    return true;
                }

                if (other is ObjectSet<T> otherAsSet && otherAsSet.Count > Count)
                {
                    return false;
                }
            }

            return ContainsAllElements(other);
        }

        /// <summary>
        /// Determines whether this set is a proper superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if this set is a proper superset of the specified collection; otherwise, <c>false</c>.</returns>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // the empty set isn't a proper superset of any set.
            if (Count == 0)
            {
                return false;
            }

            // a set is never a strict superset of itself
            if (other == this)
            {
                return false;
            }

            if (other is ICollection<T> otherAsCollection)
            {
                // if other is the empty set then this is a superset
                if (otherAsCollection.Count == 0)
                {
                    // note that this has at least one element, based on above check
                    return true;
                }

                if (other is ObjectSet<T> otherAsSet)
                {
                    if (otherAsSet.Count >= Count)
                    {
                        return false;
                    }

                    // now perform element check
                    return ContainsAllElements(otherAsSet);
                }
            }

            // couldn't fall out in the above cases; do it the long way
            ElementCount result = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
            return result.uniqueCount < Count && result.unfoundCount == 0;
        }

        /// <summary>
        /// Determines whether the current set and a specified collection share common elements.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if the current set and the specified collection have at least one common element; otherwise, <c>false</c>.</returns>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
            {
                return false;
            }

            // set overlaps itself
            if (other == this)
            {
                return true;
            }

            foreach (T element in other)
            {
                if (Contains(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether this set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection of items to compare with the current set.</param>
        /// <returns><c>true</c> if the current set and the specified collection have the same elements; otherwise, <c>false</c>.</returns>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // a set is equal to itself
            if (other == this)
            {
                return true;
            }

            if (other is ObjectSet<T> otherAsSet)
            {
                // attempt to return early: since both contain unique elements, if they have
                // different counts, then they can't be equal
                if (Count != otherAsSet.Count)
                {
                    return false;
                }

                // already confirmed that the sets have the same number of distinct elements, so if
                // one is a superset of the other then they must be equal
                return ContainsAllElements(otherAsSet);
            }
            else
            {
                if (other is ICollection<T> otherAsCollection)
                {
                    // if this count is 0 but other contains at least one element, they can't be equal
                    if (Count == 0 && otherAsCollection.Count > 0)
                    {
                        return false;
                    }
                }

                ElementCount result = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
                return result.uniqueCount == Count && result.unfoundCount == 0;
            }
        }

        /// <summary>
        /// Copy items in this set to the specified <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The array to copy items to.</param>
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0, Count);
        }

        /// <summary>
        /// Copy the specified number of items in this set to the specified <paramref name="array"/>,
        /// starting at <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">The array to copy items to.</param>
        /// <param name="arrayIndex">The index to start at.</param>
        /// <param name="count">The number of items to copy.</param>
        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            // check array index valid index into array
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, "The specified array index should be greater than or equal to 0.");

            // also throw if count less than 0
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "The specified count should be greater than or equal to 0.");

            // will array, starting at arrayIndex, be able to hold elements? Note: not
            // checking arrayIndex >= array.Length (consistency with list of allowing
            // count of 0; subsequent check takes care of the rest)
            if (arrayIndex > array.Length || count > array.Length - arrayIndex)
                throw new ArgumentException("The specified array is too small.");

            int numCopied = 0;

            if (_containsNull)
            {
                array[arrayIndex] = null;
                numCopied++;
            }

            for (int i = 0; i < _lastIndex && numCopied < count; i++)
            {
                if (_slots[i].value != null)
                {
                    array[arrayIndex + numCopied] = _slots[i].value;
                    numCopied++;
                }
            }
        }

        /// <summary>
        /// Sets the capacity of the set to the actual number of elements it contains, rounded up to a nearby
        /// implementation-specific value.
        /// </summary>
        public void TrimExcess()
        {
            Debug.Assert(Count >= 0, "_count is negative");

            if (Count == 0)
            {
                // if count is zero, clear references
                _buckets = null;
                _slots = null;
                _version++;
            }
            else
            {
                Debug.Assert(_buckets != null, "_buckets was null but _count > 0");

                // similar to IncreaseCapacity but moves down elements in case add/remove/etc
                // caused fragmentation
                int newSize = HashHelpers.GetPrime(Count);
                Slot[] newSlots = new Slot[newSize];
                int[] newBuckets = new int[newSize];

                // move down slots and rehash at the same time. newIndex keeps track of current
                // position in newSlots array
                int newIndex = 0;
                for (int i = 0; i < _lastIndex; i++)
                {
                    if (_slots[i].value != null)
                    {
                        newSlots[newIndex] = _slots[i];

                        // rehash
                        int bucket = InternalGetHashCode(newSlots[newIndex].value) % newSize;
                        newSlots[newIndex].next = newBuckets[bucket] - 1;
                        newBuckets[bucket] = newIndex + 1;

                        newIndex++;
                    }
                }

                Debug.Assert(newSlots.Length <= _slots.Length, "capacity increased after TrimExcess");

                _lastIndex = newIndex;
                _slots = newSlots;
                _buckets = newBuckets;
                _freeList = -1;
            }
        }

        #region Helper methods

        /// <summary>
        /// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime
        /// greater than or equal to capacity.
        /// </summary>
        /// <param name="capacity">The suggested capacity.</param>
        private void Initialize(int capacity)
        {
            Debug.Assert(_buckets == null, "Initialize was called but _buckets was non-null");

            int size = HashHelpers.GetPrime(capacity);

            _buckets = new int[size];
            _slots = new Slot[size];
        }

        /// <summary>
        /// Expand to new capacity. New capacity is next prime greater than or equal to suggested
        /// size. This is called when the underlying array is filled. This performs no
        /// defragmentation, allowing faster execution; note that this is reasonable since
        /// <see cref="AddIfNotPresent(T)"/> attempts to insert new elements in re-opened spots.
        /// </summary>
        private void IncreaseCapacity()
        {
            Debug.Assert(_buckets != null, "IncreaseCapacity called on a set with no elements");

            int newSize = HashHelpers.ExpandPrime(Count);
            if (newSize <= Count)
            {
                throw new ArgumentException("The maximum capacity has been reached.");
            }

            Slot[] newSlots = new Slot[newSize];

            if (_slots != null)
            {
                Array.Copy(_slots, 0, newSlots, 0, _lastIndex);
            }

            int[] newBuckets = new int[newSize];

            for (int i = 0; i < _lastIndex; i++)
            {
                int bucket = InternalGetHashCode(newSlots[i].value) % newSize;
                newSlots[i].next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }

            _slots = newSlots;
            _buckets = newBuckets;
        }

        /// <summary>
        /// Adds the specified <paramref name="value"/> to the set if it's not already present in the set.
        /// </summary>
        /// <param name="value">The value to find in or add to the set.</param>
        /// <returns><c>true</c> if the value was added; <c>false</c> if the value was already in the set.</returns>
        private bool AddIfNotPresent(T value)
        {
            if (value == null)
            {
                if (!_containsNull)
                {
                    _containsNull = true;
                    Count++;
                    _version++;

                    return true;
                }

                return false;
            }

            if (_buckets == null)
            {
                Initialize(0);
            }

            int hashCode = InternalGetHashCode(value);
            int bucket = hashCode % _buckets.Length;

            for (int i = _buckets[bucket] - 1; i >= 0; i = _slots[i].next)
            {
                if (_slots[i].value == value)
                {
                    return false;
                }
            }

            int index;
            if (_freeList >= 0)
            {
                index = _freeList;
                _freeList = _slots[index].next;
            }
            else
            {
                if (_lastIndex == _slots.Length)
                {
                    IncreaseCapacity();
                    // this will change during resize
                    bucket = hashCode % _buckets.Length;
                }
                index = _lastIndex;
                _lastIndex++;
            }

            Slot s = new Slot
            {
                value = value,
                next = _buckets[bucket] - 1
            };

            _slots[index] = s;

            _buckets[bucket] = index + 1;
            Count++;
            _version++;

            return true;
        }

        /// <summary>
        /// Adds the specified <paramref name="value"/> at the slot with the specified <paramref name="index"/>.
        /// Used during construction of a set using an existing collection.
        /// </summary>
        /// <param name="index">The index of the slot.</param>
        /// <param name="value">The value to add.</param>
        private void AddValue(int index, T value)
        {
            var hashCode = InternalGetHashCode(value);
            int bucket = hashCode % _buckets.Length;

            Debug.Assert(_freeList == -1);
            _slots[index].value = value;
            _slots[index].next = _buckets[bucket] - 1;
            _buckets[bucket] = index + 1;
        }

        /// <summary>
        /// Checks if this contains all of other's elements. Iterates over other's elements and
        /// returns <c>false</c> as soon as it finds an element in other that's not in this. Used
        /// by <see cref="IsSupersetOf"/>, <see cref="IsProperSupersetOf"/>, and <see cref="SetEquals"/>.
        /// </summary>
        /// <param name="other">The collection to check against.</param>
        /// <returns><c>true</c> if this set contains all elements in <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        private bool ContainsAllElements(IEnumerable<T> other)
        {
            foreach (T element in other)
            {
                if (!Contains(element))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Optimized subset operation where the other collection is an object set itself.
        /// </summary>
        /// <param name="other">The object set to check against.</param>
        /// <returns><c>true</c> if the current set is a subset of the specified set; otherwise, <c>false</c>.</returns>
        private bool IsSubsetOfObjectSet(ObjectSet<T> other)
        {
            foreach (T item in this)
            {
                if (!other.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Optimized intersection where the other collection is an object set itself.
        /// </summary>
        /// <param name="other">The object set to intersect with.</param>
        private void IntersectWithObjectSet(ObjectSet<T> other)
        {
            if (_containsNull && !other.Contains(item: null))
            {
                Remove(item: null);
            }

            for (int i = 0; i < _lastIndex; i++)
            {
                if (_slots[i].value != null)
                {
                    T item = _slots[i].value;
                    if (!other.Contains(item))
                    {
                        Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Implementation of intersection using an enuemrable collection. The implementation iterates over
        /// the specified <paramref name="other"/> collection. If an element is contained in the current set,
        /// it marks the element in bit array corresponding to its position in _slots. If anything is unmarked
        /// in the bit array, it gets removed from the current set.
        /// </summary>
        /// <param name="other">The collection to intersect with.</param>
        /// <remarks>
        /// This attempts to allocate on the stack, if below <see cref="StackAllocThreshold"/>.
        /// </remarks>
        private unsafe void IntersectWithEnumerable(IEnumerable<T> other)
        {
            Debug.Assert(_buckets != null, "_buckets shouldn't be null; callers should check first");

            // keep track of current last index; don't want to move past the end of our bit array
            // (could happen if another thread is modifying the collection)
            int originalLastIndex = _lastIndex;
            int intArrayLength = BitHelper.ToIntArrayLength(originalLastIndex);

            BitHelper bitHelper;
            if (intArrayLength <= StackAllocThreshold)
            {
                int* bitArrayPtr = stackalloc int[intArrayLength];
                bitHelper = new BitHelper(bitArrayPtr, intArrayLength);
            }
            else
            {
                int[] bitArray = new int[intArrayLength];
                bitHelper = new BitHelper(bitArray, intArrayLength);
            }

            var otherContainsNull = false;

            // mark if contains: find index of in slots array and mark corresponding element in bit array
            foreach (T item in other)
            {
                if (item == null)
                {
                    otherContainsNull = true;
                }
                else
                {
                    int index = InternalIndexOf(item);
                    if (index >= 0)
                    {
                        bitHelper.MarkBit(index);
                    }
                }
            }

            if (_containsNull && !otherContainsNull)
            {
                Remove(item: null);
            }

            // if anything unmarked, remove it. Perf can be optimized here if BitHelper had a
            // FindFirstUnmarked method.
            for (int i = 0; i < originalLastIndex; i++)
            {
                if (_slots[i].value != null && !bitHelper.IsMarked(i))
                {
                    Remove(_slots[i].value);
                }
            }
        }

        /// <summary>
        /// Used internally by set operations which have to rely on bit array marking. This is like
        /// <see cref="Contains(T)"/> but returns index in slots array.
        /// </summary>
        /// <param name="item">The item to look up.</param>
        /// <returns>The index of the <paramref name="item"/> if found; otherwise, <c>-1</c>.</returns>
        private int InternalIndexOf(T item)
        {
            Debug.Assert(_buckets != null, "_buckets was null; callers should check first");

            int hashCode = InternalGetHashCode(item);
            for (int i = _buckets[hashCode % _buckets.Length] - 1; i >= 0; i = _slots[i].next)
            {
                if (_slots[i].value == item)
                {
                    return i;
                }
            }
            // wasn't found
            return -1;
        }

        /// <summary>
        /// Optimized symmetric except where the other collection is an object set itself.
        /// </summary>
        /// <param name="other">The collection to compare against.</param>
        private void SymmetricExceptWithUniqueObjectSet(ObjectSet<T> other)
        {
            foreach (T item in other)
            {
                if (!Remove(item))
                {
                    AddIfNotPresent(item);
                }
            }
        }

        /// <summary>
        /// Implementation notes:
        ///
        /// Used for symmetric except when other isn't an object set. This is more tedious because
        /// other may contain duplicates. The object set optimization could fail in these situations:
        ///
        /// 1. Other has a duplicate that's not in this: the object set technique would add and then remove it.
        /// 2. Other has a duplicate that's in this: the object set technique would remove then add it back.
        ///
        /// In general, its presence would be toggled each time it appears in other.
        ///
        /// This technique uses bit marking to indicate whether to add/remove the item. If already
        /// present in collection, it will get marked for deletion. If added from other, it will
        /// get marked as something not to remove.
        /// </summary>
        /// <param name="other">The collection to compare against.</param>
        private unsafe void SymmetricExceptWithEnumerable(IEnumerable<T> other)
        {
            int originalLastIndex = _lastIndex;
            int intArrayLength = BitHelper.ToIntArrayLength(originalLastIndex);

            BitHelper itemsToRemove;
            BitHelper itemsAddedFromOther;
            if (intArrayLength <= StackAllocThreshold / 2)
            {
                int* itemsToRemovePtr = stackalloc int[intArrayLength];
                itemsToRemove = new BitHelper(itemsToRemovePtr, intArrayLength);

                int* itemsAddedFromOtherPtr = stackalloc int[intArrayLength];
                itemsAddedFromOther = new BitHelper(itemsAddedFromOtherPtr, intArrayLength);
            }
            else
            {
                int[] itemsToRemoveArray = new int[intArrayLength];
                itemsToRemove = new BitHelper(itemsToRemoveArray, intArrayLength);

                int[] itemsAddedFromOtherArray = new int[intArrayLength];
                itemsAddedFromOther = new BitHelper(itemsAddedFromOtherArray, intArrayLength);
            }

            var addedNull = false;
            var removeNull = false;

            foreach (T item in other)
            {
                if (item == null)
                {
                    if (!_containsNull)
                    {
                        Add(item: null);
                        addedNull = true;
                    }
                    else
                    {
                        if (!addedNull)
                        {
                            removeNull = true;
                        }
                    }
                }
                else
                {
                    bool added = AddOrGetLocation(item, out int location);
                    if (added)
                    {
                        // wasn't already present in collection; flag it as something not to remove
                        // *NOTE* if location is out of range, we should ignore. BitHelper will
                        // detect that it's out of bounds and not try to mark it. But it's
                        // expected that location could be out of bounds because adding the item
                        // will increase _lastIndex as soon as all the free spots are filled.
                        itemsAddedFromOther.MarkBit(location);
                    }
                    else
                    {
                        // already there...if not added from other, mark for remove.
                        // *NOTE* Even though BitHelper will check that location is in range, we want
                        // to check here. There's no point in checking items beyond originalLastIndex
                        // because they could not have been in the original collection
                        if (location < originalLastIndex && !itemsAddedFromOther.IsMarked(location))
                        {
                            itemsToRemove.MarkBit(location);
                        }
                    }
                }
            }

            if (removeNull)
            {
                Remove(item: null);
            }

            // if anything marked, remove it
            for (int i = 0; i < originalLastIndex; i++)
            {
                if (itemsToRemove.IsMarked(i))
                {
                    Remove(_slots[i].value);
                }
            }
        }

        /// <summary>
        /// Add if not already in the set. Returns an out param indicating index where added. This
        /// is used by <see cref="SymmetricExceptWith(IEnumerable{T})"/> because it needs to know the
        /// following things:
        ///
        /// - whether the item was already present in the collection or added from other
        /// - where it's located (if already present, it will get marked for removal, otherwise marked for keeping)
        /// </summary>
        /// <param name="value">The value to add or get.</param>
        /// <param name="location">The index of the slot where the item was found or added.</param>
        /// <returns><c>true</c> if the item was added; otherwise, <c>false</c>.</returns>
        private bool AddOrGetLocation(T value, out int location)
        {
            Debug.Assert(_buckets != null, "_buckets is null, callers should have checked");

            int hashCode = InternalGetHashCode(value);
            int bucket = hashCode % _buckets.Length;
            for (int i = _buckets[bucket] - 1; i >= 0; i = _slots[i].next)
            {
                if (_slots[i].value == value)
                {
                    location = i;
                    return false; //already present
                }
            }

            int index;
            if (_freeList >= 0)
            {
                index = _freeList;
                _freeList = _slots[index].next;
            }
            else
            {
                if (_lastIndex == _slots.Length)
                {
                    IncreaseCapacity();
                    // this will change during resize
                    bucket = hashCode % _buckets.Length;
                }

                index = _lastIndex;
                _lastIndex++;
            }

            _slots[index].value = value;
            _slots[index].next = _buckets[bucket] - 1;
            _buckets[bucket] = index + 1;

            Count++;
            _version++;
            location = index;

            return true;
        }

        /// <summary>
        /// Determines counts that can be used to determine equality, subset, and superset. This
        /// is only used when other is an IEnumerable and not a ObjectSet. If other is a ObjectSet
        /// these properties can be checked faster without use of marking because we can assume
        /// other has no duplicates.
        ///
        /// The following count checks are performed by callers:
        /// 1. Equals: checks if unfoundCount = 0 and uniqueFoundCount = _count; i.e. everything
        /// in other is in this and everything in this is in other
        /// 2. Subset: checks if unfoundCount >= 0 and uniqueFoundCount = _count; i.e. other may
        /// have elements not in this and everything in this is in other
        /// 3. Proper subset: checks if unfoundCount > 0 and uniqueFoundCount = _count; i.e
        /// other must have at least one element not in this and everything in this is in other
        /// 4. Proper superset: checks if unfound count = 0 and uniqueFoundCount strictly less
        /// than _count; i.e. everything in other was in this and this had at least one element
        /// not contained in other.
        ///
        /// An earlier implementation used delegates to perform these checks rather than returning
        /// an ElementCount struct; however this was changed due to the perf overhead of delegates.
        /// </summary>
        /// <param name="other">The other collection to check against.</param>
        /// <param name="returnIfUnfound">Allows us to finish faster for equals and proper superset
        /// because unfoundCount must be 0.</param>
        /// <returns>The counts of the elements.</returns>
        private unsafe ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
        {
            ElementCount result;

            // need special case in case this has no elements.
            if (Count == 0)
            {
                int numElementsInOther = 0;
                foreach (T item in other)
                {
                    numElementsInOther++;
                    // break right away, all we want to know is whether other has 0 or 1 elements
                    break;
                }
                result.uniqueCount = 0;
                result.unfoundCount = numElementsInOther;
                return result;
            }

            Debug.Assert((_buckets != null) && (Count > 0), "_buckets was null but count greater than 0");

            int originalLastIndex = _lastIndex;
            int intArrayLength = BitHelper.ToIntArrayLength(originalLastIndex);

            BitHelper bitHelper;
            if (intArrayLength <= StackAllocThreshold)
            {
                int* bitArrayPtr = stackalloc int[intArrayLength];
                bitHelper = new BitHelper(bitArrayPtr, intArrayLength);
            }
            else
            {
                int[] bitArray = new int[intArrayLength];
                bitHelper = new BitHelper(bitArray, intArrayLength);
            }

            var foundNull = false;

            // count of items in other not found in this
            int unfoundCount = 0;
            // count of unique items in other found in this
            int uniqueFoundCount = 0;

            foreach (T item in other)
            {
                if (item == null)
                {
                    if (_containsNull)
                    {
                        if (!foundNull)
                        {
                            foundNull = true;
                            uniqueFoundCount++;
                        }
                    }
                    else
                    {
                        unfoundCount++;
                        if (returnIfUnfound)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    int index = InternalIndexOf(item);
                    if (index >= 0)
                    {
                        if (!bitHelper.IsMarked(index))
                        {
                            // item hasn't been seen yet
                            bitHelper.MarkBit(index);
                            uniqueFoundCount++;
                        }
                    }
                    else
                    {
                        unfoundCount++;
                        if (returnIfUnfound)
                        {
                            break;
                        }
                    }
                }
            }

            result.uniqueCount = uniqueFoundCount;
            result.unfoundCount = unfoundCount;
            return result;
        }

        /// <summary>
        /// Gets the hash code for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item to get a hash code for.</param>
        /// <returns>The hash code for the specified <paramref name="item"/>.</returns>
        private static int InternalGetHashCode(T item) => RuntimeHelpers.GetHashCode(item) & Lower31BitMask;

        #endregion

        /// <summary>
        /// Used for set checking operations (using enumerables) that rely on counting.
        /// </summary>
        internal struct ElementCount
        {
            internal int uniqueCount;
            internal int unfoundCount;
        }

        /// <summary>
        /// Representation of a slot in the set.
        /// </summary>
        internal struct Slot
        {
            /// <summary>
            /// The index of the next slot in the current bucket. <c>-1</c> if last.
            /// </summary>
            internal int next;

            /// <summary>
            /// The value stored in the slot.
            /// </summary>
            internal T value;
        }

        /// <summary>
        /// Enumerator used to enumerate over the set.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            /// <summary>
            /// The set to enumerate over.
            /// </summary>
            private readonly ObjectSet<T> _set;

            /// <summary>
            /// The version of the set obtained upon creation of the enumerator, used to detect concurrent
            /// updates to the set during enumeration.
            /// </summary>
            private readonly int _version;

            /// <summary>
            /// The current index of the enumeration. Starts off at <c>-1</c> which is used to evaluate
            /// whether the set contains <c>null</c>.
            /// </summary>
            private int _index;

            /// <summary>
            /// Creates a new enumerator instance for the specified <paramref name="set"/>.
            /// </summary>
            /// <param name="set">The set to create an enumerator for.</param>
            internal Enumerator(ObjectSet<T> set)
            {
                _set = set;
                _index = -1;
                _version = set._version;
                Current = default;
            }

            /// <summary>
            /// Disposes the enumerator.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumeration to the next element.
            /// </summary>
            /// <returns><c>true</c> if a next element was found; otherwise, <c>false</c>.</returns>
            public bool MoveNext()
            {
                if (_version != _set._version)
                {
                    throw new InvalidOperationException("The set has been updated during the enumeration.");
                }

                if (_index == -1)
                {
                    _index++;

                    if (_set._containsNull)
                    {
                        Current = default;
                        return true;
                    }
                }

                while (_index < _set._lastIndex)
                {
                    var value = _set._slots[_index].value;
                    if (value != null)
                    {
                        Current = value;
                        _index++;
                        return true;
                    }
                    _index++;
                }

                _index = _set._lastIndex + 1;
                Current = default;
                return false;
            }

            /// <summary>
            /// Gets the current value.
            /// </summary>
            public T Current { get; private set; }

            /// <summary>
            /// Gets the current value.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    if (_index == -1 || _index == _set._lastIndex + 1)
                    {
                        throw new InvalidOperationException("The current value can't be retrieved in the current state of the enumerator.");
                    }

                    return Current;
                }
            }

            /// <summary>
            /// Resets the enumerator.
            /// </summary>
            void IEnumerator.Reset()
            {
                if (_version != _set._version)
                {
                    throw new InvalidOperationException("The set has been updated during the enumeration.");
                }

                _index = -1;
                Current = default;
            }
        }
    }
}
