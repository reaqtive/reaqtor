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
    /// Interface representing a reflection provider used to introspect <see cref="MemberInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the custom attributes data defined on the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified member.</returns>
        IEnumerable<CustomAttributeData> GetCustomAttributesData(MemberInfo member);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified member, or an array with zero elements if no attributes are defined.</returns>
        IReadOnlyList<object> GetCustomAttributes(MemberInfo member, bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified member and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array of custom attributes applied to the specified member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        IReadOnlyList<object> GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit);

        /// <summary>
        /// Gets the declaring type of the specified member.
        /// </summary>
        /// <param name="member">The member to get the declaring type for.</param>
        /// <returns>The declaring type of the specified member.</returns>
        Type GetDeclaringType(MemberInfo member);

        /// <summary>
        /// Gets a value that identifies a metadata element.
        /// </summary>
        /// <param name="member">The member to get the metadata token for.</param>
        /// <returns>A value which, in combination with <see cref="MemberInfo.Module" />, uniquely identifies a metadata element.</returns>
        int GetMetadataToken(MemberInfo member);

        /// <summary>
        /// Gets the module the member is defined in.
        /// </summary>
        /// <param name="member">The member to get the module for.</param>
        /// <returns>The module defining the member.</returns>
        Module GetModule(MemberInfo member);

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="member">The member to get the name for.</param>
        /// <returns>The name of the member.</returns>
        string GetName(MemberInfo member);

        /// <summary>
        /// Gets the class object that was used to obtain the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member for which to get the reflected type.</param>
        /// <returns>The <see cref="Type"/> object through which the specified <see cref="MemberInfo"/> object was obtained.</returns>
        Type GetReflectedType(MemberInfo member);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="member"/>; otherwise, false.</returns>
        bool IsDefined(MemberInfo member, Type attributeType, bool inherit);
    }
}
