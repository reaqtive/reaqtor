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

#if NETSTANDARD2_0

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CS1591 // XML docs missing for late bound accessor mirror image of System.Reflection.Emit functionality.
#pragma warning disable CA1062 // Null checks omitted; just a forwarder.
#pragma warning disable CA1810 // Initialization of static fields in static constructor.

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Reflection.Emit
{
    public class PropertyBuilder : PropertyInfo
    {
        private static readonly Action<object, object> s_SetSetMethod;
        private static readonly Action<object, object> s_SetGetMethod;
        private static readonly Action<object, object> s_SetCustomAttribute;

        internal readonly PropertyInfo _property;

        static PropertyBuilder()
        {
            var type_System_Reflection_Emit_MethodBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.MethodBuilder");
            var type_System_Reflection_Emit_PropertyBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.PropertyBuilder");
            var type_System_Reflection_Emit_CustomAttributeBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.CustomAttributeBuilder");
            var method_System_Reflection_Emit_PropertyBuilder_SetSetMethod = type_System_Reflection_Emit_PropertyBuilder.GetMethod(nameof(SetSetMethod), new[] { type_System_Reflection_Emit_MethodBuilder });
            var method_System_Reflection_Emit_PropertyBuilder_SetGetMethod = type_System_Reflection_Emit_PropertyBuilder.GetMethod(nameof(SetGetMethod), new[] { type_System_Reflection_Emit_MethodBuilder });
            var method_System_Reflection_Emit_PropertyBuilder_SetCustomAttribute = type_System_Reflection_Emit_PropertyBuilder.GetMethod(nameof(SetCustomAttribute), new[] { type_System_Reflection_Emit_CustomAttributeBuilder });

            var builder = Expression.Parameter(typeof(object), "this");
            var method = Expression.Parameter(typeof(object), "methodBuilder");
            var attribute = Expression.Parameter(typeof(object), "attribute");

            var propertyBuilder = Expression.Convert(builder, type_System_Reflection_Emit_PropertyBuilder);
            var methodBuilder = Expression.Convert(method, type_System_Reflection_Emit_MethodBuilder);
            var customAttributeBuilder = Expression.Convert(attribute, type_System_Reflection_Emit_CustomAttributeBuilder);

            s_SetSetMethod = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                    propertyBuilder,
                    method_System_Reflection_Emit_PropertyBuilder_SetSetMethod,
                    methodBuilder
                ),
                builder,
                method
            ).Compile();

            s_SetGetMethod = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                    propertyBuilder,
                    method_System_Reflection_Emit_PropertyBuilder_SetGetMethod,
                    methodBuilder
                ),
                builder,
                method
            ).Compile();

            s_SetCustomAttribute = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                    propertyBuilder,
                    method_System_Reflection_Emit_PropertyBuilder_SetCustomAttribute,
                    customAttributeBuilder
                ),
                builder,
                attribute
            ).Compile();
        }

        public PropertyBuilder(object builder) => _property = (PropertyInfo)builder;

        public override PropertyAttributes Attributes => _property.Attributes;

        public override bool CanRead => _property.CanRead;

        public override bool CanWrite => _property.CanWrite;

        public override Type PropertyType => _property.PropertyType.Unwrap();

        public override Type DeclaringType => _property.DeclaringType.Unwrap();

        public override string Name => _property.Name;

        public override Type ReflectedType => _property.ReflectedType.Unwrap();

        public override MethodInfo[] GetAccessors(bool nonPublic) => _property.GetAccessors(nonPublic); // REVIEW: Unwrap?

        public override MethodInfo GetGetMethod(bool nonPublic) => _property.GetGetMethod(nonPublic).Unwrap();

        public override ParameterInfo[] GetIndexParameters() => _property.GetIndexParameters(); // REVIEW: Unwrap?

        public override MethodInfo GetSetMethod(bool nonPublic) => _property.GetSetMethod(nonPublic).Unwrap();

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => _property.GetValue(obj, invokeAttr, binder, index, culture);

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => _property.SetValue(obj, value, invokeAttr, binder, index, culture);

        public override object[] GetCustomAttributes(bool inherit) => _property.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _property.GetCustomAttributes(attributeType.Unwrap(), inherit);

        public override bool IsDefined(Type attributeType, bool inherit) => _property.IsDefined(attributeType.Unwrap(), inherit);

        public void SetGetMethod(MethodBuilder builder) => s_SetGetMethod(_property, builder._method);
        public void SetSetMethod(MethodBuilder builder) => s_SetSetMethod(_property, builder._method);
        public void SetCustomAttribute(CustomAttributeBuilder builder) => s_SetCustomAttribute(_property, builder._builder);
    }
}

#endif
