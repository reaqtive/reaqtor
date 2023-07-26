// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Serializer for LINQ expression trees using JSON as the serialization format.
    /// </summary>
    public class ExpressionJsonSerializer : JsonSerializer<Expression, ExpressionJsonSerializationContext>
    {
        #region Constructors

        /// <summary>
        /// Creates a new expression tree serializer instance, initialized with default rules.
        /// Additional rules may be added using the Rules property.
        /// </summary>
        public ExpressionJsonSerializer()
            : this(RuleOptions.Default, typeResolutionService: null)
        {
        }

        /// <summary>
        /// Creates a new expression tree serializer instance, with the specified configuration parameters.
        /// Additional rules may be added using the Rules property.
        /// </summary>
        /// <param name="options">Rule set configuration flags.</param>
        public ExpressionJsonSerializer(RuleOptions options)
            : this(RuleConfiguration.Get(options), typeResolutionService: null)
        {
        }

        /// <summary>
        /// Creates a new expression tree serializer instance, with the specified configuration parameters.
        /// Additional rules may be added using the Rules property.
        /// </summary>
        /// <param name="options">Rule set configuration flags.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        public ExpressionJsonSerializer(RuleOptions options, ITypeResolutionService typeResolutionService)
            : this(RuleConfiguration.Get(options), typeResolutionService)
        {
        }

        /// <summary>
        /// Creates a new expression tree serializer instance, initialized with the given rule table.
        /// </summary>
        /// <param name="ruleTable">Rule table to use in the serializer instance.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        protected ExpressionJsonSerializer(RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> ruleTable, ITypeResolutionService typeResolutionService)
            : base(
                () => new ExpressionJsonSerializationContext(),
                ctx => Json.Expression.Object(new Dictionary<string, Json.Expression>
                {
                    { "Context", ctx.Context.ToJson() },
                    { "Value", ctx.Payload }
                }),
                json => ((Json.ObjectExpression)json).Members.Let(members => new Contextual<ExpressionJsonSerializationContext, Json.Expression>
                {
                    Context = ExpressionJsonSerializationContext.FromJson(members["Context"], typeResolutionService),
                    Payload = members["Value"]
                }),
                ruleTable)
        {
        }

        #endregion

        #region Methods

        #region Singleton pattern

        /// <summary>
        /// Singleton instance with default expression tree serializer configuration.
        /// </summary>
        private static ExpressionJsonSerializer s_instance;

        /// <summary>
        /// Gets a singleton instance of the expression tree serializer with default rules.
        /// The returned instance is read-only, preventing addition of extra rules.
        /// </summary>
        public static ExpressionJsonSerializer Instance
        {
            get
            {
                //
                // No double-locking pattern; don't care about having different instances, as the main
                // cost is in the rule tables, where we do guard against this.
                //
                s_instance ??= new ExpressionJsonSerializer(RuleOptions.Default | RuleOptions.ReadOnly, typeResolutionService: null);

                return s_instance;
            }
        }

        #endregion

        #region Convenience overloads for serialization and deserialization

        /// <summary>
        /// Serializes the given expression tree into a JSON representation.
        /// </summary>
        /// <param name="input">Expression tree to serialize.</param>
        /// <returns>JSON representation of the specified expression tree.</returns>
        public Json.Expression Serialize(Expression input)
        {
            var res = base.Serialize(input);
            return res;
        }

        /// <summary>
        /// Deserializes the given JSON representation into an expression tree object.
        /// </summary>
        /// <param name="input">JSON representation of an expression tree.</param>
        /// <returns>Expression tree represented by the specified JSON representation.</returns>
        public new Expression Deserialize(Json.Expression input)
        {
            var res = (Expression)base.Deserialize(input);
            return res;
        }

        #endregion

        #endregion
    }
}
