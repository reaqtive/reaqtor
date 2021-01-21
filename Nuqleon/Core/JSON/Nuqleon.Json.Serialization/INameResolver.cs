// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System.Collections.Generic;
using System.Reflection;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Interface to resolve names used in keys of properties in JSON objects.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface should be thread-safe.
    /// </remarks>
    public interface INameResolver
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword 'Property'.

        /// <summary>
        /// Gets expected JSON property names for the specified CLR property.
        /// </summary>
        /// <param name="property">The property for which to get JSON property names.</param>
        /// <returns>A sequence of JSON property names to recognize when deserializing the specified CLR property.</returns>
        IEnumerable<string> GetNames(PropertyInfo property);

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Gets expected JSON property names for the specified CLR field.
        /// </summary>
        /// <param name="field">The field for which to get JSON property names.</param>
        /// <returns>A sequence of JSON property names to recognize when deserializing the specified CLR field.</returns>
        IEnumerable<string> GetNames(FieldInfo field);
    }
}
