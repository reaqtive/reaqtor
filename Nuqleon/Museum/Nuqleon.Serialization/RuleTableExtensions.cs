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
    /// Read-only view on a rule table.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    internal sealed class RuleTableReadOnly<TInput, TOutput, TContext> : RuleTableBase<TInput, TOutput, TContext>
    {
        #region Fields

        /// <summary>
        /// Base table to provide a read-only view over.
        /// </summary>
        private readonly RuleTableBase<TInput, TOutput, TContext> _baseTable;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a read-only view on a rule table.
        /// </summary>
        /// <param name="baseTable">Base table to provide a read-only view over.</param>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        public RuleTableReadOnly(RuleTableBase<TInput, TOutput, TContext> baseTable, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            : base(wrap, unwrap)
        {
            _baseTable = baseTable;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries to get the rule with the specified name from the base table.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <param name="rule">Rule with the specified name.</param>
        /// <returns>true if a rule with the specified name was found; otherwise, false.</returns>
        public override bool TryGetRule(string name, out Rule<TInput, TOutput, TContext> rule) => _baseTable.TryGetRule(name, out rule);

        /// <summary>
        /// Gets a list of rules that can process objects of the given type using an exact type match from the base table.
        /// </summary>
        /// <param name="type">Type to lookup rules for.</param>
        /// <returns>Rules that match the given type, not considering subtyping.</returns>
        public override IEnumerable<Rule<TInput, TOutput, TContext>> this[Type type] => _baseTable[type];

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the serializer.
        /// </summary>
        /// <returns>Sequence of rules inherited from the base table.</returns>
        protected override IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumeratorCore() => _baseTable.GetEnumerator();

        /// <summary>
        /// Addition method, enforcing read-only view by throwing a NotSupportException upon attempts to extend the table.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        protected override void AddCore(Rule<TInput, TOutput, TContext> rule) => throw new NotSupportedException("This rule table is read-only.");

        #endregion
    }

    /// <summary>
    /// Mutable extension rule table on top of a rule table.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    internal sealed class RuleTableExtension<TInput, TOutput, TContext> : RuleTableBase<TInput, TOutput, TContext>
    {
        #region Fields

        /// <summary>
        /// Base table to provide a table extension for.
        /// </summary>
        private readonly RuleTableBase<TInput, TOutput, TContext> _baseTable;

        /// <summary>
        /// Mutable extension table to add rules to.
        /// </summary>
        private readonly RuleTable<TInput, TOutput, TContext> _extension;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an extension table layered on a rule table.
        /// </summary>
        /// <param name="baseTable">Base table to provide a table extension for.</param>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        public RuleTableExtension(RuleTableBase<TInput, TOutput, TContext> baseTable, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            : base(wrap, unwrap)
        {
            _baseTable = baseTable;
            _extension = new RuleTable<TInput, TOutput, TContext>(wrap, unwrap);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries to get the rule with the specified name by a lookup in the base table first, followed by a lookup in the extension table.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <param name="rule">Rule with the specified name.</param>
        /// <returns>true if a rule with the specified name was found; otherwise, false.</returns>
        public override bool TryGetRule(string name, out Rule<TInput, TOutput, TContext> rule)
        {
            if (_baseTable.TryGetRule(name, out rule))
                return true;

            return _extension.TryGetRule(name, out rule);
        }

        /// <summary>
        /// Gets a list of rules that can process objects of the given type using an exact type match from the base table, followed by matching rules in the extension table.
        /// </summary>
        /// <param name="type">Type to lookup rules for.</param>
        /// <returns>Rules that match the given type, not considering subtyping.</returns>
        public override IEnumerable<Rule<TInput, TOutput, TContext>> this[Type type] => _baseTable[type].Concat(_extension[type]);

        /// <summary>
        /// Gets an enumerator to iterate over the rules inherited from the base table, followed by those added by the extension table.
        /// </summary>
        /// <returns>Sequence of rules inherited from the base table, followed by those added by the extension table.</returns>
        protected override IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumeratorCore()
        {
            foreach (var rule in _baseTable)
                yield return rule;

            foreach (var rule in _extension)
                yield return rule;
        }

        /// <summary>
        /// Adds a serialization rule to the extension table.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        protected override void AddCore(Rule<TInput, TOutput, TContext> rule)
        {
            _extension.Add(rule);
        }

        #endregion
    }

    /// <summary>
    /// Concatenation of two rule tables.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    internal sealed class RuleTableConcat<TInput, TOutput, TContext> : RuleTableBase<TInput, TOutput, TContext>
    {
        #region Fields

        /// <summary>
        /// First table included in the concatenation.
        /// </summary>
        private readonly RuleTableBase<TInput, TOutput, TContext> _first;

        /// <summary>
        /// Second table included in the concatenation.
        /// </summary>
        private readonly RuleTableBase<TInput, TOutput, TContext> _second;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a concatenation of two rule tables.
        /// </summary>
        /// <param name="first">First table included in the concatenation.</param>
        /// <param name="second">Second table included in the concatenation.</param>
        /// <param name="wrap">Function to wrap a series of recursively acquired serialization outputs.</param>
        /// <param name="unwrap">Function to unwrap a piece of recursive serialization output.</param>
        public RuleTableConcat(RuleTableBase<TInput, TOutput, TContext> first, RuleTableBase<TInput, TOutput, TContext> second, Func<IDictionary<string, Expression>, Expression> wrap, Func<Expression, string, Expression> unwrap)
            : base(wrap, unwrap)
        {
            _first = first;
            _second = second;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries to get the rule with the specified name by a lookup in the first table, followed by a lookup in the second table.
        /// </summary>
        /// <param name="name">Name of the rule to get.</param>
        /// <param name="rule">Rule with the specified name.</param>
        /// <returns>true if a rule with the specified name was found; otherwise, false.</returns>
        public override bool TryGetRule(string name, out Rule<TInput, TOutput, TContext> rule)
        {
            if (_first.TryGetRule(name, out rule))
                return true;

            return _second.TryGetRule(name, out rule);
        }

        /// <summary>
        /// Gets a list of rules that can process objects of the given type using an exact type match from both included rule tables, in the order of concatenation.
        /// </summary>
        /// <param name="type">Type to lookup rules for.</param>
        /// <returns>Rules that match the given type, not considering subtyping.</returns>
        public override IEnumerable<Rule<TInput, TOutput, TContext>> this[Type type] => _first[type].Concat(_second[type]);

        /// <summary>
        /// Gets an enumerator to iterate over the rules in the concatenated rule tables.
        /// </summary>
        /// <returns>Sequence of rules in the included rule tables, in the order of concatenation.</returns>
        protected override IEnumerator<Rule<TInput, TOutput, TContext>> GetEnumeratorCore()
        {
            foreach (var rule in _first)
                yield return rule;

            foreach (var rule in _second)
                yield return rule;
        }

        /// <summary>
        /// Throws a NotSupportedException. To extend a concatenated rule table, call Extend.
        /// </summary>
        /// <param name="rule">Rule to add to the serializer's rule table.</param>
        protected override void AddCore(Rule<TInput, TOutput, TContext> rule)
        {
            throw new NotSupportedException("Rule table doesn't support addition of rules.");
        }

        #endregion
    }
}
