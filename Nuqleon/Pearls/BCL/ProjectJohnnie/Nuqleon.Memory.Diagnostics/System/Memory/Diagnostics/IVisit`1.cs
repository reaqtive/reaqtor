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
    /// Interface for the reified representation of the visit to an object.
    /// </summary>
    /// <typeparam name="TEdge">The type used to represent edges in the object graph.</typeparam>
    /// <seealso cref="IObjectVisitor{TEdge}"/>
    public interface IVisit<out TEdge>
    {
        /// <summary>
        /// Dispatches the current reified visit operation to the specified object <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        void Accept(IObjectVisitor<TEdge> visitor);
    }
}
