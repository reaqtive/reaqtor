// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    internal sealed class EntityReader : ReaderBase
    {
        private readonly IQueryEngineRegistry _registry;

        public EntityReader(Stream stream, IQueryEngineRegistry registry, ISerializationPolicy policy)
            : base(stream, policy)
        {
            _registry = registry;
        }

        public ReactiveEntity Load(ReactiveEntityKind kind)
        {
            var expression = DeserializeExpression();
            var uri = _serializer.Deserialize<Uri>(_stream);
            var state = _serializer.Deserialize<object>(_stream);
            var entity = CreateEntity(kind, uri, expression, state);
            entity.Deserialize(_serializer, _stream);
            return entity;
        }

        private Expression DeserializeExpression()
        {
            if (_serializer.Version < Versioning.v3)
            {
                return DeserializeExpressionV1();
            }
            else
            {
                Debug.Assert(_serializer.Version >= Versioning.v3);
                return DeserializeExpressionV3();
            }
        }

        private Expression DeserializeExpressionV1()
        {
            return _serializer.Deserialize<Expression>(_stream);
        }

        private Expression DeserializeExpressionV3()
        {
            var isTemplatized = _serializer.Deserialize<bool>(_stream);
            if (isTemplatized)
            {
                var templateKey = _serializer.Deserialize<string>(_stream);

                if (!_registry.Templates.TryGetValue(templateKey, out DefinitionEntity template))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template '{0}' missing, cannot infer types for entity.", templateKey));
                }

                var templateType = template.Expression.Type;
                var parameterizedFuncType = templateType.FindGenericType(typeof(Func<,>));
                if (parameterizedFuncType != null)
                {
                    var argument = DeserializeTemplateArgument(parameterizedFuncType.GetGenericArguments()[0]);
                    return Expression.Invoke(Expression.Parameter(templateType, templateKey), argument);
                }
                else if (templateType.FindGenericType(typeof(Func<>)) != null)
                {
                    return Expression.Invoke(Expression.Parameter(templateType, templateKey));
                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template '{0}' is not a lambda expression with 0 or 1 parameters.", templateKey));
                }
            }
            else
            {
                return _serializer.Deserialize<Expression>(_stream);
            }
        }

        private Expression DeserializeTemplateArgument(Type templateArgumentType)
        {
            if (!TupleTypeEnumerator.TryEnumerate(templateArgumentType, out Type[] tupleTypes))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template lambda argument '{0}' is not a tuple type.", templateArgumentType));
            }

            var args = new Expression[tupleTypes.Length];
            for (var i = 0; i < tupleTypes.Length; ++i)
            {
                var type = tupleTypes[i];
                var nodeType = (ExpressionType)_serializer.Deserialize<int>(_stream);
                args[i] = nodeType switch
                {
                    ExpressionType.Constant => Expression.Constant(_serializer.Deserialize(type, _stream), type),
                    ExpressionType.Parameter => Expression.Parameter(type, _serializer.Deserialize<string>(_stream)),
                    _ => throw new InvalidOperationException("Only constant and parameter expressions are supported."),
                };
            }

            return ExpressionTupletizer.Pack(args);
        }

        private static ReactiveEntity CreateEntity(ReactiveEntityKind kind, Uri uri, Expression expression, object state)
        {
            ReactiveEntity entity = kind switch
            {
                ReactiveEntityKind.Observable => new ObservableDefinitionEntity(uri, expression, state),
                ReactiveEntityKind.Observer => new ObserverDefinitionEntity(uri, expression, state),
                ReactiveEntityKind.Stream => new SubjectEntity(uri, expression, state),
                ReactiveEntityKind.StreamFactory => new StreamFactoryDefinitionEntity(uri, expression, state),
                ReactiveEntityKind.SubscriptionFactory => new SubscriptionFactoryDefinitionEntity(uri, expression, state),
                ReactiveEntityKind.Subscription => new SubscriptionEntity(uri, expression, state),
                ReactiveEntityKind.ReliableSubscription => new ReliableSubscriptionEntity(uri, expression, state),
                ReactiveEntityKind.Other => new OtherDefinitionEntity(uri, expression, state),
                _ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unexpected reactive entity type '{0}'.", kind), nameof(kind)),
            };
            entity.OnPersisted();
            return entity;
        }

        private sealed class TupleTypeEnumerator
        {
            private static readonly Type[] _tupleTypes = new[]
            {
                typeof(Tuple<>),
                typeof(Tuple<,>),
                typeof(Tuple<,,>),
                typeof(Tuple<,,,>),
                typeof(Tuple<,,,,>),
                typeof(Tuple<,,,,,>),
                typeof(Tuple<,,,,,,>),
                typeof(Tuple<,,,,,,,>),
            };

            public static bool TryEnumerate(Type type, out Type[] innerTypes)
            {
                innerTypes = TryEnumerate(type, out IEnumerable<Type> types) ? types.AsArray() : null;
                return innerTypes != null;
            }

            public static bool TryEnumerate(Type type, out IEnumerable<Type> innerTypes)
            {
                if (type.IsGenericType && !type.IsGenericTypeDefinition)
                {
                    var genericDefinition = type.GetGenericTypeDefinition();
                    if (_tupleTypes.Contains(genericDefinition))
                    {
                        if (type.GenericTypeArguments.Length < 8)
                        {
                            innerTypes = type.GenericTypeArguments;
                        }
                        else
                        {
                            if (!TryEnumerate(type.GenericTypeArguments[7], out IEnumerable<Type> restTypes))
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Final type parameter of tuple '{0}' is not a nested tuple.", type.GenericTypeArguments[7]));
                            }
                            innerTypes = type.GenericTypeArguments.Take(7).Concat(restTypes);
                        }

                        return true;
                    }
                }

                innerTypes = default;
                return false;
            }
        }
    }
}
