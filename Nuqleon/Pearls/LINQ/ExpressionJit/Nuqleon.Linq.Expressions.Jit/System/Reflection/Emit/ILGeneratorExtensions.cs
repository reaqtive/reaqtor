// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Reflection.Emit
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="ILGenerator"/>
    /// </summary>
    internal static class ILGeneratorExtensions
    {
        /// <summary>
        /// Helper method to emit a `ldarg` instruction with the shortest opcode applicable.
        /// </summary>
        /// <param name="il">The IL generator to emit to.</param>
        /// <param name="index">The index of the argument to load.</param>
        public static void EmitLdarg(this ILGenerator il, int index)
        {
            switch (index)
            {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        il.Emit(OpCodes.Ldarg_S, (byte)index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }
    }
}
