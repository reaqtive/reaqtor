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
    internal interface IPersistedDictionaryOperationFactory<TKey, TValue> : IDictionaryOperationFactory<TKey, TValue>, IPersistedOperationFactory<IPersistedDictionary<TKey, TValue>>
    {
    }

    internal static class PersistedDictionaryOperation
    {
        public static IPersistedDictionaryOperationFactory<TKey, TValue> WithType<TKey, TValue>() => new PersistedDictionaryOperationFactory<TKey, TValue>();

        private sealed class PersistedDictionaryOperationFactory<TKey, TValue> : IPersistedDictionaryOperationFactory<TKey, TValue>
        {
            public CountDictionaryOperation<TKey, TValue> Count() => DictionaryOperation.Count<TKey, TValue>();

            public EnumerateEnumerableOperation<KeyValuePair<TKey, TValue>> Enumerate() => DictionaryOperation.Enumerate<TKey, TValue>();

            public GetIdPersistedOperation<IPersistedDictionary<TKey, TValue>> GetId() => GetId<IPersistedDictionary<TKey, TValue>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public AddDictionaryOperation<TKey, TValue> Add(TKey key, TValue value) => DictionaryOperation.Add(key, value);

            AddCollectionOperation<KeyValuePair<TKey, TValue>> ICollectionOperationFactory<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> value) => CollectionOperation.Add(value);

            public ClearCollectionOperation<KeyValuePair<TKey, TValue>> Clear() => DictionaryOperation.Clear<TKey, TValue>();

            public ContainsCollectionOperation<KeyValuePair<TKey, TValue>> Contains(KeyValuePair<TKey, TValue> value) => DictionaryOperation.Contains(value);

            public ContainsKeyDictionaryOperation<TKey, TValue> ContainsKey(TKey key) => DictionaryOperation.ContainsKey<TKey, TValue>(key);

            public CopyToCollectionOperation<KeyValuePair<TKey, TValue>> CopyTo(KeyValuePair<TKey, TValue>[] array, int index) => DictionaryOperation.CopyTo(array, index);

            public RemoveCollectionOperation<KeyValuePair<TKey, TValue>> Remove(KeyValuePair<TKey, TValue> value) => DictionaryOperation.Remove(value);

            public IsReadOnlyCollectionOperation<KeyValuePair<TKey, TValue>> IsReadOnly() => DictionaryOperation.IsReadOnly<TKey, TValue>();

            public ThisResultOperation<T> This<T>() => Operation.This<T>();

            public ThisResultOperation<IPersistedDictionary<TKey, TValue>> This() => This<IPersistedDictionary<TKey, TValue>>();

            CountReadOnlyCollectionOperation<KeyValuePair<TKey, TValue>> IReadOnlyCollectionOperationFactory<KeyValuePair<TKey, TValue>>.Count() => CollectionOperation.Count<KeyValuePair<TKey, TValue>>();

            public GetDictionaryOperation<TKey, TValue> Get(TKey key) => DictionaryOperation.Get<TKey, TValue>(key);

            public SetDictionaryOperation<TKey, TValue> Set(TKey key, TValue value) => DictionaryOperation.Set<TKey, TValue>(key, value);

            public GetKeysDictionaryOperation<TKey, TValue> GetKeys() => DictionaryOperation.GetKeys<TKey, TValue>();

            public GetValuesDictionaryOperation<TKey, TValue> GetValues() => DictionaryOperation.GetValues<TKey, TValue>();

            public RemoveDictionaryOperation<TKey, TValue> Remove(TKey key) => DictionaryOperation.Remove<TKey, TValue>(key);

            public TryGetValueDictionaryOperation<TKey, TValue> TryGetValue(TKey key) => DictionaryOperation.TryGetValue<TKey, TValue>(key);
        }
    }
}
