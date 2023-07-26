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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Json.Expressions;

    /// <summary>
    /// Rule table for serialization of statement trees.
    /// </summary>
    internal class StatementTreeRuleTable : JsonRuleTable<ExpressionJsonSerializationContext>
    {
        #region Singleton pattern

        /// <summary>
        /// Single instance creation lock.
        /// </summary>
        private static readonly object s_singletonLock = new();

        /// <summary>
        /// Singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        private static volatile RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> s_instance;

        /// <summary>
        /// Gets the singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Instance
        {
            get
            {
                //
                // Double-locking pattern to ensure single initialization even in the face of multiple
                // threads racing for initialization.
                //
                if (s_instance == null)
                {
                    lock (s_singletonLock)
                    {
                        s_instance ??= new StatementTreeRuleTable();
                    }
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Creates an rule table with default rules for expression tree serialization.
        /// </summary>
        protected StatementTreeRuleTable()
        {
            //
            // Initialize rules.
            //
            AddStatementRules();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds rules for statement tree nodes.
        /// </summary>
        private void AddStatementRules()
        {
            Add(
                "Block", default(BlockExpression),
                (be, serialize, ctx) =>
                {
                    var frame = ctx.Scope.Serialization.PushFrame(be.Variables);

                    var variables = frame.ToJson(ctx.Types);
                    var type = serialize(be.Type, ctx);
                    var expressions = serialize(be.Expressions, ctx);

                    var res = Json.Expression.Object(new Dictionary<string, Json.Expression>
                    {
                        { "Variables", variables },
                        { "Type", type },
                        { "Expressions", expressions },
                    });

                    ctx.Scope.Serialization.PopFrame();

                    return res;
                },
                (jo, deserialize, ctx) =>
                {
                    var o = (Json.ObjectExpression)jo;

                    var frame = ctx.Scope.Deserialization.PushFrame(o.Members["Variables"], ctx.Types);

                    var variables = frame.Parameters;
                    var type = (Type)deserialize(o.Members["Type"], ctx);
                    var expressions = (ReadOnlyCollection<Expression>)deserialize(o.Members["Expressions"], ctx);

                    ctx.Scope.Deserialization.PopFrame();

                    return Expression.Block(type, variables, expressions);
                }
            );

            Add((LoopExpression le) => Expression.Loop(le.Body, le.BreakLabel, le.ContinueLabel));

            Add((GotoExpression ge) => Expression.MakeGoto(ge.Kind, ge.Target, ge.Value, ge.Type));
            Add((LabelExpression le) => Expression.Label(le.Target, le.DefaultValue));
            Add(
                "LabelTarget", default(LabelTarget),
                (lt, serialize, ctx) => ctx.Labels.RegisterLabel(lt, ctx.Types),
                (jo, deserialize, ctx) => ctx.Labels.GetLabel(jo)
            );

            Add((TryExpression te) => Expression.MakeTry(te.Type, te.Body, te.Finally, te.Fault, te.Handlers));

            //
            // We don't push a frame for the Variable parameter and assume it's in scope. If the parameter
            // is not in scope, it will end up in the global table.
            //
            Add((CatchBlock cb) => Expression.MakeCatchBlock(cb.Test, cb.Variable, cb.Body, cb.Filter));
            Add(
                "CatchBlocks", default(ReadOnlyCollection<CatchBlock>),
                (cb, serialize, ctx) => Json.Expression.Array(from c in cb select serialize(c, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (CatchBlock)deserialize(e, ctx)).ToList().AsReadOnly()
            );

            Add((SwitchExpression se) => Expression.Switch(se.Type, se.SwitchValue, se.DefaultBody, se.Comparison, se.Cases));
            Add((SwitchCase sc) => Expression.SwitchCase(sc.Body, sc.TestValues));
            Add(
                "SwitchCases", default(ReadOnlyCollection<SwitchCase>),
                (sc, serialize, ctx) => Json.Expression.Array(from c in sc select serialize(c, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (SwitchCase)deserialize(e, ctx)).ToList().AsReadOnly()
            );

            Add((RuntimeVariablesExpression rve) => Expression.RuntimeVariables(rve.Variables));
        }

        #endregion
    }
}
