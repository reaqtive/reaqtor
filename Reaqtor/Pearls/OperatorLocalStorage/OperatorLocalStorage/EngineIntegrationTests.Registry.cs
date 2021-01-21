// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reactive.Linq;

using Reaqtor;
using Reaqtor.Metadata;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        /// <summary>
        /// Implementation of <see cref="IReactiveMetadata"/> using in-memory dictionaries.
        /// </summary>
        private sealed class Registry : ReactiveMetadataBase
        {
            //
            // NB: This utility implements a very narrow case needed for integration tests. It's by no means meant to be reusable as-is.
            //

            public new Dictionary<Uri, IReactiveObservableDefinition> Observables { get; } = new Dictionary<Uri, IReactiveObservableDefinition>();
            public new Dictionary<Uri, IReactiveObserverDefinition> Observers { get; } = new Dictionary<Uri, IReactiveObserverDefinition>();
            public new Dictionary<Uri, IReactiveStreamProcess> Streams { get; } = new Dictionary<Uri, IReactiveStreamProcess>();

            protected override TResult Execute<TResult>(Expression expression)
            {
                var pars = FreeVariableScanner.Scan(expression).Distinct().ToArray();
                var args = new Expression[pars.Length];

                for (var i = 0; i < pars.Length; i++)
                {
                    var par = pars[i];

                    var queryableType = par.Type;

                    args[i] = par.Name switch
                    {
                        "rx://metadata/observables" => Expression.Constant(new EnumerableQueryableDictionary<Uri, IReactiveObservableDefinition>(Observables), queryableType),
                        "rx://metadata/observers" => Expression.Constant(new EnumerableQueryableDictionary<Uri, IReactiveObserverDefinition>(Observers), queryableType),
                        "rx://metadata/streams" => Expression.Constant(new EnumerableQueryableDictionary<Uri, IReactiveStreamProcess>(Streams), queryableType),
                        _ => throw new NotSupportedException(),
                    };
                }

                var bound = Expression.Invoke(Expression.Lambda(expression, pars), args);

                var reduced = BetaReducer.ReduceEager(bound, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: false);

                var res = reduced.Evaluate<TResult>();

                return res;
            }

            private sealed class EnumerableQueryableDictionary<K, V> : IQueryableDictionary<K, V>
            {
                private readonly IDictionary<K, V> _dictionary;

                public EnumerableQueryableDictionary(IDictionary<K, V> dictionary)
                {
                    _dictionary = dictionary;
                }

                public V this[K key] => _dictionary[key];

                public IQueryable<K> Keys => _dictionary.Keys.AsQueryable();

                public IQueryable<V> Values => _dictionary.Values.AsQueryable();

                public int Count => _dictionary.Count;

                IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => _dictionary.Keys;

                IEnumerable<V> IReadOnlyDictionary<K, V>.Values => _dictionary.Values;

                public bool ContainsKey(K key) => _dictionary.ContainsKey(key);

                public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _dictionary.GetEnumerator();

                public bool TryGetValue(K key, out V value) => _dictionary.TryGetValue(key, out value);

                IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

                public Expression Expression => Expression.Constant(_dictionary.AsQueryable());

                public Type ElementType => typeof(KeyValuePair<K, V>);

                public IQueryProvider Provider => _dictionary.AsQueryable().Provider;
            }
        }
    }
}
