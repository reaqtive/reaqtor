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
using System.IO;

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted queue.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier to use for the queue.</param>
        /// <returns>A new persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedQueue<T> CreateQueue<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var queue = new Queue(this);
            _items.Add(id, queue);
            return CreateQueueCore<T>(id, queue);
        }

        /// <summary>
        /// Gets a persisted queue with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier of the queue to retrieve.</param>
        /// <returns>An existing persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted queue type.</exception>
        public IPersistedQueue<T> GetQueue<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateQueueCore<T>(id, (Queue)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="queue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
        /// <param name="id">The identifier of the queue.</param>
        /// <param name="queue">The storage entity representing the queue.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="queue"/>.</returns>
        private static IPersistedQueue<T> CreateQueueCore<T>(string id, Queue queue) => queue.Create<T>(id);

        /// <summary>
        /// Storage entity representing the queue.
        /// </summary>
        /// <remarks>
        /// Persistence of a queue looks as follows:
        /// <c>
        /// metadata/head = 3
        /// metadata/tail = 8
        /// items/3 = 42  (head of queue, cf. Dequeue/Peek)
        /// items/4 = 43
        /// items/5 = 44
        /// items/6 = 45
        /// items/7 = 46
        /// </c>
        /// The keys of the items are in the range [head..tail), where head and tail are mutable (cf. Enqueue and Dequeue methods).
        /// <list type="bullet">
        ///   <item><c>queue.Enqueue(v)</c> results in <c>Edit("metadata/tail", tail + 1); Add($"items/{tail}", v)</c></item>
        ///   <item><c>queue.Dequeue()</c> results in <c>Edit("metadata/head", head - 1); Delete($"items/{head}")</c></item>
        /// </list>
        /// </remarks>
        private sealed class Queue : PersistableBase
        {
            /// <summary>
            /// The category to store the metadata (i.e. the head, using the <see cref="HeadKey"/> key, and the tail, using the <see cref="TailKey"/>) in.
            /// </summary>
            private const string MetadataCategory = "metadata";

            /// <summary>
            /// The category to store the data (i.e. the queue elements) in.
            /// </summary>
            private const string ItemsCategory = "items";

            /// <summary>
            /// The key to store the head index in (using the <see cref="MetadataCategory"/> category).
            /// </summary>
            private const string HeadKey = "head";

            /// <summary>
            /// The key to store the tail index in (using the <see cref="MetadataCategory"/> category).
            /// </summary>
            private const string TailKey = "tail";

            /// <summary>
            /// The eventual objects containing the queue elements, set by <see cref="LoadCore(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>. Indexes in this array range over the elements in [head..tail).
            /// </summary>
            private EventualObject[] _data;

            //
            // NB: Head and tail are represented as 64-bit signed integers, so we have a range of [0..2^63) to cause roll-over,
            //     which is far sufficient for the lifetime of any queue. 2^63 ~ 10^19, so at 10^N enqueues per second, we have
            //     10^(19-N) seconds of lifetime before tail overflows. A day is about 10^5 seconds, so we end up with 10^(14-N)
            //     days. Even if N were 10 (i.e. 10^10 enqueues per second, or 10GHz), we'd have 10^4 days or 27 years. Common
            //     values for N are expected to not exceed 3 (i.e. 1000 qps).
            //

            /// <summary>
            /// The head index of the queue as currently saved to the key/value store (if <see cref="PersistableBase.HasSaved"/> is <c>true</c>). This value gets edited upon successful <see cref="SaveCore(IStateWriter)"/> operations (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            /// <remarks>
            /// This index is inclusive, i.e. an element is persisted at this index unless it's equal to <see cref="_tail"/> when the queue is empty.
            /// </remarks>
            private long _head;

            /// <summary>
            /// The tail index of the queue as currently saved to the key/value store (if <see cref="PersistableBase.HasSaved"/> is <c>true</c>). This value gets edited upon successful <see cref="SaveCore(IStateWriter)"/> operations (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            /// <remarks>
            /// This index is exclusive, i.e. it represents the index that will be written to next by an enqueue operation.
            /// </remarks>
            private long _tail;

            /// <summary>
            /// The state change manager used to keep track of the enqueue and dequeue operations that have taken place since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private readonly StateChangedManager<DirtyState> _dirty = new();

            /// <summary>
            /// The pending edits obtained from <see cref="_dirty"/> on the last call to <see cref="SaveCore(IStateWriter)"/>, for use by <see cref="OnSavedCore"/> to edit <see cref="_head"/> and <see cref="_tail"/> upon a successful save operation.
            /// </summary>
            private DirtyState[] _dirtyHistory;

            /// <summary>
            /// Creates a new entity representing a queue.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Queue(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.Queue"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.Queue;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
            /// <param name="id">The identifier of the queue.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedQueue<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    var value = new Wrapper<T>(id, this, Restore<T>());
                    _wrapper = value;
                    return value;
                }
                else
                {
                    return (IPersistedQueue<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the queue from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Remove the metadata associated with the queue.
                //

                writer.DeleteItem(MetadataCategory, HeadKey);
                writer.DeleteItem(MetadataCategory, TailKey);

                //
                // Remove the elements stored in items/i in the range [head..tail).
                //
                // NB: Pending in-memory edits are not reflected in _head and _tail.
                //

                for (var i = _head; i < _tail; i++)
                {
                    writer.DeleteItem(ItemsCategory, GetKeyForIndex(i));
                }
            }

            /// <summary>
            /// Loads the queue from storage.
            /// </summary>
            /// <param name="reader">The reader to load the queue from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Deserialize metadata/head and metadata/tail as integers.
                //

                using (var headStream = reader.GetItemReader(MetadataCategory, HeadKey))
                {
                    _head = GetDeserializer<long>().Deserialize(headStream);
                }

                using (var tailStream = reader.GetItemReader(MetadataCategory, TailKey))
                {
                    _tail = GetDeserializer<long>().Deserialize(tailStream);
                }

                //
                // Prepare an array of eventual objects and fill with the elements obtained from the reader at items/i with indexes in range [head..tail).
                //
                // NB: Tail is an exclusive index.
                //

                var length = _tail - _head;

                _data = new EventualObject[length];

                for (var i = 0; i < length; i++)
                {
                    var key = GetKeyForIndex(_head + i);

                    using var itemStream = reader.GetItemReader(ItemsCategory, key);

                    _data[i] = EventualObject.FromState(itemStream);
                }
            }

            /// <summary>
            /// Saves the queue to storage.
            /// </summary>
            /// <param name="writer">The writer to write the queue to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Enqueue, Dequeue).");

                //
                // The head and tail index values to persist. These values can be null in case of a differential checkpoint and no change is needed (i.e. Enqueue edits tail, Dequeue edits head).
                //

                var head = default(long?);
                var tail = default(long?);

                //
                // The number of elements to delete from the currently persisted head index, and the number of elements that were added beyond the currently persisted tail index.
                //
                // In particular, delete operations take place in range [_head, _head + headDeleteCount) and add operations take place in range [_tail, _tail + tailAddCount).
                //

                var headDeleteCount = 0L;
                var tailAddCount = 0L;

                //
                // Snapshot the dirty state (containing edits and length changes). See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                _dirtyHistory = _dirty.SaveState();

                //
                // Get the head and tail index values by incorporating all in-memory edits (if any).
                //

                var (headValue, tailValue) = GetHeadAndTail(_dirtyHistory);

                //
                // Check if we need to save all the state in case of a full checkpoint or a new queue that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    //
                    // Store the head and tail index, and cause the persistence code to perform a write for all elements using tailAddCount.
                    //

                    head = headValue;
                    tail = tailValue;

                    headDeleteCount = 0;
                    tailAddCount = tailValue - headValue; // NB: Tail is an exclusive index.
                }
                else
                {
                    //
                    // Add up all the enqueue and dequeue counts from the edit pages to determine how many items to delete from the head and how many items to add from the tail.
                    // Note that traversal order of the edit pages doesn't matter because + is commutative.
                    //

                    foreach (var dirty in _dirtyHistory)
                    {
                        headDeleteCount += dirty.DequeueCount;
                        tailAddCount += dirty.EnqueueCount;
                    }

                    //
                    // Adjust the head or tail index to a non-null value if necessary, causing persistence.
                    //

                    if (headDeleteCount > 0)
                    {
                        head = headValue;
                    }

                    if (tailAddCount > 0)
                    {
                        tail = tailValue;
                    }
                }

                //
                // Persist the head index and/or the tail index if necessary.
                //

                if (head != null)
                {
                    using var headStream = writer.GetItemWriter(MetadataCategory, HeadKey);

                    GetSerializer<long>().Serialize(head.Value, headStream);
                }

                if (tail != null)
                {
                    using var tailStream = writer.GetItemWriter(MetadataCategory, TailKey);

                    GetSerializer<long>().Serialize(tail.Value, tailStream);
                }

                //
                // First perform deletes between the old head index and the new head index. This range matches [_head, _head + headDeleteCount) == [head - headDeleteCount, head).
                //

                if (headDeleteCount > 0)
                {
                    for (var i = headValue - headDeleteCount; i < headValue; i++)
                    {
                        var key = GetKeyForIndex(i);

                        writer.DeleteItem(ItemsCategory, key);
                    }
                }

                //
                // Next, perform additions between the old tail index and the next tail index. This range matches [_tail, _tail + tailAddCount) == [tail - tailAddCount, tail).
                //

                if (tailAddCount > 0)
                {
                    //
                    // Save each edited element by dispatching to IQueuePersistence.Save which has access to the queue element static type used for serialization.
                    //
                    // NB: We perform a single dispatch to persist all of the newly added elements at the tail of the queue to reduce seek times in the underlying Queue<T> (see Wrapper<T>).
                    //

                    var target = (IQueuePersistence)_wrapper;

                    target.Save(SerializationFactory, tailAddCount, GetTailStreams());

                    // <summary>
                    // Local function to get a sequence of streams to persist the enqueued elements to. The sequence will yield streams for the elements at indexes [tail - tailAddCount, tail),
                    // resulting in in-order access of the key/value store.
                    // </summary>
                    IEnumerable<Stream> GetTailStreams()
                    {
                        var offset = tailValue - tailAddCount;

                        for (var i = 0; i < tailAddCount; i++)
                        {
                            var index = offset + i;
                            var key = GetKeyForIndex(index);
                            var itemStream = writer.GetItemWriter(ItemsCategory, key);

                            yield return itemStream;
                        }
                    }
                }
            }

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
                // Reflect the head and tail indexes that were persisted in the _head and _tail fields.
                //

                (_head, _tail) = GetHeadAndTail(_dirtyHistory);

                //
                // We no longer need the edit pages.
                //

                _dirtyHistory = null;
            }

            /// <summary>
            /// Gets the current head and tail indexes of the queue, incorporating any pending in-memory edits on the specified <paramref name="dirty"/> edit pages.
            /// </summary>
            /// <param name="dirty">The edit pages to consider for the head and tail index determination.</param>
            /// <returns>A tuple containing the current head and tail indexes of the queue, including the edits on the specified <paramref name="dirty"/> edit pages.</returns>
            private (long head, long tail) GetHeadAndTail(DirtyState[] dirty)
            {
                //
                // Start from the currently persisted values for the head and tail indexes.
                //

                var head = _head;
                var tail = _tail;

                //
                // Advance the head index by adding all the dequeue operations, and advance the tail index by adding all the enqueue operations.
                //
                // Note that the order of additions doesn't matter.
                //

                foreach (var edit in dirty)
                {
                    head += edit.DequeueCount;
                    tail += edit.EnqueueCount;
                }

                return (head, tail);
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}.Enqueue(T)"/> when an enqueue operation takes place.
            /// </summary>
            private void Enqueue()
            {
                //
                // Mark the state as dirty and increase the enqueue count in the latest edit page.
                //

                StateChanged = true;

                _dirty.State.EnqueueCount++;
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}.Dequeue"/> when a dequeue operation takes place.
            /// </summary>
            private void Dequeue()
            {
                //
                // Mark the state as dirty and increase the dequeue count in the latest edit page.
                //

                StateChanged = true;

                _dirty.State.DequeueCount++;
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory queue representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a new empty queue instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
            /// <returns>An instance of type Queue{<typeparamref name="T"/>} containing the data represented by the storage entity.</returns>
            private Queue<T> Restore<T>()
            {
                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize queue elements from. Otherwise, return a fresh queue instance.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<T>();

                    //
                    // Deserialize all the queue elements.
                    //

                    var res = new Queue<T>(_data.Length);

                    //
                    // NB: The eventual objects loaded by LoadCore occur in the enqueue order.
                    //

                    for (var i = 0; i < _data.Length; i++)
                    {
                        res.Enqueue(_data[i].Deserialize(deserializer));
                    }

                    _data = null;

                    return res;
                }
                else
                {
                    return new Queue<T>();
                }
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="Queue"/> storage entity to the statically typed <see cref="Wrapper{T}"/> instance.
            /// </summary>
            private interface IQueuePersistence
            {
                /// <summary>
                /// Saves the specified <paramref name="tailCount"/> number of queue elements from the tail of the queue (in default queue enumeration order, i.e. moving towards the tail) to the streams yielded by the <paramref name="streams"/> sequence.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="tailCount">The number of queue elements to save starting from the tail of the queue.</param>
                /// <param name="streams">The streams to save the queue elements to. This sequence has a length equal to <paramref name="tailCount"/>.</param>
                /// <example>
                /// Consider a queue obtained by the following operation sequence:
                /// <c>
                /// Enqueue(42); Enqueue(43); Enqueue(44);
                /// </c>
                /// If persistence of a tail length 2 is requested, the following save operations should take place:
                /// <c>
                /// streams[0].Save(43); streams[1].Save(44);
                /// </c>
                /// </example>
                void Save(ISerializerFactory serializerFactory, long tailCount, IEnumerable<Stream> streams);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted queue with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedQueue<T>, IQueuePersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly Queue _storage;

                /// <summary>
                /// The stored queue, always reflecting the latest in-memory state.
                /// </summary>
                private readonly Queue<T> _queue;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the queue.</param>
                /// <param name="storage">The storage entity representing the queue.</param>
                /// <param name="queue">The initial queue. This could either be the result of deserializing persisted state, or an empty queue for a new entity.</param>
                public Wrapper(string id, Queue storage, Queue<T> queue)
                    : base(id)
                {
                    _storage = storage;
                    _queue = queue;
                }

                /// <summary>
                /// Gets the number of elements in the queue.
                /// </summary>
                public int Count => _queue.Count;

                /// <summary>
                /// Removes and returns the object at the beginning of the queue.
                /// </summary>
                /// <returns>The object that is removed from the beginning of the queue.</returns>
                /// <exception cref="InvalidOperationException">The queue is empty.</exception>
                public T Dequeue()
                {
                    //
                    // First dequeue the next element from the queue, allowing an exception to occur prior to having touched the storage entity.
                    //

                    var res = _queue.Dequeue();

                    //
                    // Track the dequeue operation in the storage entity.
                    //

                    _storage.Dequeue();

                    return res;
                }

                /// <summary>
                /// Adds an object to the end of the queue.
                /// </summary>
                /// <param name="value">The object to add to the queue.</param>
                public void Enqueue(T value)
                {
                    //
                    // First enqueue the element in the queue. For consistency, we do this prior to updating the storage entity.

                    //
                    _queue.Enqueue(value);

                    //
                    // Track the enqueue operation in the storage entity.
                    //

                    _storage.Enqueue();
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the queue, in dequeueing order.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the queue.</returns>
                public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

                /// <summary>
                /// Returns the object at the beginning of the queue without removing it.
                /// </summary>
                /// <returns>The object at the beginning of the queue.</returns>
                /// <exception cref="InvalidOperationException">The queue is empty.</exception>
                public T Peek() => _queue.Peek();

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the queue, in dequeueing order.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the queue.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the specified <paramref name="tailCount"/> number of queue elements from the tail of the queue (in default queue enumeration order, i.e. moving towards the tail) to the streams yielded by the <paramref name="streams"/> sequence.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="tailCount">The number of queue elements to save starting from the tail of the queue.</param>
                /// <param name="streams">The streams to save the queue elements to. This sequence has a length equal to <paramref name="tailCount"/>.</param>
                void IQueuePersistence.Save(ISerializerFactory serializerFactory, long tailCount, IEnumerable<Stream> streams)
                {
                    //
                    // The obtain the specified number of elements from the tail in dequeue order, we have to enumerate over the whole queue and skip from the front.
                    //
                    // PERF: Is this O(queueCount) traversal in lieu of an O(tailCount) lookup cost tolerable? If not, we should re-implement Queue<T> to enable fast random access.
                    //

                    var skip = _queue.Count - tailCount;

                    Invariant.Assert(skip >= 0, "Requested tail count greater than available queue size.");

                    using var queueEnumerator = _queue.GetEnumerator();

                    //
                    // Skip the elements we don't care about.
                    //

                    for (var i = 0; i < skip; i++)
                    {
                        Invariant.Assert(queueEnumerator.MoveNext(), "Failed to skip.");
                    }

                    //
                    // Get the serializer once to reduce overheads.
                    //

                    var serializer = serializerFactory.GetSerializer<T>();

                    //
                    // Save the remaining tailCount elements to the streams provided.
                    //

                    using var streamEnumerator = streams.GetEnumerator();

                    for (var i = 0L; i < tailCount; i++)
                    {
                        Invariant.Assert(queueEnumerator.MoveNext(), "No more elements in queue.");
                        Invariant.Assert(streamEnumerator.MoveNext(), "No more streams to write to.");

                        var element = queueEnumerator.Current;

                        using var stream = streamEnumerator.Current;

                        serializer.Serialize(element, stream);
                    }

                    Invariant.Assert(!queueEnumerator.MoveNext(), "Should have reached last element of the queue.");
                    Invariant.Assert(!streamEnumerator.MoveNext(), "Should have consumed all streams.");
                }
            }

            /// <summary>
            /// State kept on dirty pages to keep track of all edits that have to be persisted in the next checkpoint.
            /// </summary>
            private sealed class DirtyState
            {
                /// <summary>
                /// The total number of enqueue operations.
                /// </summary>
                public int EnqueueCount;

                /// <summary>
                /// The total number of dequeue operations.
                /// </summary>
                public int DequeueCount;
            }
        }
    }
}
