// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    internal enum SortedDictionaryOperationKind
    {
    }

    internal interface ISortedDictionaryOperationFactory<TKey, TValue> : IDictionaryOperationFactory<TKey, TValue>
    {
    }

    internal abstract class SortedDictionaryOperation : OperationBase
    {
        public abstract SortedDictionaryOperationKind Kind { get; }

        public static ISortedDictionaryOperationFactory<TKey, TValue> WithElementType<TKey, TValue>() => new SortedDictionaryOperationFactory<TKey, TValue>();

        public static AddDictionaryOperation<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value) => new(key, value);

        public static ClearCollectionOperation<KeyValuePair<TKey, TValue>> Clear<TKey, TValue>() => new();

        public static ContainsCollectionOperation<KeyValuePair<TKey, TValue>> Contains<TKey, TValue>(KeyValuePair<TKey, TValue> value) => new(value);

        public static ContainsKeyDictionaryOperation<TKey, TValue> ContainsKey<TKey, TValue>(TKey key) => new(key);

        public static CopyToCollectionOperation<KeyValuePair<TKey, TValue>> CopyTo<TKey, TValue>(KeyValuePair<TKey, TValue>[] array, int index) => new(array, index);

        public static CountDictionaryOperation<TKey, TValue> Count<TKey, TValue>() => new();

        public static EnumerateEnumerableOperation<KeyValuePair<TKey, TValue>> Enumerate<TKey, TValue>() => new();

        public static IsReadOnlyCollectionOperation<KeyValuePair<TKey, TValue>> IsReadOnly<TKey, TValue>() => new();

        public static RemoveCollectionOperation<KeyValuePair<TKey, TValue>> Remove<TKey, TValue>(KeyValuePair<TKey, TValue> value) => new(value);

        public static GetDictionaryOperation<TKey, TValue> Get<TKey, TValue>(TKey key) => new(key);

        public static SetDictionaryOperation<TKey, TValue> Set<TKey, TValue>(TKey key, TValue value) => new(key, value);

        public static GetKeysDictionaryOperation<TKey, TValue> GetKeys<TKey, TValue>() => new();

        public static GetValuesDictionaryOperation<TKey, TValue> GetValues<TKey, TValue>() => new();

        public static RemoveDictionaryOperation<TKey, TValue> Remove<TKey, TValue>(TKey key) => new(key);

        public static TryGetValueDictionaryOperation<TKey, TValue> TryGetValue<TKey, TValue>(TKey key) => new(key);

        private sealed class SortedDictionaryOperationFactory<TKey, TValue> : ISortedDictionaryOperationFactory<TKey, TValue>
        {
            public AddDictionaryOperation<TKey, TValue> Add(TKey key, TValue value) => Add<TKey, TValue>(key, value);

            public ClearCollectionOperation<KeyValuePair<TKey, TValue>> Clear() => Clear<TKey, TValue>();

            public ContainsCollectionOperation<KeyValuePair<TKey, TValue>> Contains(KeyValuePair<TKey, TValue> value) => Contains<TKey, TValue>(value);

            public ContainsKeyDictionaryOperation<TKey, TValue> ContainsKey(TKey key) => ContainsKey<TKey, TValue>(key);

            public CopyToCollectionOperation<KeyValuePair<TKey, TValue>> CopyTo(KeyValuePair<TKey, TValue>[] array, int index) => CopyTo<TKey, TValue>(array, index);

            CountReadOnlyCollectionOperation<KeyValuePair<TKey, TValue>> IReadOnlyCollectionOperationFactory<KeyValuePair<TKey, TValue>>.Count() => new();

            public CountDictionaryOperation<TKey, TValue> Count() => Count<TKey, TValue>();

            public EnumerateEnumerableOperation<KeyValuePair<TKey, TValue>> Enumerate() => Enumerate<TKey, TValue>();

            public IsReadOnlyCollectionOperation<KeyValuePair<TKey, TValue>> IsReadOnly() => IsReadOnly<TKey, TValue>();

            public RemoveCollectionOperation<KeyValuePair<TKey, TValue>> Remove(KeyValuePair<TKey, TValue> value) => Remove<TKey, TValue>(value);

            AddCollectionOperation<KeyValuePair<TKey, TValue>> ICollectionOperationFactory<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> value) => new(value);

            public GetDictionaryOperation<TKey, TValue> Get(TKey key) => Get<TKey, TValue>(key);

            public SetDictionaryOperation<TKey, TValue> Set(TKey key, TValue value) => Set(key, value);

            public GetKeysDictionaryOperation<TKey, TValue> GetKeys() => GetKeys<TKey, TValue>();

            public GetValuesDictionaryOperation<TKey, TValue> GetValues() => GetValues<TKey, TValue>();

            public RemoveDictionaryOperation<TKey, TValue> Remove(TKey key) => Remove<TKey, TValue>(key);

            public TryGetValueDictionaryOperation<TKey, TValue> TryGetValue(TKey key) => TryGetValue<TKey, TValue>(key);
        }
    }
}
