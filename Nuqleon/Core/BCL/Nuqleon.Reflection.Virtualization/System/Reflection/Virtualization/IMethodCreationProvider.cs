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
    /// Interface representing a reflection provider used to instantiate <see cref="MethodInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IMethodCreationProvider
    {
        /// <summary>
        /// Makes a generic method with the specified generic method definition and type arguments.
        /// </summary>
        /// <param name="genericMethodDefinition">The generic method definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic method with the specified generic method definition and type arguments.</returns>
        MethodInfo MakeGenericMethod(MethodInfo genericMethodDefinition, params Type[] typeArguments);
    }
}
