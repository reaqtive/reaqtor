// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Reflection;

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Bonsai serializer for expressions.
    /// </summary>
    /// <remarks>
    /// Instantiates the expression Bonsai serializer.
    /// </remarks>
    /// <param name="liftFactory">Factory to produce object serializer.</param>
    /// <param name="reduceFactory">Factory to produce object deserializer.</param>
    /// <param name="version">Version of Bonsai to use.</param>
    /// <param name="mergeContext">true if recursive calls to serialize should use a root-level context, false otherwise.</param>
    public class ExpressionSlimBonsaiSerializer(Func<Type, Func<object, object>> liftFactory, Func<Type, Func<object, object>> reduceFactory, Version version, bool mergeContext)
    {
        private readonly Func<Type, Func<object, object>> _liftFactory = liftFactory;
        private readonly Func<Type, Func<object, object>> _reduceFactory = reduceFactory;

        private readonly Version _version = version;

        private readonly bool _mergeContext = mergeContext;
        private SerializationState _serializationState;
        private DeserializationState _deserializationState;

        /// <summary>
        /// Instantiates the expression Bonsai serializer.
        /// </summary>
        /// <param name="liftFactory">Factory to produce object serializer.</param>
        /// <param name="reduceFactory">Factory to produce object deserializer.</param>
        /// <param name="version">Version of Bonsai to use.</param>
        public ExpressionSlimBonsaiSerializer(Func<Type, Func<object, object>> liftFactory, Func<Type, Func<object, object>> reduceFactory, Version version)
            : this(liftFactory, reduceFactory, version, mergeContext: true)
        {
        }

        /// <summary>
        /// Serializes an expression to Bonsai.
        /// </summary>
        /// <param name="expression">Expression to serialize.</param>
        /// <returns>Bonsai representation of the given expression.</returns>
        public Json.Expression Serialize(ExpressionSlim expression)
        {
            if (_mergeContext && _serializationState != null)
            {
                return new SerializerImpl(_serializationState, _liftFactory).Visit(expression);
            }
            else
            {
                var state = new SerializationState(_version);

                _serializationState = state;

                var body = new SerializerImpl(state, _liftFactory).Visit(expression);
                var context = state.ToJson();

                _serializationState = null;

                // PERF: We could consider creating a specialized IDictionary for the
                //       top-level JSON structure to avoid Dictionary allocation.

                return Json.Expression.Object(new Dictionary<string, Json.Expression>(2)
                {
                    { "Context", context },
                    { "Expression", body },
                });
            }
        }

        /// <summary>
        /// Deserializes a Bonsai expression.
        /// </summary>
        /// <param name="expression">Bonsai expression to deserialize.</param>
        /// <returns>Slim expression represented by the given Bonsai.</returns>
        public ExpressionSlim Deserialize(Json.Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            Json.Expression expr;

            if (_mergeContext && _deserializationState != null)
            {
                expr = expression;
            }
            else
            {
                if (expression.NodeType == Json.ExpressionType.Null)
                {
                    return null;
                }

                if (expression is not Json.ObjectExpression obj)
                    throw new BonsaiParseException("Expected a JSON object containing the Bonsai representation of an expression.", expression);

                if (!obj.Members.TryGetValue("Context", out Json.Expression context))
                    throw new BonsaiParseException("Expected a JSON object property 'node.Context' containing the context object.", expression);

                if (!obj.Members.TryGetValue("Expression", out expr))
                    throw new BonsaiParseException("Expected a JSON object property 'node.Expression' containing the expression tree.", expression);

                _deserializationState = new DeserializationState(context, _version);
            }

            if (expr.NodeType == Json.ExpressionType.Null)
            {
                return null;
            }

            var visitor = new DeserializerImpl(_deserializationState, _reduceFactory);

            return visitor.Visit(expr);
        }

        private sealed class SerializerImpl(SerializationState state, Func<Type, Func<object, object>> liftFactory) : ExpressionSlimToBonsaiConverter(state)
        {
            private readonly Func<Type, Func<object, object>> _liftFactory = liftFactory;

            protected override Json.Expression VisitConstantValue(ObjectSlim value)
            {
                var liftedValue = value.CanLift ? value.Lift(LiftAndStringify) : value.Value.ToString();
                return Json.Expression.Parse(liftedValue, ensureTopLevelObjectOrArray: false);
            }

            private Func<object, string> LiftAndStringify(Type type)
            {
                var lifter = _liftFactory(type);
                return o => lifter(o).ToString();
            }
        }

        private sealed class DeserializerImpl(DeserializationState state, Func<Type, Func<object, object>> reduceFactory) : BonsaiToExpressionSlimConverter(state)
        {
            private readonly Func<Type, Func<object, object>> _reduceFactory = reduceFactory;

            protected override ObjectSlim VisitConstantValue(Json.Expression value, TypeSlim type)
            {
                return ObjectSlim.Create(value ?? Json.Expression.Null(), type, ReduceJsonFactory);
            }

            private Func<Json.Expression, object> ReduceJsonFactory(Type type)
            {
                var reducer = _reduceFactory(type);
                return lifted => reducer(lifted);
            }
        }
    }
}
