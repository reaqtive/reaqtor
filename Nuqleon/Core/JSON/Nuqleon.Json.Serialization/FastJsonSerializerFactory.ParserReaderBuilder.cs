﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

#if !NO_IO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Nuqleon.Json.Parser;

namespace Nuqleon.Json.Serialization
{
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// Utility to build parsers optimized for a specified type.
        /// </summary>
        /// <remarks>This type is thread-safe.</remarks>
        internal class ParserReaderBuilder
        {
            //
            // NB: These are static cached delegates for primitive type parsers; they get lazily set on first usage.
            //

            private static ParseReaderFunc<object> s_parseAny;

            private static ParseReaderFunc<string> s_parseString;

            private static ParseReaderFunc<bool> s_parseBool;
            private static ParseReaderFunc<sbyte> s_parseSByte;
            private static ParseReaderFunc<byte> s_parseByte;
            private static ParseReaderFunc<short> s_parseInt16;
            private static ParseReaderFunc<ushort> s_parseUInt16;
            private static ParseReaderFunc<int> s_parseInt32;
            private static ParseReaderFunc<uint> s_parseUInt32;
            private static ParseReaderFunc<long> s_parseInt64;
            private static ParseReaderFunc<ulong> s_parseUInt64;
            private static ParseReaderFunc<float> s_parseSingle;
            private static ParseReaderFunc<double> s_parseDouble;
            private static ParseReaderFunc<decimal> s_parseDecimal;
            private static ParseReaderFunc<char> s_parseChar;
            private static ParseReaderFunc<DateTime> s_parseDateTime;
            private static ParseReaderFunc<DateTimeOffset> s_parseDateTimeOffset;

            private static ParseReaderFunc<bool?> s_parseNullableBool;
            private static ParseReaderFunc<sbyte?> s_parseNullableSByte;
            private static ParseReaderFunc<byte?> s_parseNullableByte;
            private static ParseReaderFunc<short?> s_parseNullableInt16;
            private static ParseReaderFunc<ushort?> s_parseNullableUInt16;
            private static ParseReaderFunc<int?> s_parseNullableInt32;
            private static ParseReaderFunc<uint?> s_parseNullableUInt32;
            private static ParseReaderFunc<long?> s_parseNullableInt64;
            private static ParseReaderFunc<ulong?> s_parseNullableUInt64;
            private static ParseReaderFunc<float?> s_parseNullableSingle;
            private static ParseReaderFunc<double?> s_parseNullableDouble;
            private static ParseReaderFunc<decimal?> s_parseNullableDecimal;
            private static ParseReaderFunc<char?> s_parseNullableChar;
            private static ParseReaderFunc<DateTime?> s_parseNullableDateTime;
            private static ParseReaderFunc<DateTimeOffset?> s_parseNullableDateTimeOffset;

            //
            // NB: The following generic methods are used by the parser builder in a late bound fashion.
            //

            private static readonly MethodInfo s_create = typeof(ParserReaderBuilder).GetMethod(nameof(Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createObjectParser = typeof(ParserReaderBuilder).GetMethod(nameof(CreateObjectParser), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createAnyObjectParser = typeof(ParserReaderBuilder).GetMethod(nameof(CreateAnyObjectParser), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_createArrayParser = typeof(ParserReaderBuilder).GetMethod(nameof(CreateArrayParser), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            private static readonly MethodInfo s_nullOr = typeof(ParserReaderBuilder).GetMethod(nameof(NullOr), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly MethodInfo s_toArray = typeof(ParserReaderBuilder).GetMethod(nameof(ToArray), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            private static readonly MethodInfo s_toList = typeof(ParserReaderBuilder).GetMethod(nameof(ToList), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            //
            // NB: This dictionary maps types onto parsers (weakly typed due to their generic nature) and serves two purposes:
            //
            //     1. cache parser functions that have been built already in order to speed up the compilation process
            //     2. keep a placeholder entry for a "forwarder" when building a parser for an object as part of supporting cyclic types
            //
            // NB: We use a ConditionalWeakTable in order to avoid rooting collectible types in case the user holds a strong reference
            //     to the generated deserializer.
            //
            // THREADING: This type is thread-safe.
            //

            private readonly ConditionalWeakTable<Type, StrongBox<object>> _parsers = new();

            //
            // THREADING: Implementations of INameProvider are assumed to be thread-safe.
            //

            private readonly INameResolver _resolver;

            /// <summary>
            /// Creates a new parser builder using the specified <paramref name="resolver"/> to resolve JSON key names for members.
            /// </summary>
            /// <param name="resolver">Resolver used to resolve JSON key names for members.</param>
            public ParserReaderBuilder(INameResolver resolver) => _resolver = resolver;

            /// <summary>
            /// Creates a specialized parser that can deserialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build a parser and deserializer for.</typeparam>
            /// <returns>A specialized parser that can deserialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// This is the top-level entry method for the deserializer factory to call.
            /// </remarks>
            public ParseReaderFunc<T> Build<T>()
            {
                //
                // NB: This method is provided for symmetry with the emitter builder and can be used to seed parser builder
                //     state for use throughout the parser build process (see BuilderContext for an example).
                //

                return Create<T>();
            }

            /// <summary>
            /// Creates a specialized parser that can deserialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build a parser and deserializer for.</typeparam>
            /// <returns>A specialized parser that can deserialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// This is the top-level implementation method and also gets called recursively as we traverse a type's structure.
            /// Note that this method does the bookkeeping for caching of already built parsers and for the support for cyclic types.
            /// </remarks>
            private ParseReaderFunc<T> Create<T>()
            {
                var type = typeof(T);

                var res = _parsers.GetOrCreateValue(type);
                if (res.Value == null)
                {
                    //
                    // The forwarder is a parser that simply forwards to another parser which is initially set to null. Upon
                    // detecting a cycle, the forwarder will be used for recursive deserialization calls. After we got the
                    // underlying (real) parser, we assign to the parser making the forwarder call the real thing. This gets
                    // us support for deserialization of cyclic types in a rather simple manner.
                    //
                    var parser = default(ParseReaderFunc<T>);

                    var forwarder = new ParseReaderFunc<T>((TextReader reader, ParserContext ctx) => parser(reader, ctx));
                    res.Value = forwarder;

                    parser = CreateParser<T>(type);
                    res.Value = parser;

                    return parser;
                }
                else
                {
                    return (ParseReaderFunc<T>)res.Value;
                }
            }

            /// <summary>
            /// Creates a specialized parser that can deserialize an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build a parser and deserializer for.</typeparam>
            /// <param name="type">The type of the object to build a parser and deserializer for.</param>
            /// <returns>A specialized parser that can deserialize an object of type <typeparamref name="T"/>.</returns>
            /// <remarks>
            /// Don't call this method recursively. Always use <seealso cref="Create{T}"/> to recursively build up a parser.
            /// </remarks>
            private ParseReaderFunc<T> CreateParser<T>(Type type)
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
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON deserializer for type '{0}' is not supported. Only single-dimensional arrays are supported.", type));
                    }

                    var toArray = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(ArrayBuilder<>).MakeGenericType(elem), type), s_toArray.MakeGenericMethod(elem));
                    return (ParseReaderFunc<T>)s_createArrayParser.MakeGenericMethod(elem, type).Invoke(this, new object[] { toArray });
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
                                return ParserAs<bool?, T>(s_parseNullableBool ??= Parser.ParseNullableBoolean);
                            case TypeCode.SByte:
                                return ParserAs<sbyte?, T>(s_parseNullableSByte ??= Parser.ParseNullableSByte);
                            case TypeCode.Byte:
                                return ParserAs<byte?, T>(s_parseNullableByte ??= Parser.ParseNullableByte);
                            case TypeCode.Int16:
                                return ParserAs<short?, T>(s_parseNullableInt16 ??= Parser.ParseNullableInt16);
                            case TypeCode.UInt16:
                                return ParserAs<ushort?, T>(s_parseNullableUInt16 ??= Parser.ParseNullableUInt16);
                            case TypeCode.Int32:
                                return ParserAs<int?, T>(s_parseNullableInt32 ??= Parser.ParseNullableInt32);
                            case TypeCode.UInt32:
                                return ParserAs<uint?, T>(s_parseNullableUInt32 ??= Parser.ParseNullableUInt32);
                            case TypeCode.Int64:
                                return ParserAs<long?, T>(s_parseNullableInt64 ??= Parser.ParseNullableInt64);
                            case TypeCode.UInt64:
                                return ParserAs<ulong?, T>(s_parseNullableUInt64 ??= Parser.ParseNullableUInt64);
                            case TypeCode.Char:
                                return ParserAs<char?, T>(s_parseNullableChar ??= Parser.ParseNullableChar);
                            case TypeCode.Single:
                                return ParserAs<float?, T>(s_parseNullableSingle ??= Parser.ParseNullableSingle);
                            case TypeCode.Double:
                                return ParserAs<double?, T>(s_parseNullableDouble ??= Parser.ParseNullableDouble);
                            case TypeCode.Decimal:
                                return ParserAs<decimal?, T>(s_parseNullableDecimal ??= Parser.ParseNullableDecimal);
                            case TypeCode.DateTime:
                                return ParserAs<DateTime?, T>(s_parseNullableDateTime ??= Parser.ParseNullableDateTime);
                        }

                        if (type == typeof(DateTimeOffset))
                        {
                            return ParserAs<DateTimeOffset?, T>(s_parseNullableDateTimeOffset ??= Parser.ParseNullableDateTimeOffset);
                        }

                        var nonNullParser = s_createObjectParser.MakeGenericMethod(type).Invoke(this, ArrayBuilder<object>.Empty);
                        var nullableParser = s_nullOr.MakeGenericMethod(type).Invoke(obj: null, new object[] { nonNullParser });
                        return (ParseReaderFunc<T>)nullableParser;
                    }
                    else if (def == typeof(List<>) || def == typeof(IList<>) || def == typeof(IReadOnlyList<>) || def == typeof(IEnumerable<>))
                    {
                        //
                        // CONSIDER: Add support for other commonly used collection types. This may need to be made extensible.
                        //

                        var elemType = type.GetGenericArguments()[0];
                        var toList = Delegate.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(ArrayBuilder<>).MakeGenericType(elemType), type), s_toList.MakeGenericMethod(elemType));
                        return (ParseReaderFunc<T>)s_createArrayParser.MakeGenericMethod(elemType, type).Invoke(this, new object[] { toList });
                    }
                    else if (def == typeof(Dictionary<,>) || def == typeof(IDictionary<,>) || def == typeof(IReadOnlyDictionary<,>))
                    {
                        var args = type.GetGenericArguments();

                        var keyType = args[0];
                        if (keyType != typeof(string))
                        {
                            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON deserializer for type '{0}' is not supported. Only dictionaries with string keys are supported.", type));
                        }

                        var valueType = args[1];
                        return (ParseReaderFunc<T>)s_createAnyObjectParser.MakeGenericMethod(valueType).Invoke(this, ArrayBuilder<object>.Empty);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Creating a JSON deserializer for type '{0}' is not supported.", type));
                    }
                }
                else if (type == typeof(DateTimeOffset))
                {
                    return ParserAs<DateTimeOffset, T>(s_parseDateTimeOffset ??= Parser.ParseDateTimeOffset);
                }
                else if (type == typeof(object))
                {
                    return ParserAs<object, T>(s_parseAny ??= Parser.ParseAny);
                }
                else
                {
                    return (Type.GetTypeCode(type)) switch
                    {
                        TypeCode.String => ParserAs<string, T>(s_parseString ??= Parser.ParseString),
                        TypeCode.Boolean => ParserAs<bool, T>(s_parseBool ??= Parser.ParseBoolean),
                        TypeCode.SByte => ParserAs<sbyte, T>(s_parseSByte ??= Parser.ParseSByte),
                        TypeCode.Byte => ParserAs<byte, T>(s_parseByte ??= Parser.ParseByte),
                        TypeCode.Int16 => ParserAs<short, T>(s_parseInt16 ??= Parser.ParseInt16),
                        TypeCode.UInt16 => ParserAs<ushort, T>(s_parseUInt16 ??= Parser.ParseUInt16),
                        TypeCode.Int32 => ParserAs<int, T>(s_parseInt32 ??= Parser.ParseInt32),
                        TypeCode.UInt32 => ParserAs<uint, T>(s_parseUInt32 ??= Parser.ParseUInt32),
                        TypeCode.Int64 => ParserAs<long, T>(s_parseInt64 ??= Parser.ParseInt64),
                        TypeCode.UInt64 => ParserAs<ulong, T>(s_parseUInt64 ??= Parser.ParseUInt64),
                        TypeCode.Char => ParserAs<char, T>(s_parseChar ??= Parser.ParseChar),
                        TypeCode.Single => ParserAs<float, T>(s_parseSingle ??= Parser.ParseSingle),
                        TypeCode.Double => ParserAs<double, T>(s_parseDouble ??= Parser.ParseDouble),
                        TypeCode.Decimal => ParserAs<decimal, T>(s_parseDecimal ??= Parser.ParseDecimal),
                        TypeCode.DateTime => ParserAs<DateTime, T>(s_parseDateTime ??= Parser.ParseDateTime),
                        _ => CreateObjectParser<T>(),
                    };
                }
            }

            /// <summary>
            /// Checks whether the specified type is supported for deserialization.
            /// </summary>
            /// <param name="type">The type to check.</param>
            private static void CheckTypeSupported(Type type)
            {
                //
                // CONSIDER: Add support for enums and nullable enums by allowing deserialization of a string and/or an integer number to an enum value.
                //           Both representations can easily be turned into an enum value; when using an integer, we can just convert; when using a string,
                //           we can use the syntax trie functionality to calculate the value without calling Enum.Parse.
                //
                //           Flags enums are a bit more complex; we'd either have to stick with integer representations, or support sophisticated parsing
                //           of a , or | separated list of string representations, or require these to be represented as arrays of either integers or
                //           strings where each element represents a flag to be |'ed together.
                //

                if (type.IsEnum)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Deserialization of type '{0}' is not supported. The type is an enumeration.", type));

                //
                // NB: Pointer types and by-ref types cannot be used for generic parameters, so should not make it here.
                //

                Debug.Assert(!type.IsPointer);
                Debug.Assert(!type.IsByRef);
            }

            /// <summary>
            /// Creates a specialized parser that can deserialize a JSON array with elements of type <typeparamref name="TElement"/>.
            /// </summary>
            /// <typeparam name="TElement">The type of the elements in the collection to deserialize.</typeparam>
            /// <typeparam name="TCollection">The type of the collection to return. This collection type should have <typeparamref name="TElement"/> as the element type.</typeparam>
            /// <param name="getResult">Function to convert the array builder that's built during parsing into an instance of the specified <typeparamref name="TCollection"/> type. The array builder passed to the function may be null, requiring the function to return an empty collection.</param>
            /// <returns>A specialized parser that can deserialize an JSON array containing objects of type <typeparamref name="TElement"/> into an instance of the <typeparamref name="TCollection"/> type.</returns>
            private ParseReaderFunc<TCollection> CreateArrayParser<TElement, TCollection>(Func<ArrayBuilder<TElement>, TCollection> getResult)
                where TCollection : class
            {
                var elementParser = Create<TElement>();

                return (TextReader reader, ParserContext ctx) =>
                {
                    //
                    // NB: We don't have to skip leading whitespace. The top-level parser skips leading whitespace, and predecessors to an array
                    //     are either a [ token or a , token or a : token , which trim trailing whitespace prior to dispatching to this parser
                    //     function.
                    //
                    // Parser.SkipWhiteSpace(reader);
                    //

                    var c = reader.Peek();

                    switch (c)
                    {
                        case 'n':
                            {
                                if (reader.Read(ctx.char4, 0, 4) == 4)
                                {
                                    unsafe
                                    {
                                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                                        fixed (char* s = ctx.char4)
                                        {
                                            var val = *((long*)s);

                                            if (val == NULL)
                                            {
                                                return null;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case '[':
                            {
                                reader.Read();

                                Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the [ token.

                                c = reader.Peek();

                                if (c == ']')
                                {
                                    reader.Read();
                                    Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the ] token.
                                    return getResult(null);
                                }

                                var builder = ArrayBuilder.Create<TElement>();

                                while (c >= 0)
                                {
                                    builder.Add(elementParser(reader, ctx));

                                    //
                                    // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                                    //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                                    //           arrays which already guarantee trimming trailing whitespace.
                                    //

                                    Parser.SkipWhiteSpace(reader);

                                    c = reader.Read();

                                    switch (c)
                                    {
                                        case ',':
                                            Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the , token.
                                            break;
                                        case ']':
                                            Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the ] token.
                                            return getResult(builder);
                                        default:
                                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing array expecting either ',' or ']'.", (char)c), -1, ParseError.UnexpectedToken);
                                    }
                                }

                                //
                                // NB: We return from the loop upon encountering the closing ] token.
                                //

                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected end of stream when parsing array."), -1, ParseError.PrematureEndOfInput);
                            }
                    }

                    throw new ParseException("Expected Array or null.", -1, ParseError.UnexpectedToken);
                };
            }

            /// <summary>
            /// Creates a specialized parser that can deserialize a JSON Object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to build a parser and deserializer for.</typeparam>
            /// <returns>A specialized parser that can deserialize a JSON Object of type <typeparamref name="T"/>.</returns>
            private ParseReaderFunc<T> CreateObjectParser<T>()
            {
                var type = typeof(T);

                if (type.IsInterface)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Deserialization of type '{0}' is not supported. The type is an interface.", type));
                if (type.IsAbstract)
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Deserialization of type '{0}' is not supported. The type is abstract.", type));

                //
                // NB: Nullable value types are handled explicitly by the CreateParser method and will result in a call to CreateObjectParser<T>
                //     where T is the non-nullable type. The NullOr<T> parser combinator is then applied to the result.
                //

                Debug.Assert(!(type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)));

                var hasNull = !type.IsValueType;

                if (hasNull)
                {
                    var defaultCtor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, binder: null, ArrayBuilder<Type>.Empty, modifiers: null);

                    //
                    // CONSIDER: Add support for types that get instantiated through a constructor, e.g. anonymous types and tuples. This would
                    //           require the resolver to support GetNames for ParameterInfo objects from the constructor. Assignments need to
                    //           happen to locals, so we'd need to generate an expression with locals for each constructor argument and assign
                    //           these from the recursive parsing using the trie. Once the whole object is parsed, the constructor can be called
                    //           with these locals. The absence of certain JSON properties would translate into the use of default values, but
                    //           this may need to be reconsidered with a configuration option (we'd need Boolean values too in order to encode
                    //           the "undefined" state of a property). This holds for the current mode too.
                    //

                    if (defaultCtor == null)
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Deserialization of type '{0}' is not supported. The type does not have a default constructor.", type));
                }

                //
                // NB: We use dynamic code gen to build an efficient trie to match keys, dispatch into recursive deserializers for members, and
                //     perform assignments. For object creation, we use a simple NewExpression to beat slow Activator.CreateInstance<T> calls.
                //     Note that we could stitch all these fragments together in a bigger expression tree but initial experiments show that the
                //     gains are not very high.
                //

                var compiledTrie = CompileTrie<T>(type);

                //
                // PERF: We convert the IList<Assign<T>> to an Assign<T>[] to make indexing really cheap in the parser below. The use of an IList<T>
                //       leads to a bunch of virtual calls that can be avoided, while the use of an array simply turns into ldelem instructions.
                //

                var terminals = compiledTrie.Terminals.ToArray();

                var @new = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

                return (TextReader reader, ParserContext ctx) =>
                {
                    //
                    // NB: We don't have to skip leading whitespace. The top-level parser skips leading whitespace, and predecessors to an object
                    //     are either a [ token or a , token or a : token , which trim trailing whitespace prior to dispatching to this parser
                    //     function.
                    //
                    // Parser.SkipWhiteSpace(reader);
                    //

                    var c = reader.Peek();

                    switch (c)
                    {
                        case 'n':
                            {
                                if (hasNull && reader.Read(ctx.char4, 0, 4) == 4)
                                {
                                    unsafe
                                    {
                                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                                        fixed (char* s = ctx.char4)
                                        {
                                            var val = *((long*)s);

                                            if (val == NULL)
                                            {
                                                return default;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case '{':
                            {
                                var res = @new();

                                reader.Read();

                                Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the { token.

                                c = reader.Peek();

                                if (c == '}')
                                {
                                    reader.Read();
                                    Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the } token.
                                    return res;
                                }

                                while (c >= 0)
                                {
                                    c = reader.Read();

                                    if (c != '\"')
                                    {
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting a '\"' token for the begin of a key name.", (char)c), -1, ParseError.UnexpectedToken);
                                    }

                                    var assign = default(Assign<T>);

                                    if (compiledTrie.Eval(reader, out int idx))
                                    {
                                        assign = terminals[idx];
                                        reader.Read();
                                    }

                                    if (assign == null)
                                    {
                                        Parser.SkipString(reader);
                                    }

                                    Parser.SkipWhiteSpace(reader); // NB: This skips leading whitespace before the : token; we can't rely on the trie to do it.

                                    c = reader.Read();

                                    if (c != ':')
                                    {
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object. Unexpected character '{0}' found when a ':' was expected.", (char)c), -1, ParseError.UnexpectedToken);
                                    }

                                    Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the : token.

                                    if (assign != null)
                                    {
                                        assign(ref res, reader, ctx);
                                    }
                                    else
                                    {
                                        Parser.SkipOne(reader);
                                    }

                                    //
                                    // CONSIDER: Parsers for primitives don't trim trailing whitespace. We could change this and eliminate the need to
                                    //           skip whitespace here ourselves. This trimming is duplicate work if we're dealing with objects or
                                    //           arrays which already guarantee trimming trailing whitespace.
                                    //

                                    Parser.SkipWhiteSpace(reader);

                                    c = reader.Read();

                                    switch (c)
                                    {
                                        case ',':
                                            Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the , token.
                                            break;
                                        case '}':
                                            Parser.SkipWhiteSpace(reader); // NB: We *have* to skip trailing whitespace after the } token.
                                            return res;
                                        default:
                                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected character '{0}' encountered when parsing object expecting either ',' or '}}'.", (char)c), -1, ParseError.UnexpectedToken);
                                    }
                                }

                                throw new ParseException("Unexpected end of stream when parsing object and expecting a key.", -1, ParseError.PrematureEndOfInput);
                            }
                    }

                    if (hasNull)
                    {
                        throw new ParseException("Expected Object or null.", -1, ParseError.UnexpectedToken);
                    }
                    else
                    {
                        throw new ParseException("Expected Object.", -1, ParseError.UnexpectedToken);
                    }
                };
            }

            /// <summary>
            /// Compiles a trie that maps JSON Object keys onto assignment delegates that perform recursive deserialization and assignment to members.
            /// </summary>
            /// <typeparam name="T">The type of the object to deserialize members for.</typeparam>
            /// <param name="type">The type of the object to deserialize members for.</param>
            /// <returns>A compiled trie with terminals that are precompiled assignment delegates.</returns>
            private CompiledTrieReader<Assign<T>> CompileTrie<T>(Type type)
            {
                var trie = new SyntaxTrie<Assign<T>>();

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite && p.GetIndexParameters().Length == 0))
                {
                    var parsePropertyAndAssign = GetParseAndAssignFunction<T>(prop.PropertyType, prop);

                    foreach (var name in _resolver.GetNames(prop))
                    {
                        trie.Add(name, parsePropertyAndAssign);
                    }
                }

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly)) // NB: Don't need to check for IsLiteral; these are static anyway.
                {
                    var parseFieldAndAssign = GetParseAndAssignFunction<T>(field.FieldType, field);

                    foreach (var name in _resolver.GetNames(field))
                    {
                        trie.Add(name, parseFieldAndAssign);
                    }
                }

                var compiledTrie = trie.CompileReader();

                return compiledTrie;
            }

            /// <summary>
            /// Creates a recursive parser function that can deserialize an object of type <paramref name="elementType"/> and assign it to the specified <paramref name="member"/> in an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of the object to assign a member of.</typeparam>
            /// <param name="elementType">The type of the member to deserialize and assign.</param>
            /// <param name="member">The member to deserialize a value for and to assign the value to.</param>
            /// <returns>An assignment delegate that, given an object and a string input, can deserialize a value and assign it to the specified member.</returns>
            private Assign<T> GetParseAndAssignFunction<T>(Type elementType, MemberInfo member)
            {
                var parser = s_create.MakeGenericMethod(elementType).Invoke(this, ArrayBuilder<object>.Empty);

                var res = Expression.Parameter(typeof(T).MakeByRefType(), "res");
                var rdr = Expression.Parameter(typeof(TextReader), "reader");
                var ctx = Expression.Parameter(typeof(ParserContext), "ctx");

                var value = Expression.Invoke(Expression.Constant(parser, typeof(ParseReaderFunc<>).MakeGenericType(elementType)), rdr, ctx);
                var body = Expression.Assign(Expression.MakeMemberAccess(res, member), value);

                return Expression.Lambda<Assign<T>>(body, res, rdr, ctx).Compile();
            }

            /// <summary>
            /// Creates a specialized parser that can deserialize a JSON Object whose properties have values of type <typeparamref name="TValue"/>.
            /// </summary>
            /// <typeparam name="TValue">The type of the values of the properties in the object to build a parser and deserializer for.</typeparam>
            /// <returns>A specialized parser that can deserialize a JSON Object whose properties have values of type <typeparamref name="TValue"/>.</returns>
            private ParseReaderFunc<Dictionary<string, TValue>> CreateAnyObjectParser<TValue>()
            {
                var parseValue = Create<TValue>();
                return (TextReader reader, ParserContext ctx) => Parser.ParseAnyObject(reader, ctx, parseValue);
            }

            /// <summary>
            /// Parser combinator that extends the specified parser for a non-nullable value type by adding a check for a JSON null literal to produce a parser for a nullable value type.
            /// </summary>
            /// <typeparam name="T">The non-nullable value type of the original parser.</typeparam>
            /// <param name="parser">The original parser to extend with JSON null literal checking support.</param>
            /// <returns>A parser that wraps the specified parser and adds parsing logic to match a JSON null literal.</returns>
            private static ParseReaderFunc<T?> NullOr<T>(ParseReaderFunc<T> parser)
                where T : struct
            {
                return (TextReader reader, ParserContext ctx) =>
                {
                    //
                    // NB: We don't have to skip leading whitespace. The top-level parser skips leading whitespace, and predecessors to an object
                    //     are either a [ token or a , token or a : token , which trim trailing whitespace prior to dispatching to this parser
                    //     function.
                    //
                    // Parser.SkipWhiteSpace(reader);
                    //

                    var c = reader.Peek();

                    switch (c)
                    {
                        case 'n':
                            {
                                if (reader.Read(ctx.char4, 0, 4) == 4)
                                {
                                    unsafe
                                    {
                                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                                        fixed (char* s = ctx.char4)
                                        {
                                            var val = *((long*)s);

                                            if (val == NULL)
                                            {
                                                return null;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            return parser(reader, ctx);
                    }

                    throw new ParseException("Expected Object or null.", -1, ParseError.UnexpectedToken);
                };
            }

            /// <summary>
            /// Reinterpret cast for parsers, used to perform a cast when the compiler can't prove validity of a regular cast.
            /// </summary>
            /// <typeparam name="T">The type parameter of the specified parser.</typeparam>
            /// <typeparam name="R">The type parameter of the returned parser.</typeparam>
            /// <param name="parser">The parser to convert.</param>
            /// <returns>The converted parser.</returns>
            private static ParseReaderFunc<R> ParserAs<T, R>(ParseReaderFunc<T> parser) => (ParseReaderFunc<R>)(object)parser;

            /// <summary>
            /// Converts an array builder to a list containing the elements in the builder. If the builder is null, an empty list is returned.
            /// </summary>
            /// <typeparam name="T">The type of the elements in the array builder.</typeparam>
            /// <param name="builder">The array builder to convert to a list.</param>
            /// <returns>A list containing the elements in the builder.</returns>
            private static List<T> ToList<T>(ArrayBuilder<T> builder) => builder != null ? builder.ToList() : new List<T>();

            /// <summary>
            /// Converts an array builder to an array containing the elements in the builder. If the builder is null, a singleton of an empty array is returned.
            /// </summary>
            /// <typeparam name="T">The type of the elements in the array builder.</typeparam>
            /// <param name="builder">The array builder to convert to an array.</param>
            /// <returns>An array containing the elements in the builder.</returns>
            private static T[] ToArray<T>(ArrayBuilder<T> builder) => builder != null ? builder.ToArray() : ArrayBuilder<T>.Empty;

            /// <summary>
            /// Delegate that combines a recursive deserialization from the specified string with an assignment to a member in the specified object.
            /// </summary>
            /// <typeparam name="T">The type of the object to assign a member of.</typeparam>
            /// <param name="res">A reference to the object being initialized.</param>
            /// <param name="reader">The text reader to read from.</param>
            /// <param name="ctx">The parser context.</param>
            private delegate void Assign<T>(ref T res, TextReader reader, ParserContext ctx);
        }
    }
}
#endif
