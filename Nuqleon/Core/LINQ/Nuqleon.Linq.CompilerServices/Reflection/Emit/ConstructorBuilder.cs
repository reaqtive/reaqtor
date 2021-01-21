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
    public class ConstructorBuilder : ConstructorInfo
    {
        private static readonly Func<object, ILGenerator> s_GetILGenerator;
        private static readonly Action<object, int, ParameterAttributes, string> s_DefineParameter;

        internal readonly ConstructorInfo _ctor;

        static ConstructorBuilder()
        {
            var type_System_Reflection_Emit_ConstructorBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.ConstructorBuilder");
            var method_System_Reflection_Emit_ConstructorBuilder_GetILGenerator = type_System_Reflection_Emit_ConstructorBuilder.GetMethod(nameof(GetILGenerator), Type.EmptyTypes);
            var method_System_Reflection_Emit_ConstructorBuilder_DefineParameter = type_System_Reflection_Emit_ConstructorBuilder.GetMethod(nameof(DefineParameter), new[] { typeof(int), typeof(ParameterAttributes), typeof(string) });

            var builder = Expression.Parameter(typeof(object), "this");

            s_GetILGenerator = Expression.Lambda<Func<object, ILGenerator>>(
                Expression.New(
                    typeof(ILGenerator).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        Expression.Convert(builder, type_System_Reflection_Emit_ConstructorBuilder),
                        method_System_Reflection_Emit_ConstructorBuilder_GetILGenerator
                    )
                ),
                builder
            ).Compile();

            var index = Expression.Parameter(typeof(int), "index");
            var attributes = Expression.Parameter(typeof(ParameterAttributes), "attributes");
            var name = Expression.Parameter(typeof(string), "name");

            s_DefineParameter = Expression.Lambda<Action<object, int, ParameterAttributes, string>>(
                Expression.Call(
                    Expression.Convert(builder, type_System_Reflection_Emit_ConstructorBuilder),
                    method_System_Reflection_Emit_ConstructorBuilder_DefineParameter,
                    index,
                    attributes,
                    name
                ),
                builder,
                index,
                attributes,
                name
            ).Compile();
        }

        public ConstructorBuilder(object builder) => _ctor = (ConstructorInfo)builder;

        public override MethodAttributes Attributes => _ctor.Attributes;

        public override RuntimeMethodHandle MethodHandle => _ctor.MethodHandle;

        public override Type DeclaringType => _ctor.DeclaringType.Unwrap();

        public override string Name => _ctor.Name;

        public override Type ReflectedType => _ctor.ReflectedType.Unwrap();

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => _ctor.Invoke(invokeAttr, binder, parameters, culture);

        public override MethodImplAttributes GetMethodImplementationFlags() => _ctor.MethodImplementationFlags;

        public override ParameterInfo[] GetParameters() => _ctor.GetParameters();

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => _ctor.Invoke(obj, invokeAttr, binder, parameters, culture);

        public override object[] GetCustomAttributes(bool inherit) => _ctor.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _ctor.GetCustomAttributes(attributeType.Unwrap(), inherit);

        public override bool IsDefined(Type attributeType, bool inherit) => _ctor.IsDefined(attributeType.Unwrap(), inherit);

        public ILGenerator GetILGenerator() => s_GetILGenerator(_ctor);

        public void DefineParameter(int index, ParameterAttributes attributes, string name) => s_DefineParameter(_ctor, index, attributes, name);
    }
}
