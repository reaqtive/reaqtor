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
using System.Memory;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Finds free variables, represented as ParameterExpression nodes, in an expression tree.
    /// </summary>
    public static class FreeVariableScanner
    {
        /// <summary>
        /// Singleton instance of an empty array of parameter expressions.
        /// </summary>
        private static readonly ParameterExpression[] s_empty = Array.Empty<ParameterExpression>();

        /// <summary>
        /// Object pool of free variable scanners.
        /// </summary>
        private static readonly ObjectPool<Scanner> s_freeVariableScanners = new(() => new Scanner(), Environment.ProcessorCount);

        /// <summary>
        /// Object pool of free variable finders.
        /// </summary>
        private static readonly ObjectPool<Finder> s_freeVariableFinders = new(() => new Finder(), Environment.ProcessorCount);

        /// <summary>
        /// Scans the given expression for free variables.
        /// </summary>
        /// <param name="expression">Expression to scan.</param>
        /// <returns>Sequence of free variables in the given expression. The resulting expressions are distinct and returned in no particular order.</returns>
        public static IEnumerable<ParameterExpression> Scan(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            using var scanner = s_freeVariableScanners.New();

            var impl = scanner.Object;
            impl.Visit(expression);
            return (IEnumerable<ParameterExpression>)impl.Free ?? s_empty;
        }

        /// <summary>
        /// Checks whether the given expression has free variables.
        /// </summary>
        /// <param name="expression">Expression to scan.</param>
        /// <returns>true if one or more free variables have been found; otherwise, false.</returns>
        public static bool HasFreeVariables(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            using var finder = s_freeVariableFinders.New();

            var impl = finder.Object;
            impl.Visit(expression);
            return impl.HasFreeVariable;
        }

        private sealed class Scanner : ImplBase
        {
            // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

            public HashSet<ParameterExpression> Free;

            protected override void AddFreeVariable(ParameterExpression node)
            {
                Free ??= new HashSet<ParameterExpression>();

                Free.Add(node);
            }

            protected override void ClearCore()
            {
                // NB: The lifetime of the hash set goes beyond the scanner, so we shouldn't Clear it here;
                //     instead, just null out the reference so another use of the scanner can lazily allocate
                //     a new hash set when it encounters a free variable.

                Free = null;
            }
        }

        private sealed class Finder : ImplBase
        {
            // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

            public bool HasFreeVariable;

            public override Expression Visit(Expression node)
            {
                if (HasFreeVariable)
                {
                    return node;
                }

                return base.Visit(node);
            }

            protected override void AddFreeVariable(ParameterExpression node) => HasFreeVariable = true;

            protected override void ClearCore() => HasFreeVariable = false;
        }

        private abstract class ImplBase : ExpressionVisitor, IClearable
        {
            // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

            protected readonly Stack<IList<ParameterExpression>> _environment = new();

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var parameters = node.Parameters;

                var hasScope = parameters.Count > 0;
                if (hasScope)
                {
                    _environment.Push(parameters);
                }

                Visit(node.Body);

                if (hasScope)
                {
                    _environment.Pop();
                }

                return node;
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                var variables = node.Variables;

                var hasScope = variables.Count > 0;
                if (hasScope)
                {
                    _environment.Push(variables);
                }

                Visit(node.Expressions);

                if (hasScope)
                {
                    _environment.Pop();
                }

                return node;
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                var variable = node.Variable;

                if (variable != null)
                {
                    _environment.Push(new[] { variable });
                }

                Visit(node.Filter);
                Visit(node.Body);

                if (variable != null)
                {
                    _environment.Pop();
                }

                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                foreach (var frame in _environment)
                {
                    for (int i = 0, n = frame.Count; i < n; i++)
                    {
                        if (frame[i] == node)
                        {
                            return node;
                        }
                    }
                }

                AddFreeVariable(node);
                return node;
            }

            protected abstract void AddFreeVariable(ParameterExpression node);

            public void Clear()
            {
                _environment.Clear();
                ClearCore();
            }

            protected abstract void ClearCore();
        }
    }
}
