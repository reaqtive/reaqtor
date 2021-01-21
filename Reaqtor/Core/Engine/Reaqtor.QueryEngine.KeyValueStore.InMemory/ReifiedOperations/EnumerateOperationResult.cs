// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class EnumerateOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly IReadOnlyList<KeyValuePair<TKey, Sequenced<TValue>>> _results;

        public EnumerateOperationResult(IReadOnlyList<KeyValuePair<TKey, Sequenced<TValue>>> results) => _results = results;

        public override Exception Exception => null;

        public override object Result => _results.Select(kvp => new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value.Object)).GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is EnumerateOperationResult<TKey, TValue> enumerableResult && enumerableResult._results.Count == _results.Count)
            {
                var cnt = _results.Count;

                for (var i = 0; i < cnt; i++)
                {
                    if (!EqualityComparer<TKey>.Default.Equals(_results[i].Key, enumerableResult._results[i].Key) || _results[i].Value.SequenceId != enumerableResult._results[i].Value.SequenceId)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode() => _results.Aggregate(0, (acc, x) => acc ^ x.Key.GetHashCode() & x.Value.SequenceId.GetHashCode());
    }
}
