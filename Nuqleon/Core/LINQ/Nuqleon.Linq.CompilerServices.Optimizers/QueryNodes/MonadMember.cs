// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A representation of the monad member like source nodes and the results of operators.
    /// </summary>
    /// <remarks>
    /// Creates a representation of a monad member.
    /// </remarks>
    /// <param name="elementType">The element type of the monad.</param>
    public abstract class MonadMember(Type elementType) : QueryTree
    {

        /// <summary>
        /// The element type of the monad.
        /// </summary>
        public Type ElementType { get; } = elementType;
    }
}
