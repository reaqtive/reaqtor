// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a factory for wildcard objects used to match any subtree of a given type.
    /// </summary>
    /// <typeparam name="TSource">Type of the wildcard objects.</typeparam>
    public interface IWildcardFactory<TSource>
    {
        /// <summary>
        /// Creates a new wildcard object for the given hole in a pattern.
        /// </summary>
        /// <param name="hole">Parameter expression used for holes in a pattern, which will be matched by the wildcard.</param>
        /// <returns>Wildcard for the given hole in a pattern.</returns>
        TSource CreateWildcard(ParameterExpression hole);
    }
}
