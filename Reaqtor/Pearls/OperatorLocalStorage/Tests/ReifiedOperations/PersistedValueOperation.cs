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
    internal interface IPersistedValueOperationFactory<T> : IValueOperationFactory<T>, IPersistedOperationFactory<IPersistedValue<T>>
    {
    }

    internal static class PersistedValueOperation
    {
        public static IPersistedValueOperationFactory<T> WithType<T>() => new PersistedValueOperationFactory<T>();

        private sealed class PersistedValueOperationFactory<T> : IPersistedValueOperationFactory<T>
        {
            public GetValueOperation<T> Get() => ValueOperation.WithType<T>().Get();

            public GetIdPersistedOperation<IPersistedValue<T>> GetId() => GetId<IPersistedValue<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public SetValueOperation<T> Set(T value) => ValueOperation.WithType<T>().Set(value);

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedValue<T>> This() => This<IPersistedValue<T>>();
        }
    }
}
