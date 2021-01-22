// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;
using System.Linq;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class EnumerateOperation<TKey, TValue> : ReifiedOperation<TKey, TValue>
    {
        private readonly Func<TKey, bool> _filter;

        public EnumerateOperation(Func<TKey, bool> filter) => _filter = filter;

        public override OperationType OperationType => OperationType.Enumerate;

        public override OperationResult<TKey, TValue> Apply(ref ImmutableSortedDictionary<TKey, Sequenced<TValue>> dictionary)
        {
            return new EnumerateOperationResult<TKey, TValue>(dictionary.Where(kvp => _filter(kvp.Key)).ToList());
        }
    }
}
