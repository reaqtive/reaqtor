// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

using Reaqtive.Storage;

namespace Tests.ReifiedOperations
{
    internal interface IPersistedLinkedListOperationFactory<T> : ILinkedListOperationFactory<T>, IPersistedOperationFactory<IPersistedLinkedList<T>>
    {
    }

    internal static class PersistedLinkedListOperation
    {
        public static IPersistedLinkedListOperationFactory<T> WithType<T>() => new PersistedLinkedListOperationFactory<T>();

        private sealed class PersistedLinkedListOperationFactory<T> : IPersistedLinkedListOperationFactory<T>
        {
            public AddCollectionOperation<T> Add(T value) => LinkedListOperation.Add(value);

            public AddAfterLinkedListOperation<T> AddAfter(ILinkedListNode<T> node, T value) => LinkedListOperation.AddAfter<T>(node, value);

            public AddBeforeLinkedListOperation<T> AddBefore(ILinkedListNode<T> node, T value) => LinkedListOperation.AddBefore<T>(node, value);

            public AddFirstLinkedListOperation<T> AddFirst(T value) => LinkedListOperation.AddFirst<T>(value);

            public AddLastLinkedListOperation<T> AddLast(T value) => LinkedListOperation.AddLast<T>(value);

            public ClearCollectionOperation<T> Clear() => LinkedListOperation.Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => LinkedListOperation.Contains(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => LinkedListOperation.CopyTo(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => LinkedListOperation.Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => LinkedListOperation.Enumerate<T>();

            public FindLinkedListOperation<T> Find(T value) => LinkedListOperation.Find<T>(value);

            public FindLastLinkedListOperation<T> FindLast(T value) => LinkedListOperation.FindLast<T>(value);

            public FirstLinkedListOperation<T> First() => LinkedListOperation.First<T>();

            FirstReadOnlyLinkedListOperation<T> IReadOnlyLinkedListOperationFactory<T>.First() => ReadOnlyLinkedListOperation.First<T>();

            public GetIdPersistedOperation<IPersistedLinkedList<T>> GetId() => GetId<IPersistedLinkedList<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => LinkedListOperation.IsReadOnly<T>();

            public LastLinkedListOperation<T> Last() => LinkedListOperation.Last<T>();

            LastReadOnlyLinkedListOperation<T> IReadOnlyLinkedListOperationFactory<T>.Last() => ReadOnlyLinkedListOperation.Last<T>();

            public RemoveCollectionOperation<T> Remove(T value) => LinkedListOperation.Remove(value);

            public RemoveLinkedListOperation<T> Remove(ILinkedListNode<T> node) => LinkedListOperation.Remove<T>(node);

            public RemoveFirstLinkedListOperation<T> RemoveFirst() => LinkedListOperation.RemoveFirst<T>();

            public RemoveLastLinkedListOperation<T> RemoveLast() => LinkedListOperation.RemoveLast<T>();

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedLinkedList<T>> This() => This<IPersistedLinkedList<T>>();
        }
    }
}
