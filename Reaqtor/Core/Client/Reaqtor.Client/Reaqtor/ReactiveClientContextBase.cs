// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    using Metadata;

    /// <summary>
    /// Base class for reactive processing service proxy contexts.
    /// </summary>
    public abstract partial class ReactiveClientContextBase : IReactiveProxy
    {
        #region Client

        /// <summary>
        /// Gets the client-side operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveClientProxyBase Client { get; }

        /// <summary>
        /// Gets the stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TInput, TOutput>(uri);

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TArgs, TInput, TOutput>(uri);

        /// <summary>
        /// Gets the stream, represented as a subject, with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="uri">URI identifying the stream.</param>
        /// <returns>Subject object that can be used to receive and publish data.</returns>
        public IAsyncReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri) => Client.GetStream<TInput, TOutput>(uri);

        /// <summary>
        /// Gets the observable with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public IAsyncReactiveQbservable<T> GetObservable<T>(Uri uri) => Client.GetObservable<T>(uri);

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArgs, IAsyncReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri) => Client.GetObservable<TArgs, TResult>(uri);

        /// <summary>
        /// Gets the observer with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public IAsyncReactiveQbserver<T> GetObserver<T>(Uri uri) => Client.GetObserver<T>(uri);

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArgs, IAsyncReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri) => Client.GetObserver<TArgs, TResult>(uri);

        /// <summary>
        /// Gets the subscription factory with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IAsyncReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri) => Client.GetSubscriptionFactory(uri);

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IAsyncReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri) => Client.GetSubscriptionFactory<TArgs>(uri);

        /// <summary>
        /// Gets the subscription with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription.</param>
        /// <returns>Subscription object that can be used to dispose the subscription.</returns>
        public IAsyncReactiveQubscription GetSubscription(Uri uri) => Client.GetSubscription(uri);

        /// <summary>
        /// Gets the query provider that is used to build observables, observers, and streams.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider => Client.Provider;

        #endregion

        #region Definition

        /// <summary>
        /// Gets the definition operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveDefinitionProxyBase Definition { get; }

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
        public Task DefineStreamFactoryAsync<TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null, CancellationToken token = default) => Definition.DefineStreamFactoryAsync(uri, streamFactory, state, token);

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
        public Task DefineStreamFactoryAsync<TArgs, TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null, CancellationToken token = default) => Definition.DefineStreamFactoryAsync(uri, streamFactory, state, token);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        public Task UndefineStreamFactoryAsync(Uri uri, CancellationToken token) => Definition.UndefineStreamFactoryAsync(uri, token);

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        public Task DefineObservableAsync<T>(Uri uri, IAsyncReactiveQbservable<T> observable, object state = null, CancellationToken token = default) => Definition.DefineObservableAsync(uri, observable, state, token);

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
        public Task DefineObservableAsync<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> observable, object state = null, CancellationToken token = default) => Definition.DefineObservableAsync(uri, observable, state, token);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        public Task UndefineObservableAsync(Uri uri, CancellationToken token) => Definition.UndefineObservableAsync(uri, token);

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        public Task DefineObserverAsync<T>(Uri uri, IAsyncReactiveQbserver<T> observer, object state = null, CancellationToken token = default) => Definition.DefineObserverAsync(uri, observer, state, token);

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
        public Task DefineObserverAsync<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> observer, object state = null, CancellationToken token = default) => Definition.DefineObserverAsync(uri, observer, state, token);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        public Task UndefineObserverAsync(Uri uri, CancellationToken token) => Definition.UndefineObserverAsync(uri, token);

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        public Task DefineSubscriptionFactoryAsync(Uri uri, IAsyncReactiveQubscriptionFactory subscriptionFactory, object state = null, CancellationToken token = default) => Definition.DefineSubscriptionFactoryAsync(uri, subscriptionFactory, state, token);

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        public Task DefineSubscriptionFactoryAsync<TArgs>(Uri uri, IAsyncReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null, CancellationToken token = default) => Definition.DefineSubscriptionFactoryAsync(uri, subscriptionFactory, state, token);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        public Task UndefineSubscriptionFactoryAsync(Uri uri, CancellationToken token) => Definition.UndefineSubscriptionFactoryAsync(uri, token);

        #endregion

        #region Metadata

        /// <summary>
        /// Gets the metadata operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveMetadataProxyBase Metadata { get; }

        /// <summary>
        /// Gets a queryable dictionary of stream factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveStreamFactoryDefinition> StreamFactories => Metadata.StreamFactories;

        /// <summary>
        /// Gets a queryable dictionary of stream objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveStreamProcess> Streams => Metadata.Streams;

        /// <summary>
        /// Gets a queryable dictionary of observable definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition> Observables => Metadata.Observables;

        /// <summary>
        /// Gets a queryable dictionary of observer definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveObserverDefinition> Observers => Metadata.Observers;

        /// <summary>
        /// Gets a queryable dictionary of subscription factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveSubscriptionFactoryDefinition> SubscriptionFactories => Metadata.SubscriptionFactories;

        /// <summary>
        /// Gets a queryable dictionary of subscription objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveSubscriptionProcess> Subscriptions => Metadata.Subscriptions;

        #endregion
    }
}
