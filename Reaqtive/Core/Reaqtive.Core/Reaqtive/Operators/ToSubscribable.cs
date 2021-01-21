// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    /// <summary>
    /// Subscribable sequence encapsulating an observable sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements produced by the subscribable sequence.</typeparam>
    internal sealed class ToSubscribable<TSource> : SubscribableBase<TSource>
    {
        private readonly IObservable<TSource> _source;

        /// <summary>
        /// Creates a new subscribable wrapper around the specified observable sequence.
        /// </summary>
        /// <param name="source">Observable sequence to wrap.</param>
        public ToSubscribable(IObservable<TSource> source)
        {
            Debug.Assert(source != null);
            _source = source;
        }

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ContextSwitchOperator<ToSubscribable<TSource>, TSource>
        {
            private SingleAssignmentSubscription subscription;

            public _(ToSubscribable<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:ToSubscribable";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                var sub = base.OnSubscribe();

                // return a dummy subscription here, don't subscribe to the observable yet
                // we're not ready at the moment, we'll subscribe when we receive the OnStart notification
                subscription = new SingleAssignmentSubscription();

                return OnSubscribeCore(sub); // NB: We don't want OnSubscribe itself to be a lazy iterator.
            }

            private IEnumerable<ISubscription> OnSubscribeCore(IEnumerable<ISubscription> sub)
            {
                foreach (var s in sub)
                {
                    yield return s;
                }

                yield return subscription;
            }

            protected override void OnStart()
            {
                base.OnStart();

                // now we're ready to process messages, so just subscribe to the parent observable
                var disposable = Params._source.Subscribe(this);
                subscription.Subscription = new _Subscription(disposable);
            }

            public override void OnError(Exception error)
            {
                base.OnError(error);
                subscription.Dispose(); // only dispose the Rx subscription; otherwise, we'd kill the scheduler's work too
            }

            public override void OnCompleted()
            {
                base.OnCompleted();
                subscription.Dispose(); // only dispose the Rx subscription; otherwise, we'd kill the scheduler's work too
            }

            /// <summary>
            /// Encapsulates an Rx subscription (IDisposable)
            /// </summary>
            private sealed class _Subscription : ISubscription
            {
                private readonly IDisposable _disposable;

                public _Subscription(IDisposable disposable)
                {
                    Debug.Assert(disposable != null);
                    _disposable = disposable;
                }

                public void Accept(ISubscriptionVisitor visitor)
                {
                    // Do nothing here
                }

                public void Dispose()
                {
                    _disposable.Dispose();
                }
            }
        }
    }
}
