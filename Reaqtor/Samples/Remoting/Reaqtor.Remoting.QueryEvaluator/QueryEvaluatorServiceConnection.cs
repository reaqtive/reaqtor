// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive.Scheduler;
using Reaqtive.TestingFramework;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.QueryEngine;
using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.QueryEvaluator
{
    public class QueryEvaluatorServiceConnection : RemotingReactiveServiceConnectionBase, IReactiveQueryEvaluatorConnection
    {
        private static readonly Lazy<PhysicalScheduler> s_scheduler = new(() => PhysicalScheduler.Create());
        private static readonly Uri s_qeId = new("reactor:/qe");

        private IReactivePlatformConfiguration _configuration;
        private TraceSource _traceSource;
        private IReactiveMetadataCache _metadata;
        private IKeyValueStore _keyValueStore;
        private CommandTextParser<Expression> _commandTextParser;
        private CheckpointingQueryEngine _engine;

        public IScheduler Scheduler { get; private set; }

        public override void Configure(IReactivePlatformConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Start()
        {
            _traceSource = GetTraceSource(_configuration);
            Scheduler = GetScheduler(_configuration, _traceSource);
            _metadata = GetMetadata(_configuration);
            _keyValueStore = GetKeyValueStore(_configuration);
            _commandTextParser = new CommandTextParser<Expression>(new UnifyingSerializationHelpers(_metadata));
            ServiceInstanceHelpers.SetMessagingServiceInstance(AppDomain.CurrentDomain, GetMessaging(_configuration));
            StartCore();
        }

        public void Checkpoint()
        {
            var connection = _configuration.StateStoreConnection;
            if (connection == null)
            {
                throw new InvalidOperationException("State storage has not been initialized.");
            }
            var stateWriter = new StateStoreConnectionStateWriter(connection, CheckpointKind.Differential);
            _engine.CheckpointAsync(stateWriter).Wait();
        }

        public void Unload()
        {
            _engine.UnloadAsync().Wait();
        }

        public void Recover()
        {
            StartCore();
        }

        protected virtual Dictionary<string, object> OperatorContextElements =>
            new Dictionary<string, object>
            {
                { Platform.Constants.ContextKey.Name, s_qeId },
                { MessageRouter.ContextHandle, new MessageRouter() },
            };

        protected virtual Func<Expression, Expression> Rewriter => expr => expr;

        protected virtual CheckpointingQueryEngine CreateQueryEngine(Uri uri, IReactiveServiceResolver resolver, IScheduler scheduler, IReactiveMetadata metadata, IKeyValueStore keyValueStore, TraceSource traceSource, IReadOnlyDictionary<string, object> contextElements)
        {
            return new QueryEvaluatorQueryEngine(uri, resolver, scheduler, metadata, keyValueStore, traceSource, contextElements);
        }

        protected virtual void DefineBuiltinOperators()
        {
            TryDefineObservable<Tuple<IReactiveQbservable<T>>, T>(
                new Uri(HostOperatorConstants.CleanupSubscriptionUri),
                t => t.Item1.AsSubscribable().CleanupSubscription().AsQbservable());

            TryDefineSubscriptionFactory<Tuple<IReactiveQbservable<T>, IReactiveQbserver<T>>>(
                new Uri(HostOperatorConstants.SubscribeWithCleanupSubscriptionUri),
                t => t.Item1.CleanupSubscription().QuotedSubscribe(t.Item2));
        }

        protected bool TryDefineObservable<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> expression)
        {
            try
            {
                _engine.ReactiveService.DefineObservable<TArgs, TResult>(uri, expression, null);
                return true;
            }
            catch (EntityAlreadyExistsException)
            {
                return false;
            }
        }

        protected bool TryDefineSubscriptionFactory<TArgs>(Uri uri, Expression<Func<TArgs, IReactiveQubscription>> expression)
        {
            try
            {
                var factory = _engine.ReactiveService.Provider.CreateQubscriptionFactory<TArgs>(expression);
                _engine.ReactiveService.DefineSubscriptionFactory<TArgs>(uri, factory, null);
                return true;
            }
            catch (EntityAlreadyExistsException)
            {
                return false;
            }
        }

        #region IRemotingConnection Implementation

        private void StartCore()
        {
            _engine = CreateQueryEngine(s_qeId, new MockReactiveServiceResolver(), Scheduler.CreateChildScheduler(), _metadata, _keyValueStore, _traceSource, OperatorContextElements);
            SetEngineOptions(_engine, _configuration);
            var stateStore = _configuration.StateStoreConnection;
            if (stateStore != null)
            {
                var stateReader = new StateStoreConnectionStateReader(stateStore);
                _engine.RecoverAsync(stateReader).Wait();
            }
            DefineBuiltinOperators();
            var serviceProvider = new QueryEvaluatorServiceProvider(_engine, _metadata, Rewriter);
            Connection = new ReactiveServiceConnection<Expression>(serviceProvider, _commandTextParser, _commandTextParser);
        }


        #endregion

        protected override void DisposeCore()
        {
            if (_traceSource != null)
            {
                _traceSource.Flush();
                _traceSource.Close();
            }

            Scheduler.Dispose();
        }

        private static TraceSource GetTraceSource(IReactivePlatformConfiguration configuration)
        {
            var listener = default(TraceListener);
            switch (configuration.TraceListenerType)
            {
                case TraceListenerType.Console:
                    listener = new ConsoleTraceListener();
                    break;
                case TraceListenerType.File:
                    listener = new TextWriterTraceListener(configuration.TraceListenerFileName);
                    break;
            }

            var traceSource = default(TraceSource);
            if (listener != null)
            {
                traceSource = new TraceSource("QueryEvaluator")
                {
                    Switch = new SourceSwitch("all") { Level = SourceLevels.All },
                    Listeners = { listener }
                };
            }

            return traceSource;
        }

        private static IScheduler GetScheduler(IReactivePlatformConfiguration configuration, TraceSource traceSource)
        {
            switch (configuration.SchedulerType)
            {
                case SchedulerType.Default:
                    var physicalScheduler = configuration.SchedulerThreadCount.HasValue
                        ? PhysicalScheduler.Create(configuration.SchedulerThreadCount.Value)
                        : PhysicalScheduler.Create();
                    physicalScheduler.TraceSource = traceSource;
                    return new SchedulerProxy(new LogicalScheduler(physicalScheduler), physicalScheduler);
                case SchedulerType.Test:
                    return new SchedulerProxy(new TestScheduler());
                case SchedulerType.Logging:
                    return new SchedulerProxy(new LoggingTestScheduler());
                case SchedulerType.Static:
                    return new SchedulerProxy(new LogicalScheduler(s_scheduler.Value));
                default:
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unexpected scheduler type '{0}'.", configuration.SchedulerType));
            }
        }

        private static IReactiveMetadataCache GetMetadata(IReactivePlatformConfiguration configuration)
        {
            switch (configuration.StorageType)
            {
                case MetadataStorageType.Remoting:
                    {
                        var metadata = new AzureReactiveMetadata(
                            new StorageConnectionTableClient(configuration.StorageConnection),
                            new AzureStorageResolver());
                        return new ReactiveMetadataCache(metadata, new ReactiveMetadataInMemory());
                    }
                // case MetadataStorageType.Azure:
                //     {
                //         var storageAccount = CloudStorageAccount.Parse(configuration.AzureConnectionString);
                //         var metadata = new AzureReactiveMetadata(new AzureTableClient(storageAccount.CreateCloudTableClient()), new AzureStorageResolver());
                //         return new ReactiveMetadataCache(metadata, new ReactiveMetadataInMemory());
                //     }
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected metadata storage type '{0}'.", configuration.StorageType));
            }
        }

        private static IReactiveMessagingConnection GetMessaging(IReactivePlatformConfiguration configuration)
        {
            return configuration.MessagingConnection;
        }

        private static IKeyValueStore GetKeyValueStore(IReactivePlatformConfiguration configuration)
        {
            return new KeyValueStore(configuration.KeyValueStoreConnection);
        }

        private static void SetEngineOptions(CheckpointingQueryEngine engine, IReactivePlatformConfiguration configuration)
        {
            var options = engine.Options;
            var userSettings = configuration.EngineOptions;
            if (userSettings != null)
            {
                var optionsType = typeof(ConfigurationOptions);
                var stringType = typeof(string);

                foreach (var kv in userSettings)
                {
                    var property = optionsType.GetProperty(kv.Key);
                    if (property != null && property.PropertyType == stringType)
                    {
                        property.SetValue(options, kv.Value);
                    }
                    else if (property != null)
                    {
                        var parser = (from m in property.PropertyType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                      where m.Name == "TryParse"
                                      let p = m.GetParameters()
                                      where p.Length == 2 && p[0].ParameterType == typeof(string)
                                      select m)
                                    .SingleOrDefault();

                        if (parser != null)
                        {
                            var args = new object[] { kv.Value, null };
                            var result = parser.Invoke(null, args);
                            if (result.GetType() == typeof(bool) && (bool)result)
                            {
                                property.SetValue(options, args[1]);
                            }
                        }
                    }
                }
            }
        }
    }
}
