// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class RemoveOperation<TKey, TValue> : ReifiedOperation<TKey, TValue>
    {
        public RemoveOperation(TKey key) => Key = key;

        public override OperationType OperationType => OperationType.Remove;

        public TKey Key { get; }

        public override OperationResult<TKey, TValue> Apply(ref ImmutableSortedDictionary<TKey, Sequenced<TValue>> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(Key))
            {
                dictionary = dictionary.Remove(Key);

                return new RemoveOperationResult<TKey, TValue>(keyDoesNotExist: false);
            }

            return new RemoveOperationResult<TKey, TValue>(keyDoesNotExist: true);
        }
    }
}
