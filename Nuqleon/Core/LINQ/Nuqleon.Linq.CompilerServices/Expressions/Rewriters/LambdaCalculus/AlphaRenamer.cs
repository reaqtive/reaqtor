// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Eliminates name conflicts for variables in expression trees. Because variables get represented by ParameterExpression nodes
    /// which have referential identity, syntactic name conflicts can occur.
    /// </summary>
    /// <example>
    /// Consider the following definitions of the well-known K and I combinators:
    /// <code>
    /// K = \x.\y.x
    /// I = \x.x
    /// </code>
    /// The following function is a valid representation of KI = (\x.\y.x)(\x.x) = \y.\x.x
    /// <code>
    /// var x1 = Expression.Parameter(typeof(int), "x");
    /// var x2 = Expression.Parameter(typeof(int), "x");
    /// var ki = Expression.Lambda(Expression.Lambda(x2, x2), x1); // x => x => x
    /// </code>
    /// The following function is a valid representation of K = \x.\y.x
    /// <code>
    /// var x1 = Expression.Parameter(typeof(int), "x");
    /// var x2 = Expression.Parameter(typeof(int), "x");
    /// var k  = Expression.Lambda(Expression.Lambda(x1, x2), x1); // x => x => x
    /// </code>
    /// Syntactically, both functions have an identical textual representation, requiring object identity checks to distinguish their semantic meaning.
    /// </example>
    public static class AlphaRenamer
    {
        /// <summary>
        /// Eliminates name conflicts for bound variables in the given expression, by substituting conflicting occurrences for fresh parameter expressions.
        /// </summary>
        /// <param name="expression">Expression to eliminate name conflicts in.</param>
        /// <returns>Expression without name conflicts for bound variables. Global variables are not affected.</returns>
        public static Expression EliminateNameConflicts(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new Impl("Param_").Visit(expression);
        }

        private sealed class Impl : ScopedExpressionVisitorBase
        {
            private readonly string _unnamedParameterRenamePrefix;

            private readonly SyntaxTrie _trie;
            private readonly Stack<Dictionary<ParameterExpression, ParameterExpression>> _environment;

            public Impl(string unnamedParameterRenamePrefix)
            {
                _unnamedParameterRenamePrefix = unnamedParameterRenamePrefix;

                _trie = new SyntaxTrie();
                _trie.Add(unnamedParameterRenamePrefix);

                _environment = new Stack<Dictionary<ParameterExpression, ParameterExpression>>();
            }

            protected override void Push(IEnumerable<ParameterExpression> parameters)
            {
                var newScope = new Dictionary<ParameterExpression, ParameterExpression>();

                foreach (var parameter in parameters)
                {
                    var originalName = parameter.Name;
                    var newName = originalName;

                    if (string.IsNullOrWhiteSpace(originalName))
                    {
                        originalName = _unnamedParameterRenamePrefix;
                        newName = parameter.ToString();
                    }

                    // TODO: This can be optimized by providing prefix matching functionality in the syntax trie.
                    var i = 0;
                    while (_trie.Contains(newName))
                    {
                        newName = originalName + i++;
                    }

                    var newParameter = parameter;

                    if (newName != originalName)
                    {
                        newParameter = Expression.Parameter(parameter.Type, newName);
                    }

                    newScope[parameter] = newParameter;
                    _trie.Add(newName);
                }

                _environment.Push(newScope);
            }

            protected override void Pop()
            {
                var oldScope = _environment.Pop();

                foreach (var parameter in oldScope.Values)
                {
                    _trie.Remove(parameter.Name);
                }
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                foreach (var scope in _environment)
                {
                    if (scope.TryGetValue(node, out var res))
                    {
                        return res;
                    }
                }

                if (!string.IsNullOrWhiteSpace(node.Name))
                {
                    _trie.Add(node.Name);
                }

                return base.VisitParameter(node);
            }
        }
    }
}
