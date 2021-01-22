// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Reaqtive;

namespace Reaqtor.Reliable
{
    public sealed class ReliableMultiSubjectProxy<TInput, TOutput> : IReliableMultiSubject<TInput, TOutput>
    {
        private readonly Uri _uri;

        public ReliableMultiSubjectProxy(Uri uri) => _uri = uri ?? throw new ArgumentNullException(nameof(uri));

        public IReliableObserver<TInput> CreateObserver() => new ObserverProxy(_uri);

        public IReliableSubscription Subscribe(IReliableObserver<TOutput> observer) => new SubscriptionProxy(_uri, observer);

        public void Dispose() { }

        private sealed class ObserverProxy : IReliableObserver<TInput>, IOperator, ISubscription
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2213 // Not disposing `_inputs`; instead, we dispose the underlying resources.

            private readonly Uri _uri;
            private readonly StableCompositeSubscription _inputs = new();

            private IReliableObserver<TInput> _observer;
            private ISubscription _observerAsSubscription;
            private IOperator _observerAsOperator;

            public ObserverProxy(Uri uri) => _uri = uri;

            #region IOperator

            public IEnumerable<ISubscription> Inputs => _inputs;

            public void Subscribe() { }

            public void SetContext(IOperatorContext context)
            {
                if (context.ExecutionEnvironment is not IReliableExecutionEnvironment reliableExecutionEnvironment)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Subject with URI '{0}' does not have a valid instance of type '{1}'.", _uri, typeof(IReliableMultiSubject<TInput, TOutput>)));
                }

                _observer = reliableExecutionEnvironment.GetReliableSubject<TInput, TOutput>(_uri).CreateObserver();
                _observerAsSubscription = _observer as ISubscription;

                if (_observerAsSubscription != null)
                {
                    _inputs.Add(_observerAsSubscription);
                }
                else if ((_observerAsOperator = _observer as IOperator) != null)
                {
                    _inputs.AddRange(_observerAsOperator.Inputs);
                    _observerAsOperator.SetContext(context);
                }
            }

            public void Start() => _observerAsOperator?.Start();

            #endregion

            #region ISubscription

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

            #endregion

            #region IReliableObserver<T>

            public Uri ResubscribeUri => _observer.ResubscribeUri;

            public void OnStarted() => _observer.OnStarted();

            public void OnNext(TInput item, long sequenceId) => _observer.OnNext(item, sequenceId);

            public void OnError(Exception error) => _observer.OnError(error);

            public void OnCompleted() => _observer.OnCompleted();

            #endregion

            #region IDisposable

            public void Dispose()
            {
                // TODO: if we ever switch to the SubscriptionVisitor
                //       model for disposal, we should remove this.
                _observerAsSubscription?.Dispose();
                _observerAsOperator?.Dispose();
            }

            #endregion

#pragma warning restore CA2213
#pragma warning restore IDE0079
        }

        private sealed class SubscriptionProxy : ReliableSubscriptionBase
        {
            private readonly Uri _uri;
            private readonly IReliableObserver<TOutput> _observer;
            private readonly SingleAssignmentSubscription _subscription = new();
            private readonly IEnumerable<ISubscription> _inputs;

            private IReliableSubscription _reliableSubscription;

            public SubscriptionProxy(Uri uri, IReliableObserver<TOutput> observer)
            {
                _uri = uri;
                _observer = observer;
                _inputs = new[] { _subscription };
            }

            #region IOperator

            public override IEnumerable<ISubscription> Inputs => _inputs;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                if (context.ExecutionEnvironment is not IReliableExecutionEnvironment reliableExecutionEnvironment)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Subject with URI '{0}' does not have a valid instance of type '{1}'.", _uri, typeof(IReliableMultiSubject<TInput, TOutput>)));
                }

                _reliableSubscription = reliableExecutionEnvironment.GetReliableSubject<TInput, TOutput>(_uri).Subscribe(_observer);
                _subscription.Subscription = _reliableSubscription;
            }

            #endregion

            #region ReliableSubscriptionBase

            // TODO: we should have a visitor for IReliableSubscription
            // that hits all of these methods via the `Accept` method.

            public override Uri ResubscribeUri => _reliableSubscription.ResubscribeUri;

            public override void Start(long sequenceId)
            {
                // TODO: if we ever switch to the SubscriptionVisitor
                //       model for this `Start`, we should remove this.
                _reliableSubscription.Start(sequenceId);
            }

            public override void AcknowledgeRange(long sequenceId)
            {
                // TODO: if we ever switch to the SubscriptionVisitor
                //       model for `AcknowledgeRange`, we should remove this.
                _reliableSubscription.AcknowledgeRange(sequenceId);
            }

            public override void DisposeCore()
            {
                // TODO: if we ever switch to the SubscriptionVisitor
                //       model for `Dispose`, we should remove this.
                _reliableSubscription.Dispose();
            }

            #endregion
        }
    }
}
