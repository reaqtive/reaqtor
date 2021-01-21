// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Defines a table with serialization/deserialization rules.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public class RuleTable<TInput, TOutput, TContext> : RuleTableBase<TInput, TOutput, TContext>
    {
        #region Fields

        /// <summary>
        /// Table of rules, indexed by name. Used for efficient lookup of rules during deserialization.
        /// </summary>
        private readonly Dictionary<string, Rule<TInput, TOutput, TContext>> _rules;

        /// <summary>
        /// Table of rules, indexed by type. Used for efficient lookup of rules during serialization.
        /// For a given object, this table is scanned for the object's type, and for all of its base types and interfaces.
        /// </summary>
        private readonly Dictionary<Type, RuleList> _ruleIndex;

        /// <summary>
        /// Cached empty rule table.
        /// </summary>
        private readonly IEnumerable<Rule<TInput, TOutput, TContext>> _none;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new serializer with the given configuration parameters.
        /// </summary>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        public RuleTable(Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            : base(wrap, unwrap)
        {
            _rules = new Dictionary<string, Rule<TInput, TOutput, TContext>>();
            _ruleIndex = new Dictionary<Type, RuleList>();
            _none = Enumerable.Empty<Rule<TInput, TOutput, TContext>>();
        }

        #endregion

        #region Methods

        #region Accessors

        /// <summary>
        /// Tries to get the rule with the specified name.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <param name="rule">Rule with the specified name.</param>
        /// <returns>true if a rule with the specified name was found; otherwise, false.</returns>
        public override bool TryGetRule(string name, out Rule<TInput, TOutput, TContext> rule)
        {
            //
            // Safe for concurrent read-only access during deserialization.
            //
            return _rules.TryGetValue(name, out rule);
        }

        /// <summary>
        /// Gets a list of rules that can process objects of the given type using an exact type match.
        /// </summary>
        /// <param name="type">Type to lookup rules for.</param>
        /// <returns>Rules that match the given type, not considering subtyping.</returns>
        public override IEnumerable<Rule<TInput, TOutput, TContext>> this[Type type]
        {
            get
            {
                //
                // Safe for concurrent read-only access during serialization.
                //
                _ruleIndex.TryGetValue(type, out RuleList rulesForType);
                return rulesForType ?? _none;
            }
        }

        #endregion

        #region IEnumerable<T> implementation (used for collection initializer syntax)

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the serializer.
        /// </summary>
        /// <returns>Sequence of rules in the serializer, in evaluation order on a per-type basis.</returns>
        protected override IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumeratorCore()
        {
            //
            // Use of SelectMany to preserve order of rules on a per-type basis.
            //
            return _ruleIndex.SelectMany(rule => rule.Value).GetEnumerator();
        }

        #endregion

        #region Private implementation

        /// <summary>
        /// Adds a serialization rule to the serializer's rule table.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        protected override void AddCore(Rule<TInput, TOutput, TContext> rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            if (TryGetRule(rule.Name, out _))
                throw new InvalidOperationException("A rule with the specified name already exists.");

            _rules[rule.Name] = rule;

            if (!_ruleIndex.TryGetValue(rule.InputType, out RuleList rulesForT))
                _ruleIndex[rule.InputType] = rulesForT = new RuleList();

            rulesForT.Add(rule);
        }

        /// <summary>
        /// Ordered list of rules with thread-safe enumeration for use during serialization.
        /// </summary>
        private sealed class RuleList : IEnumerable<Rule<TInput, TOutput, TContext>>
        {
            /// <summary>
            /// List of rules.
            /// </summary>
            private readonly List<Rule<TInput, TOutput, TContext>> _rules;

            /// <summary>
            /// Creates a new rule list.
            /// </summary>
            public RuleList()
            {
                _rules = new List<Rule<TInput, TOutput, TContext>>();
            }

            /// <summary>
            /// Adds the specified rule to the end of the list.
            /// </summary>
            /// <param name="rule">Rule to add to the list.</param>
            public void Add(Rule<TInput, TOutput, TContext> rule)
            {
                _rules.Add(rule);
            }

            /// <summary>
            /// Gets an enumerator to iterate over the rules in the list.
            /// </summary>
            /// <returns>Enumerator to iterate over the rules in the list.</returns>
            public IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumerator()
            {
                //
                // We're only protecting for concurrent read accesses, assuming the list has been
                // fully initialized upon rule table creation, prior to read access used during
                // serialization. In other words, an "initialize before use" policy holds, which is
                // encouraged by the Serializer constructors by passing in the table rather than
                // exposing it for mutation. Also, notice the list can only grow.
                //
                var n = _rules.Count;
                for (int i = 0; i < n; i++)
                    yield return _rules[i];
#if FALSE
                //
                // This approach would be more conservative, but we can live with a simpler access
                // pattern of returning the underlying list's enumerator. See remarks above in order
                // to understand why the read-only assumption is safe (enough).
                //
                for (int i = 0; true; i++)
                {
                    var next = default(Rule<TInput, TOutput, TContext>);
                    lock (_rules)
                    {
                        if (i < _rules.Count)
                            next = _rules[i];
                    }

                    if (next == null)
                        break;

                    yield return next;
                }
#endif
            }

            /// <summary>
            /// Gets an enumerator to iterate over the rules in the list.
            /// </summary>
            /// <returns>Enumerator to iterate over the rules in the list.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion

        #endregion
    }
}
