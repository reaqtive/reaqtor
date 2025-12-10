// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
// IG - 2025/12  - Remove CLR serialization.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Exception raised when an expression contains unbound parameters which prevent the expression to be processed.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "The standard object creation patterns don't apply.")]
    public sealed partial class UnboundParameterException : Exception
    {
        /// <summary>
        /// Creates a new unbound parameter exception for the specified expression and the unbound parameters.
        /// </summary>
        /// <param name="message">Message to describe the error condition.</param>
        /// <param name="expression">Expression with unbound parameters.</param>
        /// <param name="parameters">Unbound parameters in the expression.</param>
        public UnboundParameterException(string message, Expression expression, IEnumerable<ParameterExpression> parameters)
            : base(CreateMessage(message, expression, parameters))
        {
            Debug.Assert(expression != null);
            Debug.Assert(parameters != null);

            Expression = expression;
            Parameters = parameters.ToReadOnly();
        }

        /// <summary>
        /// Gets the expression which has unbound parameters.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Gets the unbound parameters in the expression.
        /// </summary>
        public ReadOnlyCollection<ParameterExpression> Parameters { get; }

        private static string CreateMessage(string message, Expression expression, IEnumerable<ParameterExpression> parameters)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var parametersString = string.Join("', '", parameters.Select(p => p.Name));
            var expressionString = expression.ToCSharpString();
            return string.Format(CultureInfo.InvariantCulture, "{0} Parameters: '{1}'. Expression: '{2}'.", message ?? "There are unbound parameters in the expression.", parametersString, expressionString);
        }

        /// <summary>
        /// Throws an unbound parameter exception if the specified expression contains unbound parameters.
        /// </summary>
        /// <param name="expression">Expression to check for unbound parameters.</param>
        /// <param name="message">Exception message to use for signaling unbound parameters.</param>
        public static void ThrowIfOpen(Expression expression, string message)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var variables = FreeVariableScanner.Scan(expression);

            if (variables.Any())
            {
                throw new UnboundParameterException(message, expression, variables);
            }
        }
    }
}
