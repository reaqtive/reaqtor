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

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Abstract base class for serializers using JSON as the serialization format.
    /// </summary>
    /// <typeparam name="TSource">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object threaded through serialization and deserialization.</typeparam>
    public abstract class JsonSerializer<TSource, TContext> : SerializerBase<object, Json.Expression, TContext>
    {
        #region Fields

        /// <summary>
        /// Inlined JSON expression types.
        /// </summary>
        private static readonly string[] INLINED = new[] { Json.ExpressionType.Number.ToString(), Json.ExpressionType.Null.ToString(), Json.ExpressionType.Boolean.ToString() };

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new JSON-based serializer with rules for commonly used (mostly primitive) types from the BCL.
        /// </summary>
        /// <param name="newContext">Function to get a new serialization context during serialization.</param>
        /// <param name="addContext">Function to add serialization context to the serialization output.</param>
        /// <param name="getContext">Function to get deserialization context from deserialization input.</param>
        /// <param name="ruleTable">Rule table with serialization and deserialization rules.</param>
        protected JsonSerializer(Func<TContext> newContext, Func<Contextual<TContext, Json.Expression>, Json.Expression> addContext, Func<Json.Expression, Contextual<TContext, Json.Expression>> getContext, RuleTableBase<object, Json.Expression, TContext> ruleTable)
            : base(
                tag =>
                    INLINED.Contains(tag.Name)
                    ? tag.Value
                    : Json.Expression.Object(
                          new Dictionary<string, Json.Expression>
                          {
                              { "Type", Json.Expression.String(tag.Name) },
                              { "Value", tag.Value }
                          }
                      ),
                json =>
                    INLINED.Contains(json.NodeType.ToString())
                    ? new TaggedByRule<Json.Expression> { Name = ((Json.ConstantExpression)json).NodeType.ToString(), Value = json }
                    : ((Json.ObjectExpression)json).Let(jo => new TaggedByRule<Json.Expression>
                    {
                        Name = (string)((Json.ConstantExpression)jo.Members["Type"]).Value,
                        Value = jo.Members["Value"]
                    }),
                newContext,
                addContext,
                getContext,
                ruleTable)
        {
        }

        #endregion
    }
}
