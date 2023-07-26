// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Provides expression tree rewrite functionality to substitute anonymous types with tuple types.
    /// This rewrite is useful to reduce dependencies on compiler-generated types, e.g. for serialization.
    /// </summary>
    public static class AnonymousTypeTupletizer
    {
        /// <summary>
        /// Substitutes occurrences of anonymous types in the specified expression by replacing those for tuple types.
        /// Types visible in the expression result type are included in the tupletization.
        /// </summary>
        /// <param name="expression">Expression to tupletize.</param>
        /// <param name="unitValue">Expression representing the unit value, used for anonymous types with no properties (e.g. new { } in C#). The type of the expression can't be a value type, but you can use a boxing conversion to work around this limitation.</param>
        /// <returns>Expression tree with anonymous types substituted for tuple types.</returns>
        /// <remarks>
        /// Assignments to Visual Basic anonymous type properties will cause the rewrite to fail.
        /// Semantics of tuples are similar to those of C# anonymous types, with every property participating in equality semantics.
        /// </remarks>
        public static Expression Tupletize(Expression expression, Expression unitValue)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (unitValue == null)
                throw new ArgumentNullException(nameof(unitValue));

            return TupletizeImpl(expression, unitValue, excludeVisibleTypes: false);
        }

        /// <summary>
        /// Substitutes occurrences of anonymous types in the specified expression by replacing those for tuple types.
        /// Types visible in the expression result type can be excluded explicitly.
        /// </summary>
        /// <param name="expression">Expression to tupletize.</param>
        /// <param name="unitValue">Expression representing the unit value, used for anonymous types with no properties (e.g. new { } in C#). The type of the expression can't be a value type, but you can use a boxing conversion to work around this limitation.</param>
        /// <param name="excludeVisibleTypes">Indicates whether to exclude visible types (i.e. occurring in the expression result type) from the tupletization rewrite.</param>
        /// <returns>Expression tree with anonymous types substituted for tuple types.</returns>
        /// <remarks>
        /// Assignments to Visual Basic anonymous type properties will cause the rewrite to fail.
        /// Semantics of tuples are similar to those of C# anonymous types, with every property participating in equality semantics.
        /// </remarks>
        public static Expression Tupletize(Expression expression, Expression unitValue, bool excludeVisibleTypes)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (unitValue == null)
                throw new ArgumentNullException(nameof(unitValue));

            return TupletizeImpl(expression, unitValue, excludeVisibleTypes);
        }

        private static Expression TupletizeImpl(Expression expression, Expression unitValue, bool excludeVisibleTypes)
        {
            if (unitValue.Type.IsValueType)
                throw new ArgumentException("Unit value can't be a value type.", nameof(unitValue));

            var atf = new AnonymousTypeFinder();
            var anons = atf.Find(expression, excludeVisibleTypes);

            var tt = new TypeTupletizer(anons, unitValue);

            foreach (var anon in anons)
            {
                tt.Visit(anon);
            }

            var tupletize = new Tupletizer(tt.TypeMap, tt.ConstructorMap, tt.PropertyMap, tt.ConstantConverters);
            var res = tupletize.Visit(expression);

            return res;
        }

        private sealed class AnonymousTypeFinder : ExpressionVisitor
        {
            private readonly TypeFinder _finder = new();

            public HashSet<Type> Find(Expression expression, bool excludeVisibleTypes)
            {
                Visit(expression);

                var res = _finder._anons;

                if (excludeVisibleTypes)
                {
                    var exclusionFinder = new TypeFinder();
                    exclusionFinder.Visit(expression.Type);
                    res.ExceptWith(exclusionFinder._anons);
                }

                return res;
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    _finder.Visit(node.Type);
                }

                return base.Visit(node);
            }

            private sealed class TypeFinder : TypeVisitor
            {
                public readonly HashSet<Type> _anons = new();

                public override Type Visit(Type type)
                {
                    if (type != null && !type.IsGenericTypeDefinition && type.IsAnonymousType())
                    {
                        _anons.Add(type);
                    }

                    return base.Visit(type);
                }
            }
        }

        private sealed class TypeTupletizer : TypeVisitor
        {
            private static ConstructorInfo s_invalidOperationExceptionCtor;

            private static ConstructorInfo InvalidOperationExceptionCtor => s_invalidOperationExceptionCtor ??= typeof(InvalidOperationException).GetConstructor(new[] { typeof(string) });

            private readonly Expression _unit;

            public TypeTupletizer(IEnumerable<Type> subst, Expression unit)
            {
                TypeMap = subst.ToDictionary(t => t, t => default(Type));
                ConstructorMap = new Dictionary<Type, Func<NewExpression, Func<Expression, Expression>, Expression>>();
                PropertyMap = new Dictionary<Type, Func<MemberExpression, Func<Expression, Expression>, Expression>>();
                ConstantConverters = new Dictionary<Type, Func<object, Func<object, object>, object>>();
                _unit = unit;
            }

            public Dictionary<Type, Type> TypeMap { get; }

            public Dictionary<Type, Func<NewExpression, Func<Expression, Expression>, Expression>> ConstructorMap { get; }

            public Dictionary<Type, Func<MemberExpression, Func<Expression, Expression>, Expression>> PropertyMap { get; }

            public Dictionary<Type, Func<object, Func<object, object>, object>> ConstantConverters { get; }

            public override Type Visit(Type type)
            {
                if (TypeMap.TryGetValue(type, out Type res))
                {
                    if (res != null)
                    {
                        if (res == typeof(RecursionCanary))
                        {
                            throw new NotSupportedException("Recursive type definition detected for type " + type + ".");
                        }

                        return res;
                    }
                    else
                    {
                        TypeMap[type] = typeof(RecursionCanary);

                        return VisitAnonymous(type);
                    }
                }

                return base.Visit(type);
            }

            private Type VisitAnonymous(Type type)
            {
                Debug.Assert(type.IsAnonymousType());

                var newType = default(Type);
                var getNewCtor = default(Func<NewExpression, Func<Expression, Expression>, Expression>);
                var getNewMember = default(Func<MemberExpression, Func<Expression, Expression>, Expression>);
                var convertConstant = default(Func<object, Func<object, object>, object>);

                var oldCtor = type.GetConstructors().Single();
                var oldPars = oldCtor.GetParameters();

                var n = oldPars.Length;

                if (n == 0)
                {
                    newType = _unit.Type;
                    getNewCtor = (ne, _) => _unit;
                    convertConstant = (ce, _) => _unit.Evaluate<object>();
                }
                else
                {
                    var ctors = new List<ConstructorInfo>();
                    var props = new Dictionary<MemberInfo, Stack<PropertyInfo>>();
                    var convp = Expression.Parameter(type);
                    var convr = Expression.Parameter(typeof(Func<object, object>));
                    var conve = default(Expression);

                    var oldProps = type.GetProperties().ToDictionary(p => p.Name, p => p);

                    var max = ExpressionTupletizer.TupleTypeCount - 2 /* T1..T7 */;

                    var fst = (n / max) * max;
                    var rem = n - fst;

                    if (rem == 0)
                    {
                        fst = n - max;
                        rem = max;
                    }

                    for (var i = fst; i >= 0; i -= max)
                    {
                        var tupleArgs = default(Type[]);
                        var constArgs = default(Expression[]);

                        if (newType == null)
                        {
                            tupleArgs = new Type[rem];
                            constArgs = new Expression[rem];
                        }
                        else
                        {
                            tupleArgs = new Type[rem + 1];
                            tupleArgs[rem] = newType;

                            constArgs = new Expression[rem + 1];
                            constArgs[rem] = conve;
                        }

                        var originalProps = new List<PropertyInfo>(rem);

                        for (var j = 0; j < rem; j++)
                        {
                            var par = oldPars[i + j];
                            var prop = oldProps[par.Name];
                            tupleArgs[j] = Visit(par.ParameterType);
                            originalProps.Add(prop);
                        }

                        newType = ExpressionTupletizer.TupleTypes[tupleArgs.Length].MakeGenericType(tupleArgs);

                        var ctor = newType.GetConstructor(tupleArgs);
                        ctors.Add(ctor);

                        if (rem == max)
                        {
                            var rest = newType.GetProperty("Rest");

                            foreach (var kv in props)
                            {
                                kv.Value.Push(rest);
                            }
                        }

                        var ctorPars = ctor.GetParameters();

                        for (var j = 0; j < originalProps.Count; j++)
                        {
                            var prop = originalProps[j];

                            var newProp = new Stack<PropertyInfo>();
                            var item = newType.GetProperty("Item" + (j + 1));
                            newProp.Push(item);

                            props[prop] = newProp;

                            var member = (Expression)Expression.MakeMemberAccess(convp, prop);

                            if (TypeMap.ContainsKey(prop.PropertyType))
                            {
                                member = Expression.Convert(Expression.Invoke(convr, member), item.PropertyType);
                            }
                            else
                            {
                                var ctorParameterType = ctorPars[j].ParameterType;
                                var argType = member.Type;

                                if (!ctorParameterType.IsAssignableFrom(argType))
                                {
                                    var cantConvertMsg = string.Format(CultureInfo.InvariantCulture, "Can't convert constant of type '{0}' to '{1}'.", argType, ctorParameterType);
                                    var cantConvertException = Expression.New(InvalidOperationExceptionCtor, Expression.Constant(cantConvertMsg));
                                    member = Expression.Throw(cantConvertException, ctorParameterType);
                                }
                            }

                            constArgs[j] = member;
                        }

                        conve = Expression.New(ctor, constArgs);

                        rem = max;
                    }

                    getNewCtor = (ne, visit) =>
                    {
                        var res = default(Expression);

                        var args = ne.Arguments.Select(visit).ToArray();

                        var k = args.Length;
                        foreach (var c in ctors)
                        {
                            var paras = c.GetParameters();
                            var m = paras.Length;

                            var bundle = new Expression[m];

                            if (res != null)
                            {
                                bundle[m - 1] = res;
                                m--;
                            }

                            k -= m;

                            for (var j = 0; j < m; j++)
                            {
                                bundle[j] = args[k + j];
                            }

                            res = Expression.New(c, bundle);
                        }

                        return res;
                    };

                    getNewMember = (me, visit) =>
                    {
                        var expr = visit(me.Expression);

                        foreach (var p in props[me.Member])
                        {
                            expr = Expression.MakeMemberAccess(expr, p);
                        }

                        return expr;
                    };

                    var convf = Expression.Lambda(conve, convp, convr);
                    var convd = default(Delegate);

                    convertConstant = (cst, visit) =>
                    {
                        convd ??= convf.Compile();

                        return convd.DynamicInvoke(cst, visit);
                    };
                }

                TypeMap[type] = newType;
                ConstructorMap[type] = getNewCtor;
                PropertyMap[type] = getNewMember;
                ConstantConverters[type] = convertConstant;

                return newType;
            }

            private struct RecursionCanary
            {
            }
        }

        private sealed class Tupletizer : TypeSubstitutionExpressionVisitor
        {
            private readonly Dictionary<Type, Func<NewExpression, Func<Expression, Expression>, Expression>> _ctorMap;
            private readonly Dictionary<Type, Func<MemberExpression, Func<Expression, Expression>, Expression>> _propMap;
            private readonly Dictionary<Type, Func<object, Func<object, object>, object>> _constConv;

            public Tupletizer(Dictionary<Type, Type> typeMap, Dictionary<Type, Func<NewExpression, Func<Expression, Expression>, Expression>> ctorMap, Dictionary<Type, Func<MemberExpression, Func<Expression, Expression>, Expression>> propMap, Dictionary<Type, Func<object, Func<object, object>, object>> constConv)
                : base(typeMap)
            {
                _ctorMap = ctorMap;
                _propMap = propMap;
                _constConv = constConv;
            }

            protected override Expression VisitNew(NewExpression node)
            {
                if (_ctorMap.TryGetValue(node.Type, out Func<NewExpression, Func<Expression, Expression>, Expression> rewrite))
                {
                    return rewrite(node, Visit);
                }

                return base.VisitNew(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (_propMap.TryGetValue(node.Member.DeclaringType, out Func<MemberExpression, Func<Expression, Expression>, Expression> rewrite))
                {
                    return rewrite(node, Visit);
                }

                return base.VisitMember(node);
            }

            protected override object ConvertConstant(object originalValue, Type newType) => ConvertConstant(originalValue);

            private object ConvertConstant(object originalValue)
            {
                var res = originalValue;

                if (originalValue != null)
                {
                    var t = originalValue.GetType();

                    if (_constConv.TryGetValue(t, out Func<object, Func<object, object>, object> rewrite))
                    {
                        res = rewrite(originalValue, ConvertConstant);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert constant of type '{0}'.", t));
                    }
                }

                return res;
            }
        }
    }
}
