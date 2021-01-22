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
    /// Interface representing a reflection provider used to introspect <see cref="ParameterInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IParameterInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the attributes for the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the attributes for.</param>
        /// <returns>A <see cref="ParameterAttributes"/> object representing the attributes for this parameter.</returns>
        ParameterAttributes GetAttributes(ParameterInfo parameter);

        /// <summary>
        /// Gets the custom attributes data defined on the specified parameter.
        /// </summary>
        /// <param name="parameter">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified parameter.</returns>
        IEnumerable<CustomAttributeData> GetCustomAttributesData(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified parameter, or an array with zero elements if no attributes are defined.</returns>
        IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified parameter and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array of custom attributes applied to the specified parameter, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating whether the specified parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to check for a default value.</param>
        /// <returns>true if the specified parameter has a default value; otherwise, false.</returns>
        bool HasDefaultValue(ParameterInfo parameter);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="parameter"/>; otherwise, false.</returns>
        bool IsDefined(ParameterInfo parameter, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        /// <param name="parameter">The parameter to get the member for.</param>
        /// <returns>The member which implanted the parameter represented by <paramref name="parameter"/>.</returns>
        MemberInfo GetMember(ParameterInfo parameter);

        /// <summary>
        /// Gets a value that identifies this parameter in metadata.
        /// </summary>
        /// <param name="parameter">The parameter to get the metadata token for.</param>
        /// <returns>A value which, in combination with the module, uniquely identifies this parameter in metadata.</returns>
        int GetMetadataToken(ParameterInfo parameter);

        /// <summary>
        /// Gets the name of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the name for.</param>
        /// <returns>The name of the specified parameter.</returns>
        string GetName(ParameterInfo parameter);

        /// <summary>
        /// Gets the type of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the type for.</param>
        /// <returns>The type of the specified parameter.</returns>
        Type GetParameterType(ParameterInfo parameter);

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list.
        /// </summary>
        /// <param name="parameter">The parameter to get the position for.</param>
        /// <returns>An integer representing the position the specified parameter occupies in the parameter list.</returns>
        int GetPosition(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetOptionalCustomModifiers(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetRequiredCustomModifiers(ParameterInfo parameter);
    }
}
