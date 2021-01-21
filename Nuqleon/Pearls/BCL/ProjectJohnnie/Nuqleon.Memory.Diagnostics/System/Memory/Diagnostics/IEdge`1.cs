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
    /// Interface representing a directed edge in an object graph, with type <typeparamref name="T"/> for the origin of the edge.
    /// </summary>
    /// <typeparam name="T">The type of the origin of the edge.</typeparam>
    public interface IEdge<out T> : IEdge
    {
        /// <summary>
        /// Gets the origin of the edge.
        /// </summary>
        /// <example>
        /// For example, this could be an object of type <see cref="TimeSpan"/> with the <see cref="IEdge.Access"/>
        /// property set to a <see cref="FieldAccess"/> object referring to the <c>TimeSpan._ticks</c> field.
        /// </example>
        T Origin { get; }
    }
}
