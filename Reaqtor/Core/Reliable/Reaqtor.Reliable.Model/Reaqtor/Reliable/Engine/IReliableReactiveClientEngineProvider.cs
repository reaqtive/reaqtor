// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Reliable.Client;
using System;
using System.Linq.Expressions;

namespace Reaqtor.Reliable.Engine
{
    public interface IReliableReactiveClientEngineProvider
    {
        #region Observer

        IReliableReactiveObserver<T> GetObserver<T>(Uri observerUri);

        #endregion

        #region Subscription

        void CreateSubscription(Uri subscriptionUri, Expression subscription, object state);

        void DeleteSubscription(Uri subscriptionUri);

        void StartSubscription(Uri subscriptionUri, long sequenceId);

        void AcknowledgeRange(Uri subscriptionUri, long sequenceId);

        Uri GetSubscriptionResubscribeUri(Uri subscriptionUri);

        #endregion

        #region Stream

        void CreateStream(Uri streamUri, Expression stream, object state);

        void DeleteStream(Uri streamUri);

        void CreateObserver(Uri streamUri);

        #endregion
    }
}
