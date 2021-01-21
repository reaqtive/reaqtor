// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// An expression rewriter that rewrites using a set of user defined lambdas
    /// that are applied when the runtime type of the expression matches the user
    /// supplied value.
    /// </summary>
    public class TypeBasedExpressionRewriter : ScopedExpressionVisitor<ParameterExpression>
    {
        private readonly List<TypedRewriter> _typeRewriters;

        /// <summary>
        /// Instantiates the type-based expression rewriter.
        /// </summary>
        public TypeBasedExpressionRewriter() => _typeRewriters = new List<TypedRewriter>();

        /// <summary>
        /// Visits an expression, looping over the list of user supplied
        /// rewriters, and rewriting the expression if the expression type
        /// matches any of the supplied rewriters.
        /// </summary>
        /// <param name="node">The expression.</param>
        /// <returns>The result of the visit.</returns>
        public override Expression Visit(Expression node)
        {
            var res = base.Visit(node);

            if (res != null)
            {
                foreach (var rewriter in _typeRewriters)
                {
                    if (rewriter.Match(res))
                    {
                        return rewriter.Rewrite(res);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Adds a rewriter for a specified type. The user supplied rewriter is applied
        /// whenever an expression type can be assigned to the user supplied type.
        /// </summary>
        /// <param name="type">The type that triggers the rewriter.</param>
        /// <param name="rewriter">The expression rewriter.</param>
        public void Add(Type type, Func<Expression, Expression> rewriter)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (rewriter == null)
                throw new ArgumentNullException(nameof(rewriter));

            _typeRewriters.Add(new TypedRewriter(type, rewriter));
        }

        /// <summary>
        /// Adds a rewriter for a specified generic type definition. The user supplied
        /// rewriter is applied whenever an expression type implements or extends some
        /// closure over the user supplied generic type definition.
        /// </summary>
        /// <param name="type">The type that triggers the rewriter.</param>
        /// <param name="rewriter">The expression rewriter.</param>
        public void AddDefinition(Type type, Func<Expression, Expression> rewriter)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (rewriter == null)
                throw new ArgumentNullException(nameof(rewriter));
            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException("Expected a generic type definition.", nameof(type));

            _typeRewriters.Add(new TypedRewriter(type, rewriter, isOpenGeneric: true));
        }

        /// <summary>
        /// Maps the specified parameter on state tracked in the scoped symbol table. This method gets called for each declaration site of a variable.
        /// </summary>
        /// <param name="parameter">Parameter expression that's being declared.</param>
        /// <returns>State to associate with the given parameter.</returns>
        protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

        private sealed class TypedRewriter
        {
            private readonly Type _type;
            private readonly Func<Expression, Expression> _rewriter;
            private readonly bool _isOpenGeneric;

            public TypedRewriter(Type type, Func<Expression, Expression> rewriter, bool isOpenGeneric)
                : this(type, rewriter)
            {
                _isOpenGeneric = isOpenGeneric;
            }

            public TypedRewriter(Type type, Func<Expression, Expression> rewriter)
            {
                _type = type;
                _rewriter = rewriter;
            }

            public bool Match(Expression node)
            {
                if (_isOpenGeneric && node.Type.FindGenericType(_type) != null)
                {
                    return true;
                }
                else
                {
                    return _type.IsAssignableFrom(node.Type);
                }
            }

            public Expression Rewrite(Expression node) => _rewriter(node);
        }
    }
}
