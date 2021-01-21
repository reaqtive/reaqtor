// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

namespace Reaqtor.Metadata
{
    /// <summary>
    /// Interface representing a subscription factory definition in a reactive processing service.
    /// </summary>
    public interface IAsyncReactiveSubscriptionFactoryDefinition : IAsyncReactiveDefinedResource
    {
        /// <summary>
        /// Gets the subscription factory defined by the definition.
        /// </summary>
        /// <returns>Representation of the subscription factory defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        IAsyncReactiveQubscriptionFactory ToSubscriptionFactory();

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
        IAsyncReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>();
    }
}
