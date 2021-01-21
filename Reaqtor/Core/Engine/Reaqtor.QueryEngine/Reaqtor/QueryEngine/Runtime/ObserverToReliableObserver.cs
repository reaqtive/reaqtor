// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    internal class ObserverToReliableObserver<T> : IObserver<T>, IOperator, ISubscription
    {
        private readonly IReliableObserver<T> _reliableObserver;
        private readonly ISubscription _asSubscription;
        private readonly IOperator _asOperator;

        public ObserverToReliableObserver(IReliableObserver<T> reliableObserver)
        {
            _reliableObserver = reliableObserver;
            _asSubscription = reliableObserver as ISubscription;

            if (_asSubscription == null)
            {
                _asOperator = reliableObserver as IOperator;
            }
        }

        #region IObserver<T>

        public void OnCompleted() => _reliableObserver.OnCompleted();

        public void OnError(Exception error) => _reliableObserver.OnError(error);

        public void OnNext(T value) => _reliableObserver.OnNext(value, 0);

        #endregion

        #region IOperator

        public IEnumerable<ISubscription> Inputs { get; private set; }

        public void Subscribe()
        {
            Inputs = OnSubscribe();
        }

        private IEnumerable<ISubscription> OnSubscribe()
        {
            if (_asSubscription != null)
            {
                return new[] { _asSubscription };
            }
            else if (_asOperator != null)
            {
                return _asOperator.Inputs;
            }

            return Array.Empty<ISubscription>();
        }

        public void SetContext(IOperatorContext context)
        {
            if (_asOperator != null)
            {
                _asOperator.SetContext(context);
            }
        }

        public void Start()
        {
            _asOperator?.Start();
        }

        #endregion

        #region ISubscription

        public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // TODO: if we ever switch to the SubscriptionVisitor
            //       model for disposal, we should remove this.
            if (_asSubscription != null)
            {
                _asSubscription.Dispose();
            }
            else if (_asOperator != null)
            {
                _asOperator.Dispose();
            }
        }

        #endregion
    }
}
