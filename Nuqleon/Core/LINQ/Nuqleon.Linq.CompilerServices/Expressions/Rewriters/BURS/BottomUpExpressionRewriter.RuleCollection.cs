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
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public class ExpressionRewriterRuleCollection<TTarget> : IEnumerable<Rule<ExpressionTreeBase, TTarget>>
        where TTarget : ITree
    {
        private readonly RuleCollection<ExpressionTreeBase, TTarget> _rules;

        internal ExpressionRewriterRuleCollection(RuleCollection<ExpressionTreeBase, TTarget> rules) => _rules = rules;

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <typeparam name="TResult">Return type of the expression.</typeparam>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<TResult>(Expression<Func<TResult>> pattern, Expression<Func<TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var invokeGoal = goal.Compile();
            AddCore(pattern, goal, ts => invokeGoal(), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter in the expression.</typeparam>
        /// <typeparam name="TResult">Return type of the expression.</typeparam>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<T1, TResult>(Expression<Func<T1, TResult>> pattern, Expression<Func<TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var invokeGoal = goal.Compile();
            AddCore(pattern, goal, ts => invokeGoal(ts[0]), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter in the expression.</typeparam>
        /// <typeparam name="T2">Type of the second parameter in the expression.</typeparam>
        /// <typeparam name="TResult">Return type of the expression.</typeparam>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> pattern, Expression<Func<TTarget, TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var invokeGoal = goal.Compile();
            AddCore(pattern, goal, ts => invokeGoal(ts[0], ts[1]), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter in the expression.</typeparam>
        /// <typeparam name="T2">Type of the second parameter in the expression.</typeparam>
        /// <typeparam name="T3">Type of the third parameter in the expression.</typeparam>
        /// <typeparam name="TResult">Return type of the expression.</typeparam>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> pattern, Expression<Func<TTarget, TTarget, TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var invokeGoal = goal.Compile();
            AddCore(pattern, goal, ts => invokeGoal(ts[0], ts[1], ts[2]), cost);
        }

        /// <summary>
        /// Adds a new rewrite rule.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter in the expression.</typeparam>
        /// <typeparam name="T2">Type of the second parameter in the expression.</typeparam>
        /// <typeparam name="T3">Type of the third parameter in the expression.</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter in the expression.</typeparam>
        /// <typeparam name="TResult">Return type of the expression.</typeparam>
        /// <param name="pattern">Lambda expression representing the pattern matched by the rule.</param>
        /// <param name="goal">Lambda expression representing the rewrite goal applied by the rule.</param>
        /// <param name="cost">Cost to apply the rule.</param>
        public void Add<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> pattern, Expression<Func<TTarget, TTarget, TTarget, TTarget, TTarget>> goal, int cost)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            var invokeGoal = goal.Compile();
            AddCore(pattern, goal, ts => invokeGoal(ts[0], ts[1], ts[2], ts[3]), cost);
        }

        /// <summary>
        /// Gets an enumerator to iterate over the rewrite rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the rewrite rules in the collection.</returns>
        public IEnumerator<Rule<ExpressionTreeBase, TTarget>> GetEnumerator() => _rules.GetEnumerator();

        /// <summary>
        /// Gets an enumerator to iterate over the rewrite rules in the collection.
        /// </summary>
        /// <returns>Enumerator to iterate over the rewrite rules in the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void AddCore(LambdaExpression pattern, LambdaExpression goal, Func<TTarget[], TTarget> invokeGoal, int cost)
        {
            //
            // The call to _rules.AddCore will circle back here through virtual dispatch and the OnAdded event.
            //
            var translateToExpressionTree = new Func<ExpressionTreeBase[], ExpressionTreeBase>(wildcards =>
            {
                var map = new Dictionary<ParameterExpression, ExpressionTree<ParameterExpression>>();
                foreach (ExpressionTreeWildcard wildcard in wildcards)
                {
                    map[wildcard.Expression] = wildcard;
                }

                var visitor = new ExpressionTreeConversionWithDeBruijn(map);
                var expressionTree = visitor.Visit(pattern.Body);

                return expressionTree;
            });

            var rule = new Rule<ExpressionTreeBase, TTarget>(pattern, translateToExpressionTree, goal, invokeGoal, cost);
            _rules.AddCore(rule);
        }
    }
}
