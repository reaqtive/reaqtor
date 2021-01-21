// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using Expression = System.Linq.Expressions.ExpressionSlim;
    using LambdaExpression = System.Linq.Expressions.LambdaExpressionSlim;
    using MemberExpression = System.Linq.Expressions.MemberExpressionSlim;
    using NewExpression = System.Linq.Expressions.NewExpressionSlim;
    using ParameterExpression = System.Linq.Expressions.ParameterExpressionSlim;

    using FreeVariableScanner = System.Reflection.FreeVariableScannerSlim;

    using Type = System.Reflection.TypeSlim;
    using PropertyInfo = System.Reflection.PropertyInfoSlim;

    #endregion
#endif

    /// <summary>
    /// Provides a set of methods to pack and unpack expressions into/from tuples.
    /// </summary>
#if USE_SLIM
    public static class ExpressionSlimTupletizer
#else
    public static class ExpressionTupletizer
#endif
    {
#if USE_SLIM
        private static readonly Type s_voidType = typeof(void).ToTypeSlim();
#else
        private static readonly Type s_voidType = typeof(void);
#endif

        /// <summary>
        /// Packs the specified expressions into an expression that creates a tuple of the expressions.
        /// </summary>
        /// <param name="expressions">Expressions to pack in a tuple.</param>
        /// <returns>Expression representing the creation of a tuple of the given expressions.</returns>
        public static Expression Pack(IEnumerable<Expression> expressions)
        {
            if (expressions == null)
                throw new ArgumentNullException(nameof(expressions));

            var exprs = expressions.AsArray();

            if (exprs.Length == 0)
                throw new ArgumentException("At least one expression should be supplied.", nameof(expressions));

            foreach (var e in exprs)
            {
#if USE_SLIM
                var type = Derive(e);
#else
                var type = e.Type;
#endif
                if (type == s_voidType)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' of type 'void' cannot be used as a component of a tuple.", e), nameof(expressions));
                }
            }

            return PackImpl(exprs);
        }

        /// <summary>
        /// Unpacks the specified expression representing a tuple into the expressions of its components.
        /// </summary>
        /// <param name="expression">Expression to unpack into its component expressions.</param>
        /// <returns>Expressions representing the components of the tuple represented by the given expression.</returns>
        public static IEnumerable<Expression> Unpack(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return UnpackImpl(expression);
        }

        /// <summary>
        /// Packs the lambda expression parameters into a tuple.
        /// If the lambda expression has no parameters, this method is a no-op.
        /// If the lambda expression has exactly one parameter, the parameter will get packed in a 1-tuple.
        /// </summary>
        /// <param name="expression">Expression to pack parameters for.</param>
        /// <returns>Lambda expression with parameters packed as a tuple.</returns>
        public static LambdaExpression Pack(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return PackImpl(expression, voidType: null);
        }

        /// <summary>
        /// Packs the lambda expression parameters into a tuple.
        /// If the lambda expression has no parameters, a dummy parameter of the specified 'void' type will be inserted.
        /// If the lambda expression has exactly one parameter, the parameter will get packed in a 1-tuple.
        /// </summary>
        /// <param name="expression">Expression to pack parameters for.</param>
        /// <param name="voidType">Type used to represent an empty parameter list as a single 'void' parameter.</param>
        /// <returns>Lambda expression with parameters packed as a tuple.</returns>
        public static LambdaExpression Pack(LambdaExpression expression, Type voidType)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (voidType == null)
                throw new ArgumentNullException(nameof(voidType));

            return PackImpl(expression, voidType);
        }

        /// <summary>
        /// Creates a lambda expression whose parameters are packed in a tuple.
        /// If no parameters are specified, a lambda with no parameters is returned.
        /// If exactly one parameter is specified, the parameter will get packed in a 1-tuple.
        /// </summary>
        /// <param name="body">Body of the lambda expression. Occurrences of the specified parameters will get packed in tuples.</param>
        /// <param name="parameters">Parameters to be packed into a single lambda parameter.</param>
        /// <returns>Lambda expression with parameters packed as a tuple.</returns>
        public static LambdaExpression Pack(Expression body, IEnumerable<ParameterExpression> parameters)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return PackImpl(body, parameters.AsArray(), voidType: null);
        }

        /// <summary>
        /// Creates a lambda expression whose parameters are packed in a tuple.
        /// If no parameters are specified, a lambda with a dummy parameter of the specified 'void' type will be returned.
        /// If exactly one parameter is specified, the parameter will get packed in a 1-tuple.
        /// </summary>
        /// <param name="body">Body of the lambda expression. Occurrences of the specified parameters will get packed in tuples.</param>
        /// <param name="parameters">Parameters to be packed into a single lambda parameter.</param>
        /// <param name="voidType">Type used to represent an empty parameter list as a single 'void' parameter.</param>
        /// <returns>Lambda expression with parameters packed as a tuple.</returns>
        public static LambdaExpression Pack(Expression body, IEnumerable<ParameterExpression> parameters, Type voidType)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (voidType == null)
                throw new ArgumentNullException(nameof(voidType));

            return PackImpl(body, parameters.AsArray(), voidType);
        }

        /// <summary>
        /// Unpacks the lambda expression with a tuple-based parameter into a lambda expression with parameters for the tuple components.
        /// If the lambda expression has no parameters, this method is a no-op.
        /// </summary>
        /// <param name="expression">Expression to unpack the tuple-based parameter into parameters for its components.</param>
        /// <returns>Lambda expression with parameters unpacked from the tuple-based parameter.</returns>
        public static LambdaExpression Unpack(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return UnpackImpl(expression, voidType: null);
        }

        /// <summary>
        /// Unpacks the lambda expression with a tuple-based parameter into a lambda expression with parameters for the tuple components.
        /// If the lambda expression has no parameters, this method is a no-op.
        /// If the lambda expression has one parameter of the specified 'void' type, it will get unpacked into a lambda expression with no parameters.
        /// </summary>
        /// <param name="expression">Expression to unpack the tuple-based parameter into parameters for its components.</param>
        /// <param name="voidType">Type used to represent an empty parameter list as a single 'void' parameter.</param>
        /// <returns>Lambda expression with parameters unpacked from the tuple-based parameter.</returns>
        public static LambdaExpression Unpack(LambdaExpression expression, Type voidType)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (voidType == null)
                throw new ArgumentNullException(nameof(voidType));

            return UnpackImpl(expression, voidType);
        }

        /// <summary>
        /// Unpacks the lambda expression with a tuple-based parameter into a lambda body and its set of parameters.
        /// </summary>
        /// <param name="expression">Expression to unpack the tuple-based parameter into parameters for its components.</param>
        /// <param name="body">Lambda body with parameters unpacked from the tuple-based parameter.</param>
        /// <param name="parameters">The set of parameters that occur in the lambda body.</param>
        public static void Unpack(LambdaExpression expression, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            UnpackImpl(expression, voidType: null, out body, out parameters);
        }

        /// <summary>
        /// Unpacks the lambda expression with a tuple-based parameter into a lambda body and its set of parameters.
        /// </summary>
        /// <param name="expression">Expression to unpack the tuple-based parameter into parameters for its components.</param>
        /// <param name="voidType">Type used to represent an empty parameter list as a single 'void' parameter.</param>
        /// <param name="body">Lambda body with parameters unpacked from the tuple-based parameter.</param>
        /// <param name="parameters">The set of parameters that occur in the lambda body.</param>
        public static void Unpack(LambdaExpression expression, Type voidType, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (voidType == null)
                throw new ArgumentNullException(nameof(voidType));

            UnpackImpl(expression, voidType, out body, out parameters);
        }

        /// <summary>
        /// Checks whether the specified type is a packed tuple.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if the specified type is a tuple; otherwise, false.</returns>
        public static bool IsTuple(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!type.IsGenericType() || type.IsGenericTypeDefinition())
            {
                return false;
            }

            var genDef = type.GetGenericTypeDefinition();
            if (!TupleTypes.Contains(genDef))
            {
                return false;
            }

            if (genDef == TupleTypes[TupleTypeCount - 1])
            {
                return IsTuple(type.GetGenericArguments().Last());
            }

            return true;
        }

        private static LambdaExpression PackImpl(LambdaExpression expression, Type voidType)
        {
            if (expression.Parameters.Count == 0 && voidType == null)
            {
                return expression;
            }

            return PackImpl(expression.Body, expression.Parameters, voidType);
        }

        private static LambdaExpression PackImpl(Expression body, IList<ParameterExpression> parameters, Type voidType)
        {
            if (parameters.Count == 0)
            {
                if (voidType != null)
                {
                    return Expression.Lambda(body, Expression.Parameter(voidType, "_"));
                }
                else
                {
                    return Expression.Lambda(body, parameters);
                }
            }

#if USE_SLIM
            var tupleType = Derive(Pack(parameters));
#else
            var tupleType = Pack(parameters).Type;
#endif

            var tupleParameter = Expression.Parameter(tupleType, "t");

            var map = new Dictionary<ParameterExpression, Expression>();

            var lhs = (Expression)tupleParameter;
            const int restPosition = TupleTypeCount - 1 /* T1..T7,TRest */;

            var i = 1;
#if USE_SLIM
            var genericArguments = tupleType.GetGenericArguments();
            var emptyTypes = EmptyReadOnlyCollection<TypeSlim>.Instance;
#endif
            foreach (var parameter in parameters)
            {
                if (i == restPosition)
                {
#if USE_SLIM
                    lhs = Expression.Property(lhs, tupleType.GetProperty("Rest", genericArguments[restPosition - 1], emptyTypes, canWrite: false));
                    tupleType = genericArguments[restPosition - 1];
                    genericArguments = tupleType.GetGenericArguments();
#else
                    lhs = Expression.Property(lhs, "Rest");
#endif
                    i = 1;
                }

#if USE_SLIM
                map[parameter] = Expression.Property(lhs, tupleType.GetProperty(ItemNames[i], genericArguments[i - 1], emptyTypes, canWrite: false));
#else
                map[parameter] = Expression.Property(lhs, ItemNames[i]);
#endif
                i++;
            }

            var subst = new ParameterBasedTupletizer(map);

            var newBody = subst.Visit(body);

            var result = Expression.Lambda(newBody, tupleParameter);

            return result;
        }

#if USE_SLIM
        private sealed class ParameterBasedTupletizer : ScopedExpressionSlimVisitor<Expression>
#else
        private sealed class ParameterBasedTupletizer : ScopedExpressionVisitor<Expression>
#endif
        {
            private readonly Dictionary<ParameterExpression, Expression> _map;

            public ParameterBasedTupletizer(Dictionary<ParameterExpression, Expression> map)
            {
                _map = map;
            }

            protected override Expression GetState(ParameterExpression parameter) => parameter;

#if USE_SLIM
            protected internal override Expression VisitParameter(ParameterExpression node)
#else
            protected override Expression VisitParameter(ParameterExpression node)
#endif
            {
                if (TryLookup(node, out Expression res))
                {
                    return res;
                }

                if (_map.TryGetValue(node, out res))
                {
                    return res;
                }

                return node;
            }
        }

        private static LambdaExpression UnpackImpl(LambdaExpression expression, Type voidType)
        {
            var count = expression.Parameters.Count;
            if (count == 0)
            {
                return expression;
            }

            UnpackImpl(expression, voidType, out Expression body, out IEnumerable<ParameterExpression> parameters);

            return Expression.Lambda(body, parameters);
        }

        private static void UnpackImpl(LambdaExpression expression, Type voidType, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            var count = expression.Parameters.Count;
            if (count == 0)
            {
                body = expression.Body;
                parameters = expression.Parameters;
                return;
            }

            if (count != 1)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' has more than one parameter. In order to detupletize a lambda expression, it should have exactly one parameter of a tuple type.", expression));
            }

            var tupleParameter = expression.Parameters[0];
            var parameterType = tupleParameter.Type;

            if (parameterType == voidType)
            {
#if USE_SLIM
                if (FreeVariableScanner.Find(expression.Body).Contains(tupleParameter))
#else
                if (FreeVariableScanner.Scan(expression.Body).Contains(tupleParameter))
#endif
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The 'void' parameter is bound in the body of lambda expression '{0}'. In order to detupletize a lambda expression with a 'void' parameter, the parameter should be unbound.", expression));
                }

                body = expression.Body;
                parameters = Array.Empty<ParameterExpression>();
                return;
            }

            if (!IsTuple(parameterType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The type of parameter '{0}' in '{1}' is not a valid tuple type: '{2}'.", tupleParameter, expression, parameterType));
            }

            var tupleType = tupleParameter.Type;

            var newParameters = new List<ParameterExpression>();

            const int max = TupleTypeCount - 1 /* T1..T7,TRest */;

            var type = tupleType;

            var i = 0;
            while (true)
            {
                var args = type.GetGenericArguments();

                var n = args.Length;

                if (args.Length == max)
                {
                    n--;
                }

                for (var j = 0; j < n; j++)
                {
                    var arg = args[j];
                    newParameters.Add(Expression.Parameter(arg, "p" + i++));
                }

                if (args.Length == max)
                {
                    type = args.Last();
                }
                else
                {
                    break;
                }
            }

            var subst = new ParameterBasedDetupletizer(tupleParameter, newParameters);

            body = subst.Visit(expression.Body);
            parameters = newParameters;
        }

#if USE_SLIM
        private sealed class ParameterBasedDetupletizer : ScopedExpressionSlimVisitor<Expression>
#else
        private sealed class ParameterBasedDetupletizer : ScopedExpressionVisitor<Expression>
#endif
        {
            private readonly ParameterExpression _tupleParameter;
            private readonly IReadOnlyList<Expression> _parameters;

            public ParameterBasedDetupletizer(ParameterExpression tupleParameter, IReadOnlyList<Expression> parameters)
            {
                _tupleParameter = tupleParameter;
                _parameters = parameters;
            }

            protected override Expression GetState(ParameterExpression parameter)
            {
                return parameter;
            }

            private bool IsTupleAccessor(MemberExpression node, out int position)
            {
                var decl = node.Member.DeclaringType;

                if (decl.IsGenericType() && TupleTypes.Contains(decl.GetGenericTypeDefinition()))
                {
                    var n = 0;

#if USE_SLIM
                    var name = ((PropertyInfoSlim)node.Member).Name;
#else
                    var name = node.Member.Name;
#endif
                    if (name == "Rest")
                    {
                        n = TupleTypeCount - 2;
                    }
                    else
                    {
#if NET5_0 || NETSTANDARD2_1
                        n = int.Parse(name[4.. /* "Item".Length */], CultureInfo.InvariantCulture);
#else
                        n = int.Parse(name.Substring(4 /* "Item".Length */), CultureInfo.InvariantCulture);
#endif
                    }

                    if (node.Expression is MemberExpression lhs && IsTupleAccessor(lhs, out position))
                    {
                        position += n;
                        return true;
                    }

                    if (ReferenceEquals(node.Expression, _tupleParameter))
                    {
                        position = n;
                        return true;
                    }
                }

                position = 0;
                return false;
            }

#if USE_SLIM
            protected internal override Expression VisitMember(MemberExpression node)
#else
            protected override Expression VisitMember(MemberExpression node)
#endif
            {
                if (IsTupleAccessor(node, out int position))
                {
                    return _parameters[position - 1];
                }

                return base.VisitMember(node);
            }
        }

        private static NewExpression PackImpl(Expression[] expressions)
        {
            var n = expressions.Length;

            const int max = TupleTypeCount - 2 /* T1..T7 */;

            var fst = (n / max) * max;
            var rem = n - fst;

            if (rem == 0)
            {
                fst = n - max;
                rem = max;
            }

            var tuple = default(NewExpression);

            for (var i = fst; i >= 0; i -= max)
            {
                var len = rem + (tuple != null ? 1 : 0);

                var args = new Expression[len];
                Array.Copy(expressions, i, args, 0, rem);

                if (tuple != null)
                {
                    args[len - 1] = tuple;
                }

#if USE_SLIM
                var types = args.Select(Derive).ToReadOnly();
#else
                var types = args.Select(arg => arg.Type).ToArray();
#endif
                var tupleType = TupleTypes[args.Length].MakeGenericType(types);

                var members = new PropertyInfo[len];
                for (var j = 0; j < rem; j++)
                {
#if USE_SLIM
                    var property = tupleType.GetProperty(ItemNames[j + 1], types[j], EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: false);
#else
                    var property = tupleType.GetProperty(ItemNames[j + 1]);
#endif
                    members[j] = property;
                }

                if (tuple != null)
                {
#if USE_SLIM
                    members[len - 1] = tupleType.GetProperty("Rest", types[len - 1], EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: false);
#else
                    members[len - 1] = tupleType.GetProperty("Rest");
#endif
                }

                tuple = Expression.New(tupleType.GetConstructor(types), args, members);

                rem = max;
            }

            return tuple;
        }

        private static Expression[] UnpackImpl(Expression expression)
        {
            var res = UnpackImplIterator(expression).ToArray();
            return res;
        }

        private static IEnumerable<Expression> UnpackImplIterator(Expression expression)
        {
            var maxTupleType = TupleTypes[TupleTypeCount - 1];

            var tuple = expression;

            while (tuple != null)
            {
                if (tuple is not NewExpression newExpr)
                {
                    throw new InvalidOperationException("Only NewExpression nodes can be used to unpack tuples.");
                }

                var type = newExpr.Type;

                var tupleType = default(Type);

                if (type.IsGenericType())
                {
                    tupleType = type.GetGenericTypeDefinition();
                }

                if (tupleType == null || !TupleTypes.Contains(tupleType))
                {
                    throw new InvalidOperationException("Specified type is not a tuple type.");
                }

                var hasRest = tupleType == maxTupleType;

#if USE_SLIM
                var n = newExpr.ArgumentCount - (hasRest ? 1 : 0);

                for (var i = 0; i < n; i++)
                {
                    var arg = newExpr.GetArgument(i);
                    yield return arg;
                }

                if (hasRest)
                {
                    tuple = newExpr.GetArgument(n);
                }
#else
                var n = newExpr.Arguments.Count - (hasRest ? 1 : 0);

                for (var i = 0; i < n; i++)
                {
                    var arg = newExpr.Arguments[i];
                    yield return arg;
                }

                if (hasRest)
                {
                    tuple = newExpr.Arguments[n];
                }
#endif
                else
                {
                    tuple = null;
                }
            }
        }

#if USE_SLIM
        private static readonly TypeSlimDerivationVisitor s_derivationVisitor = new();

        private static TypeSlim Derive(ExpressionSlim expression)
        {
            var result = s_derivationVisitor.Visit(expression);
            if (result == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not derive type of '{0}' for use as component in tuple.", expression));
            }
            return result;
        }
#endif

        internal const int TupleTypeCount = 9; // NB: Keep this in sync with the arrays below

        internal static readonly Type[] TupleTypes = new Type[]
        {
#if USE_SLIM
            typeof(Placeholder).ToTypeSlim(), // Placeholder for the user-supplied unit type
            typeof(Tuple<>).ToTypeSlim(),
            typeof(Tuple<,>).ToTypeSlim(),
            typeof(Tuple<,,>).ToTypeSlim(),
            typeof(Tuple<,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,,,>).ToTypeSlim(),
#else
            typeof(Placeholder), // Placeholder for the user-supplied unit type
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>),
#endif
        };

        internal static readonly string[] ItemNames = new string[] // NB: Ensure that this matches the last tuple type's size
        {
            null,
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6",
            "Item7",
        };

        private sealed class Placeholder
        {
        }
    }
}
