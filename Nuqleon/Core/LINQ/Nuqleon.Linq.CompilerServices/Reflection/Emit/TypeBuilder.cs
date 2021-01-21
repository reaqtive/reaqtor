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
#pragma warning disable CA1062 // Null checks omitted; just a forwarder.
#pragma warning disable CA1720 // Conflicts with reserved keywords; mirror image.
#pragma warning disable CA1810 // Initialization of static fields in static constructor.

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Reflection.Emit
{
    public class TypeBuilder : Type
    {
        private static readonly Func<Type, MethodInfo, MethodInfo> s_GetMethod;
        private static readonly Action<object, ConstructorInfo, byte[]> s_SetCustomAttribute;
        private static readonly Func<object, MethodAttributes, CallingConventions, Type[], ConstructorBuilder> s_DefineConstructor;
        private static readonly Func<object, string, Type, FieldAttributes, FieldBuilder> s_DefineField;
        private static readonly Func<object, string, MethodAttributes, Type, Type[], MethodBuilder> s_DefineMethod1;
        private static readonly Func<object, string, MethodAttributes, CallingConventions, Type, Type[], MethodBuilder> s_DefineMethod2;
        private static readonly Func<object, string, PropertyAttributes, Type, Type[], PropertyBuilder> s_DefineProperty;
        private static readonly Func<object, Type> s_CreateType;

        internal readonly Type _type;

        static TypeBuilder()
        {
            var type_System_Reflection_Emit_TypeBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.TypeBuilder");
            var method_System_Reflection_Emit_TypeBuilder_CreateType = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(CreateType), Type.EmptyTypes);
            var method_System_Reflection_Emit_TypeBuilder_GetMethod = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(GetMethod), new[] { typeof(Type), typeof(MethodInfo) });
            var method_System_Reflection_Emit_TypeBuilder_SetCustomAttribute = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(SetCustomAttribute), new[] { typeof(ConstructorInfo), typeof(byte[]) });
            var method_System_Reflection_Emit_TypeBuilder_DefineConstructor = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(DefineConstructor), new[] { typeof(MethodAttributes), typeof(CallingConventions), typeof(Type[]) });
            var method_System_Reflection_Emit_TypeBuilder_DefineField = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(DefineField), new[] { typeof(string), typeof(Type), typeof(FieldAttributes) });
            var method_System_Reflection_Emit_TypeBuilder_DefineMethod1 = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(DefineMethod), new[] { typeof(string), typeof(MethodAttributes), typeof(Type), typeof(Type[]) });
            var method_System_Reflection_Emit_TypeBuilder_DefineMethod2 = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(DefineMethod), new[] { typeof(string), typeof(MethodAttributes), typeof(CallingConventions), typeof(Type), typeof(Type[]) });
            var method_System_Reflection_Emit_TypeBuilder_DefineProperty = type_System_Reflection_Emit_TypeBuilder.GetMethod(nameof(DefineProperty), new[] { typeof(string), typeof(PropertyAttributes), typeof(Type), typeof(Type[]) });

            var builder = Expression.Parameter(typeof(object), "this");
            var typeBuilder = Expression.Convert(builder, type_System_Reflection_Emit_TypeBuilder);

            var name = Expression.Parameter(typeof(string), "name");
            var type = Expression.Parameter(typeof(Type), "type");
            var method = Expression.Parameter(typeof(MethodInfo), "method");
            var constructor = Expression.Parameter(typeof(ConstructorInfo), "constructor");
            var bytes = Expression.Parameter(typeof(byte[]), "bytes");
            var types = Expression.Parameter(typeof(Type[]), "types");
            var fieldAttributes = Expression.Parameter(typeof(FieldAttributes), "fieldAttributes");
            var methodAttributes = Expression.Parameter(typeof(MethodAttributes), "methodAttributes");
            var propertyAttributes = Expression.Parameter(typeof(PropertyAttributes), "propertyAttributes");
            var callingConventions = Expression.Parameter(typeof(CallingConventions), "callingConventions");

            s_GetMethod = Expression.Lambda<Func<Type, MethodInfo, MethodInfo>>(
                Expression.Call(
                    method_System_Reflection_Emit_TypeBuilder_GetMethod,
                    type,
                    method
                ),
                type,
                method
            ).Compile();

            s_SetCustomAttribute = Expression.Lambda<Action<object, ConstructorInfo, byte[]>>(
                Expression.Call(
                    typeBuilder,
                    method_System_Reflection_Emit_TypeBuilder_SetCustomAttribute,
                    constructor,
                    bytes
                ),
                builder,
                constructor,
                bytes
            ).Compile();

            s_DefineConstructor = Expression.Lambda<Func<object, MethodAttributes, CallingConventions, Type[], ConstructorBuilder>>(
                Expression.New(
                    typeof(ConstructorBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        typeBuilder,
                        method_System_Reflection_Emit_TypeBuilder_DefineConstructor,
                        methodAttributes,
                        callingConventions,
                        types
                    )
                ),
                builder,
                methodAttributes,
                callingConventions,
                types
            ).Compile();

            s_DefineField = Expression.Lambda<Func<object, string, Type, FieldAttributes, FieldBuilder>>(
                Expression.New(
                    typeof(FieldBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        typeBuilder,
                        method_System_Reflection_Emit_TypeBuilder_DefineField,
                        name,
                        type,
                        fieldAttributes
                    )
                ),
                builder,
                name,
                type,
                fieldAttributes
            ).Compile();

            s_DefineMethod1 = Expression.Lambda<Func<object, string, MethodAttributes, Type, Type[], MethodBuilder>>(
                Expression.New(
                    typeof(MethodBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        typeBuilder,
                        method_System_Reflection_Emit_TypeBuilder_DefineMethod1,
                        name,
                        methodAttributes,
                        type,
                        types
                    )
                ),
                builder,
                name,
                methodAttributes,
                type,
                types
            ).Compile();

            s_DefineMethod2 = Expression.Lambda<Func<object, string, MethodAttributes, CallingConventions, Type, Type[], MethodBuilder>>(
                Expression.New(
                    typeof(MethodBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        typeBuilder,
                        method_System_Reflection_Emit_TypeBuilder_DefineMethod2,
                        name,
                        methodAttributes,
                        callingConventions,
                        type,
                        types
                    )
                ),
                builder,
                name,
                methodAttributes,
                callingConventions,
                type,
                types
            ).Compile();

            s_DefineProperty = Expression.Lambda<Func<object, string, PropertyAttributes, Type, Type[], PropertyBuilder>>(
                Expression.New(
                    typeof(PropertyBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        typeBuilder,
                        method_System_Reflection_Emit_TypeBuilder_DefineProperty,
                        name,
                        propertyAttributes,
                        type,
                        types
                    )
                ),
                builder,
                name,
                propertyAttributes,
                type,
                types
            ).Compile();

            s_CreateType = Expression.Lambda<Func<object, Type>>(
                Expression.Call(
                    typeBuilder,
                    method_System_Reflection_Emit_TypeBuilder_CreateType
                ),
                builder
            ).Compile();
        }

        public TypeBuilder(object builder) => _type = (Type)builder;

        public override string AssemblyQualifiedName => _type.AssemblyQualifiedName;

        public override Type BaseType => _type.BaseType;

        public override string FullName => _type.FullName;

        public override Guid GUID => _type.GUID;

        public override Module Module => _type.Module;

        public override string Namespace => _type.Namespace;

        public override Type UnderlyingSystemType => _type.UnderlyingSystemType;

        public override string Name => _type.Name;

        public override Assembly Assembly => _type.Assembly;

        protected override TypeAttributes GetAttributeFlagsImpl() => _type.Attributes;

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => _type.GetConstructor(bindingAttr, binder, callConvention, types.Unwrap(), modifiers);

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => _type.GetConstructors(bindingAttr);

        public override Type GetElementType() => _type.GetElementType();

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => _type.GetEvent(name, bindingAttr);

        public override EventInfo[] GetEvents(BindingFlags bindingAttr) => _type.GetEvents(bindingAttr);

        public override FieldInfo GetField(string name, BindingFlags bindingAttr) => _type.GetField(name, bindingAttr);

        public override FieldInfo[] GetFields(BindingFlags bindingAttr) => _type.GetFields(bindingAttr);

        public override Type GetInterface(string name, bool ignoreCase) => _type.GetInterface(name, ignoreCase);

        public override Type[] GetInterfaces() => _type.GetInterfaces();

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => _type.GetMembers(bindingAttr);

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => _type.GetMethod(name, bindingAttr, binder, callConvention, types.Unwrap(), modifiers);

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => _type.GetMethods(bindingAttr);

        public override Type GetNestedType(string name, BindingFlags bindingAttr) => _type.GetNestedType(name, bindingAttr);

        public override Type[] GetNestedTypes(BindingFlags bindingAttr) => _type.GetNestedTypes(bindingAttr);

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => _type.GetProperties(bindingAttr);

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) => _type.GetProperty(name, bindingAttr, binder, returnType.Unwrap(), types.Unwrap(), modifiers);

        protected override bool HasElementTypeImpl() => _type.HasElementType;

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) => _type.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);

        protected override bool IsArrayImpl() => _type.IsArray;

        protected override bool IsByRefImpl() => _type.IsByRef;

        protected override bool IsCOMObjectImpl() => _type.IsCOMObject;

        protected override bool IsPointerImpl() => _type.IsPointer;

        protected override bool IsPrimitiveImpl() => _type.IsPrimitive;

        public override object[] GetCustomAttributes(bool inherit) => _type.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _type.GetCustomAttributes(attributeType.Unwrap(), inherit);

        public override bool IsDefined(Type attributeType, bool inherit) => _type.IsDefined(attributeType.Unwrap(), inherit);

        public static MethodInfo GetMethod(Type type, MethodInfo method) => s_GetMethod(type.Unwrap(), method.Unwrap());

        public void SetCustomAttribute(ConstructorInfo constructor, byte[] bytes) => s_SetCustomAttribute(_type, constructor.Unwrap(), bytes);
        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions conventions, Type[] parameters) => s_DefineConstructor(_type, attributes, conventions, parameters.Unwrap());
        public FieldBuilder DefineField(string name, Type type, FieldAttributes attributes) => s_DefineField(_type, name, type.Unwrap(), attributes);
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameters) => s_DefineMethod1(_type, name, attributes, returnType.Unwrap(), parameters.Unwrap());
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions conventions, Type returnType, Type[] parameters) => s_DefineMethod2(_type, name, attributes, conventions, returnType.Unwrap(), parameters.Unwrap());
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameters) => s_DefineProperty(_type, name, attributes, returnType.Unwrap(), parameters.Unwrap());
        public Type CreateType() => s_CreateType(_type);

        public override Type MakeArrayType() => _type.MakeArrayType();
        public override Type MakeArrayType(int rank) => _type.MakeArrayType(rank);
        public override Type MakeByRefType() => _type.MakeByRefType();
        public override Type MakePointerType() => _type.MakePointerType();
        public override Type MakeGenericType(params Type[] typeArguments) => _type.MakeGenericType(typeArguments.Unwrap());
    }
}
