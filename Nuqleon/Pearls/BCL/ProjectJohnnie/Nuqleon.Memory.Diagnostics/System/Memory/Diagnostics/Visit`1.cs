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
    /// Reified representation of the visit to an object.
    /// </summary>
    /// <typeparam name="TObject">The type of the object being visited.</typeparam>
    /// <typeparam name="TEdge">The type used to represent edges in the object graph.</typeparam>
    public class Visit<TObject, TEdge> : IVisit<TEdge>
    {
        /// <summary>
        /// Creates a new reified representation of a visit to an object.
        /// </summary>
        /// <param name="obj">The object being visited.</param>
        /// <param name="edge">The incoming edge that led to the object being visited.</param>
        public Visit(TObject obj, TEdge edge)
        {
            Object = obj;
            Edge = edge;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1720 // Identifier 'Object' contains type name.

        /// <summary>
        /// Gets the object being visited.
        /// </summary>
        public TObject Object { get; }

#pragma warning restore CA1720
#pragma warning restore IDE0079

        /// <summary>
        /// Gets the incoming edge that led to the object being visited.
        /// </summary>
        public TEdge Edge { get; }

        /// <summary>
        /// Dispatches the current reified visit operation to the specified object <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        public void Accept(IObjectVisitor<TEdge> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(Object, Edge);
        }
    }
}
