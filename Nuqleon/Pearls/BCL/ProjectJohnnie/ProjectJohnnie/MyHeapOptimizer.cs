// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;

namespace ProjectJohnnie
{
    //
    // WARNING: This type of optimizer takes away guarantees about reference equality, which may be
    //          used in various places.
    //

    internal sealed class MyHeapOptimizer : HeapOptimizer
    {
        private static readonly MethodInfo s_getEqualityComparer = typeof(HeapOptimizer).GetMethod(nameof(GetEqualityComparer), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private readonly HashSet<Type> _immutableTypes = new()
        {
            typeof(decimal),
            typeof(Uri),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),

            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>),
        };

        protected override bool IsImmutable(Type type)
        {
            if (base.IsImmutable(type))
            {
                return true;
            }

            if (_immutableTypes.Contains(type))
            {
                return true;
            }

            if (typeof(ConstantExpression).IsAssignableFrom(type) ||
                typeof(DefaultExpression).IsAssignableFrom(type))
            {
                return true;
            }

            if (type.IsConstructedGenericType)
            {
                return IsImmutable(type.GetGenericTypeDefinition()) && type.GetGenericArguments().All(IsImmutable);
            }

            return false;
        }

        protected override IEqualityComparer<T> GetEqualityComparer<T>()
        {
            if (TryGetEqualityComparer(typeof(T), out var res))
            {
                return (IEqualityComparer<T>)res;
            }

            return base.GetEqualityComparer<T>();
        }

        private object GetEqualityComparer(Type type)
        {
            return s_getEqualityComparer.MakeGenericMethod(new[] { type }).Invoke(this, parameters: null);
        }

        private bool TryGetEqualityComparer(Type type, out object res)
        {
            if (type == typeof(Uri))
            {
                res = new UriEqualityComparer();
                return true;
            }

            if (type == typeof(DefaultExpression) || type == typeof(ConstantExpression))
            {
                res = new ExpressionEqualityComparer();
                return true;
            }

            if (type.IsConstructedGenericType)
            {
                var genDef = type.GetGenericTypeDefinition();
                var genArgs = type.GetGenericArguments();

                var eqType = default(Type);

                if (genDef == typeof(Tuple<>))
                {
                    eqType = typeof(TupleEqualityComparer<>);
                }
                else if (genDef == typeof(Tuple<,>))
                {
                    eqType = typeof(TupleEqualityComparer<,>);
                }
                else if (genDef == typeof(Tuple<,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,>);
                }
                else if (genDef == typeof(Tuple<,,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,,>);
                }
                else if (genDef == typeof(Tuple<,,,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,,,>);
                }
                else if (genDef == typeof(Tuple<,,,,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,,,,>);
                }
                else if (genDef == typeof(Tuple<,,,,,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,,,,,>);
                }
                else if (genDef == typeof(Tuple<,,,,,,,>))
                {
                    eqType = typeof(TupleEqualityComparer<,,,,,,,>);
                }

                if (eqType != null)
                {
                    var args = genArgs.Select(GetEqualityComparer).ToArray();
                    res = Activator.CreateInstance(eqType.MakeGenericType(genArgs), args);
                    return true;
                }
            }

            res = null;
            return false;
        }

        private sealed class UriEqualityComparer : IEqualityComparer<Uri>
        {
            public bool Equals(Uri x, Uri y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return x.OriginalString == y.OriginalString;
            }

            public int GetHashCode(Uri obj)
            {
                if (obj == null)
                    return 0;

                return obj.OriginalString.GetHashCode();
            }
        }

        private sealed class ExpressionEqualityComparer : IEqualityComparer<Expression>
        {
            public bool Equals(Expression x, Expression y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.NodeType != y.NodeType)
                    return false;

                return x.NodeType switch
                {
                    ExpressionType.Default => x.Type == y.Type,
                    ExpressionType.Constant => x.Type == y.Type && object.Equals(((ConstantExpression)x).Value, ((ConstantExpression)y).Value) /* TODO */,
                    _ => object.Equals(x, y),
                };
            }

            public int GetHashCode(Expression obj)
            {
                if (obj == null)
                    return 0;

                return obj.NodeType switch
                {
                    ExpressionType.Default => obj.Type.GetHashCode(),
                    ExpressionType.Constant => ((ConstantExpression)obj).Value?.GetHashCode() ?? 0,
                    _ => obj.GetHashCode(),
                };
            }
        }
    }
}
