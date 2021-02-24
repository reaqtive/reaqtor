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
    /// Interface representing a reflection provider used to introspect <see cref="MethodBase"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IMethodBaseIntrospectionProvider : IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the attributes associated with the specified method.
        /// </summary>
        /// <param name="method">The property to get the method for.</param>
        /// <returns>The attributes associated with the specified method.</returns>
        MethodAttributes GetAttributes(MethodBase method);

        /// <summary>
        /// Gets the calling convention of the specified method.
        /// </summary>
        /// <param name="method">The method to get the calling convention for.</param>
        /// <returns>The calling convention of the specified method.</returns>
        CallingConventions GetCallingConvention(MethodBase method);

        /// <summary>
        /// Gets the method body of the specified method.
        /// </summary>
        /// <param name="method">The method to get the method body for.</param>
        /// <returns>The method body of the specified method.</returns>
        MethodBody GetMethodBody(MethodBase method);

        /// <summary>
        /// Gets a handle to the internal metadata representation of the specified method.
        /// </summary>
        /// <param name="method">The method to get a method handle for.</param>
        /// <returns>A <see cref="RuntimeMethodHandle" /> object.</returns>
        RuntimeMethodHandle GetMethodHandle(MethodBase method);

        /// <summary>
        /// Returns the <see cref="MethodImplAttributes" /> flags of the specified method.
        /// </summary>
        /// <param name="method">The method to get the implementation flags for.</param>
        /// <returns>The <see cref="MethodImplAttributes"/> flags.</returns>
        MethodImplAttributes GetMethodImplementationFlags(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        /// <param name="method">The method to check for generic parameters.</param>
        /// <returns>true if the specified <paramref name="method" /> object represents a generic method that contains unassigned generic type parameters; otherwise, false.</returns>
        bool ContainsGenericParameters(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the specified method is generic.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the specified <paramref name="method" /> represents a generic method; otherwise, false.</returns>
        bool IsGenericMethod(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the specified method is a generic method definition.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the specified <paramref name="method" /> represents a generic method definition; otherwise, false.</returns>
        bool IsGenericMethodDefinition(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the current method or constructor is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        bool IsSecurityCritical(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the method or constructor is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        bool IsSecuritySafeCritical(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the method or constructor is security-transparent at the current trust level; otherwise, false.</returns>
        bool IsSecurityTransparent(MethodBase method);

        /// <summary>
        /// Returns the <see cref="MethodInfo"/> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <param name="method">The method to get the base definition for.</param>
        /// <returns>A <see cref="MethodInfo"/> object for the first implementation of this method.</returns>
        MethodInfo GetBaseDefinition(MethodInfo method);

        /// <summary>
        /// Gets the parameters of the specified method or constructor.
        /// </summary>
        /// <param name="method">The method or constructor to get the parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) of the specified <see cref="MethodBase"/> instance.</returns>
        IReadOnlyList<ParameterInfo> GetParameters(MethodBase method);
    }
}
