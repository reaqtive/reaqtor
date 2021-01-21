// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable
{
    /// <summary>
    /// Subscribable-space implementation of the ZeroMQ based firehose
    /// </summary>
    /// <typeparam name="T">the type for the deserialized messages</typeparam>
    public class FirehoseSubscribable<T> : SubscribableBase<T>
    {
        /// <summary>
        /// The messaging topic.
        /// </summary>
        private readonly Uri _topic;

        /// <summary>
        /// The only observer of this observable
        /// </summary>
        private IObserver<T> _theObserver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirehoseObservable{T}"/> class.
        /// </summary>
        /// <param name="messageTopic">The message topic.</param>
        public FirehoseSubscribable(Uri messageTopic)
        {
            _topic = messageTopic ?? throw new ArgumentNullException(nameof(messageTopic));
        }

        /// <summary>
        /// Subscribes the specified observer to the firehose
        /// </summary>
        /// <param name="observer">the observer</param>
        /// <returns>an object representing a subscription</returns>
        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            if (_theObserver != null)
            {
                throw new InvalidOperationException(
                    "There should be only one observer attached to the Firehose Observable");
            }

            _theObserver = observer;
            return new _(this, observer);
        }

        /// <summary>
        /// the underlying <see cref="ISubcription"/> implementation for the firehose
        /// </summary>
        private sealed class _ : Operator<Uri, T>, IUnloadableOperator
        {
            /// <summary>
            /// late bound subscription to the underlying ZeroMQ based firehose
            /// </summary>
            private readonly SingleAssignmentDisposable _lateBoundSubscription = new();

            /// <summary>
            /// the context for the current subscription
            /// </summary>
            private IOperatorContext _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="_"/> class.
            /// </summary>
            /// <param name="firehoseSubscribable">the parent subscribable</param>
            /// <param name="observer">the observer to subscribe to the firehose</param>
            public _(FirehoseSubscribable<T> firehoseSubscribable, IObserver<T> observer)
                : base(firehoseSubscribable._topic, observer)
            {
            }

            /// <summary>
            /// Sets the context on the subscription
            /// </summary>
            /// <param name="context">the context</param>
            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);
                _context = context;
            }

            /// <summary>
            /// Called when the QE is unloaded
            /// </summary>
            public void Unload()
            {
                _lateBoundSubscription.Dispose();
            }

            /// <summary>
            /// called to start the subscription
            /// </summary>
            protected override void OnStart()
            {
                base.OnStart();

                var wrappedSubscription = FirehoseSubscriptionManager.Instance.Subscribe(Params.ToCanonicalString(), _context, Output);
                _lateBoundSubscription.Disposable = wrappedSubscription;
            }

            /// <summary>
            /// called when subscription is getting disposed
            /// </summary>
            protected override void OnDispose()
            {
                base.OnDispose();

                _lateBoundSubscription.Dispose();
            }
        }
    }
}
