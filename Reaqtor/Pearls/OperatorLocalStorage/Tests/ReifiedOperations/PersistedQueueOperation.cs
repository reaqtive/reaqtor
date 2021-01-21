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
    internal interface IPersistedQueueOperationFactory<T> : IQueueOperationFactory<T>, IPersistedOperationFactory<IPersistedQueue<T>>
    {
    }

    internal static class PersistedQueueOperation
    {
        public static IPersistedQueueOperationFactory<T> WithType<T>() => new PersistedQueueOperationFactory<T>();

        private sealed class PersistedQueueOperationFactory<T> : IPersistedQueueOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => QueueOperation.Count<T>();

            public DequeueQueueOperation<T> Dequeue() => QueueOperation.Dequeue<T>();

            public EnqueueQueueOperation<T> Enqueue(T value) => QueueOperation.Enqueue(value);

            public EnumerateEnumerableOperation<T> Enumerate() => QueueOperation.Enumerate<T>();

            public GetIdPersistedOperation<IPersistedQueue<T>> GetId() => GetId<IPersistedQueue<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public PeekQueueOperation<T> Peek() => QueueOperation.Peek<T>();

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedQueue<T>> This() => This<IPersistedQueue<T>>();
        }
    }
}
