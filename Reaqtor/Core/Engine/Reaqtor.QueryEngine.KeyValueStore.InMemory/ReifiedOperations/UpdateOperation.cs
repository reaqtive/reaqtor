// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class UpdateOperation<TKey, TValue> : ReifiedOperation<TKey, TValue>
    {
        private long _sequenceId = -1;

        public UpdateOperation(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override OperationType OperationType => OperationType.Update;

        public TKey Key { get; }

        public TValue Value { get; }

        public override OperationResult<TKey, TValue> Apply(ref ImmutableSortedDictionary<TKey, Sequenced<TValue>> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(Key))
            {
                var val = _sequenceId == -1 ? new Sequenced<TValue>(Value) : new Sequenced<TValue>(Value, _sequenceId);

                _sequenceId = val.SequenceId;

                dictionary = dictionary.SetItem(Key, val);

                return new UpdateOperationResult<TKey, TValue>(keyNotFound: false);
            }

            return new UpdateOperationResult<TKey, TValue>(keyNotFound: true);
        }
    }
}
