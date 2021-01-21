// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Memory;

using Reaqtive.Expressions;

namespace Reaqtor.Reactive.Expressions
{
    //
    // CONSIDER: Move to System.Linq.CompilerServices if there's a meaningful generalization.
    //

    /// <summary>
    /// Represents a quoted value, i.e. a value that has an expression representation attached to it.
    /// </summary>
    /// <typeparam name="T">Type of the quoted value.</typeparam>
    public class Quoted<T> : IQuoted<T>
    {
        private readonly IDiscardable<Expression> _expression;

        /// <summary>
        /// Creates a new quoted value using the specified expression tree.
        /// </summary>
        /// <param name="expression">Expression representing the quoted value.</param>
        /// <remarks>This constructor is called by the QuoteConverter in the serialization stack.</remarks>
        public Quoted(Expression expression)
            : this(expression, DefaultExpressionPolicy.Instance)
        {
        }

        /// <summary>
        /// Creates a new quoted value using the specified expression tree.
        /// </summary>
        /// <param name="value">Value of the quote.</param>
        /// <param name="expression">Expression representing the quoted value.</param>
        public Quoted(T value, Expression expression)
            : this(value, expression, DefaultExpressionPolicy.Instance)
        {
        }

        /// <summary>
        /// Creates a new quoted value using the specified expression tree.
        /// </summary>
        /// <param name="expression">Expression representing the quoted value.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public Quoted(Expression expression, IExpressionEvaluationPolicy policy)
            : this(policy.Evaluate<T>(expression), expression, policy)
        {
        }

        /// <summary>
        /// Creates a new quoted value using the specified expression tree.
        /// </summary>
        /// <param name="value">Value of the quote.</param>
        /// <param name="expression">Expression representing the quoted value.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public Quoted(T value, Expression expression, IExpressionEvaluationPolicy policy)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            if (!typeof(T).IsAssignableFrom(expression.Type))
                throw new ArgumentException("The specified expression does not have a type compatible with the specified value.", nameof(expression));

            Value = value;
            _expression = policy.InMemoryCache.Create(expression);
        }

        /// <summary>
        /// Gets the value represented by the quote.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the expression representing the value of the quote.
        /// </summary>
        public Expression Expression => _expression.Value;

        /// <summary>
        /// Returns a friendly string representation of the quoted value, including its expression representation.
        /// </summary>
        /// <returns>Friendly string representation of the quoted value.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Quoted = {0} @{{{1}}}", Value, Expression);
        }
    }
}
