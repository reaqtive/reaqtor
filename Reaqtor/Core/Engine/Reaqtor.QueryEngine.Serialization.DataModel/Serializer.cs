// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Reflection;
using System.Text;
using System.Threading;

using Newtonsoft.Json;

using Nuqleon.DataModel.Serialization.Json;
using Nuqleon.Json.Interop.Newtonsoft;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Reactive.Expressions;

using JsonExpression = Nuqleon.Json.Expressions.Expression;

namespace Reaqtor.QueryEngine
{
    //
    // TODO: Create a single serializer that could be used in the system.
    //       Merge with Reaqtor.Client\Internal\SerializationHelpers.cs
    //

    /// <summary>
    /// Serializer used for checkpointing.
    /// </summary>
    internal sealed class Serializer : BonsaiExpressionSerializer, ISerializer, IDisposable
    {
        /// <summary>
        /// Resource pool for JSON expression readers and writers.
        /// </summary>
        private static readonly JsonInteropResourcePool s_pool = new();

        /// <summary>
        /// Hash set of other binary serializable types.
        /// </summary>
        private static readonly HashSet<Type> s_otherBinaryTypes = new()
        {
            typeof(string),
            typeof(decimal),
            typeof(Guid),
            typeof(Uri),
            typeof(TimeSpan),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Dictionary<string, object>),
        };

        //
        // NB: Note for StyleCop fanatics on the team. DO NOT FORMAT MY TABLES. I don't care for the "right style";
        //     my table style below is readable, yours is not. Let tables be tables.
        //

#pragma warning disable format // Formatted as tables.

        /// <summary>
        /// Early-bound serializers.
        /// </summary>
        private static readonly Dictionary<Type, Action<Serializer, object, Stream>> s_serializers = new()
        {
            { typeof(sbyte),          (serializer, value, stream) => serializer.SerializeCore(         (sbyte)value, stream) },
            { typeof(byte),           (serializer, value, stream) => serializer.SerializeCore(          (byte)value, stream) },
            { typeof(short),          (serializer, value, stream) => serializer.SerializeCore(         (short)value, stream) },
            { typeof(ushort),         (serializer, value, stream) => serializer.SerializeCore(        (ushort)value, stream) },
            { typeof(int),            (serializer, value, stream) => serializer.SerializeCore(           (int)value, stream) },
            { typeof(uint),           (serializer, value, stream) => serializer.SerializeCore(          (uint)value, stream) },
            { typeof(long),           (serializer, value, stream) => serializer.SerializeCore(          (long)value, stream) },
            { typeof(ulong),          (serializer, value, stream) => serializer.SerializeCore(         (ulong)value, stream) },
            { typeof(float),          (serializer, value, stream) => serializer.SerializeCore(         (float)value, stream) },
            { typeof(double),         (serializer, value, stream) => serializer.SerializeCore(        (double)value, stream) },
            { typeof(decimal),        (serializer, value, stream) => serializer.SerializeCore(       (decimal)value, stream) },
            { typeof(bool),           (serializer, value, stream) => serializer.SerializeCore(          (bool)value, stream) },
            { typeof(char),           (serializer, value, stream) => serializer.SerializeCore(          (char)value, stream) },
            { typeof(string),         (serializer, value, stream) => serializer.SerializeCore(        (string)value, stream) },
            { typeof(DateTime),       (serializer, value, stream) => serializer.SerializeCore(      (DateTime)value, stream) },
            { typeof(DateTimeOffset), (serializer, value, stream) => serializer.SerializeCore((DateTimeOffset)value, stream) },
            { typeof(TimeSpan),       (serializer, value, stream) => serializer.SerializeCore(      (TimeSpan)value, stream) },
            { typeof(Guid),           (serializer, value, stream) => serializer.SerializeCore(          (Guid)value, stream) },
            { typeof(Uri),            (serializer, value, stream) => serializer.SerializeCore(           (Uri)value, stream) },
        };

        /// <summary>
        /// Early-bound data serializers.
        /// </summary>
        private static readonly Dictionary<Type, Func<JsonDataSerializer, Action<object, JsonWriter>>> s_dataSerializers = new()
        {
            { typeof(sbyte),          serializer => (value, writer) => serializer.SerializeTo(         (sbyte)value, writer) },
            { typeof(byte),           serializer => (value, writer) => serializer.SerializeTo(          (byte)value, writer) },
            { typeof(short),          serializer => (value, writer) => serializer.SerializeTo(         (short)value, writer) },
            { typeof(ushort),         serializer => (value, writer) => serializer.SerializeTo(        (ushort)value, writer) },
            { typeof(int),            serializer => (value, writer) => serializer.SerializeTo(           (int)value, writer) },
            { typeof(uint),           serializer => (value, writer) => serializer.SerializeTo(          (uint)value, writer) },
            { typeof(long),           serializer => (value, writer) => serializer.SerializeTo(          (long)value, writer) },
            { typeof(ulong),          serializer => (value, writer) => serializer.SerializeTo(         (ulong)value, writer) },
            { typeof(float),          serializer => (value, writer) => serializer.SerializeTo(         (float)value, writer) },
            { typeof(double),         serializer => (value, writer) => serializer.SerializeTo(        (double)value, writer) },
            { typeof(decimal),        serializer => (value, writer) => serializer.SerializeTo(       (decimal)value, writer) },
            { typeof(bool),           serializer => (value, writer) => serializer.SerializeTo(          (bool)value, writer) },
            { typeof(char),           serializer => (value, writer) => serializer.SerializeTo(          (char)value, writer) },
            { typeof(string),         serializer => (value, writer) => serializer.SerializeTo(        (string)value, writer) },
            { typeof(DateTime),       serializer => (value, writer) => serializer.SerializeTo(      (DateTime)value, writer) },
            { typeof(DateTimeOffset), serializer => (value, writer) => serializer.SerializeTo((DateTimeOffset)value, writer) },
            { typeof(TimeSpan),       serializer => (value, writer) => serializer.SerializeTo(      (TimeSpan)value, writer) },
            { typeof(Guid),           serializer => (value, writer) => serializer.SerializeTo(          (Guid)value, writer) },
            { typeof(Uri),            serializer => (value, writer) => serializer.SerializeTo(           (Uri)value, writer) },
        };

        /// <summary>
        /// Early-bound deserializers.
        /// </summary>
        private static readonly Dictionary<Type, Func<Serializer, Stream, object>> s_deserializers = new()
        {
            { typeof(sbyte),          (serializer, stream) => serializer.DeserializeCore<         sbyte>(stream) },
            { typeof(byte),           (serializer, stream) => serializer.DeserializeCore<          byte>(stream) },
            { typeof(short),          (serializer, stream) => serializer.DeserializeCore<         short>(stream) },
            { typeof(ushort),         (serializer, stream) => serializer.DeserializeCore<        ushort>(stream) },
            { typeof(int),            (serializer, stream) => serializer.DeserializeCore<           int>(stream) },
            { typeof(uint),           (serializer, stream) => serializer.DeserializeCore<          uint>(stream) },
            { typeof(long),           (serializer, stream) => serializer.DeserializeCore<          long>(stream) },
            { typeof(ulong),          (serializer, stream) => serializer.DeserializeCore<         ulong>(stream) },
            { typeof(float),          (serializer, stream) => serializer.DeserializeCore<         float>(stream) },
            { typeof(double),         (serializer, stream) => serializer.DeserializeCore<        double>(stream) },
            { typeof(decimal),        (serializer, stream) => serializer.DeserializeCore<       decimal>(stream) },
            { typeof(bool),           (serializer, stream) => serializer.DeserializeCore<          bool>(stream) },
            { typeof(char),           (serializer, stream) => serializer.DeserializeCore<          char>(stream) },
            { typeof(string),         (serializer, stream) => serializer.DeserializeCore<        string>(stream) },
            { typeof(DateTime),       (serializer, stream) => serializer.DeserializeCore<      DateTime>(stream) },
            { typeof(DateTimeOffset), (serializer, stream) => serializer.DeserializeCore<DateTimeOffset>(stream) },
            { typeof(TimeSpan),       (serializer, stream) => serializer.DeserializeCore<      TimeSpan>(stream) },
            { typeof(Guid),           (serializer, stream) => serializer.DeserializeCore<          Guid>(stream) },
            { typeof(Uri),            (serializer, stream) => serializer.DeserializeCore<           Uri>(stream) },
        };

        /// <summary>
        /// Early-bound data deserializers.
        /// </summary>
        private static readonly Dictionary<Type, Func<JsonDataSerializer, Func<JsonReader, object>>> s_dataDeserializers = new()
        {
            { typeof(sbyte),          serializer => reader => serializer.DeserializeFrom<         sbyte>(reader) },
            { typeof(byte),           serializer => reader => serializer.DeserializeFrom<          byte>(reader) },
            { typeof(short),          serializer => reader => serializer.DeserializeFrom<         short>(reader) },
            { typeof(ushort),         serializer => reader => serializer.DeserializeFrom<        ushort>(reader) },
            { typeof(int),            serializer => reader => serializer.DeserializeFrom<           int>(reader) },
            { typeof(uint),           serializer => reader => serializer.DeserializeFrom<          uint>(reader) },
            { typeof(long),           serializer => reader => serializer.DeserializeFrom<          long>(reader) },
            { typeof(ulong),          serializer => reader => serializer.DeserializeFrom<         ulong>(reader) },
            { typeof(float),          serializer => reader => serializer.DeserializeFrom<         float>(reader) },
            { typeof(double),         serializer => reader => serializer.DeserializeFrom<        double>(reader) },
            { typeof(decimal),        serializer => reader => serializer.DeserializeFrom<       decimal>(reader) },
            { typeof(bool),           serializer => reader => serializer.DeserializeFrom<          bool>(reader) },
            { typeof(char),           serializer => reader => serializer.DeserializeFrom<          char>(reader) },
            { typeof(string),         serializer => reader => serializer.DeserializeFrom<        string>(reader) },
            { typeof(DateTime),       serializer => reader => serializer.DeserializeFrom<      DateTime>(reader) },
            { typeof(DateTimeOffset), serializer => reader => serializer.DeserializeFrom<DateTimeOffset>(reader) },
            { typeof(TimeSpan),       serializer => reader => serializer.DeserializeFrom<      TimeSpan>(reader) },
            { typeof(Guid),           serializer => reader => serializer.DeserializeFrom<          Guid>(reader) },
            { typeof(Uri),            serializer => reader => serializer.DeserializeFrom<           Uri>(reader) },
        };

#pragma warning restore format

        /// <summary>
        /// DeserializeCore{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_deserializeCore = ((MethodInfo)ReflectionHelpers.InfoOf((Serializer s) => s.DeserializeCore<object>(null))).GetGenericMethodDefinition();

        /// <summary>
        /// SerializeCore{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_serializeCore = ((MethodInfo)ReflectionHelpers.InfoOf((Serializer s) => s.SerializeCore(default(object), null))).GetGenericMethodDefinition();

        /// <summary>
        /// Deserialize{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_deserialize = ((MethodInfo)ReflectionHelpers.InfoOf((Serializer s) => s.Deserialize<object>(null))).GetGenericMethodDefinition();

        /// <summary>
        /// Serialize{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_serialize = ((MethodInfo)ReflectionHelpers.InfoOf((Serializer s) => s.Serialize(default(object), null))).GetGenericMethodDefinition();

        /// <summary>
        /// DeserializeFrom{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_jsonDeserializeFrom = ((MethodInfo)ReflectionHelpers.InfoOf((JsonDataSerializer s) => s.DeserializeFrom<object>(default(JsonReader)))).GetGenericMethodDefinition();

        /// <summary>
        /// SerializeTo{T} generic method.
        /// </summary>
        private static readonly MethodInfo s_jsonSerializeTo = ((MethodInfo)ReflectionHelpers.InfoOf((JsonDataSerializer s) => s.SerializeTo(default(object), default(JsonWriter)))).GetGenericMethodDefinition();

        /// <summary>
        /// The version of the Bonsai serialization format to use.
        /// </summary>
        private static readonly Version s_bonsaiVersion = System.Linq.Expressions.Bonsai.Serialization.BonsaiVersion.Default;

        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private readonly JsonDataSerializer _jsonSerializer;

        /// <summary>
        /// Thread-local 8 byte buffer used to insert a length prefix placeholder to a stream upon serialization.
        /// </summary>
        private readonly ThreadLocal<byte[]> _buffer;

        /// <summary>
        /// The expression policy used to configure serialization operations involving expression trees.
        /// </summary>
        private readonly IExpressionPolicy _expressionPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        /// <param name="expressionPolicy">The expression policy.</param>
        /// <param name="version">The serializer version.</param>
        public Serializer(IExpressionPolicy expressionPolicy, Version version)
            : base(s_bonsaiVersion, liftMemoizer: expressionPolicy?.LiftMemoizer, reduceMemoizer: expressionPolicy?.ReduceMemoizer)
        {
            _expressionPolicy = expressionPolicy ?? throw new ArgumentNullException(nameof(expressionPolicy));
            Version = version ?? throw new ArgumentNullException(nameof(version));

            _jsonSerializer = (JsonDataSerializer)DataSerializer.Create(this, new QuoteConverter(expressionPolicy));
            _buffer = new ThreadLocal<byte[]>(() => new byte[8]);
        }

        /// <summary>
        /// Gets the name of the serializer, used for tagging of serialized state.
        /// </summary>
        public string Name => "JSON";

        /// <summary>
        /// Get the version to deserialize with.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Disposes resources held by the serializer.
        /// </summary>
        public void Dispose()
        {
            _buffer.Dispose();
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="stream">The stream.</param>
        public void Serialize<T>(T value, Stream stream)
        {
            if (typeof(T) == typeof(object))
            {
                var type = value == null ? typeof(object) : value.GetType();
                SerializeCore(type.AssemblyQualifiedName, stream);
                SerializeCore(value, type, stream, useCoreMethod: true);
            }
            else
            {
                SerializeCore<T>(value, stream);
            }
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">Type of value.</param>
        /// <param name="stream">The stream.</param>
        public void Serialize(object value, Type type, Stream stream)
        {
            SerializeCore(value, type, stream, useCoreMethod: false);
        }

        private void SerializeCore<T>(T value, Stream stream)
        {
            if (Version < SerializerVersioning.v2 || !TrySerializeBinaryCore(value, stream))
            {
                long placeholderPosition = stream.Position;
                var buffer = _buffer.Value;
                stream.Write(buffer, 0, buffer.Length);

                var start = stream.Position;
                _jsonSerializer.Serialize(value, stream);
                var end = stream.Position;

                var length = end - start;
                var lengthBytes = BitConverter.GetBytes(length);
                stream.Position = placeholderPosition;
                stream.Write(lengthBytes, 0, 8);

                stream.Position = end;
            }
        }

        private void SerializeCore(object value, Type type, Stream stream, bool useCoreMethod)
        {
            if (s_serializers.TryGetValue(type, out var serialize))
            {
                serialize(this, value, stream);
            }
            else
            {
                var serializeMethod = useCoreMethod
                    ? s_serializeCore.MakeGenericMethod(type)
                    : s_serialize.MakeGenericMethod(type);
                serializeMethod.Invoke(this, new[] { value, stream });
            }
        }

        private bool TrySerializeBinaryCore<T>(T value, Stream stream)
        {
            var type = typeof(T);

            if (type.TryGetSize(out var size))
            {
                var buffer = new byte[size];
                Buffer.BlockCopy(new[] { value }, 0, buffer, 0, size);
                stream.Write(buffer, 0, size);
                return true;
            }
            else if (s_otherBinaryTypes.Contains(type))
            {
                if (type == typeof(string))
                {
                    var str = value.To(default(string));
                    if (str != null)
                    {
                        var buffer = Encoding.UTF8.GetBytes(str);
                        SerializeCore(buffer.Length, stream);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        SerializeCore(-1, stream);
                    }
                }
                else if (type == typeof(decimal))
                {
                    var allBits = decimal.GetBits(value.To(default(decimal)));
                    SerializeCore(allBits.Length, stream);
                    foreach (var bits in allBits)
                    {
                        SerializeCore(bits, stream);
                    }
                }
                else if (type == typeof(Guid))
                {
                    SerializeCore(value.ToString(), stream);
                }
                else if (type == typeof(Uri))
                {
                    var uri = value.To(default(Uri));
                    var uriString = uri?.AbsoluteUri;
                    SerializeCore(uriString, stream);
                }
                else if (type == typeof(TimeSpan))
                {
                    SerializeCore(value.To(default(TimeSpan)).Ticks, stream);
                }
                else if (type == typeof(DateTime))
                {
                    var dt = value.To(default(DateTime));
                    SerializeCore(dt.Ticks, stream);
                    SerializeCore((int)dt.Kind, stream);
                }
                else if (type == typeof(DateTimeOffset))
                {
                    var dto = value.To(default(DateTimeOffset));
                    SerializeCore(dto.Ticks, stream);
                    SerializeCore(dto.Offset, stream);
                }
                // This is a very narrow optimization chosen by the fact that
                // we use Dictionary<string, object> extensively for artifact
                // state. This approach would also work for arbitrary
                // Dictionary<TKey, TValue>, where TKey and TValue are both
                // data model types.
                else if (type == typeof(Dictionary<string, object>))
                {
                    var d = value.To(default(Dictionary<string, object>));
                    SerializeCore(d.Count, stream);
                    foreach (var kv in d)
                    {
                        SerializeCore(kv.Key, stream);
                        Serialize(kv.Value, stream);
                    }
                }
                else
                {
                    return false; // Should not get here...
                }

                return true;
            }
            else if (typeof(Expression).IsAssignableFrom(typeof(T)))
            {
                var serializedExpression = Serialize(Lift(value.To(default(Expression))));
                SerializeCore(serializedExpression, stream);
                return true;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                {
                    if (!object.Equals(value, default(T)))
                    {
                        SerializeCore(true, stream);
                        SerializeCore(value, underlyingType, stream, useCoreMethod: true);
                    }
                    else
                    {
                        SerializeCore(false, stream);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deserializes from the specified stream.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>Deserialized value.</returns>
        public T Deserialize<T>(Stream stream)
        {
            if (typeof(T) == typeof(object))
            {
                var typeName = DeserializeCore<string>(stream);
                if (string.IsNullOrEmpty(typeName))
                {
                    return default;
                }

                var type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Failed to load type '{0}'.", typeName));
                }

                return (T)DeserializeCore(type, stream, useCoreMethod: true);
            }
            else
            {
                return DeserializeCore<T>(stream);
            }
        }

        /// <summary>
        /// Deserializes from the specified stream.
        /// </summary>
        /// <param name="type">Type of value.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Deserialized value.</returns>
        public object Deserialize(Type type, Stream stream)
        {
            return DeserializeCore(type, stream, useCoreMethod: false);
        }

        private T DeserializeCore<T>(Stream stream)
        {
            if (Version >= SerializerVersioning.v2 && TryDeserializeBinaryCore<T>(stream, out var result))
            {
                return result;
            }
            else
            {
                var buffer = _buffer.Value;
                stream.Read(buffer, 0, buffer.Length);

                var length = BitConverter.ToInt64(buffer, 0);

                using var wrapper = new StreamSegment(stream, stream.Position, length);

                return _jsonSerializer.Deserialize<T>(wrapper);
            }
        }

        private object DeserializeCore(Type type, Stream stream, bool useCoreMethod)
        {
            if (s_deserializers.TryGetValue(type, out var deserialize))
            {
                return deserialize(this, stream);
            }
            else
            {
                var deserializeMethod = useCoreMethod
                    ? s_deserializeCore.MakeGenericMethod(type)
                    : s_deserialize.MakeGenericMethod(type);
                return deserializeMethod.Invoke(this, new object[] { stream });
            }
        }

        private bool TryDeserializeBinaryCore<T>(Stream stream, out T result)
        {
            var type = typeof(T);

            if (type.TryGetSize(out var size))
            {
                var buffer = new byte[size];
                var arr = new T[1];
                stream.Read(buffer, 0, size);
                Buffer.BlockCopy(buffer, 0, arr, 0, size);
                result = arr[0];
                return true;
            }
            else if (s_otherBinaryTypes.Contains(type))
            {
                if (type == typeof(string))
                {
                    var len = DeserializeCore<int>(stream);
                    if (len >= 0)
                    {
                        var buffer = new byte[len];
                        stream.Read(buffer, 0, len);
                        result = Encoding.UTF8.GetString(buffer, 0, len).To(default(T));
                    }
                    else
                    {
                        result = default;
                    }
                }
                else if (type == typeof(decimal))
                {
                    var len = DeserializeCore<int>(stream);
                    var allBits = new int[len];
                    for (var i = 0; i < len; ++i)
                    {
                        allBits[i] = DeserializeCore<int>(stream);
                    }
                    result = new decimal(allBits).To(default(T));
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(DeserializeCore<string>(stream)).To(default(T));
                }
                else if (type == typeof(Uri))
                {
                    var canonicalString = DeserializeCore<string>(stream);
                    if (canonicalString != null)
                    {
                        result = new Uri(canonicalString).To(default(T));
                    }
                    else
                    {
                        result = default;
                    }
                }
                else if (type == typeof(TimeSpan))
                {
                    result = TimeSpan.FromTicks(DeserializeCore<long>(stream)).To(default(T));
                }
                else if (type == typeof(DateTime))
                {
                    result = new DateTime(DeserializeCore<long>(stream), (DateTimeKind)DeserializeCore<int>(stream)).To(default(T));
                }
                else if (type == typeof(DateTimeOffset))
                {
                    result = new DateTimeOffset(DeserializeCore<long>(stream), DeserializeCore<TimeSpan>(stream)).To(default(T));
                }
                else if (type == typeof(Dictionary<string, object>))
                {
                    var count = DeserializeCore<int>(stream);
                    var d = new Dictionary<string, object>(count);
                    for (var i = 0; i < count; ++i)
                    {
                        var key = DeserializeCore<string>(stream);
                        var value = Deserialize<object>(stream);
                        d.Add(key, value);
                    }
                    result = d.To(default(T));
                }
                else
                {
                    result = default;
                    return false; // Should not get here...
                }
                return true;
            }
            else if (typeof(Expression).IsAssignableFrom(typeof(T)))
            {
                var serializedExpression = DeserializeCore<string>(stream);
                if (serializedExpression != null)
                {
                    result = Reduce(Deserialize(serializedExpression)).To(default(T));
                }
                else
                {
                    result = default;
                }
                return true;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                {
                    if (DeserializeCore<bool>(stream))
                    {
                        result = (T)DeserializeCore(underlyingType, stream, useCoreMethod: true);
                    }
                    else
                    {
                        result = default;
                    }
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <summary>
        /// JSON hook serialization factory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Serialization factory.</returns>
        protected override Func<object, JsonExpression> GetConstantSerializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var serialize = default(Action<object, JsonWriter>);

            if (s_dataSerializers.TryGetValue(type, out var serializeFactory))
            {
                serialize = serializeFactory(_jsonSerializer);
            }
            else
            {
                var method = s_jsonSerializeTo.MakeGenericMethod(new[] { type });

                var objParam = Expression.Parameter(typeof(object));
                var writerParam = Expression.Parameter(typeof(JsonWriter));

                var serializeExpr =
                    Expression.Lambda<Action<object, JsonWriter>>(
                        Expression.Call(
                            Expression.Constant(_jsonSerializer, typeof(JsonDataSerializer)),
                            method,
                            Expression.Convert(
                                objParam,
                                type
                            ),
                            writerParam
                        ),
                        objParam, writerParam
                    );

                serialize = serializeExpr.Compile();
            }

            return obj => SerializeJsonHook(obj, serialize);
        }

        /// <summary>
        /// Serialization hook.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="serialize">The serialization callback.</param>
        /// <returns>JSON expression.</returns>
        private static JsonExpression SerializeJsonHook(object obj, Action<object, JsonWriter> serialize)
        {
            using var jsonWriter = new JsonExpressionWriter(s_pool);

            serialize(obj, jsonWriter);
            return jsonWriter.Expression;
        }

        /// <summary>
        /// JSON hook de-serialization factory.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Deserialization function.</returns>
        protected override Func<JsonExpression, object> GetConstantDeserializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var deserialize = default(Func<JsonReader, object>);

            if (s_dataDeserializers.TryGetValue(type, out var deserializeFactory))
            {
                deserialize = deserializeFactory(_jsonSerializer);
            }
            else
            {
                var method = s_jsonDeserializeFrom.MakeGenericMethod(new[] { type });

                var readerParam = Expression.Parameter(typeof(JsonReader));

                var deserializeExpr =
                    Expression.Lambda<Func<JsonReader, object>>(
                        Expression.Convert(
                            Expression.Call(
                                Expression.Constant(_jsonSerializer, typeof(JsonDataSerializer)),
                                method,
                                readerParam
                            ),
                            typeof(object)
                        ),
                        readerParam
                    );

                deserialize = deserializeExpr.Compile();
            }

            return json => DeserializeJsonHook(json, deserialize);
        }

        /// <summary>
        /// Deserialization hook.
        /// </summary>
        /// <param name="json">The JSON expression.</param>
        /// <param name="deserialize">The deserialization callback.</param>
        /// <returns>Deserialized expression.</returns>
        private static object DeserializeJsonHook(JsonExpression json, Func<JsonReader, object> deserialize)
        {
            using var jsonReader = new JsonExpressionReader(json, s_pool);

            return deserialize(jsonReader);
        }

        /// <summary>
        /// Reduces a slim expression to an expression using the specified expression policy settings.
        /// </summary>
        /// <param name="expression">The slim expression.</param>
        /// <returns>The expression represented by the slim expression.</returns>
        public override Expression Reduce(ExpressionSlim expression)
        {
            if (expression == null)
            {
                return null;
            }

            return expression.ToExpression(_expressionPolicy.ExpressionFactory, _expressionPolicy.ReflectionProvider);
        }

        private sealed class QuoteConverter : DataConverter
        {
            private readonly IExpressionEvaluationPolicy _expressionPolicy;

            public QuoteConverter(IExpressionEvaluationPolicy expressionPolicy)
            {
                _expressionPolicy = expressionPolicy;
            }

            public override object ConvertFrom(object value, Type sourceType, Type targetType)
            {
                if (value == null)
                {
                    return null;
                }

                var expr = (Expression)value;

                //
                // TODO: If we need more quote types, we should generalize this mechanism.
                //

                var quoteType = default(Type);

                var grpType = targetType.FindGenericType(typeof(IGroupedSubscribable<,>));
                if (grpType != null)
                {
                    var genArgs = grpType.GetGenericArguments();
                    quoteType = typeof(QuotedGroupedSubscribable<,>).MakeGenericType(genArgs);
                }
                else
                {
                    var subType = targetType.FindGenericType(typeof(ISubscribable<>));
                    if (subType != null)
                    {
                        var genArgs = subType.GetGenericArguments();
                        quoteType = typeof(QuotedSubscribable<>).MakeGenericType(genArgs);
                    }
                }

                if (quoteType == null)
                {
                    throw new InvalidOperationException("Attempted to restore a checkpoint of a subscribable sequence to an incompatible type.");
                }

                return Activator.CreateInstance(quoteType, new object[] { expr, _expressionPolicy });
            }

            public override object ConvertTo(object value, Type sourceType, Type targetType)
            {
                Debug.Assert(targetType == typeof(Expression));

                if (value == null)
                {
                    return null;
                }

                if (value is not IExpressible expr)
                {
                    throw new InvalidOperationException("Attempted to checkpoint a non-serializable subscribable sequence.");
                }

                return expr.Expression;
            }

            public override bool TryCanConvert(Type fromType, out Type targetType)
            {
                //
                // TODO: If we need more quote types, we should generalize this mechanism.
                //

                if (fromType.FindGenericType(typeof(IGroupedSubscribable<,>)) != null)
                {
                    targetType = typeof(Expression);
                    return true;
                }

                if (fromType.FindGenericType(typeof(ISubscribable<>)) != null)
                {
                    targetType = typeof(Expression);
                    return true;
                }

                targetType = null;
                return false;
            }
        }
    }
}
