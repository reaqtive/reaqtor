// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#define USE_POOL

using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Serializer to JSON.
    /// </summary>
    public sealed class JsonDataSerializer : DataSerializer
    {
        /// <summary>
        /// Default cache buffer size.
        /// </summary>
        private const int DEFAULT_CACHE_BUFFER_SIZE = 1024;

        /// <summary>
        /// The cache buffer size which is used for StreamReader and StreamWriter.
        /// </summary>
        private readonly int _cacheBufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class without support for expression serialization.
        /// </summary>
        public JsonDataSerializer()
            : this(DEFAULT_CACHE_BUFFER_SIZE)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class without support for expression serialization.
        /// </summary>
        /// <param name="cacheBufferSize">Size of the cache buffer.</param>
        public JsonDataSerializer(int cacheBufferSize) => _cacheBufferSize = cacheBufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization callback for expressions.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        public JsonDataSerializer(
            IExpressionSerializer expressionSerializer,
            bool includePrivate,
            params DataConverter[] converters)
            : this(expressionSerializer, DEFAULT_CACHE_BUFFER_SIZE, includePrivate, converters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class with support for expression serialization.
        /// </summary>
        /// <param name="serializeExpression">Serialization callback for expressions.</param>
        /// <param name="deserializeExpression">Deserialization callback for expressions.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        public JsonDataSerializer(
            Func<Expression, string> serializeExpression,
            Func<string, Expression> deserializeExpression,
            bool includePrivate,
            params DataConverter[] converters)
            : this(serializeExpression, deserializeExpression, DEFAULT_CACHE_BUFFER_SIZE, includePrivate, converters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization callback for expressions.</param>
        /// <param name="cacheBufferSize">Size of the cache buffer.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        public JsonDataSerializer(
            IExpressionSerializer expressionSerializer,
            int cacheBufferSize,
            bool includePrivate,
            params DataConverter[] converters)
#pragma warning disable 618
            : base(includePrivate, converters)
#pragma warning restore 618
        {
            if (expressionSerializer == null)
            {
                throw new ArgumentNullException(nameof(expressionSerializer));
            }

            var expressionConverter = new ExpressionJsonConverter(expressionSerializer);
            Serializer.Converters.Add(expressionConverter);

            _cacheBufferSize = cacheBufferSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataSerializer" /> class with support for expression serialization.
        /// </summary>
        /// <param name="serializeExpression">Serialization callback for expressions.</param>
        /// <param name="deserializeExpression">Deserialization callback for expressions.</param>
        /// <param name="cacheBufferSize">Size of the cache buffer.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        public JsonDataSerializer(
            Func<Expression, string> serializeExpression,
            Func<string, Expression> deserializeExpression,
            int cacheBufferSize,
            bool includePrivate,
            params DataConverter[] converters)
#pragma warning disable 618
            : base(includePrivate, converters)
#pragma warning restore 618
        {
            if (serializeExpression == null)
            {
                throw new ArgumentNullException(nameof(serializeExpression));
            }
            if (deserializeExpression == null)
            {
                throw new ArgumentNullException(nameof(deserializeExpression));
            }

            var expressionConverter = new ExpressionJsonConverter(serializeExpression, deserializeExpression);
            Serializer.Converters.Add(expressionConverter);

            _cacheBufferSize = cacheBufferSize;
        }

        // PERF: Consider direct conversions with the Nuqleon.Json object model through:
        //       - a Nuqleon.Json tokenizer wrapped in a JsonTextReader
        //       - a JsonTextWriter wrapped in a Nuqleon.Json printing visitor

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="writer">The JSON writer to write to.</param>
        /// <exception cref="ArgumentNullException">The JSON writer is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be serialized. Please check the inner exception.</exception>
        public void SerializeTo<T>(T value, JsonWriter writer)
        {
            // COMPAT: Users of DataSerializer tend to use reflection to discover the Serialize<T>
            //         method on the runtime type of a serializer, for late bound calls. To avoid
            //         breaking these with ambiguous matches during a call to GetMethod, we use a
            //         suffix in the method name.

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            try
            {
                Serializer.Serialize(writer, value);
            }
            catch (JsonException ex)
            {
                throw SerializeError(ex);
            }
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="writer">The text writer to write to.</param>
        /// <exception cref="ArgumentNullException">The text writer is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be serialized. Please check the inner exception.</exception>
        public void SerializeTo<T>(T value, TextWriter writer)
        {
            // COMPAT: Users of DataSerializer tend to use reflection to discover the Serialize<T>
            //         method on the runtime type of a serializer, for late bound calls. To avoid
            //         breaking these with ambiguous matches during a call to GetMethod, we use a
            //         suffix in the method name.

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            try
            {
#if USE_POOL
                using var jsonWriter = new JsonTextWriter(writer) { ArrayPool = CharArrayPool.Instance };
#else
                using var jsonWriter = new JsonTextWriter(writer);
#endif
                Serializer.Serialize(jsonWriter, value);
            }
            catch (JsonException ex)
            {
                throw SerializeError(ex);
            }
        }

        /// <summary>
        /// Deserializes the serialized stream.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="reader">The JSON reader to read from.</param>
        /// <returns>
        /// A deserialized object.
        /// </returns>
        /// <exception cref="ArgumentNullException">The JSON reader is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be deserialized. Please check the inner exception.</exception>
        public T DeserializeFrom<T>(JsonReader reader)
        {
            // COMPAT: Users of DataSerializer tend to use reflection to discover the Deserialize<T>
            //         method on the runtime type of a serializer, for late bound calls. To avoid
            //         breaking these with ambiguous matches during a call to GetMethod, we use a
            //         suffix in the method name.

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            try
            {
                return Serializer.Deserialize<T>(reader);
            }
            catch (JsonException ex)
            {
                throw DeserializeError(ex);
            }
        }

        /// <summary>
        /// Deserializes the serialized stream.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="reader">The text reader to read from.</param>
        /// <returns>
        /// A deserialized object.
        /// </returns>
        /// <exception cref="ArgumentNullException">The text reader is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be deserialized. Please check the inner exception.</exception>
        public T DeserializeFrom<T>(TextReader reader)
        {
            // COMPAT: Users of DataSerializer tend to use reflection to discover the Deserialize<T>
            //         method on the runtime type of a serializer, for late bound calls. To avoid
            //         breaking these with ambiguous matches during a call to GetMethod, we use a
            //         suffix in the method name.

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            try
            {
#if USE_POOL
                using var jsonReader = new JsonTextReader(reader) { ArrayPool = CharArrayPool.Instance };
#else
                using var jsonReader = new JsonTextReader(reader);
#endif
                return Serializer.Deserialize<T>(jsonReader);
            }
            catch (JsonException ex)
            {
                throw DeserializeError(ex);
            }
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="serialized">The serialized object.</param>
        protected override void SerializeCore<T>(T value, Stream serialized)
        {
            Debug.Assert(serialized != null, "Stream is not allowed to be null.");

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(serialized, Encoding.UTF8, _cacheBufferSize, leaveOpen: true);
#if USE_POOL
                using var jsonWriter = new JsonTextWriter(sw) { ArrayPool = CharArrayPool.Instance };
#else
                using var jsonWriter = new JsonTextWriter(sw);
#endif
                sw = null;
                Serializer.Serialize(jsonWriter, value);
            }
            finally
            {
                sw?.Dispose();
            }
        }

        /// <summary>
        /// Deserializes the serialized stream.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="serialized">The serialized object.</param>
        /// <returns>
        /// A deserialized object.
        /// </returns>
        protected override T DeserializeCore<T>(Stream serialized)
        {
            Debug.Assert(serialized != null, "Stream is not allowed to be null.");

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(serialized, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: _cacheBufferSize, leaveOpen: true);
#if USE_POOL
                using var jsonReader = new JsonTextReader(sr) { ArrayPool = CharArrayPool.Instance };
#else
                using var jsonReader = new JsonTextReader(sr);
#endif
                sr = null;
                return Serializer.Deserialize<T>(jsonReader);
            }
            finally
            {
                sr?.Dispose();
            }
        }
    }
}
