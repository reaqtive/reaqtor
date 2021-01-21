// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Provides an implement of the reactive metadata services over Azure table storage.
    /// </summary>
    public class AzureReactiveMetadataProxy : ReactiveMetadataProxy
    {
        /// <summary>
        /// Query provider that will be exposed to the IRP facilities to formulate queries against (e.g. by the user through IReactiveMetadata).
        /// </summary>
        private readonly AzureMetadataQueryProvider _queryProvider;

        /// <summary>
        /// Creates a new reactive metadata service proxy using a table client.
        /// </summary>
        /// <param name="tableClient">The table storage client.</param>
        /// <param name="storageResolver">The table address and partition key resolver.</param>
        public AzureReactiveMetadataProxy(ITableClient tableClient, IStorageResolver storageResolver)
            : this(new AzureMetadataServiceProvider(tableClient, storageResolver), new ReactiveExpressionServices(typeof(IReactiveClientProxy)))
        {
        }

        /// <summary>
        /// Creates a new reactive metadata service proxy using the specified service provider and expression tree normalization services.
        /// </summary>
        /// <param name="serviceProvider">Metadata service provider exposing the underlying query provider for use by IRP facilities.</param>
        /// <param name="expressionServices">Expression tree normalization services.</param>
        private AzureReactiveMetadataProxy(AzureMetadataServiceProvider serviceProvider, ReactiveExpressionServices expressionServices)
            : base(serviceProvider, expressionServices)
        {
            _queryProvider = (AzureMetadataQueryProvider)serviceProvider.Provider;
        }

        /// <summary>
        /// Gets a queryable dictionary of the subscriptions that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveSubscriptionProcess, AsyncReactiveSubscriptionTableEntity> Subscriptions => new AzureQueryableDictionary<IAsyncReactiveSubscriptionProcess, AsyncReactiveSubscriptionTableEntity>(_queryProvider, base.Subscriptions);

        /// <summary>
        /// Gets a queryable dictionary of the observables that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveObservableDefinition, AsyncReactiveObservableTableEntity> Observables => new AzureQueryableDictionary<IAsyncReactiveObservableDefinition, AsyncReactiveObservableTableEntity>(_queryProvider, base.Observables);

        /// <summary>
        /// Gets a queryable dictionary of the observers that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveObserverDefinition, AsyncReactiveObserverTableEntity> Observers => new AzureQueryableDictionary<IAsyncReactiveObserverDefinition, AsyncReactiveObserverTableEntity>(_queryProvider, base.Observers);

        /// <summary>
        /// Gets a queryable dictionary of the stream factories that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveStreamFactoryDefinition, AsyncReactiveStreamFactoryTableEntity> StreamFactories => new AzureQueryableDictionary<IAsyncReactiveStreamFactoryDefinition, AsyncReactiveStreamFactoryTableEntity>(_queryProvider, base.StreamFactories);

        /// <summary>
        /// Gets a queryable dictionary of the subscription factories that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveSubscriptionFactoryDefinition, AsyncReactiveSubscriptionFactoryTableEntity> SubscriptionFactories => new AzureQueryableDictionary<IAsyncReactiveSubscriptionFactoryDefinition, AsyncReactiveSubscriptionFactoryTableEntity>(_queryProvider, base.SubscriptionFactories);

        /// <summary>
        /// Gets a queryable dictionary of the streams that have been created in the service.
        /// </summary>
        /// <remarks>
        /// This property provides a statically typed accessor over the underlying raw object model provided by IReactiveMetadata.
        /// </remarks>
        public new AzureQueryableDictionary<IAsyncReactiveStreamProcess, AsyncReactiveStreamTableEntity> Streams => new AzureQueryableDictionary<IAsyncReactiveStreamProcess, AsyncReactiveStreamTableEntity>(_queryProvider, base.Streams);
    }
}
