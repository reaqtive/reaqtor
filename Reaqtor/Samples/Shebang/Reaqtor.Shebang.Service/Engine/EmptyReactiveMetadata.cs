// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Shebang.Service
{
    //
    // Registry of artifacts defined external to the query engine but available for binding purposes, e.g.
    // an external catalog of shared query operators across all engines. For this sample, we define all
    // artifacts in the engine (and hence they will end up in the checkpoint store and take some space).
    //
    // Alternatively, this could be wired up to in-memory static artifact dictionaries, or external stores
    // using a query provider. The latter is less recommended given that the interfaces are synchronous for
    // historical reasons, and because queries will run during recovery, thus making an engine dependent on
    // external state which brings additional complexity (dealing with failures, backup/restore, etc.).
    //

    public sealed class EmptyReactiveMetadata : IReactiveMetadata
    {
        public IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories => new EmptyQueryableDictionary<Uri, IReactiveStreamFactoryDefinition>();

        public IQueryableDictionary<Uri, IReactiveStreamProcess> Streams => new EmptyQueryableDictionary<Uri, IReactiveStreamProcess>();

        public IQueryableDictionary<Uri, IReactiveObservableDefinition> Observables => new EmptyQueryableDictionary<Uri, IReactiveObservableDefinition>();

        public IQueryableDictionary<Uri, IReactiveObserverDefinition> Observers => new EmptyQueryableDictionary<Uri, IReactiveObserverDefinition>();

        public IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories => new EmptyQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition>();

        public IQueryableDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions => new EmptyQueryableDictionary<Uri, IReactiveSubscriptionProcess>();

        private sealed class EmptyQueryableDictionary<Key, Value> : IQueryableDictionary<Key, Value>
        {
            public Value this[Key key] => throw new System.Collections.Generic.KeyNotFoundException();

            public IQueryable<Key> Keys => Enumerable.Empty<Key>().AsQueryable();

            public IQueryable<Value> Values => Enumerable.Empty<Value>().AsQueryable();

            public Expression Expression => Expression.Constant(this);

            public Type ElementType => typeof(KeyValuePair<Key, Value>);

            public IQueryProvider Provider => Enumerable.Empty<Key>().AsQueryable().Provider;

            public int Count => 0;

            IEnumerable<Key> IReadOnlyDictionary<Key, Value>.Keys { get { yield break; } }

            IEnumerable<Value> IReadOnlyDictionary<Key, Value>.Values { get { yield break; } }

            public bool ContainsKey(Key key) => false;

            public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator() { yield break; }

            public bool TryGetValue(Key key, out Value value)
            {
                value = default;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
