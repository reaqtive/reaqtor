// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Reaqtive.Storage;

namespace Tests.ReifiedOperations
{
    internal interface IPersistedListOperationFactory<T> : IListOperationFactory<T>, IPersistedOperationFactory<IPersistedList<T>>
    {
    }

    internal static class PersistedListOperation
    {
        public static IPersistedListOperationFactory<T> WithType<T>() => new PersistedListOperationFactory<T>();

        private sealed class PersistedListOperationFactory<T> : IPersistedListOperationFactory<T>
        {
            public AddCollectionOperation<T> Add(T value) => ListOperation.Add(value);

            public ClearCollectionOperation<T> Clear() => ListOperation.Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => ListOperation.Contains(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => ListOperation.CopyTo(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => ListOperation.Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => ListOperation.Enumerate<T>();

            public GetReadOnlyListOperation<T> Get(int index) => ListOperation.Get<T>(index);

            public GetIdPersistedOperation<IPersistedList<T>> GetId() => GetId<IPersistedList<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public IndexOfListOperation<T> IndexOf(T value) => ListOperation.IndexOf(value);

            public InsertListOperation<T> Insert(int index, T value) => ListOperation.Insert(index, value);

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => ListOperation.IsReadOnly<T>();

            public RemoveCollectionOperation<T> Remove(T value) => ListOperation.Remove(value);

            public RemoveAtListOperation<T> RemoveAt(int index) => ListOperation.RemoveAt<T>(index);

            public SetListOperation<T> Set(int index, T value) => ListOperation.Set(index, value);

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedList<T>> This() => This<IPersistedList<T>>();
        }
    }
}
