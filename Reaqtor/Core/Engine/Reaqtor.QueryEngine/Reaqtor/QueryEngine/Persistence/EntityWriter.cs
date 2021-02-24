// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtor.Expressions;

namespace Reaqtor.QueryEngine
{
    internal sealed class EntityWriter : WriterBase
    {
        public EntityWriter(Stream stream, ISerializationPolicy policy)
            : base(stream, policy)
        {
        }

        public void Save(ReactiveEntity entity)
        {
            SerializeExpression(entity.Expression);
            _serializer.Serialize(entity.Uri, _stream);
            _serializer.Serialize(entity.State, _stream);
            entity.Serialize(_serializer, _stream);
        }

        private void SerializeExpression(Expression expression)
        {
            if (_serializer.Version < Versioning.v3)
            {
                SerializeExpressionV1(expression);
            }
            else
            {
                Debug.Assert(_serializer.Version >= Versioning.v3);
                SerializeExpressionV3(expression);
            }
        }

        private void SerializeExpressionV1(Expression expression)
        {
            _serializer.Serialize(expression, _stream);
        }

        private void SerializeExpressionV3(Expression expression)
        {
            if (expression.IsTemplatized(out var parameter, out var argument))
            {
                _serializer.Serialize(true, _stream);
                _serializer.Serialize(parameter.Name, _stream);
                if (argument != null)
                {
                    SerializeTemplateArgument(argument);
                }
            }
            else
            {
                _serializer.Serialize(false, _stream);
                _serializer.Serialize(expression, _stream);
            }
        }

        private void SerializeTemplateArgument(Expression argument)
        {
            var args = ExpressionTupletizer.Unpack(argument);
            foreach (Expression arg in args)
            {
                _serializer.Serialize((int)arg.NodeType, _stream);
                switch (arg.NodeType)
                {
                    case ExpressionType.Constant:
                        _serializer.Serialize(((ConstantExpression)arg).Value, arg.Type, _stream);
                        break;
                    case ExpressionType.Parameter:
                        _serializer.Serialize(((ParameterExpression)arg).Name, _stream);
                        break;
                    default:
                        throw new InvalidOperationException("Only constant and parameter expressions are supported.");
                }
            }
        }
    }
}
