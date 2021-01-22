// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Heap-based implementation of a priority queue.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the priority queue.</typeparam>
    /// <remarks>This type is not thread safe.</remarks>
    public sealed class HeapBasedPriorityQueue<T>
    {
        /// <summary>
        /// Count for introducing priority of equal elements.
        /// </summary>
        private long _count;

        /// <summary>
        /// Array of indexed items.
        /// </summary>
        private IndexedItem[] _items;

        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapBasedPriorityQueue{T}"/> class.
        /// </summary>
        public HeapBasedPriorityQueue(IComparer<T> comparer)
            : this(16, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapBasedPriorityQueue{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">Compared used to compare elements in the queue.</param>
        public HeapBasedPriorityQueue(int capacity, IComparer<T> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _items = new IndexedItem[capacity];
            Count = 0;
            _count = 0;

            CheckHeapInvariant();
        }

        /// <summary>
        /// Gets the number of elements in the queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Peeks the element with the highest priority.
        /// </summary>
        /// <returns>The element with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Peek is not allowed on an empty queue."));
            }

            CheckHeapInvariant();
            return _items[0].Value;
        }

        /// <summary>
        /// Dequeues the element with the highest priority.
        /// </summary>
        /// <returns>The element with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);

            CheckHeapInvariant();
            return result;
        }

        /// <summary>
        /// Enqueues the specified <paramref name="item"/> in the queue.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
            if (Count >= _items.Length)
            {
                // exponential allocation.
                var temp = _items;
                _items = new IndexedItem[_items.Length * 2];
                Array.Copy(temp, _items, temp.Length);
            }

            var index = Count++;
            _items[index] = new IndexedItem { Value = item, Id = _count++ };
            Percolate(index);

            CheckHeapInvariant();
        }

        /// <summary>
        /// Removes the specified <paramref name="item"/> from the queue.
        /// </summary>
        /// <param name="item">The item to remove from the queue.</param>
        /// <returns><c>true</c> if the item has been deleted; <c>false</c> if the item was not found.</returns>
        public bool Remove(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < Count; ++i)
            {
                if (comparer.Equals(_items[i].Value, item)) // REVIEW: Should we use _comparer and check for 0?
                {
                    RemoveAt(i);
                    CheckHeapInvariant();
                    return true;
                }
            }

            CheckHeapInvariant();
            return false;
        }

        /// <summary>
        /// Determines whether the queue contains the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>
        /// <c>true</c> if the queue contains the specified <paramref name="item"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < Count; ++i)
            {
                if (comparer.Equals(_items[i].Value, item)) // REVIEW: Should we use _comparer and check for 0?
                {
                    CheckHeapInvariant();
                    return true;
                }
            }

            CheckHeapInvariant();
            return false;
        }

        /// <summary>
        /// Determines whether the element at the specified <paramref name="left"/> index has
        /// a higher priority that the element at the specified <paramref name="right"/> index.
        /// </summary>
        /// <param name="left">The index of the left element.</param>
        /// <param name="right">The index of the right element.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left"/> has a higher priority than <paramref name="right"/>;
        /// otherwise, otherwise, <c>false</c>.
        /// </returns>
        private bool IsHigherPriority(int left, int right)
        {
            Debug.Assert(left >= 0 & left < _items.Length, "Index should be in range.");
            Debug.Assert(right >= 0 & right < _items.Length, "Index should be in range.");

            var l = _items[left];
            var r = _items[right];

            var c = _comparer.Compare(l.Value, r.Value);
            if (c == 0)
            {
                c = l.Id.CompareTo(r.Id);
            }

            return c < 0;
        }

        /// <summary>
        /// Percolates the element at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to percolate.</param>
        /// <returns>The final index of the original element.</returns>
        private int Percolate(int index)
        {
            Debug.Assert(index < Count & index >= 0, "Index is out of range: " + index);

            if (index == 0)
            {
                return 0;
            }

            var parent = (index - 1) / 2;

            if (IsHigherPriority(index, parent))
            {
                var temp = _items[index];
                _items[index] = _items[parent];
                _items[parent] = temp;
                return Percolate(parent);
            }

            return index;
        }

        /// <summary>
        /// Heapifies the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        private void Heapify(int index)
        {
            Debug.Assert(index < Count & index >= 0, "Index is out of range: " + index);

            var left = (2 * index) + 1;
            var right = (2 * index) + 2;
            var first = index;

            if (left < Count && IsHigherPriority(left, first))
            {
                first = left;
            }

            if (right < Count && IsHigherPriority(right, first))
            {
                first = right;
            }

            if (first != index)
            {
                var temp = _items[index];
                _items[index] = _items[first];
                _items[first] = temp;
                Heapify(first);
            }
        }

        /// <summary>
        /// Removes the element at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        private void RemoveAt(int index)
        {
            Debug.Assert(index >= 0 & index < _items.Length, "Index should be in range.");

            _items[index] = _items[--Count];
            _items[Count] = default;

            if (index != Count)
            {
                Heapify(Percolate(index));
            }

            if (Count < _items.Length / 4)
            {
                var temp = _items;
                _items = new IndexedItem[_items.Length / 2];
                Array.Copy(temp, 0, _items, 0, Count);
            }

            CheckHeapInvariant();
        }

        [Conditional("HEAP_ASSERT")]
        [ExcludeFromCodeCoverage]
        private void CheckHeapInvariant() // Do not remove or rename this method; tests use private reflection to call it.
        {
            for (int i = 0; i < Count; i++)
            {
                if (2 * i + 1 < Count && !IsHigherPriority(i, 2 * i + 1)
                    || 2 * i + 2 < Count && !IsHigherPriority(i, 2 * i + 2))
                {
                    throw new InvalidOperationException("Heap invariant violated");
                }
            }
        }

        /// <summary>
        /// Indexed item.
        /// </summary>
        private struct IndexedItem
        {
            /// <summary>
            /// Gets or sets the real user value.
            /// </summary>
            public T Value;

            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public long Id;
        }
    }
}
