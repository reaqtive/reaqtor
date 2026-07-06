// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Subscription cleanup operator. Will update the subscription state upon termination of the subscription to the given observable sequence.
    /// This operator should only be injected in the final position before a Subscribe call in order to defer cleanup until completion of the entire query.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the operator.</typeparam>
    internal class SubscriptionCleanup<T> : SubscribableBase<T>
    {
        /// <summary>
        /// Sequence to provide subscription cleanup for.
        /// </summary>
        private readonly ISubscribable<T> _source;

        /// <summary>
        /// Creates a subscription cleanup operator for subscriptions to the given source.
        /// </summary>
        /// <param name="source">Sequence to provide subscription cleanup for.</param>
        public SubscriptionCleanup(ISubscribable<T> source)
        {
            _source = source;
        }

        /// <summary>
        /// Subscribes the specified observer to the operator.
        /// </summary>
        /// <param name="observer">Observer to send notifications to.</param>
        /// <returns>Handle to the subscription.</returns>
        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return new _(this, observer);
        }

        /// <summary>
        /// Underlying operator implementation.
        /// </summary>
        private sealed class _ : UnaryOperator<SubscriptionCleanup<T>, T>, IObserver<T>
        {
#if UNUSED // NB: Kept to keep the code similar to what's in some Reactor deployments.
            /// <summary>
            /// Host context in which the operator is run.
            /// </summary>
            private HostOperatorContext _hostContext;

            /// <summary>
            /// Subscription processed by the operator.
            /// </summary>
            private Uri _subscriptionUri;
#endif

            /// <summary>
            /// The status of the subscription
            /// </summary>
            // private SubscriptionStatus _status = SubscriptionStatus.Active;

            /// <summary>
            /// Creates a new operator implementation object instance.
            /// </summary>
            /// <param name="parent">Subscription cleanup operator used for parameterization.</param>
            /// <param name="observer">Observer to send notifications to.</param>
            public _(SubscriptionCleanup<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            /// <summary>
            /// Sets the operator context to locate the host services to perform cleanup.
            /// </summary>
            /// <param name="context">Host operator context.</param>
            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                // Note to maintainers: if the following cast fails, the environment is incorrectly configured,
                // or this operator got reused outside its intended operation environment. For more information,
                // see ExpressionRewriteHelpers.GetExpressionWithSubscriptionCleanupHook where this operator
                // gets included in instrumented subscription expressions.
                //_hostContext = (HostOperatorContext)context;
                //_subscriptionUri = context.InstanceId;
            }

            /// <summary>
            /// Processes an OnCompleted message of the upstream producer.
            /// </summary>
            public void OnCompleted()
            {
                Output.OnCompleted();
                //_status = SubscriptionStatus.Completed;
                Dispose();
            }

            /// <summary>
            /// Processes an OnError message of the upstream producer.
            /// </summary>
            /// <param name="error">Error to process.</param>
            public void OnError(Exception error)
            {
                Output.OnError(error);
                //_status = SubscriptionStatus.Error;
                Dispose();
            }

            /// <summary>
            /// Processes an OnNext message of the upstream producer.
            /// </summary>
            /// <param name="value">Element to process.</param>
            public void OnNext(T value)
            {
                Output.OnNext(value);
            }

            /// <summary>
            /// Processes the subscription request by subscribing to the source for which subscription cleanup is performed.
            /// </summary>
            /// <returns>Subscriptions owned by the subscription to the current operator.</returns>
            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            /// <summary>
            /// Processes disposal operations, causing subscription cleanup.
            /// </summary>
            protected override void OnDispose()
            {
                // The current policy triggers cleanup operations upon "spontaneous" termination of the
                // subscription (i.e. through data flow of OnError and OnCompleted) as well as user-triggered
                // disposal of the subscription that trickles down to the QE.
                // This policy can be refined on an as-need basis, e.g. to log information about the nature
                // of the termination which can be useful in diagnostics. Right now, this coarse-grained
                // approach is followed, effectively equivalent to the use of the Finally operator in Rx.
                try
                {
                    base.OnDispose();
                }
                finally
                {
                    //Tracer.TraceSource.TraceInformation("Subscription {0} completed", _subscriptionUri.ToString());

                    // Fire & forget
                    //_hostContext.SubscriptionLocatorStore.UpdateStatusAsync(_subscriptionUri, _status);
                }
            }
        }
    }
}
