// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IPropertyInfoIntrospectionProvider"/>.
    /// </summary>
    public static class PropertyInfoIntrospectionProviderExtensions
    {
        /// <summary>
        /// Returns the public get accessor for this property.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="property">The property to get the get accessor for.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the public get accessor for this property, or null if the get accessor is non-public or does not exist.</returns>
        public static MethodInfo GetGetMethod(this IPropertyInfoIntrospectionProvider provider, PropertyInfo property) => NotNull(provider).GetGetMethod(property, nonPublic: false);

        /// <summary>
        /// Returns the public set accessor for this property.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="property">The property to get the set accessor for.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the public set accessor for this property, or null if the set accessor is non-public or does not exist.</returns>
        public static MethodInfo GetSetMethod(this IPropertyInfoIntrospectionProvider provider, PropertyInfo property) => NotNull(provider).GetSetMethod(property, nonPublic: false);

        /// <summary>
        /// Returns the public accessors for this property.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="property">The property to get the accessors for.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects that reflect the public get, set, and other accessors of the property reflected by the current instance, if found; otherwise, this method returns an array with zero (0) elements.</returns>
        public static IReadOnlyList<MethodInfo> GetAccessors(this IPropertyInfoIntrospectionProvider provider, PropertyInfo property) => NotNull(provider).GetAccessors(property, nonPublic: false);

        /// <summary>
        /// Gets a value indicating whether the property has the SpecialName attribute.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="property">The property to inspect.</param>
        /// <returns>true if the property has the SpecialName attribute set; otherwise, false.</returns>
        public static bool IsSpecialName(this IPropertyInfoIntrospectionProvider provider, PropertyInfo property) => (NotNull(provider).GetAttributes(property) & PropertyAttributes.SpecialName) > PropertyAttributes.None;
    }
}
