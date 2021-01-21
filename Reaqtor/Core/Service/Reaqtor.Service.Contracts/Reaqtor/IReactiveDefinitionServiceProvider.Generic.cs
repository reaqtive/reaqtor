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
    /// Interface for definitions operations of a reactive processing service.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public interface IReactiveDefinitionServiceProvider<TExpression>
    {
        #region Observable

        /// <summary>
        /// Defines an observable using the specified expression tree representation.
        /// The observable can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observableUri">URI to identify the new observable.</param>
        /// <param name="observable">Expression representing the observable creation. (E.g. using composition of LINQ query operators.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        Task DefineObservableAsync(Uri observableUri, TExpression observable, object state, CancellationToken token);

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="observableUri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        Task UndefineObservableAsync(Uri observableUri, CancellationToken token);

        #endregion

        #region Observer

        /// <summary>
        /// Defines an observer using the specified expression tree representation.
        /// The observer can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observerUri">URI to identify the new observer.</param>
        /// <param name="observer">Expression representing the observer creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        Task DefineObserverAsync(Uri observerUri, TExpression observer, object state, CancellationToken token);

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="observerUri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        Task UndefineObserverAsync(Uri observerUri, CancellationToken token);

        #endregion

        #region Stream factory

        /// <summary>
        /// Defines a stream factory using the specified expression tree representation.
        /// The stream factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="streamFactoryUri">URI to identify the new stream factory.</param>
        /// <param name="streamFactory">Expression representing the stream factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        Task DefineStreamFactoryAsync(Uri streamFactoryUri, TExpression streamFactory, object state, CancellationToken token);

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="streamFactoryUri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token);

        #endregion

        #region Subscription factory

        /// <summary>
        /// Defines a subscription factory using the specified expression tree representation.
        /// The subscription factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI to identify the new subscription factory.</param>
        /// <param name="subscriptionFactory">Expression representing the subscription factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, TExpression subscriptionFactory, object state, CancellationToken token);

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token);

        #endregion
    }
}
