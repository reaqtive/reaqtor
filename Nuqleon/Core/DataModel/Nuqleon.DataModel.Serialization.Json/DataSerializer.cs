// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Base class for data serializer(s).
    /// </summary>
    public abstract class DataSerializer
    {
        /// <summary>
        /// Creates a new serializer instance.
        /// </summary>
        protected DataSerializer()
            : this(Array.Empty<DataConverter>())
        {
        }

        /// <summary>
        /// Creates a new serializer instance.
        /// </summary>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        protected DataSerializer(DataConverter[] converters)
            : this(CreateContractResolver(includePrivate: false), converters)
        {
        }

        /// <summary>
        /// Creates a new serializer instance.
        /// </summary>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        [Obsolete("Private reflection is no longer supported, please use alternative constructor.")]
        protected DataSerializer(bool includePrivate, params DataConverter[] converters)
            : this(CreateContractResolver(includePrivate), converters)
        {
        }

        private DataSerializer(IContractResolver resolver, DataConverter[] converters)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver
            };

            Serializer = JsonSerializer.Create(settings);

            if (converters != null)
            {
                foreach (var converter in converters)
                {
                    Serializer.Converters.Add(converter.Converter);
                }
            }
        }

        /// <summary>
        /// Gets the underlying JSON serializer.
        /// </summary>
        protected JsonSerializer Serializer { get; }

        /// <summary>
        /// Creates an instance of the data serializer, with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization callback for expressions.</param>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        public static DataSerializer Create(IExpressionSerializer expressionSerializer)
        {
#pragma warning disable 618
            return Create(expressionSerializer, includePrivate: false);
#pragma warning restore 618
        }

        /// <summary>
        /// Creates an instance of the data serializer, with support for expression serialization.
        /// </summary>
        /// <param name="serializeExpression">Serialization callback for expressions.</param>
        /// <param name="deserializeExpression">Deserialization callback for expressions.</param>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        public static DataSerializer Create(Func<Expression, string> serializeExpression, Func<string, Expression> deserializeExpression)
        {
#pragma warning disable 618
            return Create(serializeExpression, deserializeExpression, includePrivate: false);
#pragma warning restore 618
        }

        /// <summary>
        /// Creates an instance of the data serializer, with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization hooks for expressions.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        [Obsolete("Private reflection is no longer supported, please use alternative factory.")]
        public static DataSerializer Create(IExpressionSerializer expressionSerializer, bool includePrivate, params DataConverter[] converters)
        {
            return new JsonDataSerializer(expressionSerializer, includePrivate, converters);
        }

        /// <summary>
        /// Creates an instance of the data serializer, with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization hooks for expressions.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        public static DataSerializer Create(IExpressionSerializer expressionSerializer, params DataConverter[] converters)
        {
            return new JsonDataSerializer(expressionSerializer, includePrivate: false, converters);
        }

        /// <summary>
        /// Creates an instance of the data serializer, with support for expression serialization.
        /// </summary>
        /// <param name="serializeExpression">Serialization hooks for expressions.</param>
        /// <param name="deserializeExpression">Deserialization hooks for expressions.</param>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        /// <param name="converters">Custom converters to use during serialization and deserialization.</param>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        [Obsolete("Private reflection is no longer supported, please use alternative factory.")]
        public static DataSerializer Create(
            Func<Expression, string> serializeExpression,
            Func<string, Expression> deserializeExpression,
            bool includePrivate,
            params DataConverter[] converters)
        {
            return new JsonDataSerializer(serializeExpression, deserializeExpression, includePrivate, converters);
        }

        /// <summary>
        /// Creates an instance of the data serializer, without support for expression serialization.
        /// </summary>
        /// <returns>
        /// A new instance of the serializer.
        /// </returns>
        public static DataSerializer Create() => new JsonDataSerializer();

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="serialized">The serialized output.</param>
        /// <exception cref="ArgumentNullException">The output stream is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be serialized. Please check the inner exception.</exception>
        public void Serialize<T>(T value, Stream serialized)
        {
            if (serialized == null)
            {
                throw new ArgumentNullException(nameof(serialized));
            }

            try
            {
                SerializeCore(value, serialized);
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
        /// <param name="serialized">The serialized object.</param>
        /// <returns>
        /// A deserialized object.
        /// </returns>
        /// <exception cref="ArgumentNullException">The input stream is not allowed to be null.</exception>
        /// <exception cref="DataSerializerException">The object cannot be deserialized. Please check the inner exception.</exception>
        public T Deserialize<T>(Stream serialized)
        {
            if (serialized == null)
            {
                throw new ArgumentNullException(nameof(serialized));
            }

            try
            {
                return DeserializeCore<T>(serialized);
            }
            catch (JsonException ex)
            {
                throw DeserializeError(ex);
            }
        }

        /// <summary>
        /// Internal implementation of serialization.
        /// </summary>
        /// <typeparam name="T">Type of serialized objects.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="serialized">The serialized.</param>
        protected abstract void SerializeCore<T>(T value, Stream serialized);

        /// <summary>
        /// Internal implementation of (de)serialization.
        /// </summary>
        /// <typeparam name="T">Type of deserialized object.</typeparam>
        /// <param name="serialized">The serialized.</param>
        /// <returns>An instance of deserialized object.</returns>
        protected abstract T DeserializeCore<T>(Stream serialized);

        private static IContractResolver CreateContractResolver(bool includePrivate)
        {
            if (includePrivate)
            {
                return MappingContractResolver.IncludePrivate;
            }

            return MappingContractResolver.Instance;
        }

        internal static Exception SerializeError(JsonException ex) =>
            new DataSerializerException(
                "The object cannot be serialized. Please check the inner exception.",
                ex);

        internal static Exception DeserializeError(JsonException ex) =>
            new DataSerializerException(
                "The object cannot be deserialized. Please check the inner exception.",
                ex);
    }
}
