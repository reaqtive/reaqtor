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
    /// Representation of a directed edge in an object graph.
    /// </summary>
    /// <typeparam name="T">The type of the origin of the edge.</typeparam>
    public sealed class Edge<T> : Edge, IEdge<T>
    {
        /// <summary>
        /// Creates a new edge using the specified <paramref name="origin"/> object and <paramref name="access"/>.
        /// </summary>
        /// <param name="origin">The origin of the edge.</param>
        /// <param name="access">The access representation of the edge.</param>
        internal Edge(T origin, Access access)
            : base(access)
        {
            Origin = origin;
        }

        /// <summary>
        /// Gets the origin of the edge.
        /// </summary>
        public T Origin { get; }

        /// <summary>
        /// Gets a friendly string representation of the edge.
        /// </summary>
        /// <returns>A friendly string representation of the edge.</returns>
        public override string ToString() => string.Concat(Origin, Access);
    }
}
