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

namespace System.Runtime.CompilerServices
{
    internal partial class RuntimeTypeFactory
    {
        /// <summary>
        /// Implementation of closure type compilation support.
        /// </summary>
        private static class ClosureTypeCompiler
        {
            /// <summary>
            /// The lock to protect against double-initialization of the module builder.
            /// </summary>
            private static readonly object s_lock = new();

            /// <summary>
            /// The module builder used to emit dynamically generated types.
            /// </summary>
            /// <remarks>
            /// The instance of the module builder is lazily created via the <see cref="Module"/> property.
            /// </remarks>
            private static ModuleBuilder s_mod;

            /// <summary>
            /// Gets the module builder used to emit dynamically generated closure types.
            /// </summary>
            private static ModuleBuilder Module
            {
                get
                {
                    if (s_mod == null)
                    {
                        lock (s_lock)
                        {
                            if (s_mod == null)
                            {
                                s_mod = Assembly.DefineDynamicModule("Closures");
                            }
                        }
                    }

                    return s_mod;
                }
            }

            /// <summary>
            /// The type attributes to use for generated closure types.
            /// </summary>
            private const TypeAttributes ClosureTypeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;

            /// <summary>
            /// The method attributes to use for the getter and setter of the Item property.
            /// </summary>
            private const MethodAttributes ItemPropertyMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Final;

            /// <summary>
            /// The reflection object representing the constructor of ArgumentOutOfRangeException which takes a string parameter.
            /// </summary>
            private static readonly ConstructorInfo s_argumentOutOfRangeCtor = typeof(ArgumentOutOfRangeException).GetConstructor(new[] { typeof(string) });

            /// <summary>
            /// An array with a single element set to typeof(int), used for the Item indexer.
            /// </summary>
            private static readonly Type[] s_TypeArray_Int32 = new[] { typeof(int) };

            /// <summary>
            /// An array with two elements set to typeof(int) and typeof(object), used for the Item indexer.
            /// </summary>
            private static readonly Type[] s_TypeArray_Int32Object = new[] { typeof(int), typeof(object) };

            /// <summary>
            /// Creates a new generic closure type with the specified arity.
            /// </summary>
            /// <param name="arity">The number of generic parameters and closure fields to generate.</param>
            /// <returns>A newly created generic closure type.</returns>
            /// <remarks>
            /// The caller is responsible to cache closure types in order to avoid unnecessary runtime compilation cost.
            /// </remarks>
            public static Type Create(int arity)
            {
                //
                // First, define the class.
                //
                var builder = Module.DefineType("System.Runtime.CompilerServices.Closure`" + arity, ClosureTypeAttributes);

                //
                // Next, define the generic parameters using trivial Tn names,
                // without specifying any constraints.
                //
                // NB: Closures never deal with by-ref types and whatnot, so
                //     we don't have to worry about these things.
                //
                var genericParameterNames = new string[arity];

                for (var i = 0; i < arity; i++)
                {
                    genericParameterNames[i] = "T" + (i + 1);
                }

                var genericParameterTypes = builder.DefineGenericParameters(genericParameterNames);

                //
                // Declare the implementation of IRuntimeVariables which is used
                // by RuntimeVariables and Quote expressions to access a hoisted
                // local in the closure using an ordinal index.
                //
                builder.AddInterfaceImplementation(typeof(IRuntimeVariables));

                //
                // Implement IRuntimeVariables.Count in a trivial way by making
                // its getter return the arity of the closure type.
                //
                // NB: We don't have to worry about using short `ldc_i4` opcodes
                //     because we only generate closures with arity above 16.
                //
                var count = builder.DefineProperty("Count", PropertyAttributes.None, typeof(int), Type.EmptyTypes);

                var countGetter = builder.DefineMethod("get_Count", ItemPropertyMethodAttributes, typeof(int), Type.EmptyTypes);
                count.SetGetMethod(countGetter);

                var countGetterILGen = countGetter.GetILGenerator();

                countGetterILGen.Emit(OpCodes.Ldc_I4, arity);
                countGetterILGen.Emit(OpCodes.Ret);

                //
                // Implement IRuntimeVariables.Item by using `switch` statements
                // for the getter and setter implementation. From here on, we'll
                // emit the fields, the getter, and the setter hand-in-hand.
                //
                // 1. Declare the Item property.
                //
                var indexer = builder.DefineProperty("Item", PropertyAttributes.None, typeof(object), s_TypeArray_Int32);

                //
                // 2. Define the Item property get and set methods.
                //
                var indexerGetter = builder.DefineMethod("get_Item", ItemPropertyMethodAttributes, typeof(object), s_TypeArray_Int32);
                indexer.SetGetMethod(indexerGetter);

                var indexerSetter = builder.DefineMethod("set_Item", ItemPropertyMethodAttributes, typeof(void), s_TypeArray_Int32Object);
                indexer.SetSetMethod(indexerSetter);

                var indexerGetterILGen = indexerGetter.GetILGenerator();
                var indexerSetterILGen = indexerSetter.GetILGenerator();

                //
                // 3. Load the integer index passed to the indexer in arg 1.
                //
                indexerGetterILGen.Emit(OpCodes.Ldarg_1);
                indexerSetterILGen.Emit(OpCodes.Ldarg_1);

                //
                // 4. Define labels for the switch cases.
                //
                var indexerGetterLabels = new Label[arity];
                var indexerSetterLabels = new Label[arity];

                for (var i = 0; i < arity; i++)
                {
                    indexerGetterLabels[i] = indexerGetterILGen.DefineLabel();
                    indexerSetterLabels[i] = indexerSetterILGen.DefineLabel();
                }

                //
                // 5. Emit the switch instructions, followed by the fall-through
                //    code path that throws IndexOutOfRangeException.
                //
                indexerGetterILGen.Emit(OpCodes.Switch, indexerGetterLabels);
                indexerGetterILGen.Emit(OpCodes.Ldstr, "index");
                indexerGetterILGen.Emit(OpCodes.Newobj, s_argumentOutOfRangeCtor);
                indexerGetterILGen.Emit(OpCodes.Throw);

                indexerSetterILGen.Emit(OpCodes.Switch, indexerSetterLabels);
                indexerSetterILGen.Emit(OpCodes.Ldstr, "index");
                indexerSetterILGen.Emit(OpCodes.Newobj, s_argumentOutOfRangeCtor);
                indexerSetterILGen.Emit(OpCodes.Throw);

                //
                // 6. Define all the fields and their matching case in the Item
                //    getter and setter methods, using the labels generated.
                //
                for (var i = 0; i < arity; i++)
                {
                    //
                    // Emit public instance fields.
                    //
                    var type = genericParameterTypes[i];
                    var field = builder.DefineField("Item" + (i + 1), type, FieldAttributes.Public);

                    //
                    // For the getter, load the field and box it to convert to
                    // object, as required by the Item indexer.
                    //
                    // NB: The generic parameters are unconstrained, so we need
                    //     to emit the `box` instruction. The JIT will omit this
                    //     when closing the generic type of value types.
                    //
                    indexerGetterILGen.MarkLabel(indexerGetterLabels[i]);
                    indexerGetterILGen.Emit(OpCodes.Ldarg_0);
                    indexerGetterILGen.Emit(OpCodes.Ldfld, field);
                    indexerGetterILGen.Emit(OpCodes.Box, type);
                    indexerGetterILGen.Emit(OpCodes.Ret);

                    //
                    // For the setter, load the `value` argument passed to the
                    // setter and unbox it to store in the field.
                    //
                    // NB: InvalidCastException will be thrown when the type of
                    //     the object passed to the setter can't be converted to
                    //     the field type. This is by design.
                    //
                    indexerSetterILGen.MarkLabel(indexerSetterLabels[i]);
                    indexerSetterILGen.Emit(OpCodes.Ldarg_0);
                    indexerSetterILGen.Emit(OpCodes.Ldarg_2);
                    indexerSetterILGen.Emit(OpCodes.Unbox_Any, type);
                    indexerSetterILGen.Emit(OpCodes.Stfld, field);
                    indexerSetterILGen.Emit(OpCodes.Ret);
                }

                //
                // Create the type and return.
                //
                return builder.CreateTypeInfo().AsType();
            }
        }
    }
}
