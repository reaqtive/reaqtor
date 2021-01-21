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
    /// Interface representing a reflection provider used to introspect <see cref="MethodInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IMethodInfoIntrospectionProvider : IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the generic method definition of the specified generic method.
        /// </summary>
        /// <param name="method">The method to get the generic method definition for.</param>
        /// <returns>The generic method definition of the specified method.</returns>
        MethodInfo GetGenericMethodDefinition(MethodInfo method);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <param name="method">The method to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic method. Returns an empty array if the specified method is not a generic method.</returns>
        IReadOnlyList<Type> GetGenericArguments(MethodInfo method);

        /// <summary>
        /// Gets a <see cref="ParameterInfo" /> object that contains information about the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        /// <param name="method">The method to get the return parameter for.</param>
        /// <returns>A <see cref="ParameterInfo" /> object that contains information about the return type.</returns>
        ParameterInfo GetReturnParameter(MethodInfo method);

        /// <summary>
        /// Gets the return type of the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get the return type for.</param>
        /// <returns>A <see cref="Type" /> object that contains information about the return type.</returns>
        Type GetReturnType(MethodInfo method);

        /// <summary>
        /// Gets the custom attributes for the return type of the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get the custom attributes for the return type for.</param>
        /// <returns>An <see cref="ICustomAttributeProvider"/> object representing the custom attributes for the return type.</returns>
        ICustomAttributeProvider GetReturnTypeCustomAttributes(MethodInfo method);
    }
}
