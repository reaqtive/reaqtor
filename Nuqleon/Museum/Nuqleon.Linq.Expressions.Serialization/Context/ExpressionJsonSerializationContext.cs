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
using System.ComponentModel.Design;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using Json = Json.Expressions;
    using TypeSystem;

    /// <summary>
    /// (Infrastructure only) Expression tree serialization context.
    /// </summary>
    public class ExpressionJsonSerializationContext
    {
        #region Constructors

        /// <summary>
        /// (Infrastructure only) Creates a new expression tree serialization context for use during serialization.
        /// </summary>
        public ExpressionJsonSerializationContext()
            : this(new TypeSpace(), new Scope(), new Heap(), new Labels())
        {
        }

        /// <summary>
        /// Creates a new expression tree serialization context with the given tracking objects.
        /// </summary>
        /// <param name="typeSpace">Type space for tracking of types in the expression tree being (de)serialized.</param>
        /// <param name="scope">Scope tracking facility for parameters in lambda expressions and block expressions.</param>
        /// <param name="heap">Heap serialization facility, typically used for closures.</param>
        /// <param name="labels">Label serialization facility, used for statement trees.</param>
        private ExpressionJsonSerializationContext(TypeSpace typeSpace, Scope scope, Heap heap, Labels labels)
        {
            Types = typeSpace;
            Scope = scope;
            Heap = heap;
            Labels = labels;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type space for tracking of types in the expression tree being (de)serialized.
        /// </summary>
        internal TypeSpace Types { get; }

        /// <summary>
        /// Gets the scope tracking facility for parameters in lambda expressions and block expressions.
        /// </summary>
        internal Scope Scope { get; }

        /// <summary>
        /// Gets the heap serialization facility, typically used for closures.
        /// </summary>
        internal Heap Heap { get; }

        /// <summary>
        /// Gets the label serialization facility, used for statement trees.
        /// </summary>
        internal Labels Labels { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the JSON representation of the expression tree serialization context.
        /// </summary>
        /// <returns>JSON representation of the expression tree serialization context.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Object(new Dictionary<string, Json.Expression>
            {
                { "Globals", Scope.ToJson(Types) },
                { "Types", Types.ToJson() },
                { "Heap", Heap.ToJson() },
                { "Labels", Labels.ToJson() },
            });
        }

        /// <summary>
        /// Restores an expression tree serialization context, used for deserialization, from the given JSON representation.
        /// </summary>
        /// <param name="expression">JSON representation of an expression tree serialization context.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>Expression tree serialization context used for deserialization.</returns>
        public static ExpressionJsonSerializationContext FromJson(Json.Expression expression, ITypeResolutionService typeResolutionService)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var ctx = (Json.ObjectExpression)expression;

            //
            // Important! The type space needs to be rehydrated first because of its use by scope
            // tracking and label serialization.
            //
            var typeSpace = TypeSpace.FromJson(ctx.Members["Types"], typeResolutionService);

            var heap = default(Heap);
            if (ctx.Members.TryGetValue("Heap", out Json.Expression heapJson))
            {
                heap = Heap.FromJson(heapJson);
            }

            var scope = default(Scope);
            if (ctx.Members.TryGetValue("Globals", out Json.Expression globals))
            {
                scope = Scope.FromJson(globals, typeSpace);
            }

            var labels = default(Labels);
            if (ctx.Members.TryGetValue("Labels", out Json.Expression labelsJson))
            {
                labels = Labels.FromJson(labelsJson, typeSpace);
            }

            return new ExpressionJsonSerializationContext(typeSpace, scope, heap, labels);
        }

        #endregion
    }
}
