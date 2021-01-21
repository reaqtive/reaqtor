// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Operator inserted during expression rewrites to clean up edges.
    /// </summary>
    /// <typeparam name="TResult">The type of the events flowing through the operator.</typeparam>
    internal sealed class EdgeCleanup<TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly EdgeDescription[] _edges;

        public EdgeCleanup(ISubscribable<TResult> source, EdgeDescription[] edges)
        {
            _source = source;
            _edges = edges;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer) => new Impl(this, observer);

        private sealed class Impl : StatefulUnaryOperator<EdgeCleanup<TResult>, TResult>, IObserver<TResult>
        {
            public Impl(EdgeCleanup<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rce:EdgeCleanup";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(TResult value)
            {
                Output.OnNext(value);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void OnDispose()
            {
                base.OnDispose();

                //
                // NB: The implementation of edge cleanup has been dropped in favor of IDependencyOperator and the garbage collection
                //     mechanism introduced in the QE. This should be revisited when we have "deep async" and can do proper cleanup
                //     right here, which may involve making outgoing async calls to peer QEs to clean up the "other side" of the edges.
                //
                //     We've kept hte operator here, both as a placeholder for future implementation (with async support), but also to
                //     make sure existing subscriptions that use edges (and thus have been rewritten to include a cleanup step) can
                //     continue to recover. It's merely a no-op now.
                //
                //     Note that while we could remove it for new deployments, we've decided to keep it, because all the wiring to
                //     insert cleanup is already there, but also because it's useful while debugging edges to see when they are
                //     logically getting dropped (rather than being collected possibly much later).
                //

                _ = Params._edges;

                // // TODO: Delete the externals created for this subscription.
                // foreach (var e in Params._edges)
                // {
                //     // TODO: delete "e"
                // }
            }
        }
    }
}
