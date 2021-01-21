// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

#if !NO_IO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nuqleon.Json.Serialization
{
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// Utility to build emitters optimized for a specified type.
        /// </summary>
        /// <remarks>This type is thread-safe.</remarks>
        internal class EmitterWriterBuilder
        {
            //
            // NB: These are static cached delegates for primitive type emitters; they get lazily set on first usage.
            //

            private static EmitWriterAction<object> s_emitAny;

            private static EmitWriterAction<string> s_emitString;

            private static EmitWriterAction<bool> s_emitBool;
            private static EmitWriterAction<sbyte> s_emitSByte;
            private static EmitWriterAction<byte> s_emitByte;
            private static EmitWriterAction<short> s_emitInt16;
            private static EmitWriterAction<ushort> s_emitUInt16;
            private static EmitWriterAction<int> s_emitInt32;
            private static EmitWriterAction<uint> s_emitUInt32;
            private static EmitWriterAction<long> s_emitInt64;
            private static EmitWriterAction<ulong> s_emitUInt64;
            private static EmitWriterAction<float> s_emitSingle;
            private static EmitWriterAction<double> s_emitDouble;
            private static EmitWriterAction<decimal> s_emitDecimal;
            private static EmitWriterAction<char> s_emitChar;
            private static EmitWriterAction<DateTime> s_emitDateTime;
            private static EmitWriterAction<DateTimeOffset> s_emitDateTimeOffset;

            private static EmitWriterAction<bool?> s_emitNullableBool;
            private static EmitWriterAction<sbyte?> s_emitNullableSByte;
            private static EmitWriterAction<byte?> s_emitNullableByte;
            private static EmitWriterAction<short?> s_emitNullableInt16;
            private static EmitWriterAction<ushort?> s_emitNullableUInt16;
            private static EmitWriterAction<int?> s_emitNullableInt32;
            private static EmitWriterAction<uint?> s_emitNullableUInt32;
            private static EmitWriterAction<long?> s_emitNullableInt64;
            private static EmitWriterAction<ulong?> s_emitNullableUInt64;
            private static EmitWriterAction<float?> s_emitNullableSingle;
            private static EmitWriterAction<double?> s_emitNullableDouble;
            private static EmitWriterAction<decimal?> s_emitNullableDecimal;
            private static EmitWriterAction<char?> s_emitNullableChar;
            private static EmitWriterAction<DateTime?> s_emitNullableDateTime;
            private static EmitWriterAction<DateTimeOffset?> s_emitNullableDateTimeOffset;

            //
            // NB: The following generic methods are used by the emitter builder in a late bound fashion.
            //

            private static readonly MethodInfo s_create = typeof(EmitterWriterBuilder).GetMethod(nameof(Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createObjectEmitter = typeof(EmitterWriterBuilder).GetMethod(nameof(CreateObjectEmitter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createAnyObjectEmitter = typeof(EmitterWriterBuilder).GetMethod(nameof(CreateAnyObjectEmitter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createArrayEmitter = typeof(EmitterWriterBuilder).GetMethod(nameof(CreateArrayEmitter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createListEmitter = typeof(EmitterWriterBuilder).GetMethod(nameof(CreateListEmitter), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_nullOr = typeof(EmitterWriterBuilder).GetMethod(nameof(NullOr), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly MethodInfo s_writeChar = typeof(TextWriter).GetMethod("Write", new[] { typeof(char) });
            private static readonly MethodInfo s_writeString = typeof(TextWriter).GetMethod("Write", new[] { typeof(string) });

            //
            // NB: The following members and expression are used for runtime object reference cycle detection.
            //

            private static readonly FieldInfo s_cycles = typeof(EmitterContext).GetField("Cycles");
            private static readonly MethodInfo s_hashSetAdd = typeof(HashSet<object>).GetMethod("Add", new[] { typeof(object) });
            private static readonly MethodInfo s_hashSetRemove = typeof(HashSet<object>).GetMethod("Remove", new[] { typeof(object) });
            private static Expression s_throwCycle;

            //
            // NB: This dictionary maps types onto emitters (weakly typed due to their generic nature) and serves two purposes:
            //
            //     1. cache emitter functions that have been built already in order to speed up the compilation process
            //     2. keep a placeholder entry for a "forwarder" when building an emitter for an object as part of supporting cyclic types
            //
            // NB: We use a ConditionalWeakTable in order to avoid rooting collectible types in case the user holds a strong reference
            //     to the generated deserializer.
            //
            // THREADING: This type is thread-safe.
            //

            private readonly ConditionalWeakTable<Type, StrongBox<object>> _emitters = new();

            //
            // NB: Builder contexts are stateful but we don't want to allocate them for every late bound serializer site or even
            //     for every early bound serializer creation, so we use a pool.
            //
            // THREADING: This type is thread-safe.
            //

            private static readonly ObjectPool<BuilderContext> s_builderContextPool = new(() => new BuilderContext());

            //
            // THREADING: Implementations of INameProvider are assumed to be thread-safe.
            //

            private readonly INameProvider _provider;

            /// <summary>
            /// Creates a new emitter builder using the specified <paramref name="provider"/> to obtain JSON key names for members.
            /// </summary>
            /// <param name="provider">Provider used to obtain JSON key names for members.</param>
            public EmitterWriterBuilder(INameProvider provider) => _provider = provider;

            /// <summary>
            /// Creates a specialized emitter that can serialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build an emitter and serializer for.</typeparam>
            /// <returns>A specialized emitter that can serialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// This is the top-level entry method for the serializer factory to call.
            /// </remarks>
            public EmitWriterAction<T> Build<T>()
            {
                using var context = s_builderContextPool.New();

                return Create<T>(context.Object);
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build an emitter and serializer for.</typeparam>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// This is the top-level implementation method and also gets called recursively as we traverse a type's structure.
            /// </remarks>
            private EmitWriterAction<T> Create<T>(BuilderContext state)
            {
                var type = typeof(T);

                //
                // NB: We use static type analysis to decide whether we need cycle detection at runtime. If a recursive compilation call ends
                //     up here, we mark the type as having a cycle. The CompileObjectEmitter method checks for this flag after recursing into
                //     the compilation phase for all of the objects properties and fields.
                //

                var hasReferenceCycleTracking = false;

                if (!type.IsValueType)
                {
                    if (state.HasCycle.ContainsKey(type))
                    {
                        state.HasCycle[type] = true;
                    }
                    else
                    {
                        state.HasCycle[type] = false;
                        hasReferenceCycleTracking = true;
                    }
                }

                var res = CreateCore<T>(type, state);

                //
                // NB: After creating a serializer for the top-level occurrence of the type, we can safely remove the entry used for static
                //     type analysis.
                //

                if (hasReferenceCycleTracking)
                {
                    state.HasCycle.Remove(type);
                }

                return res;
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build an emitter and serializer for.</typeparam>
            /// <param name="type">The type of the object to build an emitter and serializer for.</param>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// Note that this method does the bookkeeping for caching of already built emitters and for the support for cyclic types.
            /// </remarks>
            private EmitWriterAction<T> CreateCore<T>(Type type, BuilderContext state)
            {
                var res = _emitters.GetOrCreateValue(type);
                if (res.Value == null)
                {
                    //
                    // The forwarder is an emitter that simply forwards to another emitter which is initially set to null. Upon
                    // detecting a cycle, the forwarder will be used for recursive serialization calls. After we got the
                    // underlying (real) emitter, we assign to the emitter making the forwarder call the real thing. This gets
                    // us support for serialization of cyclic types in a rather simple manner.
                    //
                    var emitter = default(EmitWriterAction<T>);

                    var forwarder = new EmitWriterAction<T>((TextWriter writer, T value, EmitterContext ctx) => emitter(writer, value, ctx));
                    res.Value = forwarder;

                    emitter = CreateEmitter<T>(type, state);
                    res.Value = emitter;

                    return emitter;
                }
                else
                {
                    return (EmitWriterAction<T>)res.Value;
                }
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build an emitter and serializer for.</typeparam>
            /// <param name="type">The type of the object to build an emitter and serializer for.</param>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// Don't call this method recursively. Always use <seealso cref="Create{T}"/> to recursively build up an emitter.
            /// </remarks>
            private EmitWriterAction<T> CreateEmitter<T>(Type type, BuilderContext state)
            {
                CheckTypeSupported(type);

                //
                // CONSIDER: Annotations on properties in objects which direct how a primitive should be deserialized, e.g.
                //           this could be used to control the format in which DateTime values are serialized. Adding this
                //           capability would likely add a parameter to CreateParser here.
                //

                if (type.IsArray)
                {
                    var elem = type.GetElementType();

                    if (elem.MakeArrayType() != type)
                    {
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON serializer for type '{0}' is not supported. Only single-dimensional arrays are supported.", type));
                    }

                    return (EmitWriterAction<T>)s_createArrayEmitter.MakeGenericMethod(elem).Invoke(this, new[] { state });
                }
                else if (type.IsConstructedGenericType)
                {
                    var def = type.GetGenericTypeDefinition();

                    if (def == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];

                        CheckTypeSupported(type);

                        switch (Type.GetTypeCode(type))
                        {
                            case TypeCode.Boolean:
                                return EmitterAs<bool?, T>(s_emitNullableBool ??= Emitter.EmitNullableBoolean);
                            case TypeCode.SByte:
                                return EmitterAs<sbyte?, T>(s_emitNullableSByte ??= Emitter.EmitNullableSByte);
                            case TypeCode.Byte:
                                return EmitterAs<byte?, T>(s_emitNullableByte ??= Emitter.EmitNullableByte);
                            case TypeCode.Int16:
                                return EmitterAs<short?, T>(s_emitNullableInt16 ??= Emitter.EmitNullableInt16);
                            case TypeCode.UInt16:
                                return EmitterAs<ushort?, T>(s_emitNullableUInt16 ??= Emitter.EmitNullableUInt16);
                            case TypeCode.Int32:
                                return EmitterAs<int?, T>(s_emitNullableInt32 ??= Emitter.EmitNullableInt32);
                            case TypeCode.UInt32:
                                return EmitterAs<uint?, T>(s_emitNullableUInt32 ??= Emitter.EmitNullableUInt32);
                            case TypeCode.Int64:
                                return EmitterAs<long?, T>(s_emitNullableInt64 ??= Emitter.EmitNullableInt64);
                            case TypeCode.UInt64:
                                return EmitterAs<ulong?, T>(s_emitNullableUInt64 ??= Emitter.EmitNullableUInt64);
                            case TypeCode.Char:
                                return EmitterAs<char?, T>(s_emitNullableChar ??= Emitter.EmitNullableChar);
                            case TypeCode.Single:
                                return EmitterAs<float?, T>(s_emitNullableSingle ??= Emitter.EmitNullableSingle);
                            case TypeCode.Double:
                                return EmitterAs<double?, T>(s_emitNullableDouble ??= Emitter.EmitNullableDouble);
                            case TypeCode.Decimal:
                                return EmitterAs<decimal?, T>(s_emitNullableDecimal ??= Emitter.EmitNullableDecimal);
                            case TypeCode.DateTime:
                                return EmitterAs<DateTime?, T>(s_emitNullableDateTime ??= Emitter.EmitNullableDateTime);
                        }

                        if (type == typeof(DateTimeOffset))
                        {
                            return EmitterAs<DateTimeOffset?, T>(s_emitNullableDateTimeOffset ??= Emitter.EmitNullableDateTimeOffset);
                        }

                        var nonNullEmitter = s_createObjectEmitter.MakeGenericMethod(type).Invoke(this, new[] { state });
                        var nullableEmitter = s_nullOr.MakeGenericMethod(type).Invoke(obj: null, new object[] { nonNullEmitter });
                        return (EmitWriterAction<T>)nullableEmitter;
                    }
                    else if (def == typeof(List<>) || def == typeof(IList<>) || def == typeof(IReadOnlyList<>) || def == typeof(IEnumerable<>))
                    {
                        //
                        // CONSIDER: Support assignability checking for these interfaces in order to allow serialization of more types? Some
                        //           concerns are the order to apply checks (e.g. IDictionary<,> before IEnumerable<>) and lack of knowledge
                        //           about the derived type's behavior and potential preference for different treatment (e.g. not as an Array
                        //           or as an Object). For now, we'll be conservative and only support a set of well-known types.
                        //

                        //
                        // CONSIDER: Add support for other commonly used collection types. This may need to be made extensible.
                        //

                        var elem = type.GetGenericArguments()[0];
                        return (EmitWriterAction<T>)s_createListEmitter.MakeGenericMethod(elem, type).Invoke(this, new[] { state });
                    }
                    else if (def == typeof(Dictionary<,>) || def == typeof(IDictionary<,>) || def == typeof(IReadOnlyDictionary<,>))
                    {
                        //
                        // CONSIDER: Support assignability checking for these interfaces in order to allow serialization of more types? Some
                        //           concerns are the order to apply checks (e.g. IDictionary<,> before IEnumerable<>) and lack of knowledge
                        //           about the derived type's behavior and potential preference for different treatment (e.g. not as an Array
                        //           or as an Object). For now, we'll be conservative and only support a set of well-known types.
                        //

                        var args = type.GetGenericArguments();

                        var keyType = args[0];
                        if (keyType != typeof(string))
                        {
                            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON serializer for type '{0}' is not supported. Only dictionaries with string keys are supported.", type));
                        }

                        var valueType = args[1];
                        return (EmitWriterAction<T>)s_createAnyObjectEmitter.MakeGenericMethod(valueType, type).Invoke(this, new[] { state });
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON serializer for type '{0}' is not supported.", type));
                    }
                }
                else if (type == typeof(DateTimeOffset))
                {
                    return EmitterAs<DateTimeOffset, T>(s_emitDateTimeOffset ??= Emitter.EmitDateTimeOffset);
                }
                else if (type == typeof(object))
                {
                    return EmitterAs<object, T>(s_emitAny ??= Emitter.EmitAny);
                }
                else
                {
                    return (Type.GetTypeCode(type)) switch
                    {
                        TypeCode.String => EmitterAs<string, T>(s_emitString ??= Emitter.EmitString),
                        TypeCode.Boolean => EmitterAs<bool, T>(s_emitBool ??= Emitter.EmitBoolean),
                        TypeCode.SByte => EmitterAs<sbyte, T>(s_emitSByte ??= Emitter.EmitSByte),
                        TypeCode.Byte => EmitterAs<byte, T>(s_emitByte ??= Emitter.EmitByte),
                        TypeCode.Int16 => EmitterAs<short, T>(s_emitInt16 ??= Emitter.EmitInt16),
                        TypeCode.UInt16 => EmitterAs<ushort, T>(s_emitUInt16 ??= Emitter.EmitUInt16),
                        TypeCode.Int32 => EmitterAs<int, T>(s_emitInt32 ??= Emitter.EmitInt32),
                        TypeCode.UInt32 => EmitterAs<uint, T>(s_emitUInt32 ??= Emitter.EmitUInt32),
                        TypeCode.Int64 => EmitterAs<long, T>(s_emitInt64 ??= Emitter.EmitInt64),
                        TypeCode.UInt64 => EmitterAs<ulong, T>(s_emitUInt64 ??= Emitter.EmitUInt64),
                        TypeCode.Char => EmitterAs<char, T>(s_emitChar ??= Emitter.EmitChar),
                        TypeCode.Single => EmitterAs<float, T>(s_emitSingle ??= Emitter.EmitSingle),
                        TypeCode.Double => EmitterAs<double, T>(s_emitDouble ??= Emitter.EmitDouble),
                        TypeCode.Decimal => EmitterAs<decimal, T>(s_emitDecimal ??= Emitter.EmitDecimal),
                        TypeCode.DateTime => EmitterAs<DateTime, T>(s_emitDateTime ??= Emitter.EmitDateTime),
                        _ => CreateObjectEmitter<T>(state),
                    };
                }
            }

            /// <summary>
            /// Checks whether the specified type is supported for serialization.
            /// </summary>
            /// <param name="type">The type to check.</param>
            private static void CheckTypeSupported(Type type)
            {
                //
                // CONSIDER: Add support for enums and nullable enums by allowing serialization of an enum value to a string and/or an integer number. For
                //           more info, see the corresponding code for the deserializer.
                //

                if (type.IsEnum)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Serialization of type '{0}' is not supported. The type is an enumeration.", type));

                //
                // NB: Pointer types and by-ref types cannot be used for generic parameters, so should not make it here.
                //

                Debug.Assert(!type.IsPointer);
                Debug.Assert(!type.IsByRef);
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize a JSON array with elements of type <typeparamref name="TElement"/>.
            /// </summary>
            /// <typeparam name="TElement">The type of the elements in the collection to serialize.</typeparam>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize an JSON array containing objects of type <typeparamref name="TElement"/> from an array instance.</returns>
            private EmitWriterAction<TElement[]> CreateArrayEmitter<TElement>(BuilderContext state)
            {
                var elementEmitter = Create<TElement>(state);

                return (TextWriter writer, TElement[] value, EmitterContext ctx) =>
                {
                    if (value == null)
                    {
                        writer.Write("null");
                    }
                    else if (value.Length == 0)
                    {
                        writer.Write("[]");
                    }
                    else
                    {
                        writer.Write("[");

                        for (var i = 0; i < value.Length; i++)
                        {
                            if (i > 0)
                            {
                                writer.Write(",");
                            }

                            var element = value[i];
                            elementEmitter(writer, element, ctx);
                        }

                        writer.Write("]");
                    }
                };
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize a JSON array with elements of type <typeparamref name="TElement"/>.
            /// </summary>
            /// <typeparam name="TElement">The type of the elements in the collection to serialize.</typeparam>
            /// <typeparam name="TCollection">The type of the collection to serialize. This collection type should implement IEnumerable{T} with <typeparamref name="TElement"/> as the element type.</typeparam>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize an JSON array containing objects of type <typeparamref name="TElement"/> from an instance of the <typeparamref name="TCollection"/> type.</returns>
            private EmitWriterAction<TCollection> CreateListEmitter<TElement, TCollection>(BuilderContext state)
                where TCollection : IEnumerable<TElement>
            {
                var elementEmitter = Create<TElement>(state);

                return (TextWriter writer, TCollection value, EmitterContext ctx) =>
                {
                    if (value == null)
                    {
                        writer.Write("null");
                    }
                    else
                    {
                        writer.Write("[");

                        using (var e = value.GetEnumerator())
                        {
                            if (e.MoveNext())
                            {
                                var element = e.Current;
                                elementEmitter(writer, element, ctx);

                                while (e.MoveNext())
                                {
                                    writer.Write(",");

                                    element = e.Current;
                                    elementEmitter(writer, element, ctx);
                                }
                            }
                        }

                        writer.Write("]");
                    }
                };
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize a JSON Object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build an emitter and serializer for.</typeparam>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize a JSON Object of type <typeparamref name="T"/>.</returns>
            private EmitWriterAction<T> CreateObjectEmitter<T>(BuilderContext state)
            {
                var type = typeof(T);

                if (type.IsInterface)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Serialization of type '{0}' is not supported. The type is an interface.", type));
                if (type.IsAbstract)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Serialization of type '{0}' is not supported. The type is abstract.", type));

                //
                // NB: Nullable value types are handled explicitly by the CreateEmitter method and will result in a call to CreateObjectEmitter<T>
                //     where T is the non-nullable type. The NullOr<T> emitter combinator is then applied to the result.
                //

                Debug.Assert(!(type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)));

                if (!type.IsValueType)
                {
                    var defaultCtor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, binder: null, ArrayBuilder<Type>.Empty, modifiers: null);

                    if (defaultCtor == null)
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Serialization of type '{0}' is not supported. The type does not have a default constructor.", type));
                }

                //
                // NB: We use dynamic code gen to build an efficient emitter to read object members and to dispatch into recursive serializers.
                //

                var compiledEmitter = CompileObjectEmitter<T>(type, state);

                return (TextWriter writer, T value, EmitterContext ctx) =>
                {
                    if (value == null)
                    {
                        writer.Write("null");
                    }
                    else
                    {
                        compiledEmitter(writer, value, ctx);
                    }
                };
            }

            /// <summary>
            /// Compiles an emitter that performs recursive serialization on the members of an object of the specified <paramref name="type"/> and emits a JSON Object.
            /// </summary>
            /// <typeparam name="T">The type of the object to serialize members for.</typeparam>
            /// <param name="type">The type of the object to serialize members for.</param>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A compiled emitter for the instances of the specified <paramref name="type"/>.</returns>
            private EmitWriterAction<T> CompileObjectEmitter<T>(Type type, BuilderContext state)
            {
                var bld = Expression.Parameter(typeof(TextWriter), "writer");
                var val = Expression.Parameter(typeof(T), "value");
                var ctx = Expression.Parameter(typeof(EmitterContext), "ctx");

                //
                // NB: We use alphabetical sorting for the keys in order to achieve determinism.
                //

                var members = new SortedDictionary<string, Expression>();

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.GetIndexParameters().Length == 0))
                {
                    var readPropertyAndEmit = GetReadAndEmitAction<T>(prop.PropertyType, prop, bld, val, ctx, state);

                    var name = _provider.GetName(prop);

                    members.Add(name, readPropertyAndEmit); // TODO: Improved error message when key is null or when key already exists.
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly)) // NB: Don't need to check for IsLiteral; these are static anyway.
                {
                    var readFieldAndAssign = GetReadAndEmitAction<T>(field.FieldType, field, bld, val, ctx, state);

                    var name = _provider.GetName(field);

                    members.Add(name, readFieldAndAssign); // TODO: Improved error message when key is null or when key already exists.
                }

                var stmts = new List<Expression>();

                //
                // NB: After we've made all recursive calls to compile serializers for properties and fields, we can detect if the static type
                //     analysis detected a cycle. If that's the case, we need to emit a runtime check for a cyclic object.
                //

                var hasReferenceCycleTracking = state.HasCycle.TryGetValue(type, out bool hasCycle) && hasCycle;

                if (hasReferenceCycleTracking)
                {
                    //
                    // if (value != null && !context.Cycles.Add(value))
                    //     throw new InvalidOperationException("An object reference cycle was detected.");
                    //

                    stmts.Add(
                        Expression.IfThen(
                            Expression.AndAlso(
                                Expression.ReferenceNotEqual(
                                    val,
                                    Expression.Constant(value: null, type)
                                ),
                                Expression.Not(
                                    Expression.Call(Expression.Field(ctx, s_cycles), s_hashSetAdd, val)
                                )
                            ),
                            s_throwCycle ??= Expression.Throw(Expression.New(typeof(InvalidOperationException).GetConstructor(new[] { typeof(string) }), Expression.Constant("An object reference cycle was detected."))))
                    );
                }

                stmts.Add(Expression.Call(bld, s_writeChar, Expression.Constant('{')));

                var n = members.Count;

                foreach (var kv in members)
                {
                    var name = kv.Key;
                    var expr = kv.Value;

                    var key = Emitter.ToJsonStringLiteral(name);

                    stmts.Add(Expression.Call(bld, s_writeString, Expression.Constant(key, typeof(string))));
                    stmts.Add(Expression.Call(bld, s_writeChar, Expression.Constant(':')));
                    stmts.Add(expr);

                    if (--n != 0)
                    {
                        stmts.Add(Expression.Call(bld, s_writeChar, Expression.Constant(',')));
                    }
                }

                stmts.Add(Expression.Call(bld, s_writeChar, Expression.Constant('}')));

                //
                // NB: After serializing an object that can contain a cycle, we can stop tracking it.
                //

                if (hasReferenceCycleTracking)
                {
                    //
                    // if (value != null)
                    //     context.Cycles.Remove(value);
                    //

                    stmts.Add(
                        Expression.IfThen(
                            Expression.ReferenceNotEqual(
                                val,
                                Expression.Constant(value: null, type)
                            ),
                            Expression.Call(Expression.Field(ctx, s_cycles), s_hashSetRemove, val)
                        )
                    );
                }

                var emitExpr = Expression.Lambda<EmitWriterAction<T>>(Expression.Block(typeof(void), stmts), bld, val, ctx);

                var compiledEmit = emitExpr.Compile();

                return compiledEmit;
            }

            /// <summary>
            /// Creates a recursive emitter function that can read and serialize an object of type <paramref name="elementType"/> from the specified <paramref name="member"/> in an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to serialize a member of.</typeparam>
            /// <param name="elementType">The type of the member to read serialize.</param>
            /// <param name="member">The member to read and serialize a value from.</param>
            /// <param name="writer">Parameter expression representing the text writer to append to.</param>
            /// <param name="value">Parameter expression representing the value to read the member from.</param>
            /// <param name="context">Parameter expression representing the emitter context.</param>
            /// <param name="builderContext">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>An expression containing the logic to read and serialize the specified member.</returns>
            private Expression GetReadAndEmitAction<T>(Type elementType, MemberInfo member, ParameterExpression writer, ParameterExpression value, ParameterExpression context, BuilderContext builderContext)
            {
                var emitter = s_create.MakeGenericMethod(elementType).Invoke(this, new[] { builderContext });

                var memberValue = Expression.MakeMemberAccess(value, member);
                var body = Expression.Invoke(Expression.Constant(emitter, typeof(EmitWriterAction<>).MakeGenericType(elementType)), writer, memberValue, context);

                return body;
            }

            /// <summary>
            /// Creates a specialized emitter that can serialize a dictionary whose values are of type <typeparamref name="TValue"/>.
            /// </summary>
            /// <typeparam name="TValue">The type of the values in the object to build an emitter and serializer for.</typeparam>
            /// <typeparam name="TDictionary">The type of the dictionary to serialize.</typeparam>
            /// <param name="state">The builder context used to maintain state on a per-invocation basis.</param>
            /// <returns>A specialized emitter that can serialize a dictionary whose values are of type <typeparamref name="TValue"/>.</returns>
            private EmitWriterAction<TDictionary> CreateAnyObjectEmitter<TValue, TDictionary>(BuilderContext state)
                where TDictionary : IEnumerable<KeyValuePair<string, TValue>>
            {
                var emitValue = Create<TValue>(state);
                return (TextWriter writer, TDictionary value, EmitterContext ctx) => Emitter.EmitAnyObject(writer, value, ctx, emitValue);
            }

            /// <summary>
            /// Emitter combinator that extends the specified emitter for a non-nullable value type by adding a check for a null reference to produce an emitter for a nullable value type.
            /// </summary>
            /// <typeparam name="T">The non-nullable value type of the original emitter.</typeparam>
            /// <param name="emitter">The original emitter to extend with null reference checking support.</param>
            /// <returns>An emitter that wraps the specified emitter and adds emission logic for null references.</returns>
            private static EmitWriterAction<T?> NullOr<T>(EmitWriterAction<T> emitter)
                where T : struct
            {
                return (TextWriter writer, T? value, EmitterContext ctx) =>
                {
                    if (value == null)
                    {
                        writer.Write("null");
                    }
                    else
                    {
                        emitter(writer, value.Value, ctx);
                    }
                };
            }

            /// <summary>
            /// Reinterpret cast for emitters, used to perform a cast when the compiler can't prove validity of a regular cast.
            /// </summary>
            /// <typeparam name="T">The type parameter of the specified emitter.</typeparam>
            /// <typeparam name="R">The type parameter of the returned emitter.</typeparam>
            /// <param name="emitter">The emitter to convert.</param>
            /// <returns>The converted emitter.</returns>
            private static EmitWriterAction<R> EmitterAs<T, R>(EmitWriterAction<T> emitter)
            {
                return (EmitWriterAction<R>)(object)emitter;
            }

            /// <summary>
            /// Holder for builder state used throughout the process of creating an emitter.
            /// </summary>
            private sealed class BuilderContext : IClearable
            {
                //
                // NB: We need to detect which custom types may introduce cycles which can cause the serializer to stack overflow.
                //     When we detect such a type, we'll insert logic that uses the EmitterContext to track object references and
                //     throw during serialization when an object cycle is detected.
                //

                public readonly Dictionary<Type, bool> HasCycle = new();

                /// <summary>
                /// Used to clear the state upon returning the instance to the pool.
                /// </summary>
                public void Clear() => HasCycle.Clear();
            }
        }
    }
}
#endif
