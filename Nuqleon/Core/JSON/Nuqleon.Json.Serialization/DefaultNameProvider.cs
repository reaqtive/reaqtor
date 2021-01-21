// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using System;
using System.Reflection;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Default name provider using reflection to provide names for fields and properties.
    /// </summary>
    public class DefaultNameProvider : INameProvider
    {
        /// <summary>
        /// Gets the singleton instance of the default name provider.
        /// </summary>
        public static INameProvider Instance { get; } = new DefaultNameProvider();

        /// <summary>
        /// Creates a new default name provider.
        /// </summary>
        protected DefaultNameProvider()
        {
        }

        /// <summary>
        /// Gets a JSON property name for the specified CLR field.
        /// </summary>
        /// <param name="field">The field for which to get JSON a property name.</param>
        /// <returns>A JSON property name to use when serializing the specified CLR field.</returns>
        public virtual string GetName(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return field.Name;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword 'Property'.

        /// <summary>
        /// Gets a JSON property name for the specified CLR property.
        /// </summary>
        /// <param name="property">The property for which to get a JSON property name.</param>
        /// <returns>A JSON property name to use when serializing the specified CLR property.</returns>
        public virtual string GetName(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.Name;
        }

#pragma warning restore CA1716
#pragma warning restore IDE0079
    }
}
