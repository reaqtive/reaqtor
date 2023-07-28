// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Reliable;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    //
    // Random note on edges:
    //
    //   Edges enable peer-to-peer management of 1-to-1 or 1-to-N streams between query engines used to support splitting of
    //   queries, which may either be necessary due to affinity requirements (e.g. an observable or observer or operator does
    //   only exist on some engines) or to support scaling. Some examples:
    //
    //     xs.A().B().C().Subscribe(o)
    //
    //   Suppose that B is only available on engine 1, while A and C are available everywhere. But xs is only available on
    //   engine 2 and o is only available on engine 3. In such a case, we need to split the query, with a few options that are
    //   available:
    //
    //     xs.A().Subscribe(t1) on engine 2
    //     t1.B().Subscribe(t2) on engine 1
    //     t2.C().Subscribe(t3) on engine 3
    //
    //   is one of the options. Others involve moving A() and C(), which can run anywhere. The decomposition requires the creation
    //   of intermediate 1-to-1 streams t1, t2, and t3 to flow events across engines. A query coordinator can come up with this
    //   decomposition by analyzing affinity for artifacts (observables, observers, etc.) and resource availability on engines.
    //   There are many ways to materialize the "execution plan". One is to create three global streams, three queries, and keep
    //   track of all of these by keeping a mapping of the top-level subscription to these resources (in order to support deletion).
    //   An alternative is to define t1, t2, t3 in engines, and have the resolver/edge mechanism do the peer-to-peer resolution
    //   (which can be a pure function if the URI for such streams includes a QE's URI as a prefix), as well as tear down of
    //   upstream/downstream resources upon completion or disposal (which reduces the burden on the QC).
    //
    //   However, edges are limited, especially when dynamically growing query operator graphs are involved (e.g. SelectMany) or
    //   when sharing of common subexpressions is warranted (though this is a 1-to-N case which works reasonably well with edges).
    //
    //   Two examples:
    //
    //   1. Suppose xs.A() occurs often across different queries. We may want to create a stream t1 and publish the results of
    //      the common subquery xs.A() into t1 by placing a subscription xs.A().Subscribe(t1) somewhere. Various queries that
    //      need the result of xs.A() can now replace it by t1, which is a 1-to-N multicast point. Lifetime of t1 is now governed
    //      by a reference count, which could be managed by the engine running the query publishing into t1, by counting the
    //      number of downstream consumers that have come in via (output) edge-based reliable subscriptions. However, the QE has
    //      a lot of bookkeeping to do (besides the ref count), because all consumers may be at diferent sequence IDs. This has
    //      worked in practice, albeit with "operator local storage" to make storage of outgoing sequenced events more efficient
    //      than an opaque (potentially huge) blob in checkpoint storage. Nonetheless, there's also a lot of work to manage
    //      the connections between engines. At some point, it just becomes easier to have the stream backed by an external
    //      event pub/sub system such as Service Bus or Event Hub, which also allows for higher degrees of fan-out and higher
    //      reliability (not tied to the upstream engine's availability).
    //
    //   2. Consider a query with a higher-order operator, e.g. xs.A().SelectMany(x => ys(x).B()).C().Subscribe(o). For each
    //      event produced by xs.A(), an inner subscription ys(x).B() is made. Depending on the lifetime of ys(x) instances
    //      (i.e. if/when these produce OnCompleted) and the event volume of xs.A(), this may exceed the limits of what a single
    //      engine can do. Edges can be put to work to support this, by having ys(x) resolve to another engine, causing bridges
    //      to be created. However, there are some limitations to consider. First, current support for edges isn't data-
    //      dependent, so we resolve based on ys, and not the value of x. So the resolver target would be a reactive service
    //      that should be a service group (or at least something backed by multiple engines); otherwise, if the target of ys
    //      is a single QE, we've just pushed the problem down one level. In other words, we need to delegate to a component
    //      that can make placement decisions. Second, the original QE is still the one that runs xs.A().SelectMany(...).C(),
    //      where ... is scaled out, but the "merge" into C() still happens locally. In various cases, we also want that piece
    //      to run elsewhere. As such, it's often turned out more advantageous to let the query coordinator rewrite occurrences
    //      of higher-order operators such as SelectMany in such a way that all "inner" artifacts (in case of SelectMany, that
    //      is inner subscriptions; for things like GroupBy and Window, that is subjects) rendez-vous with itself for lifecycle
    //      management, e.g.:
    //
    //        QE1               runs   xs.A().Subscribe(t1)
    //        QE2               runs   t2.C().Subscribe(o)
    //        QC (any of them)  runs   t1.Subscribe(x => { ... CreateSubscription(@[ys(x).B().Subscribe(t2)]); ... }, ...)
    //
    //      That is, the QC sets up streams t1 and t2. The former is used to receive upstream events, which trigger the creation
    //      of the inner subscriptions (which can be subject to placement logic, or further decomposition). Additional hooks are
    //      inserted through expression rewriting in order to observe the completion of inner subscriptions, again by calling up
    //      to the QC to keep track of the active inner subscriptions (which have unique IDs). In other words, the QC is taking
    //      over the coordination of `SelectMany` by means of creating an execution plan where it is in control of all of the
    //      lifecycle events, allowing it to keep track of all resources (streams t1 and t2, subscriptions in QE1 and QE2, and
    //      the dynamically changing list of inner subscriptions). Note that the QC is itself a distributed service backed by
    //      a state store (e.g. in CosmosDB) the keeps track of the resources and the top-level mapping of the user's subscription
    //      to all of the underlying resources. Note that this corresponds to a persisted representation of the IDisposable
    //      algebra in classic Rx (e.g. CompositeDisposable). Finally note that even for query execution like the one sketched
    //      here, edges can play a role, e.g. in a QE receiving a ys(x).B().Subscribe(t2) subscription, where ys and t2 can be
    //      bound to edges (though t2 would typically have been rewritten into an observer that refers to an external stream).
    //

    /// <summary>
    /// Implementation of an input edge receiving events from another reactive service via an observable proxy.
    /// </summary>
    /// <typeparam name="T">The type of the events flowing over the edge.</typeparam>
    /// <remarks>
    /// See <see cref="OutputEdge{T}"/> to a general description of the mechanism, but consider an observable to be external to
    /// the current engine rather than an observer. For example, xs.Subscribe(o) where o is local but xs is external. Other than
    /// this, the mechanism is fairly similar. Events are received from an upstream engine, with sequence IDs attached. These are
    /// scheduled using an item processing task to be sent into the local subscription for processing. Upon checkpointing, a
    /// watermark is persisted in order to be able to request replay in case of a failover. If a checkpoint is successful, an
    /// acknowledgement is sent to the upstream engine (via AcknowledgeRange), which can prune its outgoing queue. In case of a
    /// failover, the cross-engine subscription is recovered, as well as the persisted watermark, and a call to Start is made to
    /// request replay from the last-known sequence number.
    /// </remarks>
    internal class InputEdge<T> : ReliableMultiSubjectBase<T>, IStatefulOperator
    {
        private readonly EdgeDescription _edge;
        private readonly IReactiveServiceResolver _serviceResolver;
        private readonly IReliableReactive _parentReactiveService;
        private IReliableReactive _externalReactiveService;
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood SubscriptionDispose
        private IReliableSubscription _externalSubscription;
        private IScheduler _scheduler;
#pragma warning restore CA2213
        private Uri _resubscribeUri;
        private IOperatorContext _context;
        private ProcessingTask _task;

        public InputEdge(EdgeDescription edge, IReactiveServiceResolver serviceResolver, IReliableReactive reactiveService)
        {
            _edge = edge;
            _serviceResolver = serviceResolver;
            _parentReactiveService = reactiveService;

            // REVIEW: Should we initialize in the base class to (-1) always?
            LowWatermark = -1;
        }

        public override Uri Id => _edge.InternalUri;

        public string Name => "rce:InputEdge";

        public Version Version => Versioning.v1;

        protected override void DisposeCore()
        {
        }

        public IEnumerable<ISubscription> Inputs => Enumerable.Empty<ISubscription>();

        public void Subscribe() { }

        public void SetContext(IOperatorContext context)
        {
            Debug.Assert(context != null, "Context is not allowed to be null.");
            _context = context;
            _scheduler = context.Scheduler.CreateChildScheduler();
        }

        public override void SaveState(IOperatorStateWriter writer, Version version)
        {
            writer.Write(_resubscribeUri);
        }

        public override void LoadState(IOperatorStateReader reader, Version version)
        {
            _resubscribeUri = reader.Read<Uri>();
        }

        public override void OnStateSaved()
        {
            // TODO: Implement if we need to add extra state here.
        }

        protected override Subscription CreateNewSubscription(IReliableObserver<T> observer)
        {
            if (SubscriptionCount > 0)
            {
                throw new InvalidOperationException("Input edge can only have one subscription.");
            }

            return base.CreateNewSubscription(observer);
        }

        protected override void SubscriptionStart(long sequenceId, Subscription subscription)
        {
            if (_externalSubscription != null)
            {
                throw new InvalidOperationException("Input edge subscription can be started only once.");
            }

            _externalSubscription = CreateExternalSubscription();
            _externalSubscription.Start(sequenceId);

            base.SubscriptionStart(sequenceId, subscription);

            _task = new ProcessingTask(this);
            _scheduler.Schedule(_task);
        }

        protected override void SubscriptionAcknowledgeRange(long sequenceId, Subscription subscription)
        {
            _externalSubscription.AcknowledgeRange(sequenceId);
            base.SubscriptionAcknowledgeRange(sequenceId, subscription);
        }

        protected override void SubscriptionDispose(ReliableMultiSubjectBase<T>.Subscription subscription)
        {
            _scheduler.Dispose();

            try
            {
                _externalSubscription.Dispose();
            }
            catch (EntityNotFoundException ex)
            {
                //
                // NB: We lack TryDispose capabilities right now. It's possible for the external subscription to
                //     get disposed more than once if the current engine fails over. We use unique IDs to harden
                //     against inadvertently disposing another unrelated subscription, so EntityNotFound is a
                //     very specific case we can handle here safely.
                //

                _context.TraceSource.InputEdge_ExternalSubscription_Dispose_Failed(_externalSubscription.ResubscribeUri, Id, ex);
            }

            base.SubscriptionDispose(subscription);
            Dispose();
        }

        protected override void OnNext(T item, long sequenceId)
        {
            if (LowWatermark < 0)
            {
                // Better way to do this? Should OnStarted be used here?
                LowWatermark = sequenceId;
            }

            base.OnNext(item, sequenceId);
            _scheduler.RecalculatePriority();
        }

        protected override void OnError(Exception error)
        {
            base.OnError(error);
            _scheduler.RecalculatePriority();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            _scheduler.RecalculatePriority();
        }

        private IReliableSubscription CreateExternalSubscription()
        {
            var externalUri = _resubscribeUri ?? _edge.ExternalUri;
            Debug.Assert(externalUri != null);

            //
            // NB: See remarks on OutputEdge<T> around synchronous versus asynchronous resolution.
            //

            if (!_serviceResolver.TryResolveReliable(externalUri, out _externalReactiveService))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Can't resolve external input '{0}'.", externalUri));
            }

            var extObservable = _externalReactiveService.GetObservable<T>(externalUri);
            var thisObserver = _parentReactiveService.GetObserver<T>(_edge.InternalUri);

            // TODO: Review the Uri used and consider if we need to pass any state.
            _edge.ExternalSubscriptionUri = new Uri("edge:/" + Guid.NewGuid().ToString());
            var subscription = extObservable.Subscribe(thisObserver, _edge.ExternalSubscriptionUri, state: null);
            _resubscribeUri = subscription.ResubscribeUri;
            return subscription;
        }

        private sealed class ProcessingTask : ISchedulerTask
        {
            /// <summary>
            /// Task priority is currently static.
            /// </summary>
            /// <remarks>
            /// We should consider to introduce policies/strategies how to change priority of a task,
            /// e.g. depending on the size of the input queue.
            /// </remarks>
            private const int QueueProcessingPriority = 2;
            private const int MaximumBatchSize = 8192;

            private readonly InputEdge<T> _parent;
            private readonly int _batchSize;

            public ProcessingTask(InputEdge<T> parent, int batchSize = 128)
            {
                if (batchSize is <= 0 or > MaximumBatchSize)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, "Batch size should be a positive number not exceeding '{0}'.", MaximumBatchSize),
                        nameof(batchSize));
                }

                _parent = parent;
                _batchSize = batchSize;
            }

            /// <summary>
            /// Gets task priority.
            /// </summary>
            public long Priority => QueueProcessingPriority;

            /// <summary>
            /// Gets a value indicating whether the task is runnable.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is runnable; otherwise, <c>false</c>.
            /// </value>
            public bool IsRunnable => _parent.ItemCount > 0;

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <param name="scheduler">The scheduler.</param>
            /// <returns>
            /// True if the task has been completed.
            /// </returns>
            public bool Execute(IScheduler scheduler)
            {
                _parent.NotifySubscriptions(_batchSize);
                return false;
            }

            /// <summary>
            /// Recalculates the priority of the task. The task can become runnable
            /// as the result of this operation.
            /// </summary>
            public void RecalculatePriority()
            {
            }
        }
    }
}
