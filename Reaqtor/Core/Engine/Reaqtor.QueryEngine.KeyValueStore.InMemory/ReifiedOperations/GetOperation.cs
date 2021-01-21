// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class GetOperation<TKey, TValue> : ReifiedOperation<TKey, TValue>
    {
        public GetOperation(TKey key) => Key = key;

        public override OperationType OperationType => OperationType.Get;

        public TKey Key { get; }

        public override OperationResult<TKey, TValue> Apply(ref ImmutableSortedDictionary<TKey, Sequenced<TValue>> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(Key))
            {
                return new GetOperationResult<TKey, TValue>(dictionary[Key], keyNotFound: false);
            }

            return new GetOperationResult<TKey, TValue>(default, keyNotFound: true);
        }
    }
}
