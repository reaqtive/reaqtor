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
using System.Reflection;
using System.Runtime.CompilerServices;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.Serialization.Binary
{
    /// <summary>
    /// Binary Serializer for Data Types.
    /// </summary>
    public class DataTypeBinarySerializer
    {
        private static readonly Lazy<MethodInfo> s_hashSetPoolGetInstance = new(() => (MethodInfo)ReflectionHelpers.InfoOf(() => PooledHashSet<object>.GetInstance()));
        private static readonly Lazy<MethodInfo> s_pooledHashSetFree = new(() => (MethodInfo)ReflectionHelpers.InfoOf((PooledHashSet<object> p) => p.Free()));

        private readonly ConditionalWeakTable<Type, Delegate> _deserializers = new();
        private readonly ConditionalWeakTable<Type, Delegate> _serializers = new();

        internal DataTypeBinarySerializer() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeBinarySerializer" /> class with support for expression serialization.
        /// </summary>
        /// <param name="expressionSerializer">Serialization callback for expressions.</param>
        public DataTypeBinarySerializer(IExpressionSerializer expressionSerializer) => ExpressionSerializer = expressionSerializer;

        internal IExpressionSerializer ExpressionSerializer { get; }

        private ConditionalWeakTable<Type, Delegate>.CreateValueCallback _getSerializerCore;
        private ConditionalWeakTable<Type, Delegate>.CreateValueCallback _getDeserializerCore;

        private ConditionalWeakTable<Type, Delegate>.CreateValueCallback GetDeserializerCoreMethod
        {
            get
            {
                if (_getDeserializerCore == null)
                {
                    _getDeserializerCore = GetDeserializerCore;
                }

                return _getDeserializerCore;
            }
        }

        private ConditionalWeakTable<Type, Delegate>.CreateValueCallback GetSerializerCoreMethod
        {
            get
            {
                if (_getSerializerCore == null)
                {
                    _getSerializerCore = GetSerializerCore;
                }

                return _getSerializerCore;
            }
        }

        /// <summary>
        /// Deserialize a serialized stream containing a given type.
        /// </summary>
        /// <param name="stream">Stream containing the serialized data.</param>
        /// <returns>Deserialized object.</returns>
        public T Deserialize<T>(Stream stream) => (T)Deserialize(typeof(T), stream);

        /// <summary>
        /// Deserialize a serialized stream containing a given type
        /// </summary>
        /// <param name="type">.Net type the stream represents.</param>
        /// <param name="stream">Stream containing the serialized data.</param>
        /// <returns>Deserialized object.</returns>
        public object Deserialize(Type type, Stream stream)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var function = _deserializers.GetValue(type, GetDeserializerCoreMethod);
            if (function is Func<Stream, object> simpleFunction)
            {
                return simpleFunction(stream);
            }
            else
            {
                var recursiveFunction = (Func<DataTypeBinarySerializer, Stream, object>)function;
                return recursiveFunction(this, stream);
            }
        }

        /// <summary>
        /// Serialize an object to a stream.
        /// </summary>
        /// <param name="stream">Stream to contain the serialized object.</param>
        /// <param name="value">Object to be serialized.</param>
        public void Serialize<T>(Stream stream, T value) => Serialize(typeof(T), stream, value);

        /// <summary>
        /// Serialize an object to a stream.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="stream">Stream to contain the serialized object.</param>
        /// <param name="value">Object to be serialized.</param>
        public void Serialize(Type type, Stream stream, object value)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var action = _serializers.GetValue(type, GetSerializerCoreMethod);
            if (action is Action<Stream, object> simpleAction)
            {
                simpleAction(stream, value);
            }
            else
            {
                var recursiveAction = (Action<DataTypeBinarySerializer, bool, Stream, object>)action;
                recursiveAction(this, true, stream, value);
            }
        }

        internal void SerializeRecursive(Type type, Stream stream, object value)
        {
            _serializers.TryGetValue(type, out var action);
            Debug.Assert(action != null);
            var recursiveAction = (Action<DataTypeBinarySerializer, bool, Stream, object>)action;
            recursiveAction(this, false, stream, value);
        }

        private Delegate GetDeserializerCore(Type type)
        {
            if (!DataType.TryFromType(type, allowCycles: true, out var dataType))
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Type {0} not supported by data model.", type.FullName));
            }

            var converter = new DataTypeToDeserializer();
            var expression = converter.Visit(dataType);

            return Expression.Lambda(
                Expression.Convert(expression.Body, typeof(object)),
                expression.Parameters
            ).Compile();
        }

        private Delegate GetSerializerCore(Type type)
        {
            if (!DataType.TryFromType(type, allowCycles: true, out var dataType))
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Type {0} not supported by data model.", type.FullName));
            }

            var converter = new DataTypeToSerializer();
            var expression = converter.Visit(dataType);

            if (expression.Parameters.Count == 3)
            {
                var cycleDetector = new DataTypeToCycleDetector();
                var checkCycle = cycleDetector.Visit(dataType);

                var visitedParameter = Expression.Parameter(typeof(PooledHashSet<object>), "visited");
                var serializerParameter = expression.Parameters[0];
                var streamParameter = expression.Parameters[1];
                var innerValueParameter = expression.Parameters[2];
                var needsCheckParameter = Expression.Parameter(typeof(bool), "needsCheck");

                var valueParameter = Expression.Parameter(typeof(object), "value");

                var bodyExpressions = new List<Expression>(3)
                {
                    Expression.Assign(innerValueParameter, Expression.Convert(valueParameter, type))
                };

                if (checkCycle != null)
                {
                    bodyExpressions.Add(
                        Expression.IfThen(
                            needsCheckParameter,
                            Expression.Block(
                                new[] { visitedParameter },
                                Expression.Assign(visitedParameter, Expression.Call(s_hashSetPoolGetInstance.Value)),
                                Expression.TryFinally(
                                    Expression.Invoke(checkCycle, valueParameter, Expression.Convert(visitedParameter, typeof(HashSet<object>))),
                                    Expression.Call(visitedParameter, s_pooledHashSetFree.Value)
                                )
                            )
                        )
                    );
                }
                bodyExpressions.Add(expression.Body);

                return Expression.Lambda<Action<DataTypeBinarySerializer, bool, Stream, object>>(
                    Expression.Block(
                        new[] { innerValueParameter },
                        bodyExpressions
                    ),
                    serializerParameter,
                    needsCheckParameter,
                    streamParameter,
                    valueParameter
                ).Compile();
            }

            return Expression.Parameter(typeof(object), "input").Let(input =>
                Expression.Lambda<Action<Stream, object>>(
                    Expression.Block(
                        new[] { expression.Parameters[1] },
                        Expression.Assign(expression.Parameters[1], Expression.Convert(input, type)),
                        expression.Body
                    ),
                    expression.Parameters[0],
                    input
                )
            ).Compile();
        }
    }
}
