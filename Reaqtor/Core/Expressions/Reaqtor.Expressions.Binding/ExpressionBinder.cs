// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

#if DEBUG
using System.Linq.CompilerServices.TypeSystem;
#endif

namespace Reaqtor.Expressions.Binding
{
    /// <summary>
    /// Helper class to visit expressions and bind them to known types.
    /// </summary>
    public abstract class ExpressionBinder
    {
        /// <summary>
        /// All empty generic types for Func.
        /// </summary>
        protected static readonly HashSet<Type> FuncTypes = new()
        {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
        };

        /// <summary>
        /// Create an instance of ExpressionBinder type.
        /// </summary>
        protected ExpressionBinder()
        {
        }

        /// <summary>
        /// Bind unbound parameters in the given expression.
        /// </summary>
        /// <param name="expression">Expression to bind.</param>
        /// <returns>The result of binding unbound parameters in the given expression..</returns>
        public Expression Bind(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var freeVariables = FreeVariableScanner.Scan(expression);

            if (!freeVariables.Any())
            {
                return expression;
            }

            var bindings = new Dictionary<ParameterExpression, Expression>();

            foreach (var variable in freeVariables)
            {
                Expression value = Lookup(variable);
                if (value == null)
                {
                    // Leave unbound
                    continue;
                }

                bindings[variable] = Bind(value);
            }

            if (bindings.Count == 0)
            {
                return expression;
            }

            return ReduceEager(Inline(expression, bindings));
        }

        /// <summary>
        /// Inline an expression given the bindings.
        /// </summary>
        /// <param name="expression">Expression to inline.</param>
        /// <param name="bindings">Bindings.</param>
        /// <returns>Expression with the bindings applied to perform inlining.</returns>
        protected virtual Expression Inline(Expression expression, IDictionary<ParameterExpression, Expression> bindings) => new Inliner(bindings).Visit(expression);

        private sealed class Inliner : ScopedExpressionVisitor<Expression>
        {
            private readonly IDictionary<ParameterExpression, Expression> _map;

            public Inliner(IDictionary<ParameterExpression, Expression> map) => _map = map;

            protected override Expression GetState(ParameterExpression parameter) => parameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (TryLookup(node, out Expression res))
                {
                    return res;
                }

                return Lookup(node);
            }

            private Expression Lookup(ParameterExpression parameter)
            {
                if (_map.TryGetValue(parameter, out Expression res))
                {
                    return res;
                }

                return parameter;
            }
        }

        /// <summary>
        /// Look up parameter expressions in a table.
        /// </summary>
        /// <param name="variable">Parameter expression to look for.</param>
        /// <returns>Bound expression represented by the original expression.</returns>
        public virtual Expression Lookup(ParameterExpression variable)
        {
            if (variable == null)
                throw new ArgumentNullException(nameof(variable));

            Expression value = LookupCore(variable.Name, variable.Type);
            if (value == null)
            {
                return null;
            }

            Type originalType = variable.Type;
            Type valueType = value.Type;

            // Relaxing exact unification by allowing type substitutions in the case that the
            // types may not be equal, but are still structurally equivalent.  For example, when
            // streams are created, their generic entity type is anonymous.  When we try to bind
            // to that stream and perform the type unification below, we may end up with two
            // anonymous types that are structurally the same, but would not pass the reference
            // equality constraints of the type unification process.  This substitution overcomes
            // that limitation.
            var substitutionComparer = new StructuralSubstitutingTypeComparator();
            if (substitutionComparer.Equals(originalType, valueType) && substitutionComparer.Substitutions.Count > 0)
            {
                var substitutor = new StructuralTypeSubstitutionExpressionVisitor(substitutionComparer.Substitutions);
                value = substitutor.Apply(value);
                valueType = value.Type;
            }

#if DEBUG
            Debug.Assert(!ContainsWildcard(originalType));
#endif
            var unificationTypeSubstitutions = originalType.UnifyReferenceAssignableFrom(valueType);

            if (unificationTypeSubstitutions.Count == 0)
            {
                return value;
            }

            var substitutionVisitor = new TypeSubstitutionExpressionVisitor(unificationTypeSubstitutions);
            var result = substitutionVisitor.Apply(value);

            return result;
        }

        /// <summary>
        /// Find the function to resolve expressions.
        /// </summary>
        /// <param name="id">Id of the parameter expression.</param>
        /// <param name="type">Type of the expression.</param>
        /// <param name="funcType">Type of the function.</param>
        /// <returns>Resolved loop up function</returns>
        protected abstract Func<string, Type, Type, Expression> ResolveLookupFunc(string id, Type type, Type funcType);

        /// <summary>
        /// Core implementation of look up an expression.
        /// </summary>
        /// <param name="id">Expression Id.</param>
        /// <param name="type">Expression type.</param>
        /// <returns>Resolved expression.</returns>
        protected virtual Expression LookupCore(string id, Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Func<string, Type, Type, Expression> lookupFunc;
            Type funcType = null;

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();

                if (FuncTypes.Contains(genericType))
                {
                    funcType = type;
                    type = type.GetGenericArguments().Last();
                }
            }

            lookupFunc = ResolveLookupFunc(id, type, funcType);

            Debug.Assert(lookupFunc != null);

            return lookupFunc(id, type, funcType);
        }

        /// <summary>
        /// Reduce eager expression.
        /// </summary>
        /// <param name="expression">Reduceable expression.</param>
        /// <returns>Reduced expression.</returns>
        protected static Expression ReduceEager(Expression expression)
        {
            var current = expression;
            var reduced = default(Expression);

            var N = 16;
            var i = 0;
            while (current != reduced && i < N /* protected against recursive lambdas like omega */)
            {
                reduced = current;
                current = BetaReducer.Reduce(current, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);
                i++;
            }

            return current;
        }

        /// <summary>
        /// Get result type.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <returns>Result type.</returns>
        protected static Type GetResultType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsGenericType)
            {
                Type generic = type.GetGenericTypeDefinition();

                if (FuncTypes.Contains(generic))
                {
                    return type.GetGenericArguments().Last();
                }
            }

            return type;
        }

#if DEBUG
        private static bool ContainsWildcard(Type type)
        {
            var visitor = new WildcardChecker();
            visitor.Visit(type);
            return visitor.ContainsWildcard;
        }

        private sealed class WildcardChecker : TypeVisitor
        {
            public bool ContainsWildcard;

            public override Type Visit(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                // This check is not optimized, do not use this visitor except for debugging purposes.
                ContainsWildcard |= type.IsDefined(typeof(TypeWildcardAttribute), inherit: false);
                return base.Visit(type);
            }
        }
#endif
    }
}
