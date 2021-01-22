// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Linq.Expressions
{
    /// <summary>
    /// An interface for serialization and deserialization of expressions
    /// and slim representations of expressions.
    /// </summary>
    public interface IExpressionSerializer
    {
        /// <summary>
        /// Method to lift an expression into a slim, serializable form.
        /// </summary>
        /// <param name="expression">The expression to serialize.</param>
        /// <returns>A slim representation of the expression.</returns>
        ExpressionSlim Lift(Expression expression);

        /// <summary>
        /// Method to reduce a slim expression to an expression.
        /// </summary>
        /// <param name="expression">The slim expression.</param>
        /// <returns>The expression represented by the slim expression.</returns>
        Expression Reduce(ExpressionSlim expression);

        /// <summary>
        /// Method to serialize a slim representation of an expression.
        /// </summary>
        /// <param name="expression">The slim expression to serialize.</param>
        /// <returns>A string representing the expression.</returns>
        string Serialize(ExpressionSlim expression);

        /// <summary>
        /// Method to deserialize a serialized expression into a slim representation.
        /// </summary>
        /// <param name="expression">The serialized expression.</param>
        /// <returns>The deserialized slim expression.</returns>
        ExpressionSlim Deserialize(string expression);
    }
}
