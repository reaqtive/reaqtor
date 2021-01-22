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

namespace Reaqtor
{
    using Metadata;

    /// <summary>
    /// Base class for reactive processing service contexts.
    /// </summary>
    public abstract partial class ReactiveServiceContextBase : IReactive
    {
        #region Client

        /// <summary>
        /// Gets the client-side operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveClientBase Client { get; }

        /// <summary>
        /// Gets the stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TInput, TOutput>(uri);

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TArgs, TInput, TOutput>(uri);

        /// <summary>
        /// Gets the stream, represented as a subject, with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="uri">URI identifying the stream.</param>
        /// <returns>Subject object that can be used to receive and publish data.</returns>
        public IReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri) => Client.GetStream<TInput, TOutput>(uri);

        /// <summary>
        /// Gets the observable with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public IReactiveQbservable<T> GetObservable<T>(Uri uri) => Client.GetObservable<T>(uri);

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArgs, IReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri) => Client.GetObservable<TArgs, TResult>(uri);

        /// <summary>
        /// Gets the observer with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public IReactiveQbserver<T> GetObserver<T>(Uri uri) => Client.GetObserver<T>(uri);

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArgs, IReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri) => Client.GetObserver<TArgs, TResult>(uri);

        /// <summary>
        /// Gets the subscription factory with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri) => Client.GetSubscriptionFactory(uri);

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri) => Client.GetSubscriptionFactory<TArgs>(uri);

        /// <summary>
        /// Gets the subscription with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription.</param>
        /// <returns>Subscription object that can be used to dispose the subscription.</returns>
        public IReactiveQubscription GetSubscription(Uri uri) => Client.GetSubscription(uri);

        /// <summary>
        /// Gets the query provider that is used to build observables, observers, and streams.
        /// </summary>
        public IReactiveQueryProvider Provider => Client.Provider;

        #endregion

        #region Definition

        /// <summary>
        /// Gets the definition operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveDefinitionBase Definition { get; }

        /// <summary>
        /// Defines a stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineStreamFactory<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null) => Definition.DefineStreamFactory(uri, streamFactory, state);

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineStreamFactory<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null) => Definition.DefineStreamFactory(uri, streamFactory, state);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        public void UndefineStreamFactory(Uri uri) => Definition.UndefineStreamFactory(uri);

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObservable<T>(Uri uri, IReactiveQbservable<T> observable, object state = null) => Definition.DefineObservable(uri, observable, state);

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObservable<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state = null) => Definition.DefineObservable(uri, observable, state);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        public void UndefineObservable(Uri uri) => Definition.UndefineObservable(uri);

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObserver<T>(Uri uri, IReactiveQbserver<T> observer, object state = null) => Definition.DefineObserver(uri, observer, state);

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObserver<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state = null) => Definition.DefineObserver(uri, observer, state);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        public void UndefineObserver(Uri uri) => Definition.UndefineObserver(uri);

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineSubscriptionFactory(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state = null) => Definition.DefineSubscriptionFactory(uri, subscriptionFactory, state);

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineSubscriptionFactory<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null) => Definition.DefineSubscriptionFactory(uri, subscriptionFactory, state);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        public void UndefineSubscriptionFactory(Uri uri) => Definition.UndefineSubscriptionFactory(uri);

        #endregion

        #region Metadata

        /// <summary>
        /// Gets the metadata operations interface for the reactive processing service.
        /// </summary>
        protected abstract ReactiveMetadataBase Metadata { get; }

        /// <summary>
        /// Gets a queryable dictionary of stream factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories => Metadata.StreamFactories;

        /// <summary>
        /// Gets a queryable dictionary of stream objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveStreamProcess> Streams => Metadata.Streams;

        /// <summary>
        /// Gets a queryable dictionary of observable definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveObservableDefinition> Observables => Metadata.Observables;

        /// <summary>
        /// Gets a queryable dictionary of observer definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveObserverDefinition> Observers => Metadata.Observers;

        /// <summary>
        /// Gets a queryable dictionary of subscription factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories => Metadata.SubscriptionFactories;

        /// <summary>
        /// Gets a queryable dictionary of subscription objects.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions => Metadata.Subscriptions;

        #endregion
    }
}
