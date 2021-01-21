// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.QueryEngine.Events;
using Reaqtor.Reactive;
using Reaqtor.Reliable;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reactive event processing query engine that supports checkpointing.
    /// </summary>
    public partial class CheckpointingQueryEngine : ICheckpointingQueryEngine, IAsyncDisposable, IDisposable
    {
        private readonly IReactiveServiceResolver _serviceResolver;
        private readonly IKeyValueStore _keyValueStore;
        private readonly ISerializationPolicy _serializationPolicy;
        private readonly IDictionary<Type, Type> _unquoteTypeMap;

        private readonly QueryEngineRegistry _registry;

        private readonly IReactiveExpressionServices _expressionService;
        private readonly ServiceContext _reactiveService;
        private readonly ReliableServiceContext _reliableService;
        private readonly CoreReactiveEngine _engine;
        private readonly CheckpointableStateManager _checkpointManager;

        private readonly TransactionLogManager _transactionLogManager;
        private readonly ReadOnlyMetadataServiceContext _operatorContextReactiveService;
        private readonly ExecutionEnvironment _executionEnvironment;

        private readonly OperationTracker _tracker = new();
        private bool _disposed;

        #region Constructors

        /// <summary>
        /// Creates a new event processing engine that supports checkpointing.
        /// </summary>
        /// <param name="uri">URI identifying the engine. This URI should be unique across the cluster.</param>
        /// <param name="serviceResolver">Resolver used by the engine to locate artifacts in reactive services across the cluster.</param>
        /// <param name="scheduler">Scheduler used by the engine to process events and to run maintenance operations.</param>
        /// <param name="metadataRegistry">Metadata registry used by the engine to retrieve definitions of artifacts.</param>
        /// <param name="keyValueStore">Key value store used for persistent storage of the engine's state.</param>
        /// <param name="serializationPolicy">Serialization policy used to manage state serialization.</param>
        /// <param name="quotedTypeConversionTargets">Supplies an external mapping used to unquote query operators.</param>
        /// <param name="traceSource">Trace source used by the engine to write diagnostic information.</param>
        /// <param name="delegateCache">Compiled delegate cache used to optimize compilation of expressions. A cache may be shared across engines on the same host.</param>
        public CheckpointingQueryEngine(Uri uri, IReactiveServiceResolver serviceResolver, IScheduler scheduler, IReactiveMetadata metadataRegistry, IKeyValueStore keyValueStore, ISerializationPolicy serializationPolicy, IQuotedTypeConversionTargets quotedTypeConversionTargets, TraceSource traceSource = null, ICompiledDelegateCache delegateCache = null)
        {
            // TODO: remove trace source and make it an option
            // TODO: remove delegate cache and make it an option

            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (uri.ToCanonicalString().EndsWith("/", StringComparison.Ordinal))
                throw new ArgumentException("Container URI must not end in '/'.", nameof(uri));
            if (metadataRegistry == null)
                throw new ArgumentNullException(nameof(metadataRegistry));

            Uri = uri;
            _serviceResolver = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _keyValueStore = keyValueStore ?? throw new ArgumentNullException(nameof(keyValueStore));
            _serializationPolicy = serializationPolicy ?? throw new ArgumentNullException(nameof(serializationPolicy));
            _unquoteTypeMap = CreateUnquoterTypeMap(quotedTypeConversionTargets);
            TraceSource = traceSource;

            _expressionService = new EngineExpressionService();
            _registry = new QueryEngineRegistry(new BuiltinEntitiesRegistry(this, metadataRegistry));

            _engine = new CoreReactiveEngine(this);
            _checkpointManager = new CheckpointableStateManager(_engine, uri, traceSource);
            _reactiveService = new ServiceContext(_engine);
            _operatorContextReactiveService = new ReadOnlyMetadataServiceContext(_engine);
            _executionEnvironment = new ExecutionEnvironment(_registry, _operatorContextReactiveService);
            _reliableService = new ReliableServiceContext(_engine);

            _transactionLogManager = new TransactionLogManager(uri, _keyValueStore, _serializationPolicy);
            ServiceProvider = new AsyncReactiveEngineProvider(_engine);

            Options = new ConfigurationOptions();
            Options.ExpressionPolicy.DelegateCache = delegateCache ?? new SimpleCompiledDelegateCache();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an object exposing configuration options that can be set on the engine.
        /// </summary>
        public ConfigurationOptions Options { get; }

        /// <summary>
        /// Gets the current status of the engine.
        /// </summary>
        public QueryEngineStatus Status => _checkpointManager.Status;

        /// <summary>
        /// Gets whether an unload operation has been initiated on the engine.
        /// </summary>
        public bool HasUnloadStarted => (Status & (QueryEngineStatus.Unloading | QueryEngineStatus.Unloaded | QueryEngineStatus.UnloadRequested | QueryEngineStatus.UnloadFailed)) != 0;

        /// <summary>
        /// Gets the URI identifying the engine.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the reactive service exposed by the engine.
        /// </summary>
        public IReactive ReactiveService
        {
            get
            {
                CheckAccess();

                return _reactiveService;
            }
        }

        /// <summary>
        /// Gets the reliable reactive service exposed by the engine.
        /// </summary>
        public IReliableReactive ReliableReactiveService
        {
            get
            {
                CheckAccess();

                return _reliableService;
            }
        }

        /// <summary>
        /// Gets the scheduler used by the engine to process events and run maintenance operations.
        /// </summary>
        /// <remarks>This scheduler may be paused and resumed by the engine at various times and should not be used to schedule work that's not related to this engine instance.</remarks>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Gets the trace source used by the engine to write diagnostic information.
        /// </summary>
        public TraceSource TraceSource { get; }

        /// <summary>
        /// Get the reactive provider exposed by the engine.
        /// </summary>
        public IReactiveServiceProvider ServiceProvider { get; }

        #endregion

        #region Events

        /// <summary>
        /// An event triggered when a runtime entity is created.
        /// </summary>
        public event EventHandler<ReactiveEntityEventArgs> EntityCreated;

        /// <summary>
        /// An event triggered when a runtime entity is deleted.
        /// </summary>
        public event EventHandler<ReactiveEntityEventArgs> EntityDeleted;

        /// <summary>
        /// An event triggered when a definition entity is defined.
        /// </summary>
        public event EventHandler<ReactiveEntityEventArgs> EntityDefined;

        /// <summary>
        /// An event triggered when a definition entity is undefined.
        /// </summary>
        public event EventHandler<ReactiveEntityEventArgs> EntityUndefined;

        /// <summary>
        /// An event triggered when a runtime entity failed to load during recovery.
        /// </summary>
        public event EventHandler<ReactiveEntityLoadFailedEventArgs> EntityLoadFailed;

        /// <summary>
        /// An event triggered when a runtime entity failed to save during checkpointing.
        /// </summary>
        public event EventHandler<ReactiveEntitySaveFailedEventArgs> EntitySaveFailed;

        /// <summary>
        /// An event triggered when a runtime entity failed to save during checkpointing.
        /// </summary>
        public event EventHandler<ReactiveEntityReplayFailedEventArgs> EntityReplayFailed;

        /// <summary>
        /// An event triggered when the scheduler for the query engine is pausing.
        /// </summary>
        public event EventHandler<SchedulerPausingEventArgs> SchedulerPausing;

        /// <summary>
        /// An event triggered when the scheduler for the query engine has been paused.
        /// </summary>
        public event EventHandler<SchedulerPausedEventArgs> SchedulerPaused;

        /// <summary>
        /// An event triggered when the scheduler for the query engine is continuing.
        /// </summary>
        public event EventHandler<SchedulerContinuingEventArgs> SchedulerContinuing;

        /// <summary>
        /// An event triggered when the scheduler for the query engine has continued.
        /// </summary>
        public event EventHandler<SchedulerContinuedEventArgs> SchedulerContinued;

        #endregion

        #region Methods

        #region ICheckpointable

        /// <summary>
        /// Saves the state of the event processing engine to a store.
        /// </summary>
        /// <param name="writer">Writer to save state to.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public async Task CheckpointAsync(IStateWriter writer, CancellationToken token = default, IProgress<int> progress = null)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            using var _ = _tracker.Enter();

            CheckAccess();

            await _checkpointManager.CheckpointAsync(writer, token, progress).ConfigureAwait(false);
        }

        /// <summary>
        /// Recovers the state of the event processing engine from a store.
        /// </summary>
        /// <param name="reader">Reader to load state from.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public async Task RecoverAsync(IStateReader reader, CancellationToken token = default, IProgress<int> progress = null)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            using var _ = _tracker.Enter();

            CheckAccess();

            await _checkpointManager.RecoverAsync(reader, token, progress).ConfigureAwait(false);
        }

        /// <summary>
        /// Unloads the event processing engine.
        /// </summary>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public async Task UnloadAsync(IProgress<int> progress = null)
        {
            using var _ = _tracker.Enter();

            await _checkpointManager.UnloadAsync(progress).ConfigureAwait(false);
        }

        #endregion

#pragma warning disable IDE0079 // Remove unnecessary suppressions.
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize. (Analyzer does not yet know about IAsyncDisposable.)

#if NET5_0 || NETSTANDARD2_1
        /// <summary>
        /// Disposes resources asynchronously.
        /// </summary>
        /// <returns>A task to await the completion of the dispose operation.</returns>
        public async ValueTask DisposeAsync()
#else
        /// <summary>
        /// Disposes resources asynchronously.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task to await the completion of the dispose operation.</returns>
        public async Task DisposeAsync(CancellationToken token)
#endif
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

#pragma warning restore CA1816
#pragma warning restore IDE0079

        /// <summary>
        /// Disposes resources asynchronously.
        /// </summary>
        /// <returns>A task to await the completion of the dispose operation.</returns>
#if NET5_0 || NETSTANDARD2_1
        protected virtual async ValueTask DisposeAsyncCore()
#else
        protected virtual async Task DisposeAsyncCore()
#endif
        {
            if (!_disposed)
            {
                await UnloadAsync().ConfigureAwait(false);
                await _tracker.DisposeAsync().ConfigureAwait(false);

                _checkpointManager.Dispose();
                _transactionLogManager.Dispose();
                _registry.Dispose();

                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    //
                    // NB: We want to have the same effect as DisposeAsync in terms of unloading the engine
                    //     and erecting the operation barrier.
                    //

                    UnloadAsync().Wait();
                    _tracker.DisposeAsync()
#if NET5_0 || NETSTANDARD2_1
                        .AsTask()
#endif
                        .Wait();

                    _checkpointManager.Dispose();
                    _transactionLogManager.Dispose();
                    _registry.Dispose();

                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Processes a request to create an operator context to thread through an initialized artifact using SetContext.
        /// </summary>
        /// <param name="instanceId">URI identifying the artifact instance that will receive the operator context.</param>
        /// <returns>Operator context that will be passed to the artifact during its initialization using SetContext.</returns>
        protected virtual IHostedOperatorContext CreateOperatorContext(Uri instanceId)
        {
            // Notice we use the _reactiveService here rather than the ReactiveService. This is to avoid
            // hitting EngineUnloadedException in operators upon attempting to access the service while
            // an unload is in progress. Longer term, we should phase the unload a bit more, such that
            // operators can get a heads-up about the unload being initiated.
            return new HostedOperatorContext(instanceId, Scheduler, TraceSource, _executionEnvironment, _operatorContextReactiveService);
        }

        /// <summary>
        /// Checks whether the caller has access to the engine.
        /// </summary>
        private void CheckAccess()
        {
            var status = Status;
            if ((status & (QueryEngineStatus.Unloading | QueryEngineStatus.Unloaded | QueryEngineStatus.UnloadRequested | QueryEngineStatus.UnloadFailed)) != QueryEngineStatus.None)
            {
                throw new EngineUnloadedException(string.Format(CultureInfo.InvariantCulture, "Query engine '{0}' has been unloaded and cannot accept new requests.", Uri));
            }
        }

        #endregion

        #region BuiltinQueryRegistry

        private Expression LookupForeignFunction(string key)
        {
            var lookup = Options.ForeignFunctionBinder;
            if (lookup != null)
            {
                return lookup(key);
            }

            return null;
        }

        #endregion

        #region Expression Rewrites

#pragma warning disable format // Formatted as tables.

        private static readonly Dictionary<Type, Type> s_unquoteTypeMap = new()
        {
            { typeof(IReactiveQubjectFactory<,>),    typeof(IReliableSubjectFactory<,>) },
            { typeof(IReactiveSubjectFactory<,>),    typeof(IReliableSubjectFactory<,>) },
            { typeof(IReactiveQubject),              typeof(IMultiSubject)              },
            { typeof(IReactiveQubject<,>),           typeof(IReliableMultiSubject<,>)   },
            { typeof(IReactiveSubject<,>),           typeof(IReliableMultiSubject<,>)   },
            { typeof(IReactiveQbservable<>),         typeof(ISubscribable<>)            },
            { typeof(IReactiveObservable<>),         typeof(ISubscribable<>)            },
            { typeof(IReactiveGroupedQbservable<,>), typeof(IGroupedSubscribable<,>)    },
            { typeof(IReactiveGroupedObservable<,>), typeof(IGroupedSubscribable<,>)    },
            { typeof(IReactiveQbserver<>),           typeof(IObserver<>)                },
            { typeof(IReactiveObserver<>),           typeof(IObserver<>)                },
            // TODO-SUBFACT
            //{ typeof(IReactiveQubscriptionFactory),   typeof(ISubscriptionFactory)      },
            //{ typeof(IReactiveQubscriptionFactory<>), typeof(ISubscriptionFactory<>)    },
            { typeof(IReactiveQubscription),         typeof(ISubscription)              },
            { typeof(IReactiveSubscription),         typeof(ISubscription)              },
        };

#pragma warning restore format

        private static IDictionary<Type, Type> CreateUnquoterTypeMap(IQuotedTypeConversionTargets quotedTypeConversionTargets)
        {
            var typeMap = quotedTypeConversionTargets?.TypeMap;

            if (typeMap == null)
            {
                return s_unquoteTypeMap;
            }
            else
            {
                var map = new Dictionary<Type, Type>(s_unquoteTypeMap);

                foreach (var entry in typeMap)
                {
                    map.Add(entry.Key, entry.Value);
                }

                return map;
            }
        }

        private Expression RewriteQuotedReactiveToSubscribable(Expression expr)
        {
            Expression detupletized = ExpressionHelpers.Detupletize(expr);

            //
            // TODO: Address the asymmetry of this rewriting step, namely the use of reliable interfaces.
            //

            var unquoter = new SubstituteAndUnquoteRewriter(_unquoteTypeMap);

            Expression result = unquoter.Apply(detupletized);

            return result;
        }

        #endregion
    }
}
