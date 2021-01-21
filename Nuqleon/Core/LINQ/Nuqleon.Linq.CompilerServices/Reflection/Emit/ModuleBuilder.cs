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
    public class ModuleBuilder
    {
        private static readonly Func<object, string, TypeAttributes, TypeBuilder> s_DefineType;

        private readonly object _builder;

        static ModuleBuilder()
        {
            var type_System_Reflection_Emit_ModuleBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.ModuleBuilder");
            var method_System_Reflection_Emit_ModuleBuilder_DefineType = type_System_Reflection_Emit_ModuleBuilder.GetMethod(nameof(DefineType), new[] { typeof(string), typeof(TypeAttributes) });

            var builder = Expression.Parameter(typeof(object), "this");
            var name = Expression.Parameter(typeof(string), "name");
            var attributes = Expression.Parameter(typeof(TypeAttributes), "attributes");

            s_DefineType = Expression.Lambda<Func<object, string, TypeAttributes, TypeBuilder>>(
                Expression.New(
                    typeof(TypeBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        Expression.Convert(
                            builder,
                            type_System_Reflection_Emit_ModuleBuilder
                        ),
                        method_System_Reflection_Emit_ModuleBuilder_DefineType,
                        name,
                        attributes
                    )
                ),
                builder,
                name,
                attributes
            ).Compile();
        }

        public ModuleBuilder(object builder) => _builder = builder;

        public TypeBuilder DefineType(string name, TypeAttributes attributes) => s_DefineType(_builder, name, attributes);
    }
}
