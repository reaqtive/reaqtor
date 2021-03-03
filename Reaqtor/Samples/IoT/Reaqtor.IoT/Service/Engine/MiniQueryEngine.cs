// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Expressions.Core;
using Reaqtor.Metadata;
using Reaqtor.QueryEngine;
using Reaqtor.Reactive;
using Reaqtor.Reliable;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.IoT
{
    //
    // Query engine implementation, specializing the Reactor Core engine on a few facilities.
    //

    public sealed class MiniQueryEngine : CheckpointingQueryEngine
    {
        private static readonly IQuotedTypeConversionTargets s_map = QuotedTypeConversionTargets.From(new Dictionary<Type, Type>
        {
            { typeof(ReactiveQbservable),     typeof(Subscribable)           },
            { typeof(ReactiveQbserver),       typeof(Observer)               },
            { typeof(ReactiveQubjectFactory), typeof(ReliableSubjectFactory) },
        });

        private readonly IngressEgressManager _iemgr;

        internal MiniQueryEngine(Uri uri, IScheduler scheduler, IKeyValueStore store, IngressEgressManager iemgr)
             : base(uri, new Resolver(), scheduler, new Registry(), store, SerializationPolicy.Default, s_map)
        {
            _iemgr = iemgr;
        }

        protected override IHostedOperatorContext CreateOperatorContext(Uri instanceId)
        {
            var res = base.CreateOperatorContext(instanceId);
            return new OperatorContext(this, res);
        }

        //
        // Instances of OperatorContext are given to artifacts hosted within the engine through the SetContext
        // method they can override. This mechanism is used to inject dependencies, and we use it here to provide
        // access to the IngressEgressManager which is used by IngressObservable<T> and EgressObserver<T>.
        //

        private sealed class OperatorContext : IHostedOperatorContext
        {
            private readonly MiniQueryEngine _parent;
            private readonly IHostedOperatorContext _context;

            public OperatorContext(MiniQueryEngine parent, IHostedOperatorContext context) => (_parent, _context) = (parent, context);

            public Uri InstanceId => _context.InstanceId;

            public IReactive ReactiveService => _context.ReactiveService;

            public IScheduler Scheduler => _context.Scheduler;

            public TraceSource TraceSource => _context.TraceSource;

            public IExecutionEnvironment ExecutionEnvironment => _context.ExecutionEnvironment;

            public bool TryGetElement<T>(string id, out T value)
            {
                if (id == "IngressEgressManager" && typeof(T) == typeof(IngressEgressManager))
                {
                    value = (T)(object)_parent._iemgr;
                    return true;
                }

                return _context.TryGetElement(id, out value);
            }
        }

        //
        // The resolver is a mechanism to find artifacts in other query engines either locally or remotely,
        // enabling distributed event processing. Think of this as a DNS lookup mechanism for reactive artifacts
        // to IReactive* services that host these, either located peer-to-peer or through a central catalog. For
        // purposes of this demo, this is way beyond what we need to do here, so we don't support this. I don't
        // expect a need for this in the context of IoT.
        //

        private sealed class Resolver : IReactiveServiceResolver
        {
            public bool TryResolve(Uri uri, out IReactive service) => throw NotSupported();

            public bool TryResolve(Uri uri, out IReactiveProxy service) => throw NotSupported();

            public bool TryResolveReliable(Uri uri, out IReliableReactive service) => throw NotSupported();

            private static Exception NotSupported() => new NotSupportedException("Peer-to-peer resolution not supported by IoT.");
        }

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

        private sealed class Registry : IReactiveMetadata
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
}
