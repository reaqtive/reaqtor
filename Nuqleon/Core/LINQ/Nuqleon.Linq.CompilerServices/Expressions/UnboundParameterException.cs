// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Exception raised when an expression contains unbound parameters which prevent the expression to be processed.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "The standard object creation patterns don't apply.")]
    [Serializable]
    public sealed partial class UnboundParameterException : Exception
    {
        [NonSerialized]
        private readonly Expression _expression;

        [NonSerialized]
        private readonly ReadOnlyCollection<ParameterExpression> _parameters;

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

            _expression = expression;
            _parameters = parameters.ToReadOnly();
        }

        /// <summary>
        /// Initializes a new instance of the UnboundParameterException class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        private UnboundParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the expression which has unbound parameters.
        /// </summary>
        public Expression Expression => _expression;

        /// <summary>
        /// Gets the unbound parameters in the expression.
        /// </summary>
        public ReadOnlyCollection<ParameterExpression> Parameters => _parameters;

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

        /// <summary>
        /// Sets the System.Runtime.Serialization.SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
