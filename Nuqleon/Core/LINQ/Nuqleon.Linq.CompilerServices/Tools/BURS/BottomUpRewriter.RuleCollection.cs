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
    /// Represents a collection of rewrite rules.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public sealed class RuleCollection<TSource, TTarget> : IEnumerable<Rule<TSource, TTarget>>
    {
        private readonly List<Rule<TSource, TTarget>> _rules;

        internal RuleCollection() => _rules = new List<Rule<TSource, TTarget>>();

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource>> pattern, Expression<Func<TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var getPattern = pattern.Compile();
            var invokeGoal = goal.Compile();
            AddCore(pattern, ws => getPattern(), goal, ts => invokeGoal(), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource, TSource>> pattern, Expression<Func<TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var getPattern = pattern.Compile();
            var invokeGoal = goal.Compile();
            AddCore(pattern, ws => getPattern(ws[0]), goal, ts => invokeGoal(ts[0]), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource, TSource, TSource>> pattern, Expression<Func<TTarget, TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var getPattern = pattern.Compile();
            var invokeGoal = goal.Compile();
            AddCore(pattern, ws => getPattern(ws[0], ws[1]), goal, ts => invokeGoal(ts[0], ts[1]), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(Expression<Func<TSource, TSource, TSource, TSource>> pattern, Expression<Func<TTarget, TTarget, TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var getPattern = pattern.Compile();
            var invokeGoal = goal.Compile();
            AddCore(pattern, ws => getPattern(ws[0], ws[1], ws[2]), goal, ts => invokeGoal(ts[0], ts[1], ts[2]), cost);
        }

        /*
        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add(LambdaExpression pattern, LambdaExpression goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var rule = new Rule<TSource, TTarget>(pattern, getPattern, goal, invokeGoal, cost);
            Add(rule);
        }
         */

        private void AddCore(LambdaExpression pattern, Func<TSource[], TSource> getPattern, LambdaExpression goal, Func<TTarget[], TTarget> invokeGoal, int cost)
        {
            var rule = new Rule<TSource, TTarget>(pattern, getPattern, goal, invokeGoal, cost);
            AddCore(rule);
        }

        internal void AddCore(Rule<TSource, TTarget> rule)
        {
            _rules.Add(rule);

            Added?.Invoke(rule);
        }

        /// <summary>
        /// Gets the number of rewrite rules in the collection.
        /// </summary>
        public int Count => _rules.Count;

        internal event Action<Rule<TSource, TTarget>> Added;

        /// <summary>
        /// Gets an enumerator to iterate over the rewrite rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the rewrite rules in the collection.</returns>
        public IEnumerator<Rule<TSource, TTarget>> GetEnumerator() => _rules.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the rewrite rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the rewrite rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
