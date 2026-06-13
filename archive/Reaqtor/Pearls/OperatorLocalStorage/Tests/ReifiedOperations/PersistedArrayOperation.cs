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
    internal interface IPersistedArrayOperationFactory<T> : IArrayOperationFactory<T>, IPersistedOperationFactory<IPersistedArray<T>>
    {
    }

    internal static class PersistedArrayOperation
    {
        public static IPersistedArrayOperationFactory<T> WithType<T>() => new PersistedArrayOperationFactory<T>();

        private sealed class PersistedArrayOperationFactory<T> : IPersistedArrayOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => ArrayOperation.Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => ArrayOperation.Enumerate<T>();

            public GetReadOnlyListOperation<T> Get(int index) => ArrayOperation.Get<T>(index);

            public GetIdPersistedOperation<IPersistedArray<T>> GetId() => GetId<IPersistedArray<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public LengthArrayOperation<T> Length() => ArrayOperation.Length<T>();

            public SetArrayOperation<T> Set(int index, T value) => ArrayOperation.Set(index, value);

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedArray<T>> This() => This<IPersistedArray<T>>();
        }
    }
}
