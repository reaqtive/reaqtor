// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using Nuqleon.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Rule table for serialization of closures.
    /// </summary>
    internal class ClosureRuleTable : JsonRuleTable<ExpressionJsonSerializationContext>
    {
        #region Singleton pattern

        /// <summary>
        /// Singleton instance for the default, read-only closure serialization rule table.
        /// </summary>
        private static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> s_instance;

        /// <summary>
        /// Gets the singleton instance for the default, read-only closure serialization rule table.
        /// </summary>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new ClosureRuleTable().AsReadOnly();
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Creates an rule table with default rules for expression tree serialization.
        /// </summary>
        protected ClosureRuleTable()
        {
            //
            // Initialize rules.
            //
            AddHeapRule();
        }

        #endregion

        #region Methods

#pragma warning disable IDE0034 // Simplify 'default' expression (used in type tables)

        /// <summary>
        /// Adds a rule for heap serialization to support typical captured closure objects.
        /// </summary>
        private void AddHeapRule()
        {
            Add(
                "ObjRef", default(object),
                o => o != null && o.GetType().IsClosureClass(),
                (obj, serialize, ctx) =>
                {
                    if (!ctx.Heap.TryGetAddress(obj, out Json.Expression res))
                    {
                        var type = obj.GetType();

                        var raw =
                            Json.Expression.Object(new Dictionary<string, Json.Expression>
                            {
                                {
                                    "Type",
                                    serialize(type, ctx)
                                },
                                {
                                    "Data",
                                    Json.Expression.Array(
                                        from field in type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                                        select (Json.Expression)Json.Expression.Object(new Dictionary<string, Json.Expression>
                                        {
                                            { "Name", serialize(field.Name, ctx) },
                                            { "Value", serialize(field.GetValue(obj), ctx) }
                                        })
                                    )
                                }
                            });

                        res = ctx.Heap.Add(obj, raw);
                    }

                    return res;
                },
                (adr, deserialize, ctx) =>
                {
                    if (!ctx.Heap.TryLookup(adr, out object res))
                    {
                        var raw = (Json.ObjectExpression)ctx.Heap.Get(adr);

                        var type = (Type)deserialize(raw.Members["Type"], ctx);

                        //
                        // We're adding the object instance prior to initializing its fields. This way
                        // recursive object references won't lead to stack overflows. Notice however
                        // that closures don't tend to have such a recursive nature, so this is done to
                        // future-proof the code.
                        //
                        res = Activator.CreateInstance(type);
                        ctx.Heap.Add(adr, res);

                        foreach (Json.ObjectExpression data in ((Json.ArrayExpression)raw.Members["Data"]).Elements)
                        {
                            var name = (string)deserialize(data.Members["Name"], ctx);
                            var value = deserialize(data.Members["Value"], ctx);

                            var field = type.GetField(name);
                            field.SetValue(res, value);
                        }
                    }

                    return res;
                }
            );
        }

#pragma warning restore IDE0034 // Simplify 'default' expression

        #endregion
    }
}
