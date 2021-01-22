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
    /// Provides names for resource artifacts.
    /// </summary>
    public static class ResourceNaming
    {
        /// <summary>
        /// Gets a typed "this" reference expression representation to the specified object.
        /// </summary>
        /// <param name="value">Object to get a typed "this" reference for.</param>
        /// <returns>Typed "this" reference expression representation.</returns>
        public static Expression GetThisReferenceExpression(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return GetThisReferenceExpression(value.GetType());
        }

        /// <summary>
        /// Gets a typed "this" reference expression representation to an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <returns>Typed "this" reference expression representation.</returns>
        public static Expression GetThisReferenceExpression(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Expression.Parameter(type, Constants.CurrentInstanceUri);
        }

        /// <summary>
        /// Checks if the given parameter expression is a "this" reference expression representation to an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <param name="parameter">The parameter expression to check.</param>
        /// <returns>True if this is a "this" reference expression or false otherwise.</returns>
        public static bool IsThisReferenceExpression(Type type, ParameterExpression parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return
                parameter.Type == type &&
                parameter.Name == Constants.CurrentInstanceUri;
        }
    }
}
