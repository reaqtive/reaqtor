// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of services for expression tree manipulation in reactive processing implementations.
    /// </summary>
    public interface IReactiveExpressionServices
    {
        /// <summary>
        /// Registers an association of an object to its expression representation.
        /// </summary>
        /// <param name="value">Object to associate with an expression representation.</param>
        /// <param name="expression">Expression representation of the object.</param>
        void RegisterObject(object value, Expression expression);

        /// <summary>
        /// Tries to find an association of the specified object to an expression representation.
        /// </summary>
        /// <param name="value">Object to find an associated expression representation for.</param>
        /// <param name="expression">Expression representation of the object, if found.</param>
        /// <returns>true if an association was found; otherwise, false.</returns>
        bool TryGetObject(object value, out Expression expression);

        /// <summary>
        /// Normalizes the specified expression. This method is typically used to process expressions prior to further processing by a service.
        /// </summary>
        /// <param name="expression">Expression to normalize.</param>
        /// <returns>Normalized expression.</returns>
        Expression Normalize(Expression expression);

        /// <summary>
        /// Gets an expression representing a named resource.
        /// </summary>
        /// <param name="type">Type of the resource.</param>
        /// <param name="uri">Name of the resource.</param>
        /// <returns>Expression representing the named resource.</returns>
        Expression GetNamedExpression(Type type, Uri uri);

        /// <summary>
        /// Tries to extract the name of a resource from its expression representation.
        /// </summary>
        /// <param name="expression">Expression to extract a name from.</param>
        /// <param name="uri">Name of the resource, if found.</param>
        /// <returns>true if the expression represents a named resource; otherwise, false.</returns>
        bool TryGetName(Expression expression, out Uri uri);
    }
}
