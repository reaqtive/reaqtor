// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;

namespace Reaqtor.Reactive
{
    internal sealed class MultiSubjectObserverProxy<T> : Observer<T>
    {
        private readonly StableCompositeSubscription _inputs = new();
        private readonly Uri _uri;

        private IObserver<T> _observer;
        private ISubscription _observerAsSubscription;
        private IOperator _observerAsOperator;

        public MultiSubjectObserverProxy(Uri uri) => _uri = uri;

        #region IOperator

        protected override IEnumerable<ISubscription> OnSubscribe() => _inputs;

        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            _observer = context.ExecutionEnvironment.GetSubject<T, T>(_uri).CreateObserver();
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

        protected override void OnStart()
        {
            base.OnStart();

            _observerAsOperator?.Start();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            // TODO: if we ever switch to the SubscriptionVisitor
            //       model for disposal, we should remove this.

            _observerAsSubscription?.Dispose();
            _observerAsOperator?.Dispose();
        }

        #endregion

        #region IObserver<T>

        protected override void OnNextCore(T value) => _observer.OnNext(value);

        protected override void OnErrorCore(Exception error) => _observer.OnError(error);

        protected override void OnCompletedCore() => _observer.OnCompleted();

        #endregion
    }
}
