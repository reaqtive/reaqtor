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
    /// Base class representation of a directed edge in an object graph.
    /// </summary>
    /// <remarks>
    /// Use the generic <see cref="Edge{T}"/> variant to get access to the source object of the edge.
    /// </remarks>
    public abstract class Edge : IEdge
    {
        /// <summary>
        /// Creates a new edge using the specified <paramref name="access"/>.
        /// </summary>
        /// <param name="access">The access representation of the edge.</param>
        protected Edge(Access access)
        {
            Access = access;
        }

        /// <summary>
        /// Gets the access representation of the edge.
        /// </summary>
        public Access Access { get; }

        /// <summary>
        /// Creates an edge originating from the specified <paramref name="origin"/> and using the specified <paramref name="access"/>.
        /// </summary>
        /// <typeparam name="T">The type of the origin of the edge.</typeparam>
        /// <param name="origin">The origin object.</param>
        /// <param name="access">The access representation of the edge.</param>
        /// <returns>A new edge instance.</returns>
        public static Edge<T> Create<T>(T origin, Access access)
        {
            if (access == null)
                throw new ArgumentNullException(nameof(access));

            return new Edge<T>(origin, access);
        }
    }
}
