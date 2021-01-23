// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Runtime.CompilerServices
{
    internal partial class RuntimeTypeFactory
    {
        /// <summary>
        /// Implementation of thunk type compilation support.
        /// </summary>
        private static class ThunkTypeCompiler
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
            /// Counter to generate unique type names with an integer suffix.
            /// </summary>
            private static int s_count;

            /// <summary>
            /// The type attributes to use for generated thunk types.
            /// </summary>
            private const TypeAttributes ThunkTypeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;

            /// <summary>
            /// The type attributes to use for generated dispatcher types.
            /// </summary>
            private const TypeAttributes DispatcherTypeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;

            /// <summary>
            /// The type attributes to use for generated inner delegate types.
            /// </summary>
            private const TypeAttributes InnerDelegateTypeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;

            /// <summary>
            /// The type attributes to use for generated tiered recompilation types.
            /// </summary>
            private const TypeAttributes TieredRecompilationTypeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass;

            /// <summary>
            /// The types of the parameters of a delegate type's instance constructor.
            /// </summary>
            private static readonly Type[] s_delegateCtorParameterTypes = new[] { typeof(object), typeof(IntPtr) };

            /// <summary>
            /// The constructor of the open generic Thunk base class, taking in the Expression{TInner}.
            /// </summary>
            private static readonly ConstructorInfo s_thunkCtor = typeof(Thunk<,,>).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)[0];

            /// <summary>
            /// The Target field on the open generic Thunk base class, holding the delegate of type TInner.
            /// </summary>
            private static readonly FieldInfo s_thunkTarget = typeof(Thunk<,,>).GetField("Target");

            /// <summary>
            /// The reflection object representing Monitor.Enter(object, out bool) used to acquire a lock.
            /// </summary>
            private static readonly MethodInfo s_monitorEnter = typeof(Monitor).GetMethod(nameof(Monitor.Enter), new[] { typeof(object), typeof(bool).MakeByRefType() });

            /// <summary>
            /// The reflection object representing Monitor.Exit(object) used to release a lock.
            /// </summary>
            private static readonly MethodInfo s_monitorExit = typeof(Monitor).GetMethod(nameof(Monitor.Exit), new[] { typeof(object) });

            /// <summary>
            /// The Expression field on the Thunk base class, of type Expression{TInner}.
            /// </summary>
            private static readonly FieldInfo s_thunkExpression = typeof(Thunk<,,>).GetField("Expression");

            /// <summary>
            /// The Compile method on the Expression{TDelegate} class.
            /// </summary>
            private static readonly MethodInfo s_expressionCompile = typeof(Expression<>).GetMethod("Compile", Type.EmptyTypes);

            /// <summary>
            /// The Compile(bool) method on the Expression{TDelegate} class.
            /// </summary>
            private static readonly MethodInfo s_expressionCompileBool = typeof(Expression<>).GetMethod("Compile", new[] { typeof(bool) });

            /// <summary>
            /// The constructor of the open generic Dispatcher base class, taking no parameters.
            /// </summary>
            private static readonly ConstructorInfo s_dispatcherCtor = typeof(Dispatcher<,,>).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)[0];

            /// <summary>
            /// The Parent field on the Dispatcher base class, of type Thunk{...}.
            /// </summary>
            private static readonly FieldInfo s_dispatcherParent = typeof(Dispatcher<,,>).GetField("Parent");

            /// <summary>
            /// The Closure field on the FunctionContext grandparent class, of type TClosure.
            /// </summary>
            private static readonly FieldInfo s_contextClosure = typeof(FunctionContext<>).GetField("Closure");

            /// <summary>
            /// The Increment(ref int) method on the Interlocked class.
            /// </summary>
            private static readonly MethodInfo s_interlockedIncrement = typeof(Interlocked).GetMethod(nameof(Interlocked.Increment), new[] { typeof(int).MakeByRefType() });

            /// <summary>
            /// Gets the module builder used to emit dynamically generated thunk (and related) types.
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
                                s_mod = Assembly.DefineDynamicModule("Thunks");
                            }
                        }
                    }

                    return s_mod;
                }
            }

            /// <summary>
            /// Creates a custom thunk type (and necessary related dispatcher and delegate types) for the specified delegate type.
            /// </summary>
            /// <param name="delegateType">The delegate type to create a thunk type for.</param>
            /// <param name="kind">The type of thunk to generate.</param>
            /// <returns>The thunk type for the specified delegate, which can be instantiated using an expression tree argument passed to the constructor.</returns>
            /// <remarks>
            /// * If the specified delegate type is generic, it should be an open generic type. It's the caller's responsibility to close the generic type.
            /// * Custom thunk types are required when no built-in thunk types for common delegate types are available. The caller is responsible to cache these runtime generated types.
            /// </remarks>
            public static Type Create(Type delegateType, ThunkBehavior kind)
            {
                Debug.Assert(delegateType != null, "Expected a non-null delegate type.");
                Debug.Assert(delegateType.GetTypeInfo().IsSubclassOf(typeof(Delegate)), "Expected a delegate type that derives from System.Delegate.");

                //
                // Get a prefix for the generated types. We'll escape special characters that
                // could upset type name parsers. Note this code runs infrequently enough, so
                // we don't have to worry too much about string allocations from Replace.
                //
                var name = delegateType.Name.Replace('+', '_').Replace('`', '_');
                var prefix = "System.Runtime.CompilerServices.<" + name + ">__";

                //
                // Code generation can differ based on whether the input type is generic or
                // not, so we'll get the generic arguments and represent a non-generic input
                // by having no generic arguments.
                //
                var genericArguments = Type.EmptyTypes;
                if (delegateType.IsGenericType)
                {
                    Debug.Assert(delegateType.IsGenericTypeDefinition, "Expected a generic delegate type.");
                    genericArguments = delegateType.GetGenericArguments();
                }

                //
                // Also get a unique ID which we'll use as the suffix for our type names; at
                // this point, we simply will use an integer and rely on the uniqueness of
                // prefixes to have sufficiently unique names.
                //
                // In order to generate type names that are conventional, we also add the `
                // syntax for generic arity. Note that the arity of the generated types is
                // one more than the original type, to accommodate for the closure parameter.
                //
                var id = GetUniqueId().ToString(CultureInfo.InvariantCulture);
                var arity = genericArguments.Length + 1;
                var suffix = id + "`" + arity;

                //
                // We'll generate custom types, so first ensure we have the module builder set
                // up to emit those types into.
                //
                var module = Module;

                //
                // We need to generate three types that comprise a thunk. To simplify matters,
                // generate all of these hand-in-hand.
                //
                // CONSIDER: There's a possibility we could reuse Func<> and Action<> types
                //           for the inner delegate type. For now, we'll generate a delegate
                //           type for each custom thunk and satisfy ourselves by the thought
                //           that custom thunks should be rare. We need the code to generate
                //           custom delegate types anyway, so using existing types is merely
                //           an optimization we can consider later.
                //
                // NB: We'll set parent types explicitly later, because we need to instantiate
                //     generic types using newly created generic type parameters first.
                //
                var thunkTypeBuilder = module.DefineType(prefix + "Thunk" + suffix, ThunkTypeAttributes);
                var dispatcherTypeBuilder = module.DefineType(prefix + "Dispatcher" + suffix, DispatcherTypeAttributes);
                var innerDelegateTypeBuilder = module.DefineType(prefix + "Delegate" + suffix, InnerDelegateTypeAttributes);

                //
                // All types will have a first generic parameter for the closure, followed
                // by zero or more generic parameters that we "inherit" from the delegate
                // type that's passed in. We'll clone the names of these parameters first.
                //
                var genericParameterNames = new string[genericArguments.Length + 1];

                var closureParameterName = "TClosure";
                genericParameterNames[0] = closureParameterName;

                for (var i = 0; i < genericArguments.Length; i++)
                {
                    genericParameterNames[i + 1] = genericArguments[i].Name;
                }

                //
                // Now make all of our types generic on these parameters. We use a helper
                // method to clone generic constraints, if any.
                //
                var thunkGenericParameters = DefineGenericParameters(thunkTypeBuilder, genericArguments, genericParameterNames);
                var dispatcherGenericParameters = DefineGenericParameters(dispatcherTypeBuilder, genericArguments, genericParameterNames);
                var innerDelegateGenericParameters = DefineGenericParameters(innerDelegateTypeBuilder, genericArguments, genericParameterNames);

                //
                // Bundle up all information to make it easier to pass around.
                //
                var info = new ThunkTypesInfo
                {
                    DelegateType = delegateType,
                    ThunkType = thunkTypeBuilder,
                    ThunkTypeGenericParameters = thunkGenericParameters,
                    DispatcherType = dispatcherTypeBuilder,
                    DispatcherTypeGenericParameters = dispatcherGenericParameters,
                    InnerDelegateType = innerDelegateTypeBuilder,
                    InnerDelegateTypeGenericParameters = innerDelegateGenericParameters,
                };

                //
                // For tiered recompilation, we need an additional helper type.
                //
                if (kind == ThunkBehavior.TieredCompilation)
                {
                    var tieredRecompilationType = module.DefineType(prefix + "Recompile" + suffix, TieredRecompilationTypeAttributes);
                    var tieredRecompliationGenericParameters = DefineGenericParameters(tieredRecompilationType, genericArguments, genericParameterNames);

                    info.TieredRecompilationType = tieredRecompilationType;
                    info.TieredRecompilationTypeGenericParameters = tieredRecompliationGenericParameters;
                }

                //
                // Next, define the types using helpers methods. This does not yet emit
                // IL code because we may need references to members across these types,
                // so we first generate the builders for all members.
                //
                var thunkTypeDefinition = DefineThunkType(info);
                var dispatcherTypeDefinition = DefineDispatcherType(info);
                var innerDelegateTypeDefinition = DefineInnerDelegateType(info);

                //
                // Bundle all information for convenience and to allow for lookup of
                // members across all the types.
                //
                var compilationInfo = new CompilationInfo
                {
                    Types = info,
                    ThunkDefinition = thunkTypeDefinition,
                    DispatcherDefinition = dispatcherTypeDefinition,
                    InnerDelegateDefinition = innerDelegateTypeDefinition,
                    Kind = kind,
                };

                //
                // For tiered recompilation, we need to define the recompiliation helper type as well.
                //
                if (kind == ThunkBehavior.TieredCompilation)
                {
                    var tieredRecompilationTypeDefinition = DefineTieredRecompilationType(info);
                    compilationInfo.TieredRecompilationDefinition = tieredRecompilationTypeDefinition;
                }

                //
                // Next, we can generate IL code for the members on all these types,
                // using the references to other members as needed.
                //
                // NB: We don't have to worry about the inner delegate type because
                //     its methods are markes as being implemented by the runtime.
                //
                BuildThunkType(compilationInfo);
                BuildDispatcherType(compilationInfo);

                //
                // For tiered recompilation, we need to build the recompiliation helper type as well.
                //
                if (kind == ThunkBehavior.TieredCompilation)
                {
                    BuildTieredRecompilationType(compilationInfo);
                }

                //
                // Build all the types; we only need the thunk type to return to our
                // caller.
                //
                info.InnerDelegateType.CreateTypeInfo();
                info.DispatcherType.CreateTypeInfo();
                info.TieredRecompilationType?.CreateTypeInfo();
                var result = info.ThunkType.CreateTypeInfo().AsType();

                //
                // Return the thunk type. Note that open generic types are expected in
                // case the input was a generic type. The caller can close them.
                //
                return result;
            }

            /// <summary>
            /// Struct containing information about the thunk types being generated;
            /// used to be able to build cross-references between these types and
            /// their members during code generation.
            /// </summary>
            private struct CompilationInfo
            {
                /// <summary>
                /// Information about the types being built.
                /// </summary>
                public ThunkTypesInfo Types;

                /// <summary>
                /// Information about the members on the thunk type.
                /// </summary>
                public ThunkTypeDefinition ThunkDefinition;

                /// <summary>
                /// Information about the members on the dispatcher type.
                /// </summary>
                public DispatcherTypeDefinition DispatcherDefinition;

                /// <summary>
                /// Information about the members on the inner delegate type.
                /// </summary>
                public InnerDelegateTypeDefinition InnerDelegateDefinition;

                /// <summary>
                /// Information about the members on the tiered recompilation type.
                /// </summary>
                public TieredRecompilationTypeDefinition TieredRecompilationDefinition;

                /// <summary>
                /// Information about the desired behavior of the thunk.
                /// </summary>
                public ThunkBehavior Kind;
            }

            /// <summary>
            /// Struct containing information about the thunk types being generated;
            /// used to be able to build cross-references between these types.
            /// </summary>
            private struct ThunkTypesInfo
            {
                /// <summary>
                /// The delegate type for which we're building thunk types.
                /// </summary>
                public Type DelegateType;

                /// <summary>
                /// The TypeBuilder for the thunk type.
                /// </summary>
                public TypeBuilder ThunkType;

                /// <summary>
                /// The generic type parameters of the thunk type.
                /// </summary>
                public Type[] ThunkTypeGenericParameters;

                /// <summary>
                /// The TypeBuilder for the dispatcher type.
                /// </summary>
                public TypeBuilder DispatcherType;

                /// <summary>
                /// The generic type parameters of the dispatcher type.
                /// </summary>
                public Type[] DispatcherTypeGenericParameters;

                /// <summary>
                /// The TypeBuilder for the inner delegate type.
                /// </summary>
                public TypeBuilder InnerDelegateType;

                /// <summary>
                /// The generic type parameters of the inner delegate type.
                /// </summary>
                public Type[] InnerDelegateTypeGenericParameters;

                /// <summary>
                /// The TypeBuilder for the tiered recompilation type.
                /// </summary>
                public TypeBuilder TieredRecompilationType;

                /// <summary>
                /// The generic type parameters of the tiered recompilation type.
                /// </summary>
                public Type[] TieredRecompilationTypeGenericParameters;
            }

            /// <summary>
            /// Defines generic type parameters on the specified type builder using the specified generic arguments of the
            /// original delegate type to copy generic parameter constraints from. An additional first type parameter for
            /// the closure will be added without any constraints.
            /// </summary>
            /// <param name="builder">The type builder to define the generic parameters on.</param>
            /// <param name="genericArguments">The generic arumgents to clone the generic constraints from.</param>
            /// <param name="names">The names of the generic parameters to define. This array should have a length that's one longer than the arguments to account for the first closure parameter.</param>
            /// <returns>An array containing the generic type parameters that were defined on the type builder.</returns>
            private static Type[] DefineGenericParameters(TypeBuilder builder, Type[] genericArguments, string[] names)
            {
                Debug.Assert(names.Length >= 1, "Expected at least a closure type parameter.");
                Debug.Assert(genericArguments.Length + 1 == names.Length, "Expected an additional generic parameter for the closure.");

                //
                // Generate the generic parameters using the given names.
                //
                var genericTypeParameterBuilders = builder.DefineGenericParameters(names);

                //
                // We may need a type substitutor at some point while executing the loop below;
                // see remarks on the type constraint cloning logic below. We don't need it if
                // we don't encounter any constraints, so allocate it lazily when needed.
                //
                var substGenericParameters = default(TypeSubstitutor);

                //
                // Next, clone constraints on generic parameters, if any.
                //
                for (var i = 0; i < genericArguments.Length; i++)
                {
                    //
                    // Get the incoming generic argument and the generic parameter type builder
                    // that corresponds to it, skipping over the closure parameter.
                    //
                    var genericArgument = genericArguments[i];
                    var genericParameter = genericTypeParameterBuilders[i + 1];

                    //
                    // First, copy special constraints, i.e. class, struct, or new().
                    //
                    var constraints = genericArgument.GenericParameterAttributes & GenericParameterAttributes.SpecialConstraintMask;
                    var attributes = constraints;

                    if (Type.GetType("Mono.Runtime") == null)
                    {
                        // NB: The following throws on Mono.
                        attributes |= genericParameter.GenericParameterAttributes;
                    }

                    genericParameter.SetGenericParameterAttributes(attributes);

                    //
                    // Next, analyze any interface or base class constraints. This is a bit
                    // more involved because constraints on the original type may refer to
                    // its open generic parameters, e.g.
                    //
                    //   delegate R F<T1, T2, R>(T1 t1, T2 t2) where T1 : R where T2 : G<T1>
                    //
                    var typeConstraints = genericArgument.GetGenericParameterConstraints();

                    if (typeConstraints.Length > 0)
                    {
                        //
                        // First, create a type substitutor that can rewrite the types that
                        // occur in constraints. We'll substitute references to the original
                        // generic parameters for the newly created parameters. Note we only
                        // allocate this if needed.
                        //
                        if (substGenericParameters == null)
                        {
                            var substitutionMap = new Dictionary<Type, Type>(genericArguments.Length);
                            for (var j = 0; j < genericArguments.Length; j++)
                            {
                                substitutionMap.Add(genericArguments[j], genericTypeParameterBuilders[j + 1]);
                            }

                            substGenericParameters = new TypeSubstitutor(substitutionMap);
                        }

                        //
                        // There's no particular order in which constraints will be found
                        // in the array, so we keep two locals. If null, we didn't find
                        // the corresponding constraint(s).
                        //
                        var baseTypeConstraint = default(Type);
                        var interfaceConstraints = default(List<Type>);

                        for (var j = 0; j < typeConstraints.Length; j++)
                        {
                            var typeConstraint = typeConstraints[j];

                            //
                            // Rewrite the type constraint to refer to the newly created
                            // generic type parameters, e.g. for T1 in the example:
                            //
                            //   delegate R F<T1, T2, R>(T1 t1, T2 t2) where T1 : R where T2 : G<T1>
                            //
                            //   class Thunk<TClosure, T1, T2, R> where T1 : R where T2 : G<T1>
                            //                         |                ^                   ^
                            //                         |                | (ref eq)          | (rewrite)
                            //                         +----------------+-------------------+
                            //
                            //
                            var rewrittenTypeConstraint = substGenericParameters.Visit(typeConstraint);

                            //
                            // IsClass indicates a base type constraint. We should at
                            // most one such constraint.
                            //
                            if (typeConstraint.IsClass)
                            {
                                Debug.Assert(baseTypeConstraint == null, "Expected only one base class constraint.");
                                baseTypeConstraint = rewrittenTypeConstraint;
                            }
                            else
                            {
                                //
                                // Lazily allocate the interface constraints list,
                                // which we'll turn into an array later.
                                //
                                interfaceConstraints ??= new List<Type>(typeConstraints.Length);
                                interfaceConstraints.Add(rewrittenTypeConstraint);
                            }
                        }

                        //
                        // Set base type constraint, if any.
                        //
                        if (baseTypeConstraint != null)
                        {
                            genericParameter.SetBaseTypeConstraint(baseTypeConstraint);
                        }

                        //
                        // Set interface constraints, if any.
                        //
                        if (interfaceConstraints != null)
                        {
                            genericParameter.SetInterfaceConstraints(interfaceConstraints.ToArray());
                        }
                    }
                }

                //
                // Return the type builders, which inherit from System.Type, so we can
                // use array covariance here to match the return type.
                //
                return genericTypeParameterBuilders.Map(/* no closure */ t => t);
            }

            /// <summary>
            /// Defines the members on the thunk type without performing IL code emission.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types used to create cross-type references.</param>
            /// <returns>An object containing information about the defined members on the thunk type.</returns>
            private static ThunkTypeDefinition DefineThunkType(ThunkTypesInfo info)
            {
                var delegateType = info.DelegateType;
                var builder = info.ThunkType;
                var genericParameters = info.ThunkTypeGenericParameters;
                var innerDelegateType = info.InnerDelegateType;

                //
                // First, set the base class to Thunk<TDelegate, TClosure, TInner> by
                // closing the delegate and inner delegate types as needed.
                //
                var closedDelegateType = delegateType;
                var genericParametersWithoutClosure = genericParameters.RemoveFirst();

                if (genericParametersWithoutClosure.Length > 0)
                {
                    closedDelegateType = delegateType.MakeGenericType(genericParametersWithoutClosure);
                }

                var closureType = genericParameters[0];
                var closedInnerDelegateType = innerDelegateType.MakeGenericType(genericParameters);
                var parentType = typeof(Thunk<,,>).MakeGenericType(closedDelegateType, closureType, closedInnerDelegateType);
                builder.SetParent(parentType);

                //
                // Next, define all the members upfront so we can refer to them from
                // each other during IL generation.
                //
                // 1. The constructor `.ctor(Expression<TInner>)`
                //
                var expressionOfInnerDelegateType = typeof(Expression<>).MakeGenericType(closedInnerDelegateType);
                var ctorParameterTypes = new[] { expressionOfInnerDelegateType };
                var ctor = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, ctorParameterTypes);

                //
                // 2. The Compiler property and its getter `TInner Compiler { get; }`
                //
                var propCompiler = builder.DefineProperty("Compiler", PropertyAttributes.None, closedInnerDelegateType, Type.EmptyTypes);
                var mtdCompilerGetter = builder.DefineMethod("get_Compiler", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, closedInnerDelegateType, Type.EmptyTypes);
                propCompiler.SetGetMethod(mtdCompilerGetter);

                //
                // 3. The CompilerCore property and its getter `TInner CompilerCore { get; }`
                //
                var propCompilerCore = builder.DefineProperty("CompilerCore", PropertyAttributes.None, closedInnerDelegateType, Type.EmptyTypes);
                var mtdCompilerCoreGetter = builder.DefineMethod("get_CompilerCore", MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName, closedInnerDelegateType, Type.EmptyTypes);
                propCompilerCore.SetGetMethod(mtdCompilerCoreGetter);

                //
                // 4. The method encapsulating the JIT compiler call to install in CompilerCore `R Jit(TClosure, T1, ..., Tn)`
                //
                GetInvokeParameterTypesAndReturnType(delegateType, genericParametersWithoutClosure, out var invokeParameterTypes, out var invokeReturnType);

                var jitReturnType = invokeReturnType;
                var jitParameterTypes = new Type[invokeParameterTypes.Length + 1];
                jitParameterTypes[0] = closureType;
                Array.Copy(invokeParameterTypes, 0, jitParameterTypes, 1, invokeParameterTypes.Length);

                var mtdJit = builder.DefineMethod("Jit", MethodAttributes.Private | MethodAttributes.HideBySig, jitReturnType, jitParameterTypes);

                //
                // 5. The `void Compile()` method to perform expression tree compilation, called by the Jit method.
                //
                var mtdCompile = builder.DefineMethod("Compile", MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof(void), Type.EmptyTypes);

                //
                // 6. The CreateDelegate method `TDelegate CreateDelegate(TClosure)`
                //
                var mtdCreateDelegate = builder.DefineMethod("CreateDelegate", MethodAttributes.Public | MethodAttributes.HideBySig, closedDelegateType, new[] { closureType });

                //
                // Return all the info.
                //
                return new ThunkTypeDefinition
                {
                    ParentType = parentType,
                    ExpressionType = expressionOfInnerDelegateType,
                    ctor = ctor,
                    get_Compiler = mtdCompilerGetter,
                    get_CompilerCore = mtdCompilerCoreGetter,
                    CreateDelegate = mtdCreateDelegate,
                    Jit = mtdJit,
                    Compile = mtdCompile,
                };
            }

            /// <summary>
            /// Builds the thunk type by emitting IL to its members.
            /// </summary>
            /// <param name="info">Information about the members of the thunk type to emit.</param>
            private static void BuildThunkType(CompilationInfo info)
            {
                MakeThunkTypeConstructor(info.ThunkDefinition);
                MakeThunkTypeCompilerProperty(info.ThunkDefinition);
                MakeThunkTypeCompilerCoreProperty(info);
                MakeThunkTypeCreateDelegateMethod(info);
                MakeThunkTypeJitMethod(info);
                MakeThunkTypeCompileMethod(info);
            }

            /// <summary>
            /// Emits the IL code for the thunk type constructor.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeConstructor(ThunkTypeDefinition info)
            {
                var parentType = info.ParentType;
                var getCompilerCore = info.get_CompilerCore;

                //
                // Get the IL generator.
                //
                var il = info.ctor.GetILGenerator();

                //
                // First, emit the base constructor call.
                //
                var baseCtor = TypeBuilder.GetConstructor(parentType, s_thunkCtor);
                Debug.Assert(baseCtor != null, "Could not find base ctor.");
                il.Emit(OpCodes.Ldarg_0);         // this
                il.Emit(OpCodes.Ldarg_1);         // Expression<TInner>
                il.Emit(OpCodes.Call, baseCtor);  // base(expr)

                //
                // Next, get the JIT compiler delegate and store it in the Inner field.
                //
                // NB: The JIT compiler delegate is fetched from the non-virtual CompilerCore
                //     property. Thunk invalidation uses the virtual Compiler property. This
                //     avoids having to call a virtual from the constructor.
                //
                var innerField = TypeBuilder.GetField(parentType, s_thunkTarget);
                Debug.Assert(innerField != null, "Could not find Target field.");
                il.Emit(OpCodes.Ldarg_0);               // this (for stfld)
                il.Emit(OpCodes.Ldarg_0);               // this (for call)
                il.Emit(OpCodes.Call, getCompilerCore); // this.CompilerCore
                il.Emit(OpCodes.Stfld, innerField);     // this.Target = this.CompilerCore

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            /// <summary>
            /// Emits the IL code for the thunk type Compiler property.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeCompilerProperty(ThunkTypeDefinition info)
            {
                var getCompilerCore = info.get_CompilerCore;

                //
                // Get the IL generator.
                //
                var il = info.get_Compiler.GetILGenerator();

                //
                // Simply get the CompilerCore property.
                //
                // NB: This duplication of properties is used to avoid virtual calls
                //     from the constructor. See remarks on constructor.
                //
                il.Emit(OpCodes.Ldarg_0);                // this
                il.Emit(OpCodes.Call, getCompilerCore);  // this.CompilerCore
                il.Emit(OpCodes.Ret);                    // return this.CompilerCore
            }

            /// <summary>
            /// Emits the IL code for the thunk type CompilerCore property.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeCompilerCoreProperty(CompilationInfo info)
            {
                var thunk = info.ThunkDefinition;
                var innerDelegate = info.InnerDelegateDefinition;
                var jit = thunk.Jit;

                //
                // Close the dispatcher and inner delegate types over our generic parameters.
                //
                var thunkGenericParameters = info.Types.ThunkTypeGenericParameters;
                var innerDelegateType = info.Types.InnerDelegateType.MakeGenericType(thunkGenericParameters);

                //
                // Use TypeBuilder factory methods to create constructor and method infos
                // that incorporate our generic parameters.
                //
                var innerDelegateCtor = TypeBuilder.GetConstructor(innerDelegateType, innerDelegate.ctor);
                Debug.Assert(innerDelegateCtor != null, "Could not find delegate .ctor.");

                //
                // Get the IL generator.
                //
                var il = thunk.get_CompilerCore.GetILGenerator();

                //
                // Return an inner delegate instance with the Jit method as the target.
                //
                il.Emit(OpCodes.Ldarg_0);                    // this
                il.Emit(OpCodes.Ldftn, jit);                 // Jit
                il.Emit(OpCodes.Newobj, innerDelegateCtor);  // new InnerDelegate(this, Jit)
                il.Emit(OpCodes.Ret);                        // return new InnerDelegate(this, Jit)
            }

            /// <summary>
            /// Emits the IL code for the thunk type CreateDelegate method.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeCreateDelegateMethod(CompilationInfo info)
            {
                var thunk = info.ThunkDefinition;
                var dispatcher = info.DispatcherDefinition;
                var resultDelegate = info.Types.DelegateType;

                //
                // Close the dispatcher type over our generic parameters.
                //
                var thunkGenericParameters = info.Types.ThunkTypeGenericParameters;
                var dispatcherType = info.Types.DispatcherType.MakeGenericType(thunkGenericParameters);

                //
                // Get the type of the delegate to return.
                //
                var delegateType = thunk.CreateDelegate.ReturnType;

                //
                // Use TypeBuilder factory methods to create constructor and method infos
                // that incorporate our generic parameters.
                //
                var dispatcherCtor = TypeBuilder.GetConstructor(dispatcherType, dispatcher.ctor);
                var dispatcherInvokeMethod = TypeBuilder.GetMethod(dispatcherType, dispatcher.Invoke);
                var delegateCtor = resultDelegate.GetConstructors()[0];
                if (delegateType.IsGenericType)
                {
                    delegateCtor = TypeBuilder.GetConstructor(delegateType, delegateCtor);
                }

                Debug.Assert(dispatcherCtor != null, "Could not find dispatcher .ctor.");
                Debug.Assert(dispatcherInvokeMethod != null, "Could not find dispatcher Invoke method.");
                Debug.Assert(delegateCtor != null, "Could not find delegate .ctor.");

                //
                // Get the IL generator.
                //
                var il = thunk.CreateDelegate.GetILGenerator();

                //
                // Emit the code to instantiate the inner delegate referring to the Invoke
                // method on a dispatcher instance parameterized by the closure.
                //
                il.Emit(OpCodes.Ldarg_0);                        // this
                il.Emit(OpCodes.Ldarg_1);                        // closure
                il.Emit(OpCodes.Newobj, dispatcherCtor);         // new Dispatcher(this, closure)
                il.Emit(OpCodes.Ldftn, dispatcherInvokeMethod);  // new Dispatcher(this, closure).Invoke
                il.Emit(OpCodes.Newobj, delegateCtor);           // new InnerDelegate(new Dispatcher(this, closure).Invoke)
                il.Emit(OpCodes.Ret);                            // return new InnerDelegate(new Dispatcher(this, closure).Invoke)
            }

            /// <summary>
            /// Emits the IL code for the thunk type Jit method.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeJitMethod(CompilationInfo info)
            {
                var thunk = info.ThunkDefinition;

                //
                // Get the number of parameters passed to the delegate in order to emit the
                // call to the compiled inner delegate after performing JIT compilation.
                //
                var invoke = info.Types.DelegateType.GetMethod("Invoke");
                var count = invoke.GetParameters().Length;

                //
                // Get the IL generator.
                //
                var il = thunk.Jit.GetILGenerator();

                //
                // Close the inner delegate type over our generic parameters.
                //
                var thunkGenericParameters = info.Types.ThunkTypeGenericParameters;
                var innerDelegateType = info.Types.InnerDelegateType.MakeGenericType(thunkGenericParameters);

                //
                // Use TypeBuilder factory methods to create constructor and method infos
                // that incorporate our generic parameters.
                //
                var targetFld = TypeBuilder.GetField(thunk.ParentType, s_thunkTarget);
                var invokeMtd = TypeBuilder.GetMethod(innerDelegateType, info.InnerDelegateDefinition.Invoke);

                Debug.Assert(targetFld != null, "Could not find thunk Target field.");
                Debug.Assert(invokeMtd != null, "Could not find delegate Invoke method.");

                //
                // Invoke Compile method.
                //
                il.Emit(OpCodes.Ldarg_0);                  //   this
                il.Emit(OpCodes.Callvirt, thunk.Compile);  //   this.Compile()

                //
                // Invoke the compiled delegate.
                //
                il.Emit(OpCodes.Ldarg_0);                  //   this
                il.Emit(OpCodes.Ldfld, targetFld);         //   this.Target

                //
                // NB: We need to emit one more `ldarg` than the number of delegate parameters;
                //     this is to account for the closure parameter. Also note that we start at
                //     argument 1, to skip over the `this` parameter.
                //
                for (var i = 1; i <= count + 1; i++)
                {
                    il.EmitLdarg(i);                       //   closure, arg1, ..., argn
                }

                il.Emit(OpCodes.Callvirt, invokeMtd);      //   this.Target(closure, arg1, ..., argn)

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            /// <summary>
            /// Emits the IL code for the thunk type Compile method.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types, and their members.</param>
            private static void MakeThunkTypeCompileMethod(CompilationInfo info)
            {
                var thunk = info.ThunkDefinition;

                //
                // Use TypeBuilder factory methods to create constructor and method infos
                // that incorporate our generic parameters.
                //
                var expressionFld = TypeBuilder.GetField(thunk.ParentType, s_thunkExpression);
                var targetFld = TypeBuilder.GetField(thunk.ParentType, s_thunkTarget);

                Debug.Assert(expressionFld != null, "Could not find thunk Expression field.");
                Debug.Assert(targetFld != null, "Could not find thunk Target field.");

                //
                // Get the IL generator.
                //
                var il = thunk.Compile.GetILGenerator();

                //
                // Generate the core logic.
                //
                switch (info.Kind)
                {
                    case ThunkBehavior.Compiling:
                        Compiling();
                        break;
                    case ThunkBehavior.Interpreting:
                        Interpreting();
                        break;
                    case ThunkBehavior.TieredCompilation:
                        TieredCompilation();
                        break;
                }

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);

                void Compiling()
                {
                    //
                    // Declare a Boolean local to keep track of the outcome of acquiring the lock.
                    //
                    var lockTaken = il.DeclareLocal(typeof(bool));

                    //
                    // Define a label to exit the finally block using a `leave` instruction.
                    //
                    var afterFinally = il.DefineLabel();

                    //
                    // Define a label to jump to the end of the try block to bypass the logic to
                    // compile the expression tree if Expression is null.
                    //
                    var endTry = il.DefineLabel();

                    //
                    // Define a label to jump to the end of the finally block to bypass the call
                    // to Monitor.Exit in case the lock wasn't taken.
                    //
                    var endFinally = il.DefineLabel();

                    //
                    // Use TypeBuilder factory methods to create constructor and method infos
                    // that incorporate our generic parameters.
                    //
                    var compileMtd = TypeBuilder.GetMethod(thunk.ExpressionType, s_expressionCompile);

                    Debug.Assert(compileMtd != null, "Could not find expression Compile method.");

                    //
                    // Initialize locals.
                    //
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Stloc, lockTaken);      //   lockTaken = false

                    //
                    // Run compilation code protected by the lock.
                    //
                    il.BeginExceptionBlock();               //   .try {
                    il.Emit(OpCodes.Ldarg_0);               //         this
                    il.Emit(OpCodes.Ldloca, lockTaken);     //         &lockTaken
                    il.Emit(OpCodes.Call, s_monitorEnter);  //         Monitor.Enter(this, &lockTaken)
                    il.Emit(OpCodes.Ldarg_0);               //         this
                    il.Emit(OpCodes.Ldfld, expressionFld);  //         this.Expression
                    il.Emit(OpCodes.Brfalse, endTry);       //         if (this.Expression != null) {
                    il.Emit(OpCodes.Ldarg_0);               //             this (for Target field)
                    il.Emit(OpCodes.Ldarg_0);               //             this (for Expression field)
                    il.Emit(OpCodes.Ldfld, expressionFld);  //             this.Expression
                    il.Emit(OpCodes.Callvirt, compileMtd);  //             this.Expression.Compile()
                    il.Emit(OpCodes.Stfld, targetFld);      //             this.Target = this.Expression.Compile()
                    il.Emit(OpCodes.Ldarg_0);               //             this
                    il.Emit(OpCodes.Ldnull);                //             null
                    il.Emit(OpCodes.Stfld, expressionFld);  //             this.Expression = null
                    il.MarkLabel(endTry);                   //         }
                    il.Emit(OpCodes.Leave, afterFinally);   //         goto exit;
                    il.BeginFinallyBlock();                 //   } finally {
                    il.Emit(OpCodes.Ldloc, lockTaken);      //         if (!lockTaken)
                    il.Emit(OpCodes.Brfalse, endFinally);   //             goto skip;
                    il.Emit(OpCodes.Ldarg_0);               //         this
                    il.Emit(OpCodes.Call, s_monitorExit);   //         Monitor.Exit(this)
                    il.MarkLabel(endFinally);               // skip:
                    il.Emit(OpCodes.Endfinally);            //         ;
                    il.EndExceptionBlock();                 //   }
                    il.MarkLabel(afterFinally);             // exit:
                }

                void Interpreting()
                {
                    //
                    // Declare a local to store the expression read from the Expression field.
                    //
                    var expressionLoc = il.DeclareLocal(thunk.ExpressionType);

                    //
                    // Use TypeBuilder factory methods to create constructor and method infos
                    // that incorporate our generic parameters.
                    //
                    var compileMtd = TypeBuilder.GetMethod(thunk.ExpressionType, s_expressionCompileBool);

                    Debug.Assert(compileMtd != null, "Could not find expression Compile(bool) method.");

                    //
                    // Define a label to jump to the end of the method to bypass the logic to
                    // compile the expression tree if Expression is null.
                    //
                    var exit = il.DefineLabel();

                    //
                    // Run compilation code.
                    //
                    il.Emit(OpCodes.Ldarg_0);               // this
                    il.Emit(OpCodes.Ldfld, expressionFld);  // this.Expression
                    il.Emit(OpCodes.Stloc, expressionLoc);  // expression = this.Expression
                    il.Emit(OpCodes.Ldloc, expressionLoc);  // expression
                    il.Emit(OpCodes.Brfalse, exit);         // if (expression != null) {
                    il.Emit(OpCodes.Ldarg_0);               //     this (for Target field)
                    il.Emit(OpCodes.Ldloc, expressionLoc);  //     expression
                    il.Emit(OpCodes.Ldc_I4_1);              //     true
                    il.Emit(OpCodes.Callvirt, compileMtd);  //     expression.Compile(preferInterpretation: true)
                    il.Emit(OpCodes.Stfld, targetFld);      //     this.Target = expression.Compile(preferInterpretation: true)
                    il.Emit(OpCodes.Ldarg_0);               //     this
                    il.Emit(OpCodes.Ldnull);                //     null
                    il.Emit(OpCodes.Stfld, expressionFld);  //     this.Expression = null
                    il.MarkLabel(exit);                     // }
                }

                void TieredCompilation()
                {
                    //
                    // Declare a local to store the expression read from the Expression field.
                    //
                    var expressionLoc = il.DeclareLocal(thunk.ExpressionType);

                    //
                    // Define a label to jump to the end of the method to bypass the logic to
                    // compile the expression tree if Expression is null.
                    //
                    var exit = il.DefineLabel();

                    //
                    // Get the constructor of the tiered recompilation type.
                    //
                    var recompileCtor = info.TieredRecompilationDefinition.ctor;

                    //
                    // Get the Invoke method of the tiered recompilation type, used to create a delegate.
                    //
                    var recompileInvoke = info.TieredRecompilationDefinition.Invoke;

                    //
                    // Get the delegate constructor for Target.
                    //
                    var delegateType = info.Types.DelegateType;
                    var delegateCtor = delegateType.GetConstructors()[0];

                    //
                    // Run compilation code.
                    //
                    il.Emit(OpCodes.Ldarg_0);                // this
                    il.Emit(OpCodes.Ldfld, expressionFld);   // this.Expression
                    il.Emit(OpCodes.Stloc, expressionLoc);   // expression = this.Expression
                    il.Emit(OpCodes.Ldloc, expressionLoc);   // expression
                    il.Emit(OpCodes.Brfalse, exit);          // if (expression != null) {
                    il.Emit(OpCodes.Ldarg_0);                //     this (for Target field)
                    il.Emit(OpCodes.Ldarg_0);                //     this (for ctor argument)
                    il.Emit(OpCodes.Ldloc, expressionLoc);   //     expression
                    il.Emit(OpCodes.Newobj, recompileCtor);  //     new Recompile(this, expression)
                    il.Emit(OpCodes.Ldftn, recompileInvoke); //     new Recompile(this, expression).Invoke
                    il.Emit(OpCodes.Newobj, delegateCtor);   //     new TInner(new Recompile(this, expression))
                    il.Emit(OpCodes.Stfld, targetFld);       //     this.Target = new TInner(new Recompile(this, expression).Invoke)
                    il.Emit(OpCodes.Ldarg_0);                //     this
                    il.Emit(OpCodes.Ldnull);                 //     null
                    il.Emit(OpCodes.Stfld, expressionFld);   //     this.Expression = null
                    il.MarkLabel(exit);                      // }
                }
            }

            /// <summary>
            /// Struct containing information about the thunk type and its members.
            /// </summary>
            private struct ThunkTypeDefinition
            {
                /// <summary>
                /// The base class of the thunk type, closed over the generic parameters defined on the thunk type.
                /// </summary>
                public Type ParentType;

                /// <summary>
                /// The generic Expression type closed over the inner delegate type, which is on its turn closed over the generic parameters defined on the thunk type.
                /// </summary>
                public Type ExpressionType;

                /// <summary>
                /// The instance constructor of the thunk type.
                /// </summary>
                /// <example>
                /// <c>public .ctor(Expression{TInner})</c>
                /// </example>
                public ConstructorBuilder ctor;

                /// <summary>
                /// The property getter for the Compiler property.
                /// </summary>
                /// <example>
                /// <c>public TInner get_Compiler</c>
                /// </example>
                public MethodBuilder get_Compiler;

                /// <summary>
                /// The property getter for the CompilerCore property.
                /// </summary>
                /// <example>
                /// <c>public TInner get_CompilerCore</c>
                /// </example>
                public MethodBuilder get_CompilerCore;

                /// <summary>
                /// The Jit method.
                /// </summary>
                /// <example>
                /// <c>public TResult Jit(TClosure, T1, ..., Tn)</c>
                /// </example>
                public MethodBuilder Jit;

                /// <summary>
                /// The Compile method.
                /// </summary>
                /// <example>
                /// <c>public virtual void Compile()</c>
                /// </example>
                public MethodBuilder Compile;

                /// <summary>
                /// The CreateDelegate method.
                /// </summary>
                /// <example>
                /// <c>public TDelegate Create(TClosure)</c>
                /// </example>
                public MethodBuilder CreateDelegate;
            }

            /// <summary>
            /// Defines the members on the dispatcher type without performing IL code emission.
            /// </summary>
            /// <param name="info">Information about the thunk dispatcher and related types used to create cross-type references.</param>
            /// <returns>An object containing information about the defined members on the dispatcher type.</returns>
            private static DispatcherTypeDefinition DefineDispatcherType(ThunkTypesInfo info)
            {
                var delegateType = info.DelegateType;
                var thunkType = info.ThunkType;
                var builder = info.DispatcherType;
                var genericParameters = info.DispatcherTypeGenericParameters;
                var innerDelegateType = info.InnerDelegateType;

                //
                // We need the generic arguments to close over the Dispatcher<,,> base class
                // and the parent thunk type, so allocate the type array once.
                //
                var closedDelegateType = delegateType;
                var genericParametersWithoutClosure = genericParameters.RemoveFirst();

                if (genericParametersWithoutClosure.Length > 0)
                {
                    closedDelegateType = delegateType.MakeGenericType(genericParametersWithoutClosure);
                }

                var closureType = genericParameters[0];
                var closedInnerDelegateType = innerDelegateType.MakeGenericType(genericParameters);
                var genericArguments = new[] { closedDelegateType, closureType, closedInnerDelegateType };

                //
                // First, get the FunctionContext<TClosure> type by closing over the closure
                // generic parameter.
                //
                var contextType = typeof(FunctionContext<>).MakeGenericType(closureType);

                //
                // Second, set the base class to Dispatcher<TDelegate, TClosure, TInner> by
                // closing the delegate and inner delegate types as needed.
                //
                var parentType = typeof(Dispatcher<,,>).MakeGenericType(genericArguments);
                builder.SetParent(parentType);

                //
                // Next, define all the members upfront so we can refer to them from
                // each other during IL generation.
                //
                // 1. The constructor `.ctor(<Bar>__Thunk<TInner>, TClosure)`
                //
                var parentThunkType = thunkType.MakeGenericType(genericArguments);
                var ctorParameterTypes = new[] { parentThunkType, closureType };
                var ctor = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, ctorParameterTypes);

                //
                // 2. The invoke method `R Invoke(T1, ..., Tn)`
                //
                GetInvokeParameterTypesAndReturnType(delegateType, genericParametersWithoutClosure, out var invokeParameterTypes, out var invokeReturnType);

                var mtdInvoke = builder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig, invokeReturnType, invokeParameterTypes);

                //
                // Return all the info.
                //
                return new DispatcherTypeDefinition
                {
                    ParentType = parentType,
                    ContextType = contextType,
                    ctor = ctor,
                    Invoke = mtdInvoke,
                };
            }

            /// <summary>
            /// Builds the dispatcher type by emitting IL to its members.
            /// </summary>
            /// <param name="info">Information about the members of the thunk type to emit.</param>
            private static void BuildDispatcherType(CompilationInfo info)
            {
                MakeDispatcherTypeConstructor(info.DispatcherDefinition);
                MakeDispatcherInvokeMethod(info);
            }

            /// <summary>
            /// Emits the IL code for the dispatcher type constructor.
            /// </summary>
            /// <param name="info">Information about the dispatcher type and its members.</param>
            private static void MakeDispatcherTypeConstructor(DispatcherTypeDefinition info)
            {
                var parentType = info.ParentType;
                var contextType = info.ContextType;

                //
                // Get the IL generator.
                //
                var il = info.ctor.GetILGenerator();

                //
                // First, emit the base constructor call.
                //
                var baseCtor = TypeBuilder.GetConstructor(parentType, s_dispatcherCtor);
                Debug.Assert(baseCtor != null, "Could not find base ctor.");
                il.Emit(OpCodes.Ldarg_0);         // this
                il.Emit(OpCodes.Call, baseCtor);  // base(expr)

                //
                // Next, store the parent thunk in the Parent field.
                //
                var parentField = TypeBuilder.GetField(parentType, s_dispatcherParent);
                Debug.Assert(parentField != null, "Could not find Parent field.");
                il.Emit(OpCodes.Ldarg_0);               // this
                il.Emit(OpCodes.Ldarg_1);               // parent
                il.Emit(OpCodes.Stfld, parentField);    // this.Parent = parent

                //
                // Next, store the closure instance in the Closure field.
                //
                var closureField = TypeBuilder.GetField(contextType, s_contextClosure);
                Debug.Assert(closureField != null, "Could not find Closure field.");
                il.Emit(OpCodes.Ldarg_0);               // this
                il.Emit(OpCodes.Ldarg_2);               // closure
                il.Emit(OpCodes.Stfld, closureField);   // this.Closure = closure

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            /// <summary>
            /// Emits the IL code for the dispatcher type Invoke method.
            /// </summary>
            /// <param name="info">Information about the dispatcher type and related types, and their members.</param>
            private static void MakeDispatcherInvokeMethod(CompilationInfo info)
            {
                var dispatcherInfo = info.DispatcherDefinition;
                var parentType = dispatcherInfo.ParentType;
                var contextType = dispatcherInfo.ContextType;

                //
                // Get the number of parameters passed to the delegate in order to emit the
                // call to the compiled inner delegate after performing JIT compilation.
                //
                var invoke = info.Types.DelegateType.GetMethod("Invoke");
                var count = invoke.GetParameters().Length;

                //
                // Close the thunk and inner delegate types over our generic parameters.
                //
                var dispatcherGenericParameters = info.Types.DispatcherTypeGenericParameters;
                var thunkParentType = info.Types.ThunkType.MakeGenericType(dispatcherGenericParameters).BaseType;
                var innerDelegateType = info.Types.InnerDelegateType.MakeGenericType(dispatcherGenericParameters);

                //
                // Get the IL generator.
                //
                var il = dispatcherInfo.Invoke.GetILGenerator();

                //
                // First, the thunk stored in the Parent field.
                //
                var parentField = TypeBuilder.GetField(parentType, s_dispatcherParent);
                Debug.Assert(parentField != null, "Could not find Parent field.");
                il.Emit(OpCodes.Ldarg_0);               // this
                il.Emit(OpCodes.Ldfld, parentField);    // this.Parent

                //
                // Next, traverse to the inner delegate stored in the thunk's Target field.
                //
                var targetField = TypeBuilder.GetField(thunkParentType, s_thunkTarget);
                Debug.Assert(targetField != null, "Could not find Target field.");
                il.Emit(OpCodes.Ldfld, targetField);    // this.Parent.Target

                //
                // Now load the closure field to pass it as the first parameter to the target delegate.
                //
                var closureField = TypeBuilder.GetField(contextType, s_contextClosure);
                Debug.Assert(targetField != null, "Could not find Closure field.");
                il.Emit(OpCodes.Ldarg_0);               // this
                il.Emit(OpCodes.Ldfld, closureField);   // this.Closure

                //
                // Get the Invoke method of the inner delegate closed over our generic parameters.
                //
                var invokeMtd = TypeBuilder.GetMethod(innerDelegateType, info.InnerDelegateDefinition.Invoke);
                Debug.Assert(invokeMtd != null, "Could not find Invoke method.");

                //
                // Finally, fetch the formal parameters and invoke the delegate.
                //
                for (var i = 1; i <= count; i++)
                {
                    il.EmitLdarg(i);                    // arg1, ..., argn
                }

                il.Emit(OpCodes.Callvirt, invokeMtd);   // this.Parent.Target(this.Closure, arg1, ..., argn)

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            /// <summary>
            /// Struct containing information about the dispatcher type and its members.
            /// </summary>
            private struct DispatcherTypeDefinition
            {
                /// <summary>
                /// The base class of the dispatcher type, closed over the generic parameters defined on the dispatcher type.
                /// </summary>
                public Type ParentType;

                /// <summary>
                /// The grandparent FunctionContext generic type, closed over the generic closure parameter defined on the dispatcher type.
                /// </summary>
                public Type ContextType;

                /// <summary>
                /// The instance constructor of the dispatcher type.
                /// </summary>
                /// <example>
                /// <c>public .ctor(Thunk{...}, TClosure)</c>
                /// </example>
                public ConstructorBuilder ctor;

                /// <summary>
                /// The Invoke method.
                /// </summary>
                /// <example>
                /// <c>public TResult Invoke(T1, ..., Tn)</c>
                /// </example>
                public MethodBuilder Invoke;
            }

            /// <summary>
            /// Defines the members on the inner delegate type without performing IL code emission.
            /// </summary>
            /// <param name="info">Information about the inner delegate type and related types used to create cross-type references.</param>
            /// <returns>An object containing information about the defined members on the inner delegate type.</returns>
            private static InnerDelegateTypeDefinition DefineInnerDelegateType(ThunkTypesInfo info)
            {
                var delegateType = info.DelegateType;
                var genericParameters = info.InnerDelegateTypeGenericParameters;
                var builder = info.InnerDelegateType;

                //
                // First, set the parent type to MulticastDelegate.
                //
                builder.SetParent(typeof(MulticastDelegate));

                //
                // Next, define all the members upfront so we can refer to them from
                // each other during IL generation.
                //
                // 1. The constructor `.ctor(object, IntPtr)`
                //
                var ctor = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard, s_delegateCtorParameterTypes);
                ctor.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

                //
                // 2. The invoke method `R Invoke(TClosure, T1, ..., Tn)`
                //
                var genericParametersWithoutClosure = genericParameters.RemoveFirst();

                GetInvokeParameterTypesAndReturnType(delegateType, genericParametersWithoutClosure, out var invokeParameterTypes, out var invokeReturnType);

                var invokeParameterTypesWithClosure = new Type[invokeParameterTypes.Length + 1];
                invokeParameterTypesWithClosure[0] = genericParameters[0];
                Array.Copy(invokeParameterTypes, 0, invokeParameterTypesWithClosure, 1, invokeParameterTypes.Length);

                var mtdInvoke = builder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, invokeReturnType, invokeParameterTypesWithClosure);
                mtdInvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

                //
                // Return all the info.
                //
                return new InnerDelegateTypeDefinition
                {
                    ctor = ctor,
                    Invoke = mtdInvoke,
                };
            }

            /// <summary>
            /// Struct containing information about the inner delegate type and its members.
            /// </summary>
            private struct InnerDelegateTypeDefinition
            {
                /// <summary>
                /// The instance constructor of the inner delegate type.
                /// </summary>
                /// <example>
                /// <c>public .ctor(object, IntPtr)</c>
                /// </example>
                public ConstructorBuilder ctor;

                /// <summary>
                /// The Invoke method.
                /// </summary>
                /// <example>
                /// <c>public TResult Invoke(TClosure, T1, ..., Tn)</c>
                /// </example>
                public MethodBuilder Invoke;
            }

            /// <summary>
            /// Gets the parameter types and return type for methods with the same signature as the Invoke method of the specified delegate type,
            /// where occurrences of generic delegate type parameters are substituted for the specified generic arguments.
            /// </summary>
            /// <param name="delegateType">The delegate type to derive the Invoke method parameters and return type from.</param>
            /// <param name="genericArguments">The generic arguments to substitute the generic delegate type parameters for.</param>
            /// <param name="parameterTypes">(Output) The parameter types of the delegate Invoke method after substituting generic delegate type parameters for the specified generic arguments.</param>
            /// <param name="returnType">(Output) The return type of the delegate Invoke method after substituting generic delegate type parameters for the specified generic arguments.</param>
            private static void GetInvokeParameterTypesAndReturnType(Type delegateType, Type[] genericArguments, out Type[] parameterTypes, out Type returnType)
            {
                Debug.Assert(delegateType.IsGenericType == genericArguments.Length > 0, "Expected genericity to match.");

                //
                // Type substitutor to replace occurrences of generic delegate parameter types for
                // the specified generic arguments. If the delegate type is non-generic, this local
                // will be null.
                //
                var subst = default(TypeSubstitutor);

                //
                // Build up the type substitutor if the delegate type is generic.
                //
                if (delegateType.IsGenericType)
                {
                    Debug.Assert(delegateType.IsGenericTypeDefinition, "Expected definition of a generic delegate type.");

                    var genericParameters = delegateType.GetGenericArguments();

                    var map = new Dictionary<Type, Type>(genericParameters.Length);
                    for (var i = 0; i < genericParameters.Length; i++)
                    {
                        map.Add(genericParameters[i], genericArguments[i]);
                    }

                    subst = new TypeSubstitutor(map);
                }

                //
                // Get the original delegate type's Invoke method.
                //
                var invokeMethod = delegateType.GetMethod("Invoke");
                Debug.Assert(invokeMethod != null, "Expected Invoke method.");

                //
                // Get the parameter types and apply type substitution, if needed.
                //
                var parameters = invokeMethod.GetParameters();

                parameterTypes = new Type[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;

                    if (subst != null)
                    {
                        parameterType = subst.Visit(parameterType);
                    }

                    parameterTypes[i] = parameterType;
                }

                //
                // Get the return type and apply type substitution, if needed.
                //
                returnType = invokeMethod.ReturnType;

                if (subst != null)
                {
                    returnType = subst.Visit(returnType);
                }
            }

            /// <summary>
            /// Builds a wrapper around a delegate that's used to trigger recompilation after a number of invocations.
            /// </summary>
            /// <param name="info">Information about the thunk type and related types used to create cross-type references.</param>
            /// <returns>An object containing information about the defined members on the recompiliation type.</returns>
            private static TieredRecompilationTypeDefinition DefineTieredRecompilationType(ThunkTypesInfo info)
            {
                var thunkType = info.ThunkType;
                var delegateType = info.DelegateType;
                var builder = info.TieredRecompilationType;
                var genericParameters = info.TieredRecompilationTypeGenericParameters;
                var innerDelegateType = info.InnerDelegateType;

                //
                // We need the generic arguments to close over the parent thunk type.
                //
                var closedDelegateType = delegateType;
                var genericParametersWithoutClosure = genericParameters.RemoveFirst();

                if (genericParametersWithoutClosure.Length > 0)
                {
                    closedDelegateType = delegateType.MakeGenericType(genericParametersWithoutClosure);
                }

                var closureType = genericParameters[0];
                var closedInnerDelegateType = innerDelegateType.MakeGenericType(genericParameters);
                var genericArguments = new[] { closedDelegateType, closureType, closedInnerDelegateType };

                //
                // Next, define all the members upfront so we can refer to them from
                // each other during IL generation.
                //
                // 1. The constructor `.ctor(<Bar>__Thunk<TInner>, Expression<TInner>)`
                //
                var parentThunkType = thunkType.MakeGenericType(genericArguments);
                var expressionOfInnerDelegateType = typeof(Expression<>).MakeGenericType(closedInnerDelegateType);
                var ctorParameterTypes = new[] { parentThunkType, expressionOfInnerDelegateType };
                var ctor = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, ctorParameterTypes);

                //
                // 2. The ` _parent` field
                //
                var fldParent = builder.DefineField("_parent", parentThunkType, FieldAttributes.Private | FieldAttributes.InitOnly);

                //
                // 3. The `Expression<TInner> _expression` field
                //
                var fldExpression = builder.DefineField("_expression", expressionOfInnerDelegateType, FieldAttributes.Private | FieldAttributes.InitOnly);

                //
                // 4. The `int _hitCount` field
                //
                var fldHitCount = builder.DefineField("_hitCount", typeof(int), FieldAttributes.Private);

                //
                // 5. The `TInner _target` field
                //
                var fldTarget = builder.DefineField("_target", closedInnerDelegateType, FieldAttributes.Private);

                //
                // 6. The `R Invoke(TClosure, T1, ..., Tn)` method
                //
                GetInvokeParameterTypesAndReturnType(delegateType, genericParametersWithoutClosure, out var invokeParameterTypes, out var invokeReturnType);

                var parameterTypes = new Type[invokeParameterTypes.Length + 1];
                parameterTypes[0] = closureType;
                Array.Copy(invokeParameterTypes, 0, parameterTypes, 1, invokeParameterTypes.Length);

                var mtdInvoke = builder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig, invokeReturnType, parameterTypes);

                //
                // Return all the info.
                //
                return new TieredRecompilationTypeDefinition
                {
                    ctor = ctor,
                    Parent = fldParent,
                    Expression = fldExpression,
                    HitCount = fldHitCount,
                    Target = fldTarget,
                    Invoke = mtdInvoke,
                };
            }

            /// <summary>
            /// Builds the tiered recompiliation type by emitting IL to its members.
            /// </summary>
            /// <param name="info">Information about the members of the thunk type to emit.</param>
            private static void BuildTieredRecompilationType(CompilationInfo info)
            {
                MakeTieredRecompilationTypeConstructor(info.TieredRecompilationDefinition);
                MakeTieredRecompilationTypeInvokeMethod(info);
            }

            /// <summary>
            /// Emits the IL code for the tiered recompilation type constructor.
            /// </summary>
            /// <param name="info">Information about the tiered recompilation type and its members.</param>
            private static void MakeTieredRecompilationTypeConstructor(TieredRecompilationTypeDefinition info)
            {
                //
                // Get the IL generator.
                //
                var il = info.ctor.GetILGenerator();

                //
                // First, emit the base constructor call.
                //
                var baseCtor = typeof(object).GetConstructor(Type.EmptyTypes);
                Debug.Assert(baseCtor != null, "Could not find base ctor.");
                il.Emit(OpCodes.Ldarg_0);         // this
                il.Emit(OpCodes.Call, baseCtor);  // base()

                //
                // Next, store the parent thunk in the _parent field.
                //
                il.Emit(OpCodes.Ldarg_0);                // this
                il.Emit(OpCodes.Ldarg_1);                // parent
                il.Emit(OpCodes.Stfld, info.Parent);     // this._parent = parent

                //
                // Next, store the expression in the _expression field.
                //
                il.Emit(OpCodes.Ldarg_0);                // this
                il.Emit(OpCodes.Ldarg_2);                // expression
                il.Emit(OpCodes.Stfld, info.Expression); // this._expression = expression

                //
                // Next, initialize the _hitCount field to 0.
                //
                il.Emit(OpCodes.Ldarg_0);                // this
                il.Emit(OpCodes.Ldc_I4_0);               // 0
                il.Emit(OpCodes.Stfld, info.HitCount);   // this._hitCount = 0;

                //
                // Finally, initialize the target by compiling the expression tree.
                //
                var compileMtd = TypeBuilder.GetMethod(info.Expression.FieldType, s_expressionCompileBool);

                Debug.Assert(compileMtd != null, "Could not find expression Compile(bool) method.");

                il.Emit(OpCodes.Ldarg_0);                // this
                il.Emit(OpCodes.Ldarg_2);                // expression
                il.Emit(OpCodes.Ldc_I4_1);               // true
                il.Emit(OpCodes.Callvirt, compileMtd);   // expression.Compile(preferInterpretation: true)
                il.Emit(OpCodes.Stfld, info.Target);     // this._target = expression.Compile(preferInterpretation: true)

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            private static void MakeTieredRecompilationTypeInvokeMethod(CompilationInfo info)
            {
                //
                // Get the IL generator.
                //
                var il = info.TieredRecompilationDefinition.Invoke.GetILGenerator();

                //
                // Get the number of parameters passed to the delegate in order to emit the
                // call to the target delegate after performing tiered compilation checks.
                //
                var delegateType = info.Types.DelegateType;
                var invoke = delegateType.GetMethod("Invoke");
                var count = invoke.GetParameters().Length + 1 /* closure */;

                //
                // Get the Invoke method of the inner delegate closed over our generic parameters.
                //
                var innerDelegateType = info.Types.InnerDelegateType;
                var invokeMtd = TypeBuilder.GetMethod(innerDelegateType, info.InnerDelegateDefinition.Invoke);
                Debug.Assert(invokeMtd != null, "Could not find Invoke method.");

                //
                // Get the target delegate field containing the delegate to invoke, the hit count
                // field to do bookkeeping, the expression field in case we need to recompile, and
                // the parent field to reassign the target if we recompiled.
                //
                var target = info.TieredRecompilationDefinition.Target;
                var hitCount = info.TieredRecompilationDefinition.HitCount;
                var expression = info.TieredRecompilationDefinition.Expression;
                var parent = info.TieredRecompilationDefinition.Parent;

                //
                // Get the Compile method in case we need to recompile.
                //
                var compileMtd = TypeBuilder.GetMethod(expression.FieldType, s_expressionCompile);

                Debug.Assert(compileMtd != null, "Could not find expression Compile method.");

                //
                // Define a branch target to jump over recompilation if threshold is not met.
                //
                var eval = il.DefineLabel();

                //
                // First, do the tiered compilation bookkeeping by incrementing and checking hit count.
                //
                il.Emit(OpCodes.Ldarg_0);                                          // this
                il.Emit(OpCodes.Ldflda, hitCount);                                 // ref this._hitCount
                il.Emit(OpCodes.Call, s_interlockedIncrement);                     // Interlocked.Increment(ref this._hitCount)
                il.Emit(OpCodes.Ldc_I4, JitConstants.TieredCompilationThreshold);  // threshold
                il.Emit(OpCodes.Bne_Un, eval);                                     // if (Interlocked.Increment(ref this._hitCount) == threshold)

                //
                // Recompile if threshold is met.
                //
                il.Emit(OpCodes.Ldarg_0);                                          //     this (for this._target assignment)
                il.Emit(OpCodes.Ldarg_0);                                          //     this (for this._expression read)
                il.Emit(OpCodes.Ldfld, expression);                                //     this._expression
                il.Emit(OpCodes.Callvirt, compileMtd);                             //     this._expression.Compile()
                il.Emit(OpCodes.Stfld, target);                                    //     this._target = this._expression.Compile()

                //
                // Reassign Target on parent as well.
                //
                var genericParameters = info.Types.TieredRecompilationTypeGenericParameters;
                var thunkParentType = info.Types.ThunkType.MakeGenericType(genericParameters).BaseType;

                var targetField = TypeBuilder.GetField(thunkParentType, s_thunkTarget);
                Debug.Assert(targetField != null, "Could not find Target field.");

                il.Emit(OpCodes.Ldarg_0);                                          //     this
                il.Emit(OpCodes.Ldfld, parent);                                    //     this._parent
                il.Emit(OpCodes.Ldarg_0);                                          //     this
                il.Emit(OpCodes.Ldfld, target);                                    //     this._target
                il.Emit(OpCodes.Stfld, targetField);                               //     this._parent.Target = this._target

                //
                // Reaching point of recompilation code, either due to skipping or fall through.
                //
                il.MarkLabel(eval);

                //
                // Get the target delegate from the field.
                //
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, target);

                //
                // Fetch the formal parameters and invoke the delegate.
                //
                for (var i = 1; i <= count; i++)
                {
                    il.EmitLdarg(i);                    // closure, arg1, ..., argn
                }

                il.Emit(OpCodes.Callvirt, invokeMtd);   // this._target(closure, arg1, ..., argn)

                //
                // Return.
                //
                il.Emit(OpCodes.Ret);
            }

            /// <summary>
            /// Struct containing information about the tiered recompilation type and its members.
            /// </summary>
            private struct TieredRecompilationTypeDefinition
            {
                /// <summary>
                /// The instance constructor of the tiered recompilation type.
                /// </summary>
                /// <example>
                /// <c>public .ctor(Thunk{TInner}, Expression{TInner})</c>
                /// </example>
                public ConstructorBuilder ctor;

                /// <summary>
                /// The parent field.
                /// </summary>
                /// <example>
                /// <c>private readonly Thunk{TInner} _parent</c>
                /// </example>
                public FieldBuilder Parent;

                /// <summary>
                /// The expression field.
                /// </summary>
                /// <example>
                /// <c>private readonly Expression{TInner} _expression</c>
                /// </example>
                public FieldBuilder Expression;

                /// <summary>
                /// The hit count field.
                /// </summary>
                /// <example>
                /// <c>private int _hitCount</c>
                /// </example>
                public FieldBuilder HitCount;

                /// <summary>
                /// The target field.
                /// </summary>
                /// <example>
                /// <c>private TInner _target</c>
                /// </example>
                public FieldBuilder Target;

                /// <summary>
                /// The Invoke method.
                /// </summary>
                /// <example>
                /// <c>public TResult Invoke(TClosure, T1, ..., Tn)</c>
                /// </example>
                public MethodBuilder Invoke;
            }

            /// <summary>
            /// Gets a unique integer to use in type names.
            /// </summary>
            /// <returns>A unique integer value.</returns>
            private static int GetUniqueId() => Interlocked.Increment(ref s_count);
        }
    }
}
