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
    /// Interface representing a visitor for object graphs.
    /// </summary>
    /// <typeparam name="TEdge">The type used to represent edges in the object graph.</typeparam>
    /// <remarks>
    /// Visitors for object graphs are non-recursive in order to avoid stack overflows when traversing complex
    /// object graphs. The <see cref="IObjectWalker{TEdge}"/> interface can be used to implement object graph
    /// walkers which traverse an entire object graph, while visitors can be used to dispatch the visit for a
    /// single object and edge.
    /// </remarks>
    public interface IObjectVisitor<in TEdge>
    {
        /// <summary>
        /// Visits an object of the specified type, providing the edge that led to the visit of the object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to visit.</typeparam>
        /// <param name="obj">The object to visit.</param>
        /// <param name="edge">The incoming edge.</param>
        void Visit<TObject>(TObject obj, TEdge edge);
    }
}
