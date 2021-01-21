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
    /// Interface representing a resource definition in a reactive processing service. Definitions are cold entities.
    /// </summary>
    /// <remarks>The IAsyncDisposable implementation will undefine the object represented by the definition.</remarks>
    public interface IAsyncReactiveDefinedResource : IAsyncReactiveResource
    {
        /// <summary>
        /// Gets a flag indicating whether the definition is parameterized.
        /// </summary>
        /// <remarks>Type information of the parameter can be inferred through analysis of the expression tree.</remarks>
        bool IsParameterized { get; }

        /// <summary>
        /// Gets the state that was passed during definition of the resource.
        /// </summary>
        /// <remarks>Implementers can provide statically typed accessors in derived types.</remarks>
        object State { get; }

        /// <summary>
        /// Gets the date and time when the resource was defined.
        /// </summary>
        DateTimeOffset DefinitionTime { get; }
    }
}
