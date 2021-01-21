// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Provides a set of combinators for optimizers.
    /// </summary>
    public static class Optimizer
    {
        /// <summary>
        /// Gets an optimizer that does no optimizations.
        /// </summary>
        /// <returns>An optimizer that does no optimizations.</returns>
        public static IOptimizer Nop() => NopOptimizer.Instance;

        /// <summary>
        /// Creates an optimizer that chains the given optimizers sequentially.
        /// </summary>
        /// <param name="first">The first optimizer.</param>
        /// <param name="second">The second optimizer.</param>
        /// <returns>An optimizer that chains the given optimizers sequentially.</returns>
        public static IOptimizer Then(this IOptimizer first, IOptimizer second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new ThenOptimizer(first, second);
        }

        /// <summary>
        /// Creates an optimizer that applies the given optimizer until a fixed point is found or retry limit is hit.
        /// </summary>
        /// <param name="optimizer">The optimizer whose fixed point to find.</param>
        /// <param name="throwOnCycle">Flag indicating whether to throw on finding a cycle.</param>
        /// <param name="maxIterations">The maximum number of iterations.</param>
        /// <returns>An optimizer that applies the given optimizer until a fixed point is found or retry limit is hit.</returns>
        public static IOptimizer FixedPoint(this IOptimizer optimizer, bool throwOnCycle, int maxIterations)
        {
            if (optimizer == null)
                throw new ArgumentNullException(nameof(optimizer));

            if (maxIterations < 0)
                throw new ArgumentOutOfRangeException(nameof(maxIterations));

            return new FixedPointOptimizer(optimizer, maxIterations, throwOnCycle);
        }

        /// <summary>
        /// Creates an optimizer that applies the given optimizer until a fixed point is found.
        /// </summary>
        /// <param name="optimizer">The optimizer whose fixed point to find.</param>
        /// <param name="throwOnCycle">Flag indicating whether to throw on finding a cycle.</param>
        /// <returns>An optimizer that applies the given optimizer until a fixed point is found.</returns>
        public static IOptimizer FixedPoint(this IOptimizer optimizer, bool throwOnCycle)
        {
            if (optimizer == null)
                throw new ArgumentNullException(nameof(optimizer));

            return new FixedPointOptimizer(optimizer, int.MaxValue, throwOnCycle);
        }

        /// <summary>
        /// Creates an optimizer that applies the given optimizer until a fixed point is found.
        /// </summary>
        /// <param name="optimizer">The optimizer whose fixed point to find.</param>
        /// <returns>An optimizer that applies the given optimizer until a fixed point is found.</returns>
        public static IOptimizer FixedPoint(this IOptimizer optimizer)
        {
            if (optimizer == null)
                throw new ArgumentNullException(nameof(optimizer));

            return new FixedPointOptimizer(optimizer, int.MaxValue, throwOnCycle: true);
        }

        private sealed class ThenOptimizer : IOptimizer
        {
            private readonly IOptimizer _first;
            private readonly IOptimizer _second;

            public ThenOptimizer(IOptimizer first, IOptimizer second)
            {
                _first = first;
                _second = second;
            }

            public QueryTree Optimize(QueryTree queryTree) => _second.Optimize(_first.Optimize(queryTree));
        }

        private sealed class FixedPointOptimizer : IOptimizer
        {
            private readonly IOptimizer _optimizer;
            private readonly int _maxIterations;
            private readonly bool _throwOnCycle;

            public FixedPointOptimizer(IOptimizer optimizer, int maxIterations, bool throwOnCycle)
            {
                _optimizer = optimizer;
                _maxIterations = maxIterations;
                _throwOnCycle = throwOnCycle;
            }

            public QueryTree Optimize(QueryTree queryTree)
            {
                var history = new HashSet<QueryTree>(new QueryExpressionEqualityComparator());

                var current = queryTree;
                var reduced = default(QueryTree);

                var i = 0;
                while (current != reduced)
                {
                    if (!history.Add(current))
                    {
                        if (_throwOnCycle)
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Irreducible recursive query tree detected: '{0}'.", current));
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (i == _maxIterations)
                    {
                        break;
                    }

                    reduced = current;
                    current = _optimizer.Optimize(current);

                    i++;
                }

                return current;
            }
        }
    }
}
