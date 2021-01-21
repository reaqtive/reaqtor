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
    /// Represents a tree in a non-generic manner.
    /// </summary>
    public interface ITree
    {
        /// <summary>
        /// Gets the data stored in the node.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets the children of the current tree node.
        /// </summary>
        IReadOnlyList<ITree> Children { get; }

        /// <summary>
        /// Dispatches the current tree instance to the tree visitor's Visit method.
        /// </summary>
        /// <param name="visitor">Visitor to accept the tree.</param>
        /// <returns>Result of the visit.</returns>
        ITree Accept(ITreeVisitor visitor);

        /// <summary>
        /// Updates the tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        ITree Update(IEnumerable<ITree> children);

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        string ToStringFormat();

        /// <summary>
        /// Gets a string representation of the current node and its children, using the specified indentation level.
        /// </summary>
        /// <param name="indent">Indentation level. Recursive calls to obtain a string representation of child nodes will use an incremented indentation level.</param>
        /// <returns>String representation of the current node.</returns>
        string ToString(int indent);
    }
}
