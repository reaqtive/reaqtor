// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/06/2017 - Created this type.
//

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides a set of helpers methods to analyze types.
    /// </summary>
    public static class TypeHelpers
    {
        /// <summary>
        /// Gets the size of a reference based on the bitness of the process.
        /// </summary>
        public static readonly int IntPtrSize = IntPtr.Size;

        /// <summary>
        /// The generated <c>SizeOf{T}</c> method which uses IL instructions to obtain the size of a struct.
        /// </summary>
        private static readonly MethodInfo s_sizeof = BuildSizeOfHelper().GetMethod(nameof(SizeOf));

        /// <summary>
        /// Gets the size of instances of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type for which to get the size.</typeparam>
        /// <returns>The size of instances of the type.</returns>
        /// <remarks>
        /// Note that the value returned from this method does not provide any guarantees about the size of
        /// values of a struct type when they're embedded in other types, due to alignment and padding.
        /// </remarks>
        public static int SizeOf<T>() => SizeOf(typeof(T));

        /// <summary>
        /// Gets the size of instances of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for which to get the size.</param>
        /// <returns>The size of instances of the type.</returns>
        /// <remarks>
        /// Note that the value returned from this method does not provide any guarantees about the size of
        /// values of a struct type when they're embedded in other types, due to alignment and padding.
        /// </remarks>
        public static int SizeOf(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsValueType)
            {
                return (int)s_sizeof.MakeGenericMethod(type).Invoke(obj: null, parameters: null);
            }
            else if (type.IsArray)
            {
                var elementType = type.GetElementType();

                if (elementType.MakeArrayType() == type)
                {
                    return IntPtrSize * 2 + 4; // SyncBlock + MethodTable + Length
                }
                else
                {
                    var rank = type.GetArrayRank();

                    return IntPtrSize * 2 + rank * 4; // SyncBlock + MethodTable + Length for each dimension
                }
            }
            else
            {
                //
                // NB: The type handle of a runtime type refers to the MethodTable which stores the size
                //     of instances of the type in the second DWORD, thus at offset 4. This is the same
                //     for 32-bit and 64-bit processes.
                //

                return Marshal.ReadInt32(type.TypeHandle.Value, 4);
            }
        }

        /// <summary>
        /// Dynamically builds a type at runtime, providing a <c>SizeOf{T}</c> static method that returns
        /// the size of a struct using the <c>sizeof</c> IL instruction.
        /// </summary>
        /// <returns>A type containing a <c>int SizeOf{T}()</c> static method.</returns>
        private static Type BuildSizeOfHelper()
        {
            const string asmModName = "SizeOfHelper";

            //
            // Define a dynamic assembly and module only usable for execution of dynamic code at runtime.
            //

#if NETSTANDARD || NET5_0
            var asm = AssemblyBuilder.DefineDynamicAssembly(
#else
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(
#endif
                new AssemblyName(asmModName),
                AssemblyBuilderAccess.Run
            );

            var mod = asm.DefineDynamicModule(asmModName);

            //
            // Define a type to hold the helper method. Note we don't emit a global method because it's
            // tricky to make generic methods due to restrictions in the DefineGlobalMethod overloads.
            //

            var typ = mod.DefineType(nameof(SizeOf), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed);

            //
            // Define a static SizeOf{T} method with a single generic parameter. We'll check for valid
            // use in the TypeHelpers.SizeOf methods so we don't need to enforce any constraints here.
            //

            var mtd = typ.DefineMethod(nameof(SizeOf), MethodAttributes.Public | MethodAttributes.Static);
            mtd.SetReturnType(typeof(int));
            var ps = mtd.DefineGenericParameters("T");

            //
            // Generate the IL code that will effectively perform <c>sizeof !!0; ret;</c>.
            //

            var ilg = mtd.GetILGenerator();
            ilg.Emit(OpCodes.Sizeof, ps[0]);
            ilg.Emit(OpCodes.Ret);

            //
            // Compile and return the type.
            //

            return typ.CreateType();
        }
    }
}
