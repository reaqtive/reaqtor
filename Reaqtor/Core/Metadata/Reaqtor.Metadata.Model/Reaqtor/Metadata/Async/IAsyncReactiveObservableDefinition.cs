// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor.Metadata
{
    /// <summary>
    /// Interface representing an observable definition in a reactive processing service.
    /// </summary>
    public interface IAsyncReactiveObservableDefinition : IAsyncReactiveDefinedResource
    {
        /// <summary>
        /// Gets the observable defined by the definition.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <returns>Representation of the observable defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        IAsyncReactiveQbservable<T> ToObservable<T>();

        /// <summary>
        /// Gets the parameterized observable defined by the definition.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <returns>Representation of the parameterized observable defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        Func<TArgs, IAsyncReactiveQbservable<TResult>> ToObservable<TArgs, TResult>();
    }
}
