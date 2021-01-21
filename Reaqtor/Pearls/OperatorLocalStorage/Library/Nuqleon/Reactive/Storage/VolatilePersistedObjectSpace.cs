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
using System.Runtime.CompilerServices;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Object space implementation using in-memory data types.
    /// </summary>
    public sealed class VolatilePersistedObjectSpace : IPersistedObjectSpace
    {
        /// <summary>
        /// The registry holding the entries of the object space.
        /// </summary>
        private readonly Dictionary<string, IPersisted> _registry = new();

        /// <summary>
        /// Creates a persisted array.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier to use for the array.</param>
        /// <param name="length">The length of the array.</param>
        /// <returns>A new persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedArray<T> CreateArray<T>(string id, int length) => Create<IPersistedArray<T>>(id, () => new ArrayWrapper<T>(id, length < 0 ? throw new ArgumentOutOfRangeException(nameof(length)) : new T[length]));

        /// <summary>
        /// Creates a persisted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
        /// <param name="id">The identifier to use for the dictionary.</param>
        /// <returns>A new persisted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(string id) => Create<IPersistedDictionary<TKey, TValue>>(id, () => new DictionaryWrapper<TKey, TValue>(id, new Dictionary<TKey, TValue>()));

        /// <summary>
        /// Creates a persisted linked list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier to use for the linked list.</param>
        /// <returns>A new persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedLinkedList<T> CreateLinkedList<T>(string id) => Create<IPersistedLinkedList<T>>(id, () => new LinkedListWrapper<T>(id, new LinkedList<T>()));

        /// <summary>
        /// Creates a persisted list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier to use for the list.</param>
        /// <returns>A new persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedList<T> CreateList<T>(string id) => Create<IPersistedList<T>>(id, () => new ListWrapper<T>(id, new List<T>()));

        /// <summary>
        /// Creates a persisted queue.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier to use for the queue.</param>
        /// <returns>A new persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedQueue<T> CreateQueue<T>(string id) => Create<IPersistedQueue<T>>(id, () => new QueueWrapper<T>(id, new Queue<T>()));

        /// <summary>
        /// Creates a persisted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier to use for the set.</param>
        /// <returns>A new persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSet<T> CreateSet<T>(string id) => Create<IPersistedSet<T>>(id, () => new SetWrapper<T>(id, new HashSet<T>()));

        /// <summary>
        /// Creates a persisted sorted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier to use for the dictionary.</param>
        /// <returns>A new persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSortedDictionary<TKey, TValue> CreateSortedDictionary<TKey, TValue>(string id) => Create<IPersistedSortedDictionary<TKey, TValue>>(id, () => new SortedDictionaryWrapper<TKey, TValue>(id, new SortedDictionary<TKey, TValue>()));

        /// <summary>
        /// Creates a persisted sorted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier to use for the sorted set.</param>
        /// <returns>A new persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSortedSet<T> CreateSortedSet<T>(string id) => Create<IPersistedSortedSet<T>>(id, () => new SortedSetWrapper<T>(id, new SortedSet<T>()));

        /// <summary>
        /// Creates a persisted stack.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier to use for the stack.</param>
        /// <returns>A new persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedStack<T> CreateStack<T>(string id) => Create<IPersistedStack<T>>(id, () => new StackWrapper<T>(id, new Stack<T>()));

        /// <summary>
        /// Creates a persisted value.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the value.</typeparam>
        /// <param name="id">The identifier to use for the value.</param>
        /// <returns>A new persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedValue<T> CreateValue<T>(string id) => Create<IPersistedValue<T>>(id, () => new ValueWrapper<T>(id));

        /// <summary>
        /// Deletes the persisted object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        public void Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            lock (_registry)
            {
                if (!_registry.Remove(id))
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        /// <summary>
        /// Gets a persisted array with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier of the array to retrieve.</param>
        /// <returns>An existing persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted array type.</exception>
        public IPersistedArray<T> GetArray<T>(string id) => Get<IPersistedArray<T>>(id);

        /// <summary>
        /// Gets a persisted dictionary with the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
        /// <param name="id">The identifier of the dictionary to retrieve.</param>
        /// <returns>An existing persisted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted dictionary type.</exception>
        public IPersistedDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string id) => Get<IPersistedDictionary<TKey, TValue>>(id);

        /// <summary>
        /// Gets a persisted linked list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier of the linked list to retrieve.</param>
        /// <returns>An existing persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted linked list type.</exception>
        public IPersistedLinkedList<T> GetLinkedList<T>(string id) => Get<IPersistedLinkedList<T>>(id);

        /// <summary>
        /// Gets a persisted list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier of the list to retrieve.</param>
        /// <returns>An existing persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted list type.</exception>
        public IPersistedList<T> GetList<T>(string id) => Get<IPersistedList<T>>(id);

        /// <summary>
        /// Gets a persisted queue with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier of the queue to retrieve.</param>
        /// <returns>An existing persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted queue type.</exception>
        public IPersistedQueue<T> GetQueue<T>(string id) => Get<IPersistedQueue<T>>(id);

        /// <summary>
        /// Gets a persisted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier of the set to retrieve.</param>
        /// <returns>An existing persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted set type.</exception>
        public IPersistedSet<T> GetSet<T>(string id) => Get<IPersistedSet<T>>(id);

        /// <summary>
        /// Gets a persisted sorted dictionary with the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier of the sorted dictionary to retrieve.</param>
        /// <returns>An existing persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted dictionary type.</exception>
        public IPersistedSortedDictionary<TKey, TValue> GetSortedDictionary<TKey, TValue>(string id) => Get<IPersistedSortedDictionary<TKey, TValue>>(id);

        /// <summary>
        /// Gets a persisted sorted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier of the sorted set to retrieve.</param>
        /// <returns>An existing persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted set type.</exception>
        public IPersistedSortedSet<T> GetSortedSet<T>(string id) => Get<IPersistedSortedSet<T>>(id);

        /// <summary>
        /// Gets a persisted stack with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier of the stack to retrieve.</param>
        /// <returns>An existing persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted stack type.</exception>
        public IPersistedStack<T> GetStack<T>(string id) => Get<IPersistedStack<T>>(id);

        /// <summary>
        /// Gets a persisted value with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the value.</typeparam>
        /// <param name="id">The identifier of the value to retrieve.</param>
        /// <returns>An existing persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted value type.</exception>
        public IPersistedValue<T> GetValue<T>(string id) => Get<IPersistedValue<T>>(id);

        /// <summary>
        /// Gets an existing persisted object with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of the persisted object.</typeparam>
        /// <param name="id">The identifier of the persisted object to retrieve.</param>
        /// <returns>An existing persisted object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted value type.</exception>
        private T Get<T>(string id)
            where T : IPersisted
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            lock (_registry)
            {
                return (T)_registry[id];
            }
        }

        /// <summary>
        /// Creates an instance of a persisted object and adds it to the registry using the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of the persisted object.</typeparam>
        /// <param name="id">The identifier to use for the persisted object.</param>
        /// <param name="factory">The factory to use to create the persisted object instance.</param>
        /// <returns>A new persisted object instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        private T Create<T>(string id, Func<T> factory)
            where T : IPersisted
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var value = factory();

            lock (_registry)
            {
                if (_registry.ContainsKey(id))
                {
                    throw new InvalidOperationException(FormattableString.Invariant($"An entity with identifier '{id}' already exists."));
                }

                _registry.Add(id, value);
            }

            return value;
        }

        private sealed class ArrayWrapper<T> : PersistedBase, IPersistedArray<T>
        {
            private readonly T[] _array;

            public ArrayWrapper(string id, T[] array)
                : base(id)
            {
                _array = array;
            }

            public T this[int index]
            {
                get => _array[index];
                set => _array[index] = value;
            }

            T IReadOnlyList<T>.this[int index] => _array[index];

            public int Length => _array.Length;

            public int Count => _array.Length;

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in _array)
                {
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class DictionaryWrapper<TKey, TValue> : PersistedBase, IPersistedDictionary<TKey, TValue>
        {
            private readonly Dictionary<TKey, TValue> _dictionary;

            public DictionaryWrapper(string id, Dictionary<TKey, TValue> dictionary) : base(id)
            {
                _dictionary = dictionary;
            }

            public TValue this[TKey key]
            {
                get => _dictionary[key];
                set => _dictionary[key] = value;
            }

            public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)_dictionary).Keys;

            public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_dictionary).Values;

            public int Count => _dictionary.Count;

            public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dictionary).IsReadOnly;

            IReadOnlyCollection<TKey> IPersistedDictionary<TKey, TValue>.Keys => _dictionary.Keys;

            IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

            IReadOnlyCollection<TValue> IPersistedDictionary<TKey, TValue>.Values => _dictionary.Values;

            IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

            public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

            public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Add(item);

            public void Clear() => _dictionary.Clear();

            public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Contains(item);

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();

            public bool Remove(TKey key) => _dictionary.Remove(key);

            public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Remove(item);

            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class LinkedListWrapper<T> : PersistedBase, IPersistedLinkedList<T>
        {
            // REVIEW: In tests, assert that wrappers for adjacent nodes reflect changes correctly upon applying edits (insert before, insert after, remove).

            private readonly LinkedList<T> _list;
            private readonly ConditionalWeakTable<LinkedListNode<T>, LinkedListNodeWrapper> _nodeCache = new();

            public LinkedListWrapper(string id, LinkedList<T> list)
                : base(id)
            {
                _list = list;
            }

            public ILinkedListNode<T> First => CreateWrapper(_list.First);

            public ILinkedListNode<T> Last => CreateWrapper(_list.Last);

            public int Count => _list.Count;

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

            public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;

            public ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                var nodeWrapper = AsWrapper(node);

                var res = _list.AddAfter(nodeWrapper.Node, value);

                return CreateWrapper(res);
            }

            public void AddAfter(ILinkedListNode<T> node, ILinkedListNode<T> newNode)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));
                if (newNode == null)
                    throw new ArgumentNullException(nameof(newNode));

                var nodeWrapper = AsWrapper(node);
                var newNodeWrapper = AsWrapper(newNode);

                _list.AddAfter(nodeWrapper.Node, newNodeWrapper.Node);
            }

            public ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                var nodeWrapper = AsWrapper(node);

                var res = _list.AddBefore(nodeWrapper.Node, value);

                return CreateWrapper(res);
            }

            public void AddBefore(ILinkedListNode<T> node, ILinkedListNode<T> newNode)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));
                if (newNode == null)
                    throw new ArgumentNullException(nameof(newNode));

                var nodeWrapper = AsWrapper(node);
                var newNodeWrapper = AsWrapper(newNode);

                _list.AddBefore(nodeWrapper.Node, newNodeWrapper.Node);
            }

            public ILinkedListNode<T> AddFirst(T value) => CreateWrapper(_list.AddFirst(value));

            public void AddFirst(ILinkedListNode<T> node)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                var nodeWrapper = AsWrapper(node);

                _list.AddFirst(nodeWrapper.Node);
            }

            public ILinkedListNode<T> AddLast(T value) => CreateWrapper(_list.AddLast(value));

            public void AddLast(ILinkedListNode<T> node)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                var nodeWrapper = AsWrapper(node);

                _list.AddLast(nodeWrapper.Node);
            }

            public ILinkedListNode<T> Find(T value) => CreateWrapper(_list.Find(value));

            public ILinkedListNode<T> FindLast(T value) => CreateWrapper(_list.FindLast(value));

            public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

            public bool Remove(T item) => _list.Remove(item);

            public void Remove(ILinkedListNode<T> node)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                var nodeWrapper = AsWrapper(node);

                _list.Remove(nodeWrapper.Node);
            }

            public void RemoveFirst() => _list.RemoveFirst();

            public void RemoveLast() => _list.RemoveLast();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void ICollection<T>.Add(T item) => ((ICollection<T>)_list).Add(item);

            public void Clear() => _list.Clear();

            public bool Contains(T item) => _list.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

            private LinkedListNodeWrapper CreateWrapper(LinkedListNode<T> node) => node == null ? null : _nodeCache.GetValue(node, CreateWrapperCore);

            private LinkedListNodeWrapper CreateWrapperCore(LinkedListNode<T> node) => new(this, node);

            private static LinkedListNodeWrapper AsWrapper(ILinkedListNode<T> node) => (LinkedListNodeWrapper)node;

            private sealed class LinkedListNodeWrapper : ILinkedListNode<T>
            {
                private readonly LinkedListWrapper<T> _parent;

                public LinkedListNodeWrapper(LinkedListWrapper<T> parent, LinkedListNode<T> node)
                {
                    _parent = parent;
                    Node = node;
                }

                public LinkedListNode<T> Node { get; }

                public T Value
                {
                    get => Node.Value;
                    set => Node.Value = value;
                }

                public ILinkedList<T> List => _parent;

                public ILinkedListNode<T> Previous => _parent.CreateWrapper(Node.Previous);

                public ILinkedListNode<T> Next => _parent.CreateWrapper(Node.Next);

                T IReadOnlyLinkedListNode<T>.Value => Value;

                IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T>.List => List;

                IReadOnlyLinkedListNode<T> IReadOnlyLinkedListNode<T>.Previous => Previous;

                IReadOnlyLinkedListNode<T> IReadOnlyLinkedListNode<T>.Next => Next;
            }
        }

        private sealed class ListWrapper<T> : PersistedBase, IPersistedList<T>
        {
            private readonly List<T> _list;

            public ListWrapper(string id, List<T> list)
                : base(id)
            {
                _list = list;
            }

            public T this[int index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            T IReadOnlyList<T>.this[int index] => _list[index];

            public int Count => _list.Count;

            public bool IsReadOnly => false;

            public void Add(T item) => _list.Add(item);

            public void Clear() => _list.Clear();

            public bool Contains(T item) => _list.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

            public int IndexOf(T item) => _list.IndexOf(item);

            public void Insert(int index, T item) => _list.Insert(index, item);

            public bool Remove(T item) => _list.Remove(item);

            public void RemoveAt(int index) => _list.RemoveAt(index);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class QueueWrapper<T> : PersistedBase, IPersistedQueue<T>
        {
            private readonly Queue<T> _queue;

            public QueueWrapper(string id, Queue<T> queue)
                : base(id)
            {
                _queue = queue;
            }

            public int Count => _queue.Count;

            public T Dequeue() => _queue.Dequeue();

            public void Enqueue(T item) => _queue.Enqueue(item);

            public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

            public T Peek() => _queue.Peek();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class StackWrapper<T> : PersistedBase, IPersistedStack<T>
        {
            private readonly Stack<T> _stack;

            public StackWrapper(string id, Stack<T> stack)
                : base(id)
            {
                _stack = stack;
            }

            public int Count => _stack.Count;

            public IEnumerator<T> GetEnumerator() => _stack.GetEnumerator();

            public T Peek() => _stack.Peek();

            public T Pop() => _stack.Pop();

            public void Push(T value) => _stack.Push(value);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private abstract class SetWrapperBase<T, TSet> : PersistedBase, ISet<T>
            where TSet : ISet<T>
        {
            protected readonly TSet _set;

            public SetWrapperBase(string id, TSet set)
                : base(id)
            {
                _set = set;
            }

            public int Count => _set.Count;

            public bool IsReadOnly => _set.IsReadOnly;

            public bool Add(T item) => _set.Add(item);

            public void Clear() => _set.Clear();

            public bool Contains(T item) => _set.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

            public void ExceptWith(IEnumerable<T> other)
            {
                //
                // NB: Checking for `this` avoids issues with concurrent enumeration and modification.
                //

                if (other == this)
                {
                    Clear();
                }
                else
                {
                    _set.ExceptWith(other);
                }
            }

            public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

            public void IntersectWith(IEnumerable<T> other)
            {
                //
                // NB: Checking for `this` avoids issues with concurrent enumeration and modification.
                //

                if (other != this)
                {
                    _set.IntersectWith(other);
                }
            }

            public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

            public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

            public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

            public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

            public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

            public bool Remove(T item) => _set.Remove(item);

            public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

            public void SymmetricExceptWith(IEnumerable<T> other)
            {
                //
                // NB: Checking for `this` avoids issues with concurrent enumeration and modification.
                //

                if (other == this)
                {
                    Clear();
                }
                else
                {
                    _set.SymmetricExceptWith(other);
                }
            }

            public void UnionWith(IEnumerable<T> other)
            {
                //
                // NB: Checking for `this` avoids issues with concurrent enumeration and modification.
                //

                if (other != this)
                {
                    _set.UnionWith(other);
                }
            }

            void ICollection<T>.Add(T item) => Add(item);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class SetWrapper<T> : SetWrapperBase<T, HashSet<T>>, IPersistedSet<T>
        {
            public SetWrapper(string id, HashSet<T> set)
                : base(id, set)
            {
            }
        }

        private sealed class SortedDictionaryWrapper<TKey, TValue> : PersistedBase, IPersistedSortedDictionary<TKey, TValue>
        {
            private readonly SortedDictionary<TKey, TValue> _dictionary;

            public SortedDictionaryWrapper(string id, SortedDictionary<TKey, TValue> dictionary) : base(id)
            {
                _dictionary = dictionary;
            }

            public TValue this[TKey key]
            {
                get => _dictionary[key];
                set => _dictionary[key] = value;
            }

            public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)_dictionary).Keys;

            public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_dictionary).Values;

            public int Count => _dictionary.Count;

            public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dictionary).IsReadOnly;

            IReadOnlyCollection<TKey> IPersistedSortedDictionary<TKey, TValue>.Keys => _dictionary.Keys;

            IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

            IReadOnlyCollection<TValue> IPersistedSortedDictionary<TKey, TValue>.Values => _dictionary.Values;

            IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

            public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

            public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Add(item);

            public void Clear() => _dictionary.Clear();

            public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Contains(item);

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();

            public bool Remove(TKey key) => _dictionary.Remove(key);

            public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Remove(item);

            public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class SortedSetWrapper<T> : SetWrapperBase<T, SortedSet<T>>, IPersistedSortedSet<T>
        {
            public SortedSetWrapper(string id, SortedSet<T> set)
                : base(id, set)
            {
            }

            public T Max => _set.Max;

            public T Min => _set.Min;

            public ISortedSet<T> GetViewBetween(T lowerValue, T upperValue) => new SortedSetWrapper<T>(null, _set.GetViewBetween(lowerValue, upperValue));

            public IEnumerable<T> Reverse() => _set.Reverse();
        }

        private sealed class ValueWrapper<T> : PersistedBase, IPersistedValue<T>
        {
            public ValueWrapper(string id)
                : base(id)
            {
            }

            public T Value { get; set; }

            T IReadOnlyValue<T>.Value => Value;
        }
    }
}
