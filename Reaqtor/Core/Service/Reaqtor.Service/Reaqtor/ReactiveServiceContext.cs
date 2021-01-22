// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Base class for reactive processing service context with client-side, definition, and metadata operations.
    /// </summary>
    public abstract class ReactiveServiceContext : ReactiveServiceContextBase
    {
        /// <summary>
        /// Creates a new reactive processing context using the specified services.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="clientEngineProvider">Client/service provider.</param>
        protected ReactiveServiceContext(IReactiveExpressionServices expressionServices, IReactiveEngineProvider clientEngineProvider)
            : this(expressionServices, clientEngineProvider, clientEngineProvider, clientEngineProvider)
        {
        }

        /// <summary>
        /// Creates a new reactive processing context using the specified services and with default service objects.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="clientService">Client-side operation services.</param>
        /// <param name="definitionService">Definition operation services.</param>
        /// <param name="metadataService">Metadata services.</param>
        protected ReactiveServiceContext(IReactiveExpressionServices expressionServices, IReactiveClientEngineProvider clientService, IReactiveDefinitionEngineProvider definitionService, IReactiveMetadataEngineProvider metadataService)
            : this(expressionServices, new ReactiveClient(clientService, expressionServices), new ReactiveDefinition(definitionService, expressionServices), new ReactiveMetadata(metadataService, expressionServices))
        {
            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        /// <summary>
        /// Creates a new reactive processing context using the specified service objects.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="client">Client-side operation service object.</param>
        /// <param name="definition">Definition operation service object.</param>
        /// <param name="metadata">Metadata service object.</param>
        protected ReactiveServiceContext(IReactiveExpressionServices expressionServices, ReactiveClient client, ReactiveDefinition definition, ReactiveMetadata metadata)
        {
            if (expressionServices == null)
                throw new ArgumentNullException(nameof(expressionServices));

            Client = client ?? throw new ArgumentNullException(nameof(client));
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        /// <summary>
        /// Gets the client-side operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveClientBase Client { get; }

        /// <summary>
        /// Gets the definition operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveDefinitionBase Definition { get; }

        /// <summary>
        /// Gets the metadata operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveMetadataBase Metadata { get; }
    }
}
