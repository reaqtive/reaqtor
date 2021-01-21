// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    internal enum DictionaryOperationKind
    {
        Add,
        ContainsKey,
        Count,
        Get,
        Set,
        GetKeys,
        GetValues,
        Remove,
        TryGetValue,
    }

    internal interface IDictionaryOperationFactory<TKey, TValue> : ICollectionOperationFactory<KeyValuePair<TKey, TValue>>
    {
        AddDictionaryOperation<TKey, TValue> Add(TKey key, TValue value);
        ContainsKeyDictionaryOperation<TKey, TValue> ContainsKey(TKey key);
        new CountDictionaryOperation<TKey, TValue> Count();
        GetDictionaryOperation<TKey, TValue> Get(TKey key);
        SetDictionaryOperation<TKey, TValue> Set(TKey key, TValue value);
        GetKeysDictionaryOperation<TKey, TValue> GetKeys();
        GetValuesDictionaryOperation<TKey, TValue> GetValues();
        RemoveDictionaryOperation<TKey, TValue> Remove(TKey key);
        TryGetValueDictionaryOperation<TKey, TValue> TryGetValue(TKey key);
    }

    internal abstract class DictionaryOperation : OperationBase
    {
        public abstract DictionaryOperationKind Kind { get; }

        public static IDictionaryOperationFactory<TKey, TValue> WithElementType<TKey, TValue>() => new DictionaryOperationFactory<TKey, TValue>();

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

        private sealed class DictionaryOperationFactory<TKey, TValue> : IDictionaryOperationFactory<TKey, TValue>
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

    internal abstract class DictionaryOperation<TKey, TValue> : DictionaryOperation, IOperation<IDictionary<TKey, TValue>>
    {
        public abstract void Accept(IDictionary<TKey, TValue> dictionary);
    }

    internal abstract class ResultDictionaryOperation<TKey, TValue, TResult> : DictionaryOperation<TKey, TValue>, IResultOperation<IDictionary<TKey, TValue>, TResult>
    {
        public sealed override void Accept(IDictionary<TKey, TValue> dictionary) => _ = GetResult(dictionary);

        public abstract TResult GetResult(IDictionary<TKey, TValue> dictionary);
    }

    internal sealed class AddDictionaryOperation<TKey, TValue> : DictionaryOperation<TKey, TValue>
    {
        internal AddDictionaryOperation(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.Add;

        public TKey Key { get; }
        public TValue Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Add({Key}, {Value})");

        public override void Accept(IDictionary<TKey, TValue> dictionary) => dictionary.Add(Key, Value);
    }

    internal sealed class ContainsKeyDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, bool>
    {
        internal ContainsKeyDictionaryOperation(TKey key)
        {
            Key = key;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.ContainsKey;

        public TKey Key { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"ContainsKey({Key})");

        public override bool GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.ContainsKey(Key);
    }

    internal sealed class CountDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, int>
    {
        internal CountDictionaryOperation() { }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.Count;

        protected override string DebugViewCore => FormattableString.Invariant($"Count()");

        public override int GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.Count;
    }

    internal sealed class GetDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, TValue>
    {
        internal GetDictionaryOperation(TKey key)
        {
            Key = key;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.Get;

        public TKey Key { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Get({Key})");

        public override TValue GetResult(IDictionary<TKey, TValue> dictionary) => dictionary[Key];
    }

    internal sealed class SetDictionaryOperation<TKey, TValue> : DictionaryOperation<TKey, TValue>
    {
        internal SetDictionaryOperation(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.Set;

        public TKey Key { get; }
        public TValue Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Set({Key}, {Value})");

        public override void Accept(IDictionary<TKey, TValue> dictionary) => dictionary[Key] = Value;
    }

    internal sealed class GetKeysDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, IEnumerable<TKey>>
    {
        internal GetKeysDictionaryOperation() { }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.GetKeys;

        protected override string DebugViewCore => FormattableString.Invariant($"Keys()");

        public override IEnumerable<TKey> GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.Keys;
    }

    internal sealed class GetValuesDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, IEnumerable<TValue>>
    {
        internal GetValuesDictionaryOperation() { }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.GetValues;

        protected override string DebugViewCore => FormattableString.Invariant($"Values()");

        public override IEnumerable<TValue> GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.Values;
    }

    internal sealed class RemoveDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, bool>
    {
        internal RemoveDictionaryOperation(TKey key)
        {
            Key = key;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.Remove;

        public TKey Key { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Remove({Key})");

        public override bool GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.Remove(Key);
    }

    internal sealed class TryGetValueDictionaryOperation<TKey, TValue> : ResultDictionaryOperation<TKey, TValue, (bool, TValue)>
    {
        internal TryGetValueDictionaryOperation(TKey key)
        {
            Key = key;
        }

        public override DictionaryOperationKind Kind => DictionaryOperationKind.TryGetValue;

        public TKey Key { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"TryGetValue({Key})");

        public override (bool, TValue) GetResult(IDictionary<TKey, TValue> dictionary) => dictionary.TryGetValue(Key, out var val) ? (true, val) : (false, default);
    }
}
