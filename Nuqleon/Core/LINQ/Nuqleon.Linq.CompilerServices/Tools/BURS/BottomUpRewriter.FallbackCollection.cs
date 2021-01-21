// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a collection of fallback rules.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public sealed class FallbackCollection<TSource, TTarget> : IEnumerable<Fallback<TSource, TTarget>>
    {
        private readonly List<Fallback<TSource, TTarget>> _fallbacks;

        internal FallbackCollection() => _fallbacks = new List<Fallback<TSource, TTarget>>();

        /// <summary>
        /// Adds a new fallback rule.
        /// </summary>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource, TTarget>> convert, int cost)
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, node => true, cost);
        }

        /// <summary>
        /// Adds a new fallback rule.
        /// </summary>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="predicate">Lambda expression representing the predicate to apply to evaluate applicability of the rule to a source tree leaf node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource, TTarget>> convert, Expression<Func<TSource, bool>> predicate, int cost)
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, predicate, cost);
        }

        private void AddCore(Expression<Func<TSource, TTarget>> convert, Expression<Func<TSource, bool>> predicate, int cost)
        {
            var invokeConvert = convert.Compile();
            var invokePredicate = predicate.Compile();

            var fallback = new Fallback<TSource, TTarget>(convert, invokeConvert, predicate, invokePredicate, cost);
            AddCore(fallback);
        }

        internal void AddCore(Fallback<TSource, TTarget> fallback)
        {
            _fallbacks.Add(fallback);

            Added?.Invoke(fallback);
        }

        /// <summary>
        /// Gets the number of fallback rules in the collection.
        /// </summary>
        public int Count => _fallbacks.Count;

        internal event Action<Fallback<TSource, TTarget>> Added;

        /// <summary>
        /// Gets an enumerator to iterate over the fallback rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the fallback rules in the collection.</returns>
        public IEnumerator<Fallback<TSource, TTarget>> GetEnumerator() => _fallbacks.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the fallback rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the fallback rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
