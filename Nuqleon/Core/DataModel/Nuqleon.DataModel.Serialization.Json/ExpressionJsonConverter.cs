// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Nuqleon.DataModel.Serialization.Json
{
    internal sealed class ExpressionJsonConverter : JsonConverter
    {
        #region Fields

        /// <summary>
        /// Pool of StringBuilder instances used to get the string representation of a JToken
        /// in order to pass it to the expression deserializer.
        /// </summary>
        /// <remarks>
        /// We assume possible reentrancy to the data deserializer and the expression deserializer
        /// for nested expression trees, so we use twice the processor count for the number of
        /// pooled instances in order to support such cases efficiently, even if all cores are
        /// performing deserialization in parallel.
        /// 
        /// For the initial capacity of the string builders, we pick 1K to support small Bonsai
        /// trees. We allow the builders to grow to 16K, which is about twice the size of the
        /// most complex Bonsai trees seen to date (December 2016).
        /// </remarks>
        private static readonly StringBuilderPool s_pool = StringBuilderPool.Create(size: Environment.ProcessorCount * 2, capacity: 1024, maxCapacity: 16 * 1024);

        /// <summary>
        /// The expression serialization pipeline.
        /// </summary>
        private readonly IExpressionSerializer _expressionSerializer;

        /// <summary>
        /// Serialization hook for expressions.
        /// </summary>
        private readonly Func<Expression, string> _serializeExpression;

        /// <summary>
        /// Deserialization hook for expressions.
        /// </summary>
        private readonly Func<string, Expression> _deserializeExpression;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a converter for expressions.
        /// </summary>
        /// <param name="expressionSerializer">Serialization callback for expressions.</param>
        public ExpressionJsonConverter(IExpressionSerializer expressionSerializer)
        {
            Debug.Assert(expressionSerializer != null);

            _expressionSerializer = expressionSerializer;
        }

        /// <summary>
        /// Instantiates a converter for expressions.
        /// </summary>
        /// <param name="serializeExpression">Serialization callback for expressions.</param>
        /// <param name="deserializeExpression">Deserialization callback for expressions.</param>
        public ExpressionJsonConverter(Func<Expression, string> serializeExpression, Func<string, Expression> deserializeExpression)
        {
            Debug.Assert(serializeExpression != null);
            Debug.Assert(deserializeExpression != null);

            _serializeExpression = serializeExpression;
            _deserializeExpression = deserializeExpression;
        }

        #endregion

        #region Expression serialization end-to-end

        /// <summary>
        /// Serializes an expression into JSON.
        /// </summary>
        /// <param name="serializer">The JSON serializer to use.</param>
        /// <param name="expression">The expression to serialize.</param>
        /// <returns>The serialized expression.</returns>
        private JToken SerializeExpression(JsonSerializer serializer, Expression expression)
        {
            if (_expressionSerializer != null)
            {
                var slim = _expressionSerializer.Lift(expression);
                return SerializeExpressionSlim(serializer, slim);
            }
            else
            {
                return Parse(serializer, _serializeExpression(expression));
            }
        }

        /// <summary>
        /// Serializes a slim expression into JSON.
        /// </summary>
        /// <param name="serializer">The JSON serializer to use.</param>
        /// <param name="expression">The slim expression to serialize.</param>
        /// <returns>The JSON serialized expression.</returns>
        private JToken SerializeExpressionSlim(JsonSerializer serializer, ExpressionSlim expression) => Parse(serializer, _expressionSerializer.Serialize(expression));

        /// <summary>
        /// Deserializes JSON into an expression.
        /// </summary>
        /// <param name="json">The JSON to deserialize.</param>
        /// <returns>The deserialized expression.</returns>
        private Expression DeserializeExpression(JToken json)
        {
            if (json == null)
            {
                return null;
            }
            else if (json.Type == JTokenType.Null)
            {
                return null;
            }

            var jsonText = GetJsonString(json);

            if (_expressionSerializer != null)
            {
                var slim = DeserializeExpressionSlim(jsonText);
                return _expressionSerializer.Reduce(slim);
            }
            else
            {
                return _deserializeExpression(jsonText);
            }
        }

        /// <summary>
        /// Deserializes JSON into a slim representation of an expression.
        /// </summary>
        /// <param name="json">The JSON to deserialize.</param>
        /// <returns>The deserialized slim expression.</returns>
        private ExpressionSlim DeserializeExpressionSlim(string json) => _expressionSerializer.Deserialize(json);

        private static JToken Parse(JsonSerializer serializer, string json)
        {
            var stringReader = new StringReader(json);

            try
            {
                using var jsonReader = new JsonTextReader(stringReader);

                stringReader = null;

                jsonReader.DateParseHandling = DateParseHandling.None;

                return serializer.Deserialize<JToken>(jsonReader);
            }
            finally
            {
                stringReader?.Dispose();
            }
        }

        #endregion

        #region JsonConverter overrides

        public override bool CanConvert(Type objectType) => typeof(Expression).IsAssignableFrom(objectType) || typeof(ExpressionSlim).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Debug.Assert(reader != null, nameof(reader));
            Debug.Assert(objectType != null, nameof(objectType));
            Debug.Assert(serializer != null, nameof(serializer));

            var originalDateParseHandling = reader.DateParseHandling;
            reader.DateParseHandling = DateParseHandling.None;
            var json = serializer.Deserialize<JToken>(reader);
            reader.DateParseHandling = originalDateParseHandling;

            if (typeof(Expression).IsAssignableFrom(objectType))
            {
                return DeserializeExpression(json);
            }
            else
            {
                var jsonText = json == null ? "null" : GetJsonString(json);
                return DeserializeExpressionSlim(jsonText);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Debug.Assert(writer != null, nameof(writer));
            Debug.Assert(serializer != null, nameof(serializer));

            var json = value is Expression expression
                ? SerializeExpression(serializer, expression)
                : SerializeExpressionSlim(serializer, (ExpressionSlim)value);

            serializer.Serialize(writer, json);
        }

        private static string GetJsonString(JToken token)
        {
            using var builder = s_pool.New();

            var sb = builder.StringBuilder;

            using var sw = new StringWriter(sb, CultureInfo.InvariantCulture);

            using var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None };

            token.WriteTo(jtw);

            return sw.ToString();
        }

        #endregion
    }
}
