// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Interface for a visitor over non-generic trees.
    /// </summary>
    public interface ITreeVisitor
    {
        /// <summary>
        /// Visits the specified tree.
        /// </summary>
        /// <param name="node">Tree to visit.</param>
        /// <returns>Result of the visit.</returns>
        ITree Visit(ITree node);
    }
}
