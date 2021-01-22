// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Client;
using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableQueryProvider : ReliableQueryProviderBase
    {
        private readonly IReliableReactiveClientEngineProvider _provider;

        public ReliableQueryProvider(IReliableReactiveClientEngineProvider provider, IReactiveExpressionServices expressionServices)
            : base(expressionServices)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        #region Observer

        protected override IReliableReactiveObserver<T> GetObserverCore<T>(IReliableQbserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var observerUri = GetObserverUri(observer);
            return _provider.GetObserver<T>(observerUri);
        }

        #endregion

        #region Subscription

        protected override void CreateSubscriptionCore(IReliableQubscription subscription, object state)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out var uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            _provider.CreateSubscription(uri, subscription.Expression, state);
        }

        protected override void DeleteSubscriptionCore(IReliableQubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out var uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            _provider.DeleteSubscription(uri);
        }

        protected override void StartSubscriptionCore(IReliableQubscription subscription, long sequenceId)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out var uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            _provider.StartSubscription(uri, sequenceId);
        }

        protected override void AcknowledgeRangeCore(IReliableQubscription subscription, long sequenceId)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out var uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            _provider.AcknowledgeRange(uri, sequenceId);
        }

        protected override Uri GetSubscriptionResubscribeUriCore(IReliableQubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out var uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            return _provider.GetSubscriptionResubscribeUri(uri);
        }

        #endregion

        #region Stream

        protected override void CreateStreamCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream, object state)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!TryGetUriFromKnownResource(stream, out var uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from Create or GetStream?");
            }

            _provider.CreateStream(uri, stream.Expression, state);
        }

        protected override void DeleteStreamCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!TryGetUriFromKnownResource(stream, out var uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from Create or GetStream?");
            }

            _provider.DeleteStream(uri);
        }

        protected override IReliableQbserver<TInput> CreateObserverCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!TryGetUriFromKnownResource(stream, out var uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from Create or GetStream?");
            }

            _provider.CreateObserver(uri);

            // TODO: Return a proxy object.
            return null;
        }

        #endregion

        #region Private implementation

        private static Uri GetObserverUri<T>(IReliableQbserver<T> observer)
        {
            if (!TryGetUriFromKnownResource(observer, out var uri))
            {
                throw new InvalidOperationException("Cannot notify an observer without a known URI-based identity. Did you get the observer through GetObserver? Did you use DefineObserver to define the observer prior to usage?");
            }

            return uri;
        }

        private static bool TryGetUriFromKnownResource(object obj, out Uri uri)
        {
            if (obj is IKnownResource known)
            {
                uri = known.Uri;
                return true;
            }

            uri = null;
            return false;
        }

        #endregion
    }
}
