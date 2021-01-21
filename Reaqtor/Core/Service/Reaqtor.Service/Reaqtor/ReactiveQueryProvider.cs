// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Diagnostics;

namespace Reaqtor
{
    /// <summary>
    /// Reactive processing query provider using a data operations object to perform service-side operations.
    /// </summary>
    public class ReactiveQueryProvider : ReactiveQueryProviderBase
    {
        #region Constructor & fields

        private readonly IReactiveClientEngineProvider _provider;

        /// <summary>
        /// Creates a new reactive processing query provider using the specified data operations object.
        /// </summary>
        /// <param name="provider">Data operations object to delegate operations to.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public ReactiveQueryProvider(IReactiveClientEngineProvider provider, IReactiveExpressionServices expressionServices)
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
        /// <returns>Observer to send notifications to.</returns>
        protected override IReactiveObserver<T> GetObserverCore<T>(IReactiveQbserver<T> observer)
        {
            var observerUri = GetObserverUri(observer);
            return _provider.GetObserver<T>(observerUri);
        }

        #endregion

        #region Subscription operations

        /// <summary>
        /// Creates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void CreateSubscriptionCore(IReactiveQubscription subscription, object state)
        {
            Debug.Assert(subscription != null);

            var known = TryGetUriFromKnownResource(subscription, out Uri uri);
            Debug.Assert(known);

            _provider.CreateSubscription(uri, subscription.Expression, state);
        }

        /// <summary>
        /// Deletes a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to delete.</param>
        protected override void DeleteSubscriptionCore(IReactiveQubscription subscription)
        {
            Debug.Assert(subscription != null);

            if (!TryGetUriFromKnownResource(subscription, out Uri uri))
            {
                throw new InvalidOperationException("Unknown subscription object. Could not find a URI identity for the specified subscription object. Did you obtain the subscription object from Subscribe or GetSubscription?");
            }

            _provider.DeleteSubscription(uri);
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
        protected override void CreateStreamCore<TInput, TOutput>(IReactiveQubject<TInput, TOutput> stream, object state)
        {
            Debug.Assert(stream != null);

            var known = TryGetUriFromKnownResource(stream, out Uri uri);
            Debug.Assert(known);

            _provider.CreateStream(uri, stream.Expression, state);
        }

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to delete.</param>
        protected override void DeleteStreamCore<TInput, TOutput>(IReactiveQubject<TInput, TOutput> stream)
        {
            Debug.Assert(stream != null);

            if (!TryGetUriFromKnownResource(stream, out Uri uri))
            {
                throw new InvalidOperationException("Unknown stream object. Could not find a URI identity for the specified stream object. Did you obtain the stream object from Create or GetStream?");
            }

            _provider.DeleteStream(uri);
        }

        #endregion

        #region Private implementation

        private static Uri GetObserverUri<T>(IReactiveQbserver<T> observer)
        {
            if (!TryGetUriFromKnownResource(observer, out Uri uri))
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
