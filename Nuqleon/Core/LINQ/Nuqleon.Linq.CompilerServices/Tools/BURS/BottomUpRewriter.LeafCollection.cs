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
    /// Represents a collection of leaf rules.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public sealed class LeafCollection<TSource, TTarget> : IEnumerable<Leaf<TSource, TTarget>>
    {
        private readonly List<Leaf<TSource, TTarget>> _leaves;

        internal LeafCollection() => _leaves = new List<Leaf<TSource, TTarget>>();

        /// <summary>
        /// Adds a new leaf rule.
        /// </summary>
        /// <typeparam name="TLeaf">Type of the source tree leaf node. This type should be a subtype of TSource.</typeparam>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, int cost)
            where TLeaf : TSource
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, node => node is TLeaf, cost);
        }

        /// <summary>
        /// Adds a new leaf rule.
        /// </summary>
        /// <typeparam name="TLeaf">Type of the source tree leaf node. This type should be a subtype of TSource.</typeparam>
        /// <param name="convert">Lambda expression representing the conversion from the source tree leaf node to the target tree node.</param>
        /// <param name="predicate">Lambda expression representing the predicate to apply to evaluate applicability of the rule to a source tree leaf node.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, Expression<Func<TLeaf, bool>> predicate, int cost)
            where TLeaf : TSource
        {
            if (convert == null)
                throw new ArgumentNullException(nameof(convert));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            AddCore(convert, predicate, cost);
        }

        private void AddCore<TLeaf>(Expression<Func<TLeaf, TTarget>> convert, Expression<Func<TLeaf, bool>> predicate, int cost)
            where TLeaf : TSource
        {
            var invokeConvert = convert.Compile();
            var invokePredicate = predicate.Compile();

            var leaf = new Leaf<TSource, TTarget>(convert, x => invokeConvert((TLeaf)x), predicate, x => x is TLeaf lead && invokePredicate(lead), cost);
            AddCore(leaf);
        }

        internal void AddCore(Leaf<TSource, TTarget> leaf)
        {
            _leaves.Add(leaf);

            Added?.Invoke(leaf);
        }

        /// <summary>
        /// Gets the number of leaf rules in the collection.
        /// </summary>
        public int Count => _leaves.Count;

        internal event Action<Leaf<TSource, TTarget>> Added;

        /// <summary>
        /// Gets an enumerator to iterate over the leaf rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the leaf rules in the collection.</returns>
        public IEnumerator<Leaf<TSource, TTarget>> GetEnumerator() => _leaves.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the leaf rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the leaf rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
