// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.Remoting.Protocol;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    public class StorageConnectionQueryable<T> : IQueryable<T>
        where T : new()
    {
        private readonly string _name;
        private readonly IReactiveStorageConnection _connection;

        public StorageConnectionQueryable(string name, IReactiveStorageConnection connection)
        {
            _name = name;
            _connection = connection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var list = new List<T>();
            foreach (var entity in _connection.GetEntities(_name))
            {
                list.Add(Convert<T>(entity));
            }
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Type ElementType => typeof(T);

        public Expression Expression => Expression.Parameter(GetType(), _name);

        public IQueryProvider Provider => new StorageConnectionQueryProvider(_name, _connection);

        private static TConvert Convert<TConvert>(StorageEntity entity)
        {
            var newEntity = (TConvert)Activator.CreateInstance(typeof(TConvert));
            foreach (var property in entity.Properties)
            {
                var tProperty = typeof(TConvert).GetProperty(property.Key);
                if (tProperty == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find property with name '{0}' on type '{1}'.", property.Key, typeof(T)));
                SetProperty(tProperty, newEntity, property.Value);
            }
            return newEntity;
        }

        private static void SetProperty(PropertyInfo property, object obj, StorageEntityProperty data)
        {
            switch ((EdmType)data.Type)
            {
                case EdmType.Boolean:
                    Assert(property.PropertyType, typeof(bool));
                    property.SetValue(obj, bool.Parse(data.Data));
                    break;
                case EdmType.DateTime:
                    Assert(property.PropertyType, typeof(DateTimeOffset));
                    property.SetValue(obj, new DateTimeOffset(new DateTime(long.Parse(data.Data), DateTimeKind.Utc)));
                    break;
                case EdmType.Double:
                    Assert(property.PropertyType, typeof(double));
                    property.SetValue(obj, double.Parse(data.Data));
                    break;
                case EdmType.Guid:
                    Assert(property.PropertyType, typeof(Guid));
                    property.SetValue(obj, Guid.Parse(data.Data));
                    break;
                case EdmType.Int32:
                    Assert(property.PropertyType, typeof(int));
                    property.SetValue(obj, int.Parse(data.Data));
                    break;
                case EdmType.Int64:
                    Assert(property.PropertyType, typeof(long));
                    property.SetValue(obj, long.Parse(data.Data));
                    break;
                case EdmType.String:
                    Assert(property.PropertyType, typeof(string));
                    property.SetValue(obj, data.Data);
                    break;
                default:
                    throw new NotImplementedException("Could not parse data.");
            }
        }

        private static void Assert(Type actual, Type expected)
        {
            if (actual != expected)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Property types '{0}' and '{1}' are not equivalent.", actual, expected));
        }

        private sealed class StorageConnectionQueryProvider : IQueryProvider
        {
            private readonly string _name;
            private readonly IReactiveStorageConnection _connection;

            public StorageConnectionQueryProvider(string name, IReactiveStorageConnection connection)
            {
                _name = name;
                _connection = connection;
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                var finder = new PropertyComparisonValueFinder<string>("RowKey");
                finder.Visit(expression);
                if (finder.Value != null)
                {
                    return _connection.TryGetEntity(_name, finder.Value, out var entity)
                        ? new List<TElement> { Convert<TElement>(entity) }.AsQueryable()
                        : new List<TElement>().AsQueryable();
                }
                else
                {
                    var entities = _connection.GetEntities(_name).Select(e => Convert<TElement>(e)).AsQueryable();
                    var freeVariables = FreeVariableScanner.Scan(expression).ToArray();
                    if (freeVariables.Length == 1)
                    {
                        Debug.Assert(freeVariables[0].Name == _name);
                        var substitutor = new ParameterSubstitutor(freeVariables[0], Expression.Constant(entities));
                        return substitutor.Visit(expression).Evaluate<IQueryable<TElement>>();
                    }
                }

                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' not supported.", expression.ToTraceString()));
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return CreateQuery<object>(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return (TResult)expression.Evaluate();
            }

            public object Execute(Expression expression)
            {
                return Execute<object>(expression);
            }

            private sealed class ParameterSubstitutor : ScopedExpressionVisitor<ParameterExpression>
            {
                private readonly ParameterExpression _parameter;
                private readonly Expression _replacement;

                public ParameterSubstitutor(ParameterExpression parameter, Expression replacement)
                {
                    _parameter = parameter;
                    _replacement = replacement;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (!TryLookup(node, out _) && node == _parameter)
                    {
                        return _replacement;
                    }

                    return base.VisitParameter(node);
                }

                protected override ParameterExpression GetState(ParameterExpression parameter)
                {
                    return parameter;
                }
            }
        }
    }
}
