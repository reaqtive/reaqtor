// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Default name resolver using reflection to resolve names of fields and properties.
    /// </summary>
    public class DefaultNameResolver : INameResolver
    {
        /// <summary>
        /// Gets the singleton instance of the default name resolver.
        /// </summary>
        public static INameResolver Instance { get; } = new DefaultNameResolver();

        /// <summary>
        /// Creates a new default name resolver.
        /// </summary>
        protected DefaultNameResolver()
        {
        }

        /// <summary>
        /// Gets expected JSON property names for the specified CLR field.
        /// </summary>
        /// <param name="field">The field for which to get JSON property names.</param>
        /// <returns>A sequence of JSON property names to recognize when deserializing the specified CLR field.</returns>
        public virtual IEnumerable<string> GetNames(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return new[] { field.Name };
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword 'Property'.

        /// <summary>
        /// Gets expected JSON property names for the specified CLR property.
        /// </summary>
        /// <param name="property">The property for which to get JSON property names.</param>
        /// <returns>A sequence of JSON property names to recognize when deserializing the specified CLR property.</returns>
        public virtual IEnumerable<string> GetNames(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return new[] { property.Name };
        }

#pragma warning restore CA1716
#pragma warning restore IDE0079
    }
}
