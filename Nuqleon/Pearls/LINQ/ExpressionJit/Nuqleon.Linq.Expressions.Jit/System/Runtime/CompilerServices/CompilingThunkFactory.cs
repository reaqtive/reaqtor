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
    /// Thunk factory for thunks that compile expression trees.
    /// </summary>
    internal sealed class CompilingThunkFactory : ThunkFactoryBase
    {
        /// <summary>
        /// Creates a new thunk type (and related dispatcher and inner delegate types) for the specified delegate type.
        /// </summary>
        /// <param name="delegateType">The delegate for which create a thunk type.</param>
        /// <returns>A newly created thunk type.</returns>
        protected override Type CreateThunkType(Type delegateType)
        {
            return RuntimeTypeFactory.CreateThunkType(delegateType, ThunkBehavior.Compiling);
        }

        /// <summary>
        /// Tries to find a pre-compiled thunk type for the specified delegate type.
        /// </summary>
        /// <param name="delegateType">The delegate type to find a thunk type for.</param>
        /// <param name="thunkType">A thunk type for the specified delegate type.</param>
        /// <returns>true if a thunk type was found; otherwise, false.</returns>
        protected override bool TryGetPreCompiledThunk(Type delegateType, out Type thunkType)
        {
            return CompilingThunks.TypeMap.TryGetValue(delegateType, out thunkType);
        }
    }
}
