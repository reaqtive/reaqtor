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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Reflection.Emit
{
    public class ILGenerator
    {
        private static readonly Action<object, OpCode> s_Emit;
        private static readonly Action<object, OpCode, Type> s_Emit_Type;
        private static readonly Action<object, OpCode, ConstructorInfo> s_Emit_ConstructorInfo;
        private static readonly Action<object, OpCode, FieldInfo> s_Emit_FieldInfo;
        private static readonly Action<object, OpCode, MethodInfo> s_Emit_MethodInfo;
        private static readonly Action<object, OpCode, long> s_Emit_Long;
        private static readonly Action<object, OpCode, int> s_Emit_Int;
        private static readonly Action<object, OpCode, string> s_Emit_String;
        private static readonly Action<object, OpCode, object> s_Emit_Label;
        private static readonly Action<object, OpCode, object> s_Emit_Local;
        private static readonly Func<object, Label> s_DefineLabel;
        private static readonly Action<object, object> s_MarkLabel;
        private static readonly Func<object, Type, LocalBuilder> s_DeclareLocal;

        private readonly object _ilgen;

        static ILGenerator()
        {
            var type_System_Reflection_Emit_ILGenerator = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.ILGenerator");
            var type_System_Reflection_Emit_Label = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.Label");
            var type_System_Reflection_Emit_LocalBuilder = typeof(OpCode).Assembly.GetType("System.Reflection.Emit.LocalBuilder");
            var method_System_Reflection_Emit_ILGenerator_Emit = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode) });
            var method_System_Reflection_Emit_ILGenerator_Emit_Type = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(Type) });
            var method_System_Reflection_Emit_ILGenerator_Emit_ConstructorInfo = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(ConstructorInfo) });
            var method_System_Reflection_Emit_ILGenerator_Emit_FieldInfo = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(FieldInfo) });
            var method_System_Reflection_Emit_ILGenerator_Emit_MethodInfo = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(MethodInfo) });
            var method_System_Reflection_Emit_ILGenerator_Emit_Long = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(long) });
            var method_System_Reflection_Emit_ILGenerator_Emit_Int = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(int) });
            var method_System_Reflection_Emit_ILGenerator_Emit_String = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), typeof(string) });
            var method_System_Reflection_Emit_ILGenerator_Emit_Label = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), type_System_Reflection_Emit_Label });
            var method_System_Reflection_Emit_ILGenerator_Emit_Local = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(Emit), new[] { typeof(OpCode), type_System_Reflection_Emit_LocalBuilder });
            var method_System_Reflection_Emit_ILGenerator_DefineLabel = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(DefineLabel), Type.EmptyTypes);
            var method_System_Reflection_Emit_ILGenerator_MarkLabel = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(MarkLabel), new[] { type_System_Reflection_Emit_Label });
            var method_System_Reflection_Emit_ILGenerator_DeclareLocal = type_System_Reflection_Emit_ILGenerator.GetMethod(nameof(DeclareLocal), new[] { typeof(Type) });

            var @this = Expression.Parameter(typeof(object), "this");
            var opcode = Expression.Parameter(typeof(OpCode), "opcode");
            var type = Expression.Parameter(typeof(Type), "type");
            var ctor = Expression.Parameter(typeof(ConstructorInfo), "ctor");
            var field = Expression.Parameter(typeof(FieldInfo), "field");
            var method = Expression.Parameter(typeof(MethodInfo), "method");
            var intValue = Expression.Parameter(typeof(int), "x");
            var longValue = Expression.Parameter(typeof(long), "x");
            var stringValue = Expression.Parameter(typeof(string), "str");
            var ilgen = Expression.Convert(@this, type_System_Reflection_Emit_ILGenerator);
            var label = Expression.Parameter(typeof(object), "label");
            var local = Expression.Parameter(typeof(object), "local");

            s_Emit = Expression.Lambda<Action<object, OpCode>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit,
                    opcode
                ),
                @this,
                opcode
            ).Compile();

            s_Emit_Type = Expression.Lambda<Action<object, OpCode, Type>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_Type,
                    opcode,
                    type
                ),
                @this,
                opcode,
                type
            ).Compile();

            s_Emit_ConstructorInfo = Expression.Lambda<Action<object, OpCode, ConstructorInfo>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_ConstructorInfo,
                    opcode,
                    ctor
                ),
                @this,
                opcode,
                ctor
            ).Compile();

            s_Emit_FieldInfo = Expression.Lambda<Action<object, OpCode, FieldInfo>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_FieldInfo,
                    opcode,
                    field
                ),
                @this,
                opcode,
                field
            ).Compile();

            s_Emit_MethodInfo = Expression.Lambda<Action<object, OpCode, MethodInfo>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_MethodInfo,
                    opcode,
                    method
                ),
                @this,
                opcode,
                method
            ).Compile();

            s_Emit_Long = Expression.Lambda<Action<object, OpCode, long>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_Long,
                    opcode,
                    longValue
                ),
                @this,
                opcode,
                longValue
            ).Compile();

            s_Emit_Int = Expression.Lambda<Action<object, OpCode, int>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_Int,
                    opcode,
                    intValue
                ),
                @this,
                opcode,
                intValue
            ).Compile();

            s_Emit_String = Expression.Lambda<Action<object, OpCode, string>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_String,
                    opcode,
                    stringValue
                ),
                @this,
                opcode,
                stringValue
            ).Compile();

            s_Emit_Label = Expression.Lambda<Action<object, OpCode, object>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_Label,
                    opcode,
                    Expression.Convert(label, type_System_Reflection_Emit_Label)
                ),
                @this,
                opcode,
                label
            ).Compile();

            s_Emit_Local = Expression.Lambda<Action<object, OpCode, object>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_Emit_Local,
                    opcode,
                    Expression.Convert(local, type_System_Reflection_Emit_LocalBuilder)
                ),
                @this,
                opcode,
                local
            ).Compile();

            s_DefineLabel = Expression.Lambda<Func<object, Label>>(
                Expression.New(
                    typeof(Label).GetConstructor(new[] { typeof(object) }),
                    Expression.Convert(
                        Expression.Call(
                            ilgen,
                            method_System_Reflection_Emit_ILGenerator_DefineLabel
                        ),
                        typeof(object) // NB: Label is a struct, so we need to box.
                    )
                ),
                @this
            ).Compile();

            s_MarkLabel = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                    ilgen,
                    method_System_Reflection_Emit_ILGenerator_MarkLabel,
                    Expression.Convert(label, type_System_Reflection_Emit_Label)
                ),
                @this,
                label
            ).Compile();

            s_DeclareLocal = Expression.Lambda<Func<object, Type, LocalBuilder>>(
                Expression.New(
                    typeof(LocalBuilder).GetConstructor(new[] { typeof(object) }),
                    Expression.Call(
                        ilgen,
                        method_System_Reflection_Emit_ILGenerator_DeclareLocal,
                        type
                    )
                ),
                @this,
                type
            ).Compile();
        }

        public ILGenerator(object ilgen) => _ilgen = ilgen;

        public void Emit(OpCode opcode) => s_Emit(_ilgen, opcode);
        public void Emit(OpCode opcode, Type type) => s_Emit_Type(_ilgen, opcode, type.Unwrap());
        public void Emit(OpCode opcode, ConstructorInfo info) => s_Emit_ConstructorInfo(_ilgen, opcode, info.Unwrap());
        public void Emit(OpCode opcode, FieldInfo info) => s_Emit_FieldInfo(_ilgen, opcode, info.Unwrap());
        public void Emit(OpCode opcode, MethodInfo info) => s_Emit_MethodInfo(_ilgen, opcode, info.Unwrap());
        public void Emit(OpCode opcode, long x) => s_Emit_Long(_ilgen, opcode, x);
        public void Emit(OpCode opcode, int x) => s_Emit_Int(_ilgen, opcode, x);
        public void Emit(OpCode opcode, string s) => s_Emit_String(_ilgen, opcode, s);
        public void Emit(OpCode opcode, Label label) => s_Emit_Label(_ilgen, opcode, label._label);
        public void Emit(OpCode opcode, LocalBuilder local) => s_Emit_Local(_ilgen, opcode, local._local);
        public LocalBuilder DeclareLocal(Type type) => s_DeclareLocal(_ilgen, type.Unwrap());
        public Label DefineLabel() => s_DefineLabel(_ilgen);
        public void MarkLabel(Label label) => s_MarkLabel(_ilgen, label._label);
    }
}

#endif
