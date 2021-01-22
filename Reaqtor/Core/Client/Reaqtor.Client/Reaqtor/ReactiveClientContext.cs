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
    /// Base class for reactive processing service proxy context with client-side, definition, and metadata operations.
    /// </summary>
    public abstract class ReactiveClientContext : ReactiveClientContextBase
    {
        /// <summary>
        /// Creates a new reactive processing context using the specified services.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="clientServiceProvider">Client/service provider.</param>
        protected ReactiveClientContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider clientServiceProvider)
            : this(expressionServices, clientServiceProvider, clientServiceProvider, clientServiceProvider)
        {
        }

        /// <summary>
        /// Creates a new reactive processing context using the specified services and with default proxy objects.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="clientService">Client-side operation services.</param>
        /// <param name="definitionService">Definition operation services.</param>
        /// <param name="metadataService">Metadata services.</param>
        protected ReactiveClientContext(IReactiveExpressionServices expressionServices, IReactiveClientServiceProvider clientService, IReactiveDefinitionServiceProvider definitionService, IReactiveMetadataServiceProvider metadataService)
            : this(expressionServices, new ReactiveClientProxy(clientService, expressionServices), new ReactiveDefinitionProxy(definitionService, expressionServices), new ReactiveMetadataProxy(metadataService, expressionServices))
        {
        }

        /// <summary>
        /// Creates a new reactive processing context using the specified proxy objects.
        /// </summary>
        /// <param name="expressionServices">Expression tree services.</param>
        /// <param name="clientProxy">Client-side operation proxy.</param>
        /// <param name="definitionProxy">Definition operation proxy.</param>
        /// <param name="metadataProxy">Metadata service proxy.</param>
        protected ReactiveClientContext(IReactiveExpressionServices expressionServices, ReactiveClientProxy clientProxy, ReactiveDefinitionProxy definitionProxy, ReactiveMetadataProxy metadataProxy)
        {
            if (expressionServices == null)
                throw new ArgumentNullException(nameof(expressionServices));

            Client = clientProxy;
            Definition = definitionProxy;
            Metadata = metadataProxy;

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        /// <summary>
        /// Gets the client-side operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveClientProxyBase Client { get; }

        /// <summary>
        /// Gets the definition operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveDefinitionProxyBase Definition { get; }

        /// <summary>
        /// Gets the metadata operations interface for the reactive processing service.
        /// </summary>
        protected override ReactiveMetadataProxyBase Metadata { get; }
    }
}
