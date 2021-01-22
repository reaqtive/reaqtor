// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Interface for thunk types factories.
    /// </summary>
    internal interface IThunkFactory
    {
        /// <summary>
        /// Gets the thunk type for the specified delegate type. If the delegate type is
        /// generic, it should be a closed generic type. The specified closure type will
        /// be used for the closure type parameter of the thunk type.
        /// </summary>
        /// <param name="delegateType">The delegate type to get a thunk type for.</param>
        /// <param name="closureType">The closure type to parameterize the thunk type on.</param>
        /// <returns>The thunk type, closed over the specified closure type.</returns>
        Type GetThunkType(Type delegateType, Type closureType);
    }
}
