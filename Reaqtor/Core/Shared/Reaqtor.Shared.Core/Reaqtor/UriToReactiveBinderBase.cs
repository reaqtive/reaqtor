// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitted null checks for protected overrides.)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for binders that bind URI-based artifact references to an IReactive context.
    /// </summary>
    public abstract class UriToReactiveBinderBase
    {
        /// <summary>
        /// Binds the specified expression to an IReactive context represented by the ThisParameter property.
        /// </summary>
        /// <param name="expr">Expression to bind.</param>
        /// <returns>Rewritten expression where URI-based artifact references have been replaced by IReactive calls.</returns>
        public Expression Bind(Expression expr)
        {
            SetThisParameter(expr);

            var binder = new BindingVisitor(this);

            var bound = binder.Visit(expr);

            var lambda = binder.Parameters.Count > 0 ? Expression.Lambda(bound, ThisParameter) : (Expression)Expression.Lambda(bound);

            return lambda;
        }

        /// <summary>
        /// Sets the ThisParameter property.
        /// </summary>
        /// <param name="expr">Expression representing the 'this' reference.</param>
        protected void SetThisParameter(Expression expr)
        {
            if (ThisParameter != null)
            {
                return;
            }

            var findThis = new FindThisParameterVisitor(ThisType);
            findThis.Visit(expr);

            if (findThis.ThisParameter != null)
            {
                ThisParameter = findThis.ThisParameter;
            }
            else
            {
                ThisParameter = (ParameterExpression)ResourceNaming.GetThisReferenceExpression(ThisType);
            }
        }

        /// <summary>
        /// Gets the parameter representing the 'this' reference.
        /// </summary>
        protected ParameterExpression ThisParameter
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the 'this' reference.
        /// </summary>
        protected abstract Type ThisType
        {
            get;
        }

        /// <summary>
        /// Looks up the artifact with the specified identifier and type, returning its bound expression representation.
        /// </summary>
        /// <param name="id">Identifier representing the artifact.</param>
        /// <param name="type">Type of the artifact.</param>
        /// <param name="funcType">Function type for parameterized artifacts.</param>
        /// <returns>Bound expression representation of the artifact.</returns>
        protected abstract Expression Lookup(string id, Type type, Type funcType);

        private class FindThisParameterVisitor : ExpressionVisitor
        {
            private readonly Type _thisType;

            public FindThisParameterVisitor(Type thisType)
            {
                Debug.Assert(thisType != null);
                _thisType = thisType;
            }

            public ParameterExpression ThisParameter
            {
                get;
                private set;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ResourceNaming.IsThisReferenceExpression(_thisType, node))
                {
                    Debug.Assert(ThisParameter == null || ThisParameter == node);
                    ThisParameter = node;
                }

                return base.VisitParameter(node);
            }
        }

        /// <summary>
        /// Visitor to bind unbound parameters.
        /// </summary>
        protected class BindingVisitor : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly UriToReactiveBinderBase _parent;
            private readonly IDictionary<ParameterExpression, Expression> _bindings;

            /// <summary>
            /// Creates a new binding visitor.
            /// </summary>
            /// <param name="parent">Parent binder.</param>
            public BindingVisitor(UriToReactiveBinderBase parent)
            {
                _parent = parent;
                _bindings = new Dictionary<ParameterExpression, Expression>();
            }

            /// <summary>
            /// Gets the parameters to be bound.
            /// </summary>
            public ICollection<ParameterExpression> Parameters => _bindings.Keys;

            /// <summary>
            /// Maps the specified parameter on state tracked in the scoped symbol table.
            /// </summary>
            /// <param name="parameter">Parameter expression that's being declared.</param>
            /// <returns>The parameter itself.</returns>
            protected sealed override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            /// <summary>
            /// Visit parameters in the expression, checking for unbound status. If unbound, the parameter gets bound.
            /// </summary>
            /// <param name="node">Parameter to analyze.</param>
            /// <returns>If bound, the original parameter is returned. Otherwise, the result of binding the parameter is returned.</returns>
            protected sealed override Expression VisitParameter(ParameterExpression node)
            {
                if (IsUnboundParameter(node) && node != _parent.ThisParameter)
                {
                    return Bind(node);
                }

                return base.VisitParameter(node);
            }

            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);

            private Expression Bind(ParameterExpression parameter)
            {
                if (_bindings.TryGetValue(parameter, out var result))
                {
                    return result;
                }

                Type type = parameter.Type;
                string id = parameter.Name;

                Type funcType = null;

                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();

                    if (genericType == typeof(Func<>) ||
                        genericType == typeof(Func<,>) ||
                        genericType == typeof(Func<,,>))
                    {
                        funcType = type;
                        type = type.GetGenericArguments().Last();
                    }
                }

                result = _parent.Lookup(id, type, funcType);
                _bindings.Add(parameter, result);

                Debug.Assert(result != null, "Binding failed");

                return result;
            }
        }
    }
}
