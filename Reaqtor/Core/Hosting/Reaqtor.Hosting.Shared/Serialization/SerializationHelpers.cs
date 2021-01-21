// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel.Serialization.Json;
using Nuqleon.Json.Interop.Newtonsoft;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Reflection;

using JsonExpression = Nuqleon.Json.Expressions.Expression;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// Helper class to serialize and deserialize expressions and data model-
    /// compliant objects.
    /// </summary>
    public class SerializationHelpers
    {
        /// <summary>
        /// Resource pool for JSON expression readers and writers.
        /// </summary>
        private static readonly JsonInteropResourcePool s_pool = new();

        /// <summary>
        /// The method definition used to serialize data.
        /// </summary>
        private readonly MethodInfo _serializeToMethod;

        /// <summary>
        /// The method definition used to deserialize data.
        /// </summary>
        private readonly MethodInfo _deserializeFromMethod;

        /// <summary>
        /// The data converters to use while deserializing.
        /// </summary>
        private readonly DataConverter[] _dataConverters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationHelpers"/> class
        /// to serialize and deserialize expressions and data model-compliant objects.
        /// </summary>
        public SerializationHelpers()
            : this(Array.Empty<DataConverter>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationHelpers"/> class
        /// to serialize and deserialize expressions and data model-compliant objects.
        /// </summary>
        /// <param name="dataConverters">The data converters to use while deserializing.</param>
        public SerializationHelpers(DataConverter[] dataConverters)
        {
            _dataConverters = dataConverters;
            _serializeToMethod = ((MethodInfo)ReflectionHelpers.InfoOf((JsonDataSerializer j) => j.SerializeTo(default(object), default(JsonWriter)))).GetGenericMethodDefinition();
            _deserializeFromMethod = ((MethodInfo)ReflectionHelpers.InfoOf((JsonDataSerializer j) => j.DeserializeFrom<object>(default(JsonReader)))).GetGenericMethodDefinition();
        }

        /// <summary>
        /// Serializes the specified object. If the object contains expression
        /// trees, Bonsai representation is used to serialize those.
        /// </summary>
        /// <typeparam name="T">Type of the object that gets serialized.</typeparam>
        /// <param name="value">Object to serialize.</param>
        /// <param name="stream">The stream to serialize to.</param>
        /// <remarks>The use of a generic type parameter allows for "typed nulls".</remarks>
        public void Serialize<T>(T value, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            CreateDataSerializer().Serialize<T>(value, stream);
        }

        /// <summary>
        /// Deserializes the specified serialization payload to an instance of
        /// the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="stream">Serialized representation of the object.</param>
        /// <returns>
        /// Instance of the specified type resulting from deserializing the
        /// payload.
        /// </returns>
        /// <remarks>
        /// It is possible to deserialize an object to a different CLR type than
        /// the one that was used during serialization, as long as it has the
        /// same data model projection.
        /// </remarks>
        /// <exception cref="Nuqleon.DataModel.Serialization.Json.DataSerializerException">
        /// if a JsonException was thrown during deserialization of <c>json</c></exception>
        public T Deserialize<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return CreateDataSerializer().Deserialize<T>(stream);
        }

        /// <summary>
        /// Creates an expression serializer.
        /// </summary>
        /// <returns>An expression serializer.</returns>
        protected virtual IExpressionSerializer CreateExpressionSerializer()
        {
            return new SerializationHelpersExpressionSerializer(this);
        }

        private JsonDataSerializer CreateDataSerializer(IExpressionSerializer expressionSerializer)
        {
            var dataSerializer = (JsonDataSerializer)DataSerializer.Create(expressionSerializer, _dataConverters);

            if (expressionSerializer is SerializationHelpersExpressionSerializer helpersExpressionSerializer)
            {
                helpersExpressionSerializer.DataSerializer = dataSerializer;
            }

            return dataSerializer;
        }

        private JsonExpression Serialize(JsonDataSerializer serializer, object value, Type type)
        {
            using var jsonWriter = new JsonExpressionWriter(s_pool);

            // PERF: Consider adding early-bound helpers and runtime compiled dispatchers.

            var method = _serializeToMethod.MakeGenericMethod(type);
            method.Invoke(serializer, new object[] { value, jsonWriter });

            return jsonWriter.Expression;
        }

        private object Deserialize(JsonDataSerializer serializer, JsonExpression value, Type type)
        {
            using var jsonReader = new JsonExpressionReader(value, s_pool);

            // PERF: Consider adding early-bound helpers and runtime compiled dispatchers.

            var method = _deserializeFromMethod.MakeGenericMethod(type);
            var res = method.Invoke(serializer, new object[] { jsonReader });

            return res;
        }

        private JsonDataSerializer CreateDataSerializer()
        {
            return CreateDataSerializer(CreateExpressionSerializer());
        }

        /// <summary>
        /// Simple expression serializer class using data serialization hooks.
        /// </summary>
        protected class SerializationHelpersExpressionSerializer : BonsaiExpressionSerializer
        {
            private readonly SerializationHelpers _parent;

            private ExpressionToExpressionSlimConverter _lifter;
            private ExpressionSlimToExpressionConverter _reducer;

            /// <summary>
            /// Instantiate the expression serializer.
            /// </summary>
            /// <param name="parent">The parent serialization helpers.</param>
            public SerializationHelpersExpressionSerializer(SerializationHelpers parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// The data serializer used to serialize constants.
            /// </summary>
            internal JsonDataSerializer DataSerializer { get; set; }

            private ExpressionToExpressionSlimConverter Lifter => _lifter ??= CreateLifter();

            private ExpressionSlimToExpressionConverter Reducer => _reducer ??= CreateReducer();

            /// <summary>
            /// Method to reduce a slim expression to an expression.
            /// </summary>
            /// <param name="expression">A slim expression.</param>
            /// <returns>The expression represented by the slim expression.</returns>
            public override Expression Reduce(ExpressionSlim expression)
            {
                return Reducer.Visit(expression);
            }

            /// <summary>
            /// Method to lift an expression to a slim expression.
            /// </summary>
            /// <param name="expression">An expression.</param>
            /// <returns>A slim representation of the expression.</returns>
            public override ExpressionSlim Lift(Expression expression)
            {
                return Lifter.Visit(expression);
            }

            /// <summary>
            /// Creates the expression to expression slim lifter for the serializer instance.
            /// </summary>
            /// <returns>An expression to expression slim lifter.</returns>
            /// <remarks>
            /// This function should be called at most once.
            /// </remarks>
            protected virtual ExpressionToExpressionSlimConverter CreateLifter()
            {
                return new ExpressionToExpressionSlimConverter();
            }

            /// <summary>
            /// Creates the expression slim to expression reducer for the serializer instance.
            /// </summary>
            /// <returns>An expression slim to expression reducer.</returns>
            /// <remarks>
            /// This function should be called at most once.
            /// </remarks>
            protected virtual ExpressionSlimToExpressionConverter CreateReducer()
            {
                return new ExpressionSlimToExpressionConverter();
            }

            /// <summary>
            /// Gets a delegate that can be used to deserialize constant values.
            /// </summary>
            /// <param name="type">The constant value type.</param>
            /// <returns>A delegate to deserialize constant values.</returns>
            protected sealed override Func<JsonExpression, object> GetConstantDeserializer(Type type)
            {
                EnsureSerializer();
                return s => _parent.Deserialize(DataSerializer, s, type);
            }

            /// <summary>
            /// Gets a delegate that can be used to serialize constant values.
            /// </summary>
            /// <param name="type">The constant value type.</param>
            /// <returns>A delegate to serialize constant values.</returns>
            protected sealed override Func<object, JsonExpression> GetConstantSerializer(Type type)
            {
                EnsureSerializer();
                return o => _parent.Serialize(DataSerializer, o, type);
            }

            private void EnsureSerializer()
            {
                DataSerializer ??= _parent.CreateDataSerializer(this);
            }
        }
    }
}
