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
    public abstract class MonadMember : QueryTree
    {
        /// <summary>
        /// Creates a representation of a monad member.
        /// </summary>
        /// <param name="elementType">The element type of the monad.</param>
        protected MonadMember(Type elementType)
        {
            ElementType = elementType;
        }

        /// <summary>
        /// The element type of the monad.
        /// </summary>
        public Type ElementType { get; }
    }
}
