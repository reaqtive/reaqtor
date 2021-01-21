// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    public sealed class QueryEngineEnvironment : IDisposable
    {
        private static readonly IQuotedTypeConversionTargets s_map = QuotedTypeConversionTargets.From(new Dictionary<Type, Type>
        {
            { typeof(ReactiveQbservable),            typeof(Subscribable)               },
            { typeof(ReactiveQbserver),              typeof(Observer)                   },
            { typeof(ReactiveQubjectFactory),        typeof(ReliableSubjectFactory)     },
        });

        private const string QeId = "qe://qe/1";

        private readonly bool _templatize;
        private readonly InMemoryStateStore _stateStore;
        private readonly InMemoryKeyValueStore _keyValueStore;
        private readonly MockReactiveServiceResolver _resolver;
        private readonly PhysicalScheduler _physicalScheduler;
        private readonly ReificationReactiveServiceContext _context;

        private readonly SubjectManager _subjectManager;
        private LogicalScheduler _scheduler;
        private CheckpointingQueryEngine _queryEngine;

        public QueryEngineEnvironment(bool templatize)
        {
            _templatize = templatize;
            _stateStore = new InMemoryStateStore(QeId);
            _keyValueStore = new InMemoryKeyValueStore();
            _resolver = new MockReactiveServiceResolver();
            _physicalScheduler = PhysicalScheduler.Create();
            _subjectManager = new SubjectManager();
            var provider = new MockLazyReactiveEngineProvider(_subjectManager);
            var deploymentContext = new DeploymentReactiveServiceContext(provider);

#if FALSE // NB: Disabled until reification framework host is ported and artifacts are refactored out of remoting into separate libraries.
            Deployable.DefineStreamFactories(deploymentContext, _subjectManager);
            Deployable.Deploy(deploymentContext);
#endif

            _context = new ReificationReactiveServiceContext(provider);
            CreateEngine();
        }

        public IReactive MetadataContext => _context;

        public IReactive EngineContext { get; private set; }

        public void DifferentialCheckpoint()
        {
            using var writer = new InMemoryStateWriter(_stateStore, CheckpointKind.Differential);

            _queryEngine.CheckpointAsync(writer).Wait();
        }

        public void FullCheckpoint()
        {
            using var writer = new InMemoryStateWriter(_stateStore, CheckpointKind.Full);

            _queryEngine.CheckpointAsync(writer).Wait();
        }

        public void Recovery()
        {
            DisposeEngineCore();
            CreateEngine();

            using var reader = new InMemoryStateReader(_stateStore);

            _queryEngine.RecoverAsync(reader).Wait();
        }

        public void Dispose()
        {
            DisposeEngineCore();
            _physicalScheduler.Dispose();
            _resolver.Dispose();
        }

        private void CreateEngine()
        {
            _scheduler = new LogicalScheduler(_physicalScheduler);
            _queryEngine = new CheckpointingQueryEngine(new Uri(QeId), _resolver, _scheduler, _context, _keyValueStore, SerializationPolicy.Default, s_map, null);
            _queryEngine.Options.TemplatizeExpressions = _templatize;
            EngineContext = _queryEngine.ReactiveService;
        }

        private void DisposeEngineCore()
        {
            _queryEngine.UnloadAsync().Wait();
            _queryEngine.Dispose();
            _scheduler.Dispose();
            _subjectManager.Clear();
        }
    }
}
