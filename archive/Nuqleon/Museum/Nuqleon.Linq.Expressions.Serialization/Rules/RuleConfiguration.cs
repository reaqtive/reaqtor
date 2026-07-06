// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Maintains default expression tree serialization rule tables.
    /// </summary>
    internal static class RuleConfiguration
    {
        /*
         * Re-enable if caching of rule table configurations is desirable for performance.
         */
#if FALSE
        /// <summary>
        /// Cache of default rule configurations.
        /// </summary>
        /// <remarks>Consider using a ConditionalWeakTable if the slow key leak is a deal-breaker (requires reference type wrapper around RulesOptions).</remarks>
        private static Dictionary<RulesOptions, WeakReference<RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext>>> s_tables = new Dictionary<RulesOptions, WeakReference<RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext>>>();

        /// <summary>
        /// Gets a rule table with the specified configuration optiosn.
        /// </summary>
        /// <param name="options">Configuration options for the resulting rule table.</param>
        /// <returns>Rule table with the specified configuration options.</returns>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Get(RulesOptions options)
        {
            var res = default(RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext>);

            lock (s_tables)
            {
                var tableRef = default(WeakReference<RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext>>);
                if (!s_tables.TryGetValue(options, out tableRef))
                {
                    res = CreateRuleTable(options);
                    tableRef = new WeakReference<RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext>>(res);
                    s_tables[options] = tableRef;
                }
                else
                {
                    if (!tableRef.TryGetTarget(out res))
                    {
                        res = CreateRuleTable(options);
                        tableRef.SetTarget(res);
                    }
                }
            }

            return res;
        }
#else
        /// <summary>
        /// Gets a rule table with the specified configuration optiosn.
        /// </summary>
        /// <param name="options">Configuration options for the resulting rule table.</param>
        /// <returns>Rule table with the specified configuration options.</returns>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Get(RuleOptions options)
        {
            return CreateRuleTable(options);
        }
#endif

        /// <summary>
        /// Constructs a rule table with the specified configuration optiosn.
        /// </summary>
        /// <param name="options">Configuration options for the resulting rule table.</param>
        /// <returns>Rule table with the specified configuration options.</returns>
        private static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> CreateRuleTable(RuleOptions options)
        {
            var baseTable = ExpressionTreeRuleTable.Instance;

            var res = baseTable;
            if ((options & RuleOptions.CaptureClosures) != RuleOptions.None)
            {
                res = res.Concat(ClosureRuleTable.Instance);
            }

            if ((options & RuleOptions.CSharpDynamic) != RuleOptions.None)
            {
                res = res.Concat(CSharpDynamicRuleTable.Instance);
            }

            if ((options & RuleOptions.StatementTrees) != RuleOptions.None)
            {
                res = res.Concat(StatementTreeRuleTable.Instance);
            }

            if ((options & RuleOptions.ReadOnly) != RuleOptions.None)
            {
                res = res.AsReadOnly();
            }
            else
            {
                res = res.Extend();
            }

            return res;
        }
    }
}
