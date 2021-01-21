// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Reaqtive;

using Reaqtor.Reactive;
using Reaqtor.Reliable;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Implementation of an output edge sending events from this engine to another reactive service via an observer proxy.
    /// </summary>
    /// <typeparam name="T">The type of the events flowing over the edge.</typeparam>
    /// <remarks>
    /// Support for edges in this engine implementation is limited (and partial in some aspects) though functional; this engine has most
    /// commonly been used in a "edgeless" environments where a query topology is managed through a central query coordinator. More
    /// specialized implementations have been built that double down on peer-to-peer inter-engine communication to support queries whose
    /// constituents (i.e. query operators) may be affinitized to certain machines (e.g. running ML operators on nodes where a big model
    /// has been deployed, or operators that deal with decryption of data which can only take place on certain highly secured nodes) and
    /// thus require a query to be split across nodes. In an edgeless environment, the query coordinator would split such a query upfront
    /// and allocate "pipes" (often backed by services such as Serice Bus or Event Hub) to facilitate communicaton across engines, so
    /// queries end up on engines in a fully bound form with sources (observables) and sinks (observers) that subscribe to or publish to
    /// external streams that act as the "edges" between engines. The drawback of that approach is resource management when queries shut
    /// down or when terminal events (OnError, OnCompleted) flow over such edges, because the query coordinator manages the lifetime of
    /// these resources. An engine with support for edges supports a peer-to-peer resolution mechanism where inter-engine commnication
    /// is governed by a pipe whose lifetime is controlled by the engines on both ends (either 1-to-1 or 1-to-many).
    ///
    /// This output edge implementation leverages the service resolver to locate an observer in a downstream engine and obtain a proxy to
    /// it. The observer is a "reliable qbserver", thus it speaks sequence numbers and expression trees, which enables for the peer-to-
    /// peer subscription to be serialized across a channel. Sequence numbers are introduced by the base class, which is a reliable
    /// subject that persists the events that have not yet been acknowledged by the downstream engine (upon its own checkpointing). If
    /// the downstream engine fails over, a replay request is sent to us in order to replay the missed events.
    ///
    /// The structure is roughly as follows:
    ///
    /// * Consider a query xs.Subscribe(o) where o is not found in the current engine, or no definition is found in a registry wired up
    ///   to the engine. The engine decides that o must be defined elsewhere and introduces an output edge (because o is in an output,
    ///   i.e. an observer, position). Let's call this edge e.
    /// * The output edge is really a subject, so e can be used in an observer position to create a local query in the current engine
    ///   that's equivalent to xs.Subscribe(e). This accounts for events making it into the edge. However, local queries don't speak
    ///   sequence IDs, so the "reliable reactive service" head on the engine leverages helpers like `ObserverToReliableObserver{T}` to
    ///   introduce sequence IDs.
    /// * Persistence of events received by the edge, including their sequence numbers, is taken care of by the base class of the edge,
    ///   i.e. `ReliableMultiSubjectBase{T}`. Checkpoints persist the events that have not yet been acknowledged by the downstream
    ///   external engine.
    /// * The edge itself has an identifier, let's call it e_id. The external downstream observer has an id as well, which was o (the
    ///   one we failed to locate, leading us down the path of building an edge). By using the `IReactiveServiceResolver` we obtained a
    ///   proxy to the external engine that knows o. We continue by creating a subscription of the form `e_id.Subscribe(o, -1)` and
    ///   send it to the downstream engine. It finds o locally (as the resolver told us) and resolves e_id back to us. This establishes
    ///   the edge. Note the use of sequence numbers for this reliable subscription; the downstream engine is subscribing to the edge
    ///   in our engine, and starts from whatever event is currently latest (-1).
    /// * If the downstream engine fails over, it will ask for replay using whatever latest sequence number it hsa persisted (which will
    ///   make its way into Start(long) on the base class's subscription object). If the downstream engine checkpoints, it will send an
    ///   acknowledgement to us (which ends up being a call to AcknowledgeRange(long) on the reliable subscription object).
    ///
    /// Also see EdgeDescription, InputEdge (for the opposite structure where an observable source is externally defined in an upstream
    /// engine), ReliableEdgeBinder, EdgeRewriter, and ReliableMultiSubjectBase{T}.
    ///
    /// Finally note that one of the known limitations of this mechanism is the synchronous nature of resolve calls. This has been dealt
    /// with by avoiding blocking due to I/O through mechanisms such as caching of neighboring engines' registries or even by having an
    /// "assist" from a query coordinator to prime resolvers on target engines with entries for artifacts known to be external (and that
    /// hence will see a resolve request). An effort to make resolve operations asynchronous is quite involved, because the async ends
    /// up permeating the query operator space, and thus intersects with scheduling and the ability to pause when creatng a checkpoint.
    /// We can reconsider this work (cf. prototypes that have proven feasibility of this) if/when we decide to support async Rx operators.
    /// </remarks>
    internal class OutputEdge<T> : ReliableMultiSubjectBase<T>, IStatefulOperator
    {
        private readonly EdgeDescription _edge;
        private readonly IReactiveServiceResolver _serviceResolver;
        private readonly IReliableReactive _parentReactiveService;
        private IReliableReactive _externalReactiveService;
        private IReliableSubscription _externalSubscription;
        private IHostedOperatorContext _context;

        public OutputEdge(EdgeDescription edge, IReactiveServiceResolver serviceResolver, IReliableReactive reactiveService)
        {
            _edge = edge;
            _serviceResolver = serviceResolver;
            _parentReactiveService = reactiveService;
        }

        public override Uri Id => _edge.InternalUri;

        public string Name => "rce:OutputEdge";

        public Version Version => Versioning.v1;

        protected override void DisposeCore()
        {
        }

        public IEnumerable<ISubscription> Inputs => Enumerable.Empty<ISubscription>();

        public void Subscribe() { }

        public void SetContext(IOperatorContext context)
        {
            Debug.Assert(context is IHostedOperatorContext);
            _context = (IHostedOperatorContext)context;
        }

        public override void Start()
        {
            base.Start();
            _externalSubscription = CreateExternalSubscription();
        }

        protected override Subscription CreateNewSubscription(IReliableObserver<T> observer)
        {
            if (SubscriptionCount > 0)
            {
                DropAllSubscriptions();
            }

            return base.CreateNewSubscription(observer);
        }

        protected override void SubscriptionDispose(ReliableMultiSubjectBase<T>.Subscription subscription)
        {
            base.SubscriptionDispose(subscription);
            DisposeInternalSubscription();
        }

        protected override void OnNext(T item, long sequenceId)
        {
            base.OnNext(item, sequenceId);
            NotifySubscriptions();
        }

        protected override void OnError(Exception error)
        {
            base.OnError(error);
            NotifySubscriptions();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            NotifySubscriptions();
        }

        private IReliableSubscription CreateExternalSubscription()
        {
            //
            // NB: Resolve is synchronous and the environment tries its best to make it non-blocking. In case there's blocking, pause times
            //     during checkpointing can get unbounded (which we don't want). We should consider going async here, which bubbles up through
            //     the Start code path (and the subscription visitors), all the way into the recovery and create subscription code paths.
            //     Because Start may also get called when an inner subscription is created in response to receiving an event, it also spreads
            //     async to observer methods. With upcoming support for ValueTask in .NET vNext, we could entertain this idea without
            //     having to worry too much about allocation costs that have prevented us from going down the Task path on the core event
            //     processing code path.
            //
            //     Going async has other benefits as well, including support for async predicates, selectors, etc. provided we can take care
            //     of "replay" of such outgoing calls (we need them to be repeatable for recovery purposes, so we either need to persist
            //     their results or pass in a sequence number so we can ask for a "replay"). In the context of resolution, it can also enable
            //     powerful dynamic binding, e.g. if an event carries a Uri of another stream to subscribe to:
            //
            //       streamsToSubscribeTo.SelectMany(uri => ctx.GetObservable<T>(uri)).Subscribe(o)
            //
            //     Here, the GetObservable<T>(Uri) call is passed a data-dependent Uri (obtained from an event), so we'd have to dynamically
            //     bind (and thus possibly externally resolve) the observable, which requires async in an OnNext context.
            //
            // NB: We've also worked around the synchronous resolver requirement for "anonymous streams" (used for P2P communication between
            //     engines) by encoding the target QE in the URI of such streams (so the resolver can extract the target QE easily).
            //

            if (!_serviceResolver.TryResolveReliable(_edge.ExternalUri, out _externalReactiveService))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Can't resolve external output '{0}'.", _edge.ExternalUri));
            }

            var extObserver = _externalReactiveService.GetObserver<T>(_edge.ExternalUri);
            var thisObservable = _parentReactiveService.GetObservable<T>(_edge.InternalUri);

            var subscription = thisObservable.Subscribe(extObserver, _edge.ExternalSubscriptionUri, state: null);
            return subscription;
        }

        private void DisposeInternalSubscription()
        {
            var internalSubscription = _context.ReactiveService.GetSubscription(_edge.InternalSubscriptionUri);
            internalSubscription.Dispose();
        }
    }
}
