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

namespace Reaqtor
{
    /// <summary>
    /// Base class for reactive processing definition operations.
    /// </summary>
    public abstract partial class ReactiveDefinitionBase : IReactiveDefinition
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
        public void DefineStreamFactory<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            DefineStreamFactoryCore<TInput, TOutput>(uri, streamFactory, state);
        }

        /// <summary>
        /// Defines a stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineStreamFactoryCore<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null);

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineStreamFactory<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            DefineStreamFactoryCore<TArgs, TInput, TOutput>(uri, streamFactory, state);
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
        protected abstract void DefineStreamFactoryCore<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        public void UndefineStreamFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            UndefineStreamFactoryCore(uri);
        }

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        protected abstract void UndefineStreamFactoryCore(Uri uri);

        #endregion

        #region Observable

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObservable<T>(Uri uri, IReactiveQbservable<T> observable, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            DefineObservableCore<T>(uri, observable, state);
        }

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineObservableCore<T>(Uri uri, IReactiveQbservable<T> observable, object state = null);

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObservable<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            DefineObservableCore<TArgs, TResult>(uri, observable, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineObservableCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state = null);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        public void UndefineObservable(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            UndefineObservableCore(uri);
        }

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        protected abstract void UndefineObservableCore(Uri uri);

        #endregion

        #region Observer

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObserver<T>(Uri uri, IReactiveQbserver<T> observer, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            DefineObserverCore<T>(uri, observer, state);
        }

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineObserverCore<T>(Uri uri, IReactiveQbserver<T> observer, object state = null);

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineObserver<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            DefineObserverCore<TArgs, TResult>(uri, observer, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineObserverCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state = null);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        public void UndefineObserver(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            UndefineObserverCore(uri);
        }

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        protected abstract void UndefineObserverCore(Uri uri);

        #endregion

        #region SubscriptionFactory

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineSubscriptionFactory(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            DefineSubscriptionFactoryCore(uri, subscriptionFactory, state);
        }

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineSubscriptionFactoryCore(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state = null);

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        public void DefineSubscriptionFactory<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            DefineSubscriptionFactoryCore<TArgs>(uri, subscriptionFactory, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void DefineSubscriptionFactoryCore<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        public void UndefineSubscriptionFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            UndefineSubscriptionFactoryCore(uri);
        }

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        protected abstract void UndefineSubscriptionFactoryCore(Uri uri);

        #endregion
    }
}
