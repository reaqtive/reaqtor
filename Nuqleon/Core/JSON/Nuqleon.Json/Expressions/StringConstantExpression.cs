// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree node representing a JSON constant value of type String.
    /// </summary>
    internal sealed class StringConstantExpression : ConstantExpression
    {
        /// <summary>
        /// Singleton expression node object for an empty string.
        /// </summary>
        internal static readonly ConstantExpression Empty = new StringConstantExpression("");

        /// <summary>
        /// The value of the String.
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Creates a new constant expression tree node with the given string value.
        /// </summary>
        /// <param name="value">Value of the constant.</param>
        internal StringConstantExpression(string value) => _value = value;

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        /// <remarks>
        /// Always returns <see cref="ExpressionType.String"/>.
        /// </remarks>
        public override ExpressionType NodeType => ExpressionType.String;

        /// <summary>
        /// Gets the value of constant represented by this expression tree node.
        /// </summary>
        public override object Value => _value;

        /// <summary>
        /// Returns the JSON fragment corresponding to the expression tree node.
        /// </summary>
        /// <returns>JSON fragment corresponding to the expression tree node.</returns>
        public override string ToString() => _value.ToJsonString();

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder) => builder.AppendJsonString(_value);
    }
}
