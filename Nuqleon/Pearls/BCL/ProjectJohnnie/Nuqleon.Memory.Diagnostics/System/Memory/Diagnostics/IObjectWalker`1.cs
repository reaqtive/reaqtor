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
    /// Interface representing a walker for object graphs.
    /// </summary>
    /// <typeparam name="TEdge">The type used to represent edges in the object graph.</typeparam>
    /// <remarks>
    /// See remarks on <see cref="IObjectVisitor{TEdge}"/> for a discussion on the differentiation between
    /// visitors and walkers.
    /// </remarks>
    public interface IObjectWalker<in TEdge> : IObjectVisitor<TEdge>
    {
        /// <summary>
        /// Walks an object of the specified type, providing the edge that led to the visit of the object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to walk.</typeparam>
        /// <param name="obj">The object to walk.</param>
        /// <param name="edge">The incoming edge.</param>
        void Walk<TObject>(TObject obj, TEdge edge);
    }
}
