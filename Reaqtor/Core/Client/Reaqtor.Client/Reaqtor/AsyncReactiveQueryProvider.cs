// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Reactive processing query provider using a data operations object to perform service-side operations.
    /// </summary>
    public class AsyncReactiveQueryProvider : AsyncReactiveQueryProviderBase
    {
        #region Constructor & fields

        private readonly IReactiveClientServiceProvider _provider;

        /// <summary>
        /// Creates a new reactive processing query provider using the specified data operations object.
        /// </summary>
        /// <param name="provider">Data operations object to delegate operations to.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public AsyncReactiveQueryProvider(IReactiveClientServiceProvider provider, IReactiveExpressionServices expressionServices)
            : base(expressionServices)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        #endregion

        #region Observer operations

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer">Expression tree representation of an observer to get a publication observer for.</param>
        /// <param name="token">Token used to observe cancellation requests.</param>
        /// <returns>Observer to send notifications to.</returns>
        protected override Task<IAsyncReactiveObserver<T>> GetObserverAsyncCore<T>(IAsyncReactiveQbserver<T> observer, CancellationToken token)
        {
            var observerUri = GetObserverUriAsync(observer);
            return _provider.GetObserverAsync<T>(observerUri, token);
        }

        #endregion

        #region Subscription operations

        /// <summary>
        /// Creates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the subscription, or an exception.</returns>
        protected override Task CreateSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, object state, CancellationToken token)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out Uri uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from SubscribeAsync or GetSubscription?");
            }

            return _provider.CreateSubscriptionAsync(uri, subscription.Expression, state, token);
        }

        /// <summary>
        /// Deletes a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the subscription, or an exception.</returns>
        protected override Task DeleteSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, CancellationToken token)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (!TryGetUriFromKnownResource(subscription, out Uri uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from SubscribeAsync or GetSubscription?");
            }

            return _provider.DeleteSubscriptionAsync(uri, token);
        }

        #endregion

        #region Stream operations

        /// <summary>
        /// Creates a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the stream, or an exception.</returns>
        protected override Task CreateStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, object state, CancellationToken token)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!TryGetUriFromKnownResource(stream, out Uri uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from CreateAsync or GetStream?");
            }

            return _provider.CreateStreamAsync(uri, stream.Expression, state, token);
        }

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the stream, or an exception.</returns>
        protected override Task DeleteStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, CancellationToken token)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!TryGetUriFromKnownResource(stream, out Uri uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from CreateAsync or GetStream?");
            }

            return _provider.DeleteStreamAsync(uri, token);
        }

        #endregion

        #region Private implementation

        private static Uri GetObserverUriAsync<T>(IAsyncReactiveQbserver<T> observer)
        {
            if (!TryGetUriFromKnownResource(observer, out Uri uri))
            {
                throw new InvalidOperationException("Cannot notify an observer without a known URI-based identity. Did you get the observer through GetObserver? Did you use DefineObserverAsync to define the observer prior to usage?");
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
