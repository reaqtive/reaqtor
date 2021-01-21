// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect <see cref="PropertyInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IPropertyInfoIntrospectionProvider : IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the attributes associated with the specified property.
        /// </summary>
        /// <param name="property">The property to get the attributes for.</param>
        /// <returns>The attributes associated with the specified property.</returns>
        PropertyAttributes GetAttributes(PropertyInfo property);

        /// <summary>
        /// Gets the type of the specified property.
        /// </summary>
        /// <param name="property">The property to get the type for.</param>
        /// <returns>The property type type of the property.</returns>
        Type GetPropertyType(PropertyInfo property);

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        /// <param name="property">The property to check for readability.</param>
        /// <returns>true if this property can be read; otherwise, false.</returns>
        bool CanRead(PropertyInfo property);

        /// <summary>
        /// Gets a value indicating whether the property can be written.
        /// </summary>
        /// <param name="property">The property to check for writeable.</param>
        /// <returns>true if this property can be written; otherwise, false.</returns>
        bool CanWrite(PropertyInfo property);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get, set, and other accessors of the specified property.
        /// </summary>
        /// <param name="property">The property to get the accessors for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects whose elements reflect the get, set, and other accessors of the specified property. If <paramref name="nonPublic" /> is true, this array contains public and non-public get, set, and other accessors. If <paramref name="nonPublic" /> is false, this array contains only public get, set, and other accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
        IReadOnlyList<MethodInfo> GetAccessors(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Returns the public or non-public get accessor for this property.
        /// </summary>
        /// <param name="property">The property to get the get accessor for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the get accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the get accessor is non-public, or if <paramref name="nonPublic" /> is true but no get accessors exist.</returns>
        MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Returns the public or non-public set accessor for this property.
        /// </summary>
        /// <param name="property">The property to get the set accessor for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the set accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the set accessor is non-public, or if <paramref name="nonPublic" /> is true but no set accessors exist.</returns>
        MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Returns an array of all the index parameters for the property.
        /// </summary>
        /// <param name="property">The property to get the index parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
        IReadOnlyList<ParameterInfo> GetIndexParameters(PropertyInfo property);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetOptionalCustomModifiers(PropertyInfo property);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetRequiredCustomModifiers(PropertyInfo property);
    }
}
