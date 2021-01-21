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
    /// Interface for definitions operations of a reactive processing engine.
    /// </summary>
    public interface IReactiveDefinitionEngineProvider
    {
        #region Observable

        /// <summary>
        /// Defines an observable using the specified expression tree representation.
        /// The observable can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observableUri">URI to identify the new observable.</param>
        /// <param name="observable">Expression representing the observable creation. (E.g. using composition of LINQ query operators.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void DefineObservable(Uri observableUri, Expression observable, object state);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="observableUri">URI identifying the observable.</param>
        void UndefineObservable(Uri observableUri);

        #endregion

        #region Observer

        /// <summary>
        /// Defines an observer using the specified expression tree representation.
        /// The observer can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observerUri">URI to identify the new observer.</param>
        /// <param name="observer">Expression representing the observer creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void DefineObserver(Uri observerUri, Expression observer, object state);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="observerUri">URI identifying the observer.</param>
        void UndefineObserver(Uri observerUri);

        #endregion

        #region Stream factory

        /// <summary>
        /// Defines a stream factory using the specified expression tree representation.
        /// The stream factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="streamFactoryUri">URI to identify the new stream factory.</param>
        /// <param name="streamFactory">Expression representing the stream factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="streamFactoryUri">URI identifying the stream factory.</param>
        void UndefineStreamFactory(Uri streamFactoryUri);

        #endregion

        #region Subscription factory

        /// <summary>
        /// Defines a subscription factory using the specified expression tree representation.
        /// The subscription factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI to identify the new subscription factory.</param>
        /// <param name="subscriptionFactory">Expression representing the subscription factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI identifying the subscription factory.</param>
        void UndefineSubscriptionFactory(Uri subscriptionFactoryUri);

        #endregion
    }
}
