// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.CodeDom.Compiler;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to resolve runtime handles.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IReflectionHandlerResolver
    {
        /// <summary>
        /// Gets the type referenced by the specified type handle.
        /// </summary>
        /// <param name="handle">The object that refers to the type. </param>
        /// <returns>The type referenced by the specified <see cref="RuntimeTypeHandle" />, or null if the <see cref="RuntimeTypeHandle.Value" /> property of <paramref name="handle" /> is null.</returns>
        Type GetTypeFromHandle(RuntimeTypeHandle handle);

        /// <summary>
        /// Gets a <see cref="FieldInfo" /> for the field represented by the specified handle.
        /// </summary>
        /// <param name="handle">A <see cref="RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field. </param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field specified by <paramref name="handle" />.</returns>
        FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle);

        /// <summary>
        /// Gets a <see cref="FieldInfo" /> for the field represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A <see cref="RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field.</param>
        /// <param name="declaringType">A <see cref="RuntimeTypeHandle" /> structure that contains the handle to the generic type that defines the field.</param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
        FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType);

        /// <summary>
        /// Gets method information by using the method's internal metadata representation (handle).
        /// </summary>
        /// <param name="handle">The method's handle. </param>
        /// <returns>A <see cref="MethodBase"/> containing information about the method.</returns>
        MethodBase GetMethodFromHandle(RuntimeMethodHandle handle);

        /// <summary>
        /// Gets a <see cref="MethodBase" /> object for the constructor or method represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A handle to the internal metadata representation of a constructor or method.</param>
        /// <param name="declaringType">A handle to the generic type that defines the constructor or method.</param>
        /// <returns>A <see cref="MethodBase" /> object representing the method or constructor specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
        MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType);
    }
}
