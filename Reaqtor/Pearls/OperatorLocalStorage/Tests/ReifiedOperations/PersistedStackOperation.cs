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
    internal interface IPersistedStackOperationFactory<T> : IStackOperationFactory<T>, IPersistedOperationFactory<IPersistedStack<T>>
    {
    }

    internal static class PersistedStackOperation
    {
        public static IPersistedStackOperationFactory<T> WithType<T>() => new PersistedStackOperationFactory<T>();

        private sealed class PersistedStackOperationFactory<T> : IPersistedStackOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => StackOperation.Count<T>();

            public PopStackOperation<T> Pop() => StackOperation.Pop<T>();

            public PushStackOperation<T> Push(T value) => StackOperation.Push(value);

            public EnumerateEnumerableOperation<T> Enumerate() => StackOperation.Enumerate<T>();

            public GetIdPersistedOperation<IPersistedStack<T>> GetId() => GetId<IPersistedStack<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public PeekStackOperation<T> Peek() => StackOperation.Peek<T>();

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedStack<T>> This() => This<IPersistedStack<T>>();
        }
    }
}
