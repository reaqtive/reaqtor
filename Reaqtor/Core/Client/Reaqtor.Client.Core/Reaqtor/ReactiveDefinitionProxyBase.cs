// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Base class for reactive processing definition operations.
    /// </summary>
    public abstract partial class ReactiveDefinitionProxyBase : IReactiveDefinitionProxy
    {
        #region StreamFactory

        /// <summary>
        /// Defines a stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        public Task DefineStreamFactoryAsync<TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            return DefineStreamFactoryAsyncCore<TInput, TOutput>(uri, streamFactory, state, token);
        }

        /// <summary>
        /// Defines a stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        protected abstract Task DefineStreamFactoryAsyncCore<TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null, CancellationToken token = default);

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        public Task DefineStreamFactoryAsync<TArgs, TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            return DefineStreamFactoryAsyncCore<TArgs, TInput, TOutput>(uri, streamFactory, state, token);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        protected abstract Task DefineStreamFactoryAsyncCore<TArgs, TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null, CancellationToken token = default);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        public Task UndefineStreamFactoryAsync(Uri uri, CancellationToken token)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return UndefineStreamFactoryAsyncCore(uri, token);
        }

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        protected abstract Task UndefineStreamFactoryAsyncCore(Uri uri, CancellationToken token);

        #endregion

        #region Observable

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        public Task DefineObservableAsync<T>(Uri uri, IAsyncReactiveQbservable<T> observable, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            return DefineObservableAsyncCore<T>(uri, observable, state, token);
        }

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        protected abstract Task DefineObservableAsyncCore<T>(Uri uri, IAsyncReactiveQbservable<T> observable, object state = null, CancellationToken token = default);

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        public Task DefineObservableAsync<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> observable, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            return DefineObservableAsyncCore<TArgs, TResult>(uri, observable, state, token);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        protected abstract Task DefineObservableAsyncCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> observable, object state = null, CancellationToken token = default);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        public Task UndefineObservableAsync(Uri uri, CancellationToken token)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return UndefineObservableAsyncCore(uri, token);
        }

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        protected abstract Task UndefineObservableAsyncCore(Uri uri, CancellationToken token);

        #endregion

        #region Observer

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        public Task DefineObserverAsync<T>(Uri uri, IAsyncReactiveQbserver<T> observer, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DefineObserverAsyncCore<T>(uri, observer, state, token);
        }

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        protected abstract Task DefineObserverAsyncCore<T>(Uri uri, IAsyncReactiveQbserver<T> observer, object state = null, CancellationToken token = default);

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        public Task DefineObserverAsync<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> observer, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DefineObserverAsyncCore<TArgs, TResult>(uri, observer, state, token);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        protected abstract Task DefineObserverAsyncCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> observer, object state = null, CancellationToken token = default);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        public Task UndefineObserverAsync(Uri uri, CancellationToken token)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return UndefineObserverAsyncCore(uri, token);
        }

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        protected abstract Task UndefineObserverAsyncCore(Uri uri, CancellationToken token);

        #endregion

        #region SubscriptionFactory

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        public Task DefineSubscriptionFactoryAsync(Uri uri, IAsyncReactiveQubscriptionFactory subscriptionFactory, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            return DefineSubscriptionFactoryAsyncCore(uri, subscriptionFactory, state, token);
        }

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        protected abstract Task DefineSubscriptionFactoryAsyncCore(Uri uri, IAsyncReactiveQubscriptionFactory subscriptionFactory, object state = null, CancellationToken token = default);

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        public Task DefineSubscriptionFactoryAsync<TArgs>(Uri uri, IAsyncReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null, CancellationToken token = default)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            return DefineSubscriptionFactoryAsyncCore<TArgs>(uri, subscriptionFactory, state, token);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        protected abstract Task DefineSubscriptionFactoryAsyncCore<TArgs>(Uri uri, IAsyncReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null, CancellationToken token = default);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        public Task UndefineSubscriptionFactoryAsync(Uri uri, CancellationToken token)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return UndefineSubscriptionFactoryAsyncCore(uri, token);
        }

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        protected abstract Task UndefineSubscriptionFactoryAsyncCore(Uri uri, CancellationToken token);

        #endregion
    }
}
