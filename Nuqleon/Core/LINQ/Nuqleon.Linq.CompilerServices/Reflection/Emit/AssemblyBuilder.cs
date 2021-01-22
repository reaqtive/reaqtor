// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2018 - Created this file.
//

//
// NB: This file contains a late bound accessor for System.Reflection.Emit types which are unavailable on .NET Standard 2.0.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CS1591 // XML docs missing for late bound accessor mirror image of System.Reflection.Emit functionality.
#pragma warning disable CA1810 // Initialization of static fields in static constructor.

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Reflection.Emit
{
    public class AssemblyBuilder
    {
        private static readonly Func<AssemblyName, AssemblyBuilderAccess, AssemblyBuilder> s_DefineDynamicAssembly;
        private static readonly Func<object, string, ModuleBuilder> s_DefineDynamicModule;

        private readonly object _builder;

        static AssemblyBuilder()
        {
            var type_System_Reflection_Emit_AssemblyBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.AssemblyBuilder");
            var type_System_Reflection_Emit_AssemblyBuilderAccess = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.AssemblyBuilderAccess");
            var method_System_Reflection_Emit_AssemblyBuilder_DefineDynamicAssembly = type_System_Reflection_Emit_AssemblyBuilder.GetMethod(nameof(DefineDynamicAssembly), new[] { typeof(AssemblyName), type_System_Reflection_Emit_AssemblyBuilderAccess });

            var assemblyName = Expression.Parameter(typeof(AssemblyName), "name");
            var access = Expression.Parameter(typeof(AssemblyBuilderAccess), "access");

            s_DefineDynamicAssembly = Expression.Lambda<Func<AssemblyName, AssemblyBuilderAccess, AssemblyBuilder>>(
                Expression.New(
                    typeof(AssemblyBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        method_System_Reflection_Emit_AssemblyBuilder_DefineDynamicAssembly,
                        assemblyName,
                        Expression.Convert(Expression.Convert(access, typeof(int)), type_System_Reflection_Emit_AssemblyBuilderAccess)
                    )
                ),
                assemblyName,
                access
            ).Compile();

            var method_System_Reflection_Emit_AssemblyBuilder_DefineDynamicModule = type_System_Reflection_Emit_AssemblyBuilder.GetMethod(nameof(DefineDynamicModule), new[] { typeof(string) });

            var assemblyBuilder = Expression.Parameter(typeof(object), "this");
            var moduleName = Expression.Parameter(typeof(string), "name");

            s_DefineDynamicModule = Expression.Lambda<Func<object, string, ModuleBuilder>>(
                Expression.New(
                    typeof(ModuleBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        Expression.Convert(
                            assemblyBuilder,
                            type_System_Reflection_Emit_AssemblyBuilder
                        ),
                        method_System_Reflection_Emit_AssemblyBuilder_DefineDynamicModule,
                        moduleName
                    )
                ),
                assemblyBuilder,
                moduleName
            ).Compile();
        }

        public AssemblyBuilder(object builder) => _builder = builder;

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access) => s_DefineDynamicAssembly(name, access);

        public ModuleBuilder DefineDynamicModule(string name) => s_DefineDynamicModule(_builder, name);
    }
}
