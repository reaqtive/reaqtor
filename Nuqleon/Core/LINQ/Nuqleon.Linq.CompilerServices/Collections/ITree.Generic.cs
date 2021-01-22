// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a tree in a generic manner.
    /// </summary>
    /// <typeparam name="T">Type of the data contained in the tree nodes.</typeparam>
    public interface ITree<T> : ITree
    {
        /// <summary>
        /// Gets the data stored in the node.
        /// </summary>
        new T Value { get; }

        /// <summary>
        /// Gets the children of the current tree node.
        /// </summary>
        new IReadOnlyList<ITree<T>> Children { get; }

        /// <summary>
        /// Dispatches the current tree instance to the tree visitor's Visit method.
        /// </summary>
        /// <param name="visitor">Visitor to accept the tree.</param>
        /// <returns>Result of the visit.</returns>
        ITree<T> Accept(ITreeVisitor<T> visitor);

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        ITree<T> Update(IEnumerable<ITree<T>> children);
    }
}
