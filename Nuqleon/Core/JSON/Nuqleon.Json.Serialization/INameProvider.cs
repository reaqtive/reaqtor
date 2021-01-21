// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using System.Reflection;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Interface to provide names used for keys of properties in JSON objects.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface should be thread-safe.
    /// </remarks>
    public interface INameProvider
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword 'Property'.

        /// <summary>
        /// Gets a JSON property name for the specified CLR property.
        /// </summary>
        /// <param name="property">The property for which to get a JSON property name.</param>
        /// <returns>A JSON property name to use when serializing the specified CLR property.</returns>
        string GetName(PropertyInfo property);

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a JSON property name for the specified CLR field.
        /// </summary>
        /// <param name="field">The field for which to get JSON a property name.</param>
        /// <returns>A JSON property name to use when serializing the specified CLR field.</returns>
        string GetName(FieldInfo field);
    }
}
