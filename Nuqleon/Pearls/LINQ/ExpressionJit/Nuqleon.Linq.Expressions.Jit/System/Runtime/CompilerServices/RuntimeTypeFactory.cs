// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Entry-point for runtime type generation.
/// </summary>
internal static partial class RuntimeTypeFactory
{
    /// <summary>
    /// The lock to protect against double-initialization of the assembly builder.
    /// </summary>
    private static readonly Lock s_lock = new();

    //
    // NB: Deals with the following exception:
    //
    //       System.InvalidOperationException: You cannot have more than one dynamic module in each dynamic assembly in this version of the runtime.
    //

    /// <summary>
    /// The module builder used to emit dynamically generated types.
    /// </summary>
    /// <remarks>
    /// The instance of the module builder is lazily created via the <see cref="Module"/> property.
    /// </remarks>

    /// <summary>
    /// Gets the module builder used to emit dynamically generated closure types.
    /// </summary>
    private static ModuleBuilder Module
    {
        get
        {
            if (field == null)
            {
                lock (s_lock)
                {
                    if (field == null)
                    {
                        var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("<>__ExpressionJit"), AssemblyBuilderAccess.RunAndCollect);
                        field = asm.DefineDynamicModule("RuntimeTypes");
                    }
                }
            }

            return field;
        }
    }

    /// <summary>
    /// Creates a new thunk type (and related dispatcher and inner delegate types) for the specified delegate type.
    /// </summary>
    /// <param name="delegateType">The delegate for which create a thunk type.</param>
    /// <param name="kind">The type of thunk to generate.</param>
    /// <returns>A newly created thunk type.</returns>
    /// <remarks>
    /// The caller is responsible to cache thunk types in order to avoid unnecessary runtime compilation cost.
    /// </remarks>
    public static Type CreateThunkType(Type delegateType, ThunkBehavior kind) => ThunkTypeCompiler.Create(delegateType, kind);

    /// <summary>
    /// Creates a new generic closure type with the specified arity.
    /// </summary>
    /// <param name="arity">The number of generic parameters and closure fields to generate.</param>
    /// <returns>A newly created generic closure type.</returns>
    /// <remarks>
    /// The caller is responsible to cache closure types in order to avoid unnecessary runtime compilation cost.
    /// </remarks>
    public static Type CreateClosureType(int arity) => ClosureTypeCompiler.Create(arity);
}
