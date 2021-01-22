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
        /// Creates a persisted stack.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier to use for the stack.</param>
        /// <returns>A new persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedStack<T> CreateStack<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var stack = new Stack(this);
            _items.Add(id, stack);
            return CreateStackCore<T>(id, stack);
        }

        /// <summary>
        /// Gets a persisted stack with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier of the stack to retrieve.</param>
        /// <returns>An existing persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted stack type.</exception>
        public IPersistedStack<T> GetStack<T>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateStackCore<T>(id, (Stack)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified <paramref name="stack"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements stored in the stack.</typeparam>
        /// <param name="id">The identifier of the stack.</param>
        /// <param name="stack">The storage entity representing the stack.</param>
        /// <returns>A statically typed wrapper around the specified <paramref name="stack"/>.</returns>
        private static IPersistedStack<T> CreateStackCore<T>(string id, Stack stack) => stack.Create<T>(id);

        /// <summary>
        /// Storage entity representing the stack.
        /// </summary>
        /// <remarks>
        /// Persistence of a queue looks as follows:
        /// <c>
        /// metadata/count = 5
        /// items/0 = 42
        /// items/1 = 43
        /// items/2 = 44
        /// items/3 = 45
        /// items/4 = 46  (top of stack, cf. Pop/Peek)
        /// </c>
        /// The keys of the items are in the range (count..0], where count is mutable (cf. Push and Pop methods).
        /// <list type="bullet">
        ///   <item><c>queue.Push(v)</c> results in <c>Edit("metadata/count", tail + 1); Add($"items/{count}", v)</c></item>
        ///   <item><c>queue.Pop()</c> results in <c>Edit("metadata/count", count - 1); Delete($"items/{count - 1}")</c></item>
        /// </list>
        /// </remarks>
        private sealed class Stack : PersistableBase
        {
            /// <summary>
            /// The category to store the metadata (i.e. the element count, using the <see cref="CountKey"/> key) in.
            /// </summary>
            private const string MetadataCategory = "metadata";

            /// <summary>
            /// The category to store the data (i.e. the stack elements) in.
            /// </summary>
            private const string ItemsCategory = "items";

            /// <summary>
            /// The key to store the element count in (using the <see cref="MetadataCategory"/> category).
            /// </summary>
            private const string CountKey = "count";

            /// <summary>
            /// The eventual objects containing the stack elements, set by <see cref="LoadCore(IStateReader)"/> and deserialized by <see cref="Restore{T}"/>. Indexes in this array range over the elements in [0..count).
            /// </summary>
            private EventualObject[] _data;

            /// <summary>
            /// The element count of the stack as currently saved to the key/value store (if <see cref="PersistableBase.HasSaved"/> is <c>true</c>). This value gets edited upon successful <see cref="SaveCore(IStateWriter)"/> operations (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private int _count;

            /// <summary>
            /// The state change manager used to keep track of the push and pop operations that have taken place since the last successful <see cref="SaveCore(IStateWriter)"/> operation (as indicated by a call to <see cref="OnSavedCore"/>).
            /// </summary>
            private readonly StateChangedManager<DirtyState> _dirty = new();

            /// <summary>
            /// The pending edits obtained from <see cref="_dirty"/> on the last call to <see cref="SaveCore(IStateWriter)"/>, for use by <see cref="OnSavedCore"/> to edit <see cref="_count"/> upon a successful save operation.
            /// </summary>
            private DirtyState[] _dirtyHistory;

            /// <summary>
            /// Creates a new entity representing a stack.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Stack(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.Stack"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.Stack;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same type <typeparamref name="T"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the stack.</typeparam>
            /// <param name="id">The identifier of the stack.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="T"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedStack<T> Create<T>(string id)
            {
                if (_wrapper == null)
                {
                    var value = new Wrapper<T>(id, this, Restore<T>());
                    _wrapper = value;
                    return value;
                }
                else
                {
                    return (IPersistedStack<T>)_wrapper;
                }
            }

            /// <summary>
            /// Deletes the stack from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operations to.</param>
            protected override void DeleteCore(IStateWriter writer)
            {
                //
                // Remove the metadata associated with the stack.
                //

                writer.DeleteItem(MetadataCategory, CountKey);

                //
                // Remove the elements stored in items/i in the range [0..count).
                //
                // NB: Pending in-memory edits are not reflected in _count.
                //

                for (var i = 0; i < _count; i++)
                {
                    var key = GetKeyForIndex(i);

                    writer.DeleteItem(ItemsCategory, key);
                }
            }

            /// <summary>
            /// Loads the stack from storage.
            /// </summary>
            /// <param name="reader">The reader to load the stack from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Deserialize metadata/count as an integer.
                //

                using (var countStream = reader.GetItemReader(MetadataCategory, CountKey))
                {
                    _count = GetDeserializer<int>().Deserialize(countStream);
                }

                //
                // Prepare an array of eventual objects and fill with the elements obtained from the reader at items/i with indexes in range [0..count).
                //

                _data = new EventualObject[_count];

                for (var i = 0; i < _count; i++)
                {
                    var key = GetKeyForIndex(i);

                    using var itemStream = reader.GetItemReader(ItemsCategory, key);

                    _data[i] = EventualObject.FromState(itemStream);
                }
            }

            /// <summary>
            /// Saves the stack to storage.
            /// </summary>
            /// <param name="writer">The writer to write the stack to.</param>
            protected override void SaveCore(IStateWriter writer)
            {
                Debug.Assert(_wrapper != null, "StateChanged can only be set if a statically typed wrapper exists (cf. Push, Pop).");

                //
                // Stack element count value to be persisted in case of a full checkpoint, or when the stack has not yet been persisted, or if the element count changed since the last checkpoint. Otherwise, this value will be null.
                //

                var count = default(int?);

                //
                // The number of elements that have been edited relative to the new top of the stack, i.e. in range [count - tailEditCount, count). This number will include edits to existing elements (due to Pop/Push) and additions beyond the old top of the stack (due to Push).
                //

                var tailEditCount = 0;

                //
                // The number of elements that have been deleted relative to the old top of the stack, i.e. in range [_count - tailDeleteCount, _count). This number will only be strictly positive if there have been more Pop operations than Push operations.
                //

                var tailDeleteCount = 0;

                //
                // Snapshot the dirty state (containing stack balance info). See OnSavedCore for pruning of the snapshot after successful persistence.
                //
                // NB: Always create a snapshot (including for full checkpoints), so we have a clean slate for the subsequent checkpoint (full or differential).
                //

                _dirtyHistory = _dirty.SaveState();

                //
                // Get the stack element count by incorporating all in-memory edits (if any).
                //

                var (countValue, minCount) = GetCount(_dirtyHistory);

                //
                // Check if we need to save all the state in case of a full checkpoint or a new list that has not yet been persisted.
                //

                if (writer.CheckpointKind == CheckpointKind.Full || !HasSaved)
                {
                    //
                    // Store the element count, and cause the persistence code to perform a write for all elements using tailEditCount.
                    //

                    count = countValue;

                    tailEditCount = countValue;
                    tailDeleteCount = 0;
                }
                else
                {
                    //
                    // If the new element count and the original element count have changed, make sure to persist the new value.
                    //

                    if (countValue != _count)
                    {
                        count = countValue;

                        //
                        // If the new element count is less than the original element count, the difference denotes the number of elements to delete in range [_count - tailDeleteCount, _count).
                        //

                        if (countValue < _count)
                        {
                            tailDeleteCount = _count - countValue;
                        }
                    }

                    //
                    // If the minimum element count is less than the new element count, the difference denotes the number of elements to edit in range [count - tailEditCount, count).
                    //

                    if (minCount < countValue)
                    {
                        tailEditCount = countValue - minCount;
                    }

                    //
                    // Assert some invariants that are specific to differential checkpoints (e.g. due to deletes).
                    //

                    Debug.Assert(countValue >= _count || tailDeleteCount > 0, "If the new count is less than the old count, we should have deletions.", $"Debug.Assert({countValue} < {_count} ? {tailDeleteCount} > 0)");
                }

                //
                // Assert invariants that hold regardless of checkpoint type.
                //

                Debug.Assert(minCount <= _count, "Minimum count should be less than or equal to original count.", $"Debug.Assert({minCount} <= {_count})");
                Debug.Assert(minCount <= countValue, "Minimum count should be less than or equal to new count.", $"Debug.Assert({minCount} <= {countValue})");
                Debug.Assert(countValue <= _count || tailEditCount > 0, "If the new count is greater than the old count, we should have additions.", $"Debug.Assert({countValue} > {_count} ? {tailEditCount} > 0)");

                //
                // If element count has changed, persist it now.
                //

                if (count != null)
                {
                    using var countStream = writer.GetItemWriter(MetadataCategory, CountKey);

                    GetSerializer<int>().Serialize(count.Value, countStream);
                }

                //
                // First perform deletes between the new count and the old count in range (count + tailDeleteCount, count].
                //
                // NB: We perform the deletes in reverse order to align with the edit order used below.
                //
                // REVIEW: Should we go through the effort of performing in-order edits followed by deletes in order to optimize storage access patterns? Note that a layer behind the state writer could do this as well.
                //

                if (tailDeleteCount > 0)
                {
                    for (var i = tailDeleteCount - 1; i >= 0; i--)
                    {
                        var key = GetKeyForIndex(countValue + i);

                        writer.DeleteItem(ItemsCategory, key);
                    }
                }

                //
                // Next, perform edits starting from the new count working downwards according to tailEditCount. This range matches [count - tailEditCount, count).
                //

                if (tailEditCount > 0)
                {
                    //
                    // Save each edited element by dispatching to IStackPersistence.Save which has access to the stack element static type used for serialization.
                    //
                    // NB: We perform a single dispatch to persist all of the newly added elements at the top of the stack to reduce seek times in the underlying Stack<T> (see Wrapper<T>).
                    //

                    var target = (IStackPersistence)_wrapper;

                    target.Save(SerializationFactory, tailEditCount, GetTailStreams());

                    // <summary>
                    // Local function to get a sequence of streams to persist the edited elements to. The sequence will yield streams for the elements at indexes (count, count - tailEditCount],
                    // resulting in reverse-order access of the key/value store.
                    // </summary>
                    IEnumerable<Stream> GetTailStreams()
                    {
                        for (var i = countValue - 1; i >= countValue - tailEditCount; i--)
                        {
                            var key = GetKeyForIndex(i);

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
                // Reflect the element count that was persisted in the _count field.
                //

                (_count, _) = GetCount(_dirtyHistory);


                //
                // We no longer need the edit pages.
                //

                _dirtyHistory = null;
            }

            /// <summary>
            /// Gets the element count and the historical minimum element count of the stack, incorporating any pending in-memory edits on the specified <paramref name="dirty"/> edit pages.
            /// </summary>
            /// <param name="dirty">The edit pages to consider for the element count determination.</param>
            /// <returns>A tuple containing the element count of the stack, and the historical minimum element count, including the edits on the specified <paramref name="dirty"/> edit pages.</returns>
            private (int count, int minCount) GetCount(DirtyState[] dirty)
            {
                //
                // Every edit page keeps track of a stack balance relative to the element count that existed at the point of creating the new page. Simply add these to the original _count value.
                // In addition, compute the lowest element count we've ever seen starting from the original _count value by applying edit pages in chronological order. This value is used to determine the dirty element indexes.
                //

                var count = _count;
                var minCount = _count;

                foreach (var edit in dirty)
                {
                    //
                    // First, figure out "how deep we've fallen", for example because of N Pop operations followed by M Push operations.
                    //

                    var candidateMinCount = count + edit.MinimumBalance;

                    if (candidateMinCount < minCount)
                    {
                        minCount = candidateMinCount;
                    }

                    //
                    // Next, apply the net balance to the running count.
                    //

                    count += edit.Balance;

                    Debug.Assert(count >= 0);
                    Debug.Assert(minCount >= 0);
                }

                return (count, minCount);
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}.Push(T)"/> when a push operation takes place.
            /// </summary>
            private void Push()
            {
                //
                // Mark the state as dirty and update the latest edit page to account for the push operation.
                //

                StateChanged = true;

                _dirty.State.Push();
            }

            /// <summary>
            /// Called by <see cref="Wrapper{T}.Pop"/> when a pop operation takes place.
            /// </summary>
            private void Pop()
            {
                //
                // Mark the state as dirty and update the latest edit page to account for the pop operation.
                //
                // REVIEW: Should we add a means to revert StateChanged in case Push and Pop operations balance out?
                //

                StateChanged = true;

                _dirty.State.Pop();
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory stack representation with element type <typeparamref name="T"/> by deserializing state that was loaded by <see cref="LoadCore(IStateReader)"/>.
            /// If the entity has not been persisted before, this methods returns a new empty stack instance.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the stack.</typeparam>
            /// <returns>An instance of type Stack{<typeparamref name="T"/>} containing the data represented by the storage entity.</returns>
            private Stack<T> Restore<T>()
            {
                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize stack elements from. Otherwise, return a fresh stack instance.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<T>();

                    //
                    // Deserialize all the stack elements.
                    //

                    var res = new Stack<T>(_data.Length);

                    //
                    // NB: The eventual objects loaded by LoadCore occur in the push order.
                    //

                    for (var i = 0; i < _data.Length; i++)
                    {
                        res.Push(_data[i].Deserialize(deserializer));
                    }

                    _data = null;

                    return res;
                }
                else
                {
                    return new Stack<T>();
                }
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="Stack"/> storage entity to the statically typed <see cref="Wrapper{T}"/> instance.
            /// </summary>
            private interface IStackPersistence
            {
                /// <summary>
                /// Saves the specified <paramref name="tailCount"/> number of stack elements from the top of the stack (in default stack enumeration order, i.e. moving towards the bottom of the stack) to the streams yielded by the <paramref name="streams"/> sequence.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="tailCount">The number of stack elements to save starting from the top of the queue.</param>
                /// <param name="streams">The streams to save the stack elements to. This sequence has a length equal to <paramref name="tailCount"/>.</param>
                /// <example>
                /// Consider a stack obtained by the following operation sequence:
                /// <c>
                /// Push(42); Push(43); Push(44);
                /// </c>
                /// If persistence of a tail length 2 is requested, the following save operations should take place:
                /// <c>
                /// streams[0].Save(44); streams[1].Save(43);
                /// </c>
                /// </example>
                void Save(ISerializerFactory serializerFactory, int tailCount, IEnumerable<Stream> streams);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted stack with element type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the elements stored in the stack.</typeparam>
            private sealed class Wrapper<T> : PersistedBase, IPersistedStack<T>, IStackPersistence
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly Stack _storage;

                /// <summary>
                /// The stored stack, always reflecting the latest in-memory state.
                /// </summary>
                private readonly Stack<T> _stack;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the stack.</param>
                /// <param name="storage">The storage entity representing the stack.</param>
                /// <param name="stack">The initial stack. This could either be the result of deserializing persisted state, or an empty stack for a new entity.</param>
                public Wrapper(string id, Stack storage, Stack<T> stack)
                    : base(id)
                {
                    _storage = storage;
                    _stack = stack;
                }

                /// <summary>
                /// Gets the number of elements in the stack.
                /// </summary>
                public int Count => _stack.Count;

                /// <summary>
                /// Removes and returns the object at the top of the stack.
                /// </summary>
                /// <returns>The object removed from the top of the stack.</returns>
                /// <exception cref="InvalidOperationException">The stack is empty.</exception>
                public T Pop()
                {
                    //
                    // First pop the next element from the stack, allowing an exception to occur prior to having touched the storage entity.
                    //

                    var res = _stack.Pop();

                    //
                    // Track the pop operation in the storage entity.
                    //

                    _storage.Pop();

                    return res;
                }

                /// <summary>
                /// Inserts an object at the top of the stack.
                /// </summary>
                /// <param name="value">The object to push onto the stack.</param>
                public void Push(T value)
                {
                    //
                    // First push the element on the stack. For consistency, we do this prior to updating the storage entity.
                    //

                    _stack.Push(value);

                    //
                    // Track the push operation in the storage entity.
                    //

                    _storage.Push();
                }

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the stack, in pop order.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the stack.</returns>
                public IEnumerator<T> GetEnumerator() => _stack.GetEnumerator();

                /// <summary>
                /// Returns the object at the top of the stack without removing it.
                /// </summary>
                /// <returns>The object at the top of the stack.</returns>
                /// <exception cref="InvalidOperationException">The stack is empty.</exception>
                public T Peek() => _stack.Peek();

                /// <summary>
                /// Gets an enumerator to enumerate over the elements in the stack, in pop order.
                /// </summary>
                /// <returns>An enumerator to enumerate over the elements in the stack.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the specified <paramref name="tailCount"/> number of stack elements from the top of the stack (in default stack enumeration order, i.e. moving towards the bottom of the stack) to the streams yielded by the <paramref name="streams"/> sequence.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="tailCount">The number of stack elements to save starting from the top of the queue.</param>
                /// <param name="streams">The streams to save the stack elements to. This sequence has a length equal to <paramref name="tailCount"/>.</param>
                void IStackPersistence.Save(ISerializerFactory serializerFactory, int tailCount, IEnumerable<Stream> streams)
                {
                    //
                    // Get the serializer once to reduce overheads.
                    //

                    var serializer = serializerFactory.GetSerializer<T>();

                    //
                    // Zip the stack elements (most recent first) with the corresponding streams to persist to.
                    //
                    // PERF: While the enumeration below is O(tailCount), it results in a reverse order for persistence operations. Should we consider a custom Stack<T> implementation for fast in-order random access?
                    //

                    using var stackEnumerator = _stack.GetEnumerator();
                    using var streamEnumerator = streams.GetEnumerator();

                    for (var i = 0; i < tailCount; i++)
                    {
                        Invariant.Assert(stackEnumerator.MoveNext(), "No more elements in stack.");
                        Invariant.Assert(streamEnumerator.MoveNext(), "No more streams to write to.");

                        var element = stackEnumerator.Current;

                        using var stream = streamEnumerator.Current;

                        serializer.Serialize(element, stream);
                    }

                    Invariant.Assert(!streamEnumerator.MoveNext(), "Should have consumed all streams.");
                }
            }

            /// <summary>
            /// State kept on dirty pages to keep track of all push and pop operations that have to be persisted in the next checkpoint.
            /// </summary>
            private sealed class DirtyState
            {
                /// <summary>
                /// Gets the lowest stack balance value (see <see cref="Balance"/>) that has ever existed on this edit page during the edit page's lifetime.
                /// </summary>
                /// <remarks>
                /// The following invariant holds:
                /// <c>
                /// minCount[i] = count[i - 1] + minBalance[i]
                /// </c>
                /// where <c>count[i]</c> represents the computed count up to and including edit page <c>i</c>, and <c>balance[i]</c> represents the minimum stack balance on edit page <c>i</c>.
                /// By taking the minimum of the <c>minCount[i]</c> values for edit pages <c>i</c> in the range [0, n), one can determine the minimum element count that ever existed in the stack during the lifetime of edit pages [0, n).
                /// </remarks>
                public int MinimumBalance { get; private set; }

                /// <summary>
                /// Gets the stack balance accounting for all push and pop operations that occurred while this edit page was writeable.
                /// </summary>
                /// <remarks>
                /// The following invariant holds:
                /// <c>
                /// count[i] = count[i - 1] + balance[i]
                /// </c>
                /// where <c>count[i]</c> represents the computed count up to and including edit page <c>i</c>, and <c>balance[i]</c> represents the stack balance on edit page <c>i</c>.
                /// </remarks>
                public int Balance { get; private set; }

                /// <summary>
                /// Called when an element is pushed onto the stack.
                /// </summary>
                public void Push()
                {
                    Balance++;
                }

                /// <summary>
                /// Called when an element is popped from the stack.
                /// </summary>
                public void Pop()
                {
                    Balance--;

                    //
                    // Keep a local minimum of the stack balance tracked by this edit page. This value is used by the storage entity during a save operation to determine the lowest element count in order to issue delete operations.
                    //

                    if (Balance < MinimumBalance)
                    {
                        MinimumBalance = Balance;
                    }
                }
            }
        }
    }
}
