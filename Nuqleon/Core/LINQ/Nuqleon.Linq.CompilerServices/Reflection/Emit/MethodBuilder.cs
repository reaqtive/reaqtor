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
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Reflection.Emit
{
    public class MethodBuilder : MethodInfo
    {
        private static readonly Func<object, ILGenerator> s_GetILGenerator;

        internal readonly MethodInfo _method;

        static MethodBuilder()
        {
            var type_System_Reflection_Emit_MethodBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.MethodBuilder");
            var method_System_Reflection_Emit_MethodBuilder_GetILGenerator = type_System_Reflection_Emit_MethodBuilder.GetMethod(nameof(GetILGenerator), Type.EmptyTypes);

            var builder = Expression.Parameter(typeof(object), "this");

            s_GetILGenerator = Expression.Lambda<Func<object, ILGenerator>>(
                Expression.New(
                    typeof(ILGenerator).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        Expression.Convert(builder, type_System_Reflection_Emit_MethodBuilder),
                        method_System_Reflection_Emit_MethodBuilder_GetILGenerator
                    )
                ),
                builder
            ).Compile();
        }

        public MethodBuilder(object builder) => _method = (MethodInfo)builder;

        public override ICustomAttributeProvider ReturnTypeCustomAttributes => _method.ReturnTypeCustomAttributes;

        public override MethodAttributes Attributes => _method.Attributes;

        public override RuntimeMethodHandle MethodHandle => _method.MethodHandle;

        public override Type DeclaringType => _method.DeclaringType.Unwrap();

        public override string Name => _method.Name;

        public override Type ReflectedType => _method.ReflectedType.Unwrap();

        public override MethodInfo GetBaseDefinition() => _method.GetBaseDefinition().Unwrap();

        public override object[] GetCustomAttributes(bool inherit) => _method.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _method.GetCustomAttributes(attributeType, inherit);

        public override MethodImplAttributes GetMethodImplementationFlags() => _method.MethodImplementationFlags;

        public override ParameterInfo[] GetParameters() => _method.GetParameters();

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => _method.Invoke(obj, invokeAttr, binder, parameters, culture);

        public override bool IsDefined(Type attributeType, bool inherit) => _method.IsDefined(attributeType.Unwrap(), inherit);

        public ILGenerator GetILGenerator() => s_GetILGenerator(_method);

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => _method.MakeGenericMethod(typeArguments.Unwrap());
    }
}
