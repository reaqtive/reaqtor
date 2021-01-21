// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal sealed class DataTypeToCycleDetector : DataTypeVisitor<Expression, Tuple<DataProperty, Expression>>
    {
        private readonly Dictionary<Type, CycleHelper> _visited = new();

        protected override Expression VisitStructural(StructuralDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_visited.TryGetValue(type.UnderlyingType, out var cycleHelper))
            {
                _visited[type.UnderlyingType] = new CycleHelper { Expression = cycleHelper.Expression, Used = true };
                return cycleHelper.Expression;
            }

            var recursionParameter = Expression.Parameter(typeof(CycleDetector), "f");
            _visited.Add(type.UnderlyingType, new CycleHelper { Expression = recursionParameter });

            try
            {
                return base.VisitStructural(type);
            }
            finally
            {
                _visited.Remove(type.UnderlyingType);
            }
        }

        protected override Expression MakeArray(ArrayDataType type, Expression elementType)
        {
            if (elementType != null)
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                var hashSetParameter = Expression.Parameter(typeof(HashSet<object>), "hashSet");
                var valueParameter = Expression.Parameter(typeof(object), "value");
                var convertedParameter = Expression.Parameter(type.UnderlyingType, "converted");
                var lengthParameter = Expression.Parameter(typeof(int), "length");
                var loopParameter = Expression.Parameter(typeof(int), "i");

                var accessExpression = type.UnderlyingType.IsArray
                    ? (Expression)Expression.ArrayIndex(convertedParameter, loopParameter)
                    : Expression.Call(convertedParameter, type.UnderlyingType.GetMethod("get_Item", new[] { typeof(int) }), loopParameter);

                Debug.Assert(elementType.Type == typeof(CycleDetector));

                var body = Expression.Block(
                    new[] { convertedParameter },
                    Expression.Assign(convertedParameter, Expression.Convert(valueParameter, type.UnderlyingType)),
                    Expression.IfThen(
                        Expression.NotEqual(convertedParameter, ReflectionConstants.NullObject),
                        Expression.Block(
                            new[] { lengthParameter, loopParameter },
                            Expression.Assign(lengthParameter, Expression.Property(convertedParameter, ReflectionConstants.ListCount)),
                            // var i = 0; while (true) { if (i < length) { inner(value[i], hashSet); i++; } else { break; } }
                            ExpressionHelpers.For(
                                Expression.Assign(loopParameter, Expression.Constant(0, typeof(int))),
                                Expression.LessThan(loopParameter, lengthParameter),
                                Expression.PreIncrementAssign(loopParameter),
                                Expression.Invoke(elementType, elementType, accessExpression, hashSetParameter)
                            )
                        )
                    )
                );

                return Expression.Lambda<Action<object, HashSet<object>>>(body, valueParameter, hashSetParameter);
            }

            return null;
        }

        protected override Expression MakeFunction(FunctionDataType type, ReadOnlyCollection<Expression> parameterTypes, Expression returnType) => null;

        protected override Tuple<DataProperty, Expression> MakeProperty(DataProperty property, Expression propertyType) => Tuple.Create(property, propertyType);

        protected override Expression MakeQuotation(QuotationDataType type, Expression functionType) => null;

        protected override Expression MakeStructural(StructuralDataType type, ReadOnlyCollection<Tuple<DataProperty, Expression>> properties)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var hashSetParameter = Expression.Parameter(typeof(HashSet<object>), "hashSet");
            var valueParameter = Expression.Parameter(typeof(object), "value");
            var convertedParameter = Expression.Parameter(type.UnderlyingType, "converted");

            var propertyChecks = new List<Expression>
            {
                Expression.Assign(convertedParameter, Expression.Convert(valueParameter, type.UnderlyingType))
            };

            foreach (var propertyData in properties)
            {
                var property = propertyData.Item1;
                var propertyExpression = propertyData.Item2;
                if (propertyExpression != null)
                {
                    var accessExpression = Expression.Convert(Expression.MakeMemberAccess(convertedParameter, property.Property), typeof(object));
                    propertyChecks.Add(propertyExpression.Type == typeof(CycleDetector)
                        ? Expression.Invoke(propertyExpression, propertyExpression, accessExpression, hashSetParameter)
                        : Expression.Invoke(propertyExpression, accessExpression, hashSetParameter)
                    );
                }
            }

            if (propertyChecks.Count > 1)
            {
                var body = Expression.IfThen(
                    Expression.NotEqual(valueParameter, ReflectionConstants.NullObject),
                    Expression.IfThenElse(
                        Expression.Call(hashSetParameter, ReflectionConstants.HashSetAdd, valueParameter),
                        Expression.TryFinally(
                            Expression.Block(new[] { convertedParameter }, propertyChecks),
                            Expression.Call(hashSetParameter, ReflectionConstants.HashSetRemove, valueParameter)
                        ),
                        Expression.Throw(Expression.Constant(new InvalidOperationException("Objects with cycles cannot be serialized."), typeof(InvalidOperationException)))
                    )
                );

                var cycleHelper = _visited[type.UnderlyingType];
                if (!cycleHelper.Used)
                {
                    return Expression.Lambda<Action<object, HashSet<object>>>(body, valueParameter, hashSetParameter);
                }

                var recursiveLambda = Expression.Lambda<CycleDetector>(body, cycleHelper.Expression, valueParameter, hashSetParameter);
                var compiledRecursion = recursiveLambda.Compile();
                var recursionConstant = Expression.Constant(compiledRecursion);
                return Expression.Lambda<Action<object, HashSet<object>>>(
                    Expression.Invoke(recursionConstant, recursionConstant, valueParameter, hashSetParameter),
                    valueParameter,
                    hashSetParameter);
            }

            return null;
        }

        protected override Expression VisitCustom(DataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Custom data type {0} is not supported.", type.UnderlyingType.FullName));
        }

        protected override Expression VisitExpression(ExpressionDataType type) => null;

        protected override Expression VisitOpenGenericParameter(OpenGenericParameterDataType type) => throw new NotSupportedException("Open generic parameter types cannot have instances, let alone be serialized.");

        protected override Expression VisitPrimitive(PrimitiveDataType type) => null;

        private struct CycleHelper
        {
            public ParameterExpression Expression;
            public bool Used;
        }
    }
}
