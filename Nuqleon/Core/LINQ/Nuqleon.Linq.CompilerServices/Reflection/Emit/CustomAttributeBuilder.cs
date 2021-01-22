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
    public class CustomAttributeBuilder
    {
        private static readonly Func<ConstructorInfo, object[], object> s_ctor;

        internal object _builder;

        static CustomAttributeBuilder()
        {
            var type_System_Reflection_Emit_CustomAttributeBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.CustomAttributeBuilder");
            var ctor_System_Reflection_Emit_CustomAttributeBuilder = type_System_Reflection_Emit_CustomAttributeBuilder.GetConstructor(new[] { typeof(ConstructorInfo), typeof(object[]) });

            var ctor = Expression.Parameter(typeof(ConstructorInfo), "ctor");
            var data = Expression.Parameter(typeof(object[]), "data");

            s_ctor = Expression.Lambda<Func<ConstructorInfo, object[], object>>(
                Expression.New(
                    ctor_System_Reflection_Emit_CustomAttributeBuilder,
                    ctor,
                    data
                ),
                ctor,
                data
            ).Compile();
        }

        public CustomAttributeBuilder(ConstructorInfo constructor, object[] objs) => _builder = s_ctor(constructor.Unwrap(), objs);
    }
}
