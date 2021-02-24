// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Interface representing a directed edge in an object graph.
    /// </summary>
    /// <remarks>
    /// Use the generic <see cref="IEdge{T}"/> variant to get access to the source object of the edge.
    /// </remarks>
    public interface IEdge
    {
        /// <summary>
        /// Gets the access representation of the edge.
        /// </summary>
        /// <example>
        /// For example, an access pattern could be a field traversal using <see cref="FieldAccess"/>.
        /// </example>
        Access Access { get; }
    }
}
