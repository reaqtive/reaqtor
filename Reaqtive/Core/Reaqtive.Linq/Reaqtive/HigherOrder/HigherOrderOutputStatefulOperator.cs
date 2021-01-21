// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Base class for stateful operators that produce a higher order sequence.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameters passed to the operator.</typeparam>
    /// <typeparam name="TResult">Type of the elements produced by the operator in the result sequence. This type should derive from ISubscribable&lt;T&gt; (for some value of T) but this constraint is not enforced at this layer of the class hierarchy.</typeparam>
    internal abstract class HigherOrderOutputStatefulOperatorBase<TParam, TResult> : StatefulUnaryOperator<TParam, TResult>
    {
        #region Documentation

        /*
         * Concepts used in this class
         * ===========================
         *
         *   Tollbooth
         *   ---------
         *
         *   A stream of type ISubject<bool> used to send AddRef (true) and Release (false) messages to the higher order
         *   operator for it to keep track of the number of active subscriptions the operator feeds into. The base class
         *   sets up a subscription to the toolbooth to call the AddRef and Release methods on this class in order to
         *   maintain the _refCount field. In addition to subscriptions to inner streams, the operator also needs to keep
         *   the source subscription alive in order to produce new inner streams. This gets tracked separately via the
         *   _parentDisposed field.
         *
         *   Collector
         *   ---------
         *
         *   A stream of type ISubject<Uri> to process cleanup requests for inner streams which is done by the Collect
         *   method in this class. Nested streams will announce their tunnel Uri when they see their subscription count
         *   drop to 0. This is possible thanks to the concept of sealing inner streams to protect against late arrivals
         *   of subscription requests (see OnNextInner).
         *
         *   Tunnel
         *   ------
         *
         *   Dual concept to a bridge used in higher-order input operators. Where bridges represent subscriptions to inner
         *   sequences received from the higher-order input, tunnels represent streams to inner sequences produced on the
         *   higher-order output.
         */

        #endregion

        #region Fields

        /// <summary>
        /// The operator context, used to obtain access to the environment in which the operator is running.
        /// </summary>
        protected IOperatorContext _context;

        /// <summary>
        /// The number of active inner subscriptions, maintained via the tollbooth.
        /// </summary>
        /// <example>
        /// In xs.GroupBy(x => x % 2).Merge() there will be at most two active inner subscriptions, so the count
        /// can vary from 0 to 2. In xs.GroupBy(x => x % 2).SelectMany(g => g.Take(1)) the count can drop back
        /// to 0 when both groups have seen one element.
        /// </example>
        private int _refCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new hgher order output operator instance using the given parameters and the
        /// observer to report downstream notifications to.
        /// </summary>
        /// <param name="parent">Parameters used by the operator.</param>
        /// <param name="observer">Observer receiving the operator's output.</param>
        protected HigherOrderOutputStatefulOperatorBase(TParam parent, IObserver<TResult> observer)
            : base(parent, observer)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the operator context, used to obtain access to the environment in which the operator is running.
        /// </summary>
        protected IOperatorContext Context => _context;

        /// <summary>
        /// Indicates whether the operator has stopped processing inputs.
        /// </summary>
        protected bool HasStopped => Volatile.Read(ref _refCount) == 0 && OutputSubscriptionDisposed;

        /// <summary>
        /// Indicates whether the output subscription has been disposed, which can be used to avoid creating new inner streams.
        /// </summary>
        /// <example>
        /// In xs.GroupBy(x => x % 2).Take(1).Merge() the subscription to the higher order output will be disposed
        /// upon receiving the first group.
        /// </example>
        protected bool OutputSubscriptionDisposed { get; private set; }

        /// <summary>
        /// Gets the tollbooth observer for use by the nested SimpleRefCountSubject in case we run without an environment.
        /// </summary>
        protected IObserver<bool> Tollbooth { get; private set; } // TODO: can we eliminate this property?

        /// <summary>
        /// Gets the URI to the tollbooth stream.
        /// </summary>
        protected Uri TollboothUri { get; private set; }

        /// <summary>
        /// Gets the URI to the collector stream.
        /// </summary>
        protected Uri CollectorUri { get; private set; }

        /// <summary>
        /// Gets the list of known artifacts the operator depends on.
        /// </summary>
        public IEnumerable<Uri> Dependencies
        {
            get
            {
                yield return TollboothUri;
                yield return CollectorUri;

                foreach (var uri in DependenciesCore)
                {
                    yield return uri;
                }
            }
        }

        /// <summary>
        /// Gets the list of known artifacts (the subclass of) the operator depends on.
        /// </summary>
        protected abstract IEnumerable<Uri> DependenciesCore
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the context in which the operator operates.
        /// </summary>
        /// <param name="context">Context in which the operator operates.</param>
        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            _context = context;
        }

        /// <summary>
        /// Tries to get the underlying higher-order environment, if supported by the hosting infrastructure.
        /// </summary>
        /// <param name="env">The higher-order environment.</param>
        /// <returns>true if supported; otherwise, false.</returns>
        protected bool TryGetHigherOrderExecutionEnvironment(out IHigherOrderExecutionEnvironment env)
        {
            env = _context?.ExecutionEnvironment as IHigherOrderExecutionEnvironment;
            return env != null;
        }

        /// <summary>
        /// Ensures that the resource management mechanisms are set up, including the tollbooth and the collector.
        /// </summary>
        /// <param name="recovery">Indicates whether the operator was recovered or started afresh.</param>
        protected void EnsureResourceManagement(bool recovery)
        {
            var tollbooth = default(IMultiSubject<bool, bool>);
            var collector = default(IMultiSubject<Uri, Uri>);

            if (!recovery)
            {
                var guid = Guid.NewGuid().ToString("D");

                TollboothUri = new Uri("rx://tollbooth/" + guid);
                CollectorUri = new Uri("rx://collector/" + guid);

                StateChanged = true;

                if (TryGetHigherOrderExecutionEnvironment(out var env))
                {
                    // Set up the toll booth responsible to monitor subscription flow on tunnels to
                    // keep a refcount of the number of outstanding subscriptions.
                    env.CreateSimpleSubject<bool>(TollboothUri, _context);

                    // Set up the collector responsible to monitor deletions of tunnels in order to
                    // clean up operator state.
                    env.CreateSimpleSubject<Uri>(CollectorUri, _context);
                }
                else
                {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (No need to dispose toolbooth. SimplySubject<T>.Dispose is trivial. The object just gets GC'ed when the operator goes away.)

                    tollbooth = new SimpleSubject<bool>();

#pragma warning restore CA2000
#pragma warning restore IDE0079
                }
            }

            if (_context.ExecutionEnvironment != null)
            {
                if (TollboothUri != null)
                {
                    tollbooth = _context.ExecutionEnvironment.GetSubject<bool, bool>(TollboothUri);
                }

                if (CollectorUri != null)
                {
                    collector = _context.ExecutionEnvironment.GetSubject<Uri, Uri>(CollectorUri);
                }
            }

            if (tollbooth != null)
            {
                var sub = tollbooth.Subscribe(new RefCountManager(this));
                SubscriptionInitializeVisitor.Initialize(sub, _context);

                Tollbooth = tollbooth.CreateObserver();
            }

            if (collector != null)
            {
                var sub = collector.Subscribe(new Collector(this));
                SubscriptionInitializeVisitor.Initialize(sub, _context);
            }
        }

        /// <summary>
        /// Called when the operator has reached a stopped state, i.e. the reference count for subscriptions
        /// to inner streams has dropped to zero and the subscription to the operator's output has been
        /// disposed.
        /// </summary>
        protected abstract void ReleaseCore();

        /// <summary>
        /// Collects an inner stream with the specified tunnel URI.
        /// </summary>
        /// <param name="innerUri">URI of the inner stream to collect.</param>
        protected abstract void Collect(Uri innerUri);

        /// <summary>
        /// Loads the state of the operator.
        /// </summary>
        /// <param name="reader">Reader to read operator state from.</param>
        protected override void LoadStateCore(IOperatorStateReader reader)
        {
            base.LoadStateCore(reader);

            OutputSubscriptionDisposed = reader.Read<bool>();
            CollectorUri = reader.Read<Uri>();
            TollboothUri = reader.Read<Uri>();
            _refCount = reader.Read<int>();
        }

        /// <summary>
        /// Saves the state of the operator.
        /// </summary>
        /// <param name="writer">Writer to write operator state to.</param>
        protected override void SaveStateCore(IOperatorStateWriter writer)
        {
            base.SaveStateCore(writer);

            writer.Write<bool>(OutputSubscriptionDisposed);
            writer.Write<Uri>(CollectorUri);
            writer.Write<Uri>(TollboothUri);
            writer.Write<int>(_refCount);
        }

        /// <summary>
        /// Tears down the tollbooth stream and the collector stream.
        /// </summary>
        private void TearDownResourceManagement()
        {
            if (TryGetHigherOrderExecutionEnvironment(out var env))
            {
                if (TollboothUri != null)
                {
                    env.DeleteSubject<bool>(TollboothUri, _context);
                    TollboothUri = null;

                    StateChanged = true;
                }

                if (CollectorUri != null)
                {
                    env.DeleteSubject<Uri>(CollectorUri, _context);
                    CollectorUri = null;

                    StateChanged = true;
                }
            }
        }

        /// <summary>
        /// Called when the subscription to operator's output has been disposed.
        /// </summary>
        /// <returns>true if the reference count for subscriptions to inner streams is zero; otherwise, false.</returns>
        private bool OnOutputSubscriptionDisposed()
        {
            if (!OutputSubscriptionDisposed)
            {
                OutputSubscriptionDisposed = true;
                StateChanged = true;
            }

            return Volatile.Read(ref _refCount) == 0;
        }

        /// <summary>
        /// Increments the reference count for subscriptions to inner streams.
        /// </summary>
        private void AddRef()
        {
            Interlocked.Increment(ref _refCount);
            StateChanged = true;
        }

        /// <summary>
        /// Decrements the reference count for subscriptions to inner streams.
        /// </summary>
        private void Release()
        {
            var res = Interlocked.Decrement(ref _refCount);
            StateChanged = true;

            if (res == 0 && OutputSubscriptionDisposed)
            {
                ReleaseCore();
                Dispose(); // TODO: needed?
            }
        }

        #endregion

        #region Types

        /// <summary>
        /// Ref count manager subscribed to the tollbooth stream to observe AddRef/Release requests.
        /// </summary>
        private sealed class RefCountManager : TinyObserver<bool>
        {
            private readonly HigherOrderOutputStatefulOperatorBase<TParam, TResult> _parent;

            public RefCountManager(HigherOrderOutputStatefulOperatorBase<TParam, TResult> parent)
            {
                _parent = parent;
            }

            protected override void OnNextCore(bool value)
            {
                if (value)
                {
                    _parent.AddRef();
                }
                else
                {
                    _parent.Release();
                }
            }
        }

        /// <summary>
        /// Collector subscribed to the collector stream to observe stream collection requests.
        /// </summary>
        private sealed class Collector : TinyObserver<Uri>
        {
            private readonly HigherOrderOutputStatefulOperatorBase<TParam, TResult> _parent;

            public Collector(HigherOrderOutputStatefulOperatorBase<TParam, TResult> parent)
            {
                _parent = parent;
            }

            protected override void OnNextCore(Uri value)
            {
                _parent.Collect(value);
            }
        }

        // TODO: this is really the equivalent of RefCountDisposable in Rx, but isn't quite as compositional

        /// <summary>
        /// Reference-counted subscription used to clean-up the source subscription after all subscriptions to the
        /// inner streams as well as the subscription to the higher-order output sequence have been disposed.
        /// </summary>
        protected class RefCountSubscription : ISubscription
        {
            private readonly HigherOrderOutputStatefulOperatorBase<TParam, TResult> _parent;
            private readonly SingleAssignmentSubscription _inner;

            public RefCountSubscription(HigherOrderOutputStatefulOperatorBase<TParam, TResult> parent)
            {
                _parent = parent;
                _inner = new SingleAssignmentSubscription();
            }

            public ISubscription Subscription
            {
                set => _inner.Subscription = value;
            }

            public bool IsDisposed => _inner.IsDisposed;

            public void Accept(ISubscriptionVisitor visitor)
            {
                _inner.Accept(visitor);
            }

            public void Dispose()
            {
                if (_parent.OnOutputSubscriptionDisposed())
                {
                    _inner.Dispose();
                    _parent.TearDownResourceManagement();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Base class for stateful operators that produce a higher order sequence with inner sequences that get parameterized by <typeparamref name="TInnerArgs"/>.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameters passed to the operator.</typeparam>
    /// <typeparam name="TSource">Type of the elements received by the operator.</typeparam>
    /// <typeparam name="TElement">Type of the elements in the inner sequences produced by the operator.</typeparam>
    /// <typeparam name="TResult">Type of the elements produced by the operator in the result sequence. This type should derive from ISubscribable&lt;TElement&gt;.</typeparam>
    /// <typeparam name="TInnerArgs">Type of the arguments passed to the inner sequences created by the operator.</typeparam>
    internal abstract class HigherOrderOutputStatefulOperator<TParam, TSource, TElement, TResult, TInnerArgs> : HigherOrderOutputStatefulOperatorBase<TParam, TResult>, IObserver<TSource>, IDependencyOperator
        where TResult : ISubscribable<TElement>
    {
        #region Constructors

        /// <summary>
        /// Creates a new hgher order output operator instance using the given parameters and the
        /// observer to report downstream notifications to.
        /// </summary>
        /// <param name="parent">Parameters used by the operator.</param>
        /// <param name="observer">Observer receiving the operator's output.</param>
        protected HigherOrderOutputStatefulOperator(TParam parent, IObserver<TResult> observer)
            : base(parent, observer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes an OnCompleted message on the input.
        /// </summary>
        public abstract void OnCompleted();

        /// <summary>
        /// Processes an OnError message on the input.
        /// </summary>
        /// <param name="error">The error received from the source sequence.</param>
        public abstract void OnError(Exception error);

        /// <summary>
        /// Processes an OnNext message on the input.
        /// </summary>
        /// <param name="value">The value received from the source sequence.</param>
        public abstract void OnNext(TSource value);

        /// <summary>
        /// Gets the prefix to use for the URIs of inner streams created by the operator.
        /// </summary>
        protected abstract string InnerStreamPrefix
        {
            get;
        }

        /// <summary>
        /// Creates a new inner stream with the specified arguments.
        /// </summary>
        /// <param name="args">Arguments for creation of the new inner stream.</param>
        /// <param name="innerStreamUri">Tunnel URI of the newly created stream.</param>
        /// <returns>The newly created subject for the inner stream.</returns>
        protected virtual IMultiSubject<TElement, TElement> CreateInner(TInnerArgs args, out Uri innerStreamUri)
        {
            var innerStreamId = InnerStreamPrefix + Guid.NewGuid().ToString("D");
            innerStreamUri = new Uri(innerStreamId);

            if (TryGetHigherOrderExecutionEnvironment(out var env))
            {
                //
                // TODO: Right now, streams will get deleted when the last subscriber to them unsubscribes. This
                //       is made possible by "sealing" the subject after it's sent out through OnNextWindow. By
                //       using this restriction of requiring synchronous subscriptions during the outgoing OnNext
                //       call, we get away with not having to perform a deeper reachability analysis, e.g.
                //
                //         xs.Window(t, s).SelectMany(w => w.DelaySubscription(u))
                //
                //       If u is larger than t, the lifetime of the subject represented by w needs to exceed the
                //       terminal notifications in the subject. Effectively, we need to track the "potential" of
                //       the w object lingering around, which can eventually - at times unknown - lead to creation
                //       of a subscription to the subject. One option to tackle this is by adding a finalizer to w
                //       so that we can track its eventual disappearance. The pickle is to make that work after a
                //       recovery has taken place, where persisted quoted expression representations may result in
                //       multiple new instances (not to mention the wiring required).
                //
                //       So, in the interest of time and to get things running for traditional uses of Window,
                //       we will "seal" a subject as soon as we're past the OnNext stage. This does work well for
                //       cases like:
                //
                //         xs.Window(t, s).SelectMany(w => w.Sum())
                //
                //       or even when operators like SkipUntil(u) are used, given their nature of immediately
                //       setting up a subscription. In general, all operators that store the subject emitted by
                //       the Window operator are potentially problematic, e.g. CombineLatest, etc.
                //
                //       Notice that we may decide to keep this behavior, given that a common mistake in the Rx
                //       community is to cause time gaps in queries by not obeying to the synchronous subscription
                //       requirement during the OnNext call producing the inner sequence/stream.
                //

                return env.CreateRefCountSubject<TElement>(innerStreamUri, TollboothUri, CollectorUri, _context);
            }
            else
            {
                return new SimpleRefCountSubject(Tollbooth);
            }
        }

        /// <summary>
        /// Tries to get the subject for an inner stream with the specified tunnel URI.
        /// </summary>
        /// <param name="innerStreamUri">Tunnel URI of the stream to obtain the subject for.</param>
        /// <param name="inner">Subject for the inner stream, if found.</param>
        /// <returns>true if the stream was found; otherwise, false.</returns>
        protected bool TryGetInner(Uri innerStreamUri, out IMultiSubject<TElement, TElement> inner)
        {
            if (HasStopped || innerStreamUri == null)
            {
                inner = null;
                return false;
            }

            inner = Context.ExecutionEnvironment.GetSubject<TElement, TElement>(innerStreamUri);
            return true;
        }

        /// <summary>
        /// Sends the specified inner stream to the downstream observer.
        /// </summary>
        /// <param name="inner">Subject for the inner stream to sen to the downstream observer.</param>
        /// <param name="innerStreamUri">Tunnel URI of the stream; used for quotation.</param>
        protected void OnNextInner(IMultiSubject<TElement, TElement> inner, Uri innerStreamUri)
        {
            var quotedInner = Quote(inner, innerStreamUri);

            Output.OnNext(quotedInner);

            // To work around space leaks we'll enforce a synchronous subscription requirement during OnNext for now.
            // More information in the CreateWindow method.
            if (inner is ISealable seal)
            {
                seal.Seal();
            }
        }

        /// <summary>
        /// Quotes the specified subject for an inner stream using the specified tunnel URI.
        /// </summary>
        /// <param name="inner">Subject for the inner stream to sen to the downstream observer.</param>
        /// <param name="innerStreamUri">Tunnel URI of the stream; used for quotation.</param>
        /// <returns>Quoted representation of the inner stream.</returns>
        protected abstract TResult Quote(IMultiSubject<TElement, TElement> inner, Uri innerStreamUri);

        #endregion

        #region Types

        /// <summary>
        /// Simple implementation of a subject that does subscription reference counting via a tollbooth
        /// for use by higher order operators run without a reactive environment.
        /// </summary>
        private sealed class SimpleRefCountSubject : SimpleSubject<TElement>
        {
            private readonly IObserver<bool> _refCount;

            public SimpleRefCountSubject(IObserver<bool> refCount)
            {
                _refCount = refCount;
            }

            public override ISubscription Subscribe(IObserver<TElement> observer)
            {
                var res = base.Subscribe(observer);
                return new Subscription(res, this);
            }

            private sealed class Subscription : ISubscription, IOperator
            {
                private readonly ISubscription _inner;
                private readonly SimpleRefCountSubject _parent;

                public Subscription(ISubscription inner, SimpleRefCountSubject parent)
                {
                    _inner = inner;
                    _parent = parent;
                }

                public void Accept(ISubscriptionVisitor visitor)
                {
                    _inner.Accept(visitor);
                    visitor.Visit(this);
                }

                public void Dispose()
                {
                    _inner.Dispose();
                    _parent._refCount.OnNext(false);
                }

                public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

                public void Subscribe()
                {
                }

                public void SetContext(IOperatorContext context)
                {
                }

                public void Start()
                {
                    _parent._refCount.OnNext(true);
                }
            }
        }

        /// <summary>
        /// Class to keep track of the URI of an inner stream and an observer proxy to feed data into it.
        /// </summary>
        protected class Entry
        {
            public Uri Uri;
            public IObserver<TElement> Observer;
        }

        #endregion
    }

    /// <summary>
    /// Base class for stateful operators that produce a higher order sequence with inner sequences that are not parameterized.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameters passed to the operator.</typeparam>
    /// <typeparam name="TSource">Type of the elements received by the operator.</typeparam>
    internal abstract class HigherOrderOutputStatefulOperator<TParam, TSource> : HigherOrderOutputStatefulOperator<TParam, TSource, TSource, ISubscribable<TSource>, object>
    {
        #region Constructors

        /// <summary>
        /// Creates a new hgher order output operator instance using the given parameters and the
        /// observer to report downstream notifications to.
        /// </summary>
        /// <param name="parent">Parameters used by the operator.</param>
        /// <param name="observer">Observer receiving the operator's output.</param>
        protected HigherOrderOutputStatefulOperator(TParam parent, IObserver<ISubscribable<TSource>> observer)
            : base(parent, observer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Quotes the specified subject for an inner stream using the specified tunnel URI.
        /// </summary>
        /// <param name="inner">Subject for the inner stream to sen to the downstream observer.</param>
        /// <param name="innerStreamUri">Tunnel URI of the stream; used for quotation.</param>
        /// <returns>Quoted representation of the inner stream.</returns>
        protected override ISubscribable<TSource> Quote(IMultiSubject<TSource, TSource> inner, Uri innerStreamUri)
        {
            if (TryGetHigherOrderExecutionEnvironment(out var env))
            {
                return env.Quote(inner, innerStreamUri);
            }

            return inner;
        }

        /// <summary>
        /// Creates a new inner stream.
        /// </summary>
        /// <param name="innerStreamUri">Tunnel URI of the newly created stream.</param>
        /// <returns>The newly created subject for the inner stream.</returns>
        protected IMultiSubject<TSource, TSource> CreateInner(out Uri innerStreamUri)
        {
            return base.CreateInner(null, out innerStreamUri);
        }

        #endregion
    }
}
