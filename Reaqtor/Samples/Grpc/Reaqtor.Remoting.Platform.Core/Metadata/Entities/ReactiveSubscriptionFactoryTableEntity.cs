// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing a subscription factory.
    /// </summary>
    public class ReactiveSubscriptionFactoryTableEntity : ReactiveDefinedResourceTableEntity, IReactiveSubscriptionFactoryDefinition
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public ReactiveSubscriptionFactoryTableEntity()
        {
        }

        /// <summary>
        /// Gets the subscription factory defined by the definition.
        /// </summary>
        /// <returns>Representation of the subscription factory defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        public IReactiveQubscriptionFactory ToSubscriptionFactory()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on metadata entities.");
        }

        /// <summary>
        /// Gets the parameterized subscription factory defined by the definition.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <returns>Representation of the parameterized subscription factory defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        public IReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on metadata entities.");
        }
    }
}
