// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to instantiate <see cref="Type"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface ITypeCreationProvider
    {
        /// <summary>
        /// Makes a single-dimensional (vector) array type with the specified element type.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <returns>A single-dimensional (vector) array type with the specified element type.</returns>
        Type MakeArrayType(Type elementType);

        /// <summary>
        /// Makes a multi-dimensional array type with the specified element type and rank.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <param name="rank">The rank of the multi-dimensional array.</param>
        /// <returns>A multi-dimensional array type with the specified element type and rank.</returns>
        Type MakeArrayType(Type elementType, int rank);

        /// <summary>
        /// Makes a by-ref type with the specified underlying element type.
        /// </summary>
        /// <param name="elementType">The underlying element type.</param>
        /// <returns>A by-ref type with the specified underlying element type.</returns>
        Type MakeByRefType(Type elementType);

        /// <summary>
        /// Makes a generic type with the specified generic type definition and type arguments.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic type with the specified generic type definition and type arguments.</returns>
        Type MakeGenericType(Type genericTypeDefinition, params Type[] typeArguments);

        /// <summary>
        /// Makes a pointer type with the specified underlying element type.
        /// </summary>
        /// <param name="elementType">The underlying element type.</param>
        /// <returns>A pointer type with the specified underlying element type.</returns>
        Type MakePointerType(Type elementType);
    }
}
