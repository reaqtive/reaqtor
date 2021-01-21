// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Memory;
using System.Reflection;

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Implements lift, reduce, serialize, and deserialize methods for expressions
    /// using Bonsai JSON as the serialization format.
    /// </summary>
    public class BonsaiExpressionSerializer : IExpressionSerializer
    {
        #region Legacy

        private readonly Func<Type, Func<object, Json.Expression>> _liftFactory;
        private readonly Func<Type, Func<Json.Expression, object>> _reduceFactory;

        #endregion

        private readonly Version _version;
        private readonly IMemoizedDelegate<Func<Type, Func<object, Json.Expression>>> _serializeConstant;
        private readonly IMemoizedDelegate<Func<Type, Func<Json.Expression, object>>> _deserializeConstant;

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        public BonsaiExpressionSerializer()
            : this(BonsaiVersion.Default)
        {
        }

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        /// <param name="version">Version of Bonsai to use.</param>
        public BonsaiExpressionSerializer(Version version)
            : this(version, liftMemoizer: null, reduceMemoizer: null)
        {
        }

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        /// <param name="version">Version of Bonsai to use.</param>
        /// <param name="liftMemoizer">The function memoizer to use for converting types to object serializers.</param>
        /// <param name="reduceMemoizer">The function memoizer to use for converting types to object deserializers.</param>
        public BonsaiExpressionSerializer(Version version, IMemoizer liftMemoizer, IMemoizer reduceMemoizer)
        {
            _version = version;

            _serializeConstant = liftMemoizer?.Memoize<Func<Type, Func<object, Json.Expression>>>(GetConstantSerializer);
            _deserializeConstant = reduceMemoizer?.Memoize<Func<Type, Func<Json.Expression, object>>>(GetConstantDeserializer);
        }

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        /// <param name="liftFactory">Factory to produce object serializer.</param>
        /// <param name="reduceFactory">Factory to produce object deserializer.</param>
        public BonsaiExpressionSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory)
            : this(liftFactory, reduceFactory, BonsaiVersion.Default)
        {
        }

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        /// <param name="liftFactory">Factory to produce object serializer.</param>
        /// <param name="reduceFactory">Factory to produce object deserializer.</param>
        /// <param name="version">Version of Bonsai to use.</param>
        public BonsaiExpressionSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory, Version version)
        {
            // NB: We don't provide support for memoization here given that this is a legacy constructor;
            //     the caller can always pass in memoized functions themselves.

            _liftFactory = liftFactory;
            _reduceFactory = reduceFactory;
            _version = version;
        }

        /// <summary>
        /// Gets the serializer factory delegate, which can be subject to memoization.
        /// </summary>
        private Func<Type, Func<object, Json.Expression>> GetConstantSerializerDelegate => _serializeConstant?.Delegate ?? GetConstantSerializer;

        /// <summary>
        /// Gets the deserializer factory delegate, which can be subject to memoization.
        /// </summary>
        private Func<Type, Func<Json.Expression, object>> GetConstantDeserializerDelegate => _deserializeConstant?.Delegate ?? GetConstantDeserializer;

        /// <summary>
        /// Method to lift an expression into a slim, serializable form.
        /// </summary>
        /// <param name="expression">The expression to serialize.</param>
        /// <returns>A slim representation of the expression.</returns>
        public virtual ExpressionSlim Lift(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            return expression.ToExpressionSlim(); // NB: Derived class can override to specify a factory.
        }

        /// <summary>
        /// Method to reduce a slim expression to an expression.
        /// </summary>
        /// <param name="expression">The slim expression.</param>
        /// <returns>The expression represented by the slim expression.</returns>
        public virtual Expression Reduce(ExpressionSlim expression)
        {
            if (expression == null)
            {
                return null;
            }

            return expression.ToExpression(); // NB: Derived class can override to specify a factory.
        }

        /// <summary>
        /// Method to serialize a slim representation of an expression.
        /// </summary>
        /// <param name="expression">The slim expression to serialize.</param>
        /// <returns>A string representing the expression.</returns>
        public virtual string Serialize(ExpressionSlim expression)
        {
            // PERF: Consider passing in a pooled string builder instance.

            return JsonSerialize(expression).ToString();
        }

        /// <summary>
        /// Method to deserialize a serialized expression into a slim representation.
        /// </summary>
        /// <param name="expression">The serialized expression.</param>
        /// <returns>The deserialized slim expression.</returns>
        public virtual ExpressionSlim Deserialize(string expression)
        {
            var json = Json.Expression.Parse(expression, ensureTopLevelObjectOrArray: false);

            return JsonDeserialize(json);
        }

        /// <summary>
        /// Gets a delegate that can be used to serialize constant values.
        /// </summary>
        /// <param name="type">The constant value type.</param>
        /// <returns>A delegate to serialize constant values.</returns>
        protected virtual Func<object, Json.Expression> GetConstantSerializer(Type type)
        {
            if (_liftFactory != null)
            {
                return _liftFactory(type);
            }

            throw new NotImplementedException("Implemented factories in derived classes.");
        }

        /// <summary>
        /// Gets a delegate that can be used to deserialize constant values.
        /// </summary>
        /// <param name="type">The constant value type.</param>
        /// <returns>A delegate to deserialize constant values.</returns>
        protected virtual Func<Json.Expression, object> GetConstantDeserializer(Type type)
        {
            if (_reduceFactory != null)
            {
                return _reduceFactory(type);
            }

            throw new NotImplementedException("Implemented factories in derived classes.");
        }

        /// <summary>
        /// Serializes an expression to Bonsai.
        /// </summary>
        /// <param name="expression">Expression to serialize.</param>
        /// <returns>Bonsai representation of the given expression.</returns>
        private Json.Expression JsonSerialize(ExpressionSlim expression)
        {
            var serializationState = new SerializationState(_version);
            var body = new SerializerImpl(serializationState, GetConstantSerializerDelegate).Visit(expression);
            var context = serializationState.ToJson();

            // PERF: We could consider creating a specialized IDictionary for the
            //       top-level JSON structure to avoid Dictionary allocation.

            return Json.Expression.Object(new Dictionary<string, Json.Expression>(2)
            {
                { "Context", context },
                { "Expression", body },
            });
        }

        /// <summary>
        /// Deserializes a Bonsai expression.
        /// </summary>
        /// <param name="expression">Bonsai expression to deserialize.</param>
        /// <returns>Slim expression represented by the given Bonsai.</returns>
        private ExpressionSlim JsonDeserialize(Json.Expression expression)
        {
            if (expression.NodeType == Json.ExpressionType.Null)
            {
                return null;
            }


            if (expression is not Json.ObjectExpression obj)
                throw new InvalidOperationException("Expected JSON object expression.");

            if (!obj.Members.TryGetValue("Context", out Json.Expression context))
                throw new InvalidOperationException("Context not found.");

            if (!obj.Members.TryGetValue("Expression", out Json.Expression expr))
                throw new InvalidOperationException("Expression not found.");

            if (expr.NodeType == Json.ExpressionType.Null)
            {
                return null;
            }

            var deserializationState = new DeserializationState(context, _version);

            var visitor = new DeserializerImpl(deserializationState, GetConstantDeserializerDelegate);

            return visitor.Visit(expr);
        }

        private sealed class SerializerImpl : ExpressionSlimToBonsaiConverter
        {
            private readonly Func<Type, Func<object, Json.Expression>> _liftFactory;

            public SerializerImpl(SerializationState state, Func<Type, Func<object, Json.Expression>> liftFactory)
                : base(state)
            {
                _liftFactory = liftFactory;
            }

            protected override Json.Expression VisitConstantValue(ObjectSlim value)
            {
                if (!value.CanLift)
                {
                    return (Json.Expression)value.Value;
                }

                return value.Lift(_liftFactory);
            }
        }

        private sealed class DeserializerImpl : BonsaiToExpressionSlimConverter
        {
            private readonly Func<Type, Func<Json.Expression, object>> _reduceFactory;

            public DeserializerImpl(DeserializationState state, Func<Type, Func<Json.Expression, object>> reduceFactory)
                : base(state)
            {
                _reduceFactory = reduceFactory;
            }

            protected override ObjectSlim VisitConstantValue(Json.Expression value, TypeSlim type) => ObjectSlim.Create(value, type, _reduceFactory);
        }
    }
}
